using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

class Program
{
    public static async Task Main()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

        await page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();

        await page.GetByPlaceholder("name@example.com").ClickAsync();

        await page.GetByPlaceholder("name@example.com").FillAsync("cool@gmail.com");

        await page.GetByLabel("Password", new() { Exact = true }).ClickAsync();

        await page.GetByLabel("Password", new() { Exact = true }).FillAsync("Dqa68dfs!");

        await page.GetByLabel("Confirm Password").ClickAsync();

        await page.GetByLabel("Password", new() { Exact = true }).ClickAsync();

        await page.GetByLabel("Password", new() { Exact = true }).FillAsync("Daq68dfs!");

        await page.GetByLabel("Confirm Password").ClickAsync();

        await page.GetByLabel("Confirm Password").FillAsync("Daq99dns!");

        await page.GotoAsync("https://bdsagroup2chirprazor.azurewebsites.net/");

    }
}
