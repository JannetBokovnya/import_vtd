using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using importVtd.Resources;
using importVtd.startTable;

namespace importVtd.Business
{
    public class MainViewModel : MainModel
    {
        public ObservableCollection<string> Reports { get; private set; }
        public Helper Helper;
        public ObservableCollection<StatusBookmark> GetBookmark { get; private set; }
        //список импортов
        public List<ImpVTD_Making_List> ImpVtd { get; private set; }

        private string _nameTab = "";

        private IoraWCFService_ImpVtdClient _proxy;

        private IoraWCFService_ImpVtdClient Proxy
        {
            get { return _proxy ?? (_proxy = new IoraWCFService_ImpVtdClient()); }
        }

        private string _getTxtLog;
        private List<MG_List_ImpVtd> _mGList;
        private List<ThreadForMgListImpVtd> _threadList;
        private List<VtdSecList> _vtdSecList;
        private List<VtdSecParamItem> _vTdSecParamList;
        private List<VtdDataAllList> _numDogList;
        private bool _isValid;
        private List<VtdDataParams> _paramDog;
        private string _getKeyImp;
        private bool _isValidImportKey;
        private List<DbRepersTableList> _dBRepersList;
        private List<FileRepersList> _fileRepersList;
        private List<TubeJournalTableList> _tubeJournalList;
        private bool _loadVtdXlsFileTrue;
        private string _serviseVersion;
        private DispatcherTimer _пetStatusJobValueTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(40) };


        /// <summary>
        /// состояние импорта
        /// </summary>
        public string StateKey { get; set; }

        /// <summary>
        /// ключ импорта
        /// </summary>
        public string KeyImport { get; set; }

        /// <summary>
        /// ключ пользователя
        /// </summary>
        public string KeyUser { get; private set; }

        /// <summary>
        /// состояние импорта
        /// </summary>
        public string StatusJob { get; private set; }

        //тип статуса 1,0,-1
        public string StatusJobType
        {
            get { return _statusJobType; }
            set
            {
                _statusJobType = value;


                FirePropertyChanged("StatusJob");
            }
        }

        public string TypeVkladka = "";
        private List<StatisticsTable> _statistikList;

        public string Version { get; set; }

        public MainViewModel()
        {
            Reports = new ObservableCollection<string>();
            Helper = new Helper();
            GetNewImportInfo = new List<InfoForNewImportVtd>();
            KeyUser = "1";
            Version = "";
            GetBookmark = new ObservableCollection<StatusBookmark>
            {
                new StatusBookmark(true, "1"),
                new StatusBookmark(false, "2"),
                new StatusBookmark(false, "3"),
                new StatusBookmark(false, "4"),
                new StatusBookmark(false, "5"),
                new StatusBookmark(false, "6")
            };
        }

        public void Report(string message)
        {
            Reports.Add(string.Format("{0}, {1}", DateTime.Now.ToLongTimeString(), message));
        }

        public List<InfoForNewImportVtd> GetNewImportInfo { get; set; }

        /// <summary>
        /// информация по импорту, мг. нить, участок и т.д.
        /// </summary>
        /// <param name="keyImp"></param>
        /// <param name="tab"></param>
        public void GetInfoForNewImport(string keyImp, string tab)
        {
            _nameTab = tab;
            Proxy.GetInfoForNewImportCompleted += Proxy_GetInfoForNewImport;
            Proxy.GetInfoForNewImportAsync(keyImp);
        }

        private void Proxy_GetInfoForNewImport(object sender, GetInfoForNewImportCompletedEventArgs e)
        {
            GetNewImportInfo = new List<InfoForNewImportVtd>();
            Proxy.GetInfoForNewImportCompleted -= Proxy_GetInfoForNewImport;
            if (e.Result.IsValid)
            {
                GetNewImportInfo = new List<InfoForNewImportVtd>(e.Result.GetInfoNewImportVtdList);
                if (_nameTab == "tabItemNewImport")
                {
                    FirePropertyChanged("GetInfoForNewImportVtd");
                }
                if (_nameTab == "NextTabInGrid")
                {
                    FirePropertyChanged("NextTabInGrid");
                }
                if (_nameTab == "NewImport")
                {
                    FirePropertyChanged("Load_VTDXLSfileTrue");
                }
            }
            else
            {
                Report("Ошибка получения данных InfoForNewImportVtd GetInfoForNewImport");
            }
        }

