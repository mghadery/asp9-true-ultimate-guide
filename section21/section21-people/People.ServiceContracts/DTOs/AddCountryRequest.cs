using People.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.ServiceContracts.DTOs;

/// <summary>
/// DTO for add country
/// </summary>
public class AddCountryRequest
{
    public string? CountryName { get; set; }

    public static explicit operator Country(AddCountryRequest addCountryRequest)
    {
        return new Country() { CountryName = addCountryRequest.CountryName };
    }
}
