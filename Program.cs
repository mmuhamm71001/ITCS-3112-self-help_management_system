//Zainab Dar: 801462977


using System;
using System.Collections.Generic;
using StudentPlanner.Domain;
using StudentPlanner.Presentation;

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

            var service = new UserService();
            User user;

            if (choice == "2")
            {
                user = RegisterFlow(service);
            }
            else
            {
                user = LoginFlow(service);
            }

            if (user == null) return;

            IActionPlan plan = new ActionPlan();
            var taskService = new TaskService();

            if (choice == "2")
            {
                IDailyCheckin checkin = new DailyCheckin();
                RunInitialCheckin(checkin);
            }

            Dashboard dashboard = new Dashboard(user, plan, taskService);
            dashboard.Run();
        }

        private static User RegisterFlow(UserService service)
        {
            Console.WriteLine("\n--- Create Account ---");

            Console.Write("Name     : ");
            string name = Console.ReadLine()?.Trim();

            Console.Write("Email    : ");
            string email = Console.ReadLine()?.Trim();

            Console.Write("Password : ");
            string password = Console.ReadLine()?.Trim();

            User user = service.Register(name, email, password, new List<string>());
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
                case "1": mood = MoodStatus.Great; break;
                case "2": mood = MoodStatus.Okay; break;
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

        private static User LoginFlow(UserService service)
        {
            Console.WriteLine("\n--- Log In ---");

            Console.Write("Email    : ");
            string email = Console.ReadLine()?.Trim();

            Console.Write("Password : ");
            string password = Console.ReadLine()?.Trim();

            User user = service.Login(email, password);
            if (user == null)
            {
                Console.WriteLine("\n[!] Invalid email or password. Exiting.");
                return null;
            }

            return user;
        }
    }
}
