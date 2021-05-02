using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class ItemReceivedHandler
    {
        //Database Models
        public virtual int ItemReceivedHandler_ID { get; set; }
        public virtual int ServiceItemMaster_ID { get; set; }
        public virtual int EmployeeMaster_ID { get; set; }
        public virtual int? CustomerMaster_ID { get; set; }
        public virtual string Comments { get; set; }
        public virtual DateTime ReceivedDateTime { get; set; }
        public virtual int IsActive { get; set; }
        public virtual string Created_UserID { get; set; }
        public virtual string Modified_UserID { get; set; }
        public virtual DateTime? Modified_DateTime { get; set; }
        public virtual string AddCol_1 { get; set; }
        public virtual string AddCol_2 { get; set; }
        public virtual string AddCol_3 { get; set; }
        public virtual string AddCol_4 { get; set; }
        public virtual string AddCol_5 { get; set; }

        //Non-Database Models
        public virtual int ReturnValue { get; set; }



        public static ItemReceivedHandler Save(ItemReceivedHandler itemReceivedHandler)
        {
            return new ItemReceivedHandlerService().SaveItemReceivedHandler(itemReceivedHandler);
        }
        public static bool Delete(int Id)
        {
            return new ItemReceivedHandlerService().DeleteItemReceivedHandler(Id);
        }
        public static ItemReceivedHandler Get(int PrimaryKey, ISession session = null)
        {
            return new ItemReceivedHandlerService().GetItemReceivedHandler(PrimaryKey, session);
        }
        public static ItemReceivedHandler GetNew()
        {
            return new ItemReceivedHandlerService().GetNewItemReceivedHandler();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new ItemReceivedHandlerService().GetItemReceivedHandlerGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new ItemReceivedHandlerService().GetItemReceivedHandlerGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}


