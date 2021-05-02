using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using log4net;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ServicePro.BAL
{
    public class CommonUtil
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string ProductKeys = "TEST-KEYK-JEHO-VAH7,TEST-KEYB-JEHO-VAHJ";

        public static bool CheckProductKey(string a)
        {
            bool key = false;
            string[] keys = ProductKeys.ToString().Split(',').ToArray();
            foreach(var check in keys)
            {
                if(check == a)
                {
                    key = true;
                }
            }
            return key;
        }

        #region Log4Net
        public static void LogInfo(string info)
        {
            log.Info(info);
        }
        public static void LogError(object error)
        {
            log.Error(error);
        }
        public static void LogDebug(string debug)
        {
            log.Debug(debug);
        }
        #endregion

        public static DataTable ConvertToDataTable(string TableName,IList list)
        {
            DataTable dt = new DataTable(TableName);
            try
            {
                Hashtable hashColumn = (Hashtable)list[0];
                int columnCount = 0;
                foreach(string columnName in hashColumn.Keys)
                {
                    if(columnName == null)
                    {
                        dt.Columns.Add("Column"+columnCount);
                        columnCount++;
                    }
                    else
                    {
                        dt.Columns.Add(columnName);
                    }
                }
                dt.AcceptChanges();
                columnCount = 0;
                string TempColumnName; 
                for (int i = 0; i < list.Count; i++ )
                {
                    DataRow row = dt.NewRow();
                    Hashtable listTable = (Hashtable)list[i];
                    foreach (DictionaryEntry listrow in listTable)
                    {
                        
                        if (listrow.Key == null)
                        {
                            TempColumnName = "Column" + columnCount;
                            columnCount++;
                        }
                        else
                        {
                            TempColumnName = listrow.Key.ToString();
                        }
                        row[TempColumnName] = listrow.Value;
                    }
                    dt.Rows.Add(row);
                    dt.AcceptChanges();
                }
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex.Message);
            }
            return dt;
        }

        public static string Encrypt(string encryptString)
        {
            string EncryptionKey = "KESAVANVICKEY";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {  
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76  
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "KESAVANVICKEY";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {  
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76  
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}