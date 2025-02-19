using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stocks.Models;
using Stocks.ServiceContracts.DTOs;
using Stocks.ServiceContracts.Interfaces;
using Stocks.Web.Models;
using Rotativa.AspNetCore;

namespace Stocks.Controllers;

[Route("[Controller]")]
public class TradeController(
    IConfiguration configuration,
    IOptions<TradingOptions> options, 
    IFinnhubService finnhubService,
    IStocksService stocksService
    ) : Controller
{
    [Route("/{stockSymbol?}")]
    [Route("index/{stockSymbol?}")]
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
        ViewBag.DefaultOrderQuantity = options.Value.DefaultOrderQuantity;
        return View(stockTrade);
    }

    [Route("[Action]")]
    [HttpPost]
    public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
    {
        ModelState.Remove("DateAndTimeOfOrder");
        if (!ModelState.IsValid)
        {
            return RedirectToAction("index", "trade", new { stockSymbol = buyOrderRequest .StockSymbol});
        }

        buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
        await stocksService.CreateBuyOrder(buyOrderRequest);
        return RedirectToAction("orders");
    }

    [Route("[Action]")]
    [HttpPost]
    public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
    {
        ModelState.Remove("DateAndTimeOfOrder");
        if (!ModelState.IsValid)
        {
                return RedirectToAction("index", "trade", new { stockSymbol = sellOrderRequest.StockSymbol });
        }

        sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
        await stocksService.CreateSellOrder(sellOrderRequest);
        return RedirectToAction("orders");
    }


    [Route("[Action]")]
    [HttpGet]
    public async Task<IActionResult> Orders()
    {
        var buyOrders = await stocksService.GetBuyOrders();
        var sellOrders = await stocksService.GetSellOrders();
        Orders orders = new Orders() { BuyOrders = buyOrders, SellOrders = sellOrders };
        return View(orders);
    }

    [Route("[Action]")]
    [HttpGet]
    public async Task<IActionResult> OrdersAsPdf()
    {
        var buyOrders = (await stocksService.GetBuyOrders()).Select(x => (IOrderResponse)x).ToList();
        var sellOrders = (await stocksService.GetSellOrders()).Select(x => (IOrderResponse)x).ToList();
        var orders = buyOrders.Union(sellOrders).OrderBy(x=>x.DateAndTimeOfOrder).ToList();
        return new ViewAsPdf("ordersAsPdf", orders, ViewData);
    }
}
