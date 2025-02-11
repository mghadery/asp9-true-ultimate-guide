using Section15.Practice.Entities;
using Section15.Practice.ServiceContracts.DTOs;
using Section15.Practice.ServiceContracts.Interfaces;

namespace Section15.Practice.Services;

public class CountriesService : ICountriesService
{
    private List<Country> _countries = new();
    public CountryResponse AddCountry(AddCountryRequest request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (request.CountryName is null) throw new ArgumentException(nameof(request.CountryName));

        if (_countries.Any(x => x.CountryName == request.CountryName))
            throw new ArgumentException(nameof(request.CountryName));

        Country country = (Country)request;

        country.CountryId = Guid.NewGuid();
        _countries.Add(country);

        return (CountryResponse)country;
    }

    public List<CountryResponse> GetCountryList()
    {
        return _countries.Select(x => (CountryResponse)x).ToList();
    }

    public CountryResponse? GetCountry(Guid? id)
    {
        Country? country = _countries.FirstOrDefault(x => x.CountryId == id);
        if (country == null) return null;
        return (CountryResponse?)country;
    }
}
