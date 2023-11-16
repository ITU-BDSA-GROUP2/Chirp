using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure;
using EFCore;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _service;

    private readonly IAuthorRepository _authorRepo;

    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();

    public IEnumerable<CheepDto> AllCheeps { get; set; } = new List<CheepDto>();

    [StringLength(240)]
    public string CheepText { get; set; } = "";

    public PublicModel(ICheepRepository service, IAuthorRepository authorRepo)
    {
        _service = service;
        _authorRepo = authorRepo;
    }

    public async Task<ActionResult> OnGet()
    {
        var t = Convert.ToInt32(Request.Query["page"]);
        if (t > 0) t -= 1;
        Cheeps = await _service.GetCheeps(t);
        AllCheeps = await _service.GetAllCheeps();
        return await showCheeps();
    }

    public async Task<ActionResult> OnPost()
    {
            var author = User.Identity!.Name!;
        if (await _authorRepo.GetAuthorByName(author!) == null) {
            var authorDto = new AuthorDto(author, author);
            await _authorRepo.CreateNewAuthor(authorDto);
        }

        string text = Request.Form["CheepText"]!;
        if (text.Length > 0 && text.Length <= 240) {
             var cheep = new CheepDto(text, author, DateTime.UtcNow);
            await _service.CreateCheep(cheep);
        } else {
            ModelState.AddModelError("ErrorMessageLength", "You can betweem 1 and 240 characters");
            //return Page();
            return await showCheeps();
        }
        Console.WriteLine(text);
       

        return await showCheeps();
    }

    private async Task<ActionResult> showCheeps() {
        var t = Convert.ToInt32(Request.Query["page"]);
        if (t > 0) t -= 1;
        Cheeps = await _service.GetCheeps(t);
        AllCheeps = await _service.GetAllCheeps();
        return Page();
    }
}
