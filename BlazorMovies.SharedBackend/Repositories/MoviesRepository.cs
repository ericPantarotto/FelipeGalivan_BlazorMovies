//using AutoMapper;
using AutoMapper;
using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repositories;
using BlazorMovies.SharedBackend.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.SharedBackend.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IAuthenticationStateService authenticationStateService;
        private readonly IFileStorageService fileStorageService;
        private readonly IMapper mapper;
        private readonly string containerName = "movies";

        public MoviesRepository(ApplicationDbContext context,
            IAuthenticationStateService authenticationStateService,
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            this.context = context;
            this.authenticationStateService = authenticationStateService;
            this.fileStorageService = fileStorageService;
            this.mapper = mapper;
        }

        public async Task<int> CreateMovie(Movie movie)
        {
            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var poster = Convert.FromBase64String(movie.Poster);
                movie.Poster = await fileStorageService.SaveFile(poster, "jpg", containerName);
            }

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }

        public async Task DeleteMovie(int Id)
        {
            var movie = await context.Movies.FindAsync(Id);
            context.Remove(movie!);
            await context.SaveChangesAsync();
        }

        public async Task<DetailsMovieDTO?> GetDetailsMovieDTO(int id)
        {
            Movie? movie = await context.Movies.Where(x => x.Id == id)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (movie is null) { return null; }

            double voteAverage = 0.0;
            int userVote = 0;

            if (await context.MovieRatings.AnyAsync(x => x.MovieId == id))
            {
                voteAverage = await context.MovieRatings.Where(x => x.MovieId == id)
                    .AverageAsync(x => x.Rate);

                var userId = await authenticationStateService.GetCurrentUserId();

                if (userId is not null)
                {
                    var userVoteDB = await context.MovieRatings
                        .FirstOrDefaultAsync(x => x.MovieId == id && x.UserId == userId);

                    if (userVoteDB is not null)
                    {
                        userVote = userVoteDB.Rate;
                    }
                }
            }

            movie.MoviesActors = movie.MoviesActors.OrderBy(x => x.Order).ToList();

            var model = new DetailsMovieDTO
            {
                Movie = movie,
                Genres = movie.MoviesGenres.Select(x => x.Genre).ToList()!,
                Actors = movie.MoviesActors.Select(x =>
                    new Person
                    {
                        Name = x.Person?.Name!,
                        Picture = x.Person?.Picture!,
                        Character = x.Character,
                        Id = x.PersonId
                    }).ToList(),
                UserVote = userVote,
                AverageVote = voteAverage
            };
            return model;
        }

        public async Task<IndexPageDTO?> GetIndexPageDTO()
        {
            //Console.WriteLine(await authenticationStateService.GetCurrentUserId());
            var limit = 6;

            var moviesInTheaters = await context.Movies
                .Where(x => x.InTheaters).Take(limit)
                .OrderByDescending(x => x.ReleaseDate)
                .AsNoTracking()
                .ToListAsync();

            var todaysDate = DateTime.Today;

            var upcomingReleases = await context.Movies
                .Where(x => x.ReleaseDate > todaysDate)
                .OrderBy(x => x.ReleaseDate).Take(limit)
                .AsNoTracking()
                .ToListAsync();

            var response = new IndexPageDTO
            {
                InTheaters = moviesInTheaters,
                UpcomingReleases = upcomingReleases
            };

            return response;
        }

        public async Task<MovieUpdateDTO?> GetMovieForUpdate(int id)
        {
            DetailsMovieDTO? movieDetailDTO = await GetDetailsMovieDTO(id);

            if (movieDetailDTO is null) { return null; }

            List<int>? selectedGenresIds = movieDetailDTO.Genres.Select(x => x.Id).ToList();
            List<Genre>? notSelectedGenres = await context.Genres
                .Where(x => !selectedGenresIds.Contains(x.Id))
                .ToListAsync();

            MovieUpdateDTO model = new()
            {
                Movie = movieDetailDTO?.Movie!,
                SelectedGenres = movieDetailDTO?.Genres!,
                NotSelectedGenres = notSelectedGenres,
                Actors = movieDetailDTO?.Actors!
            };
            return model;
        }

        public async Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMoviesDTO filterMoviesDTO)
        {
            var moviesQueryable = context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterMoviesDTO.Title))
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.Title.Contains(filterMoviesDTO.Title));
            }

            if (filterMoviesDTO.InTheaters)
            {
                moviesQueryable = moviesQueryable.Where(x => x.InTheaters);
            }

            if (filterMoviesDTO.UpcomingReleases)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(x => x.ReleaseDate > today);
            }

            if (filterMoviesDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.MoviesGenres.Select(y => y.GenreId)
                    .Contains(filterMoviesDTO.GenreId));
            }
            if (filterMoviesDTO.OrderByVotes && moviesQueryable.Any())
            {
                var voteAverages = context.MovieRatings
                    .GroupBy(x => x.MovieId)
                    .Select(n => new { Id = n.Key, AverageRating = n.Average(x => x.Rate) });

                //moviesQueryable = from m in moviesQueryable
                //                  join r in voteAverages
                //                      on m.Id equals r.Id into grouping
                //                  from p in grouping.DefaultIfEmpty()
                //                  orderby p.AverageRating descending
                //                  select m;

                moviesQueryable = moviesQueryable.Join(voteAverages,
                    m => m.Id,
                    r => r.Id,
                    (m, r) => new { Movies = m, Rating = r.AverageRating })
                    .OrderByDescending(x => x.Rating)
                    .Select(x => x.Movies).DefaultIfEmpty()!;
            }
            
            var movies = await moviesQueryable.GetPaginatedResponse(filterMoviesDTO.Pagination);

            return movies;
        }

        public async Task UpdateMovie(Movie movie)
        {
            context.Entry(movie).State = EntityState.Detached;
            Movie? movieDB = await context.Movies
                .Include(x => x.MoviesActors)
                .Include(x => x.MoviesGenres)
                .FirstOrDefaultAsync(x => x.Id == movie.Id);

            movieDB = mapper.Map(movie, movieDB);

            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var moviePoster = Convert.FromBase64String(movie.Poster);
                movieDB!.Poster = await fileStorageService.EditFile(moviePoster,
                    "jpg", containerName, movieDB.Poster!);
            }

            if (movie.MoviesActors is not null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            movieDB!.MoviesActors = movie?.MoviesActors!;
            movieDB!.MoviesGenres = movie?.MoviesGenres!;

            await context.SaveChangesAsync();
        }
    }
}
