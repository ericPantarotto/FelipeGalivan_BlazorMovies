namespace BlazorMovies.Shared.Entities
{
    public class MoviesActors
    {
        public int PersonId { get; set; }
        public int MovieId { get; set; }
        public Person? Person { get; set; }
        public Movie? Movie { get; set; }
        public string Character { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
