using eProiect.Domain.Entities.Schedule.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Academic
{
    public class OverlapClassGroup
    {
        public List<Class> OverlapGroup { get; set; }
        public OverlapClassGroup()
        {
            OverlapGroup = new List<Class>();
        }
    }
}
