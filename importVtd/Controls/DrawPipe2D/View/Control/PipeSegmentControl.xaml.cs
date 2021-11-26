using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawPipe2D.Classes;
using DrawPipe2D.ViewModel;
using BoolToDoobleConverter = DrawPipe2D.Classes.BoolToDoobleConverter;

namespace DrawPipe2D.View.Control
{
    public partial class PipeSegmentControl : UserControl
    {
        //private const double TITLE_AREA_HEIGHT = 50.0;
        private const double TITLE_AREA_HEIGHT = 0.0;

        private const double COEFFICIENT_COMPRESS_X = 0.5d;
        private ComparisonDefects.Defects _colord;
        private DefectPercentColor.DefectColor _colorCoroz;
        private TypePipe.TypeShov _typeShov;
        private string HintDefect;
        public string typeDefect;

        private PipeControl _parent;

        /// <summary>
        /// DrawSegment(...,1) - рисуем элипс 
        /// DrawSegment(...,2) - рисуем дефект 
        /// </summary>
        public PipeSegmentControl()
        {
            InitializeComponent();
        }

        public PipeSegmentViewModel Model
        {
            get { return DataContext as PipeSegmentViewModel; }
        }

        public double W { get { return (double.IsNaN(Width) || Width == 0.0) ? ActualWidth : Width; } }
        public double H { get { return (double.IsNaN(Height) || Height == 0.0) ? ActualHeight : Height; } }
        private string _colorDefect = "62,62,62";


        public static Path GeneratePath(string data)
        {
            string pathEnvelope =
                "<Path xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Data=\"{0}\"/>";
            return XamlReader.Load(String.Format(pathEnvelope, data)) as Path;
        }

        // private double ElipsRatio { get { return leftElipse.Width / leftElipse.Height; } }
        //private double ElipsRatio { get { return 10d / 60d; } }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //  Draw(1.0);
        }

        public void Draw(double scale, Double radiusTube, ComparisonDefects.Defects colord, DefectPercentColor.DefectColor colorCoroz, TypePipe.TypeShov TypeShov)
        {
            _colord = colord;
            _colorCoroz = colorCoroz;
            _typeShov = TypeShov;

            leftLine.X1 = LineSprav.X2 = LineSprav.Y1 = 0;
            leftLine.Y2 = H - TITLE_AREA_HEIGHT;


            double w = W;

            topLine.X1 = 0;
            topLine.X2 = w;

            bottomLine.X1 = 0;
            bottomLine.X2 = w;

            bottomLine.Y1 = bottomLine.Y2 = H - TITLE_AREA_HEIGHT;
            LineSprav.X1 = LineSprav.X2 = LineSprav.Y1 = 0;
            LineSprav.Y2 = H - TITLE_AREA_HEIGHT;

            LineSprav.X1 = LineSprav.X2 = w;
            LineSprav.Y1 = 0;
            LineSprav.Y2 = H - TITLE_AREA_HEIGHT;


            int t = (int)(H - TITLE_AREA_HEIGHT) / 3;
            canvas.Height = H - TITLE_AREA_HEIGHT;

            txtLenghtPipe.Margin = new Thickness(5, t, 0, 0);


            foreach (var ts in _typeShov.TypeShovList)
            {
                for (int i = 0; i < ts.KeyList.Count; i++)
                {
                    if (Model.Segment.KeyTypePipe == ts.KeyList[i])
                    {
                        if (ts.Id == "1")
                        {
                            CalcPositionShov(scale, radiusTube, Model.Segment.Angle, 1);
                            break;
                        }
                        if (ts.Id == "2")
                        {
                            CalcPositionShov(scale, radiusTube, Model.Segment.Angle, 1);
                            double angle = Model.Segment.Angle + 6;
                            if (angle > 12)
                            {
                                angle = angle - 12;
                            }
                            CalcPositionShov(scale, radiusTube, angle, 2);
                            break;
                        }
                        if (ts.Id == "5")
                        {
                            CalcPositionShov(scale, radiusTube, Model.Segment.Angle, 5);
                            break;
                        }

                    }
                }
            }
        }

