using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository
{
    public interface IRatingRepository
    {
        Task Vote(MovieRating movieRating);
    }
}
