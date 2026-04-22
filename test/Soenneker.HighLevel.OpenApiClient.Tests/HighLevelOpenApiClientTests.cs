using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Tests.Attributes.Local;
using Soenneker.HighLevel.OpenApiClient.Models;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.HighLevel.OpenApiClient.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class HighLevelOpenApiClientTests : HostedUnitTest
{
    private readonly IConfiguration _config;

    public HighLevelOpenApiClientTests(Host host) : base(host)
    {
        _config = Resolve<IConfiguration>();
    }

    [Test]
    public void Default()
    {
    }

    [LocalOnly]
    public async Task UpsertContact_WithRequiredFields_ShouldSucceed()
    {
        // Arrange
        string apiKey = _config["HighLevel:ApiKey"]!;
        string locationId = _config["HighLevel:LocationId"]!;
        string version = _config["HighLevel:Version"] ?? "2021-07-28";

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(locationId))
        {
            // Skip test if credentials not configured
            return;
        }

        var authProvider = new AnonymousAuthenticationProvider();
        var requestAdapter = new HttpClientRequestAdapter(authProvider);
        var client = new HighLevelOpenApiClient(requestAdapter);

        var upsertContactDto = new UpsertContactDto
        {
            LocationId = locationId,
            Email = $"test_{Guid.NewGuid()}@example.com",
            FirstName = "Test",
            LastName = "Contact",
            Phone = "+15551234567"
        };

        // Act
        UpsertContactsSuccessfulResponseDto? response = await client.Contacts.Upsert.PostAsync(upsertContactDto, config =>
        {
            config.Headers.Add("Authorization", $"Bearer {apiKey}");
            config.Headers.Add("Version", version);
            config.Headers.Add("Content-Type", "application/json");
            config.Headers.Add("Accept", "application/json");
        });

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Contact);
        Assert.NotNull(response.Contact.Id);
    }
}
