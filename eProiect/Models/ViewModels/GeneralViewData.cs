
﻿using eProiect.Domain.Entities.Academic.DBModel;

﻿using eProiect.Domain.Entities.Academic;

using eProiect.Models.Schedule;
using eProiect.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eProiect.Domain.Entities.User.DBModel;

namespace eProiect.Models.ViewModels
{
    public class GeneralViewData
    {
        public UserSchedule Schedule { get; set; }
        public UserEsentialData UData {  get; set; }

        public List<UserEsentialData> UDataList { get; set; }
        public GroupOfDisciplines DisciplinesDataList { get; set; }
        public GroupOfCassRooms CassRoomsDataList { get; set; }
        public AcademicGroupsList AcademicGroupsDataList { get; set; }
        
        public List<UserDiscipline> UserDisciplinesDataList { get; set; }

    }
}