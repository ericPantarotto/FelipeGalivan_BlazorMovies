using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlazorMovies.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonRepository personRepository;

        public PeopleController(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>?>> Get()
        {
            return await personRepository.GetPeople();
        }
        [HttpGet]
        [Route("paginate")]
        public async Task<ActionResult<List<Person>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var paginatedResponse = await personRepository.GetPeople(paginationDTO);

            HttpContext.InsertPaginationParametersInResponse(paginatedResponse.TotalAmountPages);

            return paginatedResponse.Response!;

        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>?>> FilterByName(string searchText)
        {
            return await personRepository.GetPeopleByName(searchText);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            Person? person = await personRepository.GetPersonById(id);
            if (person is null) { return NotFound(); }
            return person;
        }

        [HttpGet("detail/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DetailsPersonDTO>> GetDetail(int id)
        {
            DetailsPersonDTO? detailPersonDTO = await personRepository.GetPersonDetailById(id);
            if (detailPersonDTO is null) { return NotFound(); }

            return detailPersonDTO;
        }

        //[HttpGet("search/{searchText}")]
        //public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        //{
        //    if (string.IsNullOrWhiteSpace(searchText)) { return new List<Person>(); }
        //    return await context.People.Where(x => x.Name.Contains(searchText))
        //        .Take(5)
        //        .ToListAsync();
        //}

        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            await personRepository.CreatePerson(person);
            return person.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            var personDB = await personRepository.GetPersonById(person.Id);

            if (personDB is null) { return NotFound(); }

            await personRepository.UpdatePerson(person);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var person = await personRepository.GetPersonById(id);
            if (person is null)
            {
                return NotFound();
            }

            await personRepository.DeletePerson(id);
            return NoContent();
        }
    }
}
