using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Shared.Repositories
{
    public interface IPersonRepository
    {
        Task CreatePerson(Person person);
        Task DeletePerson(int Id);
        Task<List<Person>?> GetPeople();

        Task<PaginatedResponse<List<Person>>> GetPeople(PaginationDTO paginationDTO);
        Task<List<Person>?> GetPeopleByName(string name);
        Task<Person?> GetPersonById(int id);
        Task<DetailsPersonDTO?> GetPersonDetailById(int id);
        Task UpdatePerson(Person? person);
    }
}
