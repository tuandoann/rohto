using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models
{
    public partial class HRM_ROHTOEntities
    {
        public HRM_ROHTOEntities(string connnectionStr)
            : base(connnectionStr)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
    }
}