using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class FollowerList
{
    [Required]
    public Author Author { get; set; } = new Author();

    [Required]
    public List<Author> Followers { get; set; } = new List<Author>();

}