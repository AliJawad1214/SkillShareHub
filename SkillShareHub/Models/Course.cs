using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillShareHub.Models
{
    public enum CourseStatus { Pending = 0, Approved = 1, Rejected = 2 }

    public class Course
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; }

        [Required, StringLength(2000)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // Store a URL or relative path to an image (we’ll wire up uploads later)
        public string ImageUrl { get; set; }

        // Category
        [Required(ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }   // 👈 make nullable


        public Category Category { get; set; }

        // Instructor (Identity user)
        [BindNever]
        public string InstructorId { get; set; }

        [BindNever]
        public ApplicationUser Instructor { get; set; }

        public CourseStatus Status { get; set; } = CourseStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
