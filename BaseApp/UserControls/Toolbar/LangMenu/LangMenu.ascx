<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LangMenu.ascx.cs" Inherits="UserControls_Menu_LangMenu_LangMenu" %>

<style>
    /*Для хрома. держил контролы в одном ряду.*/
    .langMenu {
        float: right;
    }

    .langMenu > .RadMenu > .rmRootGroup, 
    .langMenu > .RadMenu > .rmHorizontal > .rmItem > a.rmLink,
    .langMenu > .RadMenu > .rmHorizontal > .rmItem > a.rmLink > .rmText {
        background-color: #0096db;
        padding: 0 1px 0 1px !important;
    }
</style>
<script>
    function MyFunction(sender, args) {
        OnClientItemClicking(sender, args);

        var itemChildCount = args.get_item().get_items().get_count();
        if (itemChildCount == 0) {
            var itemImageUrl = args.get_item().get_imageUrl();
            if (itemImageUrl == null) {
                closeAllWindows();
            }
        }
    }

    
</script>
<div class="langMenu" style="vertical-align: middle; margin-right: 3px ;">
    <telerik:RadMenu ID="rmLang" runat="server"  ClickToOpen="True" EnableAutoScroll="true" 
                     OnItemClick="rmLang_OnItemClick" OnClientItemClicked="MyFunction" 
                     OnClientLoad="AddMenuToArray" OnClientMouseOver="OnClientMouseOverHandler" >
        <Items>
            <telerik:RadMenuItem runat="server" Text="RU"  StaysOpenOnClick="False" CssClass="langMenu" 
                                 Font-Bold="True" ForeColor="White" PostBack="False" meta:resourcekey="lblLangResource1" >
                <Items>
                    <telerik:RadMenuItem runat="server" Text="<b>RU</b> Русский" Value="ru-RU"/>
                    <telerik:RadMenuItem runat="server" Text="<b>EN</b> English" Value="en-US"/>
                </Items>
            </telerik:RadMenuItem>
        </Items>
    </telerik:RadMenu>
</div>

