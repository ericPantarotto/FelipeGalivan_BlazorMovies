using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Shared.DTOs
{
    public class IndexPageDTO
    {
        public List<Movie> InTheaters { get; set; } = new();
        public List<Movie> UpcomingReleases { get; set; } = new();
    }
}
