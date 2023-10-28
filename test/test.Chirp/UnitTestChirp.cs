namespace test.Chirp;
using Microsoft.AspNetCore.Mvc.Testing;

public class UnitTestChirp
{

    
    /* These tests was used for unix timestamp conversion, we dont use this anymore 
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
    */
    public class TestAPI : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> _fixture;
    protected readonly HttpClient _client;

    public TestAPI(WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    [Fact]
    protected async void CanSeePublicTimeline()
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
    protected async void CanSeePrivateTimeline(string author)
    {
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains($"{author}'s Timeline", content);
    }

    [Theory]
    [InlineData("Helge", "Hello, BDSA students!")]
    [InlineData("Rasmus", "Hej, velkommen til kurset.")]
     protected async void Check_if_we_can_find_specfic_cheeps_on_users_timeline(string author, string cheep)
    {
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains(cheep, content);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    protected async void Check_if_cheeps_exists_on_different_pages(int page)
    {
        var response = await _client.GetAsync($"/?={page}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var errorMessage = "There are no cheeps so far.";

        Assert.DoesNotContain(errorMessage,content);
    }

    // [Theory]
    // [InlineData(1000)]
    // [InlineData(1001)]
    // protected async void Check_if_cheeps_does_not_exist_on_different_pages(int page)
    // {
    //     var response = await _client.GetAsync($"/?={page}");
    //     response.EnsureSuccessStatusCode();
    //     var content = await response.Content.ReadAsStringAsync();

    //     var errorMessage = "There are no cheeps so far.";

    //     Assert.Contains(errorMessage,content);
    // }
}
}