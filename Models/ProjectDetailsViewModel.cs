namespace WebApplication1.Models;

public class ProjectDetailsViewModel
{
    public Project Project { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public Comment NewComment { get; set; } = new();
}
