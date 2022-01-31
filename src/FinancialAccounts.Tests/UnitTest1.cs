namespace FinancialAccounts.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using FinancialAccounts.Web;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;

    using Newtonsoft.Json;

    using Xunit;

    public class UnitTest1
    {
        public UnitTest1()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true).Build();

            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Development")
                .UseConfiguration(configuration));

            _client = server.CreateClient();
        }

        /// <summary>
        /// Строка запроса.
        /// </summary>
        private const string REQUEST_URI = "/api/v1/account/";

        private readonly HttpClient _client;

        /// <summary>
        /// Создать запрос.
        /// </summary>
        /// <param name="account"> Модель. </param>
        /// <param name="method"> <see cref="HttpMethod"/></param>
        /// <param name="requestUri"> Строка запроса. </param>
        /// <returns> Запрос. </returns>
        private static HttpRequestMessage CreateRequest(Account account, HttpMethod method, string requestUri)
        {
            var body = JsonConvert.SerializeObject(account);
            return new HttpRequestMessage(method, requestUri)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
        }

        private async Task DeleteData()
        {
            for (var item = 1; item < 51; item++)
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, REQUEST_URI + item);

                var response = await _client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        private async Task CreateTestData()
        {
            for (var id = 1; id < 51; id++)
            {
                var account = new Account
                {
                    Family = $"TestFamily{id}",
                    Name = $"TestName{id}",
                    SecondName = $"TestSecondName{id}",
                };

                var request = CreateRequest(account, HttpMethod.Post, REQUEST_URI);

                var response = await _client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Test()
        {
            await CreateTestData();
            await DeleteData();
        }
    }
}