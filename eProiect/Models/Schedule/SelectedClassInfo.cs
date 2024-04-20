using eProiect.Domain.Entities.Schedule;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Web;

namespace eProiect.Models.Schedule
{
    public class SelectedClassInfo
    {
        public TimeSpan StartTime { get; set; }
        public  TimeSpan EndTime { get; set; }
        public int WeekDayNr { get; set; }
        public string WeekDayName { get; set; }

        //user when sent in backend
        public int ClassType { get; set; }
        public  int UserId { get; set; }
        public  string Group { get; set; }
        public  string ClassRoom {  get; set; }

        //ctor for displaying date time in pre add page
        public SelectedClassInfo(int sHour, int sMinutes, int sDay)
        {
            StartTime = new TimeSpan(sHour, sMinutes, 0);
            WeekDayNr = sDay;
            switch (sDay)
            {
                case 0: WeekDayName = "Luni"; break;
                case 1: WeekDayName = "Marți"; break;
                case 2: WeekDayName = "Miercuri"; break;
                case 3: WeekDayName = "Joi"; break;
                case 4: WeekDayName = "Vineri"; break;
                case 5: WeekDayName = "Sâmbătă"; break;
                default: throw new Exception("SelectedClassInfo(): Invalid weekday entry.");
            }
        }

        public SelectedClassInfo(TimeSpan startTime, int dayNr)
        {
            StartTime = startTime;
            WeekDayNr=dayNr;

            //day to weekdayname
            switch(dayNr)
            {
                case 0: WeekDayName = "Luni"; break;
                case 1: WeekDayName = "Marți"; break;
                case 2: WeekDayName = "Miercuri"; break;
                case 3: WeekDayName = "Joi"; break;
                case 4: WeekDayName = "Vineri"; break;
                case 5: WeekDayName = "Sâmbătă"; break;
                default: throw new Exception("SelectedClassInfo(): Invalid weekday selected.");
            }
        }

        //for mapping into lesson object from backend.
        public SelectedClassInfo(TimeSpan startTime, TimeSpan endTime, int weekDayNr, int classType, int userId, string group, string classRoom)
        {
            StartTime=startTime;
            EndTime = endTime;
            WeekDayNr = weekDayNr;
            ClassType = classType;
            UserId = userId;
            Group=group;
            ClassRoom=classRoom;
        }
    }
}