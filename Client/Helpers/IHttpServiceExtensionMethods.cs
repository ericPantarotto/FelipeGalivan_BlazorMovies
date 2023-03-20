﻿using BlazorMovies.Shared.DTOs;

namespace BlazorMovies.Client.Helpers
{
    public static class IHttpServiceExtensionMethods
    {
        public static async Task<T?> GetHelper<T>(this IHttpService httpService,
                                                  string url,
                                                  bool includeToken = true)
        {
            var response = await httpService.Get<T>(url, includeToken);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public static async Task DeleteHelper(this IHttpService httpService, string url, int id)
        {
            var response = await httpService.Delete($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public static async Task<PaginatedResponse<T>> GetHelper<T>(this IHttpService httpService,
                                                                    string url,
                                                                    PaginationDTO paginationDTO,
                                                                    bool includeToken = true)
        {
            string newURL = url.Contains('?') ?
                 $"{url}&page={paginationDTO.Page}&recordsPerPage={paginationDTO.RecordsPerPage}" :
                 $"{url}?page={paginationDTO.Page}&recordsPerPage={paginationDTO.RecordsPerPage}";

            var httpResponse = await httpService.Get<T>(newURL, includeToken);
            var totalAmountPages = int.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault()!);
            return  new PaginatedResponse<T>
            {
                Response = httpResponse.Response,
                TotalAmountPages = totalAmountPages
            };
        }

    }
}
