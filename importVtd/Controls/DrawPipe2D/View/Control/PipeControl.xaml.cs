using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DrawPipe2D.Classes;
using DrawPipe2D.ViewModel;
using importVtd.Resources;

namespace DrawPipe2D.View.Control
{
    public partial class PipeControl : UserControl
    {
        private bool isDragging;
        private double _orignShif;
        private double startMovePosition;
        //private ComparisonDefects.Defects _colord;

        public EventHandler<ClickSegmentEventArgs> SegmentClicked;
        

        public PipeControl()
        {
            InitializeComponent();
        }

        public PipeViewModel Model { get { return DataContext as PipeViewModel; } }

        public void Init()
        {
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //разобраться!!!!!!!!!!!
            //Model.PropertyChanged -= Model_PropertyChanged;
        }
        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ("NeedShift".Equals(e.PropertyName))
            {

                Model.Scale = (HightDrowCanvas) / (Model.Pipe.RadiusTube * 2);

                Canvas.SetLeft(drawCanvas, Model.NeedShift * Model.Scale);

                Draw(Model.NeedShift * Model.Scale - 2);
            }
            // && ("Corros".Equals(e.PropertyName))
            //if ("Colord".Equals(e.PropertyName))
            if (("Corros".Equals(e.PropertyName)))
            {
                Draw(0);
            }

        }

        public double HightDrowCanvas
        {
            get
            {
                return (double.IsNaN(drawCanvas.Height) || drawCanvas.Height == 0.0)
                           ? drawCanvas.ActualHeight
                           : drawCanvas.Height;
            }
        }



        public void Draw(double shift)
        {
            //Очистить форму
            drawCanvas.Children.Clear();


            _firstKeySegment = string.Empty;
          //  Model.Scale = (HightDrowCanvas - fifty);// / (Model.Pipe.RadiusTube * 2);
            Model.Scale = (HightDrowCanvas) / (Model.Pipe.RadiusTube * 2);

            double hollowW = canvas.ActualWidth;

            //Отбирает трубы видимые на экране
            List<PipeSegmentViewModel> segTmpModelList = Model.SegModelList.Where(sm => sm.Segment.EndX*Model.Scale + shift >= 0.0)
                                                                           .Where(sm => sm.Segment.StartX*Model.Scale + shift <= hollowW).ToList();

            foreach (var segmentModel in segTmpModelList)
            {
                if (String.IsNullOrEmpty(_firstKeySegment))
                {
                    _firstKeySegment = segmentModel.Segment.KeySegment;
                    lblFirstPipeKm.Content = segmentModel.Segment.Km + " " + Resources_ImpVtd.cM;
                }

                PipeSegmentControl pipeSegmentControl = new PipeSegmentControl();
                pipeSegmentControl.DataContext = segmentModel;
                pipeSegmentControl.Width = segmentModel.Segment.Length * Model.Scale;

                pipeSegmentControl.Height = HightDrowCanvas;
                pipeSegmentControl.Background = new SolidColorBrush(Colors.Gray);
                //pipeSegmentControl.kmTxt.Text = "  " + segmentModel.Segment.Km;
                // pipeSegmentControl.lenghtpipe.Text = (segmentModel.Segment.Length).ToString();
                Canvas.SetLeft(pipeSegmentControl, segmentModel.Segment.StartX * Model.Scale);
                drawCanvas.Children.Add(pipeSegmentControl);

                pipeSegmentControl.Draw(Model.Scale, Model.Pipe.RadiusTube, Model.Colord, Model.Corros, Model.TypeShov);
            }
        }

        public void VisibleCheskDefect(bool visibleCheck)
        {
            if (visibleCheck)
            {
                //ChkShowDefects.Visibility = Visibility.Visible;
                //reportTxt.Text = "";
            }
            else
            {
                //ChkShowDefects.Visibility = Visibility.Collapsed;
                
                //reportTxt.Text = "Внимание! Идет загрузка дефектов...";
            }
            
        }


        private void canvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!isDragging)
            {
                _wasMove = false;
                isDragging = true;
                startMovePosition = e.GetPosition(canvas).X;
                _orignShif = Canvas.GetLeft(drawCanvas);
                canvas.CaptureMouse();
                Cursor = Cursors.Hand;
            }
        }

        public void Redraw()
        {

        }


        private bool _wasMove;
        private string _firstKeySegment;

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                canvas.ReleaseMouseCapture();
                Cursor = Cursors.Arrow;

                if (_wasMove)
                {
                    double shift = Canvas.GetLeft(drawCanvas);
                    Draw(shift);
                }
                else
                {
                    if (SegmentClicked != null)
                    {
                        double x = e.GetPosition(drawCanvas).X;
                        foreach (PipeSegmentViewModel model in Model.SegModelList)
                        {
                            if (model.Segment.StartX * Model.Scale <= x && x < model.Segment.EndX * Model.Scale)
                            {
                                model.IsClicked = !model.IsClicked;
                                SegmentClicked(this, new ClickSegmentEventArgs(model));
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                _wasMove = true;
                double shift = e.GetPosition(canvas).X - startMovePosition;
                Canvas.SetLeft(drawCanvas, shift + _orignShif);
            }
        }

       

        //        private double _oldScale = 0.0;


        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvas.Width = ActualWidth;
            drawCanvas.Height = canvas.Height = ActualHeight-20 ;//30;
            if (Model != null)
            {

                Model.Scale = (HightDrowCanvas) / (Model.Pipe.RadiusTube * 2);

                if (!String.IsNullOrEmpty(_firstKeySegment))
                {
                    Debug.WriteLine(_firstKeySegment);
                    Model.ScrollToSegment(_firstKeySegment);
                }
            }
        }

        private void ChkShowProdShov_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var segModel in Model.SegModelList)
            {
               // segModel.IsVisibleLineShov = (bool)ChkShowProdShov.IsChecked;
            }
        }

      
    }
}
