using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.ServiceModel.Activation;
using System.Data;
using System.Linq;
using System.Web;
using log4net;

// NOTE: If you change the class name "oraWCFService" here, you must also update the reference to "oraWCFService" in Web.config.
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public partial class oraWCFService_ImpVtd : oracleEngine_ImpVtd, IoraWCFService_ImpVtd
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(oraWCFService_ImpVtd).Name);

    #region Список всех импорта втд

    /// <summary>
    /// проверка работает импорт или нет
    /// </summary>
    /// <returns></returns>
    public JobStatus GetJobStatus()
    {
        var result = new JobStatus {JobStatus_result = new JobStatusResult()};
        return result;
    }

    public ImpVTD_Making Get_ImpVTD_Making_List()
    {
        var result = new ImpVTD_Making {ImpVTD_Making_List = new List<ImpVTD_Making_List>()};

        try
        {
            string errStr = "";

            DataTable dt = new DataTable();
            try
            {
                DBConn.DBParam[] oip = new DBConn.DBParam[0];

                DbConnAuth dbConnAuth = new DbConnAuth();
                DBConn.Conn connOra = dbConnAuth.connOra();
                connOra.ConnectionString(GetConnectionString());

                dt = connOra.ExecuteQuery<DataTable>(GetImpVtdMakingListQuery, oip);
            }
            catch (Exception ex)
            {
                errStr = ex.Message;
                Log.Error(ex);
            }
  
            if (!String.IsNullOrEmpty(errStr))
            {
                result.IsValid = false;
                result.ErrorMessage = "GetImpVTD_Making_List err" + errStr;
                return result;
            }

            if (dt != null)
            {
                IEnumerable<DataRow> query = from t in dt.AsEnumerable()
                                             select t;
                foreach (DataRow p in query)
                    result.ImpVTD_Making_List.Add(new ImpVTD_Making_List()
                        {
                            ccode = (p.Field<object>("cSection") != null ? p.Field<object>("cSection").ToString()
                                                                         : "<не указано>"),
                            CFILENAME = (p.Field<object>("CFILENAME") != null ? p.Field<object>("CFILENAME").ToString()
                                                                              : "<не указано>"),
                            cStateKey = (p.Field<object>("nstatekey") != null ? p.Field<object>("nstatekey").ToString()
                                                                              : "-1"),
                            cState1 = (p.Field<object>("cState") != null ? p.Field<object>("cState").ToString()
                                                                         : "<не указано>"),
                            dTime = (p.Field<object>("dTime") != null ? p.Field<object>("dTime").ToString()
                                                                      : "<не указано>"),
                            fio = (p.Field<object>("fio") != null ? p.Field<object>("fio").ToString() 
                                                                  : "<не указано>"),
                            NIMP_MAKING = (p.Field<object>("NIMP_MAKING") != null ? p.Field<object>("NIMP_MAKING").ToString()
                                                                                  : "<не указано>"),
                            userKey = (p.Field<object>("nUserKey") != null ? p.Field<object>("nUserKey").ToString() 
                                                                           : "<не указано>"),
                            NSTATEKEY = (p.Field<object>("NSTATEKEY") != null ? p.Field<object>("NSTATEKEY").ToString()
                                                                              : "<не указано>"),
                            cnumber_contract = (p.Field<object>("cnumber_contract") != null ? p.Field<object>("cnumber_contract").ToString()
                                                                                            : "<не указано>"),
                            ddate_contract = (p.Field<object>("ddate_contract") != null ? p.Field<object>("ddate_contract").ToString().Substring(0, 10)
                                                                                        : "<не указано>")
                        });
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = string.Format(" GetDDEImportListQuery: " + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    //Получаем атрибуты процедуры импорта по ключю.
    public ImpVTD_Making Get_ImpVTD_Making_One(string impMakingKey)
    {
        var result = new ImpVTD_Making {ImpVTD_Making_List = new List<ImpVTD_Making_List>()};

        try
        {
            string errStr;
            DataSet ds = new OracleLayer_ImpVtd().GetImpVTD_Making_One(Convert.ToDouble(impMakingKey), out errStr);

            if (!String.IsNullOrEmpty(errStr))
            {
                result.IsValid = false;
                result.ErrorMessage = "Get_ImpVTD_Making_One err" + errStr;
                return result;
            }

            IEnumerable<DataRow> query = from t in ds.Tables[0].AsEnumerable()
                                         select t;
            foreach (DataRow p in query)
                result.ImpVTD_Making_List.Add(new ImpVTD_Making_List()
                {
                    ccode = (p.Field<object>("ccode") != null ? p.Field<object>("ccode").ToString() 
                                                              : "<не указано>"),
                    CFILENAME = (p.Field<object>("CFILENAME") != null ? p.Field<object>("CFILENAME").ToString()
                                                                      : "<не указано>"),
                    cState = (p.Field<object>("cState") != null ? p.Field<object>("cState").ToString() 
                                                                : "<не указано>"),
                    dTime = (p.Field<object>("dTime") != null ? p.Field<object>("dTime").ToString() 
                                                              : "<не указано>"),
                    fio = (p.Field<object>("fio") != null ? p.Field<object>("fio").ToString() 
                                                          : "<не указано>"),
                    NIMP_MAKING = (p.Field<object>("NIMP_MAKING") != null ? p.Field<object>("NIMP_MAKING").ToString()
                                                                          : "<не указано>"),
                    NSTATEKEY = (p.Field<object>("NSTATEKEY") != null ? p.Field<object>("NSTATEKEY").ToString()
                                                                      : "<не указано>")
                });
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = string.Format(" Get_ImpVTD_Making_One: " + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    #endregion

    #region вкладка - увязка словарных значений

    public GetDBDefect GetDBDefect_List()
    {
        var result = new GetDBDefect {GetDBDefectList = new List<GetDBDefectList>()};
 
        try
        {
            string errStr;
            DataSet ds = new OracleLayer_ImpVtd().GetDictDrfect(out errStr);
            if(!String.IsNullOrEmpty(errStr))
                throw new Exception(errStr);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                IEnumerable<DataRow> query = from t in ds.Tables[0].AsEnumerable()
                                             select t;

                foreach (DataRow p in query)
                    result.GetDBDefectList.Add(new GetDBDefectList()
                    {
                        NKEYDEFECT = p.Field<object>("NKEY").ToString(),
                        CNAMEDEFECT = p.Field<object>("cdefname").ToString(),
                        CNAMEGROUPDEFECT = p.Field<object>("cgroupname").ToString()
                    });
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetDBDefect_List err:" + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    /// <summary>
    /// сптсок дефектов из базы
    /// </summary>
    /// <param name="keyImp"></param>
    /// <returns></returns>
    public GetDBDefectFile GetDBDefect_ListFile(string keyImp)
    {
        var result = new GetDBDefectFile {GetDBDefectListFile = new List<GetDBDefectListFile>()};

        try
        {
            double dkeyDefect;
            double.TryParse(keyImp, out dkeyDefect);
            string errStr;
            DataSet ds = new OracleLayer_ImpVtd().GetDictDefectFile(dkeyDefect, out errStr);
            if (!String.IsNullOrEmpty(errStr)) 
                throw new Exception(errStr); 

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow p in ds.Tables[0].Rows)
                    result.GetDBDefectListFile.Add(new GetDBDefectListFile()
                    {
                        CNAMEDEFECT = p.Field<object>("ctype").ToString(),
                    });
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetDBDefect_List err:" + ex.Message);
            Log.Error(ex);
        }
        return result;
    }

    //по ключу импорта получаем данные по импорту
    public InfoForNewImport GetInfoForNewImport(string keyImp)
    {
        var result = new InfoForNewImport {GetInfoNewImportVtdList = new List<InfoForNewImportVtd>()};

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_vtdImpKey",
                DbType = DBConn.DBTypeCustom.String,
                Value = keyImp
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());

            DataSet ds = connOra.ExecuteQuery(GetImpVtdInfoNewImportQuery, false, oip);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    result.GetInfoNewImportVtdList.Add(new InfoForNewImportVtd
                        {
                            keyContract = row["nContractKey"].ToString(),
                            keyPipe = row["nPipeKey"].ToString(),
                            keyMg = row["nMTKey"].ToString(),
                            keySection = row["nSectionKey"].ToString(),
                            nameFile = row["cFileName"].ToString(),
                            cNameVTG = row["cNameVTG"].ToString(),
                            dDateContract = GetData(row["dDateContract"].ToString()),
                            nKmBegin = ((row["nKmBegin"] as decimal?) ?? -1).ToString("F3"),
                            nKmEnd = ((row["nKmEnd"] as decimal?) ?? -1).ToString("F3"),
                            nLength = "-1",
                            cMainExecutor = row["cMainExecutor"].ToString(),
                            cNameWork = row["cNameWork"].ToString(),
                            cNumberContract = row["cNumberContract"].ToString(),
                            cSubExecutor = row["cSubExecutor"].ToString(),
                            сMTName = row["сMTName"].ToString(),

                        });

                    //cFileName        -- Название файла импорта
                    //nSectionKey      -- Ключ  участка ВТД
                    //cNameVTG,        -- название участка ВТД
                    //dDateContract,   -- дата подписания
                    //nKmBegin,        -- км. начала
                    //nKmEnd,          -- км. конца
                    //nLength,         -- длина
                    //cMainExecutor,   -- генеральный подрядчик
                    //cNameWork,       -- название комплекса работ
                    //nContractKey     -- Ключ договора
                    //cNumberContract, -- номер договора
                    //cSubExecutor     -- исполнитель работ
                    //nPipeKey         -- Ключ нити
                    //сPipeName        -- Нить
                    //nMTKey           -- Ключ газопровода
                    //сMTName          -- Газопровод
                }
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.Message;
            Log.Error(ex);
        }

        return result;

    }

    private static string GetData(string rowData)
    {
        string strData = "";
        strData = rowData;

        strData = strData.Substring(0, strData.Length - 8);

        return strData;

    }

    /// <summary>
    /// список сопоставленных дефектов
    /// </summary>
    /// <param name="keyImp"></param>
    /// <returns></returns>
    public GetDBDefectMaping GetDbDefectMaping(string keyImp)
    {
        var result = new GetDBDefectMaping {GetDBDefectMapingList = new List<GetDBDefectMapingList>()};

        try
        {
            double dkeyImp;
            double.TryParse(keyImp, out dkeyImp);
            string errStr;
            DataSet ds = new OracleLayer_ImpVtd().GetDictDefectMaping(dkeyImp, out errStr);
            if (!String.IsNullOrEmpty(errStr))
                throw new Exception(errStr);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow p in ds.Tables[0].Rows)
                    result.GetDBDefectMapingList.Add(new GetDBDefectMapingList()
                    {
                        nKeyMaping = p.Field<object>("npk").ToString(),
                        nKeyDefect = p.Field<object>("ndb_magn_anom_key").ToString(),
                        cDefectName = p.Field<object>("cdefname").ToString(),
                        cGroupDefectName = p.Field<object>("cgroupname").ToString(),
                        cFileDefectName = p.Field<object>("cfile_magn_anom_name").ToString(),
                    });
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetDBDefect_List err:" + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    public StatusAnswer_ImpVtd DeleteMapingDict(string keyMaps)
    {
        var result = new StatusAnswer_ImpVtd {IsValid = true};
        try
        {
            string[] arr = keyMaps.Split(',');
            foreach (string keyMap in arr)
            {
                double dkeyMap;
                double.TryParse(keyMap, out dkeyMap);

                DBConn.DBParam[] oip = new DBConn.DBParam[1];
                oip[0] = new DBConn.DBParam
                {
                    ParameterName = "in_nMapKey",
                    DbType = DBConn.DBTypeCustom.Double,
                    Value = dkeyMap
                };

                DbConnAuth dbConnAuth = new DbConnAuth();
                DBConn.Conn connOra = dbConnAuth.connOra();
                connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
                connOra.ExecuteNonQuery(DeleteMapDictsQuery, oip);
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = "Ошибка удаления словарей словарей" + ex.Message;
            Log.Error(ex);
        }

        return result;
    }

    public StatusAnswer_ImpVtd SaveMapingDict(List<GetDBDefectMapingList> dBDefectMapingList, string keyImp)
    {
        var result = new StatusAnswer_ImpVtd {IsValid = true};

        try
        {
            foreach (GetDBDefectMapingList item in dBDefectMapingList)
            {
                DBConn.DBParam[] oip = new DBConn.DBParam[3];

                oip[0] = new DBConn.DBParam
                {
                    ParameterName = "in_nDbDefectKey",
                    DbType = DBConn.DBTypeCustom.Number,
                    Direction = ParameterDirection.Input,
                    Value = Convert.ToDouble(item.nKeyDefect)
                };

                oip[1] = new DBConn.DBParam
                {
                    ParameterName = "in_cFileDefect",
                    DbType = DBConn.DBTypeCustom.VarChar,
                    Direction = ParameterDirection.Input,
                    Value = item.cFileDefectName
                };

                oip[2] = new DBConn.DBParam
                {
                    ParameterName = "in_nImpKey",
                    DbType = DBConn.DBTypeCustom.Number,
                    Direction = ParameterDirection.Input,
                    Value = Convert.ToDouble(keyImp)
                };

                DbConnAuth dbConnAuth = new DbConnAuth();
                DBConn.Conn connOra = dbConnAuth.connOra();
                connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
                connOra.ExecuteNonQuery(SaveMapDictsQuery, oip);
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = "Ошибка сохранения сопоставления словарей" + ex.Message;
            Log.Error(ex);
        }

        return result;
    }
    #endregion

    public DictDefectXSL_ImpVtd Get_DictDefectXSL(string fileName)
    {
        string filePath = AppDomain.CurrentDomain.BaseDirectory + "Upload\\UploadedImportVTD\\" + fileName;
        const string sheetName = "Дефекты";

        var result = new DictDefectXSL_ImpVtd {DictDefectXSL_List_ImpVtd = new List<DictDefectXSL_List_ImpVtd>()};

        DataTable dt = new DataTable();

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0");
        con.Open();
        try
        {
            OleDbDataAdapter odb = new OleDbDataAdapter("select DISTINCT [Тип дефекта] from [" + sheetName + "$]", con);
            odb.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                result.DictDefectXSL_List_ImpVtd.Add(
                    new DictDefectXSL_List_ImpVtd { CNameDict = dr[0].ToString() });
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ex.Message;

        }
        finally
        {
            con.Dispose();
        }

        return result;
    }

    #region IoraWCFService Get_MG_ImpVtd


    public MG_ImpVtd Get_MG_ImpVtd()
    {
        var result = new MG_ImpVtd {MG_List_ImpVtd = new List<MG_List_ImpVtd>()};

        try
        {
            string errStr;
            DataSet ds = new OracleLayer_ImpVtd().GetMg(out errStr);
            if (!string.IsNullOrEmpty(errStr))
            {
                result.IsValid = false;
                result.ErrorMessage = string.Format(" MG" + errStr);
                Log.Error(errStr);
            }
            else
            {
                foreach (DataRow p in ds.Tables[0].Rows)
                    result.MG_List_ImpVtd.Add(new MG_List_ImpVtd()
                        {
                            NMAIN_GAS_PIPELINE_KEY = p.Field<object>("NKEY").ToString(),
                            CNAME = p.Field<string>("CNAME")
                        });
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetMG err:" + ex.Message);
            Log.Error(ex);
        }

        return result;
    }
    #endregion


    /// <summary>
    /// возвращает нитки по ключу мг
    /// </summary>
    /// <param name="keyMg"></param>
    /// <returns></returns>
    public ThreadForMgImpVtd GetThreadsForMg(string keyMg)
    {
        var result = new ThreadForMgImpVtd
        {
            ThreadForMgListImpVtd = new List<ThreadForMgListImpVtd>
            {
                new ThreadForMgListImpVtd()
                {
                    ThreadKey = "-1",
                    Name = "<empty>",
                }
            }
        };

        try
        {

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nKey",
                DbType = DBConn.DBTypeCustom.String,
                Value = keyMg
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());

            DataSet ds = connOra.ExecuteQuery(GetThreadsForMgQuery, false, oip);

            foreach (DataRow p in ds.Tables[0].Rows)
                result.ThreadForMgListImpVtd.Add(new ThreadForMgListImpVtd()
                    {
                        ThreadKey = p.Field<object>("NKEY").ToString(),
                        Name = p.Field<string>("CNAME"),
                    });
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("getThreadsForMG err:" + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    /// <summary>
    /// Возвр количество участков ВТД по ключу нити
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public CountVtdSec GetCountVtdForThread(string key)
    {
        var result = new CountVtdSec {CountVtdResult = new CountVtd()};

        try
        {
            result.CountVtdResult.CountVtdSec = new OracleLayer_ImpVtd().GetVTDSec_Count(Convert.ToDouble(key)).ToString();
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
            Log.Error(ex);
        }

        return result;
    }


    /// <summary>
    /// список участков втд
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>

    public VtdSec GetVTDSec_List(string key)
    {
        var result = new VtdSec
        {
            VtdSecList = new List<VtdSecList>
            {
                new VtdSecList()
                {
                    VtdSectionKey = "-1",
                    NameRegion = "<empty>",
                }
            }
        };

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nPipeKey",
                DbType = DBConn.DBTypeCustom.String,
                Value = key
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());

            DataSet ds = connOra.ExecuteQuery(GetVtdSecListQuery, false, oip);

            foreach (DataRow p in ds.Tables[0].Rows)
                result.VtdSecList.Add(new VtdSecList()
                {
                    VtdSectionKey = p.Field<object>("NVTD_SECTION_KEY").ToString(),
                    NameRegion = p.Field<string>("CNAME_REGION"),
                });
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetVTDSec_List err:" + ex.Message);
            Log.Error(ex);
        }

        return result;
    }


    public VtdSecParam GetVTDSec_Param(string key)
    {
        var result = new VtdSecParam {VtdSecParamItem = new List<VtdSecParamItem>()};
   
        try
        {
            string errStr;
            DataSet ds = new OracleLayer_ImpVtd().GetVTDSec_Param(Convert.ToDouble(key), out errStr);
            if (!String.IsNullOrEmpty(errStr))
            {
                result.IsValid = false;
                result.ErrorMessage = string.Format("GetVTDSec_Param errStr =" + errStr);
                Log.Error(errStr);
                return result;
            }
            else
            {
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow p in ds.Tables[0].Rows)
                    {
                        //Конвертирование М в Км
                        result.VtdSecParamItem.Add(new VtdSecParamItem()
                        {
                            NameRegion = (p.Field<object>("CNAME_REGION") != null ? p.Field<object>("CNAME_REGION").ToString()
                                                                                    : "<не указано>"),

                            KmBeginReg = ((p.Field<object>("NKM_BEGIN_REG") as decimal?) ?? -1),

                            KmEndReg = ((p.Field<object>("NKM_END_REG") as decimal?) ?? -1)                                  
                        });
                    }
                }
                else
                {
                    result.IsValid = false;
                    result.ErrorMessage = string.Format("Данные не найдены, ключ = {0}", key);
                }
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetVTDSec_Param  :" + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    //список договоров
    public VtdNumberDogAllList GetVTD_data_AllList(string key)
    {
        var result = new VtdNumberDogAllList
        {
            VtdDataAllList = new List<VtdDataAllList>
            {
                new VtdDataAllList()
                {
                    NumberContract = "<empty>",
                    VtdDataKey = "-1",
                    ImpCount = "-1"
                }
            }
        };

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nVTDSecKey",
                DbType = DBConn.DBTypeCustom.String,
                Value = key
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());

            DataSet ds = connOra.ExecuteQuery(GetVtdDataListQuery, false, oip);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow p in ds.Tables[0].Rows)
                    result.VtdDataAllList.Add(new VtdDataAllList()
                                                {
                                                    NumberContract = p.Field<object>("cnumber_contract") != null ? p.Field<object>("cnumber_contract").ToString()
                                                                                                                 : "",
                                                    VtdDataKey = p.Field<object>("nvtd_data_key") != null ? p.Field<object>("nvtd_data_key").ToString()
                                                                                                          : "",
                                                    ImpCount = p.Field<object>("nImpCount") != null ? p.Field<object>("nImpCount").ToString()
                                                                                                    : ""
                                                });
            }
            else
            {
                result.IsValid = false;
                result.ErrorMessage = ("Нет данных GetVTD_data_List ");
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetVTD_data_List  :" + ex.Message);
            Log.Error(ex);
        }

        return result;
    }


    public VtdDogovorParams GetVTD_Data_Params(string key)
    {
        var result = new VtdDogovorParams {VtdDataParams = new List<VtdDataParams>()};

        try
        {
            string errStr;
            DataSet ds = new OracleLayer_ImpVtd().GetVTD_Data_Params(Convert.ToDouble(key), out errStr);
            if (!String.IsNullOrEmpty(errStr))
            {
                result.IsValid = false;
                result.ErrorMessage = string.Format(" GetVTD_Data_Params errStr = " + errStr);
                return result;
            }
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow p in ds.Tables[0].Rows)
                    result.VtdDataParams.Add(new VtdDataParams()
                        {
                            MainExecutor = (p.Field<object>("cMainExecutor") != null ? p.Field<object>("cMainExecutor").ToString()
                                                                                      : ""),
                            Namework = (p.Field<object>("CNAMEWORK") != null ? p.Field<object>("CNAMEWORK").ToString()
                                                                              : ""),
                            SubExecutor = (p.Field<object>("cSubExecutor") != null ? p.Field<object>("cSubExecutor").ToString()
                                                                                    : ""),
                            DateContract = (p.Field<object>("DDATE_CONTRACT") != null ? p.Field<object>("DDATE_CONTRACT").ToString()
                                                                                        : ""),
                        });
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetVTD_Data_Params  :" + ex.Message);
        }

        return result;
    }

    public VtdTubeBaza GetLeftTableData(string key)
    {
        var result = new VtdTubeBaza {ListTubeBaza = new List<TubeBaza>()};

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nVTDMakingKey",
                DbType = DBConn.DBTypeCustom.Number,
                Direction = ParameterDirection.Input,
                Value = key
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());

            DataSet ds = connOra.ExecuteQuery(FillLeftTableQuery, false, oip);

            foreach (DataRow p in ds.Tables[0].Rows)
                result.ListTubeBaza.Add(new TubeBaza()
                    {
                        PipeElementMontajKey = (p.Field<object>("nTEMKey") != null ? p.Field<object>("nTEMKey").ToString()
                                                                                       : "<не указано>"),
                        NumPipePart = (p.Field<object>("nnum_pipe_part") != null ? p.Field<object>("nnum_pipe_part").ToString()
                                                                                    : "<не указано>"),
                        LocKmBeg = (p.Field<object>("loc_km_beg") != null ? Math.Round(p.Field<decimal>("loc_km_beg"), 2).ToString()
                                                                            : "<не указано>"),
                        Length = (p.Field<object>("nLength") != null ? p.Field<object>("nLength").ToString()
                                                                      : "<не указано>"),
                        DepthPipe = (p.Field<object>("ndepth_pipe") != null ? p.Field<object>("ndepth_pipe").ToString()
                                                                              : "<не указано>"),
                        Type = (p.Field<object>("cType") != null ? p.Field<object>("cType").ToString() 
                                                                  : "<не указано>"),
                        Angle = (p.Field<object>("nAngle") != null ? p.Field<object>("nAngle").ToString()
                                                                    : "<не указано>"),
                        Count = (p.Field<object>("nCount") != null ? p.Field<object>("nCount").ToString()
                                                                    : "<не указано>"),
                        TypePipeKey = (p.Field<object>("ntypekey") != null ? p.Field<object>("ntypekey").ToString()
                                                                              : "<не указано>"),
                    });
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetLeftTableData  :" + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    public List<DefectListTube> GetLeftDefectListTube(string key)
    {
        oracleImport_ImpVtd oi = new oracleImport_ImpVtd();
        string errStr;
        DataSet ds = oi.FillLeftDefectListTube(key, out errStr);
        if (!String.IsNullOrEmpty(errStr))
            return null;

        return (from DataRow p in ds.Tables[0].Rows
                select new DefectListTube()
                {
                    LocNkm = (p.Field<object>("loc_nkm") != null ? p.Field<object>("loc_nkm").ToString() : ""), 
                    ClockwisePos = GetValueRow(p.Field<object>("ClockWisePos")), 
                    Depth = (p.Field<object>("Depth") != null ? p.Field<object>("Depth").ToString() : "<не указано>"), 
                    Length = GetValueRow(p.Field<object>("Length")), 
                    PipeWallThickness = (p.Field<object>("PipeWallThickness") != null ? p.Field<object>("PipeWallThickness").ToString() : "<не указано>"), 
                    Width = GetValueRow(p.Field<object>("Width")), 
                    PrevSeamDist = GetValueRow(p.Field<object>("NPREVSEAM_DIST")), 
                    PersentCorroz = (p.Field<object>("persentCorroz") != null ? p.Field<object>("persentCorroz").ToString() : ""), 
                    PipeElem = (p.Field<object>("s_pipe_elem") != null ? p.Field<object>("s_pipe_elem").ToString() : ""), 
                    DefectTypeKey = (p.Field<object>("NDEF_TYPE_KEY") != null ? p.Field<object>("NDEF_TYPE_KEY").ToString() : ""), 
                    Type = (p.Field<object>("cType") != null ? p.Field<object>("cType").ToString() : ""), 
                    DepthPos = (p.Field<object>("cDEPTH_POS") != null ? p.Field<object>("cDEPTH_POS").ToString() : "")
                }).ToList();
    }

    public List<DefectListTubeRight> GetRightDefectListTube(string key)
    {
        oracleImport_ImpVtd oi = new oracleImport_ImpVtd();
        string errStr;
        DataSet ds = oi.FillRightDefectListTube(key, out errStr);
        if (!String.IsNullOrEmpty(errStr))
            return null;

        return (from DataRow p in ds.Tables[0].Rows
                select new DefectListTubeRight()
                {
                    LocNkm = (p.Field<object>("loc_nkm") != null ? p.Field<object>("loc_nkm").ToString() : ""), 
                    ClockwisePos = GetValueRow(p.Field<object>("ClockWisePos")), 
                    Depth = (p.Field<object>("Depth") != null ? p.Field<object>("Depth").ToString() : "<не указано>"), 
                    Length = GetValueRow(p.Field<object>("Length")), 
                    PipewallThickness = (p.Field<object>("nthickness") != null ? p.Field<object>("nthickness").ToString() : "<не указано>"), 
                    Width = GetValueRow(p.Field<object>("Width")), PrevSeamDist = GetValueRow(p.Field<object>("NPREVSEAM_DIST")), 
                    PersentCorroz = (p.Field<object>("persentCorroz") != null ? p.Field<object>("persentCorroz").ToString() : ""), 
                    PipeElem = (p.Field<object>("NJOURKEY") != null ? p.Field<object>("NJOURKEY").ToString() : ""), 
                    DefectTypeKey = (p.Field<object>("ntype") != null ? p.Field<object>("ntype").ToString() : ""), 
                    Type = (p.Field<object>("cType") != null ? p.Field<object>("cType").ToString() : ""), 
                    DepthPos = (p.Field<object>("cDEPTH_POS") != null ? p.Field<object>("cDEPTH_POS").ToString() : "")
                }).ToList();
    }

    private static double GetValueRow(object valueRow)
    {
        if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
        {
            double result;
            if (double.TryParse(valueRow.ToString(), out result))
                return result;
        }

        return 0d;
    }

    public VtdTubeBaza GetRightTableData(string key)
    {
        var result = new VtdTubeBaza {ListTubeBaza = new List<TubeBaza>()};
        try
        {
            oracleImport_ImpVtd oi = new oracleImport_ImpVtd();
            string errStr;
            DataSet ds = oi.FillRightTable(Convert.ToDouble(key), out errStr);
            if (!String.IsNullOrEmpty(errStr))
                return null;

            foreach (DataRow p in ds.Tables[0].Rows)
                result.ListTubeBaza.Add(new TubeBaza()
                {
                    PipeElementMontajKey = (p.Field<object>("nrawkey") != null ? p.Field<object>("nrawkey").ToString() : "<не указано>"),
                    NumPipePart = (p.Field<object>("cpipeno") != null ? p.Field<object>("cpipeno").ToString() : "<не указано>"),
                    LocKmBeg = (p.Field<object>("nMTKm") != null ? p.Field<object>("nMTKm").ToString() : "<не указано>"),
                    Length = (p.Field<object>("nMTLenght") != null ? (GetValueRowDouble(p.Field<object>("nMTLenght"))).ToString() : "<не указано>"),
                    DepthPipe = (p.Field<object>("nthickness") != null ? (GetValueRowDouble(p.Field<object>("nthickness"))).ToString() : "<не указано>"),
                    Type = (p.Field<object>("ctype") != null ? p.Field<object>("ctype").ToString() : "<не указано>"),
                    TypeShort = (p.Field<object>("cTypeShort") != null ? p.Field<object>("cTypeShort").ToString() : "<не указано>"),
                    Angle = (p.Field<object>("nAngle") != null ? (GetValueRowDouble(p.Field<object>("nAngle"))).ToString() : "<не указано>"),
                    Count = (p.Field<object>("nDefCount") != null ? p.Field<object>("nDefCount").ToString()
                                                                  : "<не указано>"),
                    TypePipeKey = (p.Field<object>("ntype") != null ? p.Field<object>("ntype").ToString() : "<не указано>"),
                });
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetLeftTableData  :" + ex.Message);
            Log.Error(ex);
        }
        
        return result;
    }

    private static double GetValueRowDouble(object valueRow)
    {
        double result = 0;
        if (!DBNull.Value.Equals(valueRow))
        {
            if (Double.TryParse(valueRow.ToString(), out result))
                result =  Math.Round(result, 2);
        }

        return result;
    }

    public GetImportSecond IsOld_VTD_Available(string vtdMakingKey)
    {

        var result = new GetImportSecond {ImportSecond_Result = new ImportSecondResult(), IsValid = true};

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nVTDMakingKey",
                DbType = DBConn.DBTypeCustom.Double,
                Direction = ParameterDirection.Input,
                Value = vtdMakingKey
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            result.ImportSecond_Result.ImportSecondVTD = connOra.ExecuteQuery<int>(IsOldVtdAvailableQuery, oip);

            result.ErrorMessage = "isOld_VTD_AvailableQuery  =  " 
                                        + result.ImportSecond_Result.ImportSecondVTD;
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = "Ошибка определения повторного имполрта IsOld_VTD_Available err = " + ex.Message;
            Log.Error(ex);
        }

        return result;
    }

    public StatusAnswer_ImpVtd MapsDicts(string ikeyImport)
    {
        var result = new StatusAnswer_ImpVtd {IsValid = true};

        double newImportKey;
        double.TryParse(ikeyImport, out newImportKey);

        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam
        {
            ParameterName = "in_nVTDMakingKey",
            DbType = DBConn.DBTypeCustom.Double,
            Value = newImportKey
        };

        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            connOra.ExecuteNonQuery(VtdMapsDictsQuery, oip);
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = "Ошибка импорта связи словарей" + ex.Message;
            Log.Error(ex);
        }

        return result;
    }

    public StatusAnswer_ImpVtd VTD_MapsDicts(string ikeyImport)
    {
        var result = new StatusAnswer_ImpVtd {IsValid = true};

        double newImportKey;
        double.TryParse(ikeyImport, out newImportKey);

        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam
        {
            ParameterName = "in_nVTDMakingKey",
            DbType = DBConn.DBTypeCustom.Double,
            Value = newImportKey
        };
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            connOra.ExecuteNonQuery(VtdMapsDictsQuery, oip);
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = "Ошибка импорта связи словарей" + ex.Message;
            Log.Error(ex);
        }
        return result;
    }

    public List<VtdfileParamTable> Getvtdfile_param(string ikey)
    {
        string errStr;
        DataSet ds = new DataSet();

        GetDataEngine(GetvtdfileParamQuery, ConStr, ref ds, Convert.ToDouble(ikey), "in_nimpmakingkey", out errStr);

        if (!String.IsNullOrEmpty(errStr))
            return null;

        return (from DataRow p in ds.Tables[0].Rows
                select new VtdfileParamTable()
                {
                    ContractN = (p.Field<object>("ccontract_n") != null ? p.Field<object>("ccontract_n").ToString() : "<не указано>"), 
                    Fio = (p.Field<object>("cfio") != null ? p.Field<object>("cfio").ToString() : "<не указано>"), 
                    MainExecutor = (p.Field<object>("cMainExecutor") != null ? p.Field<object>("cMainExecutor").ToString() : "<не указано>"), 
                    Place = (p.Field<object>("cPlace") != null ? p.Field<object>("cPlace").ToString() : "<не указано>"), 
                    Position = (p.Field<object>("cposition") != null ? p.Field<object>("cposition").ToString() : "<не указано>"), 
                    SubExecutor = (p.Field<object>("cSubExecutor") != null ? p.Field<object>("cSubExecutor").ToString() : "<не указано>"), 
                    ContractDate = (p.Field<object>("dcontract_date") != null ? p.Field<object>("dcontract_date").ToString() : "<не указано>")
                }).ToList();
    }

    public string Load_VTDXLSfile(string tmp)
    {
        double newImportKey;
        double.TryParse(tmp, out newImportKey);
        OracleLayer_ImpVtd ol = new OracleLayer_ImpVtd();
        ol.Load_VTDXLSfile(newImportKey);
        return newImportKey.ToString();
    }

    public ImpLogVtd GetImpLog(string tmp)
    {
        var result = new ImpLogVtd {ImpLog_result = new ImpLog_result()};

        try
        {
            double newImportKey;
            double.TryParse(tmp, out newImportKey);
            string ret = string.Empty;

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nImp_Making_key",
                DbType = DBConn.DBTypeCustom.Double,
                Value = newImportKey
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            DataSet ds = connOra.ExecuteQuery(GetImpLogQuery, false, oip);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                {
                    ret += ds.Tables[0].Rows[i]["clogmsg"] + "\n";
                }

                result.ImpLog_result.ImpLog_result_ret = ret;
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
            Log.Error(ex);
        }

        return result;
    }

    public GetTubeJournal Get_TubeJournal(string keyImp)
    {
        var result = new GetTubeJournal {TubeJournalTableList = new List<TubeJournalTableList>()};
        try
        {
            Double newImportKey;
            Double.TryParse(keyImp, out newImportKey);

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nvtdmakingkey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = newImportKey
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            DataSet ds = connOra.ExecuteQuery(GetTubeJournalQuery, false, oip);

            foreach (DataRow p in ds.Tables[0].Rows)
                result.TubeJournalTableList.Add(new TubeJournalTableList
                    {
                        Type = (p.Field<object>("ctype") != null ? p.Field<object>("ctype").ToString() : "<не указано>"),
                        Coef = "",
                        Km = (p.Field<object>("nkm") != null ? p.Field<object>("nkm").ToString() : "<не указано>"),
                        MgKm = (p.Field<object>("nMTKm") != null ? p.Field<object>("nMTKm").ToString() : "<не указано>"),
                        SecNo = (p.Field<object>("cSecNo") != null ? p.Field<object>("cSecNo").ToString() : "<не указано>"),
                        Length = (p.Field<object>("nLength") != null ? p.Field<object>("nLength").ToString() : "<не указано>"),
                        DefCount = (p.Field<object>("nDefCount") != null ? p.Field<object>("nDefCount").ToString() : "<не указано>"),
                        MgLenght = (p.Field<object>("nMTLenght") != null ? p.Field<object>("nMTLenght").ToString() : "<не указано>"),
                        PipeNo = (p.Field<object>("cpipeno") != null ? p.Field<object>("cpipeno").ToString() : "<не указано>"),
                        RawKey = (p.Field<object>("nrawkey") != null ? p.Field<object>("nrawkey").ToString() : "<не указано>"),
                        Thickness = (p.Field<object>("nthickness") != null ? p.Field<object>("nthickness").ToString(): "<не указано>")
                     });
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("Get_TubeJournal err:" + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    public List<InfoForVtdImportTable> GetInfoForVtdImport(string tmp)
    {
        oracleImport_ImpVtd oi = new oracleImport_ImpVtd();
        double newImportKey;
        double.TryParse(tmp, out newImportKey);
        string errStr;
        DataSet ds = oi.GetInfoForVtdImport(newImportKey, out errStr);

        return (from DataRow p in ds.Tables[0].Rows
            select new InfoForVtdImportTable()
            {
                NameRegion = (p.Field<object>("CNAME_REGION") != null ? p.Field<object>("CNAME_REGION").ToString() : "<не указано>"),
                Mg = (p.Field<object>("mg") != null ? p.Field<object>("mg").ToString() : "<не указано>"), 
                KmEnd = (p.Field<object>("NKM_END") != null ? p.Field<object>("NKM_END").ToString() : "<не указано>"), 
                KmStart = (p.Field<object>("NKM_START") != null ? p.Field<object>("NKM_START").ToString() : "<не указано>"), 
                Length = (p.Field<object>("NLENGTH") != null ? p.Field<object>("NLENGTH").ToString() : "<не указано>"),
                Thread = (p.Field<object>("thread") != null ? p.Field<object>("thread").ToString() : "<не указано>")
            }).ToList();
    }

    /// <summary>
    /// статистика по импорту
    /// </summary>
    /// <param name="keyImport"></param>
    /// <returns></returns>
    public GetStatisticsTable StatisticsTable(string keyImport)
    {
        var result = new GetStatisticsTable {StatisticsTableList = new List<StatisticsTable>()};

        try
        {
            oracleImport_ImpVtd oi = new oracleImport_ImpVtd();
            Double newImportKey;
            Double.TryParse(keyImport, out newImportKey);
            string errStr;
            DataSet ds = oi.GetStatistics(newImportKey, out errStr);

            foreach (DataRow p in ds.Tables[0].Rows)
                result.StatisticsTableList.Add(new StatisticsTable()
                {
                    RepOnKmCount = (p.Field<object>("cRepOnKmCount") != null ? p.Field<object>("cRepOnKmCount").ToString()
                                                                             : "<не указано>"),
                    AvgCoefCount = (p.Field<object>("nAvgCoefCount") != null ? p.Field<object>("nAvgCoefCount").ToString()
                                                                             : "<не указано>"),
                    MagnAnomCount = (p.Field<object>("nMagnAnomCount") != null ? p.Field<object>("nMagnAnomCount").ToString()
                                                                               : "<не указано>"),
                    PipeElemCount = (p.Field<object>("nPipeElemCount") != null ? p.Field<object>("nPipeElemCount").ToString()
                                                                               : "<не указано>"),
                    UsedRepCount = (p.Field<object>("nUsedRepCount") != null ? p.Field<object>("nUsedRepCount").ToString()
                                                                             : "<не указано>"),
                    CorrosionDefect = (p.Field<object>("nCorrosionDefect") != null ? p.Field<object>("nCorrosionDefect").ToString()
                                                                                   : "<не указано>"),
                    FormDefect = (p.Field<object>("nFormDefect") != null ? p.Field<object>("nFormDefect").ToString()
                                                                         : "<не указано>"),
                    CracklikeDefect = (p.Field<object>("nCracklikeDefect") != null ? p.Field<object>("nCracklikeDefect").ToString()
                                                                                   : "<не указано>"),
                    StressCorrosionDefect = (p.Field<object>("nStressCorrosionDefect") != null ? p.Field<object>("nStressCorrosionDefect").ToString()
                                                                                               : "<не указано>"),
                    TransverseJointAnomaly = (p.Field<object>("nTransverseJointAnomaly") != null ? p.Field<object>("nTransverseJointAnomaly").ToString()
                                                                                                 : "<не указано>"),
                    AbnormalAnomaly = (p.Field<object>("nAbnormalAnomaly") != null ? p.Field<object>("nAbnormalAnomaly").ToString()
                                                                                   : "<не указано>"),
                    DefectsQty = (p.Field<object>("nDefectsQty") != null ? p.Field<object>("nDefectsQty").ToString()
                                                                         : "<не указано>")
                });
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = "Ошибка загрузки статистики " + ex.Message;
            Log.Error(ex);
        }
      
        return result;
    }

    //Новая реализация Импорта ВТД (посредством Job).
    public StatusAnswer_ImpVtd ImportVTDLaunches(string tmp)
    {
        var result = new StatusAnswer_ImpVtd {IsValid = true};

        try
        {
            double newImportKey;
            double.TryParse(tmp, out newImportKey);

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nVTDMakingKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = newImportKey
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            connOra.ExecuteNonQuery(LaunchVtdImportQuery, oip);
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = "Ошибка запуска импорта" + ex.Message;
            Log.Error(ex);
        }

        return result;
    }

    public string ImportDefects(string tmp)
    {
        oracleImport_ImpVtd oi = new oracleImport_ImpVtd();
        Double newImportKey;
        Double.TryParse(tmp, out newImportKey);
        string errStr;
        oi.ImportDefects(newImportKey, out errStr);
        return errStr;
    }

    public string ImportTubeJournal(string tmp)
    {
        oracleImport_ImpVtd oi = new oracleImport_ImpVtd();
        Double newImportKey;
        Double.TryParse(tmp, out newImportKey);
        string errStr;
        oi.ImportTubeJournal(newImportKey, out errStr);
        return errStr;
    }

    /// <summary>
    /// удаление импорта по ключу
    /// </summary>
    /// <param name="keyImport"></param>
    /// <returns></returns>
    public StatusAnswer_ImpVtd DeleteVtdImport(string keyImport)
    {
        var result = new StatusAnswer_ImpVtd {IsValid = true};

        try
        {
            OracleLayer_ImpVtd ol = new OracleLayer_ImpVtd();
            Double newImportKey;
            Double.TryParse(keyImport, out newImportKey);
            string errStr;
            ol.DeleteVtdImport(newImportKey, out errStr);

            if (!String.IsNullOrEmpty(errStr))
            {
                result.IsValid = false;
                result.ErrorMessage = errStr;
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = "Ошибка удаления импорта" + ex.Message;
            Log.Error(ex);
        }

        return result;
    }


    #region IoraWCFService Создание нового импорта
    /// <summary>
    /// версия сервисов
    /// </summary>
    /// <returns></returns>
    public ServiceVersion GetServiceVersion()
    {
        var result = new ServiceVersion {ServiceVersionResult = new ServiceVersionTxt()};

        try
        {
            result.ServiceVersionResult.ServiceVersionTxtVtd = "Version 2.0";
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
            throw;
        }
        
        return result;
    }

    /// <summary>
    /// возвращает ключ пользователя
    /// </summary>
    /// <returns></returns>
    public KeyUserVtd GetKeyUser()
    {
        var result = new KeyUserVtd {KeyUserResult = new KeyUser {KeyUserVtd = "1"}};

        try
        {
            string user = HttpContext.Current.Session["UserName"].ToString();
            //здесь запрос в базу для получения ключа пользователя

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_clogin",
                DbType = DBConn.DBTypeCustom.String,
                Value = user
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn dbconn = dbConnAuth.connOra();
            int keyuser = dbconn.ExecuteQuery<int>(Admin_mod_querys.GetUserIdQuery, oip);
            result.KeyUserResult.KeyUserVtd = keyuser.ToString();
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.Message;
            Log.Error(ex);
        }

        return result;
    }

    public KeyImpVtd Create_New_ImportVTD(string userKeyVar, string fileNameVar, string pathVar,
                                          string vtdDataKeyVar)
    {

        var result = new KeyImpVtd {KeyImpResult = new KeyImp()};

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[5];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nUserKey",
                Direction = ParameterDirection.Input,
                DbType = DBConn.DBTypeCustom.Double,
                Value = userKeyVar
            };

            oip[1] = new DBConn.DBParam
            {
                ParameterName = "in_cFileName",
                DbType = DBConn.DBTypeCustom.String,
                Value = fileNameVar
            };

            oip[2] = new DBConn.DBParam
            {
                ParameterName = "in_cPath",
                DbType = DBConn.DBTypeCustom.String,
                Value = pathVar
            };

            oip[3] = new DBConn.DBParam
            {
                ParameterName = "in_nVTDDataKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = vtdDataKeyVar
            };

            oip[4] = new DBConn.DBParam
            {
                ParameterName = "out_nRes",
                DbType = DBConn.DBTypeCustom.Double,
                Direction = ParameterDirection.Output
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
            DataSet ds = connOra.ExecuteQuery(CreateNewImportVtdQuery, false, oip);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["Name"].ToString() == "out_nRes")
                        result.KeyImpResult.KeyImpVtd = row["Value"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
            Log.Error(ex);
        }

        return result;
    }

    #endregion

    #region IoraWCFService GetDBRepers

    public StatusAnswer_ImpVtd ApplyKeyMap(string keyImp, string arrayKey)
    {
        var result = new StatusAnswer_ImpVtd {IsValid = true};

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nVTDMakingKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = keyImp
            };

            oip[1] = new DBConn.DBParam
            {
                ParameterName = "in_cKeyPairs",
                DbType = DBConn.DBTypeCustom.String,
                Value = arrayKey
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConStr);
            connOra.ExecuteNonQuery(ApplyKeyMapQuery, oip);
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("ApplyKeyMap err  = " + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    public GetDbRepers GetDbRepers(string keyImp)
    {
        var result = new GetDbRepers {DbRepersTableList = new List<DbRepersTableList>()};

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nImpMakingKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = keyImp
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());

            DataSet ds = connOra.ExecuteQuery(GetDbRepersQuery, false, oip);

            foreach (DataRow p in ds.Tables[0].Rows)
                result.DbRepersTableList.Add(new DbRepersTableList()
                {
                    Name = (p.Field<object>("cname") != null ? p.Field<object>("cname").ToString() : ""),
                    Filtertype = (p.Field<object>("nfiltertype") != null ? p.Field<object>("nfiltertype").ToString() : ""),
                    Km = (p.Field<object>("nkm") != null ? p.Field<object>("nkm").ToString() : ""),
                    ObjKey = (p.Field<object>("nobjkey") != null ? p.Field<object>("nobjkey").ToString() : ""),
                    EntityName = (p.Field<object>("cEntityName") != null ? p.Field<object>("cEntityName").ToString() : ""),
                    ObjType = (p.Field<object>("cObjType") != null ? p.Field<object>("cObjType").ToString() : ""),
                });
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetDBRepers err  = " + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    //возвращает реперы из файла
    public GetFileRepers GetFileRepers(string tmp)
    {
        var result = new GetFileRepers {FileRepersList = new List<FileRepersList>()};

        try
        {
            double newImportKey;
            double.TryParse(tmp, out newImportKey);

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nImpMakingKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = newImportKey
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());

            DataSet ds = connOra.ExecuteQuery(GetFileRepersQuery, false, oip);

            foreach (DataRow p in ds.Tables[0].Rows)
                result.FileRepersList.Add(new FileRepersList()
                    {
                        Desc = (p.Field<object>("cdesc") != null ? p.Field<object>("cdesc").ToString() : ""),
                        Label = (p.Field<object>("clabel") != null ? p.Field<object>("clabel").ToString() : ""),
                        Type = (p.Field<object>("ctype") != null ? p.Field<object>("ctype").ToString() : ""),
                        FilterType = (p.Field<object>("nfiltertype") != null ? p.Field<object>("nfiltertype").ToString() : ""),
                        Km = (p.Field<object>("nkm") != null ? p.Field<object>("nkm").ToString() : ""),
                        RawKey = (p.Field<object>("nrawkey") != null ? p.Field<object>("nrawkey").ToString() : ""),
                        SecCount = (p.Field<object>("nseccount") != null ? p.Field<object>("nseccount").ToString() : "")
                    });
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = "GetFileRepes, key =  " + tmp + ex.Message;
            Log.Error(ex);
        }
        return result;
    }

    #endregion

    #region IoraWCFService Load_VTDXLSfile

    public StatusAnswer_ImpVtd Load_VTDXLSfileTrue(string newImportKey, string fileName)
    {
        var result = new StatusAnswer_ImpVtd {IsValid = true};
        string errMsg = string.Empty;
        try
        {
            //importFile(Convert.ToDouble(newImportKey), "VTD",
            //                 AppDomain.CurrentDomain.BaseDirectory + "Upload\\UploadedImportVTD\\" + fileName, out errMsg);
            //xlsParse_ImpVtd parse = new xlsParse_ImpVtd();
            //parse.importFile(Convert.ToDouble(newImportKey), "VTD",
            //AppDomain.CurrentDomain.BaseDirectory + "Upload\\UploadedImportVTD\\" + fileName, out errMsg);
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = result.ErrorMessage = String.Format("Load_VTDXLSfileTrue: " + ex.Message + errMsg);
            Log.Error(ex);
        }

        return result;
    }

    #endregion

    #region IoraWCFService ImportTubeMatching

    /// <summary>
    /// удаление связей
    /// </summary>
    /// <param name="keyImport"></param>
    /// <param name="filekey"></param>
    /// <returns></returns>
    public RemoveBound RemoveBound(string keyImport, string filekey)
    {
        var result = new RemoveBound {RemoveBound_Result = new RemoveBoundResult()};

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nVTDMakingKey",
                DbType = DBConn.DBTypeCustom.Double,
                Direction = ParameterDirection.Input,
                Value = keyImport
            };

            oip[1] = new DBConn.DBParam
            {
                ParameterName = "in_nRawJourKey",
                DbType = DBConn.DBTypeCustom.String,
                Value = filekey
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());

            DataSet ds = connOra.ExecuteQuery(RollbackTemMappingQuery, false, oip);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string res = row["nRowCount"].ToString();
                    result.RemoveBound_Result.RemoveBoundVTD = Convert.ToInt32(res);
                }
            }

            result.ErrorMessage = "RemoveBound  =  " + result.RemoveBound_Result.RemoveBoundVTD;
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = "Ошибка удаления связи err = " + ex.Message;
            Log.Error(ex);
        }

        return result;
    }

    /// <summary>
    /// возвращает все увязанные трубные элементы
    /// </summary>
    /// <param name="vtdMakingKey"></param>
    /// <returns></returns>
    public KeyBoundResult GetTemMapping(string vtdMakingKey)
    {
        var result = new KeyBoundResult {GetKeyBoundList = new List<KeyBound>()};

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nVTDMakingKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = vtdMakingKey
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConStr);

            DataSet ds = connOra.ExecuteQuery(GetTemMappingOuery, false, oip);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    result.GetKeyBoundList.Add(new KeyBound()
                    {
                        KeyBD = row["nDbTemId"].ToString(),
                        KeyFile = row["nRawJourId"].ToString(),
                    });
                }
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("ImportTubeMatching DB_API.vtd_import_api.getTemMapping  in_nVTDMakingKey = "
                                        + vtdMakingKey + "  err = " + ex.Message);
            Log.Error(ex);
        }

        return result;
    }


    //public KeyBoundResult ImportTubeMatching(string in_nVTDMakingKey, string in_bMatchTable, int typeLink)
    /// <summary>
    /// сопоставление труб входные параметры: 1. если ручная увязка то пара ключей, 2 - автомат  - ничего не передаем кроме ключа импорта
    /// </summary>
    /// <param name="vtdMakingKey"></param>
    /// <param name="filekey"></param>
    /// <param name="dbKey"></param>
    /// <param name="typeLink"></param>
    /// <returns></returns>
    public KeyBoundResult ImportTubeMatching(string vtdMakingKey, string filekey, string dbKey, int typeLink)
    {
        var result = new KeyBoundResult
        {
            GetKeyBoundList = new List<KeyBound>(),
            ErrorMessage = ("ImportTubeMatching DB_API.vtd_import_api.addTemMapping  in_nVTDMakingKey = "
                            + vtdMakingKey + "  in_nRawJourKey =  " + filekey + "  in_nDbTemKey = " + dbKey)
        };

        DataSet ds = new DataSet();

        try
        {
            // in_nVTDMakingKey - ключ импорта
            // in_nRawJourKey - ключ ТЕМа из файла
            // in_nDbTemKey - ключ ТЕМа из БД
            switch (typeLink)
            {
                case 1:
                    {
                        DBConn.DBParam[] oip = new DBConn.DBParam[3];
                        oip[0] = new DBConn.DBParam
                        {
                            ParameterName = "in_nVTDMakingKey",
                            DbType = DBConn.DBTypeCustom.Double,
                            Value = vtdMakingKey
                        };

                        oip[1] = new DBConn.DBParam
                        {
                            ParameterName = "in_nRawJourKey",
                            DbType = DBConn.DBTypeCustom.String,
                            Value = filekey
                        };

                        oip[2] = new DBConn.DBParam
                        {
                            ParameterName = "in_nDbTemKey",
                            DbType = DBConn.DBTypeCustom.String,
                            Value = dbKey
                        };

                        DbConnAuth dbConnAuth = new DbConnAuth();
                        DBConn.Conn connOra = dbConnAuth.connOra();
                        connOra.ConnectionString(ConStr);

                        ds = connOra.ExecuteQuery(AddTemMappingQuery, false, oip);
                        break;
                    }
                case 2:
                    {
                        DBConn.DBParam[] oip = new DBConn.DBParam[2];
                        oip[0] = new DBConn.DBParam
                        {
                            ParameterName = "in_nVTDMakingKey",
                            DbType = DBConn.DBTypeCustom.Double,
                            Value = vtdMakingKey
                        };

                        oip[1] = new DBConn.DBParam
                        {
                            ParameterName = "out_cStopReason",
                            DbType = DBConn.DBTypeCustom.String,
                            Size = 1000,
                            Direction = ParameterDirection.Output
                        };

                        DbConnAuth dbConnAuth = new DbConnAuth();
                        DBConn.Conn connOra = dbConnAuth.connOra();
                        connOra.ConnectionString(ConStr);

                        ds = connOra.ExecuteQuery(RunTemMappingQuery, false, oip);

                        break;
                    }
                case 3:
                    {
                        DBConn.DBParam[] oip = new DBConn.DBParam[3];
                        oip[0] = new DBConn.DBParam
                        {
                            ParameterName = "in_nVTDMakingKey",
                            DbType = DBConn.DBTypeCustom.Double,
                            Value = vtdMakingKey
                        };

                        oip[1] = new DBConn.DBParam
                        {
                            ParameterName = "in_nRawJourKey",
                            DbType = DBConn.DBTypeCustom.Double,
                            Value = filekey
                        };

                        oip[2] = new DBConn.DBParam
                        {
                            ParameterName = "in_nDbTemKey",
                            DbType = DBConn.DBTypeCustom.Double,
                            Value = DBNull.Value
                        };

                        DbConnAuth dbConnAuth = new DbConnAuth();
                        DBConn.Conn connOra = dbConnAuth.connOra();
                        connOra.ConnectionString(ConStr);

                        ds = connOra.ExecuteQuery(AddTemMappingQuery, false, oip);
                        break;
                    }
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].TableName != "Parameters" && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    result.GetKeyBoundList.Add(new KeyBound()
                        {
                            KeyBD = row["nDbTemId"].ToString(),
                            KeyFile = row["nRawJourId"].ToString(),
                        });
                }
            }
            if (typeLink == 2)
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["Parameters"].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables["Parameters"].Rows)
                        if (row["Name"].ToString() == "out_cStopReason")
                            result.ErrorMessage = row["Value"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("ImportTubeMatching DB_API.vtd_import_api.addTemMapping  in_nVTDMakingKey = " + vtdMakingKey
                                       + "  in_nRawJourKey =  " + filekey + "  in_nDbTemKey = " + dbKey + "  err = " + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    #endregion

    //Получаем статус состояния Job(а).
    public Res_Job GetVtdJobStatus()
    {
        var result = new Res_Job {Res_Job_status = new Res_Job_status(), IsValid = true};

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[2];

            oip[0] = new DBConn.DBParam
            {
                ParameterName = "out_nVTDMakingKey",
                DbType = DBConn.DBTypeCustom.Double,
                Direction = ParameterDirection.Output
            };

            oip[1] = new DBConn.DBParam
            {
                ParameterName = "out_nReturn",
                DbType = DBConn.DBTypeCustom.Double,
                Direction = ParameterDirection.Output
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
            DataSet ds = connOra.ExecuteQuery(GetVtdJobStatusQuery, false, oip);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["Name"].ToString() == "out_nVTDMakingKey")
                    {
                        result.Res_Job_status.VTDMakingKey = row["Value"].ToString();
                    }
                    if (row["Name"].ToString() == "out_nReturn")
                    {
                        result.Res_Job_status.ReturnStatus = row["Value"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ("GetVTDJobStatus err:" + ex.Message);
            Log.Error(ex);
        }

        return result;
    }

    //Получаем атрибуты незавершенного импорта ВТД для его продолжения.
    public List<InfoForOneVtdImport> GetVtdImpInfo(string vtdImpKey)
    {
        string errStr;
        DataSet ds = new OracleLayer_ImpVtd().Get_Vtd_Imp_Info(Convert.ToDouble(vtdImpKey), out errStr);

        if (!string.IsNullOrEmpty(errStr))
            return null;

        return (from DataRow p in ds.Tables[0].Rows
            select new InfoForOneVtdImport()
            {
                cnamevtg = (p.Field<object>("cnamevtg") != null ? p.Field<object>("cnamevtg").ToString() : "<не указано>"), 
                ddatecontract = (p.Field<object>("ddatecontract") != null ? p.Field<object>("ddatecontract").ToString() : "<не указано>"), 
                nkmbegin = (p.Field<object>("nkmbegin") != null ? p.Field<object>("nkmbegin").ToString() : "<не указано>"), 
                nkmend = (p.Field<object>("nkmend") != null ? p.Field<object>("nkmend").ToString() : "<не указано>"), 
                nlength = (p.Field<object>("Length") != null ? p.Field<object>("Length").ToString() : "<не указано>"), 
                cMainExecutor = (p.Field<object>("cMainExecutor") != null ? p.Field<object>("cMainExecutor").ToString() : "<не указано>"), 
                CNAMEWORK = (p.Field<object>("CNAMEWORK") != null ? p.Field<object>("CNAMEWORK").ToString() : "<не указано>"), 
                cnumbercontract = (p.Field<object>("cnumbercontract") != null ? p.Field<object>("cnumbercontract").ToString() : "<не указано>"), 
                cSubExecutor = (p.Field<object>("cSubExecutor") != null ? p.Field<object>("cSubExecutor").ToString() : "<не указано>"), 
                сPipeName = (p.Field<object>("сPipeName") != null ? p.Field<object>("сPipeName").ToString() : "<не указано>"), 
                сMTName = (p.Field<object>("сMTName") != null ? p.Field<object>("сMTName").ToString() : "<не указано>")
            }).ToList();
    }
}