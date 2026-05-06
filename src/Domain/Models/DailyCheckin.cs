using System;

namespace StudentPlanner.Domain
{
    public class DailyCheckin : IDailyCheckin
    {
        private MoodStatus currentMood;
        private string notes;
        private DateTime date;

        public DailyCheckin()
        {
            currentMood = MoodStatus.Okay;
            notes = string.Empty;
            date = DateTime.Today;
        }

        public void RecordMood(MoodStatus mood, string notes)
        {
            this.currentMood = mood;
            this.notes = notes;
            this.date = DateTime.Today;
        }

        public void Display()
        {
            Console.WriteLine($"  Date  : {date:yyyy-MM-dd}");
            Console.WriteLine($"  Mood  : {currentMood}");
            Console.WriteLine($"  Notes : {(string.IsNullOrWhiteSpace(notes) ? "(none)" : notes)}");
        }
    }
}
