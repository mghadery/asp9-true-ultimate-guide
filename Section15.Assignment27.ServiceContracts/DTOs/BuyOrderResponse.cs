using Section15.Assignment27.Entities;

namespace Section15.Assignment27.ServiceContracts.DTOs;

public record BuyOrderResponse
{
    public Guid BuyOrderID { get; set; }
    public string? StockSymbol { get; set; }
    public string? StockName { get; set; }
    public DateTime DateAndTimeOfOrder { get; set; }
    public uint Quantity { get; set; }
    public double Price { get; set; }
    public double TradeAmount { get; set; }
    public static explicit operator BuyOrderResponse(BuyOrder buyOrder)
    {
        return new BuyOrderResponse
        {
            BuyOrderID = buyOrder.BuyOrderID,
            StockSymbol = buyOrder.StockSymbol,
            StockName = buyOrder.StockName,
            Quantity = buyOrder.Quantity,
            Price = buyOrder.Price,
            DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder
        };
    }
}
