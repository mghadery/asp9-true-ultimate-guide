using OrderManager.Core.Domain.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace OrderManager.Core.Domain.Entities;

public class OrderItem
{
    public Guid OrderItemId { get; set; }
    [Required(ErrorMessage = "Order Id is mandatory")]
    public Guid? OrderId { get; set; }
    [Required(ErrorMessage = "Product name is mandatory")]
    [MaxLength(50, ErrorMessage = "Product name cannot exceed {0} characters")]
    public string? ProductName { get; set; }
    [PositiveValidation(ErrorMessage = "Quantity must be positive")]
    public int Quantity { get; set; }
    [PositiveValidation(ErrorMessage = "Unit price must be positive")]
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public Order? Order { get; set; }

}
