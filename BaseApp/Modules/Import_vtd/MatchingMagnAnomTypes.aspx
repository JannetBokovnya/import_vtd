<%@ Page Title="" Language="C#" MasterPageFile="~/System/TopMasterPage.master" AutoEventWireup="true" CodeFile="MatchingMagnAnomTypes.aspx.cs" Inherits="Modules_Import_vtd_MatchingMagnAnomTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Сопоставление типов магнитных аномалий</title>
    <!--CssClass="i2" - для кнопок-->
    <style type="text/css">
        table {
            font-family: Arial;
            font: 12px Tahoma, Arial, Verdana, sans-serif;
        }

        .legendtext {
            color: #38848e;
            /*color: #5c5c5c;*/
            font-size: 11px;
            font-weight: bold;
            margin-left: 10px;
        }

        .divvisible {
            visibility: visible; /*position: relative;*/
            position: absolute;
            height: 1000px;
        }

        .divhidden {
            visibility: hidden; /*position: absolute;*/
            position: absolute;
        }

        /* table style */
        table.yui-datatable-theme {
            font-family: arial;
            font-size: 12px;
            border: solid 1px #7f7f7f;
        }

        /* the sort header link */
        .yui-datatable-theme th a {
            font-weight: normal;
            text-decoration: none;
            text-align: center;
            color: #000;
            display: block;
        }

        /* header cell styles */
        .yui-datatable-theme th {
            background: url(img/yui-datatable/sprite.png) repeat-x 0px 0px;
            border-color: #989898 #cbcbcb #989898 #989898;
            border-style: solid solid solid none;
            border-width: 1px 1px 1px medium;
            color: #000;
            padding: 4px 5px 4px 10px;
            text-align: center;
            vertical-align: bottom;
        }

        /* data data cell style */
        .yui-datatable-theme td {
            padding: 4px 10px 4px 10px;
            border-right: solid 1px #cbcbcb;
        }

        /* alternating row style */
        .yui-datatable-theme .alt-data-row {
            background-color: #edf5ff;
        }

        /* mouseover row style */
        .yui-datatable-theme .row-over {
            background-color: #b2d2ff;
        }

        /* select row style */
        .yui-datatable-theme .row-select {
            background-color: #426fd9;
            color: #fff;
        }
    </style>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="Body" runat="Server">
    <h1>
        <img src="i/a2.gif" width="7" height="7" align="middle" />
        Сопоставление типов магнитных аномалий</h1>
    <div style="padding-left: 20px;">
        <table width="100%" style="height: 95%;" border="0">
            <tr>
                <td>
                    <fieldset style="width: 90%; height: 50px">
                        <legend class="legendtext">Анализ файла</legend>
                        <br />
                        <asp:FileUpload ID="FileField" runat="server" Width="300" />
                        &nbsp;
                            <asp:Button ID="btnLoadFile" runat="server" Text="Загрузить" OnClick="btnLoadFile_Click"
                                Font-Size="12px" Width="70" />
                        &nbsp;
                        &nbsp;
                        <asp:Label ID="lblWarning" runat="server" EnableViewState="False" Font-Size="12px"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <fieldset style="width: 90%;">
                        <legend class="legendtext">Типы магнитных аномалий</legend>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Сопоставлено: "></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblMatched" runat="server" Text="0"></asp:Label>
                                    &nbsp;
                                    &nbsp;
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Сопоставить: "></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblToMatch" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:GridView ID="grdMagAnomTypes" runat="server" AutoGenerateColumns="False" CellPadding="4" EmptyDataText="Данных нет..."
                            CssClass="yui-datatable-theme" Width="97%" EnableModelValidation="True" OnRowDataBound="grdMagAnomTypes_RowDataBound">
                            <RowStyle CssClass="data-row" />
                            <AlternatingRowStyle CssClass="alt-data-row" />
                            <Columns>
                                <asp:BoundField DataField="magnAnomType" HeaderText="Из файла ВТД" >
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Из БД ИАС">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlMagnAnomTypesFromDB" Width="490" runat="server"></asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle Width="500px" />
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="Сохранить" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:content>

