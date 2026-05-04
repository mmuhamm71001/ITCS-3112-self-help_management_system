using System;

namespace StudentPlanner.Domain
{
    public static class TaskFactory
    {
        public static Task Create(string typeChoice, string title, DateTime dueDate, string extra)
        {
            return typeChoice switch
            {
                "1" => new Assignment(title, dueDate, extra),
                "2" => new PersonalGoal(title, dueDate, extra),
                _   => throw new ArgumentException($"Unknown task type: {typeChoice}")
            };
        }
    }
}
