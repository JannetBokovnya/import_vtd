

using importVtd.Resources;

namespace importVtd.Business
{
    public class ResProvider
    {
       
        static Resources_ImpVtd resources = new Resources_ImpVtd();

        public Resources_ImpVtd Resources_ImpVtd
        {
            get
            {
                return resources;
            }
        }

    }
}
