using System;
using System.Collections.Generic;
using StudentPlanner.Domain;
using StudentPlanner.Factories;

namespace StudentPlanner.Presentation
{
    public class Dashboard
    {
        private User currentUser;
        private List<Task> tasks;
        private IActionPlan plan;
        private ITaskRepository taskRepository;
        private bool isRunning;

        public Dashboard(User user, IActionPlan plan, ITaskRepository taskRepository)
        {
            currentUser = user;
            this.plan = plan;
            this.taskRepository = taskRepository;
            tasks = taskRepository.LoadTasks(user.GetEmail());
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
            Console.WriteLine("  3. Delete a task");
            Console.WriteLine("  4. Update task status");
            Console.WriteLine("  5. View action plan");
            Console.WriteLine("  6. View my profile");
            Console.WriteLine("  7. Exit");
            Console.WriteLine("==============================");
            Console.Write("Enter choice: ");
        }

        private void HandleInput()
        {
            string input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1": ViewTasks();        break;
                case "2": AddTask();          break;
                case "3": DeleteTask();       break;
                case "4": UpdateTaskStatus(); break;
                case "5": ViewPlan();         break;
                case "6": ViewProfile();      break;
                case "7": isRunning = false;  break;
                default:  HandleInvalidInput(input); break;
            }
        }

        private void HandleInvalidInput(string input)
        {
            string display = string.IsNullOrWhiteSpace(input) ? "(empty)" : $"\"{input}\"";
            Console.WriteLine($"\n  [!] {display} is not a valid option. Please enter a number from 1 to 7.");
        }

        private void ViewTasks()
        {
            Console.WriteLine("\n--- Your Tasks ---");

            if (tasks.Count == 0)
            {
                Console.WriteLine("  You have no tasks yet. Add one from the menu.");
                Console.WriteLine("\nPlease enter your choice again:");
                return;
            }

            foreach (var t in tasks)
                t.Display();
        }

