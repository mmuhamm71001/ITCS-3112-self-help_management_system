using System;
using System.Collections.Generic;
using StudentPlanner.Domain;

namespace StudentPlanner.Presentation
{
    /// <summary>
    /// Entry point for the Student Planner console UI.
    /// Owns the main application loop and all top-level I/O handling.
    /// </summary>
    public class Dashboard
    {
        // ------------------------------------------------------------------
        // Fields (match UML)
        // ------------------------------------------------------------------

        private User currentUser;
        private List<Task> tasks;
        private ActionPlan plan;
        private bool isRunning;

        // ------------------------------------------------------------------
        // Constructor
        // ------------------------------------------------------------------

        public Dashboard(User user)
        {
            currentUser = user;
            tasks       = new List<Task>();
            plan        = new ActionPlan();
            isRunning   = false;
        }

        // ------------------------------------------------------------------
        // Run() — main application loop
        // ------------------------------------------------------------------

        /// <summary>
        /// Starts the application loop. Keeps running until the user exits.
        /// </summary>
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

        // ------------------------------------------------------------------
        // DisplayMenu()
        // ------------------------------------------------------------------

        /// <summary>
        /// Prints the main menu options to the console.
        /// </summary>
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

        // ------------------------------------------------------------------
        // HandleInput()
        // ------------------------------------------------------------------

        /// <summary>
        /// Reads the user's menu selection and dispatches to the
        /// appropriate action. Calls HandleInvalidInput() on bad input.
        /// </summary>
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

        // ------------------------------------------------------------------
        // HandleInvalidInput()
        // ------------------------------------------------------------------

        /// <summary>
        /// Handles any input that does not match a valid menu option.
        /// Prints a friendly error message without crashing the loop.
        /// </summary>
        /// <param name="input">The unrecognised input string (may be null).</param>
        private void HandleInvalidInput(string input)
        {
            string display = string.IsNullOrWhiteSpace(input) ? "(empty)" : $"\"{input}\"";
            Console.WriteLine($"\n  [!] {display} is not a valid option. Please enter a number from 1 to 6.");
        }

        // ------------------------------------------------------------------
        // Private action handlers (stubs — filled in by teammates)
        // ------------------------------------------------------------------

        private void ViewTasks()
        {
            Console.WriteLine("\n--- Your Tasks ---");
            if (tasks.Count == 0)
            {
                Console.WriteLine("  No tasks yet.");
                return;
            }
            foreach (var t in tasks)
                t.Display();
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

            Task newTask;

            if (typeChoice == "1")
            {
                Console.Write("  Course name: ");
                string course = Console.ReadLine()?.Trim();
                newTask = new Assignment(title, dueDate, course);
            }
            else if (typeChoice == "2")
            {
                Console.Write("  Category (e.g. Health, Career): ");
                string category = Console.ReadLine()?.Trim();
                newTask = new PersonalGoal(title, dueDate, category);
            }
            else
            {
                Console.WriteLine("  [!] Invalid type. Task not added.");
                return;
            }

            tasks.Add(newTask);
            plan.AddStep(newTask);
            Console.WriteLine("  Task added successfully.");
        }

        private void ViewPlan()
        {
            Console.WriteLine("\n--- Action Plan ---");
            plan.Display();
        }

        private void DailyCheckin()
        {
            Console.WriteLine("\n--- Daily Check-in ---");
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            Console.WriteLine($"  Date: {today}\n");

            bool found = false;
            foreach (var t in tasks)
            {
                if (t is PersonalGoal goal)
                {
                    found = true;
                    Console.WriteLine($"  {t.GetSummary()}");
                    Console.Write("  Update progress % (or press Enter to skip): ");
                    string input = Console.ReadLine()?.Trim();
                    if (int.TryParse(input, out int pct))
                    {
                        goal.UpdateProgress(pct);
                        Console.WriteLine($"  Progress updated to {pct}%.");
                    }
                }
            }

            if (!found)
                Console.WriteLine("  No personal goals to check in on.");
        }
    }
}
