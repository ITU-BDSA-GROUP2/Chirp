using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
}
