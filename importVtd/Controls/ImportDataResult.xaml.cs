using System.Windows;
using System.Windows.Media;
using importVtd.Business;
using importVtd.Resources;

namespace importVtd.Controls
{
    public partial class ImportDataResult
    {
        private MainViewModel Model { get; set; }

        public ImportDataResult(MainViewModel model, bool importStateRun)
        {

            InitializeComponent();

            Model = model;

            Model.PropertyChanged -= MainModelPropertyChanged;
            Model.PropertyChanged += MainModelPropertyChanged;

            if (Model.GetNewImportInfo.Count != 0)
            {
                LblNameSection.Content = Model.GetNewImportInfo[0].cNameVTG + " ( " + Model.GetNewImportInfo[0].nKmBegin
                                                                            + " " + Resources_ImpVtd.cKm 
                                                                            + "  - " + Model.GetNewImportInfo[0].nKmEnd 
                                                                            + " " + Resources_ImpVtd.cKm + " )";
            }

            if (!importStateRun)
            {

                Model.GetStatus();
                LayoutRoot.RowDefinitions[2].Height = new GridLength(0);
                LayoutRoot.RowDefinitions[3].Height = new GridLength(0);

            }
            else
            {
                //запускаем сам импорт
                Model.TypeVkladka = "3"; //запущен импорт

                LblStatusImport.Content = "Процедура импорта запущена";
                LblStatusImport.Foreground = new SolidColorBrush(Colors.Red);

                Model.ImportVTD();
            }
        }

        private void MainModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ImportEnd")
            {
                //статистика импорта
                if (Model.StatistikList.Count > 0)
                {
                    txtNumberTub.Text = Model.StatistikList[0].PipeElemCount;
                    txtNumberMagAnom.Text = Model.StatistikList[0].MagnAnomCount;
                    txtPlotReper.Text = Model.StatistikList[0].RepOnKmCount;
                    txtKoef.Text = Model.StatistikList[0].AvgCoefCount;
                    txtNumberReper.Text = Model.StatistikList[0].UsedRepCount;
                    txtKoorozDefect.Text = Model.StatistikList[0].CorrosionDefect;
                    txtDefectForm.Text = Model.StatistikList[0].FormDefect;
                    txtTrDefect.Text = Model.StatistikList[0].CracklikeDefect;
                    txtStresKor.Text = Model.StatistikList[0].StressCorrosionDefect;
                    txtAnomShov.Text = Model.StatistikList[0].TransverseJointAnomaly;
                    txtNerasAnom.Text = Model.StatistikList[0].AbnormalAnomaly;
                    txtNumberDefect.Text = Model.StatistikList[0].DefectsQty;

                    if (Model.StatusJobType == "-1")
                    {
                        LblStatusImport.Content = Resources_ImpVtd.cImportIsRunning;
                        LblStatusImport.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        if (Model.StatusJobType == "0")
                        {
                            LblStatusImport.Content = Resources_ImpVtd.cImportEndedWithError;
                            LblStatusImport.Foreground = new SolidColorBrush(Colors.Orange);
                        }
                        else
                        {
                            LblStatusImport.Content = Resources_ImpVtd.cDataImportIsEnded;
                            LblStatusImport.Foreground = new SolidColorBrush(Colors.Green);
                        }
                    }
                }
            }
            if (e.PropertyName == "StatusJob")
            {
                if (Model.TypeVkladka == "3")
                {
                    if (Model.StatusJobType == "-1")
                    {
                        LblStatusImport.Content = Resources_ImpVtd.cImportIsRunning;
                        LblStatusImport.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        if (Model.StatusJobType == "0")
                        {
                            LblStatusImport.Content = Resources_ImpVtd.cImportEndedWithError;
                            LblStatusImport.Foreground = new SolidColorBrush(Colors.Orange);
                        }
                        else
                        {
                            LblStatusImport.Content = Resources_ImpVtd.cDataImportIsEnded;
                            LblStatusImport.Foreground = new SolidColorBrush(Colors.Green);
                        }
                    }
                }
            }
            if (e.PropertyName == "BusyIndicatorJobEnd")
            {
                if (Model.IsBusyIndicatorJobEnd)
                {
                    LayoutRoot.RowDefinitions[2].Height = new GridLength(0);
                    LayoutRoot.RowDefinitions[3].Height = new GridLength(0);
                }
            }
            if (e.PropertyName == "GetTxtLog")
            {
                TxtStatistikImportData.Text = Model.GetTxtLog;
            }
        }

        private void ImportDataResult_OnLoaded(object sender, RoutedEventArgs e)
        {
            //Model.IsShow = true;
            ViewModelBisyIndicator v = new ViewModelBisyIndicator {IsBusy = true};
        }
    }
}
