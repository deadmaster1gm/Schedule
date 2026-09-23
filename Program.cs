using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class Program
    {
        public static void Main()
        {
            Schedule schedule = new Schedule("*.9.*/2 1-5 10:00:00.000");
        }
    }
}