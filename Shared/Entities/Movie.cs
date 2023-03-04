using System.ComponentModel.DataAnnotations;

namespace BlazorMovies.Shared.Entities
{
    public class Movie {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public bool InTheaters { get; set; }
        public string Trailer { get; set; } = string.Empty;
        [Required]
        public DateTime? ReleaseDate { get; set; }
        public string? Poster { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; } = new();
        public List<MoviesActors> MoviesActors { get; set; } = new();
        public string? TitleBrief { get 
            {
                if (string.IsNullOrEmpty(Title))
                {
                    return null;
                }
                if (Title.Length > 60)
                {
                    return $"{Title[60..]} ...";
                }
                return Title;
            }
        }
    }
}
