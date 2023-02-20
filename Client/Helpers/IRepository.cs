using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Helpers
{
    public interface IRepository
    {
        List<Movie> GetMovies();
    }
}
