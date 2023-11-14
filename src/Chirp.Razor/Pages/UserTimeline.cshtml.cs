using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure;
using EFCore;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IAuthorRepository _authorRepo;
    private readonly ICheepRepository _service;
    public IEnumerable<CheepDto> Cheeps { get; set; }
    public IEnumerable<CheepDto> AllCheeps { get; set; }
    public string CheepText { get; set; }



    public UserTimelineModel(ICheepRepository service, IAuthorRepository authorRepo)
    {
        _service = service;
        _authorRepo = authorRepo;

    }

    public async Task<ActionResult> OnGet(string author)
    {
        var t = Convert.ToInt32(Request.Query["page"]);
        if (t > 0) t -= 1;
        Cheeps = await _service.GetCheepsFromAuthor(author, t);
        AllCheeps = await _service.GetAllCheepsFromAuthor(author);
        return Page();
       
    }
        public async Task<ActionResult> OnPost() 
        {
        var author = User.Identity.Name;
        var user = await _authorRepo.GetAuthorByName(author);
        if (user == null) {
            var authorDto = new AuthorDto(author, author);
            await _authorRepo.CreateNewAuthor(authorDto);
        }

        string text = Request.Form["CheepText"];
        Console.WriteLine(text);
        DateTime timestamp = DateTime.Now;
        var cheep = new CheepDto(text, author, timestamp);
        await _service.CreateCheep(cheep);

        var t = Convert.ToInt32(Request.Query["page"]);
        if (t > 0) t -= 1;
        Cheeps = await _service.GetCheepsFromAuthor(author, t);
        AllCheeps = await _service.GetAllCheepsFromAuthor(author);
        
        return Page();
    }
}
