using System;
using System.Data;
using App_Code.Admin_module_API;
using log4net;

/// <summary>
/// Summary description for oracleImport
/// </summary>
public class oracleImport_ImpVtd : oracleEngine_ImpVtd
{
    /// <summary>
    /// Блок глобальных переменных
    /// </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof(Auth).Name);

    public DataSet FillRightDefectListTube(string key, out string errMsg)
    {
        DataSet ds = new DataSet();

        double vtdMakingKey = Convert.ToDouble(key);

        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam
        {
            ParameterName = "in_nVTDMakingKey",
            DbType = DBConn.DBTypeCustom.Number,
            Direction = ParameterDirection.Input,
            Value = vtdMakingKey
        };

        constructEngine(FillRightDefectListQuery, oip, GetConnectionString(), ref ds, true, out errMsg);
        return ds;
    }

    public DataSet FillLeftDefectListTube(string key, out string errMsg)
    {
        DataSet ds = new DataSet();
        double vtdMakingKey = Convert.ToDouble(key);

        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam
        {
            ParameterName = "in_nVTDMakingKey",
            DbType = DBConn.DBTypeCustom.Number,
            Direction = ParameterDirection.Input,
            Value = vtdMakingKey
        };

        constructEngine(FillLeftDefectListQuery, oip, GetConnectionString(), ref ds, true, out errMsg);
        return ds;
    }

    /// <summary>
    /// статистика по импорту втд
    /// </summary>
    /// <param name="inId"></param>
    /// <param name="errStr"></param>
    /// <returns></returns>
    public DataSet GetStatistics(double inId, out string errStr)
    {
        DataSet ds = new DataSet();
        GetDataEngine(GetStatisticsQuery, ConStr, ref ds, inId, "in_nVTDMakingKey", out errStr);
        return ds;
    }

    /// <summary>
    /// импорт трубного журнала ВТД
    /// </summary>
    /// <param name="inId"></param>
    /// <param name="errStr"></param>    
    public void ImportTubeJournal(double inId, out string errStr)
    {
        GetDataEngine(ImportTubeJournalQuery, ConStr, inId, "in_nVTDMakingKey", out errStr);
    }

    public void ImportDefects(double inId, out string errStr)
    {
        GetDataEngine(ImportDefectsQuery, ConStr, inId, "in_nVTDMakingKey", out errStr);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inKey"></param>
    /// <param name="errMsg"></param>
    /// <returns></returns>
    public DataSet GetInfoForVtdImport(double inKey, out string errMsg)
    {
        DataSet ds = new DataSet();
        errMsg = "";
        DBConn.DBParam[] oip = new DBConn.DBParam[2];
        oip[0] = new DBConn.DBParam
        {
            ParameterName = "inKey",
            DbType = DBConn.DBTypeCustom.Double,
            Direction = ParameterDirection.Input,
            Value = inKey
        };

        oip[1] = new DBConn.DBParam
        {
            ParameterName = "Err",
            DbType = DBConn.DBTypeCustom.String,
            Direction = ParameterDirection.Output,
            Size = 100,
            Value = errMsg
        };

        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            DataTable dt = connOra.ExecuteQuery<DataTable>(GetInfoForVtdImportQuery, oip);
            ds.Tables.Add(dt);

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса. " + ex.Message;
        }
        return ds;
    }

    public DataSet FillRightTable(double key, out string errMsg)
    {
        DataSet ds = new DataSet();

        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam
        {
            ParameterName = "in_nVTDMakingKey",
            DbType = DBConn.DBTypeCustom.Number,
            Direction = ParameterDirection.Input,
            Value = key
        };

        constructEngine(FillRightTableQuery, oip, GetConnectionString(), ref ds, true, out errMsg);
        return ds;
    }
}

