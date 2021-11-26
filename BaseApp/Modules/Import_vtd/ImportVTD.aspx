<%@ Page Title="" Language="C#" MasterPageFile="~/System/TopMasterPage.master" AutoEventWireup="true" CodeFile="ImportVTD.aspx.cs" Inherits="ImportVTD" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <title>Импорт ВТД</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">

    <div style="height:100%; width:100%; font-size:0;"> 
        <object  data="data:application/x-silverlight-2," type="application/x-silverlight-2" height="100%" width="100%">
            <param name="source" value="ClientBin/importVtd.xap"/>
            <param name="initParams"  value="<%=silverlightParams%>" />
            <param name="windowless" value="true" />
        </object>
	</div>  
</asp:Content>
