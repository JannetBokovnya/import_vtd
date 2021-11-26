using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using importVtd.Business;
using importVtd.Resources;
using importVtd.startTable;

namespace importVtd.Controls
{
    public partial class NewImport
    {
        string _fileName = string.Empty;
        private List<ThreadForMgListImpVtd> _selectThreadList;
        private List<MG_List_ImpVtd> _selectMgList;
        private List<VtdSecList> _selectVtdSec;
        private List<VtdDataAllList> _selectNumberDog;
        private List<InfoForNewImportVtd> _infoNewImportInfo;
        public MainViewModel Model { get; private set; }
        private OpenFileDialog dlg;
        private bool _isloaded = true;

        public NewImport(List<InfoForNewImportVtd> getNewImportInfo, MainViewModel model)
        {
            //локализация для всех гридов телерик - подключаем локализацию
            LocalizationManager.DefaultResourceManager = Resources_ImpVtd.ResourceManager;

            InitializeComponent();

            Model = model;

            _infoNewImportInfo = getNewImportInfo;
            Model.PropertyChanged -= MainModelPropertyChanged;
            Model.PropertyChanged += MainModelPropertyChanged;

            //если переходим кликом на вкладку и импорт продолжаем т.е. не новый
            if (_infoNewImportInfo.Count > 0)
            {
                DdlMg.ItemsSource = null;
                StatusText.Text = _infoNewImportInfo[0].nameFile;
                StatusText.IsEnabled = false;
                BtnUploadFile.IsEnabled = false;
                BtnAnalizeFile.IsEnabled = false;
                btnForward.IsEnabled = false;
                BtnBack.IsEnabled = false;
                //лог анализа файла
                Model.GetImpLog(Model.KeyImport);
            }

            if (DdlMg.Items.Count == 0)
            {
                DdlMg.ItemsSource = null;
                Model.GetAllMg();
            }

            ControlsIsEnabled(false);
        }

        /// <summary>
        /// выбор нити по ключу мг
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlMG_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((DdlMg.SelectedItem != null))
            {
                string keyMg = ((MG_List_ImpVtd)DdlMg.SelectedItem).NMAIN_GAS_PIPELINE_KEY;
                DdlThread.ItemsSource = null;
                Model.GetThreadsForMg(keyMg);
            }
            else
            {
                BtnUploadFile.IsEnabled = false;
                BtnAnalizeFile.IsEnabled = false;
            }
        }

