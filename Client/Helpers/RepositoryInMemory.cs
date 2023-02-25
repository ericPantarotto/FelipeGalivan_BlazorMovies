using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Helpers
{
    public class RepositoryInMemory : IRepository
    {
        public List<Movie> GetMovies()
        {
            return new List<Movie>()
            {
                new Movie(){Title = "Spider-Man: Far From Home", ReleaseDate = new DateTime(2019, 7, 2),
                    Poster = "https://www.thecomicmonster.com/wp-content/uploads/2019/07/4A74685E-53BC-4E6F-9665-EAD23E59092A.jpeg"},
                new Movie(){Title = "Moana", ReleaseDate = new DateTime(2016, 11, 23),
                    Poster = "https://upload.wikimedia.org/wikipedia/en/thumb/2/26/Moana_Teaser_Poster.jpg/220px-Moana_Teaser_Poster.jpg"},
                new Movie(){Title = "Inception", ReleaseDate = new DateTime(2010, 7, 16),
                    Poster="https://upload.wikimedia.org/wikipedia/en/thumb/1/18/Inception_OST.jpg/220px-Inception_OST.jpg"}
            };
        }
    }
}
