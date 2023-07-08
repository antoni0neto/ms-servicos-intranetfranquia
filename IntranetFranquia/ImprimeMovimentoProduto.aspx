<%@ Page Title="Imprime Lista Movimento de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ImprimeMovimentoProduto.aspx.cs" Inherits="Relatorios.ImprimeMovimentoProduto" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
        .style2
        {
            width: 125px;
        }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        <ServerReport ReportPath="Report1.rdlc"/>
<%--        <ServerReport ReportPath="Report1.rdlc" ReportServerUrl="http://187.8.234.228:8012/ReportServerTeste" />
--%>
<%--        <LocalReport ReportPath="Report13.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DsMovimentoProduto" />
            </DataSources>
        </LocalReport>
--%>    
        <LocalReport ReportPath="Report1.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DsMovimentoProduto" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>

    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        SelectMethod="GetData" 
        TypeName="Relatorios.DataSetMovimentoProdutoTableAdapters.Sp_Movimento_ProdutosTableAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="HiddenFieldData1" Name="DataFim_1" 
                PropertyName="Value" />
            <asp:ControlParameter ControlID="HiddenFieldData2" Name="DataFim_2" 
                PropertyName="Value" />
            <asp:ControlParameter ControlID="HiddenFieldData3" Name="DataFim_3" 
                PropertyName="Value" />
            <asp:ControlParameter ControlID="HiddenFieldData4" Name="DataFim_4" 
                PropertyName="Value" />
            <asp:ControlParameter ControlID="HiddenFieldData5" Name="DataFim_5" 
                PropertyName="Value" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:HiddenField runat="server" ID="HiddenFieldData1" Value="0" />
    <asp:HiddenField runat="server" ID="HiddenFieldData2" Value="0" />
    <asp:HiddenField runat="server" ID="HiddenFieldData3" Value="0" />
    <asp:HiddenField runat="server" ID="HiddenFieldData4" Value="0" />
    <asp:HiddenField runat="server" ID="HiddenFieldData5" Value="0" />
</asp:Content>
