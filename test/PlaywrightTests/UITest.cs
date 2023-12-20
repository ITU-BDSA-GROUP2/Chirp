using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    [Test]
    public async Task HomePageHasChirpAsTitle()
    {
        await Page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Chirp!"));
    }

    [Test]
    //This test, tests if our login button leads to our login site
    public async Task LoginButtonDirectsToLoginPage()
    {
        await Page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

        var login = Page.GetByRole(AriaRole.Link, new() { Name = "Login" });

        await Expect(login).ToHaveAttributeAsync("href", "/Identity/Account/Login");

        await login.ClickAsync();

        await Expect(Page).ToHaveTitleAsync(new Regex("Log in"));
    }
    [Test]
    public async Task RegisterButtonDirectsToRegisterPage()
    {
        await Page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

        var register = Page.GetByRole(AriaRole.Link, new() { Name = "Register" });

        await Expect(register).ToHaveAttributeAsync("href", "/Identity/Account/Register");

        await register.ClickAsync();

        await Expect(Page).ToHaveTitleAsync(new Regex("Register"));
    }
    [Test]
    public async Task TestIfYouCanRegister()
    {

        await Page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();

        await Page.GetByPlaceholder("name@example.com").ClickAsync();

        await Page.GetByPlaceholder("name@example.com").FillAsync("cool1@gmail.com");

        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();

        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Dqa68dfs!");

        await Page.GetByLabel("Confirm Password").ClickAsync();

        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();

        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Daq68dfs!");

        await Page.GetByLabel("Confirm Password").ClickAsync();

        await Page.GetByLabel("Confirm Password").FillAsync("Daq99dns!");

        await Page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");



    }
    [Test]
    // When we click on public timeline we can actually see public timeline
    public async Task PublicTimelineDirectsToHomePage()
    {
        await Page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

        var publicTimeline = Page.GetByRole(AriaRole.Link, new() { Name = "Public timeline" });

        await publicTimeline.ClickAsync();

        await Expect(Page.Locator("h2")).ToHaveTextAsync("Public Timeline");
    }
    [Test]
    // When we click on public timeline we can actually see public timeline
    public async Task WhenClickUserViewTimeline()
    {
        await Page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Follow Starbuck now is what we hear the worst. — 08/01/2023 " }).GetByRole(AriaRole.Link).ClickAsync();

        await Expect(Page.Locator("h2")).ToHaveTextAsync("Jacqualine Gilcoine's Timeline");


    }
    [Test]
    public async Task UserCanLoginAndFollowAnotherUser()
    {
        await Page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

        await Page.GetByPlaceholder("Name@example.com").ClickAsync();

        await Page.GetByPlaceholder("Name@example.com").FillAsync("cool1@gmail.com");

        await Page.GetByPlaceholder("Password").ClickAsync();

        await Page.GetByPlaceholder("Password").FillAsync("Dqa68dfs!");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Follow Starbuck now is what we hear the worst. — 08/01/2023 " }).GetByRole(AriaRole.Button).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "My timeline" }).ClickAsync();

        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Follow Starbuck now is what we hear the worst. — 08/01/2023 " }).GetByRole(AriaRole.Link).ClickAsync();

        await Expect(Page.Locator("h2")).ToHaveTextAsync("Jacqualine Gilcoine's Timeline");
    }
    [Test]
    public async Task UserCanLoginAndUnfollowAnotherUser()
    {
        await Page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

        await Page.GetByPlaceholder("Name@example.com").ClickAsync();

        await Page.GetByPlaceholder("Name@example.com").FillAsync("cool1@gmail.com");

        await Page.GetByPlaceholder("Password").ClickAsync();

        await Page.GetByPlaceholder("Password").FillAsync("Dqa68dfs!");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Unfollow Starbuck now is what we hear the worst. — 08/01/2023 " }).GetByRole(AriaRole.Button).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "My timeline" }).ClickAsync();

        await Expect(Page.Locator("body")).Not.ToContainTextAsync("Starbuck now is what we hear the worst");


    }
    [Test]
    public async Task UserCannotFollowIfNotLoggedIn()
    {
        await Page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

        await Page.Locator("li").Filter(new() { HasText = "o@o.com Follow Yeet — 11/28/2023 11:26:39 Like 0" }).GetByRole(AriaRole.Button).ClickAsync();

        await Expect(Page.Locator("body")).Not.ToContainTextAsync("o@o.com Unfollow Yeet — 11/28/2023 11:26:39 Like 0");
    }

}