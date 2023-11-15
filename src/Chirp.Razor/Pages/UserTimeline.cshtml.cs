using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure;
using EFCore;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IAuthorRepository _authorRepo;
    private readonly ICheepRepository _service;
    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<CheepDto> AllCheeps { get; set; } = new List<CheepDto>();
    public string? CheepText { get; set; }



    public UserTimelineModel(ICheepRepository service, IAuthorRepository authorRepo)
    {
        _service = service;
        _authorRepo = authorRepo;

    }

    public async Task<ActionResult> OnGet(string author)
    {
        return await showCheeps(author);
       
    }
        public async Task<ActionResult> OnPost() 
        {
            var author = User.Identity!.Name!;
        if (await _authorRepo.GetAuthorByName(author!) == null) {
            var authorDto = new AuthorDto(author, author);
            await _authorRepo.CreateNewAuthor(authorDto);
        }

        string text = Request.Form["CheepText"]!;
        var cheep = new CheepDto(text, author, DateTime.Now);
        await _service.CreateCheep(cheep);

        return await showCheeps(author);
    }

    private async Task<ActionResult> showCheeps(string author) {
        var t = Convert.ToInt32(Request.Query["page"]);
        if (t > 0) t -= 1;
        Cheeps = await _service.GetCheepsFromAuthor(author, t);
        AllCheeps = await _service.GetAllCheepsFromAuthor(author);
        return Page();
    }
}
