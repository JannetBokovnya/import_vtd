using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

// NOTE: If you change the interface name "IoraWCFService" here, you must also update the reference to "IoraWCFService" in Web.config.
[ServiceContract]
public interface IoraWCFService_ImpVtd
{
    [OperationContract]
    ImpVTD_Making Get_ImpVTD_Making_List();

    [OperationContract]
    GetDBDefect GetDBDefect_List();

    [OperationContract]
    InfoForNewImport GetInfoForNewImport(string keyImp);

    [OperationContract]
    GetDBDefectFile GetDBDefect_ListFile(string keyImp);

    [OperationContract]
    GetDBDefectMaping GetDbDefectMaping(string keyImp);

    [OperationContract]
    StatusAnswer_ImpVtd DeleteMapingDict(string keyMap);

    [OperationContract]
    StatusAnswer_ImpVtd SaveMapingDict(List<GetDBDefectMapingList> dBDefectMapingList, string keyImp);

    [OperationContract]
    ImpVTD_Making Get_ImpVTD_Making_One(string key);

    [OperationContract]
    MG_ImpVtd Get_MG_ImpVtd();

    [OperationContract]
    ThreadForMgImpVtd GetThreadsForMg(string key);

    [OperationContract]
    CountVtdSec GetCountVtdForThread(string key);

    [OperationContract]
    VtdSec GetVTDSec_List(string key);

    [OperationContract]
    VtdSecParam GetVTDSec_Param(string key);

    [OperationContract]
    VtdNumberDogAllList GetVTD_data_AllList(string key);

    [OperationContract]
    DictDefectXSL_ImpVtd Get_DictDefectXSL(string fileName);

    [OperationContract]
    VtdDogovorParams GetVTD_Data_Params(string key);

    [OperationContract]
    VtdTubeBaza GetLeftTableData(string key);

    [OperationContract]
    VtdTubeBaza GetRightTableData(string key);

    [OperationContract]
    List<DefectListTube> GetLeftDefectListTube(string key);

    [OperationContract]
    List<DefectListTubeRight> GetRightDefectListTube(string key);

    [OperationContract]
    StatusAnswer_ImpVtd VTD_MapsDicts(string ikeyImport);

    [OperationContract]
    StatusAnswer_ImpVtd MapsDicts(string ikeyImport);

    [OperationContract]
    List<VtdfileParamTable> Getvtdfile_param(string tmp);

    [OperationContract]
    string Load_VTDXLSfile(string tmp);

    [OperationContract]
    ImpLogVtd GetImpLog(string tmp);

    [OperationContract]
    StatusAnswer_ImpVtd ApplyKeyMap(string tmp, string arrayKey);

    [OperationContract]
    GetTubeJournal Get_TubeJournal(string keyImp);

    [OperationContract]
    List<InfoForVtdImportTable> GetInfoForVtdImport(string tmp);

    [OperationContract]
    GetStatisticsTable StatisticsTable(string keyImport);

    [OperationContract]
    StatusAnswer_ImpVtd ImportVTDLaunches(string tmp);

    [OperationContract]
    string ImportDefects(string tmp);

    [OperationContract]
    string ImportTubeJournal(string tmp);

    [OperationContract]
    StatusAnswer_ImpVtd DeleteVtdImport(string keyImport);

    [OperationContract]
    KeyImpVtd Create_New_ImportVTD(string userKeyVar, string fileNameVar, string pathVar,
                                  string vtdDataKeyVar);

    [OperationContract]
    KeyUserVtd GetKeyUser();

    [OperationContract]
    ServiceVersion GetServiceVersion();

    [OperationContract]
    GetDbRepers GetDbRepers(string keyImp);

    [OperationContract]
    GetFileRepers GetFileRepers(string tmp);

    [OperationContract]
    StatusAnswer_ImpVtd Load_VTDXLSfileTrue(string newImportKey, string fileName);

