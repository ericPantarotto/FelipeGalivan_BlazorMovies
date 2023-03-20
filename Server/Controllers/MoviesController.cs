using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMapper mapper;
        private readonly string containerName = "movies";

        public MoviesController(ApplicationDbContext context, 
            IFileStorageService fileStorageService,
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            int limit = 6;

            var moviesInTheaters = await context.Movies
                .Where(x => x.InTheaters).Take(limit)
                .OrderByDescending(x => x.ReleaseDate)
                .ToListAsync();

            DateTime todaysDate = DateTime.Today;

            var upcomingReleases = await context.Movies
                .Where(x => x.ReleaseDate >= todaysDate)
                .OrderBy(x => x.ReleaseDate).Take(limit)
                .ToListAsync();

            IndexPageDTO response = new() 
            {
                InTheaters = moviesInTheaters,
                UpcomingReleases = upcomingReleases
            };

            return response;

        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<DetailsMovieDTO>> Get(int id)
        {
            Movie? movie = await context.Movies.Where(x => x.Id == id)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Person)
                .FirstOrDefaultAsync();

            if (movie is null) { return NotFound(); }

            double voteAverage = 0.0;
            int userVote = 0;

            if (await context.MovieRatings.AnyAsync(x => x.MovieId == id))
            {
                voteAverage = await context.MovieRatings.Where(x => x.MovieId == id)
                    .AverageAsync(x => x.Rate);

                if (HttpContext.User.Identity!.IsAuthenticated)
                {
                    var user = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name!);
                    var userId = user?.Id;

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

        [HttpPost("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Movie>>> Filter(FilterMoviesDTO filterMoviesDTO)
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
                moviesQueryable = moviesQueryable.Where(x => x.ReleaseDate >= today);
            }

            if (filterMoviesDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.MoviesGenres.Select(y => y.GenreId)
                    .Contains(filterMoviesDTO.GenreId));
            }

            if (filterMoviesDTO.OrderByVotes)
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

            await HttpContext.InsertPaginationParametersInResponse(moviesQueryable,
                filterMoviesDTO.RecordsPerPage);

            var movies =   await moviesQueryable.Paginate(filterMoviesDTO.Pagination).ToListAsync();
            return movies;
        }



        [HttpGet("update/{id}")]
        public async Task<ActionResult<MovieUpdateDTO>> PutGet(int id)
        {
            var movieActionResult = await Get(id);
            if (movieActionResult.Result is NotFoundResult) { return NotFound(); }

            DetailsMovieDTO? movieDetailDTO = movieActionResult.Value;
            List<int>? selectedGenresIds = movieDetailDTO?.Genres.Select(x => x.Id).ToList();
            List<Genre>? notSelectedGenres = await context.Genres
                .Where(x => !selectedGenresIds!.Contains(x.Id))
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

        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var poster = Convert.FromBase64String(movie.Poster);
                movie.Poster = await fileStorageService.SaveFile(poster, "jpg", containerName);
            }

            if (movie.MoviesActors is not null)
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

        [HttpPut]
        public async Task<ActionResult> Put(Movie movie)
        {
            Movie? movieDB = await context.Movies.FirstOrDefaultAsync(x => x.Id == movie.Id);

            if (movieDB is null) { return NotFound(); }

            movieDB = mapper.Map(movie, movieDB);

            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var moviePoster = Convert.FromBase64String(movie.Poster);
                movieDB.Poster = await fileStorageService.EditFile(
                    content: moviePoster,
                    extension: "jpg", 
                    containerName: containerName, 
                    fileRoute: movieDB?.Poster!);
            }

            await context.Database.ExecuteSqlInterpolatedAsync($"delete from MoviesActors where MovieId = {movie.Id}; delete from MoviesGenres where MovieId = {movie.Id}");

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
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie is null)
            {
                return NotFound();
            }

            context.Remove(movie);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
