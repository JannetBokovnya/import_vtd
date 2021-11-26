using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using importVtd.Business;
using importVtd.Controls;
using importVtd.startTable;
using Telerik.Windows.Controls;

namespace importVtd
{
    public partial class MainPage
    {
        private string _keyPressed = "";
        private const string Cr = "\r\n";
        private RadListBox rlb = new RadListBox();
        private string _keyVisibleLogPressed = "";

        public MainViewModel Model
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= MainModelPropertyChanged;
            Model.PropertyChanged += MainModelPropertyChanged;

            //версия сервиса
            Model.GetServiceVersion();

            //получаем ключ пользователя по имени
            Model.Report("Получаем ключ пользователя");
            Model.GetKeyUser();

            this.KeyDown -= OnKeyDown;
            this.KeyDown += OnKeyDown;
            Model.FirePropertyChanged("VisibleLog");           
        }

        private void MainModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //обработка события создать новый импорт
            if (e.PropertyName == "CreateNewImport")
            {
                Model.KeyImport = string.Empty;
                //очищаем переменные
                stackPanelNewImport.Child = null;
                stackPanelLinkReper.Child = null;
                tabItemNewImport.IsEnabled  = tabItemLinkReper.IsEnabled = tabCompareTube.IsEnabled = false;
                //очищаем переменные
                Model.GetNewImportInfo = new List<InfoForNewImportVtd>();

                tabItemNewImport.IsEnabled = tabItemNewImport.IsSelected = true;
            }
            //загружаем вкладку новый импорт если по ней кликнули
            if (e.PropertyName == "GetInfoForNewImportVtd")
            {
                stackPanelNewImport.Child = null;
                stackPanelNewImport.Child = (new NewImport(Model.GetNewImportInfo, Model));
                tabItemNewImport.IsEnabled = tabItemNewImport.IsSelected = true;
            }
            if (e.PropertyName == "BackNewImport")
            {
                tabItemDictDefect.IsEnabled = false;
                tabItemNewImport.IsEnabled = tabItemNewImport.IsSelected = true;
                //сохраняем состояние закладки

                for (int i = 0; i < Model.GetBookmark.Count; i++)
                {
                    if (Model.GetBookmark[i].NameBookmark == "3")
                    {
                        Model.GetBookmark.RemoveAt(i);
                        Model.GetBookmark.Add(new StatusBookmark(false, "3"));
                    }
                }
            }
            if (e.PropertyName == "NewImportNext")
            {
                TabLinkRepers tabLinkRepers = new TabLinkRepers(Model)
                {
                    DataContext = Model
                };
                stackPanelLinkReper.Child = tabLinkRepers;
                tabItemLinkReper.IsEnabled = tabItemLinkReper.IsSelected = true;
            }
            //приходит событи  - клик в основной таблице в столбце состояние процесса
            if (e.PropertyName == "NextTabInGrid")
            {
                //очищаем переменные
                stackPanelNewImport.Child = null;
                stackPanelLinkReper.Child = null;
                tabItemNewImport.IsEnabled = tabItemLinkReper.IsEnabled = false;
                NextTabInGrid();
            }
            //событие  - клик следующий в форме увязка реперов
            if (e.PropertyName == "VTD_Available")
            {
                if (Model.VtdAvailable == 1)
                {
                    //открываем вкладку повторный импорт
                    stackPanelCompareTube.Child = null;
                    CompareTubeVtd tabCompareTubeVtd = new CompareTubeVtd(Model);
                    tabCompareTubeVtd.GetTube(Model);
                    stackPanelCompareTube.Child = tabCompareTubeVtd;
                    tabCompareTube.IsEnabled = tabCompareTube.IsSelected = true;
                    tabCompareTube.Visibility = Visibility.Visible;
                }
                else
                {
                    //вкладка трубный журнал
                    stackPanelCompareTube.Child = null;
                    LayoutTube layoutTube = new LayoutTube(Model)
                    {
                        DataContext = Model
                    };
                    stackPanelCompareTube.Child = layoutTube;
                    tabCompareTube.IsEnabled = tabCompareTube.IsSelected = true;
                }
            }

            //клик на кнопку продолжить импорт (кнопка во вкладке трубный журнал)
            if (e.PropertyName == "LayoutTubeImportNext")
            {
                if (borderImportData.Child == null)
                {
                    ImportDataResult importDataResult = new ImportDataResult(Model, true) {DataContext = Model};
                    borderImportData.Child = importDataResult;
                    tabImportData.IsEnabled = tabImportData.IsSelected = true;
                }
                else
                {
                    tabImportData.IsEnabled = tabImportData.IsSelected = true;
                }
            }

