namespace FinancialAccounts.Tests
{
    using System.Linq;
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

    public class AccountTests
    {
        public AccountTests()
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

        private const int MIN_ID = 1;
        private const int MAX_ID = 51;

        private async Task DeleteAccounts()
        {
      
            for (var id = MIN_ID; id < MAX_ID; id++)
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, REQUEST_URI + id);

                var response = await _client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        private async Task Deposit(int id)
        {
            decimal[] amounts = { 0, 2, -3, 6, -2, -1, 8, -5, 11, -6 };
            var requests = amounts
                .Select(amount => new HttpRequestMessage(HttpMethod.Put, REQUEST_URI + id + "?sum=" + amount))
                .ToList();

            var getRequest = new HttpRequestMessage(HttpMethod.Get, REQUEST_URI + id);
            var getResponse = await _client.SendAsync(getRequest).ConfigureAwait(false);
            var getContent = await getResponse.Content.ReadAsStringAsync();
            for (var index = 0; index < requests.Count; index++)
            {
                var request = requests[index];
                var response = await _client.SendAsync(request).ConfigureAwait(false);
                //var content = await response.Content.ReadAsStringAsync();

                //lock (response)
                //{
                //    var sum = decimal.Parse(getContent.Replace(".", ","));
                //    if (sum + amounts[index] < 0)
                //    {
                //        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                //    }
                //    else
                //    {
                //        sum += decimal.Parse(content.Replace(".", ","));
                //        response.EnsureSuccessStatusCode();
                //        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                //    }
                //}
            }
        }

        private async Task CreateAccounts()
        {
            for (var id = MIN_ID; id < MAX_ID; id++)
            {
                var account = new Account
                {
                    Family = $"TestFamily{id}",
                    Name = $"TestName{id}",
                    SecondName = $"TestSecondName{id}",
                    Id = id,
                };

                var request = CreateRequest(account, HttpMethod.Post, REQUEST_URI);

                var response = await _client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        private async Task DoOperation()
        {
            for (var id = MIN_ID; id < MAX_ID; id++)
            {
                var tasks = new Task[10];
                for (var i = 0; i < tasks.Length; i++)
                {
                    var index = id;
                    tasks[i] = Task.Run(() => Deposit(index));
                }

                await Task.WhenAll(tasks);
            }
        }

        [Fact]
        public async Task Test()
        {
            try
            {
                await CreateAccounts();
                await DoOperation();
            }
            finally
            {
                await DeleteAccounts();
            }
        }
    }
}