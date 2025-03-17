using System.ComponentModel.DataAnnotations;

namespace OrderManager.Core.Domain.Entities;

public class Order
{
    public Guid OrderId { get; set; }
    [Required(ErrorMessage = "Order number is mandatory")]
    [RegularExpression(@"^\d{4}_\d+$", ErrorMessage = "Order number pattern is incorrect")]
    public string? OrderNumber { get; set; }
    [Required(ErrorMessage = "Customer name is mandatory")]
    [MaxLength(50, ErrorMessage = "Customer name cannot exceed {0} characters")]
    public string? CustomerName { get; set; }
    [Required(ErrorMessage = "Order date is mandatory")]
    public DateTime? OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ICollection<OrderItem>? OrderItems { get; set; }
}
