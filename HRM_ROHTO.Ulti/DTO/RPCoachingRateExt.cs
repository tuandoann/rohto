using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Util.DTO
{
    public class RPCoachingRate
    {
        public string Student { get; set; }
        public string Coaching { get; set; }
        public string Course { get; set; }
        public string Module { get; set; }
        public string IsPass { get; set; }
        public string Rate { get; set; }
        public string Comment { get; set; }
    }
}