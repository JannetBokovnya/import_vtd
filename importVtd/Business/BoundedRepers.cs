using System.ComponentModel;

namespace importVtd.Business
{
    public class BoundedRepers : INotifyPropertyChanged
    {
        private string _fileReperKey;
        private string _dbReperKey;
        private string _num;
        private string _kmByMg;
        private string _kmByDe;
        private string _lenByMg;
        private string _lenByDe;
        private string _differenceInKm;
        private string _countPoint;
        private string _koef;

        public event PropertyChangedEventHandler PropertyChanged;

        public string FileReperKey
        {
            get
            {
                return _fileReperKey;
            }
            set
            {
                if ((object.ReferenceEquals(_fileReperKey, value) != true))
                {
                    _fileReperKey = value;
                    this.OnPropertyChanged("fileReperKey");
                }
            }
        }

       
        public string DbReperKey
        {
            get
            {
                return _dbReperKey;
            }
            set
            {
                if ((object.ReferenceEquals(_dbReperKey, value) != true))
                {
                    _dbReperKey = value;
                    this.OnPropertyChanged("dbReperKey");
                }
            }
        }

       
        public string Num
        {
            get
            {
                return _num;
            }
            set
            {
                if ((object.ReferenceEquals(_num, value) != true))
                {
                    _num = value;
                    this.OnPropertyChanged("num");
                }
            }
        }

       
        public string KmByMg
        {
            get
            {
                return _kmByMg;
            }
            set
            {
                if ((object.ReferenceEquals(_kmByMg, value) != true))
                {
                    _kmByMg = value;
                    this.OnPropertyChanged("kmByMG");
                }
            }
        }


        public string KmByVtd
        {
            get
            {
                return _kmByDe;
            }
            set
            {
                if ((object.ReferenceEquals(_kmByDe, value) != true))
                {
                    _kmByDe = value;
                    this.OnPropertyChanged("kmByVTD");
                }
            }
        }

       
        public string LenByMg
        {
            get
            {
                return _lenByMg;
            }
            set
            {
                if ((object.ReferenceEquals(_lenByMg, value) != true))
                {
                    _lenByMg = value;
                    this.OnPropertyChanged("lenByMG");
                   
                }
            }
        }


        public string LenByVtd
        {
            get
            {
                return _lenByDe;
            }
            set
            {
                if ((object.ReferenceEquals(_lenByDe, value) != true))
                {
                    _lenByDe = value;
                    this.OnPropertyChanged("lenByVTD");
                }
            }
        }

       
        public string DifferenceInKm
        {
            get
            {
                return _differenceInKm;
            }
            set
            {
                if ((object.ReferenceEquals(_differenceInKm, value) != true))
                {
                    _differenceInKm = value;
                    this.OnPropertyChanged("differenceInKm");
                }
            }
        }

        
        public string CountPoint
        {
            get
            {
                return _countPoint;
            }
            set
            {
                if ((object.ReferenceEquals(_countPoint, value) != true))
                {
                    _countPoint = value;
                    this.OnPropertyChanged("countPoint");
                }
            }
        }

        
        public string Koef
        {
            get
            {
                return _koef;
            }
            set
            {
                if ((object.ReferenceEquals(_koef, value) != true))
                {
                    _koef = value;
                    this.OnPropertyChanged("koef");
                }
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

    }
}
