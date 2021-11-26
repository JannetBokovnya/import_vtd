using System;
using System.ComponentModel;

namespace DrawPipe2D.ViewModel
{
    // [DataContract]
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isShutdowning;

        #region implement INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propName));
            }
        }

        #endregion implement INotifyPropertyChanged

        public bool IsShutdowning
        {
            get { return _isShutdowning; }
            set
            {
                _isShutdowning = value;
                NotifyPropertyChanged("IsShutdowning");
            }
        }

        public virtual void Shutdown()
        {
            IsShutdowning = true;
        }
    }
}