    [OperationContract]
    StatusAnswer_ImpVtd ImportFile(double impKey, string typeOfImport, string fileName);

    [OperationContract]
    GetImportSecond IsOld_VTD_Available(string vtdMakingKey);

    [OperationContract]
    RemoveBound RemoveBound(string keyImport, string filekey);

    [OperationContract]
    KeyBoundResult ImportTubeMatching(string vtdMakingKey, string filekey, string dbKey, int typeLink);

    [OperationContract]
    KeyBoundResult GetTemMapping(string vtdMakingKey);

    [OperationContract]
    Res_Job GetVtdJobStatus();

    [OperationContract]
    List<InfoForOneVtdImport> GetVtdImpInfo(string key);

    [OperationContract]
    JobStatus GetJobStatus();
}


[DataContract]
public class ImpLogVtd : StatusAnswer_ImpVtd
{
    [DataMember]
    public ImpLog_result ImpLog_result { get; set; }
}

[DataContract]
public class ImpLog_result
{
    [DataMember]
    public string ImpLog_result_ret { get; set; }
}



[DataContract]
public class Res_Job : StatusAnswer_ImpVtd
{
    [DataMember]
    public Res_Job_status Res_Job_status { get; set; }
}

[DataContract]
public class Res_Job_status
{
    [DataMember]
    public string VTDMakingKey { get; set; } //возвращает ключ запущенного импорта

    [DataMember]
    public string ReturnStatus { get; set; } //возвращает состояние импорта
}

[DataContract]
public class KeyBoundResult: StatusAnswer_ImpVtd
{
    [DataMember]
    public List<KeyBound> GetKeyBoundList { get; set; }
}

[DataContract]
public class KeyBound
{
    [DataMember] public string KeyBD;
    [DataMember] public string KeyFile;
}


/// <summary>
/// удаляет увязанные трубы
/// </summary>
[DataContract]
public class RemoveBound : StatusAnswer_ImpVtd
{
    [DataMember]
    public RemoveBoundResult RemoveBound_Result { get; set; }
}
[DataContract]
public class RemoveBoundResult
{
    [DataMember]
    public int RemoveBoundVTD { get; set; }
}



[DataContract]
public class InfoForNewImport: StatusAnswer_ImpVtd
{
    [DataMember]
    public List<InfoForNewImportVtd> GetInfoNewImportVtdList { get; set; }
}
[DataContract]
public class InfoForNewImportVtd
{
    [DataMember]
    public string keyMg;

    [DataMember]
    public string keyPipe;

    [DataMember]
    public string keySection;

    [DataMember]
    public string keyContract;

    [DataMember]
    public string nameFile;

    [DataMember]
    public string cNameVTG; //название участка ВТД

    [DataMember]
    public string dDateContract; //дата подписания

    [DataMember]
    public string nKmBegin; //км. начала

    [DataMember]
    public string nKmEnd; //км. конца

    [DataMember]
    public string nLength; //длина

    [DataMember]
    public string cMainExecutor; //генеральный подрядчик

    [DataMember]
    public string cNameWork; //название комплекса работ

    [DataMember]
    public string cNumberContract; //номер договора

    [DataMember]
    public string cSubExecutor; //исполнитель работ

    [DataMember]
    public string сPipeName; //Нить

    [DataMember]
    public string сMTName; //Газопровод
}


[DataContract]
public class InfoForOneVtdImport
{
    [DataMember]
    public string cnamevtg;

    [DataMember]
    public string ddatecontract;

    [DataMember]
    public string nkmbegin;

    [DataMember]
    public string nkmend;

    [DataMember]
    public string nlength;

    [DataMember]
    public string cMainExecutor;

    [DataMember]
    public string CNAMEWORK;

    [DataMember]
    public string cnumbercontract;

    [DataMember]
    public string cSubExecutor;

    [DataMember]
    public string сPipeName;

    [DataMember]
    public string сMTName;
}

