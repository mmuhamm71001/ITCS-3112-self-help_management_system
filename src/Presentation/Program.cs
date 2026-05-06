//Zainab Dar: 801462977


using System;
using System.Collections.Generic;
using StudentPlanner.Domain;
using StudentPlanner.Repositories;

namespace StudentPlanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Student Planner ===\n");
            Console.WriteLine("  1. Log in");
            Console.WriteLine("  2. Create an account");
            Console.Write("Choice: ");
            string choice = Console.ReadLine()?.Trim();

            IUserRepository userRepo = new UserRepository();
            User user;

            if (choice == "2")
            {
                user = RegisterFlow(userRepo);
            }
            else
            {
                user = LoginFlow(userRepo);
            }

            if (user == null) return;

            IActionPlan plan = new ActionPlan();
            ITaskRepository taskRepo = new TaskRepository();

            if (choice == "2")
            {
                IDailyCheckin checkin = new DailyCheckin();
                RunInitialCheckin(checkin);
            }

            var dashboard = new Presentation.Dashboard(user, plan, taskRepo);
            dashboard.Run();
        }

        private static User RegisterFlow(IUserRepository userRepo)
        {
            Console.WriteLine("\n--- Create Account ---");

            Console.Write("Name     : ");
            string name = Console.ReadLine()?.Trim();

            Console.Write("Email    : ");
            string email = Console.ReadLine()?.Trim();

            Console.Write("Password : ");
            string password = Console.ReadLine()?.Trim();

            User user = userRepo.Register(name, email, password, new List<string>());
            if (user == null)
            {
                Console.WriteLine("\n[!] An account with that email already exists. Exiting.");
                return null;
            }

            Console.WriteLine($"\nAccount created! Welcome, {user.GetName()}.");
            return user;
        }

        private static void RunInitialCheckin(IDailyCheckin checkin)
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
                case "1": mood = MoodStatus.Great;   break;
                case "2": mood = MoodStatus.Okay;    break;
                case "3": mood = MoodStatus.Stressed; break;
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

        private static User LoginFlow(IUserRepository userRepo)
        {
            Console.WriteLine("\n--- Log In ---");

            Console.Write("Email    : ");
            string email = Console.ReadLine()?.Trim();

            Console.Write("Password : ");
            string password = Console.ReadLine()?.Trim();

            User user = userRepo.Login(email, password);
            if (user == null)
            {
                Console.WriteLine("\n[!] Invalid email or password. Exiting.");
                return null;
            }

            return user;
        }
    }
}
