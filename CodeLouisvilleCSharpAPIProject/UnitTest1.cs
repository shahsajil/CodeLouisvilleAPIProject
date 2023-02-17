using FluentAssertions;
using FluentAssertions.Execution;
using System.Configuration;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace CodeLouisvilleCSharpAPIProject;
public class UnitTest
{
    private readonly ITestOutputHelper output;
    public UnitTest(ITestOutputHelper output)
    {
        this.output = output;
    }

        [Fact]
        public async Task GetSteamApiTest()
        {
            string url = "https://www.valvesoftware.com/about/stats";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.valvesoftware.com");

            var response = await client.GetAsync("/about/stats");
            var content = await response.Content.ReadAsStringAsync();

            var stats = System.Text.Json.JsonSerializer.Deserialize<SteamstatsResponse>(content);
            var onlineUser = int.Parse(stats.users_online, System.Globalization.NumberStyles.AllowThousands);

            using (new AssertionScope())
            {
                response.IsSuccessStatusCode.Should().BeTrue();
                onlineUser.Should().BeGreaterThanOrEqualTo(0);
            }    
        }

        [Fact]
        public async Task GetQueryParamJsonPlaceholder()
        {
            string baseUrl = "https://jsonplaceholder.typicode.com/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            var response = await client.GetAsync("posts?id=91");
            var content = await response.Content.ReadAsStringAsync();
            output.WriteLine(content);

            var post = System.Text.Json.JsonSerializer.Deserialize<List<Posts>>(content);


            using (new AssertionScope())
            {
                response.IsSuccessStatusCode.Should().BeTrue();
                post[0].title.Should().Be("aut amet sed");
               
            }

        }

         [Fact]
         public async Task GetQueryParamJsonPlaceholder1()
         {
            string baseUrl = "https://jsonplaceholder.typicode.com/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            var response = await client.GetAsync("posts?id=2");
            var content = await response.Content.ReadAsStringAsync();

            var post = JsonConvert.DeserializeObject<List<Posts>>(content);


            using (new AssertionScope())
            {
                response.IsSuccessStatusCode.Should().BeTrue();
                post[0].title.Should().Be("qui est esse");

            }

         }

        [Fact]
        public async Task TestCreateUserPost()
        {
            string baseUrl = "https://reqres.in/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            var requestContent = new StringContent(
                JsonConvert.SerializeObject(new
            {
                name = "Sajil",
                job = "QA Engineer"
            }),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/users", requestContent);
            var content = await response.Content.ReadAsStringAsync();

            var result = System.Text.Json.JsonSerializer.Deserialize<User>(content);

            using (new AssertionScope())
            {
                response.IsSuccessStatusCode.Should().BeTrue();
                result.name.Should().Be("Sajil");
                result.job.Should().Be("QA Engineer");

            }

        }

        [Fact]
        public async Task TestDeleteUSerNotFoundForNegative()
        {
            string baseUrl = "https://reqres.in/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

        var response = await client.DeleteAsync("/api/users/9999");

        using (new AssertionScope())
            {
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }
} 

