namespace BlazorMovies.Client.Helpers
{
    public class HttpClientWithToken
    {
        public HttpClient HttpClient { get; }
        public HttpClientWithToken(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

    }
}
