using System.Collections.Generic;

namespace StudentPlanner.Domain
{
    public interface IUserRepository
    {
        User Register(string name, string email, string password, List<string> goals);
        User Login(string email, string password);
    }
}
