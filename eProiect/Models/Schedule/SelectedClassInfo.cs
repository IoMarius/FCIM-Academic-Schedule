using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Models.Schedule
{
    public class SelectedClassInfo
    {
        public TimeSpan StartTime { get; set; }
        public int WeekDayNr { get; set; }

        public SelectedClassInfo(TimeSpan startTime, int dayNr)
        {
            StartTime=startTime;
            WeekDayNr=dayNr;
        }
    }
}