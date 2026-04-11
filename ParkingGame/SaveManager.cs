using System.IO;

namespace ParkingGame
{
    public static class SaveManager
    {
        private static string savePath = "savefile.txt";

        public static List<int> LoadCompletedLevels()
        {
            if (!File.Exists(savePath))
                return new List<int>();

            return File.ReadAllLines(savePath)
                       .Where(l => int.TryParse(l, out _))
                       .Select(int.Parse)
                       .ToList();
        }

        public static void SaveCompletedLevel(int level)
        {
            List<int> completed = LoadCompletedLevels();
            if (!completed.Contains(level))
            {
                completed.Add(level);
                File.WriteAllLines(savePath, completed.Select(l => l.ToString()));
            }
        }
        public static void DeleteSave()
        {
            if (File.Exists(savePath))
                File.Delete(savePath);
        }
    }
}