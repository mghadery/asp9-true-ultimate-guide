using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.DTOs;

public interface IOrderRequest
{
    public string? StockSymbol { get; set; }

    public string? StockName { get; set; }
    public DateTime DateAndTimeOfOrder { get; set; }

}
