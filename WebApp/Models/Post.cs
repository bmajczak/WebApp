using System;

namespace WebApp.Models;

public class Post
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Excerpt { get; set; }
    public string? Content {get; set;}
    public DateTime CreatedDate { get; set; }
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}