        #region Основная вкладка
        public void GetServiceVersion()
        {
            //версия сервиса
            Proxy.GetServiceVersionCompleted += Proxy_GetServiceVersionCompleted;
            Proxy.GetServiceVersionAsync();
        }

        private void Proxy_GetServiceVersionCompleted(object sender, GetServiceVersionCompletedEventArgs e)
        {
            Proxy.GetServiceVersionCompleted -= Proxy_GetServiceVersionCompleted;
            if (e.Result.IsValid)
                Report("версия сервиса = " + e.Result.ServiceVersionResult.ServiceVersionTxtVtd);
            else
                Report("ошибка версия сервиса = " + e.Result.ErrorMessage);
        }

        public void GetKeyUser()
        {
            Proxy.GetKeyUserCompleted += Proxy_GetKeyUserCompleted;
            Proxy.GetKeyUserAsync();
        }

        private void Proxy_GetKeyUserCompleted(object sender, GetKeyUserCompletedEventArgs e)
        {
            Proxy.GetKeyUserCompleted -= Proxy_GetKeyUserCompleted;
            if (e.Result.IsValid)
            {
                KeyUser = e.Result.KeyUserResult.KeyUserVtd;
                if (e.Result.ErrorMessage != "OK")
                {
                    Report("Ошибка получения ключа, присваиваем ключ = 1");
                }
            }

            FirePropertyChanged("GetKeyUser");
        }

        #endregion

        #region Получаем список импортов для вкладки satateProcess, удаление импорта

        public void GetSatateImport()
        {
            IsShowBusy = true;
            Proxy.Get_ImpVTD_Making_ListCompleted += ProxyFillStartTable;
            Proxy.Get_ImpVTD_Making_ListAsync();
        }

        void ProxyFillStartTable(object sender, Get_ImpVTD_Making_ListCompletedEventArgs e)
        {
            IsShowBusy = false;
            Proxy.Get_ImpVTD_Making_ListCompleted -= ProxyFillStartTable;

            if (e.Result.IsValid)
            {

                ImpVtd = new List<ImpVTD_Making_List>(e.Result.ImpVTD_Making_List);
                FirePropertyChanged("GetSatateImport");

                if (ImpVtd.Count == 0)
                    Report("Список импортов пустой = 0");
            }
            else
            {
                Report("Ошибка Get_ImpVTD_Making_List - " + e.Result.ErrorMessage);
            }
        }

        //удаление импортов
        public void DeleteImport(string keyImport)
        {
            IsShowBusy = true;
            Proxy.DeleteVtdImportCompleted += ProxyDeleteVtdImport;
            Proxy.DeleteVtdImportAsync(keyImport);
        }

        void ProxyDeleteVtdImport(object sender, DeleteVtdImportCompletedEventArgs e)
        {
            Proxy.DeleteVtdImportCompleted -= ProxyDeleteVtdImport;

            IsShowBusy = false;
            if (e.Result.IsValid)
            {
                //перенаполняем данными стартовую таблицу
                Report(" stateprocess ProxyDeleteVTDImport перезаписываем таблицу");
                GetSatateImport();
            }
            else
            {
                Report("Ошибка удаления импорта +" + e.Result.ErrorMessage);
                MessageBox.Show(Resources_ImpVtd.Error + Resources_ImpVtd.ErrorDeleteImport, Resources_ImpVtd.Error, MessageBoxButton.OK);
            }
        }

        #endregion

        #region вкладка создание нового импорта newImport

        public string GetTxtLog
        {
            get { return _getTxtLog; }
            set
            {
                _getTxtLog = value;
                FirePropertyChanged("GetTxtLog");
            }
        }