        private void DrawElips(double radiusTube, double scale)
        {
            canvasElips.Children.Clear();

            double teta = Math.PI;
            DrawSegment(1, 0, 0, teta, 0d, radiusTube, scale, null, "0", "0", "");
            DrawSegment(1, 0, 0, teta, Model.Segment.Length, radiusTube, scale, null, "0", "0", "");

            DrawSegment(1, Math.PI, 0, teta, 0d, radiusTube, scale, new DoubleCollection() { 2, 2 }, "0", "0", "");
            DrawSegment(1, Math.PI, 0, teta, Model.Segment.Length, radiusTube, scale, new DoubleCollection() { 2, 2 }, "0", "0", "");
        }

        private void DrawDefects(double angledefect, double w, double h, double shiftX, double radiusTube, double scale, string typeDefect, string percentDephdefect, string hintdefect)
        {
            typeDefect = typeDefect;
            percentDephdefect = percentDephdefect;
            //angle = Helper.HaurMinuteToRadian(3, 0);

            double angle;

            string[] arrangle = angledefect.ToString().Split('.');
            if (arrangle.Length == 1)
            {
                angle = DrawPipe2D.Classes.Helper.HaurMinuteToRadian(Convert.ToDouble(arrangle[0]), 0);
            }
            else
            {
                angle = DrawPipe2D.Classes.Helper.HaurMinuteToRadian(Convert.ToDouble(arrangle[0]), Convert.ToDouble(arrangle[1]));
            }

            if (angle > 2 * Math.PI)
            {
                angle = 2 * Math.PI;
            }

            double teta = h / radiusTube;
            if (teta > 2 * Math.PI)
            {
                teta = 2 * Math.PI;
            }

            if (angle < Math.PI) // старт на передней стенке
            {
                if (angle + teta < Math.PI) // передняя стенка
                {
                    DrawSegment(2, angle, w, teta, shiftX, radiusTube, scale, null, typeDefect, percentDephdefect, hintdefect);
                }
                else if (angle + teta > Math.PI && angle + teta <= 2 * Math.PI) //пер стенка, задняя стенка
                {
                    double teta1 = Math.PI - angle;
                    DrawSegment(2, angle, w, teta1, shiftX, radiusTube, scale, null, typeDefect, percentDephdefect, hintdefect);
                    double teta2 = teta - teta1;
                    DrawSegment(2, Math.PI, w, teta2, shiftX, radiusTube, scale, new DoubleCollection() { 2, 2 }, typeDefect, percentDephdefect, hintdefect);
                }
                else
                {
                    double teta1 = Math.PI - angle;
                    DrawSegment(2, angle, w, teta1, shiftX, radiusTube, scale, null, typeDefect, percentDephdefect, hintdefect);
                    double teta2 = Math.PI;
                    DrawSegment(2, Math.PI, w, teta2, shiftX, radiusTube, scale, new DoubleCollection() { 2, 2 }, typeDefect, percentDephdefect, hintdefect);
                    double teta3 = teta - teta2 - teta1;
                    DrawSegment(2, 2 * Math.PI, w, teta3, shiftX, radiusTube, scale, null, typeDefect, percentDephdefect, hintdefect);

                }
            }
            else //старт на задней стенке
            {
                if (angle + teta <= 2 * Math.PI) // задняя стенка
                {
                    DrawSegment(2, angle, w, teta, shiftX, radiusTube, scale, new DoubleCollection() { 2, 2 }, typeDefect, percentDephdefect, hintdefect);
                }
                else if (angle + teta > 2 * Math.PI && angle + teta < 3 * Math.PI)
                {
                    double teta1 = 2 * Math.PI - angle;
                    DrawSegment(2, angle, w, teta1, shiftX, radiusTube, scale, new DoubleCollection() { 2, 2 }, typeDefect, percentDephdefect, hintdefect);
                    double teta2 = teta - teta1;
                    DrawSegment(2, 2 * Math.PI, w, teta2, shiftX, radiusTube, scale, null, typeDefect, percentDephdefect, hintdefect);
                }
                else
                {
                    double teta1 = 2 * Math.PI - angle;
                    DrawSegment(2, angle, w, teta1, shiftX, radiusTube, scale, new DoubleCollection() { 2, 2 }, typeDefect, percentDephdefect, hintdefect);
                    double teta2 = Math.PI;
                    DrawSegment(2, 2 * Math.PI, w, teta2, shiftX, radiusTube, scale, null, typeDefect, percentDephdefect, hintdefect);
                    double teta3 = teta - teta2 - teta1;
                    DrawSegment(2, 3 * Math.PI, w, teta3, shiftX, radiusTube, scale, new DoubleCollection() { 2, 2 }, typeDefect, percentDephdefect, hintdefect);
                }
            }
        }

