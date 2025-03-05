using Stocks.Entities;
using Stocks.Helpers.Validators;
using Stocks.ServiceContracts.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Stocks.ServiceContracts.DTOs;

public record BuyOrderRequest : OrderRequest
{
    [Required]
    public string? StockSymbol { get; set; }

    [Required]
    public string? StockName { get; set; }

    [Display(Name = "DateAndTime Of Order")]
    [DateTimeValidation("2000-01-01", ErrorMessage = "The {0} must be equal to or greatr than {1}")]
    public DateTime DateAndTimeOfOrder { get; set; }
    
    [Display(Name = "BuyOrder Quantity")]
    [Range(1, 100000, ErrorMessage = "{0} must be between {2} and {1}")]
    public uint Quantity { get; set; }

    [Display(Name = "BuyOrder Price")]
    [Range(1, 10000, ErrorMessage = "{0} must be between {2} and {1}")]
    public double Price { get; set; }

    public static explicit operator BuyOrder(BuyOrderRequest buyOrderRequest)
    {
        return new BuyOrder
        {
            DateAndTimeOfOrder = buyOrderRequest.DateAndTimeOfOrder,
            Price = buyOrderRequest.Price,
            Quantity = buyOrderRequest.Quantity,
            StockName = buyOrderRequest.StockName,
            StockSymbol = buyOrderRequest.StockSymbol
        };
    }
}
