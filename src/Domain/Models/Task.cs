using System;

namespace StudentPlanner.Domain
{
    public abstract class Task
    {
        protected string title;
        protected DateTime dueDate;
        protected TaskStatus status = TaskStatus.NotStarted;

        protected Task(string title, DateTime dueDate)
        {
            this.title   = title;
            this.dueDate = dueDate;
        }

        public abstract string GetSummary();
        public abstract string Serialize(string email);

        public string GetTitle()      => title;
        public DateTime GetDueDate()  => dueDate;
        public TaskStatus Status      => status;
        public void SetStatus(TaskStatus s) { status = s; }

        public virtual string GetCourseName() => string.Empty;
        public virtual string GetCategory()   => string.Empty;

        public void Display()
        {
            Console.WriteLine(GetSummary());
        }
    }
}
