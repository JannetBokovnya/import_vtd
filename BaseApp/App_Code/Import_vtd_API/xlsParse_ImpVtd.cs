using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;

    /// <summary>
    /// возможные типы импортов
    /// </summary>
    public class importType_ImpVtd
    {
        public string vtd = "vtd";
    }

    /// <summary>
    /// ПРЕДИСЛОВИЕ
    /// После долгих разбирательств, почему не стабильно работает excel  при вызове из Delphi,
    /// решено переписать парсер файлов на .Net т.к. был найден довольно простой  рабочий пример по работе с ним.
    /// 
    /// 
    /// </summary>
    public partial class oraWCFService_ImpVtd : IoraWCFService_ImpVtd
    {
        /// <summary>
        /// возможные статусы, используются в процессе  импорта
        /// </summary>
        public class statusEnum
        {
            public string D = "D";//done
            public string E = "E";//error
            public string W = "W";//warning
        }



        statusEnum _statusEnum = new statusEnum();
        /// <summary>
        /// структура описывающая данные о сопоставленных таблицах и листах
        /// </summary>
        protected struct TableNameStruct
        {
            public double nsheetkey;
            public string sheets_name;
            public string raw_table_name;
        }
        /// <summary>
        /// структура описывающая данные о сопоставленных столбцах
        /// </summary>
        protected struct columnNameStruct
        {
            /// <summary>
            /// название поля  на листе
            /// </summary>
            public string ccolname;
            /// <summary>
            /// тип поля
            /// </summary>
            public string cdatatype;
            /// <summary>
            /// может ли быть пустым
            /// </summary>
            public int canbenull;
            /// <summary>
            /// название поля в таблице БД
            /// </summary>
            public string cdbcolname;
        }


        /// <summary>
        /// получает список сопоставлений листов excel   таблицам оракла
        /// </summary>
        /// <param name="cImportType">название типа импорта</param>
        /// <param name="errMsg">сообщение об ошибке получения данных</param>
        /// <returns>возвращает датасет с одной таблице</returns>
        protected DataSet GetTableName(string cImportType, out string errMsg)
        {
            DataSet ds = new DataSet();
            errMsg = string.Empty;
            #region Old Code
            //OraInputParam[] oip = new OraInputParam[1];
            //oip[0].InputFieldName = "in_ctype";
            //oip[0].InputType = OracleType.VarChar;
            //oip[0].InputValue = cImportType;
            #endregion

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_ctype";
            oip[0].DbType = DBConn.DBTypeCustom.VarChar;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = cImportType;

            constructEngine(InportGetTableNameQuery, oip, GetConnectionString(), ref ds, true, out errMsg);
            return ds;
        }

        /// <summary>
        /// получаем список полей  таблицы по ключу названия листа excel
        /// </summary>
        /// <param name="in_sheetkey">ключ названия листа excel</param>
        /// <param name="errMsg">сообщение об ошибке получения данных</param>
        /// <returns>датасет</returns>
        protected DataSet GetColumns(double in_sheetkey, out string errMsg)
        {
            DataSet ds = new DataSet();
            errMsg = string.Empty;
            #region Old Code
            //OraInputParam[] oip = new OraInputParam[1];
            //oip[0].InputFieldName = "in_sheetkey";
            //oip[0].InputType = OracleType.Number;
            //oip[0].InputValue = in_sheetkey;
            #endregion

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_sheetkey";
            oip[0].DbType = DBConn.DBTypeCustom.Number;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = in_sheetkey;

            constructEngine(InportGetColumnsQuery, oip, GetConnectionString(), ref ds, true, out errMsg);
            return ds;
        }


        /// <summary>
        /// логирование  процесса импорта в базу
        /// </summary>
        /// <param name="impKey">ключ импорта</param>
        /// <param name="msg">сообщение</param>
        /// <param name="status">тип сообщения</param>
        /// <param name="errMsg">сообщение об ошибке получения данных</param>
        protected void LogMessage(double impKey, string msg, string status, out string errMsg)
        {
            DataSet ds = new DataSet();
            errMsg = string.Empty;
            #region Old Code
            //OraInputParam[] oip = new OraInputParam[3];
            //oip[0].InputFieldName = "in_nPK";
            //oip[0].InputType = OracleType.Number;
            //oip[0].InputValue = impKey;

            //oip[1].InputFieldName = "in_cMSG";
            //oip[1].InputType = OracleType.VarChar;
            //oip[1].InputValue = msg;

            //oip[2].InputFieldName = "in_cStatus";
            //oip[2].InputType = OracleType.VarChar;
            //oip[2].InputValue = status.ToString();
            #endregion

            DBConn.DBParam[] oip = new DBConn.DBParam[3];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nPK";
            oip[0].DbType = DBConn.DBTypeCustom.Number;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = impKey;

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_cMSG";
            oip[1].DbType = DBConn.DBTypeCustom.VarChar;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = msg;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_cStatus";
            oip[2].DbType = DBConn.DBTypeCustom.VarChar;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = status;

            constructEngine(InportWrite2LogQuery, oip, GetConnectionString(), ref ds, false, out errMsg);
        }


        public DataSet GetDataFromXLSSheet(string filePath, string SheetName, out string errMsg)
        {


            errMsg = string.Empty;
            DataSet ds = new DataSet();

            OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;");

            con.Open();
            try
            {
                OleDbDataAdapter odb = new OleDbDataAdapter("select * from [" + SheetName + "$]", con);
                odb.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
            finally
            {
                con.Dispose();
            }

            //Очистка пустых полученных строк 
            DataTable dt = ds.Tables[0].Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull)).CopyToDataTable();
            ds.Tables.Clear();
            ds.Tables.Add(dt);
            return ds;
        }

        //public bool importFile(double impKey, string typeOfImport, string fileName, out string errMsg)
        public StatusAnswer_ImpVtd ImportFile(double impKey, string typeOfImport, string fileName)
        {
            var result = new StatusAnswer_ImpVtd();
            result.IsValid = true;
            fileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\UploadedImportVTD\\" + fileName;
            string errMsg = string.Empty;
            string internalError = string.Empty;
            bool isFirstRow = true;
            StringBuilder templateColumnPartQuery = new StringBuilder();//содержит шаблонную часть  запроса с колонками

            #region проверяем, существует ли указанный файл
            if (!System.IO.File.Exists(fileName))
            {
                errMsg = string.Format("Файл {0} не найден", fileName);
                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = errMsg;
                return result;
            }
            else
            {
                LogMessage(impKey, string.Format("Файл {0} успешно найден", fileName), _statusEnum.D, out errMsg);
            }
            #endregion
            #region пытаемся получить  сопоставление таблиц БД с названиями листов excel
            DataSet tableNameDS = null;
            try
            {
                tableNameDS = GetTableName(typeOfImport.ToString(), out errMsg);
            }
            catch (Exception e)
            {
                errMsg = string.Format("Ошибка получения сопоставления названий таблиц, {0}", e.Message.ToString());
                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = errMsg;
                return result;
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                LogMessage(impKey, string.Format("Невозможно получить сопоставление названий таблиц,{0}", errMsg), _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = "Невозможно получить сопоставление названий таблиц";
                return result;
            }
            if (tableNameDS.Tables[0].Rows.Count == 0)
            {
                errMsg = "Не найдено сопоставление названий таблиц для текущего импорта";
                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = errMsg;
                return result;
            }
            else
                LogMessage(impKey, "Получено сопоставление названий таблиц для текущего импорта", _statusEnum.D, out internalError);

            #endregion
            #region полученные сопоставления  таблиц переносим во внутренние структуры
            List<TableNameStruct> tableNameList = new List<TableNameStruct>();
            IEnumerable<DataRow> query =
                from t in tableNameDS.Tables[0].AsEnumerable()
                select t;
            try
            {
                foreach (DataRow p in query)
                    tableNameList.Add(new TableNameStruct()
                    {
                        nsheetkey = Convert.ToDouble(p.Field<object>("nsheetkey")),
                        raw_table_name = p.Field<string>("raw_table_name"),
                        sheets_name = p.Field<string>("sheets_name")
                    });
            }
            catch (Exception e)
            {
                errMsg = string.Format("Ошибка получения данных о таблицах: {0}", e.Message);
                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = errMsg;
                return result;
            }
            #endregion
            #region завершаем работу метода,  если не удалось успешно перенести сопоставления во внутренние  структуры
            if (tableNameList.Count() == 0)
            {
                errMsg = "Ошибка преобразования сопоставленых названий таблиц";
                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = errMsg;
                return result; 
            }
            #endregion

            for (int i = 0; i < tableNameList.Count(); i++)
            {
                isFirstRow = true;
                //Thread.Sleep(5000);//by Gaitov
                #region получаем сопоставление полей  для текущей таблицы
                DataSet columnsNameDS = null;
                try
                {
                    columnsNameDS = GetColumns(tableNameList[i].nsheetkey, out errMsg);
                }
                catch (Exception e)
                {
                    errMsg = string.Format("Ошибка  получения полей для {0}. {1}", tableNameList[i].nsheetkey, e.Message.ToString());
                    LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                    result.IsValid = false;
                    result.ErrorMessage = errMsg;
                    return result;
                }
                #endregion
                #region проверяем удалось ли получить  сопставленные поля
                if (!string.IsNullOrEmpty(errMsg))
                {
                    LogMessage(impKey, string.Format("Не удалось получить сопоставление полей для листа {0} ", tableNameList[i].sheets_name), _statusEnum.E, out internalError);
                    result.IsValid = false;
                    result.ErrorMessage = errMsg;
                    return result;
                }
                #endregion
                #region полученные сопоставления колонок переносим во внутренние структуры
                List<columnNameStruct> columnNameList = new List<columnNameStruct>();
                IEnumerable<DataRow> queryColumns =
                from t in columnsNameDS.Tables[0].AsEnumerable()
                select t;
                try
                {
                    bool isAddComa = false;
                    templateColumnPartQuery = new StringBuilder();
                    templateColumnPartQuery.Append(" nImpKey, ");
                    foreach (DataRow p in queryColumns)
                    {
                        columnNameList.Add(new columnNameStruct()
                        {
                            canbenull = Convert.ToInt32(p.Field<object>("canbenull")),
                            ccolname = p.Field<string>("ccolname"),
                            cdatatype = p.Field<string>("cdatatype"),
                            cdbcolname = p.Field<string>("cdbcolname")
                        });
                        //формируем часть запроса с колонками для дальнейшего  использования
                        if (isAddComa)
                            templateColumnPartQuery.Append(", ");

                        templateColumnPartQuery.Append(p.Field<string>("cdbcolname"));
                        isAddComa = true;
                    }
                }
                catch (Exception e)
                {
                    errMsg = string.Format("Ошибка получения данных о таблицах: {0}", e.Message);
                    LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                    result.IsValid = false;
                    result.ErrorMessage = errMsg;
                    return result;
                }
                #endregion
                #region получаем данные с указанного  листка
                DataSet sheetWithDataDS = null;
                //т.к. excel имеет ограничение на кол-во данных на одном листе, то необходимо сделать
                //чтение много постранично для этого дабавлена переменная  multyPage
                //и все соответствующее

                int multyPage = 0;
                while (multyPage != -1)
                {
                    try
                    {
                        if (multyPage == 0)
                        {
                            //читаем настоящий лист
                            sheetWithDataDS = GetDataFromXLSSheet(fileName, tableNameList[i].sheets_name, out internalError);
                        }
                        else
                        {
                            try
                            {
                                //читаем  возможно существующий лист многостраничности
                                sheetWithDataDS = GetDataFromXLSSheet(fileName, tableNameList[i].sheets_name + "_" + multyPage.ToString(), out internalError);
                                isFirstRow = true;//должно здесь быть
                                if (sheetWithDataDS == null)
                                {
                                    multyPage = -1;
                                    break;
                                }
                                if (sheetWithDataDS.Tables[0].Rows.Count == 0)
                                {
                                    multyPage = -1;
                                    break;
                                }
                            }
                            catch
                            {
                                multyPage = -1;
                                break;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        LogMessage(impKey, string.Format("Ошибка чтения листа {0} в excel файле. {1}", tableNameList[i].sheets_name, e.Message.ToString()), _statusEnum.E, out internalError);
                        errMsg = e.Message.ToString();
                        result.IsValid = false;
                        result.ErrorMessage = errMsg;
                        return result;
                    }
                #endregion
                    #region проверяем удалось ли получить данные с указанного листка


                    if (sheetWithDataDS == null)
                    {
                        errMsg = string.Format("Ошибка формата файла, лист {0} не найден", tableNameList[i].sheets_name);
                        LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                        result.IsValid = false;
                        result.ErrorMessage = errMsg;
                        return result;
                    }


                    if (!string.IsNullOrEmpty(internalError) | sheetWithDataDS.Tables.Count == 0)
                    {
                        errMsg = string.Format("Ошибка формата файла, лист {0} не найден", tableNameList[i].sheets_name);
                        LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                        result.IsValid = false;
                        result.ErrorMessage = errMsg;
                        return result;
                    }
                    else
                    {
                        LogMessage(impKey, string.Format("Лист {0} успешно прочитан", (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString())), _statusEnum.D, out internalError);
                    }
                    #endregion
                    Hashtable checkColumnName = new Hashtable();
                    Hashtable columnData = new Hashtable();
                    int counter, currentRowNum = 0;
                    counter = currentRowNum = 0;

                    foreach (DataRow row in sheetWithDataDS.Tables[0].Rows)
                    {
                        columnData = new Hashtable();
                        foreach (DataColumn column in sheetWithDataDS.Tables[0].Columns)
                        {

                            if (isFirstRow)
                            {
                                #region читаем названия колонок
                                //string tmp = ClearString(row[column].ToString()).ToLower();
                                string tmp = ClearString(column.Caption.ToString()).ToLower();
                                if (checkColumnName.ContainsValue(tmp))
                                {
                                    errMsg = string.Format("Ошибка, для листа {0}, дублируются колонки {1}", (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString()), tmp);
                                    LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                                    result.IsValid = false;
                                    result.ErrorMessage = errMsg;
                                    return result;
                                }
                                //храним следующую связку
                                //key=индексу поля на листе(далее в цикле по индексу мы сможем определить к какому полю 
                                //относится текущее значение цикла
                                //value=обработанное название столбца ( обрабатываем т.к. выборка данных из excel 
                                //получается с некоторыми артефактами)
                                checkColumnName.Add(counter, tmp);
                                columnData.Add(counter, row[column].ToString());


                                counter++;
                                #endregion
                            }
                            else
                            {
                                columnData.Add(counter, row[column].ToString());
                                counter++;
                            }

                        }//end of foreach (DataColumn column in sheetWithDataDS.Tables[0].Columns)

                        counter = 0;//сбрасываем счетчик

                        if (isFirstRow)
                        {
                            #region проверяем наличие всех необходимых колонок на текущем листе
                            isFirstRow = false;

                            bool isFoundCriticalError = false;
                            for (int j = 0; j < columnNameList.Count(); j++)
                            {
                                string tmp = ClearString(columnNameList[j].ccolname).ToLower();
                                //if (checkColumnName[tmp] == null)
                                if (!checkColumnName.ContainsValue(tmp))
                                {
                                    if (columnNameList[j].canbenull == 0)
                                    {
                                        LogMessage(impKey, string.Format("Отсутствует  обязательная колонка {0} на листе {1}", columnNameList[j].ccolname, (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString())), _statusEnum.E, out internalError);
                                        isFoundCriticalError = true;
                                    }
                                    else if (columnNameList[j].canbenull == 1)
                                        LogMessage(impKey, string.Format("Отсутствует  необязательная колонка {0} на листе {1}", columnNameList[j].ccolname, (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString())), _statusEnum.W, out internalError);
                                }
                            }
                            if (isFoundCriticalError)
                            {
                                errMsg = string.Format("Отсутствует обязательная колонка на листе {0}, продолжение импорта невозможно", (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString()));
                                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                                result.IsValid = false;
                                result.ErrorMessage = errMsg;
                                return result;
                            }
                            #endregion
                        }
                        // else
                        // {
                        //TODO прочитали строку данных из листа и имеем маппинг названий полей с положением на листе
                        //надо генерировать  инсерты, но вначале проверить   то заполнены обязательные поля
                        /* сравниваем одновременно три массива
                         * 1. columnNameList- содержит маппинг из базы соответствий названий полей
                         * 2. checkColumnName- содержит key- индекс поля  на листке, value- обработанное название поля
                         * 3. columnData- содержит key- индекс поля, value значение
                         * пытаемся найти совпадения для обязательных полей  в следующем порядке
                         * columnNameList=checkColumnName по названию поля
                         * checkColumnName= columnData по индексу
                         */
                        bool isSkeepThisRow = true;
                        string columnNameNotFilled = string.Empty;
                        for (int j = 0; j < columnNameList.Count(); j++)
                        {
                            if (columnNameList[j].canbenull == 0)
                            {
                                isSkeepThisRow = true;
                                columnNameNotFilled = columnNameList[j].ccolname;
                                for (int k = 0; k < columnNameList.Count(); k++)
                                    if (ClearString(columnNameList[j].ccolname.ToLower()) == ClearString(checkColumnName[k].ToString().ToLower()))
                                    {
                                        if (ClearString(columnData[k].ToString()) != "")
                                        {
                                            isSkeepThisRow = false;
                                            columnNameNotFilled = string.Empty;
                                            break;
                                        }
                                        else
                                        {
                                            isSkeepThisRow = true;
                                            break;
                                        }
                                    }//if (ClearString(columnNameList[j].ccolname.ToLower()) == ClearString(checkColumnName[k].ToString().ToLower()))
                                if (isSkeepThisRow) break;
                            }
                        }

                        if (!isSkeepThisRow)
                        {
                            StringBuilder queryBulder = new StringBuilder();
                            queryBulder.Append("insert into ");
                            queryBulder.Append("db_api.dbo." + tableNameList[i].raw_table_name);
                            queryBulder.Append(" ( " + templateColumnPartQuery.ToString());
                            //for (int j = 0; j < columnNameList.Count(); j++)
                            //{
                            //    queryBulder.Append("," + columnNameList[j].cdbcolname);
                            //}
                            queryBulder.Append(" ) values ( " + impKey + " ");

                            for (int j = 0; j < columnNameList.Count(); j++)
                            {
                                //for (int k = 0; k < columnNameList.Count(); k++)//By Gaitov///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                for (int k = 0; k < checkColumnName.Count; k++)
                                    if (ClearString(columnNameList[j].ccolname.ToLower()) == ClearString(checkColumnName[k].ToString().ToLower()))
                                    {
                                        //TODO нужно проверять тип вставляемого объекта ( и если нужно добавлять ковычки
                                        //нужно написать  в базовом классе оракла, возможность вставки sql
                                        //потом вызывать вставку каждой строки\
                                        //не забыть про много страничноть
                                        try
                                        {
                                            switch (columnNameList[j].cdatatype.ToUpper())
                                            {
                                                case "N":
                                                    //queryBulder.Append("," + Convert.ToDouble(columnData[k]));
                                                    queryBulder.Append("," + (columnData[k] != "" ? columnData[k].ToString().Replace(" ", "").Replace(",", ".") : " null "));
                                                    break;
                                                case "I":
                                                    queryBulder.Append("," + (columnData[k] != "" ? columnData[k] : " null "));
                                                    break;
                                                case "D":
                                                    if (columnData[k].ToString() != "")
                                                    {
                                                        try
                                                        {
                                                            DateTime tmpDate = Convert.ToDateTime(columnData[k]);
                                                            if (columnData[k] != null)
                                                                queryBulder.Append(",convert(datetime, '" + tmpDate.Day.ToString() + "/" + tmpDate.Month.ToString() + "/" + tmpDate.Year.ToString() + "', 103) ");
                                                        }
                                                        catch (Exception e)
                                                        {
                                                            queryBulder.Append(", null");
                                                        }
                                                        
                                                    }
                                                    else
                                                        queryBulder.Append(", null");
                                                    break;

                                                case "S":
                                                    //queryBulder.Append(",'" + columnData[k] + "'");
                                                    if (columnData[k] != null)
                                                    { 
                                                        
                                                        string dataColumn = columnData[k].ToString().TrimStart(' ').TrimEnd(' ');
                                                        dataColumn = dataColumn.Replace("'", @"''");
                                                        //queryBulder.Append(",'" + columnData[k].ToString().TrimStart(' ').TrimEnd(' ') + "'");   
                                                        queryBulder.Append(",'" + dataColumn + "'");   
                                                    }
                                                     
                                                    else
                                                        queryBulder.Append(", null");
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            LogMessage(impKey, string.Format("На листе {0} в строке № {1}, неправильный формат данных. Строка будет пропущена" + e.Message,
                                                                   (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString()), 
                                                                   (currentRowNum + 2).ToString()), _statusEnum.E, out internalError);
                                           
                                        }

                                        break;
                                    }
                            }

                            queryBulder.Append(" ) ");
                            ExecNonQuery(queryBulder.ToString(), GetConnectionString());//отправляем строку данных
                            Thread.Sleep(5);
                        }
                        else
                        {
                            LogMessage(impKey, string.Format("На листе {0} в строке № {1}, не заполнена обязательная колонка {2}. Строка будет пропущена",
                                (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString()), (currentRowNum + 2).ToString(), columnNameNotFilled), _statusEnum.E, out internalError);
                            //сообщение о том, что будет пропущена строка, т.к. не заполнена  обязательная ячейка
                        }

                        // }

                        currentRowNum++;//держим индекс текущей строки

                    }//end of foreach (DataRow row in sheetWithDataDS.Tables[0].Rows)
                    multyPage++;
                }//end of while (multyPage != -1)
            }//end of for (int i = 0; i < tableNameList.Count(); i++)



            return result;
        }


        public string ClearString(string sourse)
        {
            string s = Regex.Replace(sourse, @"[^\w\.@-]", "").Replace(".", "").Replace("@", "").Replace("_", "").Replace("\n", "").Replace("Δ", "").Replace("δ", "");
            if (s.Length >= 57)
                s = s.Substring(0, 56);
            return s;


        }

        //public xlsParse_ImpVtd()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}
    }


