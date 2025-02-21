using Microsoft.EntityFrameworkCore;
using People.Entities;
using People.ServiceContracts.DTOs;
using People.ServiceContracts.Interfaces;

namespace People.Services;

public class CountriesService(AppDbContext dbContext) : ICountriesService
{
    public async Task<CountryResponse> AddCountry(AddCountryRequest request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (request.CountryName is null) throw new ArgumentException(nameof(request.CountryName));

        if (await dbContext.Countries.AnyAsync(x => x.CountryName == request.CountryName))
            throw new ArgumentException(nameof(request.CountryName));

        Country country = (Country)request;

        country.CountryId = Guid.NewGuid();
        await dbContext.Countries.AddAsync(country);
        await dbContext.SaveChangesAsync();

        return (CountryResponse)country;
    }

    public async Task<List<CountryResponse>> GetCountryList()
    {
        return await dbContext.Countries.Select(x => (CountryResponse)x).ToListAsync();
    }

    public async Task<CountryResponse?> GetCountry(Guid? id)
    {
        Country? country = await dbContext.Countries.FirstOrDefaultAsync(x => x.CountryId == id);
        if (country == null) return null;
        return (CountryResponse?)country;
    }
}
