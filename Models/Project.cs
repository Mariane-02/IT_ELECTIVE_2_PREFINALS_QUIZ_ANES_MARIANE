namespace WebApplication1.Models;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Category { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public string FullDescription { get; set; } = "";
    public string[] Features { get; set; } = Array.Empty<string>();
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string GitHubUrl { get; set; } = "";
    public string Thumbnail { get; set; } = "";
}
