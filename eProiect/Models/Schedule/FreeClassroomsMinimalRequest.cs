using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Models.Schedule
{
    public class FreeClassroomsMinimalRequest
    {
        public int StartHour { get; set; }
        public int StartMinute { get; set; }

        public int Span { get; set; }

        public int Floor { get; set; }
        public int WeekdayId { get; set; }
        public int Frequency { get; set; }
    }
}