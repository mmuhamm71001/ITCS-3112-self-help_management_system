using System;
using System.Collections.Generic;
using System.IO;

namespace StudentPlanner.Domain
{
    public class TaskService
    {
        private static readonly string DataFile = "tasks.txt";

        // Replaces all saved tasks for this user, leaving other users' data untouched.
        public void SaveTasks(string email, List<Task> tasks)
        {
            var allLines = new List<string>();

            if (File.Exists(DataFile))
            {
                foreach (var line in File.ReadAllLines(DataFile))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split('|');
                    if (parts.Length > 0 && !parts[0].Equals(email, StringComparison.OrdinalIgnoreCase))
                        allLines.Add(line);
                }
            }

            foreach (var task in tasks)
                allLines.Add(task.Serialize(email));

            File.WriteAllLines(DataFile, allLines);
        }

        public List<Task> LoadTasks(string email)
        {
            var result = new List<Task>();
            if (!File.Exists(DataFile)) return result;

            foreach (var line in File.ReadAllLines(DataFile))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split('|');
                if (parts.Length < 9) continue;
                if (!parts[0].Equals(email, StringComparison.OrdinalIgnoreCase)) continue;

                var task = Deserialize(parts);
                if (task != null) result.Add(task);
            }

            return result;
        }

        // Line format: email|type|title|dueDate|field1|isRecurring|recurrenceLabel|progressPct|status
        private static Task Deserialize(string[] parts)
        {
            string type           = parts[1];
            string title          = parts[2];
            if (!DateTime.TryParse(parts[3], out DateTime dueDate)) return null;
            string field1         = parts[4]; // courseName or category
            bool   isRecurring    = parts[5].Equals("true", StringComparison.OrdinalIgnoreCase);
            string recurrenceLabel = parts[6];
            int    progressPct    = int.TryParse(parts[7], out int p) ? p : 0;
            TaskStatus status     = Enum.TryParse<TaskStatus>(parts[8], out var s) ? s : TaskStatus.NotStarted;

            Task task;
            if (type == "Assignment")
            {
                task = new Assignment(title, dueDate, field1);
            }
            else if (type == "PersonalGoal")
            {
                var goal = new PersonalGoal(title, dueDate, field1, isRecurring, recurrenceLabel);
                goal.UpdateProgress(progressPct);
                task = goal;
            }
            else return null;

            task.SetStatus(status);
            return task;
        }
    }
}
