using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Section14.Assignment25.Models;
using Section14.Assignment25.ServiceContracts;

namespace Section14.Assignment25.Controllers;

public class TradeController(
    IConfiguration configuration,
    IOptions<TradingOptions> options, 
    IFinnhubService finnhubService) : Controller
{
    [Route("/{stockSymbol?}")]
    [HttpGet]
    public async Task<IActionResult> Index(string? stockSymbol)
    {
        if (stockSymbol is null)
            stockSymbol = options.Value.DefaultStockSymbol;

        var stockQuote = await finnhubService.GetStockPriceQuote(stockSymbol);
        var profile = await finnhubService.GetCompanyProfile(stockSymbol);

        StockTrade stockTrade = new()
        {
            StockSymbol = stockSymbol,
            StockName = profile?["name"].ToString(),
            Price = Convert.ToDouble(stockQuote?["c"].ToString()),
        };
        ViewBag.Token = configuration["token"]?.ToString();
        return View(stockTrade);
    }
}
