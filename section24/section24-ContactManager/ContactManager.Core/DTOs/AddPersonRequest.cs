using ContactManager.Core.Domain.Entities;
using ContactManager.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Core.DTOs;

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
            CountryId = addPersonRequest.CountryId is null ? Guid.NewGuid() : addPersonRequest.CountryId.Value,
            Country = null,
            DateOfBirth = addPersonRequest.DateOfBirth,
            Gender = addPersonRequest.GenderOptions.ToString(),
            Email = addPersonRequest.Email,
            PersonName = addPersonRequest.PersonName,
            ReceiveNewsLetters = addPersonRequest.ReceiveNewsLetters
        };
    }
}
