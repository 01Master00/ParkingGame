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
                       .Where(l => l.StartsWith("level:"))
                       .Select(l => int.Parse(l.Replace("level:", "")))
                       .ToList();
        }

        public static int LoadPoints()
        {
            if (!File.Exists(savePath))
                return 0;

            string pointLine = File.ReadAllLines(savePath).FirstOrDefault(l => l.StartsWith("points:"));

            if (pointLine == null) return 0;
            return int.Parse(pointLine.Replace("points:", ""));
        }

        public static void SaveCompletedLevel(int level)
        {
            List<int> completed = LoadCompletedLevels();
            int points = LoadPoints();

            if (!completed.Contains(level))
            {
                completed.Add(level);
                points += level * 100;
            }
            else
            {
                points += (level * 100) / 2;
            }

            List<string> lines = completed.Select(l => $"level:{l}").ToList();
            lines.Add($"points:{points}");
            File.WriteAllLines(savePath, lines);
        }

        public static void DeleteSave()
        {
            if (File.Exists(savePath))
                File.Delete(savePath);
        }

        public static bool SpendPoints(int amount)
        {
            int points = LoadPoints();
            if (points < amount) return false;

            points -= amount;

            List<int> completed = LoadCompletedLevels();
            List<string> lines = completed.Select(l => $"level:{l}").ToList();
            lines.Add($"points:{points}");
            File.WriteAllLines(savePath, lines);
            return true;
        }
    }
}