        /// <summary>
        /// список всех магистралей
        /// </summary>
        public List<MG_List_ImpVtd> MgList
        {
            get { return _mGList; }
            private set
            {
                _mGList = value;
                FirePropertyChanged("GetMGList");
            }
        }
        /// <summary>
        /// список нитей
        /// </summary>
        public List<ThreadForMgListImpVtd> ThreadList
        {
            get { return _threadList; }
            private set
            {
                if (value.Count > 0 && value[0].Name == "<empty>")
                    value[0].Name = Resources_ImpVtd.cChoose;

                _threadList = value;
                FirePropertyChanged("GetThreadList");
            }
        }
        /// <summary>
        /// список участков
        /// </summary>
        public List<VtdSecList> VtdSecList
        {
            get { return _vtdSecList; }
            private set
            {
                if (value.Count > 0 && value[0].NameRegion == "<empty>")
                    value[0].NameRegion = Resources_ImpVtd.cChoose;

                _vtdSecList = value;
                FirePropertyChanged("GetVTDSecList");
            }
        }
        /// <summary>
        /// параметры участка
        /// </summary>
        public List<VtdSecParamItem> VtdSecParamList
        {
            get { return _vTdSecParamList; }
            private set
            {
                if (value.Count > 0 && value[0].NameRegion == "<empty>")
                    value[0].NameRegion = Resources_ImpVtd.cChoose;

                _vTdSecParamList = value;
                FirePropertyChanged("GetVTDSec_ParamList");
            }
        }
        /// <summary>
        /// список договоров
        /// </summary>
        public List<VtdDataAllList> NumDogList
        {
            get { return _numDogList; }
            private set
            {
                if (value.Count > 0 && value[0].NumberContract == "<empty>")
                    value[0].NumberContract = Resources_ImpVtd.cChoose;

                _numDogList = value;
                FirePropertyChanged("GetNumDogList");
            }
        }

        public bool IsValid
        {
            get { return _isValid; }
            private set
            {
                _isValid = value;
                FirePropertyChanged("GetIsValid");
            }
        }
        /// <summary>
        /// параметры договора
        /// </summary>
        public List<VtdDataParams> ParamDog
        {
            get { return _paramDog; }
            private set
            {
                _paramDog = value;
                FirePropertyChanged("GetParamDog");
            }
        }
        /// <summary>
        /// ключ импорта
        /// </summary>
        public string GetKeyImp
        {
            get { return _getKeyImp; }
            private set
            {
                _getKeyImp = value;
                FirePropertyChanged("GetKeyImp");
            }
        }

        public bool IsValidImportKey
        {
            get { return _isValidImportKey; }
            private set
            {
                _isValidImportKey = value;
                FirePropertyChanged("IsValidImportKey");
            }
        }
        /// <summary>
        /// реперы из базы
        /// </summary>
        public List<DbRepersTableList> DbRepersList
        {
            get { return _dBRepersList; }
            private set
            {
                _dBRepersList = value;
                FirePropertyChanged("GetDBRepersList");
            }
        }

        /// <summary>
        /// реперы из файла
        /// </summary>
        public List<FileRepersList> FileRepersList
        {
            get { return _fileRepersList; }
            private set
            {
                _fileRepersList = value;
                FirePropertyChanged("GetFileRepersList");
            }
        }

        /// <summary>
        /// трубный журнал
        /// </summary>
        public List<TubeJournalTableList> TubeJournalList
        {
            get { return _tubeJournalList; }
            private set
            {
                _tubeJournalList = value;
                FirePropertyChanged("GetTubeJournalList");
            }
        }

        /// <summary>
        /// лог анализа файла
        /// </summary>
        /// <param name="keyImport"></param>
        public void GetImpLog(string keyImport)
        {
            Proxy.GetImpLogCompleted += GetImpLogCompleted;
            Proxy.GetImpLogAsync(keyImport);
        }

        void GetImpLogCompleted(object sender, GetImpLogCompletedEventArgs e)
        {
            Proxy.GetImpLogCompleted -= GetImpLogCompleted;
            if (e.Result.IsValid)
            {
                GetTxtLog = e.Result.ImpLog_result.ImpLog_result_ret;
            }
            else
            {
                Report("Ошибка получения лога = " + e.Result.ErrorMessage);
            }
        }

        public void GetAllMg()
        {
            Proxy.Get_MG_ImpVtdCompleted += Proxy_Get_MG_ImpVtdCompleted;
            Proxy.Get_MG_ImpVtdAsync();
        }

