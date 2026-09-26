using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class Schedule
    {
        private List<ScheduleRange> _years;
        private List<ScheduleRange> _months;
        private List<ScheduleRange> _days;
        private List<ScheduleRange> _dayOfweek;
        private List<ScheduleRange> _hour;
        private List<ScheduleRange> _minute;
        private List<ScheduleRange> _second;
        private List<ScheduleRange> _millisecond;

        public Schedule(string scheduleString)
        {
            string[] parts = scheduleString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            switch(parts.Length)
            {
                case 1: // Пришло только время формата 10:00:00.000
                    ParseDate("*.*.*");
                    _dayOfweek = ParsePart("*", 0, 6);
                    ParseTime(parts[0]);
                    break;
                case 2: // Пришли дата и время без дня недели формата 26.9.26 10:00:00.000
                    ParseDate(parts[0]);
                    _dayOfweek = ParsePart("*", 0, 6);
                    ParseTime(parts[1]);
                    break;
                case 3: // Пришла полная строка формата 26.9.26 4 10:00:00.000
                    ParseDate(parts[0]);
                    _dayOfweek = ParsePart(parts[1], 0, 6);
                    ParseTime(parts[2]);
                    break;
                default:
                    throw new ArgumentException("Неверный формат строки");
            }
        }
        private List<ScheduleRange> ParsePart(string text, int min, int max)
        {
            List<ScheduleRange> ranges = new List<ScheduleRange> ();

            string[] parts = text.Split(",");

            foreach (string part in parts)
            {
                if (part.Contains('/'))
                {
                    string[] splitStep = part.Split('/');

                    string rangePart = splitStep[0];
                    int step = int.Parse(splitStep[1]);

                    if(rangePart == "*")
                    {
                        ScheduleRange rangeAny = new ScheduleRange(min, max, step);

                        ranges.Add(rangeAny);
                    }
                    else
                    {
                        string[] splitBy = rangePart.Split('-');

                        int from = int.Parse(splitBy[0]);
                        int to = int.Parse(splitBy[1]);

                        ScheduleRange range = new ScheduleRange(from, to, step);

                        ranges.Add(range);
                    }
                }

                else if (part.Contains("-"))
                {
                    string[] splitBy = part.Split('-');

                    int from = int.Parse(splitBy[0]);
                    int to = int.Parse(splitBy[1]);

                    ScheduleRange range = new ScheduleRange(from, to, 1);

                    ranges.Add(range);
                }

                else if (part == "*")
                {
                    ScheduleRange range = new ScheduleRange(min, max, 1);
                    ranges.Add(range);
                }
                else
                {
                    int value = int.Parse(part);

                    ScheduleRange range = new ScheduleRange(value, value, 1);
                    ranges.Add(range);
                }
            }
                return ranges;
        }
        private void ParseDate(string date)
        {
            string[] dateParts = date.Split('.');

            _years = ParsePart(dateParts[0], 2000, 2100);
            _months = ParsePart(dateParts[1], 1, 12);
            _days = ParsePart(dateParts[2], 1, 32);
        }

        private void ParseTime(string time)
        {
            string[] timeParts = time.Split(':');
            string[] secondParts = timeParts[2].Split('.');

            _hour = ParsePart(timeParts[0], 0, 23);
            _minute = ParsePart(timeParts[1], 0, 59);
            _second = ParsePart(secondParts[0], 0, 59);

            if (secondParts.Length > 1)
            {
                _millisecond = ParsePart(secondParts[1], 0, 999);
            }
            else
            {
                _millisecond = ParsePart("0", 0, 999);
            }
        }

        private bool Matches(List<ScheduleRange> ranges, int value)
        {
            foreach (ScheduleRange range in ranges)
            {
                if (range.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }
        private bool IsMatch (DateTime date)
        {
            return Matches(_years, date.Year) &&
                   Matches(_months, date.Month) &&
                   Matches(_days, date.Day) &&
                   Matches(_dayOfweek, (int)date.DayOfWeek) &&
                   Matches(_hour, date.Hour) &&
                   Matches(_minute, date.Minute) &&
                   Matches(_second, date.Second) &&
                   Matches(_millisecond, date.Millisecond);
        }
    }
}

