﻿using People.ServiceContracts.DTOs;

namespace People.ServiceContracts.Interfaces
{
    //service interface
    public interface ICountriesService
    {
        /// <summary>
        /// method to add the country
        /// </summary>
        /// <param name="request">request containing added country name</param>
        /// <returns>created or modified country info</returns>
        Task<CountryResponse> AddCountry(AddCountryRequest request);
        Task<List<CountryResponse>> GetCountryList();
        Task<CountryResponse?> GetCountry(Guid? id);
    }
}
