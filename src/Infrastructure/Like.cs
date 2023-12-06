using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Like
{
    [Required]
    public int CheepId { get; set; }
    
    [Required]
    public int UserId { get; set; }


}