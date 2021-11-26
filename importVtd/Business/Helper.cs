using importVtd.Resources;

namespace importVtd.Business
{
    public class Helper
    {
        public string GetData(string dataField)
        {
            string status = Resources_ImpVtd.cNoData;

            if (!string.IsNullOrEmpty(dataField))
            {
                status = dataField;
            }
            return status;
        }
    }
}
