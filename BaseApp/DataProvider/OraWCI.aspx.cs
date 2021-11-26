using System;
using DataProvider_API;
using DataProvider_API.Enums;

public partial class DataProvider_OraWCI : System.Web.UI.Page
{
    protected OraWCI OraWciParams;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Имя вызываемой процедуры/Функции
        string _cInSql = Request["inSQL"];
        // Имя вызывающего модуля. Для проверки валидности xml
        string moduleName = Request["inModuleName"];
        // Тип возвращаемых данных. 
        string contentType = Request["ContentType"] ?? "text/xml";
        // Проводить ли проверку валидности XML
        string isCheckXsd = Request["isCheckXsd"] ?? "1";
        
        // Параметры используются в старой версии Оравцай
        // Строка с вызывающими параметрами
        string cInParams = Request["inParams"];
        // Тип. Пока не ясно
        string inType = Request["inType"];
        // Компресс. Пока не ясно
        string isCompress = Request["isCompress"];

        OraWciParams = new OraWCI(_cInSql, moduleName, cInParams, inType, isCompress, contentType, isCheckXsd);
    }


    protected string GetContentType ()
    {
        string l_res = string.Empty;
         if (ContentType == null)
         {
             l_res = "text/xml";
         }
         else
         {
             l_res = ContentType;
         }
        return l_res;
    }


    protected byte[] GetMainResBinary()
    {
        byte[] l_res = null;
        string errMsg = string.Empty;
        string oraWciType = string.Empty;
        IEngine engine = null;
        engine = new EngineApplication();
       
        DataProvider dataProvider = new DataProvider(engine);

        SupportTypes.ResponseTypes returnType = SupportTypes.ResponseTypes.Xml;

        l_res = dataProvider.BuildResponse(OraWciParams, returnType, "");

        if (!string.IsNullOrEmpty(errMsg))
        {
            // l_res = getExption(errMsg);
        }
        
        return l_res;
    }

    


    private string getExption(string errMsg)
    {
        string l_res = string.Empty;
        l_res = "<ROWSET><Result>False</Result><R><ERROR>" + errMsg + "</ERROR></R></ROWSET>";
        return l_res;
    }
}