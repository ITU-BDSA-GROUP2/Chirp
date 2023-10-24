using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure;
using EFCore;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    public readonly ICheepRepository _service;

    public IEnumerable<CheepDto> Cheeps { get; set; }

    public IEnumerable<CheepDto> AllCheeps { get; set; }

    public PublicModel(ICheepRepository service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet()
    {
        var t = Convert.ToInt32(Request.Query["page"]);
        
        if (t > 0) t -= 1;
        Cheeps = await _service.GetCheeps(t);
        AllCheeps = await _service.GetAllCheeps();
        return Page();
    }
}
