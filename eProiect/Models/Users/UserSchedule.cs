using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using eProiect.Models.Enums;

namespace eProiect.Models.Users
{
    public class ScheduleCell
    {
        public string Discipline { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string WeekDay { get; set; }
        public string Classroom { get; set; }
        public string AcademicGroup { get; set; }
        public LessonLengh LessonLengh { get; set; }


        public ScheduleCell(string _discipline, string _weekday, string _classroom, string _group, TimeSpan _start, TimeSpan _end)
        {
            Discipline= _discipline;
            StartTime= _start;
            EndTime= _end;
            WeekDay= _weekday;
            Classroom= _classroom;
            AcademicGroup= _group;

            //lojic for lesson length.
            LessonLengh = new LessonLengh();
            var lessonDiff  = _end.Subtract(_start);
            if (lessonDiff.TotalMinutes > 0)
                LessonLengh.SetLength((uint)lessonDiff.TotalMinutes / 90);
            else
                LessonLengh.SetLength(1);
        }

        public ScheduleCell()
        {
            Discipline = "NULL";
        }
    } 

    public class UserSchedule
    {
        private ScheduleCell[,] Schedule;
        private List<(string, uint)> WeekDayLookup; 
        private List<(TimeSpan, uint)> TimeLookup;
        private bool BusyOnWeekend;

        public UserSchedule()
        {
            //initializing the matrix with NULL instances of scheduleCells.
            Schedule = new ScheduleCell[6, 7];
            for(int row=0; row < 6; row++)
            {
                for(int col=0; col < 7; col++)
                {
                    Schedule[row, col] = new ScheduleCell();
                }
            }
        
            BusyOnWeekend= false;

            //initializing lookuptables
            WeekDayLookup= new List<(string, uint)>();
            TimeLookup= new List<(TimeSpan, uint)>();

            WeekDayLookup.Add(("lu", 0));
            WeekDayLookup.Add(("ma", 1));
            WeekDayLookup.Add(("mi", 2));
            WeekDayLookup.Add(("jo", 3));
            WeekDayLookup.Add(("vi", 4));
            WeekDayLookup.Add(("sm", 5));

            TimeLookup.Add((new TimeSpan(8, 0, 0), 0));
            TimeLookup.Add((new TimeSpan(9, 45, 0), 1));
            TimeLookup.Add((new TimeSpan(11, 30, 0), 2));
            TimeLookup.Add((new TimeSpan(13, 30, 0), 3));
            TimeLookup.Add((new TimeSpan(15, 15, 0), 4));
            TimeLookup.Add((new TimeSpan(17, 0, 0), 5));
            TimeLookup.Add((new TimeSpan(18, 45, 0), 6));
        }

        private uint __lookupTime(TimeSpan time)
        {
            foreach(var interval in TimeLookup)
            {
                if(interval.Item1==time)
                    return interval.Item2;
            }
            return 0;
        }

        private uint __lookupWeekday(string weekday)
        {
            foreach(var day in WeekDayLookup)
            {
                if (day.Item1 == weekday)
                    return day.Item2;
            }
            return 0;
        }

        public bool existingLesson(TimeSpan startTime, uint lessonNr)
        {
            if (__lookupTime(startTime) > 6 || lessonNr > 6)
                return false;

            if (Schedule[lessonNr, __lookupTime(startTime)].Discipline != "NULL")
                return true;

            return false;
        }

        public void addLesson(ScheduleCell lesson)
        {
            Schedule[__lookupWeekday(lesson.WeekDay), __lookupTime(lesson.StartTime)] = lesson;
            if (lesson.WeekDay == "sm")
                BusyOnWeekend = true;
        }
        
        public ScheduleCell getLesson(string weekday, uint lessonNr)
        {
            return Schedule[__lookupWeekday(weekday), lessonNr];
        }

        public ScheduleCell getLesson(TimeSpan startTime, uint lessonNr)
        {
            return Schedule[lessonNr, __lookupTime(startTime)];
        }
    
        public uint getBusyDays()
        {
            if (!BusyOnWeekend)
            {
                return 5;
            }
            return 6;
        }
    }
}