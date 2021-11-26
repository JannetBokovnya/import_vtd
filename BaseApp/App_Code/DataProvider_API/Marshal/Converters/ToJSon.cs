using System;
using System.Data;
using System.Linq;
using System.Text;
using DataProvider_API.Marshal.Validators;
using Newtonsoft.Json;

namespace DataProvider_API.Marshal
{
    /// <summary>
    /// Summary description for ToXml
    /// </summary>
    public class ToJSon : IFormatConverter
    {
        public byte[] DoConvert(DataTable dt, OraWCI oraWciParams)
        {
            string jSonStr = JsonConvert.SerializeObject(dt, Formatting.None);

            if (oraWciParams.IsCheckXsd == 1)
            {
                IValidator validator = new JSonValidator(oraWciParams);
                string errMsg = validator.CheckValid(jSonStr);

                if (String.IsNullOrEmpty(errMsg))
                {
                    jSonStr = "{'Result': true,'" + dt.TableName + "':" + jSonStr + "}";
                }
                else
                {
                    jSonStr = errMsg.Aggregate("{'Result': false, Error:'", (current, message) => current + (message + "\n"));
                    jSonStr += "'}";
                }
            }

            return Encoding.UTF8.GetBytes(jSonStr); 
        }
    }
}