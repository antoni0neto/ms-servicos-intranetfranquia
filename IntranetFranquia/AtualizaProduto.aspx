<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AtualizaProduto.aspx.cs" Inherits="Relatorios.AtualizaProduto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <div style="width: 200px;"  class="alinhamento">
                <label>Coleção:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px" 
                    Width="198px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btGravaMovimento" Text="Grava Movimento Produto" OnClick="btGravaMovimento_Click" ValidationGroup="entrada"/>
            <asp:ValidationSummary ID="ValidationSummaryEntrada" runat="server" ValidationGroup="entrada" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <div>
            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label runat="server" ID="lblMensagem" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
