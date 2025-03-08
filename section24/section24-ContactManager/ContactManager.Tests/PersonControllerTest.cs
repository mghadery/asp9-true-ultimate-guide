using ContactManager.Core.DTOs;
using ContactManager.Core.Enums;
using ContactManager.Core.ServiceContracts;
using ContactManager.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Tests;

public class PersonControllerTest
{
    private readonly PersonController _controller;
    private readonly Mock<IPersonsService> _personsServiceMock;
    private readonly IPersonsService _personsService;
    private readonly Mock<ICountriesService> _countriesServiceMock;
    private readonly ICountriesService _countriesService;
    private readonly Bogus.Faker _stringFaker;
    private readonly Bogus.Faker<PersonResponse> _personRespFaker;


    public PersonControllerTest()
    {
        _personsServiceMock= new Mock<IPersonsService>();
        _personsService = _personsServiceMock.Object;

        _countriesServiceMock = new Mock<ICountriesService>();
        _countriesService = _countriesServiceMock.Object;

        _controller = new PersonController(_personsService, _countriesService);
        _stringFaker = new Bogus.Faker();
        _personRespFaker = new Bogus.Faker<PersonResponse>()
            .RuleFor(p => p.Address, f => f.Address.FullAddress())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.Gender, f => f.PickRandom<GenderOptions>().ToString())
            .RuleFor(p => p.PersonName, f => f.Name.FullName())
            .RuleFor(p => p.CountryId, f => f.Random.Guid());
    }

    [Fact]
    public async Task Index_ModelInValid_ReturnViewResult()
    {
        List<PersonResponse> personResponses = new()
        {
            _personRespFaker.Generate()
        };

        _personsServiceMock.Setup(x => x.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(personResponses);
        _personsServiceMock.Setup(x => x.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<bool>()))
        .Returns(personResponses);

        IActionResult result = await _controller.Index(_stringFaker.Random.String2(3), _stringFaker.Random.String2(3), _stringFaker.Random.String2(3), true);

        ViewResult vr = Assert.IsType<ViewResult>(result);
        var data = Assert.IsType<List<PersonResponse>>(vr.ViewData.Model);
        data.Should().BeEquivalentTo(personResponses);
    }
}
