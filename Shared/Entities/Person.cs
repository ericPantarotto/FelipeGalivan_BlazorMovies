﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorMovies.Shared.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string? Picture { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [NotMapped]
        public string Character { get; set; } = string.Empty;

        public override bool Equals(object? obj) {
            if (obj is Person p2) {
                return Id == p2.Id;
            }
            return false;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
