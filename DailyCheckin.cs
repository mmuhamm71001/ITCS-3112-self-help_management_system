using System;

namespace StudentPlanner.Domain
{
  
    public class DailyCheckin
    {
        private DateTime checkinDate;
        private MoodStatus mood;
        private string notes;

        public DailyCheckin()
        {
            checkinDate = DateTime.Today;
            mood = MoodStatus.Okay;
            notes = string.Empty;
        }

        public void RecordMood(MoodStatus mood, string notes)
        {
            this.mood = mood;
            this.notes = notes;
            this.checkinDate = DateTime.Today;
        }

        public MoodStatus GetMood()
        {
            return mood;
        }

        public void Display()
        {
            Console.WriteLine($"Date : {checkinDate:yyyy-MM-dd}");
            Console.WriteLine($"Mood : {mood}");
            Console.WriteLine($"Notes: {notes}");
        }
    }
}
