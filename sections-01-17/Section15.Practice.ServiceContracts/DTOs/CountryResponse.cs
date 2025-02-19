using Section15.Practice.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Section15.Practice.ServiceContracts.DTOs
{
    /// <summary>
    /// Service return type
    /// </summary>
    //public class CountryResponse
    public record CountryResponse
    {
        public Guid CountryId { get; init; }
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
