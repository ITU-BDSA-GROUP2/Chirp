using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// <summary>
//   This class represents the user of the Chirp! application
//   Since this is a twitter clone every user is an author.
// </summary>
public class Author
{
    [Required]
    public int AuthorId { get; set; }
    [Required]
    public string Name { get; set; } = "";
    [Required]
    public string Email { get; set; } = "";
    [Required]
    public List<Cheep> Cheeps { get; set; } = new List<Cheep>();

    [Required]
    public string ImageUrl { get; set; } = "images/bird1.webp";
}
