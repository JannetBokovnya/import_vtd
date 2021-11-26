using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using DrawPipe2D.Classes;
using DrawPipe2D.ViewModel;

namespace DrawPipe2D
{
    public partial class MainPage : UserControl
    {
        //IMPORT_API.GetCurrentTubeJournal - 754149703

        public class ContentTable
        {
            public string Npipe_element_montaj_key;//ключ элемента монтажа
            public string Nnum_pipe_part; //номер по отчету ВТД
            public string Loc_km_beg; //Километраж левого края
            public string Nlength;//длинна
            public string Ndepth_pipe;//толщина стенки трубы (определена только если это труба - не тройник и т.п.
            public string CType; //тип (я так понимаю имеется ввиду: труба, тройник ...)
            public string NAngle; //угловое положение продольного св. шва
            public string NCount; //кол-во дефектов
            public string Ntype_pipe_key;

            string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            public ContentTable(string npipe_element_montaj_key, string nnum_pipe_part, string loc_km_beg,
                                string nlength, string ndepth_pipe, string cType, string nAngle, string nCount, string ntype_pipe_key)
            {
                Npipe_element_montaj_key = npipe_element_montaj_key;//ключ элемента монтажа
                Nnum_pipe_part = nnum_pipe_part; //номер по отчету ВТД

                loc_km_beg = loc_km_beg.Replace(".", separator.ToString());
                loc_km_beg = loc_km_beg.Replace(",", separator.ToString());

                nlength = nlength.Replace(".", separator.ToString());
                nlength = nlength.Replace(",", separator.ToString());

                nAngle = nAngle.Replace(".", separator.ToString());
                nAngle = nAngle.Replace(",", separator.ToString());


                Loc_km_beg = loc_km_beg; //Километраж левого края
                Nlength = nlength;//длинна
                Ndepth_pipe = ndepth_pipe;//толщина стенки трубы (определена только если это труба - не тройник и т.п.
                CType = cType; //тип (я так понимаю имеется ввиду: труба, тройник ...)
                NAngle = nAngle; //угловое положение продольного св. шва
                NCount = nCount; //кол-во дефектов
                Ntype_pipe_key = ntype_pipe_key;

            }


        }

        public class DefectListOracle
        {
            public double Nclockwise_pos; //-- Угловое расположение (час)
            public double Nwidth; // -- Ширина (мм)
            public double Nlength; // -- длина(ширина) (мм)
            public double Nprevseam_dist; // -- Расстояние до предыдущего сварного шва  (м)
            public string PersentCorroz; // -- процент глубины коррозии
            public string S_pipe_elem; // --Ключ трубного элемента монтажа
            public string Ndefect_type_key; //ключ типа дефекта
            public string CType; // тип дефекта
            public string CDEPTH_POS; //расположение в глубине металла
            public string Ndepth; //глубина


            public DefectListOracle(double nclockwise_pos, double nwidth, double nlength,
                                    double nprevseam_dist, string persentCorroz, string s_pipe_elem,
                                    string ndefect_type_key, string cType, string cDEPTH_POS, string ndepth)
            {
               Nclockwise_pos = nclockwise_pos;
               Nwidth = nwidth;
               Nlength = nlength;
               Nprevseam_dist=nprevseam_dist;
               PersentCorroz = persentCorroz; 
               S_pipe_elem = s_pipe_elem;
               Ndefect_type_key = ndefect_type_key;
                CType = cType;
                CDEPTH_POS = cDEPTH_POS;
                Ndepth = ndepth;
            }


        }

        private PipeViewModel _model;
        public Pipe pipe = new Pipe();
       // private DefectPercentColor.DefectColor colord1;
       // private ComparisonDefects.Defects colord;

