using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class IDProofMaster
    {
        //Database Models
        public virtual int IDProofMaster_ID { get; set; }
        public virtual int? CodeTemplate { get; set; }
        public virtual int Ref_ID { get; set; }
        public virtual int IDProofType { get; set; }
        public virtual string IDProofData { get; set; }
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
        public virtual string CodeTemplateName { get; set; }
        public virtual string IDProofTypeName { get; set; }
        public virtual int ReturnValue { get; set; }


        public static bool Save(IDProofMaster addressMaster)
        {
            return new IDProofMasterService().SaveIDProofMaster(addressMaster);
        }
        public static bool Delete(int Id)
        {
            return new IDProofMasterService().DeleteIDProofMaster(Id);
        }
        public static IDProofMaster Get(int PrimaryKey, ISession session = null)
        {
            return new IDProofMasterService().GetIDProofMaster(PrimaryKey, session);
        }
        public static IDProofMaster GetNew()
        {
            return new IDProofMasterService().GetNewIDProofMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new IDProofMasterService().GetIDProofMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}
