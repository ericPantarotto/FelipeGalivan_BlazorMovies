﻿using BlazorMovies.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovies.Shared.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(
            ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = nameof(Resource.required)
        )]
        public string Name { get; set; } = string.Empty;
        public List<MoviesGenres> MoviesGenres { get; set; } = new ();
    }
}
