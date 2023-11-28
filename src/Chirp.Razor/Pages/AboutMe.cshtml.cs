using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure;
using EFCore;

namespace Chirp.Razor.Pages;

public class AboutMeModel : PageModel
{
    private readonly IAuthorRepository _authorRepo;
    private readonly ICheepRepository _service;
    private readonly IFollowerListRepository _followRepo;

    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<CheepDto> AllCheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<FollowDto> Followers { get; set; } = new List<FollowDto>();


    public string Author { get; set;} = "";

 

    public AboutMeModel(ICheepRepository service, IAuthorRepository authorRepo, IFollowerListRepository followRepo)
    {
        _service = service;
        _authorRepo = authorRepo;
        _followRepo = followRepo;
    }

    public async Task<ActionResult> OnGet() {
        Followers = await _followRepo.GetFollowers(User.Identity.Name); 
        Cheeps = await _service.GetCheepsFromAuthor(User.Identity.Name, 0); 
        return Page();
    }

    public async Task<AuthorDto> getAuthor(int id) {
        return await _authorRepo.GetAuthorByID(id);
    }
    

    
}
