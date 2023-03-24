using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Shared.Repositories
{
    public interface IRatingRepository
    {
        Task Vote(MovieRating movieRating);
    }
}
