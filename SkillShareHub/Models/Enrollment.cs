using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SkillShareHub.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        // Course
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // Student (Identity user)
        [Required]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    }
}
