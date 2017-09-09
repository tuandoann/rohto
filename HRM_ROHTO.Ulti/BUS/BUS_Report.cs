using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRM_ROHTO.Util.DTO;
using System.Data.SqlClient;
using System.Data;

namespace HRM_ROHTO.Util.BUS
{
    public class BUS_Report : Base
    {
        public bool LoadRP_Coaching()
        {
            return base.fn_Get("pr_Report_Rate_Coaching");
        }

        public bool SearchRP_Coaching(DateTime FromDate, DateTime ToDate, Guid Coaching, int Course, int Module, int Rate, int Branch)
        {

            return base.fn_Get("pr_Report_Rate_Coaching_Fillter", new SqlParameter("@FromDate", FromDate)
                                                        , new SqlParameter("@ToDate", ToDate)
                                                        , new SqlParameter("Coaching", Coaching)
                                                        , new SqlParameter("Course", Course)
                                                        , new SqlParameter("Module", Module)
                                                        , new SqlParameter("@Rate", Rate)
                                                        , new SqlParameter("@Branch", Branch)

                );
        }

        public bool pr_Report_CoachingRating(Guid Coaching, int Course, int BranchID, int RateID)
        {

            return base.fn_Get("pr_Report_CoachingRating", new SqlParameter("@BranchID", BranchID)
                                                        , new SqlParameter("@RateID", RateID)
                                                        , new SqlParameter("CoachingID", Coaching)
                                                        , new SqlParameter("CourseID", Course));
        }

        public bool LoadRP_Student()
        {
            return base.fn_Get("pr_Report_Student_Register");
        }

        public bool SearchRP_Student(DateTime FromDate, DateTime ToDate, int Branch)
        {
            return base.fn_Get("pr_Report_Student_Register_Fillter", new SqlParameter("@FromDate", FromDate)
                                                                   , new SqlParameter("@ToDate", ToDate)
                                                                   , new SqlParameter("@Branch", Branch)
                );
        }

        public bool LoadRP_Student_MasterList()
        {
            return base.fn_Get("pr_Report_Student_MasterList");
        }

        public bool SearchRP_Student_MasterListFillter(DateTime FromDate, DateTime ToDate, int Branch)
        {
            return base.fn_Get("pr_Report_Student_MasterList_Fillter", new SqlParameter("@FromDate", FromDate)
                                                                   , new SqlParameter("@ToDate", ToDate)
                                                                   , new SqlParameter("@Branch", Branch)
                );
        }
        public double pr_Coaching_GetRatePoint(string CoachingID, int ModuleID)
        {
            double Point = 0;
            base.fn_Get("pr_Coaching_GetRatePoint", new SqlParameter("@CoachingID", CoachingID)
                                                         , new SqlParameter("@ModuleID", ModuleID));
            if (base.mn_Table.Rows.Count > 0)
            {
                double.TryParse(base.mn_Table.Rows[0][0].ToString(), out Point);
            }
            return Point;
        }

        //public bool GenerateTableRate(DataTable dt)
        //{
        //    string GenerateSql = "";
        //    GenerateSql += @"Create Table CoachingRate
        //                    (
        //                        Coaching nvarchar(200)

        //                    ";
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        GenerateSql += dt.Rows[i]["RateName"] + "int,";
        //    }
        //    return base.fn_Execute()
        //}

    }
}
