using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServicePro.BAL;
using NHibernate;
using System.Collections;

namespace ServicePro.BAL
{
    public class MasterControl
    {
        //Database Models
        public virtual int ModifierLineNo { get; set; }
        public virtual string ActivationMaster_Key { get; set; }
        public virtual string InstalledBy { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual int IsActive { get; set; }

        //Non-Database Models
        public virtual int ReturnValue { get; set; }



        public static bool Save(MasterControl masterControl)
        {
            return new MasterControlService().SaveMasterControl(masterControl);
        }
        public static bool Delete(string Id)
        {
            return new MasterControlService().DeleteMasterControl(Id);
        }
        public static MasterControl Get(string PrimaryKey,ISession session = null)
        {
            return new MasterControlService().GetMasterControl(PrimaryKey, session);
        }
        public static MasterControl GetNew()
        {
            return new MasterControlService().GetNewMasterControl();
        }
        public static IList LoginValidate(string UserId, string Password, int UserType)
        {
            return new MasterControlService().LoginValidate(UserId, Password, UserType);
        }
    }
}