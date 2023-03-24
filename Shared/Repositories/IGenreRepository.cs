using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Shared.Repositories
{
    public interface IGenreRepository
    {
        Task CreateGenre(Genre genre);
        Task<Genre?> GetGenre(int Id);
        Task<List<Genre>?> GetGenres();
        Task UpdateGenre(Genre genre);
        Task DeleteGenre(int Id);
    }
}
