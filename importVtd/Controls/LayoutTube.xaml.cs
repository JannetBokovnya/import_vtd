using System.Collections.Generic;
using System.Windows;
using importVtd.Business;
using importVtd.Resources;
using importVtd.startTable;

namespace importVtd.Controls
{
    public partial class LayoutTube
    {
        public MainViewModel Model { get; private set; }
        private List<TubeJournalTableList> tubeJournals = new List<TubeJournalTableList>();

        public LayoutTube(MainViewModel model)
        {
            InitializeComponent();
            Model = model;

            Model.PropertyChanged -= MainModelPropertyChanged;
            Model.PropertyChanged += MainModelPropertyChanged;

            if (Model.GetNewImportInfo.Count != 0)
            {
                lbreperSection.Content = Model.GetNewImportInfo[0].cNameVTG + " ( " + Model.GetNewImportInfo[0].nKmBegin +
                                         " " + Resources_ImpVtd.cKm + "  - " + Model.GetNewImportInfo[0].nKmEnd + " км )";    
            }

            Model.GetTubeJournal(Model.KeyImport);
        }

        private void LayoutTube_OnLoaded(object sender, RoutedEventArgs e)
        { 
        }

        private void MainModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GetTubeJournalList")
            {
                tubeJournals = new List<TubeJournalTableList>(Model.TubeJournalList);
                if (tubeJournals.Count == 0)
                {
                    Model.Report("не удалось наполнить  список  труб = 0");
                }
                GrdLayoutTube.ItemsSource = tubeJournals;
            }
        }

        private void BuNext_OnClick(object sender, RoutedEventArgs e)
        {
            if (tubeJournals.Count == 0)
            {
                MessageBox.Show("Не удалось получить трубный журнал, дальнейший импорт не возможен.");
            }
            else
            {
                //сделать проверку джоба на этот импорт - если джою запущен то сразу перейти на вкладку импорт
                Model.TypeVkladka = "2"; //запущен из страницы трубный журнал
                Model.GetStatusJob();
            }
        }
    }
}
