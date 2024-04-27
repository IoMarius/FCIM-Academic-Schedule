using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Schedule;

namespace eProiect.Domain.Entities.Academic
{
    
    public class Lesson
    {
        public string Discipline { get; set; }
        public string ShortName { get; set; }
        public User.User LeadingUser { get; set; }
        public string GroupName { get; set; }
        public string Type { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string WeekDay { get; set; }
        public string Classroom { get; set; }
        public LessonLength LessonLength { get; set; }
        public ClassFrequency WeekSpan { get; set; }

        public Lesson(string _discipline, string _shortname, string _lessonType, string _weekday, string _classroom,
            string _group, TimeSpan _start, TimeSpan _end, ClassFrequency _weekSpan = ClassFrequency.WEEKLY)
        {
            Discipline = _discipline;
            ShortName = _shortname;
            Type = _lessonType;
            StartTime = _start;
            EndTime = _end;
            WeekDay = _weekday;
            Classroom = _classroom;
            GroupName = _group;
            WeekSpan = _weekSpan;

            //lojic for lesson length.
            LessonLength = new LessonLength();

            if (_end - _start == new TimeSpan(1, 30, 0))
            {
                //System.Diagnostics.Debug.WriteLine($"ONE<bl>{_classroom}>");
                LessonLength.SetLength(1);
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine($"TWO<bl>{_classroom}>");
                LessonLength.SetLength(2);
            }
        }

        public Lesson()
        {
            Discipline = "NULL";
            LessonLength = new LessonLength(1);
        }

        public bool IsNull()
        {
            if (Discipline == "NULL")
                return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is Lesson lesson &&
                   Classroom == lesson.Classroom;
        }

        public override int GetHashCode()
        {
            return -549559816 + EqualityComparer<string>.Default.GetHashCode(Classroom);
        }

        public static bool operator ==(Lesson left, Lesson right)
        {
            if (
                left.Type == right.Type &&
                left.ShortName == right.ShortName &&
                left.WeekDay == right.WeekDay &&
                left.GroupName == right.GroupName &&
                left.WeekSpan == right.WeekSpan
                )
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Lesson left, Lesson right)
        {
            if (
                left.Type != right.Type ||
                left.ShortName != right.ShortName ||
                left.WeekDay != right.WeekDay ||
                left.GroupName != right.GroupName ||
                left.WeekSpan != right.WeekSpan
                )
            {
                return true;
            }
            return false;
        }
    }
    
}
