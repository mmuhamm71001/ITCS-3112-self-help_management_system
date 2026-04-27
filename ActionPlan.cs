using System;
using System.Collections.Generic;

namespace StudentPlanner.Domain
{
    /// <summary>
    /// An ordered collection of tasks that forms the user's action plan.
    /// </summary>
    public class ActionPlan
    {
        private List<Task> steps;

        public ActionPlan()
        {
            steps = new List<Task>();
        }

        /// <summary>
        /// Adds a task as the next step in the plan.
        /// </summary>
        public void AddStep(Task task)
        {
            steps.Add(task);
        }

        /// <summary>
        /// Prints all steps to the console, numbered from 1.
        /// </summary>
        public void Display()
        {
            if (steps.Count == 0)
            {
                Console.WriteLine("  No steps in your action plan yet.");
                return;
            }

            for (int i = 0; i < steps.Count; i++)
                Console.WriteLine($"  {i + 1}. {steps[i].GetSummary()}");
        }
    }
}