            if (e.PropertyName == "GetKeyUser")
            {
                //загружаем первую страницу импорта - состояние процесса
                StateProcess stateProcess = new StateProcess(Model.KeyUser, Model);
                stackPanelStateProcess.Child = stateProcess;
            }
            //скрыть или показать лог
            if (e.PropertyName == "VisibleLog")
            {
                GridAll.RowDefinitions[1].Height = GridAll.RowDefinitions[1].Height.ToString() == "0" ? new GridLength(50) : new GridLength(0);
            }
        }

        private void NextTabInGrid()
        {
            if (Model.StateKey == "0")//Если незавершенный импорт находится в стадии "Создание нового импорта".
            {
                //если новый импорт - то следующую страницу показываем - словари дефектов

                if (stackPanelLinkReper.Child == null)
                {
                    TabLinkRepers tabLinkRepers = new TabLinkRepers(Model)
                    {
                        DataContext = Model
                    };
                    stackPanelLinkReper.Child = tabLinkRepers;
                }

                tabItemNewImport.IsEnabled = tabItemLinkReper.IsEnabled = tabItemLinkReper.IsSelected = true;
            }
            if (Model.StateKey == StatusImport.ChoosingData)//Если незавершенный импорт находится в стадии "Выбор участка".
            {
                if (stackPanelLinkReper.Child == null)
                {
                    TabLinkRepers tabLinkRepers = new TabLinkRepers(Model) 
                    {DataContext = Model};
                    stackPanelLinkReper.Child = tabLinkRepers;
                }

                tabItemNewImport.IsEnabled = tabItemLinkReper.IsEnabled = tabItemLinkReper.IsSelected = true;
            }
            if ((Model.StateKey == StatusImport.LinkingReper))
            {
                TabLinkRepers tabLinkRepers = new TabLinkRepers(Model) 
                {DataContext = Model};
                stackPanelLinkReper.Child = tabLinkRepers;

                tabItemNewImport.IsEnabled = tabItemLinkReper.IsEnabled = tabItemLinkReper.IsSelected = true;
            }
            if ((Model.StateKey == StatusImport.LinkingPipe))
            {
                CompareTubeVtd tabCompareTubeVtd = new CompareTubeVtd(Model);
                tabCompareTubeVtd.GetTube(Model);
                tabCompareTubeVtd.DataContext = Model;
                stackPanelCompareTube.Child = tabCompareTubeVtd;

                TabLinkRepers tabLinkRepers = new TabLinkRepers(Model) 
                {DataContext = Model};
                stackPanelLinkReper.Child = tabLinkRepers;

                tabCompareTube.Visibility = Visibility.Visible;
                tabItemNewImport.IsEnabled = tabItemLinkReper.IsEnabled = tabCompareTube.IsEnabled = tabCompareTube.IsSelected = true;
                
            }
            if ((Model.StateKey == StatusImport.PipeForFirstImport))
            {
                stackPanelLinkReper.Child = null;
                LayoutTube layoutTube = new LayoutTube(Model)
                {
                    DataContext = Model
                };
                stackPanelLinkReper.Child = layoutTube;

                tabItemNewImport.IsEnabled = tabItemLinkReper.IsEnabled = tabCompareTube.IsEnabled = tabCompareTube.IsSelected = true;
            }
            if ((Model.StateKey == StatusImport.SuccessImport)) //если импорт успешно завершен то переходим на вкладку запуск импорта но без поцедуры запуск
            {
                if (borderImportData.Child == null)
                {
                    ImportDataResult importDataResult = new ImportDataResult(Model, false)
                    {
                        DataContext = Model
                    };
                    borderImportData.Child = importDataResult;
                    tabImportData.IsEnabled = tabItemNewImport.IsEnabled = tabImportData.IsSelected = true;
                }
                else
                {
                    tabImportData.IsSelected = tabItemNewImport.IsEnabled = true;
                }
            }
        }

        private void ReportListBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((_keyPressed + e.Key) == "CtrlC")
            {
                string s = reportListBox.SelectedItems.Cast<object>()
                                        .Aggregate(string.Empty, (current, item) => current + item + Cr);

                System.Windows.Clipboard.SetText(s);
                _keyPressed = "";
            }
            else
            {
                _keyPressed = e.Key.ToString();
            }
        }

        private void TabControlBase_OnSelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            if (tabControlBase != null)
            {
                if (tabControlBase.SelectedItem is RadTabItem)
                {
                    var tab = tabControlBase.SelectedItem as RadTabItem;

                    if (tab.Name.Equals("tabItemNewImport", StringComparison.OrdinalIgnoreCase))
                    {
                        //когда вкладка активна и мы на нее нажимаем - вызываем функцию которая по ключу импорта 
                        //показывает ключи мг и т.д.
                        if (String.IsNullOrEmpty(Model.KeyImport))
                        {
                            stackPanelNewImport.Child = null;
                            stackPanelNewImport.Child = (new NewImport(Model.GetNewImportInfo, Model));
                            tabItemNewImport.IsEnabled = tabItemNewImport.IsSelected = true;
                        }
                        else
                        {
                            Model.GetImpLog(Model.KeyImport);
                            if (stackPanelNewImport.Child == null)
                            {
                                Model.GetInfoForNewImport(Model.KeyImport, "tabItemNewImport");     
                            }
                            
                        }
                    }
                    if (tab.Name.Equals("tabImportData", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!String.IsNullOrEmpty(Model.KeyImport))
                        {
                            Model.GetImpLog(Model.KeyImport);
                        }
                    }
                }
            }
        }

        private void MainPage_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (rlb != null)
            {
                GridAll.Children.Remove(rlb);
            }

            Point p = e.GetPosition(this);
            rlb.Items.Clear();
            string[] arrVersion = Model.Version.Split(';');
            for (int i = 0; i < arrVersion.Length - 1; i++)
            {
                rlb.Items.Add(arrVersion[i]);
            }
            rlb.VerticalAlignment = VerticalAlignment.Stretch;
            rlb.HorizontalAlignment = HorizontalAlignment.Stretch;
            double top = ((ActualHeight - p.Y) - (25 * (arrVersion.Length - 1)));
            double right = ((ActualWidth - p.X) - 280);
            rlb.Margin = new Thickness(p.X, p.Y, right, top);
            GridAll.Children.Add(rlb);
        }

      
        private void MainPage_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            GridAll.Children.Remove(rlb);
        }


        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((_keyVisibleLogPressed + e.Key) == "CtrlM")
            {
                Model.FirePropertyChanged("VisibleLog");
                _keyVisibleLogPressed = "";
            }
            else
            {
                _keyVisibleLogPressed = e.Key.ToString();
            }
        }
    }
}