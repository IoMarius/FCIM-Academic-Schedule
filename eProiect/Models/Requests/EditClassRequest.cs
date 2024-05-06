using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Models.Requests
{
    public class EditClassRequest
    {
        public int ClassId { get; set; }
        public int WeekdayId { get; set; }
        public int StartHours { get; set; }
        public int StartMinutes { get; set; }
        public int Span { get; set; }
        public int Frequency { get; set; }
        public int Floor { get; set; }
        public int ClassroomId { get; set; }
        public int UserDisciplineId { get; set; }
        public int ClassTypeId { get; set; }
        public int AcademicGroupId { get; set; }
        public int AcademicGroupYear { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is EditClassRequest other)
            {
                return ClassId == other.ClassId &&
                       WeekdayId == other.WeekdayId &&
                       StartHours == other.StartHours &&
                       StartMinutes == other.StartMinutes &&
                       Span == other.Span &&
                       Frequency == other.Frequency &&
                       Floor == other.Floor &&
                       ClassroomId == other.ClassroomId &&
                       UserDisciplineId == other.UserDisciplineId &&
                       ClassTypeId == other.ClassTypeId &&
                       AcademicGroupId == other.AcademicGroupId &&
                       AcademicGroupYear == other.AcademicGroupYear;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ClassId;
                hashCode = (hashCode * 397) ^ WeekdayId;
                hashCode = (hashCode * 397) ^ StartHours;
                hashCode = (hashCode * 397) ^ StartMinutes;
                hashCode = (hashCode * 397) ^ Span;
                hashCode = (hashCode * 397) ^ Frequency;
                hashCode = (hashCode * 397) ^ Floor;
                hashCode = (hashCode * 397) ^ ClassroomId;
                hashCode = (hashCode * 397) ^ UserDisciplineId;
                hashCode = (hashCode * 397) ^ ClassTypeId;
                hashCode = (hashCode * 397) ^ AcademicGroupId;
                hashCode = (hashCode * 397) ^ AcademicGroupYear;
                return hashCode;
            }
        }
    }
}