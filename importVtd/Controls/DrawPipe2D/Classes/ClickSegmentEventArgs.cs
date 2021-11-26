using System;
using DrawPipe2D.ViewModel;

namespace DrawPipe2D.Classes
{
    public class ClickSegmentEventArgs : EventArgs
    {
        public PipeSegmentViewModel Model { get; private set; }

        public ClickSegmentEventArgs(PipeSegmentViewModel model)
        {
            Model = model;
        }
    }
}
