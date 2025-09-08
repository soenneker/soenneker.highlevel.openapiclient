using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.HighLevel.OpenApiClient.Tests;

[Collection("Collection")]
public sealed class HighLevelOpenApiClientTests : FixturedUnitTest
{
    public HighLevelOpenApiClientTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public void Default()
    {

    }
}
