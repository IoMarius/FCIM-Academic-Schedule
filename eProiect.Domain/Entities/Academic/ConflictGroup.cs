using eProiect.Domain.Entities.Schedule.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Academic
{
    public class ConflictGroup
    {
        public Class MainClass { get; set; }
        public List<Class> OverlappingClasses { get; set; }
    }
}
