using System;

namespace StudentPlanner.Domain
{
    public static class TaskFactory
    {
        // typeChoice: "1" = Assignment
        //             "2" = Health / Fitness
        //             "3" = Career
        //             "4" = Finance
        //             "5" = Learning
        //             "6" = Other (custom category)
        public static Task Create(string typeChoice, string title, DateTime dueDate, string extra,
            bool isRecurring = false, string recurrenceLabel = "")
        {
            return typeChoice switch
            {
                "1" => new Assignment(title, dueDate, extra),
                "2" => new PersonalGoal(title, dueDate, "Health",   isRecurring, recurrenceLabel),
                "3" => new PersonalGoal(title, dueDate, "Career",   isRecurring, recurrenceLabel),
                "4" => new PersonalGoal(title, dueDate, "Finance",  isRecurring, recurrenceLabel),
                "5" => new PersonalGoal(title, dueDate, "Learning", isRecurring, recurrenceLabel),
                "6" => new PersonalGoal(title, dueDate, string.IsNullOrWhiteSpace(extra) ? "Other" : extra,
                           isRecurring, recurrenceLabel),
                _   => throw new ArgumentException($"Unknown task type: {typeChoice}")
            };
        }
    }
}
