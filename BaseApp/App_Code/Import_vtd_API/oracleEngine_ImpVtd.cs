using System;
using System.Data;
using System.Web.Configuration;
using log4net;

/// <summary>
/// Summary description for oracleEngine
/// </summary>
public class oracleEngine_ImpVtd : oracleQuerys_ImpVtd
{
    /// <summary>
    /// строка подключения к мета данным
    /// </summary>
    protected readonly string ConStr = WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

    private static readonly ILog Log = LogManager.GetLogger(typeof(oracleEngine_ImpVtd).Name);

    /// <summary>
    /// выполнение запросов с произольным кол-вом входных параметров
    /// </summary>
    /// <param name="Query">запрос(серверный)</param>
    /// <param name="oip">список параметров</param>
    /// <param name="conString">строка подключения к бд</param>
    /// <param name="ds">сылка на датасет</param>
    /// <param name="needReturnValue">нужно ли возвращать значения</param>
    /// <param name="errMsg">ошибка выполнения</param>
    public void constructEngine(string Query, DBConn.DBParam[] oip, string conString, ref DataSet ds,
                                bool needReturnValue, out string errMsg)
    {
        errMsg = "";

        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            if (needReturnValue)
            {
                DataTable dt = connOra.ExecuteQuery<DataTable>(Query, oip);
                ds.Tables.Add(dt);
            }
            else
            {
                connOra.ExecuteNonQuery(Query, oip);
            }
                
        }
        catch(Exception ex)
        {
            Log.Error(ex);
            errMsg = ex.Message;
        }
    }

    /// <summary>
    /// выполнение запросов с произольным кол-вом входных параметров
    /// </summary>
    /// <param name="query">запрос(серверный)</param>
    /// <param name="oip">список параметров</param>
    /// <param name="conString">строка подключения к бд</param>
    /// <param name="needReturnValue">нужно ли возвращать значения</param>
    /// <param name="errMsg">ошибка выполнения</param>
    protected void constructEngine(string query, DBConn.DBParam[] oip, string conString, ref double res, bool needReturnValue, out string errMsg)
    {
        errMsg = "";

        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            if (needReturnValue)
            {
                res = connOra.ExecuteQuery<double>(query, oip);
            }
            else
            {
                connOra.ExecuteNonQuery(query, oip);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            throw;
        }
    }

    /// <summary>
    /// выполняет запросы не возвращающие значений insert, update, delete
    /// </summary>
    /// <param name="query">sql  строка</param>
    /// <param name="conString">строка подключения</param>
    /// <returns>текст ошибки</returns>
    protected string ExecNonQuery(string query, string conString)
    {

        string errMsg = "";
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(conString);
            connOra.ExecuteTextQuery<Int32>(query);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса. " + ex.Message;
        }
        return errMsg;
    }

    /// <summary>
    ///  выполняет  запросы с одним  входным числовым параметром
    /// </summary>
    /// <param name="query">запрос</param>
    /// <param name="conString">строка подключения</param>
    /// <param name="ds">ссылка на датасет</param>
    /// <param name="parametr">входной параметр</param>
    /// <param name="oracleParamName">название входного  параметра в оракле</param>
    /// <param name="errMsg"></param>
    protected void GetDataEngine(string query, string conString, ref DataSet ds, double parametr, string oracleParamName, out string errMsg)
    {
        errMsg = "";

        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam
        {
            ParameterName = oracleParamName,
            DbType = DBConn.DBTypeCustom.Double,
            Value = parametr
        };
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(conString);
            DataTable dt = connOra.ExecuteQuery<DataTable>(query, oip);
            ds.Tables.Add(dt);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса. " + ex.Message;
        }
    }


    /// <summary>
    /// выполнение запросов не возвращающих результат
    /// с входным числомым значением
    /// </summary>
    /// <param name="query"></param>
    /// <param name="conString"></param>
    /// <param name="parametr"></param>
    /// <param name="oracleParamName"></param>
    /// <param name="errMsg"></param>
    protected void GetDataEngine(string query, string conString, double parametr, string oracleParamName, out string errMsg)
    {
        errMsg = "";
        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam
        {
            ParameterName = oracleParamName,
            DbType = DBConn.DBTypeCustom.Double,
            Value = parametr
        };
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(conString);
            connOra.ExecuteNonQuery(query, oip);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса. " + ex.Message;
        }
    }

    protected void GetDataEngine(string query, string conString, ref DataSet ds, out string errMsg)
    {
        errMsg = "";
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(conString);
            DataTable dt = connOra.ExecuteQuery<DataTable>(query);
            ds.Tables.Add(dt);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса. " + ex.Message;
        }
    }
   
    /// <summary>
    /// return  connection string for  DB
    /// </summary>
    /// <returns></returns>
    protected string GetConnectionString()
    {
        return WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
    }
}