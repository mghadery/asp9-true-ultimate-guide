using ContactManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Core.DTOs
{
    /// <summary>
    /// Service return type
    /// </summary>
    //public class CountryResponse
    public record CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; init; }
        public static explicit operator CountryResponse(Country country)
        {
            return new CountryResponse() { CountryId = country.CountryId, CountryName = country.CountryName };
        }
        //public override bool Equals(object? obj)
        //{
        //    if (obj is not CountryResponse countryResponse) return false;
        //    return (CountryId == countryResponse.CountryId && CountryName == countryResponse.CountryName);
        //}

        //public override int GetHashCode()
        //{
        //    return (CountryId, CountryName).GetHashCode();
        //}
    }
}
