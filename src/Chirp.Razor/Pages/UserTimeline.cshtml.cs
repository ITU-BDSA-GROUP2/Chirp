using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure;
using EFCore;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _service;
    public IEnumerable<CheepDto> Cheeps { get; set; }
    public IEnumerable<CheepDto> AllCheeps { get; set; }


    public UserTimelineModel(ICheepRepository service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet(string author)
    {
        var t = Convert.ToInt32(Request.Query["page"]);
        if (t > 0) t -= 1;
        Cheeps = await _service.GetCheepsFromAuthor(author, t);
        AllCheeps = await _service.GetAllCheepsFromAuthor(author);
        return Page();
       
    }
}
