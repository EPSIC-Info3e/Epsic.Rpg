using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Epsic.Rpg.Tests.Controllers
{
    public class ApiControllerTestBase : WebApplicationFactory<Startup>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public ApiControllerTestBase()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        protected async Task<T> GetAsync<T>(string url) 
        {
            var response = await _client.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<T>(body);
        } 

        protected async Task<string> GetAsync(string url) 
        {
            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        } 

        protected async Task<T> PostAsync<T>(string url, T body) 
        {
            var response = await _client.PostAsJsonAsync(url, body);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        } 

        protected async Task<T> PutAsync<T>(string url, T body) 
        {
            var response = await _client.PutAsJsonAsync(url, body);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        } 

        protected async Task<HttpResponseMessage> DeleteAsync(string url) 
        {
            var response = await _client.DeleteAsync(url);

            return response.EnsureSuccessStatusCode();
        } 
    }
}