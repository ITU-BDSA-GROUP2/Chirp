using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class ApplicationUser : IdentityUser
{
    [Key]
    public string Email { get; set; }
    public string Name { get; set; }

}