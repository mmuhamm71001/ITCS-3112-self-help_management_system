using System;
using System.Collections.Generic;
using StudentPlanner.Domain;

namespace StudentPlanner.Presentation
{
    public class Dashboard
    {
        private User currentUser;
        private List<Task> tasks;
        private IActionPlan plan;
        private IDailyCheckin checkin;
        private bool isRunning;

        public Dashboard(User user, IActionPlan plan, IDailyCheckin checkin)
        {
            currentUser = user;
            tasks = new List<Task>();
            this.plan = plan;
            this.checkin = checkin;
            isRunning = false;
        }

        public void Run()
        {
            isRunning = true;
            Console.Clear();
            Console.WriteLine($"Welcome back, {currentUser.GetName()}!\n");

            while (isRunning)
            {
                DisplayMenu();
                HandleInput();
            }

            Console.WriteLine("\nGoodbye! Stay on top of those deadlines.");
        }

        public void DisplayMenu()
        {
            Console.WriteLine();
            Console.WriteLine("==============================");
            Console.WriteLine("     Student Planner Menu     ");
            Console.WriteLine("==============================");
            Console.WriteLine("  1. View my tasks");
            Console.WriteLine("  2. Add a task");
            Console.WriteLine("  3. View action plan");
            Console.WriteLine("  4. Daily check-in");
            Console.WriteLine("  5. View my profile");
            Console.WriteLine("  6. Exit");
            Console.WriteLine("==============================");
            Console.Write("Enter choice: ");
        }

        private void HandleInput()
        {
            string input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    ViewTasks();
                    break;
                case "2":
                    AddTask();
                    break;
                case "3":
                    ViewPlan();
                    break;
                case "4":
                    DailyCheckin();
                    break;
                case "5":
                    currentUser.DisplayProfile();
                    break;
                case "6":
                    isRunning = false;
                    break;
                default:
                    HandleInvalidInput(input);
                    break;
            }
        }

        private void HandleInvalidInput(string input)
        {
            string display = string.IsNullOrWhiteSpace(input) ? "(empty)" : $"\"{input}\"";
            Console.WriteLine($"\n  [!] {display} is not a valid option. Please enter a number from 1 to 6.");
        }

        private void ViewTasks()
        {
            Console.WriteLine("\n--- Your Tasks ---");

            if (tasks.Count == 0)
            {
                Console.WriteLine("  No tasks yet.");
                return;
            }

            foreach (var t in tasks)
            {
                t.Display();
            }
        }

        private void AddTask()
        {
            Console.WriteLine("\n--- Add Task ---");
            Console.WriteLine("  Type: (1) Assignment  (2) Personal Goal");
            Console.Write("  Choice: ");
            string typeChoice = Console.ReadLine()?.Trim();

            Console.Write("  Title: ");
            string title = Console.ReadLine()?.Trim();

            Console.Write("  Due date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine()?.Trim(), out DateTime dueDate))
            {
                Console.WriteLine("  [!] Invalid date. Task not added.");
                return;
            }

            if (typeChoice != "1" && typeChoice != "2")
            {
                Console.WriteLine("  [!] Invalid type. Task not added.");
                return;
            }

            string prompt = typeChoice == "1" ? "  Course name: " : "  Category (e.g. Health, Career): ";
            Console.Write(prompt);
            string extra = Console.ReadLine()?.Trim();

            Task newTask = TaskFactory.Create(typeChoice, title, dueDate, extra);

            tasks.Add(newTask);
            plan.AddTask(newTask);

            Console.WriteLine("  Task added successfully.");
        }

        private void ViewPlan()
        {
            Console.WriteLine("\n--- Action Plan ---");
            plan.Display();

            Console.WriteLine("\n--- Suggested Schedule ---");
            plan.DistributeAcrossDays();
        }

        private void DailyCheckin()
        {
            Console.WriteLine("\n--- Daily Check-in ---");
            Console.WriteLine("How are you feeling today?");
            Console.WriteLine("  1. Great");
            Console.WriteLine("  2. Okay");
            Console.WriteLine("  3. Stressed");
            Console.Write("Choice: ");

            string moodChoice = Console.ReadLine()?.Trim();
            MoodStatus mood;

            switch (moodChoice)
            {
                case "1":
                    mood = MoodStatus.Great;
                    break;
                case "2":
                    mood = MoodStatus.Okay;
                    break;
                case "3":
                    mood = MoodStatus.Stressed;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Mood set to Okay.");
                    mood = MoodStatus.Okay;
                    break;
            }

            Console.Write("Notes for today: ");
            string notes = Console.ReadLine()?.Trim();

            checkin.RecordMood(mood, notes);

            Console.WriteLine("\nCheck-in saved:");
            checkin.Display();
        }
    }
}