        private void Proxy_Get_MG_ImpVtdCompleted(object sender, Get_MG_ImpVtdCompletedEventArgs e)
        {
            Proxy.Get_MG_ImpVtdCompleted -= Proxy_Get_MG_ImpVtdCompleted;

            if (e.Result.IsValid)
            {
                MgList = new List<MG_List_ImpVtd>(e.Result.MG_List_ImpVtd);
            }
            else
            {
                Report("Ошибка Get_MG_ImpVtd +" + e.Result.ErrorMessage);
            }
        }
        /// <summary>
        /// список нитей по ключу газопровода
        /// </summary>
        /// <param name="keyMg"></param>
        public void GetThreadsForMg(string keyMg)
        {
            Proxy.GetThreadsForMgCompleted += ProxyFillThreadList;
            Proxy.GetThreadsForMgAsync(keyMg);
        }

        private void ProxyFillThreadList(object sender, GetThreadsForMgCompletedEventArgs e)
        {
            Proxy.GetThreadsForMgCompleted -= ProxyFillThreadList;
            if (e.Result.IsValid)
            {
                ThreadList = new List<ThreadForMgListImpVtd>(e.Result.ThreadForMgListImpVtd);
            }
            else
            {
                Report("Ошибка getThreadsForMG +" + e.Result.ErrorMessage);
            }
        }

        /// <summary>
        /// список участков
        /// </summary>
        /// <param name="keyThread"></param>
        public void GetVtdSec(string keyThread)
        {
            Proxy.GetVTDSec_ListCompleted += ProxyFillVtdList;
            Proxy.GetVTDSec_ListAsync(keyThread);
        }

        private void ProxyFillVtdList(object sender, GetVTDSec_ListCompletedEventArgs e)
        {
            Proxy.GetVTDSec_ListCompleted -= ProxyFillVtdList;

            if (e.Result.IsValid)
            {
                VtdSecList = new List<VtdSecList>(e.Result.VtdSecList);
            }
            else
            {
                Report("Ошибка список участков втд GetVTDSec_List +" + e.Result.ErrorMessage);
            }
        }

        /// <summary>
        /// параметры участка по ключу участка
        /// </summary>
        /// <param name="keySec"></param>
        public void GetVTDSec_Param(string keySec)
        {
            Proxy.GetVTDSec_ParamCompleted += ProxyFillVtdSectionParam;
            Proxy.GetVTDSec_ParamAsync(keySec);
        }

        void ProxyFillVtdSectionParam(object sender, GetVTDSec_ParamCompletedEventArgs e)
        {
            Proxy.GetVTDSec_ParamCompleted -= ProxyFillVtdSectionParam;
            if (e.Result.IsValid)
            {
                VtdSecParamList = new List<VtdSecParamItem>(e.Result.VtdSecParamItem);
            }
            else
            {
                Report("Ошибка получения параметров GetVTDSec_Param +" + e.Result.ErrorMessage);
            }
        }

        /// <summary>
        /// список номера договора
        /// </summary>
        /// <param name="keySec"></param>
        public void GetNumberDog(string keySec)
        {
            Proxy.GetVTD_data_AllListCompleted += ProxyFillNumDogovor;
            Proxy.GetVTD_data_AllListAsync(keySec);
        }

        private void ProxyFillNumDogovor(object sender, GetVTD_data_AllListCompletedEventArgs e)
        {
            Proxy.GetVTD_data_AllListCompleted -= ProxyFillNumDogovor;
            if (e.Result.IsValid)
            {
                NumDogList = new List<VtdDataAllList>(e.Result.VtdDataAllList);
            }
            else
            {
                Report("Ошибка список договоров GetVTD_data_AllList +" + e.Result.ErrorMessage);
            }
        }

        /// <summary>
        /// параметры по номеру договора
        /// </summary>
        /// <param name="keyDog"></param>
        public void GetParamNumberDog(string keyDog)
        {
            Proxy.GetVTD_Data_ParamsCompleted += ProxyFillDataParams;
            Proxy.GetVTD_Data_ParamsAsync(keyDog);
        }

