namespace SkillShareHub.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalCourses { get; set; }
        public int PendingCourses { get; set; }
        public int ApprovedCourses { get; set; }
        public int RejectedCourses { get; set; }
        public int TotalEnrollments { get; set; }

        public List<InstructorStat> InstructorStats { get; set; }
        public List<Course> TopCourses { get; set; }
    }

    public class InstructorStat
    {
        public string InstructorName { get; set; }
        public int CourseCount { get; set; }
        public int StudentCount { get; set; }
    }

}
