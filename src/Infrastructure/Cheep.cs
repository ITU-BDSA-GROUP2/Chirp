using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// <summary>
//   This class represents a message in the Chirp! application
// </summary>
public class Cheep
{
    [Required]
    public int CheepId { get; set; }
    [Required]
    [StringLength(160, MinimumLength = 1, ErrorMessage = "*message must be between 1 character and 280")]
    public string Text { get; set; } = "";
    public DateTime TimeStamp { get; set; }
    [Required]
    public int AuthorId { get; set; }
    [Required]
    public Author Author { get; set; } = new Author();

    public int Likes { get; set; } = 0;

}