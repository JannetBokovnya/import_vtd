using System.Collections.Generic;
using importVtd.Resources;

namespace importVtd.Business
{
    public class StatusImport
    {
        public const string ChoosingData = "1"; // состояние выбора данных импорта
        public const string DefectDictionary = "2"; // состояние словарного словаря
        public const string LinkingReper = "3"; // состояние увязывание реперов
        public const string LinkingPipe = "4"; // состояние увязывание труб
        public const string PipeForFirstImport = "5"; // состояние первичного импорта труб
        public const string RunningImport = "6"; // состояние запущенного импорта
        public const string SuccessImport = "7"; // состояние успешного импорта
        public const string ErrorImport = "8"; // состояние импорта завершенного с ошибкой

        private readonly Dictionary<string, string> _dictListStatusImport = new Dictionary<string, string>
        {
            { ChoosingData, Resources_ImpVtd.statusChooseImpData }, 
            { DefectDictionary, Resources_ImpVtd.cDefectsDictionary },
            { LinkingReper, Resources_ImpVtd.cLinkingReper },
            { LinkingPipe, Resources_ImpVtd.statusLinkSection },
            { PipeForFirstImport, Resources_ImpVtd.cPipeJornal },
            { RunningImport, Resources_ImpVtd.cImportIsRunning },
            { SuccessImport, Resources_ImpVtd.statusSucessCompl },
            { ErrorImport, Resources_ImpVtd.statusErrCompl },
        };

        public string GetStatusImport(string keyStatus)
        {
            string status;
            return _dictListStatusImport.TryGetValue(keyStatus, out status) ? status : "<не определено>";
        }
    }
}
