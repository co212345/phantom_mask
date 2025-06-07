using phantom_mask.Models;

namespace phantom_mask.Data
{
    public class OpeningHourParser
    {
        private static readonly Dictionary<string, DayOfWeek> DayMap = new()
        {
            ["Mon"] = DayOfWeek.Monday,
            ["Tue"] = DayOfWeek.Tuesday,
            ["Wed"] = DayOfWeek.Wednesday,
            ["Thu"] = DayOfWeek.Thursday,
            ["Thur"] = DayOfWeek.Thursday,
            ["Fri"] = DayOfWeek.Friday,
            ["Sat"] = DayOfWeek.Saturday,
            ["Sun"] = DayOfWeek.Sunday
        };

        public static List<DailyOpeningHour> Parse(string raw)
        {
            var openingHours = new List<DailyOpeningHour>();
            if(string.IsNullOrWhiteSpace(raw)) return openingHours;

            var blocks = raw.Split(" / ");
            foreach (var segment in blocks)
            {
                var parts = segment.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                List<DayOfWeek> days = new();
                int i = 0;

                // 擷取 DayPart（星期）
                while (i < parts.Length && !parts[i].Contains(":"))
                {
                    var token = parts[i].Replace(",", "");

                    if (DayMap.ContainsKey(token))
                    {
                        var day = DayMap[token];

                        if (i + 2 < parts.Length && parts[i + 1] == "-" && DayMap.ContainsKey(parts[i + 2]))
                        {
                            var end = DayMap[parts[i + 2]]==DayOfWeek.Sunday?7:(int)DayMap[parts[i + 2]];
                            for (int d = (int)day; d <= end; d++)
                                days.Add((DayOfWeek)(d%7));
                            i += 3;
                        }
                        else
                        {
                            days.Add(day);
                            i++;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }

                // 擷取 TimePart（時間區間）
                if (i + 2 < parts.Length && parts[i + 1] == "-")
                {
                    var startTime = TimeSpan.Parse(parts[i]);
                    var endTime = TimeSpan.Parse(parts[i + 2]);

                    foreach (var d in days)
                    {
                        openingHours.Add(new DailyOpeningHour
                        {
                            Day = d,
                            StartTime = startTime,
                            EndTime = endTime
                        });
                    }
                }
            }
            return openingHours;
        }
    }
}
