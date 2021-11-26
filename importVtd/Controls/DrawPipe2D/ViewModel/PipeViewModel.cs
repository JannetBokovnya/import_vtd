using System;
using System.Collections.Generic;
using System.Net;
using DrawPipe2D.Classes;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Xml.Linq;

namespace DrawPipe2D.ViewModel
{
    public class PipeViewModel : BaseViewModel
    {

        private double _needShift;
        private ComparisonDefects.Defects _colord;
        private DefectPercentColor.DefectColor _corros;
        private TypePipe.TypeShov _typeShov;
        

        public Pipe Pipe { get; private set; }

        public double Scale { get; set; }
        

        

        private double ElipsRatio { get { return 0.25; } }

        public PipeViewModel(Pipe pipe)
        {
            Pipe = pipe;
            SegModelList = new List<PipeSegmentViewModel>();
            LoadXMLFile();
            Init();
        }

        


        public List<PipeSegmentViewModel> SegModelList { get; private set; }

        private void Init()
        {
            
            foreach (PipeSegment pipeSegment in Pipe.SegmentList)
            {
                PipeSegmentViewModel segmentModel = new PipeSegmentViewModel(pipeSegment, ElipsRatio);
                SegModelList.Add(segmentModel);
            }
        }

        public double NeedShift
        {
            get { return _needShift; }
            set
            {
                _needShift = value;
                NotifyPropertyChanged("NeedShift");
            }
        }
        public ComparisonDefects.Defects Colord
        {
            get { return _colord; }
            set
            {
                _colord = value;
                NotifyPropertyChanged("Colord");
            }
        }
        
        public DefectPercentColor.DefectColor Corros
        {
            get { return _corros; }
            set
            {
                _corros = value;
                NotifyPropertyChanged("Corros");
            }
        }

        public TypePipe.TypeShov TypeShov
        {
            get { return _typeShov; }
            set { _typeShov = value; }
        }


        public void ScrollToSegment(string keySegment)
        {
            double shift = 0d;
            foreach (PipeSegmentViewModel psm in SegModelList)
            {
                shift = shift + psm.Segment.Length;
                if (keySegment.Equals(psm.Segment.KeySegment))
                {
                    shift = shift - psm.Segment.Length;
                    break;
                }
            }
            NeedShift = -shift;
        }

        private void LoadXMLFile()
        {
            WebClient xmlClient = new WebClient();
            xmlClient.DownloadStringCompleted += XMLFileLoaded;
            xmlClient.DownloadStringAsync(new Uri("defects.xml", UriKind.RelativeOrAbsolute));
        }

        void XMLFileLoaded(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string xmlData = e.Result;

                Colord = new ComparisonDefects.Defects(xmlData);
                LoadXMLTypePipe();

                //HtmlPage.Window.Alert(xmlData);

            }
        }

        private void LoadXMLTypePipe()
        {
            WebClient xmlClient = new WebClient();
            xmlClient.DownloadStringCompleted += XMLFileLoadedTypePipe;
            xmlClient.DownloadStringAsync(new Uri("TypePipe.xml", UriKind.RelativeOrAbsolute));
        }

        void XMLFileLoadedTypePipe(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string xmlData = e.Result;

                TypeShov  = new TypePipe.TypeShov(xmlData);
                LoadXMLFilecorros();
            }
        }


        private void LoadXMLFilecorros()
        {
            WebClient xmlClient = new WebClient();
            xmlClient.DownloadStringCompleted += XMLFileLoadedCorros;
            xmlClient.DownloadStringAsync(new Uri("KoorozDefact.xml", UriKind.RelativeOrAbsolute));
        }

        void XMLFileLoadedCorros(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string xmlData = e.Result;

                Corros = new DefectPercentColor.DefectColor(xmlData);

                //HtmlPage.Window.Alert(xmlData);
            }
        }



        public int Devide(int p)
        {
            return p / 2;
        }


        
    }
}