[DataContract]
public class GetDBDefect :StatusAnswer_ImpVtd
{
    [DataMember]
    public List<GetDBDefectList> GetDBDefectList { get; set; }
}

[DataContract]
public class GetDBDefectList
{
    [DataMember]
    public string NKEYDEFECT;

    [DataMember]
    public string CNAMEDEFECT;

    [DataMember]
    public string CNAMEGROUPDEFECT; 

}

[DataContract]
public class GetDBDefectFile : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<GetDBDefectListFile> GetDBDefectListFile { get; set; }
}

[DataContract]
public class GetDBDefectListFile
{
    [DataMember]
    public string CNAMEDEFECT;

}

[DataContract]
public class GetDBDefectMaping : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<GetDBDefectMapingList> GetDBDefectMapingList { get; set; }
}

[DataContract]
public class GetDBDefectMapingList
{
    [DataMember]
    public string nKeyMaping;

    [DataMember]
    public string nKeyDefect;

    [DataMember]
    public string cDefectName;

    [DataMember]
    public string cGroupDefectName;

    [DataMember]
    public string cFileDefectName;

}

/// <summary>
/// состояние джоба - есть запущенный импорт или нет
/// </summary>
[DataContract]
public class JobStatus: StatusAnswer_ImpVtd
{
    [DataMember]
    public JobStatusResult JobStatus_result { get; set; }
   
}
[DataContract]
public class JobStatusResult
{
  [DataMember]
  public string JobStatusVTD{ get; set; }
}




/// <summary>
/// возвращает повторный импорт или нет
/// </summary>
[DataContract]
public class GetImportSecond : StatusAnswer_ImpVtd
{
    [DataMember]
    public ImportSecondResult ImportSecond_Result { get; set; }
}
[DataContract]
public class ImportSecondResult
{
    [DataMember]
    public int ImportSecondVTD { get; set; }
}


[DataContract]
public class ImpVTD_Making : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<ImpVTD_Making_List> ImpVTD_Making_List { get; set; }
}

[DataContract]
public class ImpVTD_Making_List
{
    [DataMember]
    public string NIMP_MAKING;

    [DataMember]
    public string dTime;

    [DataMember]
    public string fio;

    [DataMember]
    public string CFILENAME;

    [DataMember]
    public string cState;

    [DataMember]
    public string cState1;

    [DataMember]
    public string cStateKey;

    [DataMember]
    public string NSTATEKEY;

    [DataMember]
    public string cnumber_contract;

    [DataMember]
    public string ddate_contract;

    [DataMember]
    public string ccode;

    [DataMember]
    public string userKey;

}


[DataContract]
public class DictDefectXSL_ImpVtd: StatusAnswer_ImpVtd
{
    [DataMember]
    public List<DictDefectXSL_List_ImpVtd> DictDefectXSL_List_ImpVtd { get; set; }
}
[DataContract]
public class DictDefectXSL_List_ImpVtd
{
    [DataMember] public string CNameDict;
}



[DataContract]
public class MG_ImpVtd : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<MG_List_ImpVtd> MG_List_ImpVtd { get; set; }
}

[DataContract]
public class MG_List_ImpVtd
{
    [DataMember]
    public string NMAIN_GAS_PIPELINE_KEY;
    [DataMember]
    public string CNAME;
}

[DataContract]
public class ThreadForMgImpVtd : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<ThreadForMgListImpVtd> ThreadForMgListImpVtd { get; set; }
}

[DataContract]
public class ThreadForMgListImpVtd
{
    [DataMember]
    public string ThreadKey;
    [DataMember]
    public string Name;
}

[DataContract]
public class KeyImpVtd : StatusAnswer_ImpVtd
{
    [DataMember]
    public KeyImp KeyImpResult { get; set; }
}

[DataContract]
public class KeyImp
{
    [DataMember]
    public string KeyImpVtd { get; set; }
}

[DataContract]
public class KeyUserVtd : StatusAnswer_ImpVtd
{
    [DataMember]
    public KeyUser KeyUserResult { get; set; }
}

