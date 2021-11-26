using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Resources;
using importVtd.Business;
using importVtd.Style;

namespace importVtd
{
    public partial class App : Application
    {
        public static string _UserKey;

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
        }

        private void FillStyleParams(Dictionary<string, string> dictParams)
        {
            string param;

            dictParams.TryGetValue("BorderColorNormal", out param);
            IttStyle.BorderColorNormal = param ?? "#c0c0c0";

            dictParams.TryGetValue("BorderColorHover", out param);
            IttStyle.BorderColorHover = param ?? "#ff0000";

            dictParams.TryGetValue("BorderColorFocus", out param);
            IttStyle.BorderColorFocus = param ?? "#ff0000";

            dictParams.TryGetValue("BorderColorActive", out param);
            IttStyle.BorderColorActive = param ?? "#ff0000";

            dictParams.TryGetValue("BorderColorDisabled", out param);
            IttStyle.BorderColorDisabled = param ?? "#a0a0a0";

            dictParams.TryGetValue("BackgroundColorNormal", out param);
            IttStyle.BackgroundColorNormal = param ?? "#c00000";

            dictParams.TryGetValue("BackgroundColorHover", out param);
            IttStyle.BackgroundColorHover = param ?? "#c00000";

            dictParams.TryGetValue("BackgroundColorFocus", out param);
            IttStyle.BackgroundColorFocus = param ?? "#c00000";

            dictParams.TryGetValue("BackgroundColorActive", out param);
            IttStyle.BackgroundColorActive = param ?? "#c00000";

            dictParams.TryGetValue("BackgroundColorDisabled", out param);
            IttStyle.BackgroundColorDisabled = param ?? "#a0a0a0";

            dictParams.TryGetValue("ComboBoxItemHover", out param);
            IttStyle.ComboBoxItemHover = param ?? "#c0c0c0";

            dictParams.TryGetValue("GridHeaderGradient1", out param);
            IttStyle.GridHeaderGradient1 = param ?? "#c0c0c0";

            dictParams.TryGetValue("GridHeaderGradient2", out param);
            IttStyle.GridHeaderGradient2 = param ?? "#c0c0c0";

            dictParams.TryGetValue("LabelInGroupBackground", out param);
            IttStyle.LabelInGroupBackground = param ?? "#00c000";

            dictParams.TryGetValue("LabelInGroupForeground", out param);
            IttStyle.LabelInGroupForeground = param ?? "#000000";
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            try
            {
                string cinfo = "ru-Ru";
                Dictionary<string, string> getparams = e.InitParams as Dictionary<string, string>;

                FillStyleParams(getparams);

                foreach (KeyValuePair<string, string> pair in getparams)
                {
                    if (pair.Key == "lang")
                    {
                        cinfo = pair.Value;
                        if (cinfo == "en-US")
                        {
                            cinfo = "en";
                        }
                        else
                        {
                            if (cinfo == "ru-RU")
                            {
                                cinfo = "ru-Ru";
                            }
                        }

                    }
                }




                InitializeComponent();

                CultureInfo ci = new CultureInfo(cinfo);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
            catch (Exception)
            {

                throw;
            }
           

            this.RootVisual = new MainPage();

            if (e.InitParams != null)
            {
                foreach (var item in e.InitParams)
                {
                    if (item.Key == "userKey")
                    {
                        _UserKey = item.Value.Replace("'", "");
                    }
                    //string key = item.Key;
                    //string value = item.Value;
                    //this.Resources.Add(item.Key, item.Value);
                }
            }
            //создаем модель для этого View
            MainViewModel model = new MainViewModel();
            model.Report("Модель создана");


            string v = "";
            StreamResourceInfo info = Application.GetResourceStream(new Uri("version.txt", UriKind.Relative));
            if (info == null)
            {
                model.Version = "файл версии не прочитался";
            }
            else
            {
                StreamReader reader = new StreamReader(info.Stream);

                string line = reader.ReadLine();
                while (line != null)
                {
                    v += line;
                    line = reader.ReadLine();
                }

                reader.Close();
            }

            model.Version = v;
            model.Report("Версия сборки: " + v);


            //отдаем модель View, назначив свойство DataContext 
            //ложим модель в датаконтекст вью
            var mainPage = new MainPage();
            mainPage.DataContext = model;
            model.Report("App - передем Model в MainView");
            this.RootVisual = mainPage;

            
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
