namespace FinancialAccounts.Tests;

using System;
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

/// <summary>
/// ����� ����������.
/// </summary>
public class AccountTests
{
    /// <summary>
    /// ����� ����������.
    /// </summary>
    public AccountTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var server = new TestServer(new WebHostBuilder()
            .UseStartup<Startup>()
            .UseEnvironment("Development")
            .UseConfiguration(configuration));

        _client = server.CreateClient();
    }

    /// <summary>
    /// ������ �������.
    /// </summary>
    private const string REQUEST_URI = "/api/v1/account/";

    /// <summary>
    /// ����������� ID-������������.
    /// </summary>
    private const int MIN_ID = 1;

    /// <summary>
    /// ������������ ID-������������.
    /// </summary>
    private const int MAX_ID = 51;

    /// <summary>
    /// ����������� ID-������������.
    /// </summary>
    private readonly HttpClient _client;

    /// <summary>
    /// ������� ������.
    /// </summary>
    /// <param name="account"> ������. </param>
    /// <param name="method"> <see cref="HttpMethod"/>. </param>
    /// <param name="requestUri"> ������ �������. </param>
    /// <returns> ������. </returns>
    private static HttpRequestMessage CreateRequest(Account account, HttpMethod method, string requestUri)
    {
        var body = JsonConvert.SerializeObject(account);
        return new HttpRequestMessage(method, requestUri)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
    }

    /// <summary>
    /// ������������ ������� ������ �� ������.
    /// </summary>
    /// <param name="id"> ID-������������. </param>
    private async Task Deposit(int id)
    {
        decimal[] amounts = { 0, 2, -3, 6, -2, -1, 8, -5, 11, -6 };
        var requests = amounts
            .Select(amount => CreateDepositRequest(id, amount))
            .ToList();

        foreach (var request in requests)
        {
            await _client.SendAsync(request).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// ������� ������ 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    private static HttpRequestMessage CreateDepositRequest(int id, decimal amount)
    {
        return new HttpRequestMessage(HttpMethod.Put, REQUEST_URI + id + "?sum=" + amount);
    }

    /// <summary>
    /// ������� �������� �������� � ����.
    /// </summary>
    private async Task CreateAccounts()
    {
        for (var id = MIN_ID; id < MAX_ID; id++)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, REQUEST_URI + id);
            var response = await _client.SendAsync(request).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                await CreateAccount(id).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// ���������������� �������. 
    /// </summary>
    /// <param name="id"> ID-�������. </param>
    private async Task CreateAccount(int id)
    {
        var account = new Account
        {
            Family = $"TestFamily{id}",
            Name = $"TestName{id}",
            SecondName = $"TestSecondName{id}",
            Sum = 1000,
            BirthDate = DateTime.Now.AddDays(-id)
        };

        var request = CreateRequest(account, HttpMethod.Post, REQUEST_URI);
        var response = await _client.SendAsync(request).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>
    /// ���� ������� ����������/������ ����� �� ����� �������.
    /// </summary>
    private async Task TestDeposit()
    {
        for (var id = MIN_ID; id < MAX_ID; id++)
        {
            var tasks = new Task[10];
            for (var i = 0; i < tasks.Length; i++)
            {
                var index = id;
                tasks[i] = Task.Run(() => Deposit(index));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            var sum = await CheckSum(id).ConfigureAwait(false);
            Assert.Equal(1100, sum);

            var request = CreateDepositRequest(id, -100);
            await _client.SendAsync(request).ConfigureAwait(false);
            var changedSum = await CheckSum(id).ConfigureAwait(false);
            Assert.Equal(1000, changedSum);
        }
    }

    /// <summary>
    /// ������� �������� ����� ������������.
    /// </summary>
    /// <param name="id"> ID-������������. </param>
    /// <returns> ���������� ����� �����. </returns>
    private async Task<decimal> CheckSum(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, REQUEST_URI + id);
        var response = await _client.SendAsync(request).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var formattedContent = content.Replace(".", ",");
        return decimal.Parse(formattedContent);
    }

    /// <summary>
    /// - ���������� � ����� - ����������� 50 �������������.
    /// - ���������� � 10 ������� ����������/�������� ���� �������������.
    /// </summary>
    [Fact]
    public async Task TestAccount()
    {
        for (var i = 0; i < 10; i++)
        {
            await CreateAccounts().ConfigureAwait(false);
            await TestDeposit().ConfigureAwait(false);
        }
    }
}