namespace StudentPlanner.Domain
{
    public interface IDailyCheckin
    {
        void RecordMood(MoodStatus mood, string notes);
        void Display();
    }
}
