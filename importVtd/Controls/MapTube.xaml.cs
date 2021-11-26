using System.Collections.Generic;
using System.Windows;
using DrawPipe2D.ViewModel;
using importVtd.Business;

namespace importVtd.Controls
{
    public partial class MapTube
    {
        public delegate void unboundAll(int i);

        public delegate void NewUnbound(BoundedTable restoreTable);

        private List<BoundedTable> _gridData;
        private unboundAll _unboundAll;
        private NewUnbound _unbound;
        private PipeViewModel _model;
        private PipeViewModel _modelRight;

        public MapTube()
        {
            InitializeComponent();
        }

        public void InitControlMapTube(ref List<BoundedTable> gridData, NewUnbound restoreMethod, unboundAll method, ref PipeViewModel model, ref PipeViewModel modelRight)
        {
            _model = model;
            _modelRight = modelRight;
            if (gridData.Count > 0)
            {
                GrdBoundedObjs.ItemsSource = null;
                GrdBoundedObjs.ItemsSource = gridData;
                _gridData = gridData;
            }
            _unboundAll = method;
            _unbound = restoreMethod;
        }

        private void btnUndoBound_Click(object sender, RoutedEventArgs e)
        {
            if (GrdBoundedObjs.SelectedItem != null)
            {
                BoundedTable currentRowSelected = (BoundedTable)GrdBoundedObjs.SelectedItem;

                if (_unbound != null)
                {
                    _unbound(currentRowSelected);
                    GrdBoundedObjs.ItemsSource = null;
                    if (_gridData.Count >= 0)
                    {
                        GrdBoundedObjs.ItemsSource = _gridData;
                    }
                }
            }
        }

        private void btnUndoBoundAll_Click(object sender, RoutedEventArgs e)
        {
            if (GrdBoundedObjs.SelectedItem != null)
            {
                BoundedTable currentRowSelected = (BoundedTable)GrdBoundedObjs.SelectedItem;

                for (int i = 0; i < _gridData.Count; i++)
                {
                    if (_gridData[i].LocKmBegDb == currentRowSelected.LocKmBegDb &
                        _gridData[i].LocKmBegFile == currentRowSelected.LocKmBegFile &
                        _gridData[i].AngleDb == currentRowSelected.AngleDb &
                        _gridData[i].nAngleFile == currentRowSelected.nAngleFile &
                        _gridData[i].NlengthFile == currentRowSelected.NlengthFile)
                    {
                        _unboundAll(i);
                        GrdBoundedObjs.ItemsSource = null;
                        if (_gridData.Count >= 0)
                            GrdBoundedObjs.ItemsSource = _gridData;
                        break;
                    }
                }
            }
        }

        private void grdBoundedObjs_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (GrdBoundedObjs.SelectedItem != null)
            {
                string currentRowSelectedLeft = ((BoundedTable)GrdBoundedObjs.SelectedItem).LeftKey;
                string currentRowSelectedRight = ((BoundedTable)GrdBoundedObjs.SelectedItem).RightKey;

                if (_model != null)
                {
                    _model.ScrollToSegment(currentRowSelectedLeft);
                    foreach (var segModel in _model.SegModelList)
                    {
                        if (segModel.Segment.KeySegment == currentRowSelectedLeft)
                        {
                            segModel.IsClicked = true;
                            segModel.IsLink = true;
                        }
                    }
                }
                if (_modelRight != null)
                {
                    _modelRight.ScrollToSegment(currentRowSelectedRight);
                    foreach (var segModel in _modelRight.SegModelList)
                    {
                        if (segModel.Segment.KeySegment == currentRowSelectedRight)
                        {
                            segModel.IsClicked = true;
                            segModel.IsLink = true;
                        }
                    }
                }
            }
        }
    }
}
