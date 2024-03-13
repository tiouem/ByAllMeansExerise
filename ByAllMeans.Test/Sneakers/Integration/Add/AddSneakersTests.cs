using ByAllMeans.Api.Features.Sneakers.Add;
using Testcontainers.PostgreSql;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace ByAllMeans.Test.Sneakers.Integration;

public class AddSneakersTests
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .Build();


    [Test]
    public async Task AddedSneakers_CanBeFoundById()
    {
        await _postgres.StartAsync();
        await using var appFactory = new CustomWebApplicationFactory(_postgres);
        var client = appFactory.CreateClient(new()
        {
            BaseAddress = new Uri("http://localhost:5225/api/")
        });

        var createRequest = await File.ReadAllTextAsync("./Sneakers/Integration/Add/Json/AddRequest.json");
        var createRequestParsed =
            JsonSerializer.Deserialize<AddSneakers.Command>(createRequest,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        var createResponse = await client.PostAsJsonAsync("Sneakers", createRequestParsed);
        var createResponseContent = JToken.Parse(await createResponse.Content.ReadAsStringAsync());
        var createdGuid = (Guid)createResponseContent["createdSneakersId"].First;
        
        var getByIdResponse = await client.GetAsync($"Sneakers?id={createdGuid}");
        var getByIdResponseParsed = JToken.Parse(await getByIdResponse.Content.ReadAsStringAsync());

        var expected = await File.ReadAllTextAsync("./Sneakers/Integration/Add/Json/GetByIdResponse.json");
        var expectedParsed = JToken.Parse(expected);
        expectedParsed["sneakers"]["id"] = createdGuid;
        
        getByIdResponseParsed.Should().BeEquivalentTo(expectedParsed);
        await _postgres.StopAsync();
    }
}