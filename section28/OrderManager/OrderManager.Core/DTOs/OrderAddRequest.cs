using OrderManager.Core.Domain.Entities;
using OrderManager.Core.Domain.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace OrderManager.Core.DTOs;

public class OrderAddRequest
{
    [Required(ErrorMessage = "Customer name is mandatory")]
    [MaxLength(50, ErrorMessage = "Customer name cannot exceed {0} characters")]
    public string? CustomerName { get; set; }

    public Order ToOrder()
    {
        var now = DateTime.Now;
        var thisYear = new DateTime(now.Year, 1, 1);
        var mil = (long)(now - thisYear).TotalMilliseconds;
        Order order = new Order()
        {
            CustomerName = CustomerName,
            OrderId = Guid.NewGuid(),
            OrderDate = DateTime.Now,
            OrderNumber = $"{now.Year}_{mil}"

        };
        return order;
    }
}