        public List<ContentTable> ContentTableList { get; private set; }
        public List<DefectListOracle> DefectOracleList { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            pipeControl.SegmentClicked += OnSegmenClicked;
            //LoadXMLFile2();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            ContentTableList = new List<ContentTable>();
            ContentTable contentTable;
            contentTable = new ContentTable("1201083762503", "14280", "871660.9989", "11.7527", "15.7",
                                                         "Двухшовная", "9.1", "2", "986101");
            ContentTableList.Add(contentTable);

            contentTable = new ContentTable("1201115680303", "109680", "979772.8954", "11.3283", "19.1",
                                                         "Двухшовная", "4.7", "1", "986101");
            ContentTableList.Add(contentTable);

            contentTable = new ContentTable("1201107664603", "85970", "952641.8771", "11.8469", "16.8",
                                             "Двухшовная", "1.3", "4", "986101");
            ContentTableList.Add(contentTable);

            contentTable = new ContentTable("1201090842203", "35180", "895810.8468", "11.2319", "15.7",
                                             "Спиралешовная", "1.3", "1", "986401");
            ContentTableList.Add(contentTable);

            contentTable = new ContentTable("1201096851203", "53270", "915899.5953", "11.2137", "15.7",
                                            "Спиралешовная", "7.8", "2", "986401");
            ContentTableList.Add(contentTable);



            DefectOracleList = new List<DefectListOracle>();
            DefectListOracle defectListOracle;
            defectListOracle = new DefectListOracle(8, 0, 0.79, 0, "0", "1201083762503", "93428401", "аномальный шов", "неопределенно", "0.0032");
            DefectOracleList.Add(defectListOracle);
            defectListOracle = new DefectListOracle(5.2, 0, 0.63, 0, "0", "1201083762503", "93428401", "аномальный шов", "неопределенно", "0.0032");
            DefectOracleList.Add(defectListOracle);

            defectListOracle = new DefectListOracle(4.7, 0.1, 0.03, 7.3816731590062,
                "0", "1201115680303", "93428401", "язва", "на внутренней поверхности трубы", "0.0032");
            //7.3816731590062,
            DefectOracleList.Add(defectListOracle);

            defectListOracle = new DefectListOracle(2, 1.91, 0.07, 1.4921048456167,
                "10", "1201107664603", "225501", "язва", "на внутренней поверхности трубы", "0.0032");
            DefectOracleList.Add(defectListOracle);

            defectListOracle = new DefectListOracle(1.1, 0, 0.913, 0,
                "0", "1201090842203", "93428401", "аномальный шов", "неопределенно", "0");
            DefectOracleList.Add(defectListOracle);

            defectListOracle = new DefectListOracle(9.6, 0.024, 0.156, 11.001758901721,
                "12.74", "1201096851203", "16958100903", "поперечная риска", "внешнее", "0.002");
            DefectOracleList.Add(defectListOracle);

            defectListOracle = new DefectListOracle(6.9, 0.03, 0.109, 11.0168989369068,
                "12.74", "1201096851203", "16958100903", "поперечная риска", "внешнее", "0.002");
            DefectOracleList.Add(defectListOracle);




            //pipe.LoadData(ContentTableList, DefectOracleList);

            



            _model = new PipeViewModel(pipe);
            pipeControl.DataContext = _model;
            pipeControl.Init();

            //pipeControl.Draw(0, _model.Colord);

        }



        private void OnSegmenClicked(object sender, ClickSegmentEventArgs e)
        {
            textBlock.Text = e.Model.Segment.KeySegment;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string _keySegment = "100";
            _model.ScrollToSegment(_keySegment);
            ////_model.SegModelList[20].IsClicked = true;
            foreach (var segModel in _model.SegModelList)
            {
                if (segModel.Segment.KeySegment == _keySegment)
                {
                    segModel.IsClicked = true;
                }

            }

        }

        //private void LoadXMLFile2()
        //{
        //    WebClient xmlClient = new WebClient();
        //    xmlClient.DownloadStringCompleted += XMLFileLoaded;
        //    xmlClient.DownloadStringAsync(new Uri("KoorozDefact.xml", UriKind.RelativeOrAbsolute));
        //}

        //void XMLFileLoaded(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    if (e.Error == null)
        //    {
        //        string xmlData = e.Result;

        //        colord1 = new DefectPercentColor.DefectColor(xmlData);


        //        //HtmlPage.Window.Alert(xmlData);
        //    }
        //}
    }
}
