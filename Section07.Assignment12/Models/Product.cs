using System.ComponentModel.DataAnnotations;

namespace Section07.Assignment12.Models;
public class Product
{
    [Display(Name = "Product code")]
    [Required(ErrorMessage = "{0} is mandatory")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}")]
    public int ProductCode { get; set; }

    [Required(ErrorMessage = "{0} is mandatory")]
    [Range(1, double.MaxValue, ErrorMessage = "{0} must be between {1} and {2}")]
    public double Price { get; set; }

    [Required(ErrorMessage = "{0} is mandatory")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}")]
    public int Quantity { get; set; }
}
