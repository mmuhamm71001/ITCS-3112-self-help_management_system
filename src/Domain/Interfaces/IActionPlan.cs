using System.Collections.Generic;

namespace StudentPlanner.Domain
{
    public interface IActionPlan
    {
        void AddTask(Task task);
        void Display();
        void DistributeAcrossDays();
        void DisplayWrittenPlan(Task task);
        List<Task> GetOrderedList();
        void SetPlanningContext(string mainChallenge, string dailyCommitment, bool isPersonalGoal);
        bool HasPlanningContext { get; }
    }
}
