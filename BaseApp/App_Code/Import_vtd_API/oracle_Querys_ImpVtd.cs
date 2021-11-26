/// <summary>
/// Summary description for oracleQuerys
/// </summary>
public class oracleQuerys_ImpVtd //: oracleEngine
{
    protected const string ApplyKeyMapQuery = "DB_API.vtd_import_api.ApplyKeyMap";

    protected const string RunTemMappingQuery = "DB_API.vtd_import_api.runTemMapping";

    protected const string GetTemMappingOuery = "DB_API.vtd_import_api.getTemMapping";

    protected const string AddTemMappingQuery = "DB_API.vtd_import_api.addTemMapping";

    protected const string CreateNewImportVtdQuery = "DB_API.vtd_import_api.Create_New_ImportVTD";

    protected const string DeleteMapDictsQuery = "DB_API.vtd_import_api.deleteDefMapping";

    protected const string SaveMapDictsQuery = "DB_API.vtd_import_api.saveDefMapping";

    protected const string DeleteVtdImportQuery = "DB_API.vtd_import_api.DeleteVTDImport";

    protected const string GetImpVtdMakingListQuery = "db_api.vtd_import_api.Get_ImpVTD_Making_List";

    protected const string GetDbDefectListQuery = "db_api.vtd_import_api.getDBDefectList";

    protected const string GetDbDefectListFileQuery = "db_api.vtd_import_api.getFileDefectList";

    protected const string GetDbDefectListMapingQuery = "db_api.vtd_import_api.getDefMapping";

    protected const string GetImpVtdMakingOneQuery = "DB_API.vtd_import_api.get_impvtd_making_one";

    protected const string GetVtdImpInfoQuery = "DB_API.vtd_import_api.GetVtdImpInfo";

    protected const string GetTubeJournalQuery = "DB_API.vtd_import_api.Get_TubeJournal";

    protected const string GetDbRepersQuery = "DB_API.vtd_import_api.GetDBRepers";

    protected const string GetFileRepersQuery = "DB_API.vtd_import_api.GetFileRepers";

    protected const string GetImpLogQuery = "DB_API.vtd_import_api.GetImpLog";

    protected const string GetInfoForVtdImportQuery = "DB_API.vtd_import_api.GetInfoForVtdImport";

    protected const string GetMgQuery = "DB_API.vtd_import_api.GetMG";

    protected const string GetStatisticsQuery = "DB_API.vtd_import_api.GetStatistics";

    protected const string GetThreadsForMgQuery = "DB_API.vtd_import_api.getThreadsForMG";

    //protected const string GetUserNameByIdQuery = "DB_API.IMPORT_EXT.getUserName";

    protected const string GetVtdDataListQuery = "DB_API.vtd_import_api.GetVTD_data_List";

    protected const string GetVtdDataParamsQuery = "DB_API.vtd_import_api.GetVTD_Data_Params";

    protected const string GetvtdfileParamQuery = "DB_API.vtd_import_api.getvtdfile_param";

    protected const string InportGetTableNameQuery = "DB_API.IMPORT_API.GetTableName";

    protected const string InportWrite2LogQuery = "DB_API.IMPORT_API.Write2Log";

    protected const string InportGetColumnsQuery = "DB_API.IMPORT_API.GetColumns";

    protected const string GetVtdSecCountQuery = "DB_API.vtd_import_api.GetVTDSec_Count";

    protected const string GetVtdSecListQuery = "DB_API.vtd_import_api.GetVTDSec_List";

    protected const string GetVtdSecParamQuery = "DB_API.vtd_import_api.GetVTDSec_Param";

    protected const string ImportDefectsQuery = "DB_API.vtd_import_api.ImportDefects";

    protected const string ImportTubeJournalQuery = "DB_API.vtd_import_api.ImportTubeJournal";

    protected const string LaunchVtdImportQuery = "DB_API.vtd_import_api.LaunchVTDImport";

    protected const string LoadVtdxlSfileQuery = "DB_API.vtd_import_api.Load_VTDXLSfile";

    protected const string VtdMapsDictsQuery = "DB_API.vtd_import_api.VTD_MapsDicts";

    protected const string GetVtdJobStatusQuery = "DB_API.vtd_import_api.GetJobStatus";

    protected const string FillLeftTableQuery = "DB_API.vtd_import_api.GetCurrentTubeJournal";

    protected const string FillRightDefectListQuery = "DB_API.vtd_import_api.FillRightDefects4Matching";

    protected const string FillRightTableQuery = "DB_API.vtd_import_api.Get_TubeJournal_4Matching";

    protected const string IsOldVtdAvailableQuery = "DB_API.vtd_import_api.isOld_VTD_Available";

    protected const string RollbackTemMappingQuery = "DB_API.vtd_import_api.rollbackTemMapping";

    protected const string GetImpVtdInfoNewImportQuery = "DB_API.vtd_import_api.GetVtdImpInfoWithKeys";

    protected const string FillLeftDefectListQuery = "DB_API.vtd_import_api.FillLeftTableFromTubeJournal";

    //Форма сопоставления типов магнитных аномалий
    public const string IsMagnAnomTypeMappedQuery = "ias_data.ias_map_magn_anomaly.IsMagnAnomTypeMapped";

    public const string SaveMagnAnomTypeMappingQuery = "ias_data.ias_map_magn_anomaly.saveMagnAnomTypeMapping";
}
