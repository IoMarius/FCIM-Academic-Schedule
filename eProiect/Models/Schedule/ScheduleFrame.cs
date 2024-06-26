﻿using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace eProiect.Models.Schedule
{
    public class ScheduleFrame
    {
        public List<String> WeekDays { get; set; }
        public List<(TimeSpan, TimeSpan)> TimeIntervals { get; set; }

        public ScheduleFrame()
        {
            WeekDays = new List<String> { "Luni", "Marți", "Miercuri", "Joi", "Vineri", "Sâmbătă" };
            TimeIntervals = new List<(TimeSpan, TimeSpan)>();

            TimeIntervals.Add((new TimeSpan(8, 0, 0), new TimeSpan(9, 30, 0)));
            TimeIntervals.Add((new TimeSpan(9, 45, 0), new TimeSpan(11, 15, 0)));
            TimeIntervals.Add((new TimeSpan(11, 30, 0), new TimeSpan(13, 0, 0)));
            TimeIntervals.Add((new TimeSpan(13, 30, 0), new TimeSpan(15, 0, 0)));
            TimeIntervals.Add((new TimeSpan(15, 15, 0), new TimeSpan(16, 45, 0)));
            TimeIntervals.Add((new TimeSpan(17, 0, 0), new TimeSpan(18, 30, 0)));
            TimeIntervals.Add((new TimeSpan(18, 45, 0), new TimeSpan(20, 15, 0)));
        }

        public DateTime getMondayDate()
        {
            DateTime today = DateTime.Today;
            DayOfWeek currentDayOfWeek = today.DayOfWeek;

            // Calculate the number of days to subtract to get to Monday
            int daysToSubtract = ((int)currentDayOfWeek - (int)DayOfWeek.Monday + 7) % 7;

            // Calculate the date of the Monday of the current week
            return today.AddDays(-daysToSubtract);
        }

        public bool isEvenWeek()
        {
            if (getMondayDate().Day % 2 == 0)
                return true;
            return false;
        }
    }
}