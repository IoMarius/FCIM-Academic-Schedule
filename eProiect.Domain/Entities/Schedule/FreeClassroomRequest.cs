using eProiect.Domain.Entities.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Schedule
{
    public class FreeClassroomsRequest
    {
        /*int floor, (TimeSpan, TimeSpan) timeInterval, int weekdayId*/
        public int Floor { get; set; }
        public int WeekdayId { get; set; }
        public int  Span { get; set; }
        public TimeSpan StartTime { get; set; }
        public ClassFrequency Frequency { get; set; }

    }
}
