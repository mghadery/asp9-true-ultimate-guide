using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using People.Entities;
using People.ServiceContracts.DTOs;
using People.ServiceContracts.Interfaces;
using People.Services;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace People.Tests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;

    public CountriesServiceTest(ITestOutputHelper testOutputHelper)
    {
        //var countries = new List<Country>();
        //new List<Country>() {
        // new Country() { CountryId = Guid.NewGuid() , CountryName="Iran"},
        // new Country() { CountryId = Guid.NewGuid(), CountryName="Italy" } };
        //var appDbContext = Create.MockedDbContextFor<AppDbContext>(new DbContextOptionsBuilder().Options);
        var appDbContext = Create.MockedDbContextFor<AppDbContext>();

        _countriesService = new CountriesService(appDbContext);
        _testOutputHelper = testOutputHelper;
    }

    #region AddCountry
    //when AddCountryRequest is null => ArgumentNullException
    [Fact]
    public async Task AddCountry_NullCountry()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = null;

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            //Act
            await _countriesService.AddCountry(addCountryRequest);
        });
    }


    //when AddCountryRequest.CountryName is null => ArgumentException
    [Fact]
    public async Task AddCountry_CountryNameIsNull()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = null };

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            //Act
            await _countriesService.AddCountry(addCountryRequest);
        });
    }

    //when AddCountryRequest.CountryName is duplcate => ArgumentException
    [Fact]
    public async Task AddCountry_DuplicateCountryName()
    {
        //Arrange
        AddCountryRequest? addCountryRequest1 = new() { CountryName = "Iran" };
        AddCountryRequest? addCountryRequest2 = new() { CountryName = "Iran" };

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            //Act
            await _countriesService.AddCountry(addCountryRequest1);
            await _countriesService.AddCountry(addCountryRequest2);
        });
    }

    //when AddCountryRequest is OK => created country
    [Fact]
    public async Task AddCountry_ProperCountryName()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };

        //Act
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);
        var countryResponses = await _countriesService.GetCountryList();

        //Assert
        Assert.True(countryResponse.CountryName == addCountryRequest.CountryName &&
            countryResponse.CountryId != Guid.Empty
            );

        Assert.Contains(countryResponse, countryResponses);  //, new CountryResponseComparer());
    }
    #endregion
    #region GetCountryList
    [Fact]
    public async Task GetCountryList_Empty()
    {
        //Arrange

        //Act
        var actualList = await _countriesService.GetCountryList();

        //Assert
        Assert.Empty(actualList);
    }

    [Fact]
    public async Task GetCountryList_Data()
    {
        //Arrange
        List<AddCountryRequest> addCountryRequests = new List<AddCountryRequest>()
        {
            new AddCountryRequest(){ CountryName="Iran"},
            new AddCountryRequest(){ CountryName="India"},
            new AddCountryRequest(){ CountryName="Turkey"}
        };
        List<CountryResponse> countryResponses = new List<CountryResponse>();


        //Act
        foreach (var addCountryRequest in addCountryRequests)
        {
            countryResponses.Add(await _countriesService.AddCountry(addCountryRequest));
        }
        var actualList = await _countriesService.GetCountryList();

        //Expected
        _testOutputHelper.WriteLine("Expected:");
        foreach (var item in countryResponses)
        {
            _testOutputHelper.WriteLine(item.ToString());
        }
        //Actual
        _testOutputHelper.WriteLine("Actual:");
        foreach (var item in actualList)
        {
            _testOutputHelper.WriteLine(item.ToString());
        }

        //Assert
        foreach (var countryResponse in countryResponses)
        {
            Assert.Contains(countryResponse, actualList, new CountryResponseComparer());
        }
    }
    #endregion
    #region GetCountry
    [Fact]
    public async Task GetCountry_Null()
    {
        //Arrange
        Guid? guid = null;

        //Act
        var actual = await _countriesService.GetCountry(guid);

        //Assert
        Assert.Null(actual);
    }
    [Fact]
    public async Task GetCountry_WrongId()
    {
        //Arrange
        Guid guid = Guid.NewGuid();

        //Act
        var actual = await _countriesService.GetCountry(guid);

        //Assert
        Assert.Null(actual);
    }
    [Fact]
    public async Task GetCountry_Id()
    {
        //Arrange
        AddCountryRequest addCountryRequest = new AddCountryRequest() { CountryName = "Iran" };
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);

        //Act
        var actual = await _countriesService.GetCountry(countryResponse.CountryId);

        //Assert
        Assert.Equal(countryResponse, actual);
    }
    #endregion
}

public class CountryResponseComparer : IEqualityComparer<CountryResponse>
{
    public bool Equals(CountryResponse? x, CountryResponse? y)
    {
        return (x?.CountryName == y?.CountryName && x?.CountryId == y?.CountryId);
    }

    public int GetHashCode([DisallowNull] CountryResponse obj)
    {
        return (obj.CountryName, obj.CountryId).GetHashCode();
    }
}