namespace importVtd.Business
 {
    //Используется в гриде
     public class RelatedTEM
     {
        public string KeyDb { get; private set; }
        public string NumPipePartDb { get; private set; }
        public string LocKmBegDb { get; private set; } 
        public string LengthDb { get; private set; } 
        public string DepthPipeDb { get; private set; } 
        public string TypeDb { get; private set; }
        public string AngleDb { get; private set; }

        public string KeyFile { get; private set; } 
        public string NumPipePartFile { get; private set; }
        public string LocKmBegFile { get; private set; }
        public string LengthFile { get; private set; }
        public string DepthPipeFile { get; private set; }
        public string TypeFile { get; private set; }
        public string AngleFile { get; private set; }
 
 
         public RelatedTEM(string keyDb, string locKmBegDb, string lengthDb, string depthPipeDb, string typeDb, string angleDb, string numPipePartDb,
                           string keyFile, string locKmBegFile, string lengthFile, string depthPipeFile, string typeFile, string angleFile, string numPipePartFile)
         {
            KeyDb = keyDb;
            NumPipePartDb = numPipePartDb;
            LocKmBegDb = locKmBegDb;
            LengthDb = lengthDb;
            DepthPipeDb = depthPipeDb;
            TypeDb = typeDb;
            AngleDb = angleDb;

            KeyFile = keyFile;
            NumPipePartFile = numPipePartFile;
            LocKmBegFile = locKmBegFile;
            LengthFile = lengthFile;
            DepthPipeFile = depthPipeFile;
            TypeFile = typeFile;
            AngleFile = angleFile;
         }
     }
 }
