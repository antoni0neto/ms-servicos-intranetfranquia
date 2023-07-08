<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="NFeCompararNotas.aspx.cs" Inherits="Relatorios.NFeCompararNotas" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content> 
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <br />
        <br />
        <fieldset class="login">
            <div style="width: 100%; border: 0px solid black;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data Início:&nbsp;
                    </label>
                    <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px"
                        Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data Fim:&nbsp;
                    </label>
                    <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px"
                        Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Filial:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                        Height="26px" Width="200px" OnDataBound="ddlFilial_DataBound">
                    </asp:DropDownList>
                    <br />
                    <br />
                    <br />
                </div>
                <div style="margin-top: 110px; float: right; border: 0px solid black;">
                    <br />
                    <br />
                    <br />
                    <asp:Label ID="labMoccasin" runat="server" BorderColor="Black" Width="200px" BackColor="Moccasin"
                        Text="">&nbsp;&nbsp;&nbsp;</asp:Label>
                    <asp:Label ID="labMoccasinTexto" runat="server" Text="Não tem esta nota na Retaguarda ou no Portal"></asp:Label><br />
                    <br />
                    <asp:Label ID="labLightGray" runat="server" BorderColor="Gray" Width="200px" BackColor="LightGray"
                        Text="">&nbsp;&nbsp;&nbsp;</asp:Label>
                    <asp:Label ID="labLightGrayTexto" runat="server" Text="Existe uma nota de saída em outra Filial"></asp:Label>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarNotas" Text="Buscar Notas" OnClick="btBuscarNotas_Click" />
            <asp:Label ID="labMSG" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
        <div id="nfRet" style="width: 700px; float: left; clear: both;">
            <fieldset class="login" runat="server">
                <legend>Notas Fiscais Retaguarda</legend>
                <div style="float: right; margin-top: -15px;">
                    <asp:Button runat="server" ID="btExcelRetaguarda" Text="Excel" OnClick="btExcelRetaguarda_Click"
                        ToolTip="Extrair as informações para Excel." />
                </div>
                <br />
                <asp:GridView ID="GridViewRetaguarda" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                    PageSize="1000" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                    OnPageIndexChanging="GridViewRetaguarda_PageIndexChanging" OnRowDataBound="GridViewRetaguarda_RowDataBound"
                    OnDataBound="GridViewRetaguarda_DataBound">
                    <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                    <RowStyle HorizontalAlign="Center"></RowStyle>
                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="COLUNA" HeaderText="" />
                        <asp:BoundField DataField="COD_FILIAL" HeaderText="Código Filial" />
                        <asp:BoundField DataField="FILIAL" HeaderText="Filial" />
                        <asp:BoundField DataField="NOTA_FISCAL" HeaderText="NF" />
                        <asp:BoundField DataField="EMISSAO" HeaderText="Emissão" />
                        <asp:BoundField DataField="SERIE" HeaderText="Série" />
                        <asp:BoundField DataField="ESPECIE" HeaderText="Espécie" />
                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="E/S" />
                        <asp:BoundField DataField="CFO" HeaderText="CFO" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="COR" HeaderText="Cor" />
                    </Columns>
                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                </asp:GridView>
            </fieldset>
        </div>
        <div id="nfPortal" style="width: 700px; float: right;">
            <fieldset class="login">
                <legend>Notas Fiscais Portal</legend>
                <div style="float: right; margin-top: -15px;">
                    <asp:Button runat="server" ID="btExcelPortal" Text="Excel" OnClick="btExcelPortal_Click"
                        ToolTip="Extrair as informações para Excel." />
                </div>
                <br />
                <asp:GridView ID="GridViewPortal" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                    OnPageIndexChanging="GridViewPortal_PageIndexChanging" PageSize="1000" AllowPaging="True"
                    AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="GridViewPortal_RowDataBound"
                    OnDataBound="GridViewPortal_DataBound">
                    <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                    <RowStyle HorizontalAlign="Center"></RowStyle>
                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="COLUNA" HeaderText="" />
                        <asp:BoundField DataField="COD_FILIAL" HeaderText="Código Filial" />
                        <asp:BoundField DataField="FILIAL" HeaderText="Filial" />
                        <asp:BoundField DataField="NOTA_FISCAL" HeaderText="NF" />
                        <asp:BoundField DataField="EMISSAO" HeaderText="Emissão" />
                        <asp:BoundField DataField="SERIE" HeaderText="Série" />
                        <asp:BoundField DataField="ESPECIE" HeaderText="Espécie" />
                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="E/S" />
                        <asp:BoundField DataField="CFO" HeaderText="CFO" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="COR" HeaderText="Cor" />
                    </Columns>
                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                </asp:GridView>
            </fieldset>
        </div>
    </div>
</asp:Content>
