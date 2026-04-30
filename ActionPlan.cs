using System;
using System.Collections.Generic;

namespace StudentPlanner.Domain
{
    
    public class ActionPlan
    {
        private int planId;
        private int availableDays;
        private List<Task> orderedTasks;

        public ActionPlan(int availableDays = 7)
        {
            this.planId = new Random().Next(1000, 9999);
            this.availableDays = availableDays;
            this.orderedTasks = new List<Task>();
        }

    
        public void AddTask(Task task)
        {
            orderedTasks.Add(task);
        }

        public void DistributeAcrossDays()
        {
            if (orderedTasks.Count == 0)
                return;

            Console.WriteLine("\n--- Task Distribution ---");

            int day = 1;
            foreach (var task in orderedTasks)
            {
                Console.WriteLine($"Day {day}: {task.GetSummary()}");

                day++;
                if (day > availableDays)
                    day = 1; // loop back if tasks exceed days
            }
        }

    
        public List<Task> GetOrderedList()
        {
            return orderedTasks;
        }

        public void Display()
        {
            if (orderedTasks.Count == 0)
            {
                Console.WriteLine("  No steps in your action plan yet.");
                return;
            }

            Console.WriteLine($"  Plan ID: {planId}");
            Console.WriteLine($"  Available Days: {availableDays}");

            for (int i = 0; i < orderedTasks.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {orderedTasks[i].GetSummary()}");
            }
        }
    }
}
