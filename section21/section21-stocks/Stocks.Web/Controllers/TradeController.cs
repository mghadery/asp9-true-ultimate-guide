using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stocks.Models;
using Stocks.ServiceContracts.DTOs;
using Stocks.ServiceContracts.Interfaces;
using Stocks.Web.Models;
using Rotativa.AspNetCore;
using Stocks.Web.Filters;

namespace Stocks.Controllers;

[Route("[Controller]")]
public class TradeController(
    IConfiguration configuration,
    IOptions<TradingOptions> options,
    IFinnhubService finnhubService,
    IStocksService stocksService,
    ILogger<TradeController> logger
    ) : Controller
{
    [Route("/{stockSymbol?}")]
    [Route("index/{stockSymbol?}")]
    [HttpGet]
    public async Task<IActionResult> Index(string? stockSymbol)
    {
        logger.LogInformation(
            "Action: {0} in controller: {1} with stockSymbol: {2} started",
            ControllerContext.ActionDescriptor.ActionName,
            ControllerContext.ActionDescriptor.ControllerName,
            stockSymbol);

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
    [TypeFilter(typeof(ValidateRequestFilter))]
    public async Task<IActionResult> BuyOrder(BuyOrderRequest request)
    {
        //ModelState.Remove("DateAndTimeOfOrder");
        //if (!ModelState.IsValid)
        //{
        //    return RedirectToAction("index", "trade", new { stockSymbol = request.StockSymbol });
        //}

        //request.DateAndTimeOfOrder = DateTime.Now;
        await stocksService.CreateBuyOrder(request);
        return RedirectToAction("orders");
    }

    [Route("[Action]")]
    [HttpPost]
    [TypeFilter(typeof(ValidateRequestFilter))]
    public async Task<IActionResult> SellOrder(SellOrderRequest request)
    {
        //ModelState.Remove("DateAndTimeOfOrder");
        //if (!ModelState.IsValid)
        //{
        //    return RedirectToAction("index", "trade", new { stockSymbol = request.StockSymbol });
        //}

        //request.DateAndTimeOfOrder = DateTime.Now;
        await stocksService.CreateSellOrder(request);
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
        var orders = buyOrders.Union(sellOrders).OrderBy(x => x.DateAndTimeOfOrder).ToList();
        return new ViewAsPdf("ordersAsPdf", orders, ViewData);
    }
}
