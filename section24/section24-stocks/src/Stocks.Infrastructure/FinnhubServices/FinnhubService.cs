using Microsoft.Extensions.Configuration;
using Stocks.Core.DTOs;
using Stocks.Core.FinnhubServiceContracts;
using Stocks.ServiceContracts.Interfaces;
using System.Net.Http;
using System.Text.Json;

namespace Stocks.Infrastructure.FinnhubServices;

public class FinnhubService(IConfiguration configuration, IHttpClientFactory httpClientFactory) : IFinnhubService
{
    public async Task<List<Dictionary<string, string>>> GetStocks()
    {
        string token = configuration["token"];
        HttpClient httpClient = httpClientFactory.CreateClient();

        //create http request
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={token}") //URI includes the secret token
        };

        //send request
        HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        //read response body
        string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
        //_diagnosticContext.Set("Response from finnhub", responseBody);

        //convert response body (from JSON into Dictionary)
        List<Dictionary<string, string>>? responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseBody);

        if (responseDictionary == null)
            throw new InvalidOperationException("No response from server");

        //return response dictionary back to the caller
        return responseDictionary;

    }

    public async Task<List<Stock>> GetDefaultStocksSorted(string[] defaultStocks)
    {
        string token = configuration["token"];
        HttpClient httpClient = httpClientFactory.CreateClient();

        //create http request
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={token}") //URI includes the secret token
        };

        //send request
        HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        //read response body
        string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
        //_diagnosticContext.Set("Response from finnhub", responseBody);

        //convert response body (from JSON into Dictionary)
        List<Dictionary<string, string>>? responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseBody);

        if (responseDictionary == null)
            throw new InvalidOperationException("No response from server");

        var stocks = responseDictionary.Where(x => defaultStocks.Contains(x["symbol"]))
            .Select(x => new Stock() { StockName = x["description"], StockSymbol = x["symbol"] })
            .OrderBy(x => x.StockName)
            .ToList();

        //return response dictionary back to the caller
        return stocks;
    }

    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
    {
        string token = configuration["token"];
        string url = $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={token}";
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
