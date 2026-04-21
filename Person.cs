namespace StudentPlanner.Domain
{
    /// <summary>
    /// Abstract base class representing any person in the system.
    /// </summary>
    public abstract class Person
    {
        protected string name;
        protected string email;

        protected Person(string name, string email)
        {
            this.name = name;
            this.email = email;
        }

        /// <summary>
        /// Displays the profile of the person. Must be implemented by subclasses.
        /// </summary>
        public abstract void DisplayProfile();

        /// <summary>
        /// Returns the person's full name.
        /// </summary>
        public string GetName()
        {
            return name;
        }
    }
}
