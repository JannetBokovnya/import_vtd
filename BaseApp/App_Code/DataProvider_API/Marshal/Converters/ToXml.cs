using System;
using System.Data;
using System.Text;
using DataProvider_API.Marshal.Validators;

namespace DataProvider_API.Marshal.Converters
{
    /// <summary>
    /// Summary description for ToXml
    /// </summary>
    public class ToXml : IFormatConverter
    {
        public byte[] DoConvert(DataTable dt, OraWCI oraWciParams)
        {
            byte[] bytes = null;
            DataSet dataSet = new DataSet();

            foreach (DataColumn c in dt.Columns)
                c.ColumnName = c.ColumnName.ToUpper();

            dataSet.DataSetName = "ROWSET";
            dataSet.Tables.Add(dt);

            string result = dataSet.GetXml().Replace("<ROWSET>\r\n", "<ROWSET>\r\n <Result>True</Result> \r\n ");

            if (oraWciParams.IsCheckXsd == 1)
            {
                IValidator validator = new XmlValidator(oraWciParams);
                string errMsg = validator.CheckValid(result);
                if (!String.IsNullOrEmpty(errMsg))
                {
                    result = "<ROWSET>\r\n <Result>False</Result> \r\n <R><ERROR>Результат не соответствует XSD\n"
                             + errMsg + "</ERROR></R></ROWSET>";
                } 
            }

            bytes  = Encoding.UTF8.GetBytes(result);

            return bytes;
        }
    }
}