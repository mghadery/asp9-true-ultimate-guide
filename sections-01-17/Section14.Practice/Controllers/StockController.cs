using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

namespace Section14.Practice.Controllers;

public class StockController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public StockController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [Route("stock")]
    public async Task<IActionResult> Index()
    {
        using (HttpClient httpClient = _httpClientFactory.CreateClient())
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://finnhub.io/api/v1/quote?symbol=AAPL&token=cuhilc1r01qva71ttiegcuhilc1r01qva71ttif0");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
            string response = "";
            using (StreamReader streamReader = new StreamReader(stream))
            {
                response = await streamReader.ReadToEndAsync();

                var q = JsonConvert.DeserializeObject<Quote>(response);
                var qq = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            }
            ViewBag.StockValue = response;
            return View();
        }
    }
}

public class Quote
{
    public string C { get; set; }
    public string D { get; set; }
    public string DP { get; set; }
}
