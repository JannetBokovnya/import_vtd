using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace importVtd.Business
{
    public class NewImportInfo
    {

        public string KeyMg { get; set; }
        public string KeyPipe { get; set; }
        public string KeySection { get; set; }
        public string KeyContract { get; set; }
        public string NameFile { get; set; }
        public NewImportInfo(string keyMg, string keyPipe, string keySection, string keyContract, string nameFile)
        {
            KeyMg = keyMg;
            KeyPipe = keyPipe;
            KeySection = keySection;
            KeyContract = keyContract;
            NameFile = nameFile;
        }
    }
}
