namespace test.Chirp;
using Microsoft.AspNetCore.Mvc.Testing;

public class UnitTestChirp
{

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
    }
}