[DataContract]
public class KeyUser
{
    [DataMember]
    public string KeyUserVtd { get; set; }
}

[DataContract]
public class ServiceVersion: StatusAnswer_ImpVtd
{
    [DataMember]
    public ServiceVersionTxt ServiceVersionResult { get; set; }
}

[DataContract]
public class ServiceVersionTxt
{
    [DataMember]
    public string ServiceVersionTxtVtd { get; set; }
}

[DataContract]
public class CountVtdSec : StatusAnswer_ImpVtd
{
    [DataMember]
    public CountVtd CountVtdResult { get; set; }
}

[DataContract]
public class CountVtd
{
    [DataMember]
    public string CountVtdSec { get; set; }
}


[DataContract]
public class VtdSec : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<VtdSecList> VtdSecList { get; set; }
}

[DataContract]
public class VtdSecList
{
    [DataMember]
    public string VtdSectionKey;

    [DataMember]
    public string NameRegion;
}

[DataContract]
public class VtdSecParam : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<VtdSecParamItem> VtdSecParamItem { get; set; }
}

[DataContract]
public class VtdSecParamItem
{
    [DataMember]
    public string NameRegion;

    [DataMember]
    public decimal KmBeginReg;

    [DataMember]
    public decimal KmEndReg;
}

[DataContract]
public class VtdNumberDogAllList : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<VtdDataAllList> VtdDataAllList { get; set; }
}

[DataContract]
public class VtdDataAllList
{
    [DataMember]
    public string VtdDataKey;

    [DataMember]
    public string NumberContract;

    [DataMember]
    public string ImpCount;
}


[DataContract]
public class VtdDogovorParams : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<VtdDataParams> VtdDataParams { get; set; }
}

[DataContract]
public class VtdDataParams
{
    [DataMember]
    public string DateContract;
    [DataMember]
    public string Namework;
    [DataMember]
    public string MainExecutor;
    [DataMember]
    public string SubExecutor;
}


[DataContract]
public class VtdTubeBaza : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<TubeBaza> ListTubeBaza { get; set; }
}

[DataContract]
public class TubeBaza
{
    /// <summary>
    /// ключ элемента монтажа
    /// </summary>
    [DataMember]
    public string PipeElementMontajKey;

    /// <summary>
    /// номер по отчету ВТД
    /// </summary>
    [DataMember]
    public string NumPipePart;

    /// <summary>
    /// Километраж левого края
    /// </summary>
    [DataMember]
    public string LocKmBeg;

    /// <summary>
    /// длинна
    /// </summary>
    [DataMember]
    public string Length;

    /// <summary>
    /// толщина стенки трубы (определена только если это труба - не тройник и т.п.
    /// </summary>
    [DataMember]
    public string DepthPipe;

    /// <summary>
    /// тип (я так понимаю имеется ввиду: труба, тройник ...)
    /// </summary>
    [DataMember]
    public string Type;

    [DataMember]
    public string TypeShort; 

    /// <summary>
    /// угловое положение продольного св. шва
    /// </summary>
    [DataMember]
    public string Angle;

    /// <summary>
    /// кол-во дефектов
    /// </summary>
    [DataMember]
    public string Count;

    /// <summary>
    /// тип продольного шва
    /// </summary>
    [DataMember]
    public string TypePipeKey;
}

/// <summary>
/// describe class for defect list
/// </summary>
[DataContract]
public class DefectListTable
{
    /// <summary>
    /// длина
    /// </summary>
    [DataMember]
    public string Length;

    /// <summary>
    /// угловое положение
    /// </summary>
    [DataMember]
    public string ClockWisePos;

    /// <summary>
    /// ширина
    /// </summary>
    [DataMember]
    public string Width;

    /// <summary>
    /// км
    /// </summary>
    [DataMember]
    public string LocNkm;

    /// <summary>
    /// глубина
    /// </summary>
    [DataMember]
    public string Depth;

