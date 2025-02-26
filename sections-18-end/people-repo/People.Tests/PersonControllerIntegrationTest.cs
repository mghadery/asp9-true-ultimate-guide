using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace People.Tests;

public class PersonControllerIntegrationTest:IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    public PersonControllerIntegrationTest(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Index_ToReturnView()
    {
        var res = await _httpClient.GetAsync("/persons/index");

        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var body = await res.Content.ReadAsStringAsync();
        HtmlDocument html = new HtmlDocument();
        html.LoadHtml(body);
        var document = html.DocumentNode;
        document.QuerySelectorAll("table").Should().HaveCountGreaterThan(0);
    }
}
