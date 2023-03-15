using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Shared.DTOs
{
    public class DetailsMovieDTO
    {
        public Movie? Movie { get; set; } = new();
        public List<Genre> Genres { get; set; } = new();
        public List<Person> Actors { get; set; } = new();
        public double AverageVote { get; set; }
        public int UserVote { get; set; }
    }
}