    /// <summary>
    /// толщина стенки трубы
    /// </summary>
    [DataMember]
    public string PipeWallThickness;
}

/// <summary>
/// список дефектов трубы
/// </summary>
[DataContract]
public class DefectListTube
{
    /// <summary>
    /// длина
    /// </summary>
    [DataMember]
    public double Length;

    /// <summary>
    /// угловое положение
    /// </summary>
    [DataMember]
    public double ClockwisePos;

    /// <summary>
    /// ширина
    /// </summary>
    [DataMember]
    public double Width;

    /// <summary>
    /// км
    /// </summary>
    [DataMember]
    public string LocNkm;

    /// <summary>
    /// глубина
    /// </summary>
    [DataMember]
    public string Depth;

    /// <summary>
    /// толщина стенки трубы
    /// </summary>
    [DataMember]
    public string PipeWallThickness;

    /// <summary>
    /// Расстояние до предыдущего сварного шва (м)
    /// </summary>
    [DataMember]
    public double PrevSeamDist;

    /// <summary>
    /// процент глубины коррозии
    /// </summary>
    [DataMember]
    public string PersentCorroz;

    /// <summary>
    /// Ключ трубного элемента монтажа
    /// </summary>
    [DataMember]
    public string PipeElem;

    /// <summary>
    /// ключ типа дефекта
    /// </summary>
    [DataMember]
    public string DefectTypeKey;

    /// <summary>
    /// тип дефекта
    /// </summary>
    [DataMember]
    public string Type;

    /// <summary>
    /// расположение в глубине металла
    /// </summary>
    [DataMember]
    public string DepthPos;
}

[DataContract]
public class DefectListTubeRight
{
    /// <summary>
    /// длина
    /// </summary>
    [DataMember]
    public double Length; 

    /// <summary>
    /// угловое положение
    /// </summary>
    [DataMember]
    public double ClockwisePos; 

    /// <summary>
    /// ширина
    /// </summary>
    [DataMember]
    public double Width;

    /// <summary>
    /// Km
    /// </summary>
    [DataMember]
    public string LocNkm;

    /// <summary>
    /// глубина
    /// </summary>
    [DataMember]
    public string Depth;

    /// <summary>
    /// толщина стенки трубы
    /// </summary>
    [DataMember]
    public string PipewallThickness;

    /// <summary>
    /// Расстояние до предыдущего сварного шва  (м)
    /// </summary>
    [DataMember]
    public double PrevSeamDist;

    /// <summary>
    /// процент глубины коррозии
    /// </summary>
    [DataMember]
    public string PersentCorroz;

    /// <summary>
    /// Ключ трубного элемента монтажа
    /// </summary>
    [DataMember]
    public string PipeElem;

    /// <summary>
    /// ключ типа дефекта
    /// </summary>
    [DataMember]
    public string DefectTypeKey;

    /// <summary>
    /// тип дефекта
    /// </summary>
    [DataMember]
    public string Type;

    /// <summary>
    /// расположение в глубине металла
    /// </summary>
    [DataMember]
    public string DepthPos;

}

[DataContract]
public class VtdfileParamTable
{
    [DataMember] 
    public string Place;

    [DataMember]
    public string ContractN;

    [DataMember]
    public string ContractDate;

    [DataMember]
    public string MainExecutor;

    [DataMember]
    public string SubExecutor; 

    [DataMember]
    public string Fio;

    [DataMember]
    public string Position;

}

[DataContract]
public class InfoForVtdImportTable
{
    [DataMember]
    public string Thread;

    [DataMember]
    public string Mg;

    [DataMember]
    public string NameRegion;

    [DataMember]
    public string KmStart;

    [DataMember]
    public string KmEnd;

    [DataMember]
    public string Length;   
}

[DataContract]
public class GetStatisticsTable : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<StatisticsTable> StatisticsTableList { get; set; }
}

