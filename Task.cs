using System;

namespace StudentPlanner.Domain
{
    /// <summary>
    /// Abstract base class for all tasks in the planner.
    /// </summary>
    public abstract class Task
    {
        protected string title;
        protected DateTime dueDate;

        protected Task(string title, DateTime dueDate)
        {
            this.title   = title;
            this.dueDate = dueDate;
        }

        /// <summary>
        /// Returns a one-line summary string for this task.
        /// </summary>
        public abstract string GetSummary();

        /// <summary>
        /// Returns the course name associated with this task.
        /// Defaults to empty string; overridden by Assignment.
        /// </summary>
        public virtual string GetCourseName() => string.Empty;

        /// <summary>
        /// Prints the task summary to the console.
        /// </summary>
        public void Display()
        {
            Console.WriteLine(GetSummary());
        }
    }
}
