using Stocks.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace Stocks.Entities;

public class BuyOrder
{
    public Guid BuyOrderID { get; set; }
    [Required]
    public string? StockSymbol { get; set; }
    [Required]
    public string? StockName { get; set; }
    [DateTimeValidation("2000-01-01")]
    public DateTime DateAndTimeOfOrder { get; set; }
    [Range(1, 100-000)]
    public uint Quantity { get; set; }
    [Range(1, 10-000)]
    public double Price { get; set; }
}
