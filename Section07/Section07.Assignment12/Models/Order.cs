using Microsoft.AspNetCore.Mvc.ModelBinding;
using Section07.Assignment12.Validators;
using System.ComponentModel.DataAnnotations;

namespace Section07.Assignment12.Models;
public class Order
{
    [Display(Name = "Order number")]
    public int? OrderNo { get; set; }

    [Display(Name = "Order date")]
    [Required(ErrorMessage = "{0} is mandatory")]
    [MinDateTime("2000-01-01", ErrorMessage = "{0} cannot be less than {1}")]
    public DateTime OrderDate { get; set; }

    [Display(Name = "Invoice price")]
    [Required(ErrorMessage = "{0} is mandatory")]
    [ListPriceCheck("Products", ErrorMessage = "{0} does not mach to {1} total price)")]
    public double InvoicePrice { get; set; }

    [Required(ErrorMessage = "{0} is mandatory")]
    [ProductList(1, ErrorMessage = "{0} count must be at least {1}")]
    public List<Product> Products { get; set; } = new();
}
