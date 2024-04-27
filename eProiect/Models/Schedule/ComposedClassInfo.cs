using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace eProiect.Models.Schedule
{
    public class ComposedClassInfo
    {
        public int UserDisciplineId { get; set; }
        public int TypeId {  get; set; }
        public int Year { get; set; }
        public int ClassroomId { get; set; }
        public int Frequency { get; set; }
        public int Span { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Day { get; set; }
    }
}