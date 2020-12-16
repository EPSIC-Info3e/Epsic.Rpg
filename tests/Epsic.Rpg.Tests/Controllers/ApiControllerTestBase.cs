using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.EntityFrameworkCore;
using Epsic.Rpg.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Epsic.Rpg.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Epsic.Rpg.Tests.Controllers
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<EpsicRpgDataContext>));
                if (descriptor == null) return;
                services.Remove(descriptor);

                services.AddDbContext<EpsicRpgDataContext>(options =>
                {
                    options.UseInMemoryDatabase("Epsic.Rpg");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<EpsicRpgDataContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        ResetInMemoryDatabase(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, ex.Message);
                    }
                }
            });
        }

        private static void ResetInMemoryDatabase(EpsicRpgDataContext db)
        {
            db.RemoveRange(db.Characters);
            db.SaveChanges();

            db.AddRange(new List<Character>
                        {
                            new Character { Id = 1, Name = "Pierre"},
                            new Character { Id = 2, Name = "Paul"},
                            new Character { Id = 3, Name = "Jacques"},
                        });
            db.SaveChanges();
        }
    }

    public class ApiControllerTestBase : CustomWebApplicationFactory<Startup>
    {
        private CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;
        
        [TestInitialize]
        public void SetupTest()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        protected async Task<HttpResponseMessage> GetAsync(string url) 
        {
            return await _client.GetAsync(url);
        } 

        protected async Task<T> GetAsync<T>(string url) 
        {
            var response = await _client.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(body);
        }

        protected async Task<HttpResponseMessage> PostBasicAsync<T>(string url, T body) 
        {
            return await _client.PostAsJsonAsync(url, body);
        }

        protected async Task<HttpResponseMessage> PostFileAsync(string url, byte[] file) 
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(file), "file", "filename");
            return await _client.PostAsync(url, content);
        }

        protected async Task<T> PostAsync<T>(string url, T body) 
        {
            return await PostAsync<T, T>(url, body);
        }

        protected async Task<U> PostAsync<T, U>(string url, T body) 
        {
            var response = await _client.PostAsJsonAsync(url, body);

            return await response.Content.ReadFromJsonAsync<U>();
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string url) 
        {
            var response = await _client.DeleteAsync(url);

            return response.EnsureSuccessStatusCode();
        } 
    }
}