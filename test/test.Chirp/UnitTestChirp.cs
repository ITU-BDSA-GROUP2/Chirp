namespace test.Chirp;
using Microsoft.AspNetCore.Mvc.Testing;

public class UnitTestChirp
{
    [Theory]
    [InlineData(1690891760, "08/01/23 12:09:20")]
    [InlineData(967329900, "08/26/00 22:45:00")]
    [InlineData(937925665, "09/21/99 14:54:25")]
    [InlineData(29851932, "12/12/70 12:12:12")]
    [InlineData(1049328015, "04/03/03 0:00:15")]
    public void epoch2String_Using_Different_times_Correct_Times(double unixTimeStamp, string time) {
        //Act
        var value = CheepService.UnixTimeStampToDateTimeString(unixTimeStamp);

        //Assert
        Assert.Equal(value, time);
        
    }

    [Theory]
    [InlineData(29851932, "08/01/2023 12:09:20")]
    [InlineData(965329900, "27/08/2000 12:45:00")]
    [InlineData(930920660, "12/09/1999 14:54:25")]
    [InlineData(1690891760, "12/12/1970 12:12:12")]
    [InlineData(1000000000, "03/04/2003 00:00:15")]
    public void epoch2String_Using_Different_times_Wrong_Times(double unixTimeStamp, string time) {
        // Act
        var value = CheepService.UnixTimeStampToDateTimeString(unixTimeStamp);

        // Assert
        Assert.NotEqual(time,value);
    }
    public class TestAPI : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public TestAPI(WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    [Fact]
    public async void CanSeePublicTimeline()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains("Public Timeline", content);
    }

    [Theory]
    [InlineData("Helge")]
    [InlineData("Rasmus")]
    public async void CanSeePrivateTimeline(string author)
    {
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains($"{author}'s Timeline", content);
    }
}
}