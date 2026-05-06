using System.Collections.Generic;

namespace StudentPlanner.Domain
{
    /// <summary>
    /// Value type (struct) that aggregates task counts by status.
    /// Used as a lightweight snapshot — no heap allocation needed for a small bag of counters.
    /// </summary>
    public struct TaskStats
    {
        public int Total;
        public int NotStarted;
        public int InProgress;
        public int Complete;

        public static TaskStats From(List<Task> tasks)
        {
            var s = new TaskStats();
            s.Total = tasks.Count;
            foreach (var t in tasks)
            {
                switch (t.Status)
                {
                    case TaskStatus.NotStarted: s.NotStarted++; break;
                    case TaskStatus.InProgress: s.InProgress++; break;
                    case TaskStatus.Complete:   s.Complete++;   break;
                }
            }
            return s;
        }
    }
}
