using System;
using System.Data;
using System.Web.UI.WebControls;
using log4net;


    /// <summary>
    /// Summary description for OracleLayer
    /// </summary>
    [System.ComponentModel.DataObject]
    public class OracleLayer_ImpVtd : oracleEngine_ImpVtd//oraAdmin
    {
        /// <summary>
        /// Блок глобальных переменных
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(OracleLayer_ImpVtd).Name);

        /// <summary>
        /// Возвр количество участков ВТД
        /// </summary>
        /// <param name="ikey"></param>
        /// <returns></returns>
        public int GetVTDSec_Count(Double ikey)
        {
            int res = 0;
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nPipeKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = ikey
            };
            try
            {
                DbConnAuth dbConnAuth = new DbConnAuth();
                DBConn.Conn connOra = dbConnAuth.connOra();
                connOra.ConnectionString(ConStr);
                res = connOra.ExecuteQuery<int>(GetVtdSecCountQuery, oip);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return res;
        }

        public DataSet GetVTDSec_Param(double ikey, out string errStr)
        {
            DataSet ds = new DataSet();
            GetDataEngine(GetVtdSecParamQuery, ConStr, ref ds, ikey, "in_nvtdseckey", out errStr);
            return ds;
        }

        public DataSet GetVTD_Data_Params(double ikey, out string errStr)
        {
            DataSet ds = new DataSet();
            GetDataEngine(GetVtdDataParamsQuery, ConStr, ref ds, ikey, "in_nVTDDataKey", out errStr);
            return ds;
        }

        public void Load_VTDXLSfile(double ikey)
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nImpMakingKey",
                DbType = DBConn.DBTypeCustom.String,
                Value = ikey.ToString().Replace(".", "").Replace(",", "")
            };
            try
            {
                DbConnAuth dbConnAuth = new DbConnAuth();
                DBConn.Conn connOra = dbConnAuth.connOra();
                connOra.ConnectionString(ConStr);
                connOra.ExecuteNonQuery(LoadVtdxlSfileQuery, oip);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        /// <summary>
        /// возвращает список МГ
        /// </summary>
        /// <returns></returns>
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public DataSet GetMg(out string errMsg)
        {
            DataSet ds = new DataSet();
            GetDataEngine(GetMgQuery, ConStr, ref ds, out errMsg);
            return ds;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public DataSet GetDictDrfect(out string errMsg)
        {
            DataSet ds = new DataSet();
            GetDataEngine(GetDbDefectListQuery, ConStr, ref ds, out errMsg);
            return ds;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public DataSet GetDictDefectFile(double keyImp, out string errStr)
        {
            DataSet ds = new DataSet();
            GetDataEngine(GetDbDefectListFileQuery, ConStr, ref ds, keyImp, "in_nImpKey", out errStr);
            return ds;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public DataSet GetDictDefectMaping(double keyImp, out string errStr)
        {
            DataSet ds = new DataSet();
            GetDataEngine(GetDbDefectListMapingQuery, ConStr, ref ds, keyImp, "in_nImpKey", out errStr);
            return ds;
        }

        public void DeleteVtdImport(double inId, out string errStr)
        {
            GetDataEngine(DeleteVtdImportQuery, ConStr, inId, "in_nImp_Making_key", out errStr);
        }
        
        /// <summary>
        /// Возвращает список начатых процедур импорта ВТД
        /// </summary>
        /// <returns></returns>
        public DataSet GetImpVTD_Making_One(double ikey, out string errMsg)
        {
            DataSet ds = new DataSet();
            GetDataEngine(GetImpVtdMakingOneQuery, ConStr, ref ds, ikey, "in_nImpMakingKey", out errMsg);
            return ds;
        }

        /// <summary>
        /// Получаем атрибуты незавершенного импорта ВТД для его продолжения.
        /// </summary>
        /// <returns></returns>
        public DataSet Get_Vtd_Imp_Info(double ikey, out string errMsg)
        {
            DataSet ds = new DataSet();
            GetDataEngine(GetVtdImpInfoQuery, ConStr, ref ds, ikey, "in_vtdImpKey", out errMsg);
            return ds;
        }

        //////////////////////////////////////////////////////////////////////////////////
        ///////////ФОРМА СОПОСТАВЛЕНИЯ МАГНИТНЫХ АНОМАЛИЙ/////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Наполняем выпадающие списки, переданные "по ссылке"
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="ds"></param>
        /// <param name="textField"></param>
        /// <param name="valueField"></param>
        /// <param name="defaultText"></param>
        /// <param name="defaultValue"></param>
        public static void FillDdls(ref DropDownList ddl, DataSet ds, string textField, string valueField, string defaultText, string defaultValue)
        {
            ddl.Items.Clear();
            //проходим по всем строкам 
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string sValue = row[valueField].ToString();
                    string sText = row[textField].ToString();

                    ddl.Items.Add(new ListItem(sText, sValue));
                }
            }
            if (ddl.Items.FindByValue(defaultValue) == null)
            {
                ddl.Items.Add(new ListItem(defaultText, defaultValue));
            }

            ddl.SelectedValue = defaultValue;
        }

        /// <summary>
        /// Проверка, сопоставлени ли уже указанный тип магн. аномалии из файла импорта с типом аномалии в БД?
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public double IsMagnAnomTypeMapped(string query, string param, out string errMsg)
        {
            double res = 0;

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_cTypeName",
                DbType = DBConn.DBTypeCustom.VarChar,
                Value = param
            };

            constructEngine(query, oip, ConStr, ref res, true, out errMsg);

            return res;
        }

        /// <summary>
        /// Создание уникального имени файла
        /// </summary>
        /// <returns></returns>
        public static string GetFileName()
        {
            string fileName = "user" + Guid.NewGuid();
            return fileName;
        }

        /// <summary>
        /// Сопоставляем тип магнитной аномалии из файла импорта с тимпом аномалии в БД
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public double SaveMagnAnomTypeMapping(string query, string param, out string errMsg)
        {
            double res = 0;

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_cMappingInfo",
                DbType = DBConn.DBTypeCustom.VarChar,
                Value = param
            };

            constructEngine(query, oip, ConStr, ref res, true, out errMsg);

            return res;
        }
    }


