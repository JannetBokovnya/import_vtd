using System;

public partial class UserControls_Menu_FilterMenu_FilteringMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmMainMenu.Items.AddRange(DbConnMenu.LoadMainMenuItems());
        }
    }
}