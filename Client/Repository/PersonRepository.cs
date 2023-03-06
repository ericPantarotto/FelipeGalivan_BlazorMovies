using BlazorMovies.Client.Helpers;
//using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;

namespace BlazorMovies.Client.Repository
{
    public class PersonRepository: IPersonRepository
    {
        private readonly IHttpService httpService;
        private readonly string url = "api/people";

        public PersonRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<List<Person>?> GetPeople()
        {
            var response = await httpService.Get<List<Person>>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        //public async Task<PaginatedResponse<List<Person>>> GetPeople(PaginationDTO paginationDTO)
        //{
        //    return await httpService.GetHelper<List<Person>>(url, paginationDTO);
        //}

        public async Task<List<Person>?> GetPeopleByName(string name)
        {
            var response = await httpService.Get<List<Person>>($"{url}/search/{name}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<Person?> GetPersonById(int id)
        {
            return await httpService.GetHelper<Person>($"{url}/{id}");
        }

        public async Task CreatePerson(Person person)
        {
            var response = await httpService.Post(url, person);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdatePerson(Person? person)
        {
            var response = await httpService.Put(url, person);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task DeletePerson(int Id)
        {
            var response = await httpService.Delete($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
        private async Task<T?> Get<T>(string url)
        {
            var response = await httpService.Get<T?>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }
    }
}
