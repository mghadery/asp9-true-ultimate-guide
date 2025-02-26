using People.Entities;
using People.ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace People.ServiceContracts.DTOs;

/// <summary>
/// DTO for add country
/// </summary>
public record AddPersonRequest
{
    [Required(ErrorMessage = "Name is blank")]
    public string? PersonName { get; init; }
    [Required(ErrorMessage = "Email is blank")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string? Email { get; init; }
    [Required(ErrorMessage = "DateOfBirth is blank")]
    public DateTime DateOfBirth { get; init; }
    public GenderOptions GenderOptions { get; init; }
    public string? Address { get; init; }
    public bool ReceiveNewsLetters { get; init; }
    public Guid? CountryId { get; set; }

    public static explicit operator Person(AddPersonRequest addPersonRequest)
    {
        return new Person()
        {
            Address = addPersonRequest.Address,
            CountryId = addPersonRequest.CountryId.Value,
            DateOfBirth = addPersonRequest.DateOfBirth,
            Gender = addPersonRequest.GenderOptions.ToString(),
            Email = addPersonRequest.Email,
            PersonName = addPersonRequest.PersonName,
            ReceiveNewsLetters = addPersonRequest.ReceiveNewsLetters
        };
    }
}
