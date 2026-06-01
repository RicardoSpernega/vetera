using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Backend.IntegrationTests;

public sealed class GamesEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GamesEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchEndpoint_ShouldReturnBadRequest_WhenNameIsEmpty()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/games/search?name=");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
