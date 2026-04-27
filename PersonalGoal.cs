using System;

namespace StudentPlanner.Domain
{
    /// <summary>
    /// A personal (non-academic) goal with a category and progress tracking.
    /// </summary>
    public class PersonalGoal : Task
    {
        private string category;
        private int progressPct;

        public PersonalGoal(string title, DateTime dueDate, string category)
            : base(title, dueDate)
        {
            this.category    = category;
            this.progressPct = 0;
        }

        /// <summary>
        /// Returns a summary including category and current progress.
        /// </summary>
        public override string GetSummary()
        {
            return $"[Goal] {title} | Category: {category} | Progress: {progressPct}% | Due: {dueDate:yyyy-MM-dd}";
        }

        /// <summary>
        /// Updates completion progress, clamped to [0, 100].
        /// </summary>
        /// <param name="pct">New progress percentage.</param>
        public void UpdateProgress(int pct)
        {
            progressPct = Math.Clamp(pct, 0, 100);
        }
    }
}
