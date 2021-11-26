using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DataProvider_API.Marshal.Validators
{
    /// <summary>
    /// Summary description for XmlValidator
    /// </summary>
    public class JSonValidator : IValidator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (DataProvider).Name);
        private static readonly string FolderPath = AppDomain.CurrentDomain.BaseDirectory + "Modules\\";
        private string WebPath { get; set; }
        private string ErrMsg { get; set; }

        public JSonValidator(OraWCI oraWciParams)
        {
            string query = oraWciParams.InSQL;
            if (oraWciParams.InSQL.IndexOf("(") > 0)
                query = query.Remove(query.IndexOf("("));
            query = query.Substring(query.LastIndexOf(".") + 1);

            if (oraWciParams.ModuleName != null)
                WebPath = FolderPath + oraWciParams.ModuleName + "\\ClientBin\\Schema\\" + query + ".txt";
            else
            {
                Log.Info("В запросе к OraWCI отсутствует inModuleName");
            }
        }

        //Example schema:
        //{ 'type': 'array', 
        //  'items': { 'type': 'object', 
        //             'properties': {'nnotification': { 'type': 'integer', 'required': true }, 
        //                            'cmessage': { 'type': [ 'string', 'null' ], 'required': true },
        //                            'cappname': {  'type': [ 'string', 'null' ], 'required': true }, 
        //                            'cstate': { 'type': [ 'integer', 'null' ], 'required': true },
        //                            'dcreatedate': { 'type': [ 'integer', 'null' ], 'required': true }, 
        //                            'isdelivered': { 'type': [ 'integer', 'null' ], 'required': true } 
        //                           }
        //           },
        //}
        public string CheckValid(string xml)
        {
            string xsdInRes = GetJSonFromFile();

            if (!String.IsNullOrWhiteSpace(xsdInRes))
                IsValidJSonToSchema(xml, xsdInRes);


            return ErrMsg;
        }

        private string GetJSonFromFile()
        {
            //read txt file
            string line = String.Empty;
            if (File.Exists(WebPath))
                using (StreamReader sr = new StreamReader(WebPath))
                {
                    line = sr.ReadToEnd();
                }

            return line;
        }

        private bool IsValidJSonToSchema(string jSonStr, string jSonSchema)
        {
            JsonSchema schema;
            try
            {
                schema = JsonSchema.Parse(jSonSchema);
            }
            catch (Exception ex)
            {
                throw new Exception("OraValidation: Error parsing schema " + ex.Message);
            }
            
            JArray jSonObj;
            try
            {
                jSonObj = JArray.Parse(jSonStr);
            }
            catch (Exception ex)
            {
                throw new Exception("OraValidation: Error parsing json from db " + ex.Message);
            }

            IList<string> messages;
            bool isValid = jSonObj.IsValid(schema, out messages);

            ErrMsg = String.Empty;
            foreach (string message in messages)
                ErrMsg += message + "\n";

            return isValid;
        }
    }
}