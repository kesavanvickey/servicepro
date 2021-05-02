using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class AddressMaster
    {
        //Database Models
        public virtual int AddressMaster_ID { get; set; }
        public virtual int? CodeTemplate { get; set; }
        public virtual int Ref_ID { get; set; }
        public virtual int AddressType { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual int? City { get; set; }
        public virtual int? State { get; set; }
        public virtual int? Country { get; set; }
        public virtual int? Pincode { get; set; }
        public virtual string ContactNo1 { get; set; }
        public virtual string ContactNo2 { get; set; }
        public virtual string Email { get; set; }
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
        public virtual string AddressTypeName { get; set; }
        public virtual string CityName { get; set; }
        public virtual string StateName { get; set; }
        public virtual string CountryName { get; set; }
        public virtual int ReturnValue { get; set; }
        public virtual DateTime? Created_DateTime { get; set; }


        public static bool Save(AddressMaster addressMaster)
        {
            return new AddressMasterService().SaveAddressMaster(addressMaster);
        }
        public static bool Delete(int Id)
        {
            return new AddressMasterService().DeleteAddressMaster(Id);
        }
        public static AddressMaster Get(int PrimaryKey, ISession session = null)
        {
            return new AddressMasterService().GetAddressMaster(PrimaryKey, session);
        }
        public static AddressMaster GetNew()
        {
            return new AddressMasterService().GetNewAddressMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new AddressMasterService().GetAddressMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }

    }
}
