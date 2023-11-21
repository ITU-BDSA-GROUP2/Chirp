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
    public async Task HomepageHasChirpAsTitle()
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

}