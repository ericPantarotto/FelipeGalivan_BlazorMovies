﻿//using AutoMapper;
using BlazorMovies.Shared.Repositories;
//using BlazorMovies.SharedBackend.Helpers;
using BlazorMovies.SharedBackend.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorMovies.SharedBackend
{
    public static class IServiceCollectionBlazorMovies
    {
        public static IServiceCollection AddBlazorMovies(this IServiceCollection services)
        {
            services.AddScoped<IMoviesRepository, MoviesRepository>();
            //services.AddScoped<IGenreRepository, GenresRepository>();
            //services.AddScoped<IPersonRepository, PersonRepository>();
            //services.AddScoped<IRatingRepository, RatingRepository>();
            //services.AddScoped<IUsersRepository, UsersRepository>();

            //services.AddAutoMapper(new[] { typeof(AutomapperProfiles).Assembly });

            return services;
        }
    }
}
