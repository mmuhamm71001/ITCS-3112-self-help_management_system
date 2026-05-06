namespace StudentPlanner.Domain
{
    public abstract class Person
    {
        protected string name;
        protected string email;

        protected Person(string name, string email)
        {
            this.name = name;
            this.email = email;
        }

        public abstract void DisplayProfile();

        public string GetName()  => name;
        public string GetEmail() => email;
    }
}
