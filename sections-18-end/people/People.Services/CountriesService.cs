using People.Entities;
using People.ServiceContracts.DTOs;
using People.ServiceContracts.Interfaces;

namespace People.Services;

public class CountriesService : ICountriesService
{
    private List<Country> _countries = new();

    public CountriesService(bool init = true)
    {
        if (init)
        {
            _countries.AddRange(new List<Country>()
            {
                new(){
                    CountryId = Guid.Parse("1d0ab2da-19ca-474d-a6c9-6ed136ec4b8a"),
                    CountryName = "Colombia"
                },
                new(){
                    CountryId = Guid.Parse("1f1c5d35-0d05-46f0-84e2-040375c59a86"),
                    CountryName = "Portugal"
                },
                new(){
                    CountryId = Guid.Parse("d358077f-bcce-408b-8c47-675fec6d4ab7"),
                    CountryName = "China"
                }
            });
        }
    }

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
