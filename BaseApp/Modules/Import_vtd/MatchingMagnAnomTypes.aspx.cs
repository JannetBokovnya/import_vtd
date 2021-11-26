using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Modules_Import_vtd_MatchingMagnAnomTypes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
        //    //Вычитываем из БД заведенные типы магнитных аномалий
        //    try
        //    {
        //        string errMsg = String.Empty;
        //        DataSet dsDbMagnAnomTypes = new OracleLayer_ImpVtd().GetDbMagnAnomTypes(oracleQuerys_ImpVtd.GetdbmagnanomlistQuery, out errMsg);
        //        if (!String.IsNullOrEmpty(errMsg))
        //        {
        //            ShowError("Ошибка получения типов магнитных аномалий из БД", Color.Red);
        //        }
        //        else
        //        {
        //            Session["dsDbMagnAnomTypes"] = dsDbMagnAnomTypes;
        //        }
        //        grdMagAnomTypes.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowError("Ошибка получения типов магнитных аномалий из БД: " + ex.Message, Color.Red);
        //    }
        //}
    }

    protected void btnLoadFile_Click(object sender, EventArgs e)
    {
        string strFileName = String.Empty;
        strFileName = FileField.PostedFile.FileName;

        if (!String.IsNullOrEmpty(strFileName))
        {

            if (System.IO.Path.GetExtension(strFileName).ToString().ToLower() != ".xls")
            {
                ShowError("Загруженный файл имеет неправильный формат, \n допускается загружать  файлы xls",
                          Color.Red);
                return;
            }

            //UploadDetails.Visible = true;
            string c = OracleLayer_ImpVtd.GetFileName() + System.IO.Path.GetExtension(strFileName);

            //System.IO.Path.GetFileName(strFileName);

            try
            {
                FileField.PostedFile.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Upload\\UploadedImportVTD\\" + c);
                ShowError("Файл успешно загружен", Color.Green);

                //lblWarning.Text = "Файл успешно загружен";
                //lblWarning.ForeColor = Color.Green;
            }
            catch (Exception exp)
            {
                ShowError("Произошла ошибка импорта файла: " + exp.Message, Color.Red);
            }

            //Получаем данные из Excel-файла
            string errStr = "";
            DataTable dt = GetDataFromXLSSheet(AppDomain.CurrentDomain.BaseDirectory + "Upload\\UploadedImportVTD\\" + c, "Дефекты", out errStr);
            if (!String.IsNullOrEmpty(errStr))
            {
                ShowError("Произошла ошибка чтения файла: " + errStr, Color.Red);
                return;
            }
            dt.Columns[0].ColumnName = "magnAnomType";

            //Привязываем данные из файла импорта к таблице на форме
            grdMagAnomTypes.DataSource = dt;
            grdMagAnomTypes.DataBind();
        }
        else
        {
            ShowError("Не выбран файл для загрузки...", Color.Red);
            return;
        }
        //Заполняем текстовые метки "Сопоставлено" и "Сопоставить"
        FillLables();
    }

    public void FillLables()
    {
        //Заполняем текстовые метки "Сопоставлено" и "Сопоставить"
        int notMatched = 0;
        foreach (GridViewRow row in grdMagAnomTypes.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if ((row.Cells[1].FindControl("ddlMagnAnomTypesFromDB") as DropDownList).SelectedValue == "-1")
                {
                    notMatched++;
                }
            }
        }

        lblMatched.Text = (grdMagAnomTypes.Rows.Count - notMatched).ToString();
        lblToMatch.Text = notMatched.ToString();
    }

    public DataTable GetDataFromXLSSheet(string filePath, string SheetName, out string errMsg)
    {
        errMsg = string.Empty;
        DataTable dt = new DataTable();

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0");
        con.Open();
        try
        {
            OleDbDataAdapter odb = new OleDbDataAdapter("select DISTINCT [Тип дефекта] from [" + SheetName + "$]", con);
            odb.Fill(dt);
            con.Close();
        }
        catch (Exception ex)
        {
            errMsg = ex.Message.ToString();
            return null;
        }
        finally
        {
            con.Dispose();
        }

        return dt;
    }

    private void ShowError(string Errmsg, Color color)
    {
        lblWarning.Text = Errmsg;
        lblWarning.Visible = true;
        lblWarning.ForeColor = color;
    }


    protected void grdMagAnomTypes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string errMsg = String.Empty;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Заполняем выпадающий список типами магнитных аномалий
            DropDownList ddlMagnAnomTypesFromDB = (DropDownList)e.Row.FindControl("ddlMagnAnomTypesFromDB");
            OracleLayer_ImpVtd.FillDdls(ref ddlMagnAnomTypesFromDB, Session["dsDbMagnAnomTypes"] as DataSet, "CNAME", "NKEY", "Выбрать", "-1");

            //Выставляем выпадающий список с типами магнитных аномалий в нужную позицию (если тип магнитной аномалии заведен в БД ИАС)
            try
            {
                double res = new OracleLayer_ImpVtd().IsMagnAnomTypeMapped(oracleQuerys_ImpVtd.IsMagnAnomTypeMappedQuery, e.Row.Cells[0].Text, out errMsg);
                if (res != 0)
                {
                    ddlMagnAnomTypesFromDB.SelectedValue = res.ToString();
                }
            }
            catch
            {

            }
        }
    }

    //Сохранить выбор пользователя в БД ИАС
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //Проверка: заполнен ли GridView типами магнитных аномалий для сопоставления?
        if (grdMagAnomTypes.Rows.Count != 0)
        {
            //Формируем строку для передачи в БД ИАС (для сохранения результатов операции)
            string inputString = String.Empty;
            foreach (GridViewRow row in grdMagAnomTypes.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    //Проверяем, все ли типы аномалий сопоставлены (все ли выпадающие списки - выбраны)?
                    if ((row.Cells[1].FindControl("ddlMagnAnomTypesFromDB") as DropDownList).SelectedValue != "-1")
                    {
                        inputString += row.Cells[0].Text + ":" + (row.Cells[1].FindControl("ddlMagnAnomTypesFromDB") as DropDownList).SelectedValue + ";";
                    }
                    else
                    {
                        ShowError("Не все типы магнитных аномалий сопоставлены...", Color.Red);
                        //Заполняем текстовые метки "Сопоставлено" и "Сопоставить"
                        FillLables();
                        return;
                    }
                }
            }
            //Удаляем последний символ сформированной строки ";"
            inputString = inputString.Substring(0, inputString.Length - 1);

            try
            {
                string errMsg = String.Empty;

                //Сохранение результатов в БД ИАС
                double res = new OracleLayer_ImpVtd().SaveMagnAnomTypeMapping(oracleQuerys_ImpVtd.SaveMagnAnomTypeMappingQuery, inputString, out errMsg);
                if (!String.IsNullOrEmpty(errMsg))
                {
                    ShowError("Ошибка: " + errMsg, Color.Red);
                }
                else
                {
                    if (res == -1)
                    {
                        ShowError("Ошибка сохранения данных", Color.Red);
                    }
                    else
                    {
                        ShowError("Данные успешно сохранены", Color.Green);
                        //Заполняем текстовые метки "Сопоставлено" и "Сопоставить"
                        FillLables();
                    }

                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка: " + ex.Message, Color.Red);
            }

        }
        else
        {
            ShowError("Нет типов магнитных аномалий для сопоставления...", Color.Red);
        }
    }
}