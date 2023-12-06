using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure;
using EFCore;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IAuthorRepository _authorRepo;
    private readonly ICheepRepository _service;
    private readonly IFollowerListRepository _followRepo;
    private readonly ILikeRepository _likeRepo;


    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<CheepDto> AllCheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<FollowDto> Followers { get; set; } = new List<FollowDto>();

    [BindProperty]
    public string Author { get; set;} = "";

    [BindProperty]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [StringLength(160)]
    public string CheepText { get; set; } = "";

    [BindProperty]
    public int CheepId { get; set; } = 0;

    public UserTimelineModel(ICheepRepository service, IAuthorRepository authorRepo, IFollowerListRepository followRepo, ILikeRepository likeRepo)
    {
        _service = service;
        _authorRepo = authorRepo;
        _followRepo = followRepo;
        _likeRepo = likeRepo;
        
    }

    public async Task<ActionResult> OnGet(string author)
    {
        return await ShowCheeps(author);
       
    }
    public async Task<ActionResult> OnPostCheep(string authorPage)
    {
        var author = User.Identity!.Name!;
        if (await _authorRepo.GetAuthorByName(author!) == null) 
        {
            await _authorRepo.CreateNewAuthor(author, author);
        }
        if (CheepText.Length > 0) 
        {
            await _service.CreateCheep(CheepText, author, DateTime.UtcNow);
        } else {
            ModelState.AddModelError("ErrorMessageLength", "Cheep must not be blank");
            return await ShowCheeps(authorPage);
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
        return Followers.Where(f => f.AuthorId == author!.AuthorId).FirstOrDefault() != null;
    }

    public async Task<bool> IsLiked(int id, string authorName)
    {
        if (authorName == null) {
            return false;
        }
        return await _likeRepo.IsLiked(id, authorName);
    }

    public async Task<string> GetImageUrl(string authorName) 
    {
        return await _authorRepo.GetAuthorImageUrl(authorName);
    }

    private async Task<ActionResult> ShowCheeps(string author) 
    {
        var t = Convert.ToInt32(Request.Query["page"]);
        if (t > 0) t -= 1;
        if (author.Equals(User.Identity?.Name!)) 
        {
            Cheeps = await _service.GetAllCheepsFromFollowed(author, t);
            AllCheeps = await _service.GetAllCheepsFromFollowedCount(author);
        } 
        else
        {
            Cheeps = await _service.GetCheepsFromAuthor(author, t);
            AllCheeps = await _service.GetAllCheepsFromAuthor(author);

        }
        Followers = await _followRepo.GetFollowers(User.Identity?.Name!);
        return Page();
    }
}
