using Stocks.Entities;

namespace Stocks.ServiceContracts.DTOs;

public record SellOrderResponse
{
    public Guid SellOrderID { get; set; }
    public string? StockSymbol { get; set; }
    public string? StockName { get; set; }
    public DateTime DateAndTimeOfOrder { get; set; }
    public uint Quantity { get; set; }
    public double Price { get; set; }
    public double TradeAmount { get; set; }

    public static explicit operator SellOrderResponse(SellOrder sellOrder)
    {
        return new SellOrderResponse
        {
             SellOrderID = sellOrder.SellOrderID,
            StockSymbol = sellOrder.StockSymbol,
            StockName = sellOrder.StockName,
            Quantity = sellOrder.Quantity,
            Price = sellOrder.Price,
            DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder
        };
    }

}
