using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// <summary>
//   This class represents a relation between an author and a cheep they liked. 
// </summary>

public class Like
{
    [Required]
    public int CheepId { get; set; }
    
    [Required]
    public int UserId { get; set; }
}