using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class ScheduleRange
    {
        public int From { get; }
        public int To { get; }
        public int Step { get; }

        public ScheduleRange(int from, int to, int step)
        {
            From = from;
            To = to;
            Step = step;
        }
        public bool Contains(int value)
        {
            return value >= From
                && value <= To
                && (value - From) % Step == 0;
        }
    }
}
