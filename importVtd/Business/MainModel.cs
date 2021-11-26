using System.ComponentModel;

namespace importVtd.Business
{
    public class MainModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isShowBusy;

        public void FirePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        public bool IsShowBusy
        {
            get { return _isShowBusy; }
            set
            {
                _isShowBusy = value;
                FirePropertyChanged("IsShowBusy");
            }
        }
    }
}
