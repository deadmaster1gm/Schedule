using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class Schedule
    {
        public Schedule(string scheduleString)
        {
            string[] parts = scheduleString.Split(' ');
            string[] dateParts = parts[0].Split('.');
            string weekDay = parts[1];
            string[] timeParts = parts[2].Split(":");
            string[] secondParts = timeParts[2].Split(".");
            string year = dateParts[0];
            string month = dateParts[1];
            string day = dateParts[2];
            string hour = timeParts[0];
            string minute = timeParts[1];
            string second = secondParts[0];
            string millisecond = secondParts[1];
        }
        private ScheduleRange ParsePart(string text, int min, int max)
        {
            if (text == "*")
            {
                return new ScheduleRange(min, max, 1);
            }

            if (text.Contains('-'))
            {
                string[] parts = text.Split('-');

                int from = int.Parse(parts[0]);
                int to = int.Parse(parts[1]);

                return new ScheduleRange(from, to, 1);
            }

            int value = int.Parse(text);

            return new ScheduleRange(value, value, 1);
        }
    }
}

