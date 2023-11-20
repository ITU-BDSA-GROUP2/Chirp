using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class FollowerList
{
    [Required]
    public int AuthorID { get; set; }
    [Required]
    public Author Author { get; set; }
    [Required]
    public int FollowedAuthorID { get; set; }
    [Required]
    public Author FollowedAuthor { get; set; }
}