        private void DeleteTask()
        {
            Console.WriteLine("\n--- Delete a Task ---");

            if (tasks.Count == 0)
            {
                Console.WriteLine("  You have no tasks to delete.");
                Console.WriteLine("\nPlease enter your choice again:");
                return;
            }

            for (int i = 0; i < tasks.Count; i++)
                Console.WriteLine($"  {i + 1}. {tasks[i].GetSummary()}");

            Console.Write("\n  Enter the number of the task to delete: ");
            string input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out int choice) || choice < 1 || choice > tasks.Count)
            {
                Console.WriteLine("  [!] Invalid selection. No task deleted.");
                return;
            }

            Task removed = tasks[choice - 1];
            tasks.RemoveAt(choice - 1);
            taskRepository.SaveTasks(currentUser.GetEmail(), tasks);
            Console.WriteLine($"  Task \"{removed.GetTitle()}\" deleted successfully.");
        }

        private void UpdateTaskStatus()
        {
            Console.WriteLine("\n--- Update Task Status ---");

            if (tasks.Count == 0)
            {
                Console.WriteLine("  You have no tasks to update.");
                Console.WriteLine("\nPlease enter your choice again:");
                return;
            }

            for (int i = 0; i < tasks.Count; i++)
                Console.WriteLine($"  {i + 1}. {tasks[i].GetSummary()}");

            Console.Write("\n  Enter the number of the task to update: ");
            string input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out int choice) || choice < 1 || choice > tasks.Count)
            {
                Console.WriteLine("  [!] Invalid selection.");
                return;
            }

            Task task = tasks[choice - 1];

            Console.WriteLine($"\n  Current status: {task.Status}");
            Console.WriteLine("  New status:");
            Console.WriteLine("    1. Not Started");
            Console.WriteLine("    2. In Progress");
            Console.WriteLine("    3. Complete");
            Console.Write("  Choice: ");
            string statusInput = Console.ReadLine()?.Trim();

            TaskStatus newStatus = statusInput switch
            {
                "1" => TaskStatus.NotStarted,
                "2" => TaskStatus.InProgress,
                "3" => TaskStatus.Complete,
                _   => task.Status
            };

            if (statusInput != "1" && statusInput != "2" && statusInput != "3")
            {
                Console.WriteLine("  [!] Invalid choice. Status not changed.");
                return;
            }

            task.SetStatus(newStatus);

            if (newStatus == TaskStatus.Complete && task is PersonalGoal pg)
                pg.UpdateProgress(100);

            taskRepository.SaveTasks(currentUser.GetEmail(), tasks);
            Console.WriteLine($"  Status updated to: {newStatus}");
        }

        private void ViewProfile()
        {
            currentUser.DisplayProfile();

            TaskStats stats = TaskStats.From(tasks);
            Console.WriteLine($"\n  Task Summary: {stats.Total} total | {stats.NotStarted} not started | {stats.InProgress} in progress | {stats.Complete} complete");

            Console.WriteLine("\n  Tasks:");
            if (tasks.Count == 0)
            {
                Console.WriteLine("    (none)");
            }
            else
            {
                for (int i = 0; i < tasks.Count; i++)
                    Console.WriteLine($"    {i + 1}. {tasks[i].GetSummary()}");
            }
            Console.WriteLine("====================");
        }

        private void AddTask()
        {
            Console.WriteLine("\n--- Add Task ---");
            Console.WriteLine("  Type:");
            Console.WriteLine("    1. Assignment");
            Console.WriteLine("    2. Health / Fitness");
            Console.WriteLine("    3. Career");
            Console.WriteLine("    4. Finance");
            Console.WriteLine("    5. Learning");
            Console.WriteLine("    6. Other");
            Console.Write("  Choice: ");
            string typeChoice = Console.ReadLine()?.Trim();

            if (typeChoice != "1" && typeChoice != "2" && typeChoice != "3" &&
                typeChoice != "4" && typeChoice != "5" && typeChoice != "6")
            {
                Console.WriteLine("  [!] Invalid type. Task not added.");
                return;
            }

            Console.Write("  Title: ");
            string title = Console.ReadLine()?.Trim();

            bool isGoal = typeChoice != "1";

            bool isRecurring = false;
            string recurrenceLabel = "";
            if (isGoal)
            {
                Console.Write("  Is this a recurring task? (y/n): ");
                string recurInput = Console.ReadLine()?.Trim().ToLower();
                if (recurInput == "y" || recurInput == "yes")
                {
                    isRecurring = true;
                    Console.WriteLine("  How often?");
                    Console.WriteLine("    1. Daily");
                    Console.WriteLine("    2. Weekly");
                    Console.WriteLine("    3. Monthly");
                    Console.Write("  Choice: ");
                    recurrenceLabel = Console.ReadLine()?.Trim() switch
                    {
                        "1" => "Daily",
                        "2" => "Weekly",
                        "3" => "Monthly",
                        _   => "Daily"
                    };
                }
            }

            DateTime dueDate;
            if (isRecurring)
            {
                Console.Write("  Due date (MM-dd-yyyy, or Enter to skip): ");
                string dateInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(dateInput))
                    dueDate = DateTime.Today.AddYears(1);
                else if (!DateTime.TryParse(dateInput, out dueDate))
                {
                    Console.WriteLine("  [!] Invalid date. Using 1 year from today.");
                    dueDate = DateTime.Today.AddYears(1);
                }
            }
            else
            {
                Console.Write("  Due date (MM-dd-yyyy): ");
                if (!DateTime.TryParse(Console.ReadLine()?.Trim(), out dueDate))
                {
                    Console.WriteLine("  [!] Invalid date. Task not added.");
                    return;
                }
            }

            string extra = "";
            if (typeChoice == "1")
            {
                Console.Write("  Course name: ");
                extra = Console.ReadLine()?.Trim();
            }
            else if (typeChoice == "6")
            {
                Console.Write("  Category (e.g. Hobby, Relationship): ");
                extra = Console.ReadLine()?.Trim();
            }

            Task newTask = TaskFactory.Create(typeChoice, title, dueDate, extra, isRecurring, recurrenceLabel);

            tasks.Add(newTask);
            plan.AddTask(newTask);
            taskRepository.SaveTasks(currentUser.GetEmail(), tasks);

            Console.WriteLine("  Task added successfully.");

            BuildActionPlan(newTask, isGoal);
        }

        private void BuildActionPlan(Task task, bool isPersonalGoal)
        {
            Console.WriteLine("\n--- Build Your Action Plan ---");

            Console.Write("  1. What is your biggest challenge or obstacle right now? ");
            string challenge = Console.ReadLine()?.Trim();

            bool recurring = task is PersonalGoal pg && pg.IsRecurring;
            string timeQuestion = recurring
                ? "  2. How much time can you commit per session? "
                : "  2. How much time can you realistically commit each day to get this done? ";
            Console.Write(timeQuestion);
            string dailyTime = Console.ReadLine()?.Trim();

            plan.SetPlanningContext(
                string.IsNullOrWhiteSpace(challenge) ? "(not set)" : challenge,
                string.IsNullOrWhiteSpace(dailyTime) ? "(not set)" : dailyTime,
                isPersonalGoal
            );

            plan.DisplayWrittenPlan(task);
        }

        private void ViewPlan()
        {
            Console.WriteLine("\n--- Action Plan ---");

            if (!plan.HasPlanningContext)
            {
                Console.WriteLine("  No action plan yet. Add a task first and your plan will be built automatically.");
                return;
            }

            plan.Display();

            Console.WriteLine("\n--- Suggested Schedule ---");
            plan.DistributeAcrossDays();
        }
    }
}