        //выбор нити
        private void DDLThread_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((DdlThread.SelectedItem != null) &&
                (((ThreadForMgListImpVtd)DdlThread.SelectedItem).ThreadKey != "-1"))
            {
                //загружаеи сразу по ключу участки
                DdlVtdList.ItemsSource = null;
                Model.GetVtdSec(((ThreadForMgListImpVtd)DdlThread.SelectedItem).ThreadKey);
            }
            else if (DdlVtdList.Items.Count > 0)
            {
                 DdlVtdList.ItemsSource = null;
            } 
        }

        /// <summary>
        /// обработка клика по выбранному участку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DDLVTDList_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((DdlVtdList.SelectedItem != null) &&
                (((VtdSecList)DdlVtdList.SelectedItem).VtdSectionKey != "-1"))
            {
                //параметры участка
                Model.GetVTDSec_Param(((VtdSecList)DdlVtdList.SelectedItem).VtdSectionKey);

                //номер договора
                Model.GetNumberDog(((VtdSecList)DdlVtdList.SelectedItem).VtdSectionKey);
            }
            else
            {
                TxtKmStartTab2.Text = "";
                TxtKmEndTab2.Text = "";
                TxtKmLenTab2.Text = "";
                BtnUploadFile.IsEnabled = false;
                BtnAnalizeFile.IsEnabled = false;
                if (DdlNumDogovor.Items.Count > 0)
                {
                    DdlNumDogovor.ItemsSource = null;
                }
            }
        }

        //Выбор номера договора в выпадающем списке
        private void NumDogovor_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((DdlNumDogovor.SelectedItem != null) &&
                (((VtdDataAllList)DdlNumDogovor.SelectedItem).VtdDataKey != "-1"))
            {
                //Проверяем, можно ли выполнить Импорт ВТД на данный участок ВТД, и под данным договором на выполнение ВТД?
                if (((VtdDataAllList)DdlNumDogovor.SelectedItem).ImpCount == "0")
                {
                    ControlsIsEnabled(true);
                    BtnAnalizeFile.IsEnabled = false;
                    Model.GetParamNumberDog(((VtdDataAllList)DdlNumDogovor.SelectedItem).VtdDataKey);
                }
                else
                {
                    if (_infoNewImportInfo.Count > 0)
                    {
                        Model.GetParamNumberDog(((VtdDataAllList)DdlNumDogovor.SelectedItem).VtdDataKey);
                    }
                    else
                    {
                        //Продолжать импорт на данном участке и под данным договором - нельзя. Уже есть импорт.
                        ControlsIsEnabled(false);
                        //Очищаем текстовые поля блока договора на проведение ВТД
                        ClearVtdDogovorData();
                        string textError = Resources_ImpVtd.tbNoDataDog1
                                           + ((VtdDataAllList)DdlNumDogovor.SelectedItem).NumberContract + ", "
                                           + Resources_ImpVtd.tbNoDataDog2
                                           + ((VtdDataAllList)DdlNumDogovor.SelectedItem).NumberContract
                                           + Resources_ImpVtd.tbNoDataDog3 + ". "
                                           + Resources_ImpVtd.tbNoDataDog4;

                        TbNoDataDog.Text = textError;
                        TbNoDataDog.Visibility = Visibility.Visible;
                        GrdDataDog.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                GrdDataDog.Visibility = Visibility.Collapsed;
                TxtDateContract.Text = "";
                TxtNameWork.Text = "";
                TxtMainExecutor.Text = "";
                TxtSubExecutor.Text = "";
                BtnUploadFile.IsEnabled = false;
                BtnAnalizeFile.IsEnabled = false;
            }
        }
       
        //загрузка файла новая!

        /// <summary>
        /// клик на кнопку загрузка файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUploadFile_OnClick(object sender, RoutedEventArgs e)
        {
            UploadFile();
        }
        
        private void UploadFile()
        {
            string path = HtmlPage.Document.DocumentUri.AbsoluteUri;
            int indexSlash1 = path.LastIndexOf('/');
            path = path.Substring(0, indexSlash1);
            string uri = path + "/RadUploadHandler.ashx";

            RadUpload1.UploadServiceUrl = uri;
            RadUpload1.Filter = "Exel Files (*.xls)|*.xls|All Files(*.*)|*.*"; 
            RadUpload1.ShowFileDialog();
            RadUpload1.StartUpload();
        }

        private void NewImport_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= MainModelPropertyChanged;
        }

        private void RadUpload1_FileUploadFailed(object sender, FileUploadFailedEventArgs e)
        {
            Model.Report("Ошибка загрузки файла на сервер");
            _isloaded = false;
        }

        private void RadUpload1_OnUploadFinished(object sender, RoutedEventArgs e)
        {
            if (_isloaded)
            {
                BtnAnalizeFile.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Ошибка загрузки файла на сервер. Проверьте доступ к каталогу или каталог не найден.");
                BtnAnalizeFile.IsEnabled = false;
            }
        }

        private void RadUpload1_FileUploadStarting(object sender, FileUploadStartingEventArgs e)
        {
            StatusText.Text = e.SelectedFile.Name;
            _fileName = e.SelectedFile.Name;
           
            e.FileParameters.Add("Filename", e.SelectedFile.Name);
        }
        
        /// <summary>
        /// клик на кнопке анализ файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnalizeFile_Click(object sender, RoutedEventArgs e)
        {
            Model.Create_New_ImportVTD(Model.KeyUser, _fileName, "C:\\",
                                       ((VtdDataAllList) DdlNumDogovor.SelectedItem).VtdDataKey);
        }

     
        //Отключаем или включаем контролы формы
        private void ControlsIsEnabled(bool flag)
        {
            BtnAnalizeFile.IsEnabled = flag;
            BtnUploadFile.IsEnabled = flag;
        }

        private void ClearVtdDogovorData()
        {
            TxtDateContract.Text = String.Empty;
            TxtNameWork.Text = String.Empty;
            TxtMainExecutor.Text = String.Empty;
            TxtSubExecutor.Text = String.Empty;
        }

        //клик вперед
        private void BtnForward_OnClick(object sender, RoutedEventArgs e)
        {
            StatusText.IsEnabled = false;
            BtnUploadFile.IsEnabled = false;
            BtnAnalizeFile.IsEnabled = false;
            DdlMg.IsEnabled = DdlThread.IsEnabled = DdlVtdList.IsEnabled = DdlNumDogovor.IsEnabled = false;

            //проверяем правильно залиты словари
            Model.MapsDicts();
        }

        //кнопка назад
        private void BtnBack_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void MainModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //получаем данные лога
            if (e.PropertyName == "GetTxtLog")
            {
                TxtInfoTab2.Text = Model.GetTxtLog;
            }
            //список MG
            if (e.PropertyName == "GetMGList")
            {
                _selectMgList = new List<MG_List_ImpVtd>();

                foreach (MG_List_ImpVtd mg in Model.MgList)
                {
                    _selectMgList.Add(new MG_List_ImpVtd { CNAME = mg.CNAME, NMAIN_GAS_PIPELINE_KEY = mg.NMAIN_GAS_PIPELINE_KEY });
                }

                DdlMg.ItemsSource = null;
                DdlMg.ItemsSource = _selectMgList;

                if (_infoNewImportInfo.Count > 0)
                {
                    foreach (MG_List_ImpVtd mg in _selectMgList)
                    {
                        MG_List_ImpVtd dn = mg;
                        if (dn.NMAIN_GAS_PIPELINE_KEY == _infoNewImportInfo[0].keyMg)
                        {
                            DdlMg.SelectedIndex = DdlMg.Items.IndexOf(mg);
                            DdlMg.IsEnabled = false;
                            break;
                        }
                    }
                }
                else
                {
                    DdlMg.SelectedIndex = DdlMg.Items.IndexOf(_selectMgList[0]);
                }
            }
            //список нитей
            if (e.PropertyName =="GetThreadList")
            {
                _selectThreadList = new List<ThreadForMgListImpVtd>();

                foreach (ThreadForMgListImpVtd thread in Model.ThreadList)
                {
                    _selectThreadList.Add(new ThreadForMgListImpVtd { Name = thread.Name, ThreadKey = thread.ThreadKey });
                }

                DdlThread.ItemsSource = null;
                DdlThread.ItemsSource = _selectThreadList;

                //если известна нить то показываем нужную нить
                if (_infoNewImportInfo.Count > 0)
                {
                    foreach (ThreadForMgListImpVtd thread in _selectThreadList)
                    {
                        ThreadForMgListImpVtd dn = thread;
                        if (dn.ThreadKey == _infoNewImportInfo[0].keyPipe)
                        {
                            DdlThread.SelectedIndex = DdlThread.Items.IndexOf(thread);
                            DdlThread.IsEnabled = false;
                            break;
                        }
                    }
                }
                else
                {
                    DdlThread.SelectedIndex = DdlThread.Items.IndexOf(_selectThreadList[0]);
                }
            }
            //список участков
            if (e.PropertyName == "GetVTDSecList")
            {
                if (Model.VtdSecList.Count != 0)
                {

                    TbNoData.Visibility = Visibility.Collapsed;
                    _selectVtdSec = new List<VtdSecList>();

                    foreach (VtdSecList vtdSec in Model.VtdSecList)
                    {
                        _selectVtdSec.Add(new VtdSecList
                        {
                            NameRegion = vtdSec.NameRegion,
                            VtdSectionKey = vtdSec.VtdSectionKey
                        });
                    }

                    DdlVtdList.ItemsSource = null;
                    DdlVtdList.ItemsSource = _selectVtdSec;

                    //если известен участок то его показываем
                    if (_infoNewImportInfo.Count > 0)
                    {
                        foreach (VtdSecList vtdSec in _selectVtdSec)
                        {
                            VtdSecList dn = vtdSec;
                            if (dn.VtdSectionKey == _infoNewImportInfo[0].keySection)
                            {
                                DdlVtdList.SelectedIndex = DdlVtdList.Items.IndexOf(vtdSec);
                                DdlVtdList.IsEnabled = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        DdlVtdList.SelectedIndex = DdlVtdList.Items.IndexOf(_selectVtdSec[0]);
                    }
                }
                else
                {
                    GrdDataSelect.Visibility = Visibility.Collapsed;
                    TbNoData.Visibility = Visibility.Visible;
                    DdlVtdList.ItemsSource = null;
                }
            }
            //параметры участка
            if (e.PropertyName == "GetVTDSec_ParamList")
            {
                TxtKmStartTab2.Text = Model.VtdSecParamList[0].KmBeginReg.ToString("F3");
                TxtKmEndTab2.Text = Model.VtdSecParamList[0].KmEndReg.ToString("F3");
                TxtKmLenTab2.Text = (Model.VtdSecParamList[0].KmEndReg - Model.VtdSecParamList[0].KmBeginReg).ToString("F3");

                GrdDataSelect.Visibility = Visibility.Visible;
                TbNoData.Visibility = Visibility.Collapsed;
            }
            //список договоров
            if (e.PropertyName == "GetNumDogList")
            {
                if (Model.NumDogList.Count() != 0)
                {
                    _selectNumberDog = new List<VtdDataAllList>();

                    foreach (VtdDataAllList vtdData in Model.NumDogList)
                    {
                        _selectNumberDog.Add(new VtdDataAllList
                        {
                            NumberContract = vtdData.NumberContract,
                            VtdDataKey = vtdData.VtdDataKey,
                            ImpCount = vtdData.ImpCount
                        });
                    }

                    DdlNumDogovor.ItemsSource = null;
                    DdlNumDogovor.ItemsSource = _selectNumberDog;
                    DdlNumDogovor.DisplayMemberPath = "NumberContract";

                    //если известен номер договора то его показываем
                    if (_infoNewImportInfo.Count > 0)
                    {
                        foreach (VtdDataAllList vtdData in _selectNumberDog)
                        {
                            VtdDataAllList dn = vtdData;
                            if (dn.VtdDataKey == _infoNewImportInfo[0].keyContract)
                            {
                                DdlNumDogovor.SelectedIndex = DdlNumDogovor.Items.IndexOf(vtdData);
                                DdlNumDogovor.IsEnabled = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        DdlNumDogovor.SelectedIndex = DdlNumDogovor.Items.IndexOf(_selectNumberDog[0]);
                    }
                }
            }
            //параметры договора
            if (e.PropertyName == "GetParamDog")
            {
                TxtDateContract.Text = "  " + Model.Helper.GetData(Model.ParamDog[0].DateContract);
                TxtNameWork.Text = Resources_ImpVtd.gbNewImportPlaseWork + "  " + Model.Helper.GetData(Model.ParamDog[0].Namework);
                TxtMainExecutor.Text = "  " + Model.Helper.GetData(Model.ParamDog[0].MainExecutor);
                TxtSubExecutor.Text = "  " + Model.Helper.GetData(Model.ParamDog[0].SubExecutor);

                TbNoDataDog.Visibility = Visibility.Collapsed;
                GrdDataDog.Visibility = Visibility.Visible;

            }
            //ошибка с параметрами договора
            if (e.PropertyName == "GetIsValid")
            {
                if (!Model.IsValid)
                {
                    TxtDateContract.Text = "";
                    TxtNameWork.Text = "";
                    TxtMainExecutor.Text = "";
                    TxtSubExecutor.Text = "";
                }
            }
            if (e.PropertyName == "GetKeyImp")
            {
                Model.KeyImport = Model.GetKeyImp;

                //Продолжаем Импорт ВТД. Загружаем Excel-файл с данными ВТД на сервер
                Model.Load_VTDXLSfile(Model.KeyImport, _fileName);

                DdlNumDogovor.IsEnabled = false;
                BtnUploadFile.IsEnabled = false;
                BtnAnalizeFile.IsEnabled = false;
            }
            if (e.PropertyName == "IsValidImportKey")
            {
                if (!Model.IsValidImportKey)
                {
                    //Получаем Лог анализа файла
                    Model.GetImpLog(Model.KeyImport);
                }
            }
            if (e.PropertyName == "Load_VTDXLSfileTrue")
            {
                btnForward.IsEnabled = true;
            }
        }

        private void RadUpload1_OnFilesSelected(object sender, FilesSelectedEventArgs e)
        {
            int indexLastFile = e.SelectedFiles.Count - 1;
            if (e.SelectedFiles[indexLastFile].Size == 0)
            {
                MessageBox.Show(Resources_ImpVtd.cCouldNotLoadFile);
            }
        }
    }
}