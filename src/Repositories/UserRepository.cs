using System;
using System.Collections.Generic;
using System.IO;
using StudentPlanner.Domain;

namespace StudentPlanner.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly string DataFile = "users.txt";
        private readonly List<User> users = new List<User>();

        public UserRepository()
        {
            LoadUsers();
        }

        public User Register(string name, string email, string password, List<string> goals)
        {
            foreach (var u in users)
                if (u.GetEmail().Equals(email, StringComparison.OrdinalIgnoreCase))
                    return null;

            var user = new User(name, email, password, goals);
            users.Add(user);
            SaveUsers();
            return user;
        }

        public User Login(string email, string password)
        {
            foreach (var u in users)
                if (u.GetEmail().Equals(email, StringComparison.OrdinalIgnoreCase) && u.Authenticate(password))
                    return u;
            return null;
        }

        private void SaveUsers()
        {
            var lines = new List<string>();
            foreach (var u in users)
                lines.Add(u.Serialize());
            File.WriteAllLines(DataFile, lines);
        }

        private void LoadUsers()
        {
            if (!File.Exists(DataFile)) return;
            foreach (var line in File.ReadAllLines(DataFile))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var u = User.Deserialize(line);
                if (u != null) users.Add(u);
            }
        }
    }
}
