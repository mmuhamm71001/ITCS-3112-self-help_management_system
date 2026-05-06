namespace StudentPlanner.Domain
{
    public interface IAuthenticatable
    {
        bool Authenticate(string password);
    }
}
