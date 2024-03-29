using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using eProiect.Models.Enums;

namespace eProiect.Models.Users
{

#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    public class Lesson
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        public string Discipline { get; set; }
        public string Type { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string WeekDay { get; set; }
        public string Classroom { get; set; }
        public string AcademicGroup { get; set; }
        public LessonLengh LessonLengh { get; set; }
        public LessonWeekType WeekSpan { get; set; }



        public Lesson(string _discipline, string _lessonType, string _weekday, string _classroom, 
            string _group, TimeSpan _start, TimeSpan _end, LessonWeekType _weekSpan=LessonWeekType.FULL)
        {
            Discipline= _discipline;
            Type= _lessonType;
            StartTime= _start;
            EndTime= _end;
            WeekDay= _weekday;
            Classroom= _classroom;
            AcademicGroup= _group;
            WeekSpan= _weekSpan;

            //lojic for lesson length.
            LessonLengh = new LessonLengh();
            var lessonDiff  = _end.Subtract(_start);
            if (lessonDiff.TotalMinutes > 0)
                LessonLengh.SetLength((uint)lessonDiff.TotalMinutes / 90);
            else
                LessonLengh.SetLength(1);
        }

        public Lesson()
        {
            Discipline = "NULL";
        }

        public bool isNull()
        {
            if (Discipline == "NULL")
                return true;
            return false;
        }

        public static bool operator ==(Lesson left, Lesson right) {
            if (
                left.Type == right.Type &&
                left.Discipline ==right.Discipline &&
                left.WeekDay==right.WeekDay && 
                left.AcademicGroup==right.AcademicGroup &&
                left.WeekSpan==right.WeekSpan
                )
            {
                return true;
            }
            return false;
        }
        
        public static bool operator !=(Lesson left, Lesson right) {
            if (
                left.Type != right.Type ||
                left.Discipline !=right.Discipline ||
                left.WeekDay!=right.WeekDay ||
                left.AcademicGroup != right.AcademicGroup ||
                left.WeekSpan != right.WeekSpan
                )
            {
                return true;
            }
            return false;
        }
    }

    
    public class UserSchedule
    {
        private (Lesson, Lesson)[,] Schedule;
        private List<(string, uint)> WeekDayLookup; 
        private List<(TimeSpan, uint)> TimeLookup;
        private bool BusyOnWeekend;
        public TimeSpan LatestClass;

        public UserSchedule()
        {
            //initializing the matrix with NULL instances of scheduleCells.
            Schedule = new (Lesson, Lesson)[6, 7];
            for(int row=0; row < 6; row++)
            {
                for(int col=0; col < 7; col++)
                {
                    Schedule[row, col] = (new Lesson(), new Lesson());
                }
            }
        
            BusyOnWeekend= false;
            LatestClass = new TimeSpan(0, 0, 0);
            
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

        //ADD SIMILAR LOGIC FOR REMOVING LESSONS
        private void __update_latest_lesson(TimeSpan endTime)
        {
            if (TimeSpan.Compare(endTime, LatestClass)==1)
            {
                LatestClass = endTime;
            }
        }

        public bool existingLesson(TimeSpan startTime, uint lessonNr)
        {
            if (__lookupTime(startTime) > 6 || lessonNr > 6)
                return false;

            if (
                Schedule[lessonNr, __lookupTime(startTime)].Item1.Discipline != "NULL" ||
                Schedule[lessonNr, __lookupTime(startTime)].Item2.Discipline != "NULL"
                )
                return true;

            return false;
        }

        public void addLesson(Lesson lesson)
        {
            //if full add to both cells
            if (lesson.WeekSpan == LessonWeekType.FULL) { 
                Schedule[__lookupWeekday(lesson.WeekDay), __lookupTime(lesson.StartTime)].Item1 = lesson;
                Schedule[__lookupWeekday(lesson.WeekDay), __lookupTime(lesson.StartTime)].Item2 = lesson;
            }
            else if(lesson.WeekSpan==LessonWeekType.EVEN)
                Schedule[__lookupWeekday(lesson.WeekDay), __lookupTime(lesson.StartTime)].Item1 = lesson;
            else if(lesson.WeekSpan==LessonWeekType.ODD)
                Schedule[__lookupWeekday(lesson.WeekDay), __lookupTime(lesson.StartTime)].Item2 = lesson;

            if (lesson.WeekDay == "sm")
                BusyOnWeekend = true;
            __update_latest_lesson(lesson.EndTime);
        }
        
        public Lesson getLessonEven(string weekday, uint lessonNr)
        {
            return Schedule[__lookupWeekday(weekday), lessonNr].Item1;
        } 
        public Lesson getLessonOdd(string weekday, uint lessonNr)
        {
            return Schedule[__lookupWeekday(weekday), lessonNr].Item2;
        }

        public Lesson getLessonEven(TimeSpan startTime, uint lessonNr)
        {
            return Schedule[lessonNr, __lookupTime(startTime)].Item1;
        }
        
        public Lesson getLessonOdd(TimeSpan startTime, uint lessonNr)
        {
            return Schedule[lessonNr, __lookupTime(startTime)].Item2;
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