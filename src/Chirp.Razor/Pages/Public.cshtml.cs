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

    private readonly IFollowerListRepository _followRepo;


    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();

    public IEnumerable<CheepDto> AllCheeps { get; set; } = new List<CheepDto>();

    public IEnumerable<FollowDto> Followers { get; set; } = new List<FollowDto>();

    [BindProperty]
    [StringLength(160, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
    public string CheepText { get; set; }

    [BindProperty]
    public string Author { get; set;}

    public PublicModel(ICheepRepository service, IAuthorRepository authorRepo, IFollowerListRepository followRepo)
    {
        _service = service;
        _authorRepo = authorRepo;
        _followRepo = followRepo;
    }

    public async Task<ActionResult> OnGet()
    {
        return await ShowCheeps();
    }

    public async Task<ActionResult> OnPostCheep()
    {
        var author = User.Identity!.Name!;
        if (await _authorRepo.GetAuthorByName(author!) == null) {
            await _authorRepo.CreateNewAuthor(author, author);
        }

        if (!ModelState.IsValid) 
        {
            return await ShowCheeps();
        }
       
        return RedirectToPage();
    }

    public async Task<ActionResult> OnPostFollow() {
        var user = User.Identity!;

        if (user.Name == null) {
            return Redirect("/Identity/Account/Register");
        }

        await _followRepo.Follow(user.Name, Author);

        return RedirectToPage();
    }

    public async Task<ActionResult> OnPostUnfollow() {
        var user = User.Identity!;

        if (user.Name == null) {
            return Redirect("/Identity/Account/Register"); //Should never be possible.
        }

        await _followRepo.UnFollow(user.Name, Author);

        return RedirectToPage();
    }

    public async Task<bool> IsFollowed(string authorName) {
        var author = await _authorRepo.GetAuthorByName(authorName);

        return Followers.Where(f => f.AuthorId == author.AuthorId).FirstOrDefault() != null;
    
    }

    private async Task<ActionResult> ShowCheeps() {
        var t = Convert.ToInt32(Request.Query["page"]);
        if (t > 0) t -= 1;
        Cheeps = await _service.GetCheeps(t);
        AllCheeps = await _service.GetAllCheeps();
        Followers = await _followRepo.GetFollowers(User.Identity!.Name!);
        return Page();
    }
}
