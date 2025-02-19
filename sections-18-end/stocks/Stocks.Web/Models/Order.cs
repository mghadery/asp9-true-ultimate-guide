
using Stocks.ServiceContracts.DTOs;

namespace Stocks.Web.Models;

public class Order
{
    public string? StockInfo { get; set; }
    public string? DateAndTimeOfOrder { get; set; }
    public uint Quantity { get; set; }
    public double Price { get; set; }
    public string? OrderType { get; set; }

    public static explicit operator Order(BuyOrderResponse response)
    {
        return new Order()
        {
            DateAndTimeOfOrder = response.DateAndTimeOfOrder.ToString("yyyy-MM-dd HH:mm:ss"),
            Quantity = response.Quantity,
            Price = response.Price,
            StockInfo = $"{response.StockName}({response.StockSymbol})",
            OrderType = "BUY"
        };
    }
    public static explicit operator Order(SellOrderResponse response)
    {
        return new Order()
        {
            DateAndTimeOfOrder = response.DateAndTimeOfOrder.ToString("yyyy-MM-dd HH:mm:ss"),
            Quantity = response.Quantity,
            Price = response.Price,
            StockInfo = $"{response.StockName}({response.StockSymbol})",
            OrderType = "SELL"
        };
    }
}
