 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProiect.Domain.Entities.Academic;

namespace eProiect.Domain.Entities.User
{
    public class UserSchedule
    {
        public (Lesson, Lesson)[,] Schedule;
        private readonly List<(string, uint)> WeekDayLookup;
        private readonly List<(TimeSpan, uint)> TimeLookup;
        private bool BusyOnWeekend;
        public TimeSpan LatestClass;

        public UserSchedule()
        {
            //initializing the matrix with NULL instances of scheduleCells.
            Schedule = new (Lesson, Lesson)[6, 7];
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    Schedule[row, col] = (new Lesson(), new Lesson());
                }
            }

            BusyOnWeekend = false;
            LatestClass = new TimeSpan(0, 0, 0);

            //initializing lookuptables
            WeekDayLookup = new List<(string, uint)>();
            TimeLookup = new List<(TimeSpan, uint)>();

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
            foreach (var interval in TimeLookup)
            {
                if (interval.Item1 == time)
                    return interval.Item2;
            }
            return 0;
        }

        private uint __lookupWeekday(string weekday)
        {
            foreach (var day in WeekDayLookup)
            {
                if (day.Item1 == weekday)
                    return day.Item2;
            }
            return 0;
        }

        //ADD SIMILAR LOGIC FOR REMOVING LESSONS
        private void __update_latest_lesson(TimeSpan endTime)
        {
            if (TimeSpan.Compare(endTime, LatestClass) == 1)
            {
                LatestClass = endTime;
            }
        }

        public bool ExistingLesson(TimeSpan startTime, uint lessonNr)
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

        public void AddLesson(Lesson lesson)
        {
            //if full add to both cells
            if (lesson.WeekSpan == ClassFrequency.WEEKLY)
            {
                Schedule[__lookupWeekday(lesson.WeekDay), __lookupTime(lesson.StartTime)].Item1 = lesson;
                Schedule[__lookupWeekday(lesson.WeekDay), __lookupTime(lesson.StartTime)].Item2 = lesson;
            }
            else if (lesson.WeekSpan == ClassFrequency.EVEN)
                Schedule[__lookupWeekday(lesson.WeekDay), __lookupTime(lesson.StartTime)].Item1 = lesson;
            else if (lesson.WeekSpan == ClassFrequency.ODD)
                Schedule[__lookupWeekday(lesson.WeekDay), __lookupTime(lesson.StartTime)].Item2 = lesson;

            if (lesson.WeekDay == "sm")
                BusyOnWeekend = true;
            __update_latest_lesson(lesson.EndTime);
        }

        public Lesson GetLessonEven(string weekday, uint lessonNr)
        {
            return Schedule[__lookupWeekday(weekday), lessonNr].Item1;
        }
        
        public Lesson GetLessonOdd(string weekday, uint lessonNr)
        {
            return Schedule[__lookupWeekday(weekday), lessonNr].Item2;
        }

        public Lesson GetLessonEven(TimeSpan startTime, uint lessonNr)
        {
            return Schedule[lessonNr, __lookupTime(startTime)].Item1;
        }

        public Lesson GetLessonOdd(TimeSpan startTime, uint lessonNr)
        {
            return Schedule[lessonNr, __lookupTime(startTime)].Item2;
        }

        public uint GetBusyDays()
        {
            if (!BusyOnWeekend)
            {
                return 5;
            }
            return 6;
        }

    }
}
