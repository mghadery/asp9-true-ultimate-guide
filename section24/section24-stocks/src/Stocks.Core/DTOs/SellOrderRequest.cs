using Stocks.Core.Domain.Entities;
using Stocks.Core.Validators;
using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.DTOs;

public record SellOrderRequest : IOrderRequest
{
    [Required]
    public string? StockSymbol { get; set; }
    [Required]
    public string? StockName { get; set; }
    [Display(Name = "DateAndTime Of Order")]
    [DateTimeValidation("2000-01-01", ErrorMessage = "The {0} must be equal to or greatr than {1}")]
    public DateTime DateAndTimeOfOrder { get; set; }
    [Range(1, 100000)]
    public uint Quantity { get; set; }
    [Range(1, 10000)]
    public double Price { get; set; }

    public static explicit operator SellOrder(SellOrderRequest sellOrderRequest)
    {
        return new SellOrder
        {
            DateAndTimeOfOrder = sellOrderRequest.DateAndTimeOfOrder,
            Price = sellOrderRequest.Price,
            Quantity = sellOrderRequest.Quantity,
            StockName = sellOrderRequest.StockName,
            StockSymbol = sellOrderRequest.StockSymbol
        };
    }
}
