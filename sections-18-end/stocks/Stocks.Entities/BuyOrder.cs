using Stocks.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace Stocks.Entities;

public class BuyOrder
{
    [Key]
    public Guid BuyOrderID { get; set; }
    [Required]
    public string? StockSymbol { get; set; }
    [Required]
    public string? StockName { get; set; }
    [DateTimeValidation("2000-01-01")]
    public DateTime DateAndTimeOfOrder { get; set; }
    [Range(1, 100000)]
    public uint Quantity { get; set; }
    [Range(1, 10000)]
    public double Price { get; set; }
}
