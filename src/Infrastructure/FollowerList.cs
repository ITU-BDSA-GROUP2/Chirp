using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// <summary>
//   This class represents a relation between an author following another author.
// </summary>

public class FollowerList
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int FollowedAuthorId { get; set; }
}