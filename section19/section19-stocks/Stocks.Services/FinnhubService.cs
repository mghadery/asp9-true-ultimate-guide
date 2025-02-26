using Microsoft.Extensions.Configuration;
using Stocks.ServiceContracts.Interfaces;
using System.Text.Json;

namespace Stocks.Services;

public class FinnhubService(IConfiguration configuration, IHttpClientFactory httpClientFactory) : IFinnhubService
{
    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
    {
        string token = configuration["token"];
        string url = $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={token}";
        using(HttpClient httpClient = httpClientFactory.CreateClient())
        {
            HttpRequestMessage httpRequestMessage = 
                new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponseMessage.StatusCode != System.Net.HttpStatusCode.OK) return null;
            Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
            using (StreamReader streamReader = new StreamReader(stream))
            {
                var resp = streamReader.ReadToEnd();
                var result = JsonSerializer.Deserialize<Dictionary<string, object>>(resp);
                if (result == null || result.Count == 0) throw new InvalidOperationException();
                return result;
            }
        }
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
    {
        string token = configuration["token"];
        string url = $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={token}";
        using (HttpClient httpClient = httpClientFactory.CreateClient())
        {
            HttpRequestMessage httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponseMessage.StatusCode != System.Net.HttpStatusCode.OK) return null;
            Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
            using (StreamReader streamReader = new StreamReader(stream))
            {
                var resp = streamReader.ReadToEnd();
                var result = JsonSerializer.Deserialize<Dictionary<string, object>>(resp);
                if (result == null || result.Count == 0) throw new InvalidOperationException();
                return result;
            }
        }
    }
}
