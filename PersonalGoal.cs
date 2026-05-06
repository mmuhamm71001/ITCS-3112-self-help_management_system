using System;

namespace StudentPlanner.Domain
{
    public class PersonalGoal : Task
    {
        private string category;
        private int progressPct;
        private bool isRecurring;
        private string recurrenceLabel;

        public bool IsRecurring => isRecurring;
        public string RecurrenceLabel => recurrenceLabel;

        public PersonalGoal(string title, DateTime dueDate, string category,
            bool isRecurring = false, string recurrenceLabel = "")
            : base(title, dueDate)
        {
            this.category       = category;
            this.progressPct    = 0;
            this.isRecurring    = isRecurring;
            this.recurrenceLabel = recurrenceLabel;
        }

        public override string GetCategory() => category;

        public override string GetSummary()
        {
            string recurPart = isRecurring && !string.IsNullOrEmpty(recurrenceLabel)
                ? $" | Recurring: {recurrenceLabel}"
                : "";
            return $"[Goal] {title} | Category: {category}{recurPart} | Progress: {progressPct}% | Due: {dueDate:yyyy-MM-dd} | Status: {status}";
        }

        public override string Serialize(string email)
        {
            return $"{email}|PersonalGoal|{title}|{dueDate:yyyy-MM-dd}|{category}|{isRecurring}|{recurrenceLabel}|{progressPct}|{status}";
        }

        public void UpdateProgress(int pct)
        {
            progressPct = Math.Clamp(pct, 0, 100);
        }
    }
}
