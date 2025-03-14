using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models;


public record Product
{
    public Product(int id, string name)
    {
        Id= id; Name = name;
    }
    [Range(0, int.MaxValue, ErrorMessage = "Only positive")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is blank!")]
    public string Name { get; set; }
}

