using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Text;
using DrawPipe2D.Classes;
using DrawPipe2D.ViewModel;
using Telerik.Windows;
using Telerik.Windows.Controls.GridView;
using importVtd.Business;
using importVtd.Resources;
using importVtd.startTable;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace importVtd.Controls
{
    public partial class CompareTubeVtd
    {
        private int _countItemUnBoundedTem;
        private readonly SolidColorBrush _colorLinkedPipe = new SolidColorBrush(Colors.Green);

        private void ResetItemPosition()
        {
            _countItemUnBoundedTem = 0;
        }

        private List<DrawPipe2D.MainPage.ContentTable> ContentTableList { get; set; }
        private List<DrawPipe2D.MainPage.ContentTable> ContentTableListRight { get; set; }

        private readonly Pipe _pipe = new Pipe();
        private readonly Pipe _pipeFile = new Pipe();

        private MainViewModel Model { get; set; }
        private PipeViewModel _model;
        private PipeViewModel _modelFile;

        private string _keySegmentTubeLeft = "";
        private string _keySegmentTubeRight = "";
        private const bool MarkTube = true;
        private const bool MarkTubeFile = true;
        private readonly IoraWCFService_ImpVtdClient _proxy = new IoraWCFService_ImpVtdClient();


        private ObservableCollection<RelatedTEM> RelatedTemList { get; set; }

        public CompareTubeVtd(MainViewModel model)
        {
            InitializeComponent();

            Model = model;
            PipeControlBd.SegmentClicked += OnSegmenClicked;
            PipeControlFile.SegmentClicked += OnSegmentClickedFile;
        }

        public void GetTube(MainViewModel model)
        {
            Model = model;

            if (Model.GetNewImportInfo.Count != 0)
            {
                LbCompareTube.Content = Model.GetNewImportInfo[0].cNameVTG + " ( " + Model.GetNewImportInfo[0].nKmBegin 
                                                                           +  " " + Resources_ImpVtd.cKm + " - " 
                                                                           + Model.GetNewImportInfo[0].nKmEnd + " " 
                                                                           + Resources_ImpVtd.cKm + " )";
            }

            Model.PropertyChanged -= MainModelPropertyChanged;
            Model.PropertyChanged += MainModelPropertyChanged;


            RelatedTemList = new ObservableCollection<RelatedTEM>();
            Model.GetLeftTable(Model.KeyImport);

        }


        private void MainModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GetDBTubeBazaList")
            {
                List<TubeBaza> table = new List<TubeBaza>(Model.DbListTubeBaza);
                if (table.Count == 0)
                {
                    MessageBox.Show(Resources_ImpVtd.linkNoReperBD, Resources_ImpVtd.Information, MessageBoxButton.OK);
                    Model.Report("не удалось наполнить  список  трубного журнала из бд = 0");
                    //непонятно что делать если нет массива
                }
                else
                {
                    //заполняем таблицу
                    GrdTubeBdLeft.ItemsSource = table;
                    //рисуем трубы (данные из базы)
                    InitDrowPipeLeft(table);
                }
            }
            if (e.PropertyName == "GetDBTubeFileList")
            {
                List<TubeBaza> table = new List<TubeBaza>(Model.DbListTubeFile);
                if (table.Count == 0)
                {
                    MessageBox.Show(Resources_ImpVtd.linkNoReperBD, Resources_ImpVtd.Information, MessageBoxButton.OK);
                    Model.Report("не удалось наполнить  список  трубного журнала из фийла = 0");
                    //непонятно что делать если нет массива
                }
                else
                {
                    GrdTubeFileRight.ItemsSource = table;
                    InitdrowPipeRight(table);
                }
            }
            if (e.PropertyName == "ImportTubeMatchingOneRow")
            {
                GetlinkTemTable(Model.ListKeyBounds);
            }

            //таблица с увязанными темами
            if (e.PropertyName == "GetTemMapping")
            {
                if (Model.TemMappingList.Count > 0)
                {
                    GetlinkTemTable(Model.TemMappingList);
                }
                else
                {
                    GrdTubeBound.ItemsSource = null;
                }

                SetActiveLinkButton();
            }
            if (e.PropertyName == "RemoveItem")
            {
                // снимаем подсветки с левой таблицы и невидимые строки в правой и переначитываем таблицу

                foreach (var item in GrdTubeBdLeft.Items)
                {
                    var row1 = GrdTubeBdLeft.ItemContainerGenerator.ContainerFromItem(item) as GridViewRow;
                    if (row1 != null)
                    {
                        row1.Background = null;
                    }
                }

                foreach (var segModelFile in _modelFile.SegModelList)
                {
                    segModelFile.IsLink = false;
                    segModelFile.IsLinkCanvas = false;

                }

                AddFilterToGrid(GrdTubeFileRight, "LocKmBeg");

                foreach (var segModel in _model.SegModelList)
                {
                    segModel.IsLink = false;
                    segModel.IsLinkCanvas = false;
                }

                //при удалении связи очищаем данные в таблице
                RelatedTemList = new ObservableCollection<RelatedTEM>();

                Model.GetTemMapping();
            }
        }

        //выделяем цветом увязанные темы
        private void GetlinkTemTable(List<KeyBound> listKeyBoundsAll)
        {
            List<KeyBound> listKeyBounds = new List<KeyBound>(listKeyBoundsAll);

            TubeBaza bd = new TubeBaza();
            TubeBaza file = new TubeBaza();

            RelatedTemList.Clear();
            GrdTubeFileRight.FilterDescriptors.Clear();

            List<TubeBaza> temDb = GrdTubeBdLeft.ItemsSource as List<TubeBaza>;
            List<TubeBaza> temFile = GrdTubeFileRight.ItemsSource as List<TubeBaza>;

            GrdTubeBdLeft.UnselectAll();

            foreach (var itemKeyBounds in listKeyBounds)
            {
                bd = new TubeBaza();
                file = new TubeBaza();

                // выделяем цветом увязанные тзмы
                if (!String.IsNullOrEmpty(itemKeyBounds.KeyBD))
                {
                    TubeBaza pipeDb = temDb.First(x => x.PipeElementMontajKey == itemKeyBounds.KeyBD);

                    if (pipeDb != null)
                    {
                        bd = pipeDb;

                        // Задаю выделение уже увязанных ТЕМов из БД
                        GridViewRow row1 = GrdTubeBdLeft.ItemContainerGenerator.ContainerFromItem(bd) as GridViewRow;
                        if (row1 != null)
                        {
                            row1.Background = _colorLinkedPipe;
                        }
                        
                        // Задаю выделение для увязанных ТЕМов в мультике
                        PipeSegmentViewModel pipeSegment =
                            _model.SegModelList.First(x => x.Segment.KeySegment == itemKeyBounds.KeyBD);
                        if (pipeSegment != null)
                        {
                            pipeSegment.IsLink = true;
                            pipeSegment.IsLinkCanvas = true;
                        }
                    }
                }

                // Поиск соответствующего ТЕМа из файла
                TubeBaza pipeFile = temFile.First(x => x.PipeElementMontajKey == itemKeyBounds.KeyFile);
                if (pipeFile != null)
                {
                    file = pipeFile;
                }

                RelatedTemList.Add(
                (new RelatedTEM(bd.PipeElementMontajKey, bd.LocKmBeg, bd.Length, bd.DepthPipe, bd.Type, bd.Angle, bd.NumPipePart,
                    file.PipeElementMontajKey, file.LocKmBeg, file.Length, file.DepthPipe, file.Type, file.Angle, file.NumPipePart)));

            }


            var lastPipeFile = listKeyBounds.Max(x => x.KeyFile);
            if (lastPipeFile != null)
            {
                TubeBaza pipeFile = temFile.First(x => x.PipeElementMontajKey == lastPipeFile);
                if (pipeFile != null)
                {
                    AddFilterToGrid(GrdTubeFileRight, "LocKmBeg", pipeFile.LocKmBeg);
                }
            }

            GrdTubeBound.ItemsSource = null;
            GrdTubeBound.ItemsSource = RelatedTemList;

            SetActiveLinkButton();
        }

        private void AddFilterToGrid(RadGridView rgv, string columnName, string valueKey = "-1")
        {
            rgv.FilterDescriptors.Clear();

            if (valueKey != "-1")
            {
                GridViewColumn cityColumn = rgv.Columns[columnName];
                IColumnFilterDescriptor columnDescriptor = cityColumn.ColumnFilterDescriptor;
                columnDescriptor.SuspendNotifications();
                columnDescriptor.FieldFilter.Filter1.Operator = FilterOperator.IsGreaterThan;
                columnDescriptor.FieldFilter.Filter1.Value = valueKey;
                columnDescriptor.ResumeNotifications(); 
            }          
        }

        /// <summary>
        /// рисуем трубу (данные из базы)
        /// </summary>
        /// <param name="tableLeft"></param>
        private void InitDrowPipeLeft(List<TubeBaza> tableLeft)
        {
            ContentTableList = new List<DrawPipe2D.MainPage.ContentTable>();
            DrawPipe2D.MainPage.ContentTable contentTable;

            foreach (TubeBaza tubeBaza in tableLeft)
            {
                contentTable = new DrawPipe2D.MainPage.ContentTable(tubeBaza.PipeElementMontajKey, 
                                                                    tubeBaza.NumPipePart,
                                                                    tubeBaza.LocKmBeg, 
                                                                    tubeBaza.Length, 
                                                                    tubeBaza.DepthPipe,
                                                                    tubeBaza.Type, 
                                                                    tubeBaza.Angle, 
                                                                    tubeBaza.Count, 
                                                                    tubeBaza.TypePipeKey);

                ContentTableList.Add(contentTable);
            }

            _pipe.LoadData(ContentTableList);

            _model = new PipeViewModel(_pipe);
            PipeControlBd.DataContext = _model;
            PipeControlBd.Init();
        }

        private void InitdrowPipeRight(List<TubeBaza> tableRight)
        {
            ContentTableListRight = new List<DrawPipe2D.MainPage.ContentTable>();

            foreach (TubeBaza tubeBaza in tableRight)
            {
                var contentTableRight = new DrawPipe2D.MainPage.ContentTable(tubeBaza.PipeElementMontajKey, 
                                                                             tubeBaza.NumPipePart,
                                                                             tubeBaza.LocKmBeg, 
                                                                             tubeBaza.Length, 
                                                                             tubeBaza.DepthPipe,
                                                                             tubeBaza.Type, 
                                                                             tubeBaza.Angle, 
                                                                             tubeBaza.Count, 
                                                                             tubeBaza.TypePipeKey);

                ContentTableListRight.Add(contentTableRight);
            }

            _pipeFile.LoadData(ContentTableListRight);

            _modelFile = new PipeViewModel(_pipeFile);
            PipeControlFile.DataContext = _modelFile;
            PipeControlFile.Init();
        }

        private void SetActiveLinkButton()
        {
            bool isBtnEnable = GrdTubeBdLeft.SelectedItem != null && GrdTubeFileRight.SelectedItem != null;

            BtnBind.IsEnabled = isBtnEnable;
            BtnBindAuto.IsEnabled = (GrdTubeBound.Items.Count > 0) && //Автоузязывание доступно после первой ручной
                                    (GrdTubeBdLeft.Items.Count > 0) && // есть ли что увязывать
                                    (GrdTubeFileRight.Items.Count > 0);

        }

        private void grdTubeBDLeft_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            SetActiveLinkButton();

            if (GrdTubeBdLeft.SelectedItem != null)
            {
                string selectedRow = ((TubeBaza)GrdTubeBdLeft.SelectedItem).PipeElementMontajKey;
                DrawTemFromDb(selectedRow);
            }
        }

        private void GrdTubeFileRight_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            SetActiveLinkButton();

            if (GrdTubeFileRight.SelectedItem != null)
            {
                var selectedRowFile = ((TubeBaza)GrdTubeFileRight.SelectedItem).PipeElementMontajKey;

                DrawTemFromFile(selectedRowFile);
            }
        }

        /// <summary>
        /// увязать трубы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBind_OnClick(object sender, RoutedEventArgs e)
        {
            //предел отклонения увязки длины труб??? - ориентирововчно берем 0
            double deviation;
            double.TryParse("0", out deviation);

            //отправляем в базу данные на связывание , если связались ок, то наполняем таблицу связанных элементов по строке... 
            //+ в таблице красим другим цветом увязанные строки
            StringBuilder mappedKeys = new StringBuilder();
            mappedKeys.Append(((TubeBaza)GrdTubeBdLeft.SelectedItem).PipeElementMontajKey + "," +
                                ((TubeBaza)GrdTubeFileRight.SelectedItem).PipeElementMontajKey + ";");

            ResetItemPosition();

            //вызываем процедуру добавления ручного добавления ключей в базе
            AddBoundedCurrentRow(Model.KeyImport, ((TubeBaza)GrdTubeFileRight.SelectedItem).PipeElementMontajKey,
                                 ((TubeBaza)GrdTubeBdLeft.SelectedItem).PipeElementMontajKey, 1);
        }

        /// <summary>
        /// увязать трубы автоматом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBindAuto_OnClick(object sender, RoutedEventArgs e)
        {
            GrdTubeFileRight.SelectedItems.Clear();
            ResetItemPosition();
            //вызываем процедуру увязки труб автоматом в базе
            //typeLink = 2 - увязать автоматом
            AddBoundedCurrentRow(Model.KeyImport, "", "", 2);
        }

        /// <summary>
        /// увязать без сопоставления - передаем только ключ трубы из базы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNoBind_OnClick(object sender, RoutedEventArgs e)
        {
            ResetItemPosition();
            List<TubeBaza> lstTemsFromFile = GrdTubeFileRight.SelectedItems.Cast<TubeBaza>().OrderBy(x => x.PipeElementMontajKey).ToList();
            foreach (var row in lstTemsFromFile)
            {
                AddBoundedCurrentRow(Model.KeyImport, (row).PipeElementMontajKey, null, 3);
            }
        }

        //увязываем пару ключей //увязка автоматом
        private void AddBoundedCurrentRow(string keyImport, string filekey, string dbKey, int typeLink)
        {
            //показать индикатор занятости
            Model.IsShowBusy = true;

            _proxy.ImportTubeMatchingCompleted -= Proxy_ImportTubeMatchingCompleted;
            _proxy.ImportTubeMatchingCompleted += Proxy_ImportTubeMatchingCompleted;
            _proxy.ImportTubeMatchingAsync(keyImport, filekey, dbKey, typeLink, _proxy);
        }

        private void Proxy_ImportTubeMatchingCompleted(object sender, ImportTubeMatchingCompletedEventArgs e)
        {
            //Обновление таблицы с увязанными темами только при последнем выбранном ТЕМе
            //GrdTubeFileRight.SelectedItems.Count > 0 
            _countItemUnBoundedTem++;
            if (GrdTubeFileRight.SelectedItems.Count > 0 &&
                _countItemUnBoundedTem != GrdTubeFileRight.SelectedItems.Count)
                return;

            Model.Report(e.Result.ErrorMessage);

            //Не выводить информационные сообщение, которые создаются в сервисе.
            if (!e.Result.ErrorMessage.StartsWith("ImportTubeMatching"))
            {
                if (e.Result.IsValid)
                    MessageBox.Show(e.Result.ErrorMessage, Resources_ImpVtd.Information, MessageBoxButton.OK);
                else
                    MessageBox.Show(e.Result.ErrorMessage, Resources_ImpVtd.Error, MessageBoxButton.OK);
            }

            //убрать индикатор занятости
            Model.IsShowBusy = false;

            Model.GetTemMapping();
        }

        /// <summary>
        /// убрать связь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUndo_OnClick(object sender, RoutedEventArgs e)
        {
            if (GrdTubeBound.SelectedItems.Count != 0)
            {
                // Поиск первого выбранного ТЕМа для отмены увязывания
                var linkedTems = GrdTubeBound.SelectedItems.Cast<RelatedTEM>().ToList();
                string minTemKey = linkedTems.Min(x => x.KeyFile);

                Model.DeleteBounded(Model.KeyImport, minTemKey);
            }

            SetActiveLinkButton();
        }

        private void OnSegmenClicked(object sender, ClickSegmentEventArgs e)
        {
            foreach (var item in GrdTubeBdLeft.Items)
            {
                var gp = item as TubeBaza;
                if (gp != null && String.Equals(gp.PipeElementMontajKey, e.Model.Segment.KeySegment))
                {
                    GrdTubeBdLeft.SelectedItem = item;
                    GrdTubeBdLeft.ScrollIntoView(item);
                    break;
                }
            }
        }

        private void OnSegmentClickedFile(object sender, ClickSegmentEventArgs e)
        {
            foreach (var item in GrdTubeFileRight.Items)
            {
                var gp = item as TubeBaza;
                if (gp != null && String.Equals(gp.PipeElementMontajKey, e.Model.Segment.KeySegment))
                {
                    var row2 = GrdTubeFileRight.ItemContainerGenerator.ContainerFromItem(item) as GridViewRow;
                    if (row2 != null)
                    {
                        if (row2.Visibility == Visibility.Visible)
                        {
                            GrdTubeFileRight.SelectedItem = item;
                            GrdTubeFileRight.ScrollIntoView(item);
                            break;
                        }
                    }
                }
            }
        }

        private void BuNext_OnClick(object sender, RoutedEventArgs e)
        {
            //сделать проверку джоба на этот импорт - если джою запущен то сразу перейти на вкладку импорт
            Model.TypeVkladka = "2"; //запущен из страницы трубный журнал
            Model.GetStatusJob();
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Model.FirePropertyChanged("NewImportNext");
        }

        private void RadExpander_OnCollapsed(object sender, RadRoutedEventArgs e)
        {
            LayoutRoot.RowDefinitions[1].Height = new GridLength(30);
        }

        private void RadExpander_OnExpanded(object sender, RadRoutedEventArgs e)
        {
            if (LayoutRoot != null)
            {
                LayoutRoot.RowDefinitions[1].Height = new GridLength(150);    
            }
        }

        private void GrdTubeBdLeft_OnRowLoaded(object sender, RowLoadedEventArgs e)
        {
            var loadedRow = e.Row;
            var tubeBaza = (TubeBaza)loadedRow.Item;

            var countFoundedTems = RelatedTemList.Where(x => x.KeyDb == tubeBaza.PipeElementMontajKey);

            loadedRow.Background = (countFoundedTems.Any()) ? _colorLinkedPipe : null;
        }

        private void GrdTubeBound_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                RelatedTEM item = (RelatedTEM) e.AddedItems[0];

                DrawTemFromDb(item.KeyDb);
                DrawTemFromFile(item.KeyFile);
            }

            SetActiveLinkButton();
        }

        private void DrawTemFromDb(string selectedRowDb)
        {
            if (_model != null && !String.IsNullOrEmpty(selectedRowDb))
            {

                _model.ScrollToSegment(selectedRowDb);

                if (_keySegmentTubeLeft != "")
                {
                    foreach (var segModel in _model.SegModelList)
                    {
                        if (segModel.Segment.KeySegment == _keySegmentTubeLeft)
                        {
                            segModel.IsClicked = false;
                            _keySegmentTubeLeft = "";
                            break;
                        }
                    }
                }

                foreach (var segModel in _model.SegModelList)
                {
                    if (segModel.Segment.KeySegment == selectedRowDb)
                    {
                        if (MarkTube)
                        {
                            segModel.IsClicked = true;
                            segModel.IsLink = false;
                            _keySegmentTubeLeft = selectedRowDb;
                            break;
                        }
                        else
                        {
                            segModel.IsClicked = false;
                            break;
                        }
                    }
                }
            }
        }

        private void DrawTemFromFile(string selectedRowFile)
        {
            if (_modelFile != null && 
                !String.IsNullOrEmpty(selectedRowFile))
            {

                _modelFile.ScrollToSegment(selectedRowFile);

                if (_keySegmentTubeRight != "")
                {

                    foreach (var segModelFile in _modelFile.SegModelList)
                    {
                        if (segModelFile.Segment.KeySegment == _keySegmentTubeRight)
                        {
                            segModelFile.IsClicked = false;
                            _keySegmentTubeRight = "";
                            break;
                        }
                    }
                }

                foreach (var segModelFile in _modelFile.SegModelList)
                {
                    if (segModelFile.Segment.KeySegment == selectedRowFile)
                    {
                        if (MarkTubeFile)
                        {
                            segModelFile.IsClicked = true;
                            segModelFile.IsLink = false;
                            _keySegmentTubeRight = selectedRowFile;
                            break;
                        }
                        else
                        {
                            segModelFile.IsClicked = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}