        private void DrawSegment(int typeElem, double angle, double w, double teta, double shiftX, double radiusTube, double scale, DoubleCollection strokeDashArray, string typeDefect, string percentdefect, string hintdefect)
        {
            w = w * scale;
            shiftX = shiftX * scale;
            radiusTube = radiusTube * scale;
            int npercentDephdefect;
            Int32.TryParse(percentdefect, out npercentDephdefect);

            List<Point> points = new List<Point>();

            int _typeElem = typeElem;
            double tmpAngle = angle;
            double stepAngle = 0.2;

            double x = shiftX + Model.GetProjectionShift(tmpAngle, radiusTube);
            double y = Math.Cos(tmpAngle) * radiusTube;
            y = radiusTube - y;
            Point p = new Point(x, y);
            points.Add(p);

            bool isFinish = false;
            do
            {
                if (tmpAngle + stepAngle < teta + angle)
                {
                    tmpAngle = tmpAngle + stepAngle;
                    p = CalcPoint(tmpAngle, radiusTube, shiftX, Model.ElipsRatio);
                    points.Add(p);

                }
                else
                {
                    stepAngle = (angle + teta) - tmpAngle;
                    tmpAngle = tmpAngle + stepAngle;
                    p = CalcPoint(tmpAngle, radiusTube, shiftX, Model.ElipsRatio);
                    points.Add(p);
                    isFinish = true;
                }

            } while (!isFinish);


            StringBuilder s = new StringBuilder();
            s.Append("M " + (int)points[0].X + " " + (int)points[0].Y + " ");
            //s.Append("M " + points[0].X + " " + points[0].Y + " ");


            for (int i = 1; i < points.Count; i++)
            {
                s.Append(" L " + (int)points[i].X + " " + (int)points[i].Y);
            }


            if (_typeElem == 2)
            {
                // нижняя граница
                s.Append(" L " + (int)(points[points.Count - 1].X + w) + " " + (int)points[points.Count - 1].Y);

                for (int i = points.Count - 2; i >= 0; i--)
                {
                    s.Append(" L " + (int)(points[i].X + w) + " " + (int)points[i].Y);
                }
                s.Append(" Z");
            }


            Path path1 = new Path();
            path1 = GeneratePath(s.ToString());
            var o = path1.GetValue(Path.DataProperty);

            path1.SetValue(Path.DataProperty, o);
            path1.StrokeThickness = 1.0;
            path1.StrokeDashArray = strokeDashArray;
            path1.Tag = hintdefect;
            // path1.MouseEnter += path1_MouseEnter;
            path1.MouseLeave += (path1_MouseLeave);


            if (_typeElem == 2)
            {
                bool bb = true;
                foreach (var cd in _colord.DefectList)
                {
                    for (int i = 0; i < cd.KeyList.Count; i++)
                    {
                        if (typeDefect == cd.KeyList[i])
                        {
                            var cc = _colorCoroz.DefectColorList;
                            _colorDefect = cd.Color;
                            for (int ii = 0; ii < cc.Count; ii++)
                            {
                                if (cd.Id == cc[ii].Parent)
                                {
                                    int min;
                                    Int32.TryParse(cc[ii].MinPercent, out min);
                                    int max;
                                    Int32.TryParse(cc[ii].MaxPercent, out max);

                                    if ((min <= npercentDephdefect) && (npercentDephdefect < max))
                                    {
                                        _colorDefect = cc[ii].Color;
                                        bb = false;
                                        break;
                                    }

                                }
                            }

                        }
                    }
                    if (bb == false)
                        break;
                }

                string[] _colorDefect1 = _colorDefect.Split(',');
                byte R = Convert.ToByte(_colorDefect1[0]);
                byte G = Convert.ToByte(_colorDefect1[1]);
                byte B = Convert.ToByte(_colorDefect1[2]);
                path1.Stroke = new SolidColorBrush(new Color() { A = 150, R = R, G = G, B = B });
                path1.Fill = new SolidColorBrush(new Color() { A = 150, R = R, G = G, B = B });


                EllipseGeometry ellipsDefect = new EllipseGeometry();
                ellipsDefect.Center = new Point(x, y);
                ellipsDefect.RadiusX = 10;
                ellipsDefect.RadiusY = 10;

                //для того что бы удобнее попасть на дефект рисуем окружность
                Path ellipsDefectPath = new Path();
                ellipsDefectPath.Fill = new SolidColorBrush(new Color() { A = 0, R = 255, G = 255, B = 255 });
                ellipsDefectPath.Tag = hintdefect;
                // ellipsDefectPath.MouseEnter += path1_MouseEnter;
                ellipsDefectPath.MouseLeave += path1_MouseLeave;
                ellipsDefectPath.Data = ellipsDefect;
                // canvasDefect.Children.Add(ellipsDefectPath);

                // canvasDefect.Children.Add(path1);
            }
            else
            {
                path1.SetBinding(Path.StrokeProperty, new Binding("LinesBrush"));
                Binding b = new Binding("IsSelected") { Converter = new BoolToDoobleConverter() };
                path1.SetBinding(Path.StrokeThicknessProperty, b);
                canvasElips.Children.Add(path1);
            }
        }



