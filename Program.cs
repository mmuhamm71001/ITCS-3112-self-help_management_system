//Zainab Dar: 801462977

using System;
using StudentPlanner.Domain;
using StudentPlanner.Presentation;

namespace StudentPlanner
{
    /// <summary>
    /// Application entry point. Handles login before handing off to Dashboard.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Student Planner ===\n");

            // Demo user — replace with UserService.Login() once auth layer is wired up
            User user = new User("Mariam", "mariam@email.com", "password123");

            Console.Write("Email    : ");
            string email = Console.ReadLine()?.Trim();

            Console.Write("Password : ");
            string password = Console.ReadLine()?.Trim();

            if (email != user.GetEmail() || !user.Authenticate(password))
            {
                Console.WriteLine("\n[!] Invalid credentials. Exiting.");
                return;
            }

            Dashboard dashboard = new Dashboard(user);
            dashboard.Run();
        }
    }
}
