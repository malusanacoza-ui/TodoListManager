using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


    public class TodoTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsPinned { get; set; }

        [Required]
        public PriorityLevel Priority { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ValidateNever]
    public string UserId { get; set; }

        [ValidateNever]
        public IdentityUser User { get; set; }
    }

