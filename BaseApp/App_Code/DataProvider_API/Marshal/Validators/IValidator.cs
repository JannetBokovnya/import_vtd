namespace DataProvider_API.Marshal.Validators
{
    /// <summary>
    /// Class for validation TextFotmat (Xml, jSon, etc)
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Returns string with errors, if validation is failed
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        string CheckValid(string xml);
    }
}