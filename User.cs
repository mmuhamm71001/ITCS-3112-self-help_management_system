using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace StudentPlanner.Domain
{
    /// <summary>
    /// Represents a registered user of the planner application.
    /// Extends Person with authentication capabilities and goal tracking.
    /// </summary>
    public class User : Person, IAuthenticatable
    {
        private string passwordHash;
        private List<string> goals;

        public User(string name, string email, string password, List<string> goals = null)
            : base(name, email)
        {
            this.passwordHash = HashPassword(password);
            this.goals = goals ?? new List<string>();
        }

        // Used internally by Deserialize — accepts an already-hashed password.
        private static User FromHash(string name, string email, string existingHash, List<string> goals)
        {
            var u = new User(name, email, "_placeholder_", goals);
            u.passwordHash = existingHash;
            return u;
        }

        public override void DisplayProfile()
        {
            Console.WriteLine("====================");
            Console.WriteLine("  User Profile");
            Console.WriteLine("====================");
            Console.WriteLine($"  Name  : {name}");
            Console.WriteLine($"  Email : {email}");
            if (goals.Count > 0)
            {
                Console.WriteLine("  Goals :");
                for (int i = 0; i < goals.Count; i++)
                    Console.WriteLine($"    {i + 1}. {goals[i]}");
            }
            else
            {
                Console.WriteLine("  Goals : (none set)");
            }
            Console.WriteLine("====================");
        }

        public bool Authenticate(string password)
        {
            return HashPassword(password) == passwordHash;
        }

        /// <summary>Serializes user data to a pipe-delimited string for file storage.</summary>
        public string Serialize()
        {
            string goalList = goals.Count > 0 ? string.Join(";", goals) : "";
            return $"{name}|{email}|{passwordHash}|{goalList}";
        }

        /// <summary>Reconstructs a User from a serialized line. Returns null if the line is malformed.</summary>
        public static User Deserialize(string line)
        {
            string[] parts = line.Split('|');
            if (parts.Length < 3) return null;

            string name = parts[0];
            string email = parts[1];
            string hash = parts[2];
            var goals = new List<string>();
            if (parts.Length > 3 && !string.IsNullOrEmpty(parts[3]))
                goals.AddRange(parts[3].Split(';'));

            return FromHash(name, email, hash, goals);
        }

        // ------------------------------------------------------------------
        // Private helpers
        // ------------------------------------------------------------------

        private static string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
