using System.Collections.Generic;

namespace StudentPlanner.Domain
{
    public interface ITaskRepository
    {
        void SaveTasks(string email, List<Task> tasks);
        List<Task> LoadTasks(string email);
    }
}
