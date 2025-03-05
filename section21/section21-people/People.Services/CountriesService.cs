using CsvHelper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using People.Entities;
using People.ServiceContracts.DTOs;
using People.ServiceContracts.Interfaces;
using People.ServiceContracts.Repositories;
using System.Formats.Asn1;
using System.Globalization;

namespace People.Services;

public class CountriesService(ICountryRepository countryRepository) : ICountriesService
{
    public async Task<CountryResponse> AddCountry(AddCountryRequest request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (request.CountryName is null) throw new ArgumentException(nameof(request.CountryName));

        var countries = await countryRepository.Get(c => c.CountryName == request.CountryName);
        if (countries.Count > 0)
            throw new ArgumentException(nameof(request.CountryName));

        Country country = (Country)request;

        country.CountryId = Guid.NewGuid();
        await countryRepository.Add(country);

        return (CountryResponse)country;
    }

    public async Task<List<CountryResponse>> GetCountryList()
    {
        return (await countryRepository.GetAll()).Select(c => (CountryResponse)c).ToList();
    }

    public async Task<CountryResponse?> GetCountry(Guid? id)
    {
        if (id is null) return null;
        Country? country = (await countryRepository.Get(c => c.CountryId == id)).FirstOrDefault();
        if (country == null) return null;
        return (CountryResponse?)country;
    }

    public async Task<bool> ImportCountries(Stream stream)
    {
        stream.Position = 0;
        using (ExcelPackage package = new ExcelPackage(stream))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 1; i <= worksheet.Rows.Count(); i++)
            {
                string countryName = worksheet.Cells[i, 1].Value.ToString();
                if ((await countryRepository.Get(x => x.CountryName == countryName)).Count() > 0) continue;
                countryRepository.Add(new Country() { CountryName = countryName });
            }
        }

        return true;
    }
}