        private void ProxyFillDataParams(object sender, GetVTD_Data_ParamsCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                ParamDog = new List<VtdDataParams>(e.Result.VtdDataParams);
            }
            else
            {
                Report("Ошибка параметры договора GetVTD_Data_Params +" + e.Result.ErrorMessage);
                IsValid = false;
            }
        }
        /// <summary>
        /// создание импорта, возвращает ключ импорта
        /// </summary>
        /// <param name="userKeyVar"></param>
        /// <param name="fileNameVar"></param>
        /// <param name="pathVar"></param>
        /// <param name="vtdDataKeyVar"></param>
        public void Create_New_ImportVTD(string userKeyVar, string fileNameVar, string pathVar,
                                         string vtdDataKeyVar)
        {
            IsShowBusy = true;
            Proxy.Create_New_ImportVTDCompleted += proxyCreate_New_ImportVTD;
            Proxy.Create_New_ImportVTDAsync(userKeyVar, fileNameVar, pathVar, vtdDataKeyVar);
        }

        private void proxyCreate_New_ImportVTD(object sender, Create_New_ImportVTDCompletedEventArgs e)
        {

            Proxy.Create_New_ImportVTDCompleted -= proxyCreate_New_ImportVTD;
            if (e.Result.IsValid)
            {
                GetKeyImp = e.Result.KeyImpResult.KeyImpVtd;
            }
            else
            {
                IsShowBusy = false;
                IsValidImportKey = false;
                Report("Ошибка proxyCreate_New_ImportVTD  создании нового импорта ВТД" + e.Result.ErrorMessage);
                MessageBox.Show("Create_New_ImportVTD " + e.Result.ErrorMessage, Resources_ImpVtd.Error, MessageBoxButton.OK);
            }
        }
        /// <summary>
        /// загрузка ексель файла
        /// </summary>
        public void Load_VTDXLSfile(string keyImport, string fileName)
        {
            Proxy.ImportFileCompleted += Proxy_ImportFileCompleted;
            Proxy.ImportFileAsync(Convert.ToDouble(keyImport), "VTD", fileName);
        }

        void Proxy_ImportFileCompleted(object sender, ImportFileCompletedEventArgs e)
        {
            Proxy.ImportFileCompleted -= Proxy_ImportFileCompleted;
            if (e.Result.IsValid)
            {
                //Получаем Лог анализа файла
                GetImpLog(KeyImport);
                GetInfoForNewImport(KeyImport, "NewImport");
            }
            else
            {
                //Получаем Лог анализа файла
                GetImpLog(KeyImport);
                MessageBox.Show(Resources_ImpVtd.Error + "Resources_ImpVtd.Error " + e.Result.ErrorMessage, Resources_ImpVtd.Error, MessageBoxButton.OK);
                Report("Ошибка ImportFile загрузка файла в базу" + e.Result.ErrorMessage);
            }

            IsShowBusy = false;
        }

        //проверка словарей на сопоставление
        public void MapsDicts()
        {
            Proxy.MapsDictsCompleted += Proxy_MapsDictsCompleted;
            Proxy.MapsDictsAsync(KeyImport);
        }

        void Proxy_MapsDictsCompleted(object sender, MapsDictsCompletedEventArgs e)
        {
            Proxy.MapsDictsCompleted -= Proxy_MapsDictsCompleted;
            if (e.Result.IsValid)
            {
                //проверяем правильно лизалиты словари
                FirePropertyChanged("NewImportNext");
            }
            else
            {
                Report("Ошибка MapsDicts" + e.Result.ErrorMessage);
            }
        }

        #endregion

        #region Вкладка увязка реперов
        /// <summary>
        /// реперы из базы
        /// </summary>
        public void GetDbRepers(string keyImport)
        {
            IsShowBusy = true;
            Proxy.GetDbRepersCompleted += ProxyGetDbRepers;
            Proxy.GetDbRepersAsync(keyImport);
        }

        private void ProxyGetDbRepers(object sender, GetDbRepersCompletedEventArgs e)
        {
            IsShowBusy = false;
            Proxy.GetDbRepersCompleted -= ProxyGetDbRepers;
            if (e.Result.IsValid)
            {
                DbRepersList = new List<DbRepersTableList>(e.Result.DbRepersTableList);
            }
            else
            {
                Report("не удалось наполнить  список  реперов из БД - " + e.Result.ErrorMessage);
            }
        }

        /// <summary>
        /// реперы из файла
        /// </summary>
        /// <param name="keyImport"></param>
        public void GetFileRepers(string keyImport)
        {
            IsShowBusy = true;
            Proxy.GetFileRepersCompleted += ProxyGetFileRepers;
            Proxy.GetFileRepersAsync(keyImport);
        }

        private void ProxyGetFileRepers(object sender, GetFileRepersCompletedEventArgs e)
        {
            IsShowBusy = false;
            Proxy.GetFileRepersCompleted -= ProxyGetFileRepers;
            if (e.Result.IsValid)
            {
                FileRepersList = new List<FileRepersList>(e.Result.FileRepersList);
            }
            else
            {
                Report("не удалось наполнить  список  реперов из файла - " + e.Result.ErrorMessage);
            }
        }

        public void AddKeyMapRepers(string keyImport, string keyRepers)
        {
            IsShowBusy = true;
            Proxy.ApplyKeyMapCompleted += Proxy_ApplyKeyMapCompleted;
            Proxy.ApplyKeyMapAsync(keyImport, keyRepers.TrimEnd(';'));
        }

        private void Proxy_ApplyKeyMapCompleted(object sender, ApplyKeyMapCompletedEventArgs e)
        {
            IsShowBusy = false;
            Proxy.ApplyKeyMapCompleted -= Proxy_ApplyKeyMapCompleted;
            if (e.Result.IsValid)
            {
                Report("реперы увязались ок");
                //после увязки реперов вызываем функцию оипределения повторного импорта
                //FirePropertyChanged("ApplyKeyMap");
                GetIsImportSecond(KeyImport);
            }
            else
            {
                MessageBox.Show("Ошибка увязки реперов " + e.Result.ErrorMessage);
                Report("Ошибка импорта увязанных реперов " + e.Result.ErrorMessage);
            }
        }

        #endregion

        #region Проверка повторный импорт или нет
        public int VtdAvailable
        {
            get { return _vtdAvailable; }
            private set
            {
                _vtdAvailable = value;
                FirePropertyChanged("VTD_Available");
            }
        }

        private void GetIsImportSecond(string keyImport)
        {
            IsShowBusy = true;
            Proxy.IsOld_VTD_AvailableCompleted += Proxy_IsOld_VTD_AvailableCompleted;
            Proxy.IsOld_VTD_AvailableAsync(keyImport);

        }

        void Proxy_IsOld_VTD_AvailableCompleted(object sender, IsOld_VTD_AvailableCompletedEventArgs e)
        {
            Proxy.IsOld_VTD_AvailableCompleted -= Proxy_IsOld_VTD_AvailableCompleted;

            if (e.Result.IsValid)
            {
                VtdAvailable = e.Result.ImportSecond_Result.ImportSecondVTD;
                Report(e.Result.ErrorMessage);
            }
            else
            {
                Report("Ошибка получения данных IsOld_VTD_Available err = " + e.Result.ErrorMessage); 
            }
        }

        #endregion


        #region вкладка Трубный журнал

        public void GetTubeJournal(string keyImport)
        {
            IsShowBusy = true;
            Proxy.Get_TubeJournalCompleted += proxyGet_TubeJournalCompleted;
            Proxy.Get_TubeJournalAsync(keyImport);
        }

        private void proxyGet_TubeJournalCompleted(object sender, Get_TubeJournalCompletedEventArgs e)
        {
            IsShowBusy = false;
            Proxy.Get_TubeJournalCompleted -= proxyGet_TubeJournalCompleted;
            if (e.Result.IsValid)
            {
                Report(" Получили список трубного журнала всего записей  " + e.Result.TubeJournalTableList.Count().ToString());
                TubeJournalList = new List<TubeJournalTableList>(e.Result.TubeJournalTableList);
            }
            else
            {

                Report("Ошибка получения данных Get_TubeJournal err = " + e.Result.ErrorMessage);
            }
        }
        #endregion

        #region вкладка импорт данных

        public List<StatisticsTable> StatistikList
        {
            get { return _statistikList; }
            private set
            {
                _statistikList = value;
                FirePropertyChanged("ImportEnd");
            }
        }

        private bool _isImport = false;

        public void ImportVTD()
        {
            IsShowBusy = false;
            _isImport = true;

            //Job процедуры Импорта ВТД не запущен. Можем продолжать процедуру Импорта ВТД.
            //если вызывали по кнопке добавить новый импорт

            //  :  0 - остановлено с ошибкой
            //  :  1 - не существует ("готово к работе" в оригинале в Oracle)
            //  : -1 - выполняется в данный момент,

            //запускаем импорт
            Report("Запускаем импорт ImportVTDLaunches");
            IsBusyIndicatorJobEnd = false; //запускаем импорт
            Proxy.ImportVTDLaunchesCompleted += ProxyImportVtdLaunchesCompleted;
            Proxy.ImportVTDLaunchesAsync(KeyImport);
        }

        public bool IsBusyIndicatorJobEnd
        {
            get { return _busyIndicatorEnd; }
            private set
            {
                _busyIndicatorEnd = value;
                FirePropertyChanged("BusyIndicatorJobEnd");
            }
        }

        #region TestTimer

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Report("Сработал таймер, запустил GetStatusJob()");
            GetStatusJob();
            if (StatusJobType == "0" || StatusJobType == "1")
            {
                Report("Таймер StatusJobType = " + StatusJobType);
                _пetStatusJobValueTimer.Stop();
                Report("таймер остановлен");
               
                //если импорт выполнился то статистика
                GetStatus();
                IsBusyIndicatorJobEnd = true;
            }
        }

        #endregion

        private void ProxyImportVtdLaunchesCompleted(object sender, ImportVTDLaunchesCompletedEventArgs e)
        {
            IsShowBusy = false;
            _isImport = false;

            Proxy.ImportVTDLaunchesCompleted -= ProxyImportVtdLaunchesCompleted;
            if (e.Result.IsValid)
            {
                Report("запускаем задержку 5 сек потом  GetStatusJob(); и после вызова тоже задержка 5 сек");
                Thread.Sleep(5000);
                GetImpLog(KeyImport);
                GetStatusJob();
                Thread.Sleep(5000);
                //запускаем таймер
                Report("Запустился импорт ImportVTDLaunches - ок");
                Report("запускаем таймер с периодом 10сек");
                _пetStatusJobValueTimer = new DispatcherTimer();
                _пetStatusJobValueTimer.Tick += dispatcherTimer_Tick;
                _пetStatusJobValueTimer.Interval = new TimeSpan(0, 0, 10);
                _пetStatusJobValueTimer.Start();
            }
            else
            {
                Report("Ошибка запуска импорта ImportVTDLaunches err = " + e.Result.ErrorMessage);
                MessageBox.Show(Resources_ImpVtd.Error + "Ошибка импорта err= " + e.Result.ErrorMessage,
                                Resources_ImpVtd.Error, MessageBoxButton.OK);
                IsBusyIndicatorJobEnd = true;
                GetStatusJob();
            }

        }


        //состояние Job

        public void GetStatusJob()
        {
            
            Proxy.GetVtdJobStatusCompleted += Proxy_GetVTDJobStatusCompleted;
            Proxy.GetVtdJobStatusAsync();
        }




        private void Proxy_GetVTDJobStatusCompleted(object sender, GetVtdJobStatusCompletedEventArgs e)
        {
            Proxy.GetVtdJobStatusCompleted -= Proxy_GetVTDJobStatusCompleted;

            if (e.Result.IsValid)
            {
                StatusJob = e.Result.Res_Job_status.VTDMakingKey;
                StatusJobType = e.Result.Res_Job_status.ReturnStatus;
                Report("Выполнился GetStatusJob StatusJob = " + e.Result.Res_Job_status.VTDMakingKey + "  StatusJobType = " + e.Result.Res_Job_status.ReturnStatus + " isImport =" + _isImport);
            }
            else
            {
                Report("Ошибка проверки статуса " + e.Result.ErrorMessage);
            }
        }

        
        //после окончания импорта запускаем статистику
        public void GetStatus()
        {
            Proxy.StatisticsTableCompleted += ProxyStatisticsTableCompleted;
            Proxy.StatisticsTableAsync(KeyImport);
        }

        private void ProxyStatisticsTableCompleted(object sender, StatisticsTableCompletedEventArgs e)
        {
            Proxy.StatisticsTableCompleted -= ProxyStatisticsTableCompleted;
            if (e.Result.IsValid)
            {
                StatistikList = new List<StatisticsTable>(e.Result.StatisticsTableList);
            }
            else
            {
                Report("Ошибка статистики err = " + e.Result.ErrorMessage);
            }

        }

        #endregion

        #region вкладка раскладка труб

        private List<TubeBaza> _dListTubeBaza;
        private List<TubeBaza> _dBListTubeFile;
        private string _statusJobType;
        private List<KeyBound> _listKeyBounds;
        private bool _busyIndicatorEnd;
        private int _vtdAvailable;
        private int _removeItem;
        private List<KeyBound> _temMapping;

        public List<TubeBaza> DbListTubeBaza
        {
            get { return _dListTubeBaza; }
            private set
            {
                _dListTubeBaza = value;
                FirePropertyChanged("GetDBTubeBazaList");
            }
        }

        public List<TubeBaza> DbListTubeFile
        {
            get { return _dBListTubeFile; }
            private set
            {
                _dBListTubeFile = value;
                FirePropertyChanged("GetDBTubeFileList");
            }
        }

        /// <summary>
        /// список труб из базы
        /// </summary>
        /// <param name="keyImport"></param>
        public void GetLeftTable(string keyImport)
        {
            IsShowBusy = true;
            Proxy.GetLeftTableDataCompleted += proxy_GetLeftTableCompleted;

            Proxy.GetLeftTableDataAsync(keyImport);
        }

        private void proxy_GetLeftTableCompleted(object sender, GetLeftTableDataCompletedEventArgs e)
        {
            IsShowBusy = false;
            Proxy.GetLeftTableDataCompleted -= proxy_GetLeftTableCompleted;

            if (e.Result.IsValid)
            {
                DbListTubeBaza  = new List<TubeBaza>(e.Result.ListTubeBaza);
                if (e.Result.ListTubeBaza.Length > 0)
                {
                    IsShowBusy = true;

                    Proxy.GetRightTableDataCompleted += proxy_GetRightTableCompleted;
                    Proxy.GetRightTableDataAsync(KeyImport);
                }
            }
            else
            {
                Report("Ошибка получения данных GetLeftTableData err = " + e.Result.ErrorMessage);
            }

        }
        /// <summary>
        /// наполнение правой таблицы - темы из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proxy_GetRightTableCompleted(object sender, GetRightTableDataCompletedEventArgs e)
        {
            IsShowBusy = false;
            Proxy.GetRightTableDataCompleted -= proxy_GetRightTableCompleted;
            if (e.Result.IsValid)
            {
                DbListTubeFile = new List<TubeBaza>(e.Result.ListTubeBaza);
                //список увязанных темов
                GetTemMapping();
            }
            else
            {
                Report("Ошибка получения данных GetRightTableData err = " + e.Result.ErrorMessage);
            }
        }

        //список увязанных темов
        public void GetTemMapping()
        {
            IsShowBusy = true;
            Proxy.GetTemMappingCompleted += Proxy_GetTemMappingCompleted;
            Proxy.GetTemMappingAsync(KeyImport);
        }

        public List<KeyBound> TemMappingList
        {
            get { return _temMapping; }
            private set
            {
                _temMapping = value;
                FirePropertyChanged("GetTemMapping");
            }
        }

        void Proxy_GetTemMappingCompleted(object sender, GetTemMappingCompletedEventArgs e)
        {
            IsShowBusy = false;

            Proxy.GetTemMappingCompleted -= Proxy_GetTemMappingCompleted;
             if (e.Result.IsValid)
             {
                 TemMappingList = new List<KeyBound>(e.Result.GetKeyBoundList);
             }
             else
             {
                 Report("Ошибка получения данных GetTemMapping err = " + e.Result.ErrorMessage);
             }
        }

        public List<KeyBound> ListKeyBounds
        {
            get { return _listKeyBounds; }
            set
            {
                _listKeyBounds = value;
                FirePropertyChanged("ImportTubeMatchingOneRow");
            }
        }

        private int RemoveItem
        {
            get { return _removeItem; }
            set
            {
                _removeItem = value;
                FirePropertyChanged("RemoveItem");
            }
        }

        public void DeleteBounded(string keyImport, string inFileKey)
        {
            IsShowBusy = true;
            Proxy.RemoveBoundCompleted += Proxy_RemoveBoundCompleted;
            Proxy.RemoveBoundAsync(keyImport, inFileKey);
        }

        void Proxy_RemoveBoundCompleted(object sender, RemoveBoundCompletedEventArgs e)
        {
            IsShowBusy = false;
            Proxy.RemoveBoundCompleted -= Proxy_RemoveBoundCompleted;

            if (e.Result.IsValid)
            {
                RemoveItem = e.Result.RemoveBound_Result.RemoveBoundVTD;
                Report("удаление связи труб RemoveItem = " + RemoveItem.ToString());
            }
            else
            {
                Report(e.Result.ErrorMessage);
            }
        }

        #endregion
    }
}
