using System.Data;

namespace DataProvider_API.Marshal
{
    /// <summary>
    /// Interface for class which converts DataTable to TextFormat (XML, jSon, etc)
    /// </summary>
    public interface IFormatConverter
    {
        /// <summary>
        /// Method gets DataTable, converts to TextFormat(XML, jSon), 
        /// checks its with schema and returns byte array
        /// </summary>
        /// <param name="dt">Receiving data</param>
        /// <param name="oraWciParams">OraWCI var for cheking with schema</param>
        /// <returns></returns>
        byte[] DoConvert(DataTable dt, OraWCI oraWciParams);
    }
}