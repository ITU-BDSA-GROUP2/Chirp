using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using EFCore;

namespace Chirp.Razor.Pages;

public class AboutMeModel : PageModel
{
    private readonly IAuthorRepository _authorRepo;
    private readonly ICheepRepository _service;
    private readonly IFollowerListRepository _followRepo;

    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    

    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<CheepDto> AllCheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<FollowDto> Followers { get; set; } = new List<FollowDto>();


    public string Author { get; set;} = "";

    [BindProperty]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Username { get; set;}

    [BindProperty]
    [DisplayFormat(ConvertEmptyStringToNull = false)]

    public string Email { get; set; }
 

    public AboutMeModel(
    ICheepRepository service, 
    IAuthorRepository authorRepo,
    IFollowerListRepository followRepo,
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager
    )
    {
        _service = service;
        _authorRepo = authorRepo;
        _followRepo = followRepo;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ActionResult> OnGet() {
        if(User.Identity?.Name == null) 
        {
            return Redirect("/Identity/Account/Login"); 
        }

        Followers = await _followRepo.GetFollowers(User.Identity.Name); 
        Cheeps = await _service.GetCheepsFromAuthor(User.Identity.Name, 0); 
        return Page();
    }

    public async Task<AuthorDto> getAuthor(int id) {
        return await _authorRepo.GetAuthorByID(id);
    }

    public async Task<AuthorDto> getCurrentAuthor(string name) {
        return await _authorRepo.GetAuthorByName(name);
    }

    public async Task<ActionResult> OnPostUpdateAuthor() {

        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        string currentName = user.UserName;

        string currentEmail = user.Email;

        if (currentName.Equals(Username) && currentEmail.Equals(Email))
        {
            return RedirectToPage();
        } 

            // Update email if it doesn't exist
            var emailCheck = await _userManager.FindByEmailAsync(Email);

            // Update Username
            var usernameCheck = await _userManager.FindByNameAsync(Username);

            var authorNameCheck = await _authorRepo.GetAuthorByName(Username);

            if (usernameCheck != null && !currentName.Equals(Username) && authorNameCheck!=null)
            {
                ModelState.AddModelError(string.Empty, "Username already in use");
            }
            if (emailCheck != null && !currentEmail.Equals(Email))
            {
                ModelState.AddModelError(string.Empty, "Email already in use");
            }

            if (ModelState.IsValid) {
                user.UserName = Username;
                user.Email = Email;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _authorRepo.UpdateAuthor(currentName, user.UserName, user.Email);
                }
            }
            
                


            if(User.Identity?.Name == null) 
        {
            return Redirect("/Identity/Account/Login"); 
        }

        Followers = await _followRepo.GetFollowers(User.Identity.Name); 
        Cheeps = await _service.GetCheepsFromAuthor(User.Identity.Name, 0); 
        return Page();

    }

    // public async Task<ActionResult> OnPostDeleteAuthor() {

    // }

    

    
}
