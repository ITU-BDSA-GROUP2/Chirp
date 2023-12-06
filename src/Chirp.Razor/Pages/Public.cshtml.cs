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

    private readonly ILikeRepository _likeRepo;



    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();

    public IEnumerable<CheepDto> AllCheeps { get; set; } = new List<CheepDto>();

    public IEnumerable<FollowDto> Followers { get; set; } = new List<FollowDto>();

    [BindProperty]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [StringLength(160)]
    public string CheepText { get; set; } = "";

    [BindProperty]
    public string Author { get; set;} = "";
    [BindProperty]
    public int CheepId { get; set; } = 0;

    public PublicModel(ICheepRepository service, IAuthorRepository authorRepo, IFollowerListRepository followRepo, ILikeRepository likeRepo)
    {
        _service = service;
        _authorRepo = authorRepo;
        _followRepo = followRepo;
        _likeRepo = likeRepo;

    }

    public async Task<ActionResult> OnGet()
    {
        return await ShowCheeps();
    }

    public async Task<ActionResult> OnPostCheep()
    {
        var author = User.Identity!.Name!;
        if (await _authorRepo.GetAuthorByName(author!) == null) 
        {
            await _authorRepo.CreateNewAuthor(author, author);
        }
        if (CheepText.Length > 0) 
        {
            await _service.CreateCheep(CheepText, author, DateTime.UtcNow);
        } 
        else
        {
            ModelState.AddModelError("ErrorMessageLength", "Cheep must not be blank");
            return await ShowCheeps();

        }
        return RedirectToPage();
    }

    public async Task<ActionResult> OnPostFollow() 
    {
        var user = User.Identity!;

        if (user.Name == null) 
        {
            return Redirect("/Identity/Account/Register");
        }

        await _followRepo.Follow(user.Name, Author);

        return RedirectToPage();
    }

    public async Task<ActionResult> OnPostUnfollow() 
    {
        var user = User.Identity!;

        if (user.Name == null) 
        {
            return Redirect("/Identity/Account/Register"); //Should never be possible.
        }

        await _followRepo.UnFollow(user.Name, Author);

        return RedirectToPage();
    }

    public async Task<ActionResult> OnPostLike()
    {
        var user = User.Identity!;

        if (user.Name == null) 
        {
            return Redirect("/Identity/Account/Register"); //Should never be possible.
        }

        if (CheepId == 0) {
            return RedirectToPage();

        } 
        await _likeRepo.Like(CheepId, user.Name);
        return RedirectToPage();
    }

    public async Task<bool> IsFollowed(string authorName) 
    {
        var author = await _authorRepo.GetAuthorByName(authorName);
        return Followers.Where(f => f.AuthorId == author.AuthorId).FirstOrDefault() != null;
    
    }

    private async Task<ActionResult> ShowCheeps() 
    {
        var t = Convert.ToInt32(Request.Query["page"]);
        if (t > 0) t -= 1;
        Cheeps = await _service.GetCheeps(t);
        AllCheeps = await _service.GetAllCheeps();
        Followers = await _followRepo.GetFollowers(User.Identity!.Name!);
        return Page();
    }
}
