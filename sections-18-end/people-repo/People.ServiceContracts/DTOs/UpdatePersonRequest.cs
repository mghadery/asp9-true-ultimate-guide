using People.Entities;
using People.ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace People.ServiceContracts.DTOs;

/// <summary>
/// DTO for add country
/// </summary>
public record UpdatePersonRequest
{
    [Required(ErrorMessage = "Id is blank")]
    public Guid PersonId { get; set; }

    [Required(ErrorMessage = "Name is blank")]
    public string? PersonName { get; set; }
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string? Email { get; init; }
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; init; }
    public GenderOptions GenderOptions { get; init; }
    public string? Address { get; init; }
    public bool ReceiveNewsLetters { get; init; }
    public Guid? CountryId { get; init; }

    public static explicit operator Person(UpdatePersonRequest updatePersonRequest)
    {
        return new Person()
        {
            PersonId = updatePersonRequest.PersonId,
            Address = updatePersonRequest.Address,
            CountryId = updatePersonRequest.CountryId.Value,
            DateOfBirth = updatePersonRequest.DateOfBirth,
            Gender = updatePersonRequest.GenderOptions.ToString(),
            Email = updatePersonRequest.Email,
            PersonName = updatePersonRequest.PersonName,
            ReceiveNewsLetters = updatePersonRequest.ReceiveNewsLetters
        };
    }

    public static explicit operator UpdatePersonRequest(PersonResponse personResponse)
    {
        return new UpdatePersonRequest()
        {
            PersonId = personResponse.PersonId,
            Address = personResponse.Address,
            CountryId = personResponse.CountryId,
            DateOfBirth = personResponse.DateOfBirth,
            GenderOptions = (GenderOptions)Enum.Parse(typeof(GenderOptions), personResponse.Gender, true),
            Email = personResponse.Email,
            PersonName = personResponse.PersonName,
            ReceiveNewsLetters = personResponse.ReceiveNewsLetters
        };
    }
}
