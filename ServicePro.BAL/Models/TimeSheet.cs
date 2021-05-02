using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePro.BAL
{
    public class TimeSheet
    {
        //Database Models
        public virtual int TimeSheet_ID { get; set; }
        public virtual int EmployeeMaster_ID { get; set; }
        public virtual DateTime TimeSheet_Date { get; set; }
        public virtual DateTime CheckInDateTime { get; set; }
        public virtual DateTime? CheckOutDateTime { get; set; }
        public virtual string AddCol_1 { get; set; }
        public virtual string AddCol_2 { get; set; }
        public virtual string AddCol_3 { get; set; }
        public virtual string AddCol_4 { get; set; }
        public virtual string AddCol_5 { get; set; }
        public virtual int IsActive { get; set; }

        //Non Database Models
        public virtual int ReturnValue { get; set; }
        public virtual string EmployeeName { get; set; }


        public static bool Save(TimeSheet timeSheet)
        {
            return new TimeSheetService().SaveTimeSheet(timeSheet);
        }
        public static bool Delete(int Id)
        {
            return new TimeSheetService().DeleteTimeSheet(Id);
        }
        public static TimeSheet Get(int PrimaryKey, ISession session = null)
        {
            return new TimeSheetService().GetTimeSheet(PrimaryKey, session);
        }
        public static TimeSheet GetNew()
        {
            return new TimeSheetService().GetNewTimeSheet();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new TimeSheetService().GetTimeSheetGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new TimeSheetService().GetTimeSheetGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}