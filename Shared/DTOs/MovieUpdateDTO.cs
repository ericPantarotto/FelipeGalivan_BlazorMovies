using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorMovies.Shared.DTOs
{
    public class MovieUpdateDTO
    {
        public Movie? Movie { get; set; }
        public List<Person> Actors { get; set; } = new();
        public List<Genre> SelectedGenres { get; set; } = new();
        public List<Genre> NotSelectedGenres { get; set; } = new();
    }
}
