private void DailyCheckin()
{
    Console.WriteLine("\n--- Daily Check-in ---");
    Console.WriteLine("How are you feeling today?");
    Console.WriteLine("  1. Great");
    Console.WriteLine("  2. Okay");
    Console.WriteLine("  3. Stressed");
    Console.Write("Choice: ");

    string moodChoice = Console.ReadLine()?.Trim();
    MoodStatus mood;

    switch (moodChoice)
    {
        case "1":
            mood = MoodStatus.Great;
            break;
        case "2":
            mood = MoodStatus.Okay;
            break;
        case "3":
            mood = MoodStatus.Stressed;
            break;
        default:
            Console.WriteLine("Invalid choice. Mood set to Okay.");
            mood = MoodStatus.Okay;
            break;
    }

    Console.Write("Notes for today: ");
    string notes = Console.ReadLine()?.Trim();

    checkin.RecordMood(mood, notes);

    Console.WriteLine("\nCheck-in saved:");
    checkin.Display();
}
