using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class FollowerList
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int FollowedAuthorId { get; set; }
}