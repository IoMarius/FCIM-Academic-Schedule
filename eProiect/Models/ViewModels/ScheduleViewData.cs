using eProiect.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Models.ViewModels
{
    public class ScheduleViewData
    {
        public UserSchedule Schedule { get; set; }
        public UserEsentialData UData {  get; set; } 
    }
}