using System;
using System.Collections.Generic;

namespace StudentPlanner.Domain
{
    public class ActionPlan : IActionPlan
    {
        private int planId;
        private int availableDays;
        private List<Task> orderedTasks;

        private string mainChallenge;
        private string dailyCommitment;
        private bool isPersonalGoal;

        public bool HasPlanningContext { get; private set; }

        public ActionPlan(int availableDays = 7)
        {
            this.planId = new Random().Next(1000, 9999);
            this.availableDays = availableDays;
            this.orderedTasks = new List<Task>();
            this.HasPlanningContext = false;
        }

        public void SetPlanningContext(string mainChallenge, string dailyCommitment, bool isPersonalGoal)
        {
            this.mainChallenge   = mainChallenge;
            this.dailyCommitment = dailyCommitment;
            this.isPersonalGoal  = isPersonalGoal;
            this.HasPlanningContext = true;
        }

        public void AddTask(Task task)
        {
            orderedTasks.Add(task);
        }

        public void DistributeAcrossDays()
        {
            if (orderedTasks.Count == 0)
                return;

            int day = 1;
            foreach (var task in orderedTasks)
            {
                Console.WriteLine($"  Day {day}: {task.GetSummary()}");
                day++;
                if (day > availableDays)
                    day = 1;
            }
        }

        public List<Task> GetOrderedList()
        {
            return orderedTasks;
        }

        public void Display()
        {
            Console.WriteLine($"  Plan ID       : {planId}");
            Console.WriteLine($"  Available Days: {availableDays}");

            if (HasPlanningContext)
            {
                Console.WriteLine();
                Console.WriteLine("  -- Your Plan Context --");
                Console.WriteLine($"  {"Challenge",-15}: {mainChallenge}");
                Console.WriteLine($"  {"Commitment",-15}: {dailyCommitment}");
            }

            Console.WriteLine();

            if (orderedTasks.Count == 0)
            {
                Console.WriteLine("  No tasks in your action plan yet. Add tasks from the main menu.");
                return;
            }

            Console.WriteLine("  Tasks:");
            for (int i = 0; i < orderedTasks.Count; i++)
                Console.WriteLine($"  {i + 1}. {orderedTasks[i].GetSummary()}");
        }

        public void DisplayWrittenPlan(Task task)
        {
            var (verb, finishPhrase, tip, typeLabel) = GetStyle(task);

            bool isRecurring = task is PersonalGoal pg && pg.IsRecurring;
            string recurrenceLabel = isRecurring ? ((PersonalGoal)task).RecurrenceLabel : "";

            if (isRecurring && !string.IsNullOrEmpty(recurrenceLabel))
                typeLabel = $"{typeLabel} · {recurrenceLabel}";

            int daysLeft = Math.Max(1, (task.GetDueDate() - DateTime.Today).Days);
            string dueStr  = task.GetDueDate().ToString("yyyy-MM-dd");
            string dueTag  = isRecurring ? "Target End" : "Due       ";

            Console.WriteLine();
            Console.WriteLine($"  === Action Plan: \"{task.GetTitle()}\" ===");
            Console.WriteLine($"  Type      : {typeLabel}");
            Console.WriteLine($"  {dueTag} : {dueStr}  ({daysLeft} day{(daysLeft == 1 ? "" : "s")} from today)");
            Console.WriteLine($"  Challenge : {mainChallenge}");
            Console.WriteLine($"  Commitment: {dailyCommitment}");
            Console.WriteLine();
            Console.WriteLine("  Your Step-by-Step Plan:");
            Console.WriteLine("  " + new string('─', 46));

            if (isRecurring)
                DisplayRecurringPlan(task, verb, recurrenceLabel);
            else if (daysLeft <= 3)
                WriteDayByDayPlan(task, daysLeft, verb, finishPhrase);
            else if (daysLeft <= 6)
                WriteTwoPhasePlan(task, daysLeft, verb, finishPhrase);
            else
                WriteThreePhasePlan(task, daysLeft, verb, finishPhrase);

            Console.WriteLine("  " + new string('─', 46));
            Console.WriteLine($"  Tip: {tip}");
        }

        // ── Style lookup ──────────────────────────────────────────────────

        private (string verb, string finishPhrase, string tip, string typeLabel) GetStyle(Task task)
        {
            if (!isPersonalGoal)
                return (
                    "study and complete work for",
                    "final review — then submit or turn in your work",
                    "Steady daily work beats last-minute cramming.",
                    "Assignment"
                );

            string cat = task.GetCategory().Trim().ToLower();

            if (cat == "health" || cat.Contains("health") || cat.Contains("fitness") || cat.Contains("workout") || cat.Contains("exercise"))
                return (
                    "train / work toward",
                    "celebrate your progress — every rep counts",
                    "Consistency beats intensity. Showing up is the win.",
                    "Health Goal"
                );

            if (cat == "career" || cat.Contains("career") || cat.Contains("job") || cat.Contains("professional"))
                return (
                    "work on",
                    "reflect on your progress and outline your next steps",
                    "Small daily actions build lasting career momentum.",
                    "Career Goal"
                );

            if (cat == "finance" || cat.Contains("finance") || cat.Contains("money") || cat.Contains("budget") || cat.Contains("saving"))
                return (
                    "review and act on",
                    "review your results and set your next milestone",
                    "What gets tracked gets improved. Keep at it.",
                    "Finance Goal"
                );

            if (cat == "learning" || cat.Contains("learn") || cat.Contains("study") || cat.Contains("skill") || cat.Contains("course"))
                return (
                    "practice and make progress on",
                    "review what you've learned and note what comes next",
                    "Regular practice is how skills become permanent.",
                    "Learning Goal"
                );

            // Generic — use the capitalized category name as the label
            string label = string.IsNullOrWhiteSpace(cat) ? "Personal Goal"
                : $"{task.GetCategory().Trim()} Goal";
            return (
                "make progress on",
                "reflect on how far you've come and what's next",
                $"Keep showing up for \"{task.GetTitle()}\" — progress compounds.",
                label
            );
        }

