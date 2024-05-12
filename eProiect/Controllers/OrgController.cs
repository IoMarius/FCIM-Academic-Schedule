using eProiect.BusinessLogic;
using eProiect.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eProiect.Controllers
{
    public class OrgController : Controller
    {
        protected readonly IOrg _organizational;

        public OrgController()
        {
            var bl = new BuissinesLogic();
            _organizational=bl.GetOrgBl();
        }

        //move all org stuff here...
    }
}