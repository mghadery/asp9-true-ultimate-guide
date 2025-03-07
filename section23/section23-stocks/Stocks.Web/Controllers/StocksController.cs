using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stocks.Models;
using Stocks.ServiceContracts.DTOs;
using Stocks.ServiceContracts.Interfaces;
using Stocks.Services;

namespace Stocks.Web.Controllers;

public class StocksController : Controller
{
    private readonly string[] _top25PopularStocks;
    private readonly IFinnhubService _finnhubService;

    public StocksController(IOptions<TradingOptions> options, IFinnhubService finnhubService)
    {
        _top25PopularStocks = options.Value.Top25PopularStocks?.Split(",") ?? new string[0];
        _finnhubService = finnhubService;
    }

    [Route("stocks/explore/{stockSymbol?}")]
    [HttpGet]
    public async Task<IActionResult> Explore(string? stockSymbol)
    {
        //var tasks = _top25PopularStocks.Select(async x=>
        //new Stock()
        //{
        //    StockSymbol = x,
        //    StockName = 
        //(await _finnhubService.GetCompanyProfile(x))?["name"].ToString()
        //}).ToList();
        //var model = (await Task.WhenAll(tasks)).ToList();

        var stocks = await _finnhubService.GetStocks();
        var model = stocks.Where(x => _top25PopularStocks.Contains(x["symbol"]))
            .Select(x => new Stock() { StockName = x["description"], StockSymbol = x["symbol"] })
            .ToList();
        ViewBag.StockSymbol = stockSymbol;
        return View(model);
    }

    [Route("stocks/ExploreDefaultStocksSorted/{stockSymbol?}")]
    [HttpGet]
    public async Task<IActionResult> ExploreDefaultStocksSorted(string? stockSymbol)
    {
        //var tasks = _top25PopularStocks.Select(async x=>
        //new Stock()
        //{
        //    StockSymbol = x,
        //    StockName = 
        //(await _finnhubService.GetCompanyProfile(x))?["name"].ToString()
        //}).ToList();
        //var model = (await Task.WhenAll(tasks)).ToList();

        var model = await _finnhubService.GetDefaultStocksSorted(_top25PopularStocks);

        ViewBag.StockSymbol = stockSymbol;
        return View(model);
    }
}
