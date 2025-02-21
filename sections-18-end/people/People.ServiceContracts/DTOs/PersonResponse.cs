using People.Entities;
using People.ServiceContracts.Enums;

namespace People.ServiceContracts.DTOs;

/// <summary>
/// DTO for add country
/// </summary>
public record PersonResponse
{
    public Guid PersonId { get; init; }
    public string? PersonName { get; init; }
    public string? Email { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string? Gender { get; init; }
    public string? Address { get; init; }
    public bool ReceiveNewsLetters { get; init; }
    public Guid? CountryId { get; init; }
    public string? Country { get; set; }

    public static explicit operator PersonResponse(Person person)
    {
        return new PersonResponse()
        {
            PersonId = person.PersonId,
            Address = person.Address,
            CountryId = person.CountryId,
            Country = person.Country.CountryName,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            Email = person.Email,
            PersonName = person.PersonName,
            ReceiveNewsLetters = person.ReceiveNewsLetters
        };
    }
}
