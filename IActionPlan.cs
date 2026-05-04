using System.Collections.Generic;

namespace StudentPlanner.Domain
{
    public interface IActionPlan
    {
        void AddTask(Task task);
        void Display();
        void DistributeAcrossDays();
        List<Task> GetOrderedList();
    }
}
