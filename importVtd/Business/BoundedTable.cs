using System.Collections.Generic;

namespace importVtd.Business
{
    public class BoundedTable
    {
        private Dictionary<string, string> _data = new Dictionary<string, string>();

        public BoundedTable(string leftKey, string rightKey)
        {
            LeftKey = leftKey;
            RightKey = rightKey;
        }

        public string LeftKey { get; private set; }

        public string RightKey { get; private set; }

        public string LocKmBegDb
        {
            get { return (!_data.ContainsKey("loc_km_begDB") ? string.Empty : _data["loc_km_begDB"]); }
            set
            {
                if (!_data.ContainsKey("loc_km_begDB"))
                    _data.Add("loc_km_begDB", value);
                else
                    _data["loc_km_begDB"] = value;
            }
        }

        public string LengthDb
        {
            get { return (!_data.ContainsKey("LengthDb") ? string.Empty : _data["LengthDb"]); }
            set
            {
                if (!_data.ContainsKey("LengthDb"))
                    _data.Add("LengthDb", value);
                else
                    _data["LengthDb"] = value;
            }
        }

        public string DepthPipeDb
        {
            get { return (!_data.ContainsKey("ndepth_pipeDB") ? string.Empty : _data["ndepth_pipeDB"]); }
            set
            {
                if (!_data.ContainsKey("ndepth_pipeDB"))
                    _data.Add("ndepth_pipeDB", value);
                else
                    _data["ndepth_pipeDB"] = value;
            }
        }

        public string TypeDb
        {
            get { return (!_data.ContainsKey("cTypeDB") ? string.Empty : _data["cTypeDB"]); }
            set
            {
                if (!_data.ContainsKey("cTypeDB"))
                    _data.Add("cTypeDB", value);
                else
                    _data["cTypeDB"] = value;

            }
        }

        public string AngleDb
        {
            get { return (!_data.ContainsKey("nAngleDB") ? string.Empty : _data["nAngleDB"]); }
            set
            {
                if (!_data.ContainsKey("nAngleDB"))
                    _data.Add("nAngleDB", value);
                else
                    _data["nAngleDB"] = value;
            }
        }

        public string LocKmBegFile
        {
            get { return (!_data.ContainsKey("loc_km_begFile") ? string.Empty : _data["loc_km_begFile"]); }
            set
            {
                if (!_data.ContainsKey("loc_km_begFile"))
                    _data.Add("loc_km_begFile", value);
                else
                    _data["loc_km_begFile"] = value;
            }
        }

        public string NlengthFile
        {
            get { return (!_data.ContainsKey("nlengthFile") ? string.Empty : _data["nlengthFile"]); }
            set
            {
                if (!_data.ContainsKey("nlengthFile"))
                    _data.Add("nlengthFile", value);
                else
                    _data["nlengthFile"] = value;
            }
        }

        public string NdepthPipeFile
        {
            get { return (!_data.ContainsKey("ndepth_pipeFile") ? string.Empty : _data["ndepth_pipeFile"]); }
            set
            {
                if (!_data.ContainsKey("ndepth_pipeFile"))
                    _data.Add("ndepth_pipeFile", value);
                else
                    _data["ndepth_pipeFile"] = value;
            }
        }

        public string cTypeFile
        {
            get { return (!_data.ContainsKey("cTypeFile") ? string.Empty : _data["cTypeFile"]); }
            set
            {
                if (!_data.ContainsKey("cTypeFile"))
                    _data.Add("cTypeFile", value);
                else
                    _data["cTypeFile"] = value;
            }
        }

        public string nAngleFile
        {
            get { return (!_data.ContainsKey("nAngleFile") ? string.Empty : _data["nAngleFile"]); }
            set
            {
                if (!_data.ContainsKey("nAngleFile"))
                    _data.Add("nAngleFile", value);
                else
                    _data["nAngleFile"] = value;
            }
        }

    }
}
