using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Comment
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    [Required(ErrorMessage = "Please enter your name.")]
    [StringLength(40, MinimumLength = 2, ErrorMessage = "Name must be 2-40 characters.")]
    [Display(Name = "Your name")]
    public string Author { get; set; } = "";

    [Required(ErrorMessage = "Please enter a comment.")]
    [StringLength(500, MinimumLength = 2, ErrorMessage = "Comment must be 2-500 characters.")]
    [Display(Name = "Comment")]
    public string Message { get; set; } = "";

    public DateTime PostedAt { get; set; }
}
