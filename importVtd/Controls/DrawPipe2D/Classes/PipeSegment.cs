using System;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Windows.Shapes;
using DrawPipe2D.ViewModel;

namespace DrawPipe2D.Classes
{
    public class PipeSegment
    {
        public double StartX { get; set; }
        public double EndX { get; set; }
        public string Hint { get; set; }
        public string Km { get; set; }
        public double Angle { get; set; }
        public double TubeRadius { get; set; }
        public string KeySegment { get; set; }
        public string KeyTypePipe { get; set; }
        public bool VisibleCanvasDefect { get; set; }
        public bool VisibleLineShov { get; set; }
        public string LenghtPipe { get; set; }

        public PipeSegment(double startX, double endX, string hint, string km, double angle, double tubeRadius, string keySegment, bool visibleCanvasDefect, bool visibleLineShov, string keytypePipe, string lenghtPipe)
        {
            StartX = startX;
            EndX = endX;
            Hint = hint;
            Km = km;
            Angle = angle;
            TubeRadius = tubeRadius;
            KeySegment = keySegment;
            KeyTypePipe = keytypePipe;
            VisibleCanvasDefect = visibleCanvasDefect;
            VisibleLineShov = visibleLineShov;
            LenghtPipe = lenghtPipe;
            DefectList = new List<Defect>();
        }
        
        public List<Defect> DefectList { get; private set; }


        public double Length
        {
            get
            {
                return Math.Abs(EndX - StartX);
            }
        }
        public static Path GeneratePath(string data)
        {
            string pathEnvelope =
                "<Path xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Data=\"{0}\"/>";
            return XamlReader.Load(String.Format(pathEnvelope, data)) as Path;
        }
    }
}
