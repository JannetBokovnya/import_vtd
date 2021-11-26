using System.Runtime.Serialization;


/// <summary>
/// Базовый класс для "[DataContract]" классов, которым необходимо возвращать статус 
/// выполнения сервисной функции и сообщение об ошибке
/// </summary>

[DataContract]
public class StatusAnswer_ImpVtd
{
    [DataMember]
    public bool IsValid { get; set; }

    [DataMember]
    public string ErrorMessage { get; set; }

    #region Ctor

    public StatusAnswer_ImpVtd()
    {
        IsValid = true;
        ErrorMessage = "OK";
    }
    #endregion Ctor
}
