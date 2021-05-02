using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePro.BAL
{
    public class UserMaster
    {
        //Database Models
        public virtual int UserMaster_ID { get; set; }
        public virtual int EmployeeMaster_ID { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual int RollType { get; set; }
        public virtual string AddCol_1 { get; set; }
        public virtual string AddCol_2 { get; set; }
        public virtual string AddCol_3 { get; set; }
        public virtual string AddCol_4 { get; set; }
        public virtual string AddCol_5 { get; set; }
        public virtual int IsActive { get; set; }
        public virtual DateTime? Modified_DateTime { get; set; }

        //Non Database Models
        public virtual int ReturnValue { get; set; }
        public virtual string RollTypeName { get; set; }


        public static UserMaster Save(UserMaster userMaster)
        {
            return new UserMasterService().SaveUserMaster(userMaster);
        }
        public static bool Delete(int Id)
        {
            return new UserMasterService().DeleteUserMaster(Id);
        }
        public static UserMaster Get(int PrimaryKey, ISession session = null)
        {
            return new UserMasterService().GetUserMaster(PrimaryKey, session);
        }
        public static UserMaster GetNew()
        {
            return new UserMasterService().GetNewUserMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new UserMasterService().GetUserMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new UserMasterService().GetUserMasterGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}