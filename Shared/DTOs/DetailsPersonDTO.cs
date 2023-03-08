using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Shared.DTOs
{
    public class DetailsPersonDTO
    {
        public Person? Person { get; set; }
        public List<Movie?>? Movies { get; set; }
    }
}
