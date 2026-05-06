using System;

namespace StudentPlanner.Domain
{
    public class Assignment : Task
    {
        private string courseName;

        public Assignment(string title, DateTime dueDate, string courseName)
            : base(title, dueDate)
        {
            this.courseName = courseName;
        }

        public override string GetSummary()
        {
            return $"[Assignment] {title} | Course: {courseName} | Due: {dueDate:yyyy-MM-dd} | Status: {status}";
        }

        public override string Serialize(string email)
        {
            return $"{email}|Assignment|{title}|{dueDate:yyyy-MM-dd}|{courseName}|false||0|{status}";
        }

        public override string GetCourseName() => courseName;
    }
}