[DataContract]
public class StatisticsTable
{
    [DataMember]
    public string PipeElemCount;

    [DataMember]
    public string MagnAnomCount;

    [DataMember]
    public string UsedRepCount;

    [DataMember]
    public string RepOnKmCount;

    [DataMember]
    public string AvgCoefCount;

    /// <summary>
    /// Коррозионный дефект
    /// </summary>
    [DataMember]
    public string CorrosionDefect;

    /// <summary>
    /// Дефект формы
    /// </summary>
    [DataMember]
    public string FormDefect;

    /// <summary>
    /// Трещиноподобный дефект
    /// </summary>
    [DataMember]
    public string CracklikeDefect;

    /// <summary>
    /// Стресс-коррозионный дефект
    /// </summary>
    [DataMember]
    public string StressCorrosionDefect;

    /// <summary>
    /// Аномалия поперечного шва
    /// </summary>
    [DataMember]
    public string TransverseJointAnomaly;

    /// <summary>
    /// Нерасчетная аномалия
    /// </summary>
    [DataMember]
    public string AbnormalAnomaly;

    /// <summary>
    /// Количество чисто дефектов металла без 6-й группы.
    /// </summary>
    [DataMember]
    public string DefectsQty; 

}


[DataContract]
public class GetTubeJournal : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<TubeJournalTableList> TubeJournalTableList { get; set; }
}


[DataContract]
public class TubeJournalTableList
{
    [DataMember]
    public string RawKey;

    /// <summary>
    /// номер трубы
    /// </summary>
    [DataMember]
    public string PipeNo; 

    /// <summary>
    /// км по втд
    /// </summary>
    [DataMember]
    public string Km;

    /// <summary>
    /// Км по МГ
    /// </summary>
    [DataMember]
    public string MgKm;

    /// <summary>
    /// коэффициент???
    /// </summary>
    [DataMember]
    public string Coef;

    /// <summary>
    /// длина по МГ
    /// </summary>
    [DataMember]
    public string MgLenght;

    /// <summary>
    /// длина по втд
    /// </summary>
    [DataMember]
    public string Length; 

    /// <summary>
    /// толщина стенки
    /// </summary>
    [DataMember]
    public string Thickness;

    /// <summary>
    /// тип трубы
    /// </summary>
    [DataMember]
    public string Type;
 
    /// <summary>
    /// участок увязки
    /// </summary>
    [DataMember]
    public string SecNo; 

    /// <summary>
    /// количество дефектов
    /// </summary>
    [DataMember]
    public string DefCount; 
}

[DataContract]
public class GetDbRepers : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<DbRepersTableList> DbRepersTableList { get; set; }
}

[DataContract]
public class DbRepersTableList
{
    [DataMember]
    public string ObjKey; // Ключ репера

    [DataMember]
    public string Km; // Километраж

    [DataMember]
    public string Name; // Название

    [DataMember]
    public string Filtertype; // ????

    [DataMember]
    public string EntityName; // Название объекта

    [DataMember]
    public string ObjType; // Тип объекта

}



[DataContract]
public class GetFileRepers : StatusAnswer_ImpVtd
{
    [DataMember]
    public List<FileRepersList> FileRepersList { get; set; }
}

[DataContract]
public class FileRepersList
{
    /// <summary>
    /// километраж
    /// </summary>
    [DataMember]
    public string Km; // километраж
    /// <summary>
    /// тип объекта
    /// </summary>
    [DataMember]
    public string Type; // тип объекта

    /// <summary>
    /// название объекта
    /// </summary>
    [DataMember]
    public string Desc; // 
    /// <summary>
    /// фильтр
    /// </summary>
    [DataMember]
    public string FilterType; // 
    /// <summary>
    /// ключ
    /// </summary>
    [DataMember]
    public string RawKey; // ключ

    [DataMember]
    public string Label; // 
    /// <summary>
    /// количество секций
    /// </summary>
    [DataMember]
    public string SecCount; // 

}
