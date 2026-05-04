using System;
using System.Security.Cryptography;
using System.Text;

namespace StudentPlanner.Domain
{
    /// <summary>
    /// Represents a registered user of the planner application.
    /// Extends Person with authentication capabilities.
    /// </summary>
    public class User : Person, IAuthenticatable
    {
        private string passwordHash;

        public User(string name, string email, string password)
            : base(name, email)
        {
            this.passwordHash = HashPassword(password);
        }

        /// <summary>
        /// Displays the user's profile information to the console.
        /// </summary>
        public override void DisplayProfile()
        {
            Console.WriteLine("====================");
            Console.WriteLine("  User Profile");
            Console.WriteLine("====================");
            Console.WriteLine($"  Name  : {name}");
            Console.WriteLine($"  Email : {email}");
            Console.WriteLine("====================");
        }

        /// <summary>
        /// Verifies a plaintext password against the stored hash.
        /// </summary>
        /// <param name="password">The plaintext password to check.</param>
        /// <returns>True if the password matches, false otherwise.</returns>
        public bool Authenticate(string password)
        {
            string attempt = HashPassword(password);
            return attempt == passwordHash;
        }

        // ------------------------------------------------------------------
        // Private helpers
        // ------------------------------------------------------------------

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
