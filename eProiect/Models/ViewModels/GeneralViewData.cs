﻿using eProiect.Models.Schedule;
using eProiect.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Models.ViewModels
{
    public class GeneralViewData
    {
        public UserSchedule Schedule { get; set; }
        public UserEsentialData UData {  get; set; }
        public SelectedClassInfo ClassInfo { get; set; }
    }
}