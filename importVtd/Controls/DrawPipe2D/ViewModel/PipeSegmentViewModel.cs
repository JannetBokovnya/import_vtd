using System;
using System.Net;
using System.Windows;
using System.Windows.Media;
using DrawPipe2D.Classes;

namespace DrawPipe2D.ViewModel
{
    public class PipeSegmentViewModel : BaseViewModel
    {
        private bool _isSelected;
        private string _hint;
        private Point _beginShov;
        private Point _endShov;
        private double _elipsRatio;


        private bool _isVisibleDashLineShov;
        private bool _isVisibleLineShov;
        private bool _isVisibleCanvasDefect;
        private bool _isClicked;
        private bool _isLink;
        private Brush _linesBrush = new SolidColorBrush(Colors.Green);
        private Point _beginbottomLine;
        private Point _endbottomLine;
        private string _lenghtPipe;
        private Brush _canvasBrush = new SolidColorBrush(new Color() { A = 150, R = 211, G = 211, B = 211 });
        private bool _isLinkCanvas;

        //радиус трубы 1м 44 см
        //длина трубы в см
        //длина дефекта в мм


        public PipeSegmentViewModel(PipeSegment segment, double elipsRatio)
        {
            Segment = segment;
            _elipsRatio = elipsRatio;
            Hint = Segment.Hint;
            LenghtPipe = Segment.LenghtPipe;

           // IsVisibleLineShov = segment.VisibleLineShov;
            IsVisibleLineShov = true;
            IsVisibleCanvasDefect = segment.VisibleCanvasDefect;

        }

        public PipeSegment Segment { get; private set; }


        public Double ElipsRatio
        {
            get { return _elipsRatio; }
        }

        public Point BeginbottomLine
        {
            get { return _beginbottomLine; }
            set
            {
                _beginShov = value;
                NotifyPropertyChanged("BeginbottomLine");
            }
        }


        public Point EndbottomLine
        {
            get { return _endbottomLine; }
            set
            {
                _endShov = value;
                NotifyPropertyChanged("EndbottomLine");
            }
        }




        public bool IsVisibleCanvasDefect
        {
            get { return _isVisibleCanvasDefect; }
            set
            {
                _isVisibleCanvasDefect = value;
                NotifyPropertyChanged("IsVisibleCanvasDefect");
            }
        }

        public bool IsVisibleLineShov
        {
            get { return _isVisibleLineShov; }
            set
            {
                _isVisibleLineShov = value;
                NotifyPropertyChanged("IsVisibleLineShov");
            }
        }

        public bool IsVisibleDashLineShov
        {
            get { return _isVisibleDashLineShov; }
            set
            {
                _isVisibleDashLineShov = value;
                NotifyPropertyChanged("IsVisibleDashLineShov");
            }
        }


        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
                CalcLinesBrush();
            }
        }

        public bool IsClicked
        {
            get
            {
                return _isClicked;
            }
            set
            {
                _isClicked = value;

                NotifyPropertyChanged("IsClicked");
                CalcLinesBrush();
            }
        }

        public bool IsLink
        {
            get
            {
                return _isLink;
            }
            set
            {
                _isLink = value;

                NotifyPropertyChanged("IsLink");
                CalcLinesBrush();
            }
        }

        public bool IsLinkCanvas
        {
            get
            {
                return _isLinkCanvas;
            }
            set
            {
                _isLinkCanvas = value;

                NotifyPropertyChanged("IsLinkCanvas");
                CalcLinesBrushCanvas();
            }
        }

        public Brush LinesBrush
        {
            get
            {
                return _linesBrush;
            }
            set
            {
                _linesBrush = value;
                NotifyPropertyChanged("LinesBrush");
            }
        }

        public Brush CanvasBrush
        {
            get
            {
                return _canvasBrush;
            }
            set
            {
                _canvasBrush = value;
                NotifyPropertyChanged("CanvasBrush");
            }
        }

        private void CalcLinesBrush()
        {
            if (IsSelected && !IsClicked)
            {
                LinesBrush = new SolidColorBrush(Colors.Orange);
            }
            else if (IsClicked)
            {
                LinesBrush = new SolidColorBrush(Colors.Red);
                if (IsLink)
                {
                    LinesBrush = new SolidColorBrush(Colors.Purple);
                }
            }
            else if (IsLink)
            {
                LinesBrush = new SolidColorBrush(Colors.Purple);
                
            }
            else
            {
                LinesBrush = new SolidColorBrush(Colors.Green);
                
            }
        }


        private void CalcLinesBrushCanvas()
        {
            if (IsLinkCanvas)
            {
                CanvasBrush = new SolidColorBrush(new Color() { A = 50, R = 255, G = 0, B = 0 });
            }
            else
            {
                CanvasBrush = new SolidColorBrush(new Color() { A = 150, R = 211, G = 211, B = 211 });
            }
        }

        public string Hint
        {
            get
            {
                return _hint;
            }
            set
            {
                _hint = value;
                NotifyPropertyChanged("Hint");
            }
        }

        public string LenghtPipe
        {
            get
            {
                return _lenghtPipe;
            }
            set
            {
                _lenghtPipe = value;
                NotifyPropertyChanged("LenghtPipe");
            }
        }

        public double GetProjectionShift(double angle, double radiusTube)
        {
            return Math.Sin(angle) * radiusTube * ElipsRatio;
        }

    }
}
