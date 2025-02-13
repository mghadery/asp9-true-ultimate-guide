using Section15.Practice.ServiceContracts.DTOs;
using Section15.Practice.ServiceContracts.Interfaces;
using Section15.Practice.Services;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace Section15.Practice.Tests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;

    public CountriesServiceTest(ITestOutputHelper testOutputHelper)
    {
        _countriesService = new CountriesService(false);
        _testOutputHelper = testOutputHelper;
    }

    #region AddCountry
    //when AddCountryRequest is null => ArgumentNullException
    [Fact]
    public void AddCountry_NullCountry()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = null;

        //Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            //Act
            _countriesService.AddCountry(addCountryRequest);
        });
    }


    //when AddCountryRequest.CountryName is null => ArgumentException
    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = null };

        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Act
            _countriesService.AddCountry(addCountryRequest);
        });
    }

    //when AddCountryRequest.CountryName is duplcate => ArgumentException
    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
        //Arrange
        AddCountryRequest? addCountryRequest1 = new() { CountryName = "Iran" };
        AddCountryRequest? addCountryRequest2 = new() { CountryName = "Iran" };

        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Act
            _countriesService.AddCountry(addCountryRequest1);
            _countriesService.AddCountry(addCountryRequest2);
        });
    }

    //when AddCountryRequest is OK => created country
    [Fact]
    public void AddCountry_ProperCountryName()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };

        //Act
        CountryResponse countryResponse = _countriesService.AddCountry(addCountryRequest);
        var countryResponses = _countriesService.GetCountryList();

        //Assert
        Assert.True(countryResponse.CountryName == addCountryRequest.CountryName &&
            countryResponse.CountryId != Guid.Empty
            );

        Assert.Contains(countryResponse, countryResponses);  //, new CountryResponseComparer());
    }
    #endregion
    #region GetCountryList
    [Fact]
    public void GetCountryList_Empty()
    {
        //Arrange

        //Act
        var actualList = _countriesService.GetCountryList();

        //Assert
        Assert.Empty(actualList);
    }

    [Fact]
    public void GetCountryList_Data()
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
            countryResponses.Add(_countriesService.AddCountry(addCountryRequest));
        }
        var actualList = _countriesService.GetCountryList();

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
    public void GetCountry_Null()
    {
        //Arrange
        Guid? guid = null;

        //Act
        var actual = _countriesService.GetCountry(guid);

        //Assert
        Assert.Null(actual);
    }
    [Fact]
    public void GetCountry_WrongId()
    {
        //Arrange
        Guid guid = Guid.NewGuid();

        //Act
        var actual = _countriesService.GetCountry(guid);

        //Assert
        Assert.Null(actual);
    }
    [Fact]
    public void GetCountry_Id()
    {
        //Arrange
        AddCountryRequest addCountryRequest = new AddCountryRequest() { CountryName = "Iran" };
        CountryResponse countryResponse = _countriesService.AddCountry(addCountryRequest);

        //Act
        var actual = _countriesService.GetCountry(countryResponse.CountryId);

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