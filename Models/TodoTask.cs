using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class TodoTask
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsPinned { get; set; }

    public PriorityLevel Priority { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string UserId { get; set; }
    public IdentityUser User { get; set; }
}
