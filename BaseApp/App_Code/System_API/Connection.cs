using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for connection
/// </summary>
namespace App_Code.System_API
{
    public class Connection
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DataProvider).Name);
        private string con_meta = WebConfig.GetDBConnection();

        public int CheckAccess(string moduleName)
        {
            int access = 0;
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            //подстановка входного параметра
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "p_cModuleName";
            oip[0].DbType = DBConn.DBTypeCustom.String;
            oip[0].Value = moduleName;

            DbConnAuth dbConnAuth = new DbConnAuth();
            try
            {
                DBConn.Conn connOra = dbConnAuth.connOra();
                access = connOra.ExecuteQuery<int>("gis_meta_api.admin_api.checkmoduleaccess", oip);
            }
            catch (Exception e)
            {
                log.Error(e);
                access = 0;
            }
            return access;
        }
    }
}