        // ── Recurring habit plan ──────────────────────────────────────────

        private void DisplayRecurringPlan(Task task, string verb, string recurrenceLabel)
        {
            string freq    = recurrenceLabel.ToLower(); // "daily", "weekly", "monthly"
            string freqAdv = freq == "weekly" ? "each week"
                           : freq == "monthly" ? "each month"
                           : "every day";

            Console.WriteLine($"  Week 1 (Days 1–7) — Starting the Habit");
            Console.WriteLine($"    Day 1   : Do your first session of \"{task.GetTitle()}\". Keep it short and manageable.");
            Console.WriteLine($"    Days 2–7: Show up {freqAdv}, even briefly. Consistency matters more than perfection.");
            Console.WriteLine();
            Console.WriteLine($"  Weeks 2–3 (Days 8–21) — Building Consistency");
            Console.WriteLine($"    Aim to hit your commitment of {dailyCommitment} {freqAdv}.");
            Console.WriteLine($"    When \"{mainChallenge}\" gets in the way, just start — even 5 minutes counts.");
            Console.WriteLine($"    Track each session: seeing your streak build is its own motivation.");
            Console.WriteLine();
            Console.WriteLine($"  Week 4+ (Day 22 onward) — Making It Stick");
            Console.WriteLine($"    This is now a habit. Guard your scheduled time — treat it like a commitment.");
            Console.WriteLine($"    Push for gradual improvement {freqAdv}: small increases compound over time.");
        }

        // ── Deadline-based plans ──────────────────────────────────────────

        private void WriteDayByDayPlan(Task task, int daysLeft, string verb, string finishPhrase)
        {
            for (int d = 1; d <= daysLeft; d++)
            {
                string dayLabel = d == daysLeft ? $"Day {d} [DUE]" : $"Day {d}      ";
                string action;

                if (d == 1)
                    action = $"Break \"{task.GetTitle()}\" into small steps. Start the first one today.";
                else if (d == daysLeft)
                    action = $"Wrap up: {finishPhrase}.";
                else
                    action = $"Keep going — {verb} \"{task.GetTitle()}\". Push through: {mainChallenge}.";

                Console.WriteLine($"  {dayLabel}: {action}");
            }
        }

        private void WriteTwoPhasePlan(Task task, int daysLeft, string verb, string finishPhrase)
        {
            int mid = daysLeft / 2;

            Console.WriteLine($"  Phase 1 (Days 1–{mid}) — Getting Started");
            Console.WriteLine($"    Day 1    : List the steps needed to {verb} \"{task.GetTitle()}\". Start the first one today.");
            Console.WriteLine($"    Days 2–{mid} : Spend your {dailyCommitment} each day making steady progress.");
            Console.WriteLine($"              When \"{mainChallenge}\" gets in the way, take a short break and come back.");
            Console.WriteLine();
            Console.WriteLine($"  Phase 2 (Days {mid + 1}–{daysLeft}) — Final Push");
            Console.WriteLine($"    Days {mid + 1}–{daysLeft - 1}: Stay focused — keep your {dailyCommitment} sessions going.");
            Console.WriteLine($"    Day {daysLeft} [DUE]: {Capitalize(finishPhrase)}. You made it!");
        }

        private void WriteThreePhasePlan(Task task, int daysLeft, string verb, string finishPhrase)
        {
            int p1End = daysLeft / 3;
            int p2End = (daysLeft * 2) / 3;

            Console.WriteLine($"  Phase 1 (Days 1–{p1End}) — Getting Started");
            Console.WriteLine($"    Day 1      : Write down every step needed to {verb} \"{task.GetTitle()}\". Set a small first milestone.");
            Console.WriteLine($"    Days 2–{p1End}  : Start with the easiest parts first. Build the habit of {dailyCommitment} per day.");
            Console.WriteLine();
            Console.WriteLine($"  Phase 2 (Days {p1End + 1}–{p2End}) — Building Momentum");
            Console.WriteLine($"    Days {p1End + 1}–{p2End - 1}: Commit your {dailyCommitment} daily to {verb} \"{task.GetTitle()}\".");
            Console.WriteLine($"              When you face \"{mainChallenge}\", acknowledge it and stay on schedule.");
            Console.WriteLine($"    Day {p2End}     : Check your progress — are you on pace? Adjust remaining steps if needed.");
            Console.WriteLine();
            Console.WriteLine($"  Phase 3 (Days {p2End + 1}–{daysLeft}) — Final Push");
            Console.WriteLine($"    Days {p2End + 1}–{daysLeft - 1}: Focus only on finishing. No new distractions — close out \"{task.GetTitle()}\".");
            Console.WriteLine($"    Day {daysLeft} [DUE]: {Capitalize(finishPhrase)}. You made it!");
        }

        private static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
