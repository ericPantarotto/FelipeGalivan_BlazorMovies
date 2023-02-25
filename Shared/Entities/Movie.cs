namespace BlazorMovies.Shared.Entities
{
    public class Movie {
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; } = string.Empty;
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