        private void path1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // defectTextGrid.Visibility = Visibility.Collapsed;
        }

        private Point CalcPoint(double angle, double radiusTube, double shiftX, double elipsRatio)
        {
            double x = Math.Sin(angle) * radiusTube * elipsRatio + shiftX;
            double y = Math.Cos(angle) * radiusTube;
            y = radiusTube - y;
            Point p = new Point(x, y);
            return p;
        }


        private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Model.IsSelected = true;
        }

        private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Model.IsSelected = false;
        }



        private void CalcPositionShov(double scale, double radiusTube, double angleprodShov, int typeshov)
        {
            #region createProdolShov

            Line prodolShov2 = new Line();

            double w = W;

            string[] arrangle = Model.Segment.Angle.ToString().Split('.');


            double y_y = (angleprodShov * (H - TITLE_AREA_HEIGHT)) / 12.0;

            switch (typeshov)
            {
                case 1:
                case 2:

                    Line l = new Line
                    {
                        Stroke = new SolidColorBrush(Colors.Red),
                        X1 = 0,
                        X2 = w,
                        Y1 = y_y,
                        Y2 = y_y
                    };

                    //l.Stroke = new SolidColorBrush(new Color() { A = 150, R = 0, G = 0, B = 246 });
                    canvasShov.Children.Add(l);

                    break;

                case 5:
                    canvasShov.Children.Clear();
                    Line ls = new Line
                    {
                        Stroke = new SolidColorBrush(Colors.Red),
                        X1 = 0,
                        X2 = w,
                        Y1 = H - TITLE_AREA_HEIGHT,
                        Y2 = 0
                    };
                    canvasShov.Children.Add(ls);
                    break;

                default:
                    Debug.Assert(true);
                    break;
            }


            #endregion
        }



    }
}

//•	Коррозия до 15%				          салатовый   A = 150, R = 111, G = 255, B = 183
//•	Коррозия 15-30%				          голубой     A = 150, R = 75, G = 160, B = 245
//•	Коррозия 30-50%				          синий       A = 150, R = 20, G = 64, B = 168 
//•	Коррозия > 50%				          красный     A = 150, R = 255, G = 0, B = 0
//•	Дефекты формы				          желтый      A = 150, R = 221, G = 179, B = 11
//•	Трещины					              зеленый     A = 150, R = 0, G = 128, B = 0
//•	Аномальные швы				          красный     A = 150, R = 179, G = 0, B = 0
//•	Технологические дефекты металла	      коричневый  A = 150, R = 64, G = 0, B = 0
//•	Неопределенные дефекты			      темно-серый A = 150, R = 62, G = 62, B = 62
