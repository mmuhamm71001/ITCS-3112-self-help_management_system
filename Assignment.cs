using System;

namespace StudentPlanner.Domain
{
    /// <summary>
    /// An academic assignment tied to a specific course.
    /// </summary>
    public class Assignment : Task
    {
        private string courseName;

        public Assignment(string title, DateTime dueDate, string courseName)
            : base(title, dueDate)
        {
            this.courseName = courseName;
        }

        /// <summary>
        /// Returns a summary including the course name and due date.
        /// </summary>
        public override string GetSummary()
        {
            return $"[Assignment] {title} | Course: {courseName} | Due: {dueDate:yyyy-MM-dd}";
        }

        /// <summary>
        /// Returns the name of the course this assignment belongs to.
        /// </summary>
        public override string GetCourseName()
        {
            return courseName;
        }
    }
}
