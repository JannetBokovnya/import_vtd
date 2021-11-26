using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using importVtd.Business;
using importVtd.Resources;
using importVtd.startTable;

namespace importVtd.Controls
{
    public partial class StateProcess
    {
        private readonly string _userKey;
        private StatusImport _statusImport;
        private List<ImpVTD_Making_List> _data = new List<ImpVTD_Making_List>();

        private MainViewModel Model { get; set; }

        public StateProcess(string keyUser, MainViewModel model)
        {
            InitializeComponent();

            Model = model;
            //инициализируем во вкладке ключ пользователя
            _userKey = keyUser;

        }

        /// <summary>
        /// клик на столбец - статус импорта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var gr = (sender as UIElement).ParentOfType<GridViewRow>();
            var grItem = ((ImpVTD_Making_List)gr.Item);

            // Отменить восстановление состояния импорта для инного пользователя
            // или при существующем запущенном импорте
            Model.GetStatusJob();
            if (!CanResumeImportState())
            {
                return;
            }

            Model.StateKey = grItem.NSTATEKEY;
            Model.KeyImport = grItem.NIMP_MAKING;

            Model.Report("stateprocess Border_MouseLeftButtonDown обрабатываем клик по столбцу статус импорта");
            Model.GetInfoForNewImport(Model.KeyImport, "NextTabInGrid");
        }

        private bool CanResumeImportState()
        {
            bool canResume = false;

            if (Model.StatusJobType == "-1" && (Model.StateKey == StatusImport.RunningImport || 
                                                Model.StateKey == StatusImport.SuccessImport || 
                                                Model.StateKey == StatusImport.ErrorImport))
            {
                canResume = true;
            }
            else if (Model.StatusJobType != "-1")
            {
                canResume = true;
            }

            return canResume;
        }

        /// <summary>
        /// клик в таблице - пользователь по ключу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void radImpVTD_Making_List_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            //показывать или нет кнопку удалить импорт
            ImpVTD_Making_List selectedRow = (ImpVTD_Making_List)radImpVTD_Making_List.SelectedItem;

            Model.KeyImport = selectedRow.NIMP_MAKING;

            string cStateKey = selectedRow.cStateKey;

            // Удаление импорта не активно для 
            // 6 - импорт запущен
            // 7 - импорт завершен успешно
            // 8 - импорт завершен с ошибкой удалить можно.
            switch (cStateKey)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "8":
                    {
                        buDeleteImport.IsEnabled = true;
                        break;
                    }
                default:
                    {
                        buDeleteImport.IsEnabled = false;
                        break;
                    }
            }
        }

        /// <summary>
        /// удаление импорта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuDeleteImport_OnClick(object sender, RoutedEventArgs e)
        {
            ImpVTD_Making_List selectedRow = (ImpVTD_Making_List)radImpVTD_Making_List.SelectedItem;
            //проверяем ключ пользователя - удалять импорт может тольео тот кто создал
            if (_userKey == selectedRow.userKey)
            {
                MessageBoxResult res = MessageBox.Show(Resources_ImpVtd.msgDelImport, Resources_ImpVtd.msgDelImportAttantion, MessageBoxButton.OKCancel);

                if (res == MessageBoxResult.OK)
                {
                    //запускаем удаление импорта
                    Model.Report("stateprocess BuDeleteImport_OnClick Удаление импорта");
                    Model.DeleteImport(selectedRow.NIMP_MAKING);
                }
            }
            else
            {
                MessageBox.Show(Resources_ImpVtd.cErrUnpermittedDeleteImport);
            }
        }

        /// <summary>
        /// добавить новый импорт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuAddNewImport_OnClick(object sender, RoutedEventArgs e)
        {
            //событие создать новый импорт
            //вначвле проверяем есть ли запущенный импорт
            Model.TypeVkladka = "1"; //запущен если клик на кнопке новый импорт
            Model.GetStatusJob();
        }

        private void StateProcess_OnLoaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= MainModelPropertyChanged;
            Model.PropertyChanged += MainModelPropertyChanged;

            //получаем список импортов
            Model.Report("stateProcess получаем список импортов ");
            Model.GetSatateImport();
        }

        private void MainModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GetSatateImport")
            {
                //соответствие статус импорта и ключ
                _statusImport = new StatusImport();

                _data = Model.ImpVtd;

                for (int i = 0; i < _data.Count(); i++)
                {
                    _data[i].cState = _statusImport.GetStatusImport(_data[i].cStateKey);
                }

                radImpVTD_Making_List.ItemsSource = null;
                radImpVTD_Making_List.ItemsSource = _data;

                //статус джоб
                Model.TypeVkladka = "0"; //запущен начальной страницы
                Model.GetStatusJob();

            }
            if (e.PropertyName == "StatusJob")
            {
                if (Model.StatusJobType == "0" || Model.StatusJobType == "1")
                {
                    //Job процедуры Импорта ВТД не запущен. Можем продолжать процедуру Импорта ВТД.
                    //если вызывали по кнопке добавить новый импорт

                    //  :  0 - остановлено с ошибкой
                    //  :  1 - не существует ("готово к работе" в оригинале в Oracle)
                    //  : -1 - выполняется в данный момент,
                    if (Model.StatusJobType == "0")
                    {
                        lbStatusImport.Content = Resources_ImpVtd.cLastImportWithError;
                        
                        lbStatusImport.Foreground = new SolidColorBrush(Colors.Orange);
                    }
                    else
                    {
                        lbStatusImport.Content = "";
                        lbStatusImport.Foreground = new SolidColorBrush(Colors.Green);
                    }

                    if (Model.TypeVkladka == "1")
                    {
                        Model.FirePropertyChanged("CreateNewImport");
                    }
                    if (Model.TypeVkladka == "2")
                    {
                        //Загрузка формы импорта дефекта
                        Model.FirePropertyChanged("LayoutTubeImportNext");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Model.StatusJob))
                    {
                        for (int i = 0; i < _data.Count(); i++)
                        {
                            if (Model.StatusJob == _data[i].NIMP_MAKING)
                            {
                                string name = _data[i].CFILENAME;
                                lbStatusImport.Content = Resources_ImpVtd.cImportIsRun1 + name + Resources_ImpVtd.cImportIsRun2;
                                lbStatusImport.Foreground = new SolidColorBrush(Colors.Red);
                                return;
                            }
                        }
                    }
                    else
                    {
                        lbStatusImport.Content = Resources_ImpVtd.cRunningImportAnotherDeny;
                        lbStatusImport.Foreground = new SolidColorBrush(Colors.Red);
                    }

                    //ВРЕМЕННО!!!!!!!!!!!!!!!!!!!!
                    if (Model.TypeVkladka == "2")
                    {
                        Model.FirePropertyChanged("LayoutTubeImportNext");
                    }
                }
            }
        }
    }
}