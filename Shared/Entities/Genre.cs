using System.ComponentModel.DataAnnotations;

namespace BlazorMovies.Shared.Entities {
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; } = string.Empty;
        //public List<MoviesGenres> MoviesGenres { get; set; } = new List<MoviesGenres>();
    }
}
