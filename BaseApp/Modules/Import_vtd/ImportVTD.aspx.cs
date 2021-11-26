using System;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Web;
using Resources;

public partial class ImportVTD : System.Web.UI.Page
{
    public string silverlightParams;

    protected void Page_Load(object sender, EventArgs e)
    {
        silverlightParams = "lang=" + CultureInfo.CurrentCulture.Name;
        ResourceSet resourceSet = IttSilverlightStyle.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
        foreach (DictionaryEntry entry in resourceSet)
        {
            silverlightParams += "," + entry.Key + "=" + entry.Value;
        }
		
        SetModuleName("IMPORTVTD");
    }


    private void SetModuleName(string p_cModuleName)
    {
        System_TopMasterPage master = (System_TopMasterPage)this.Master;
        master.SetModuleName(p_cModuleName);
    }

    //Перегружаем "Культуру" для данной страницы (этот метод вызвается самым первым, раньше всех других)
    protected override void InitializeCulture()
    {
        //Выставляем "Культуру" для данной страницы, в зависимости выбранного ранее пользователем (берем из сессионной переменной Session["lang"])
        if (HttpContext.Current.Session["lang"] != null)
        {
            String selectedLanguage = HttpContext.Current.Session["lang"].ToString();
            UICulture = selectedLanguage;
            Culture = selectedLanguage;

            

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(selectedLanguage);

        }
        base.InitializeCulture();
    }
	
}
