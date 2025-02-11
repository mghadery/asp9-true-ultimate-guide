using Section07.Practice.Validators;
using System.ComponentModel.DataAnnotations;

namespace Section07.Practice.Models;

public class Person: IValidatableObject
{
    public int Id { get; set; }
    [Required(ErrorMessage = "{0} is manadatory")]
    [Display(Name = "Person Name")]
    [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1}")]
    [RegularExpression("^[a-z A-Z/]+$", ErrorMessage = "{0} must be only chars")]
    public string? Name { get; set; }
    public string? Author { get; set; }
    [Url(ErrorMessage = "Only URL")]
    public string? Website { get; set; }
    public string? Pass { get; set; }
    [Compare("Pass")]
    public string? PassRep { get; set; }
    [Display(Name = "Date of birth")]
    [YearRange(minYear: 1, maxYear: 2100, ErrorMessage = "{0} year not in the rang {1} to {2}")]
    //[YearRange(ErrorMessage = "DoB year not in the rang {1} to {2}")]
    public DateTime? DateOfBirth { get; set; }

    [DateRange("DateOfBirth")]
    public DateTime? DateOfMarriage { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateOfBirth is not null && DateOfBirth > DateTime.Now)
            yield return new ValidationResult($"bad {nameof(DateOfBirth)}",
                new[] { nameof(DateOfBirth) });
        if (DateOfMarriage is not null && DateOfMarriage > DateTime.Now)
            yield return new ValidationResult($"bad {nameof(DateOfMarriage)}",
                new[] { nameof(DateOfBirth), nameof(DateOfMarriage) });
    }
}
