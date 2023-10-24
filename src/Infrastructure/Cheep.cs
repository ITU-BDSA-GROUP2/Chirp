using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Cheep
{
    public int CheepId { get; set; }
    [Required]
    [StringLength(280, MinimumLength = 1, ErrorMessage = "*message must be between 1 character and 280")]
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public int AuthorId { get; set; }
    public required Author Author { get; set; }

}