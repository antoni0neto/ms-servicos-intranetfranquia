<%@ Page Title="Atualizar Movimento Estoque" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gest_estoque_atualiza.aspx.cs" Inherits="Relatorios.gest_estoque_atualiza" %>

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
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios
            de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Atualizar Movimento Estoque</span>
        <div style="float: right; padding: 0;">
            <a href="gest_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Atualizar Movimento Estoque</legend>
            <div style="width: 800px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Categoria:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlCategoria" DataValueField="COD_CATEGORIA"
                        DataTextField="CATEGORIA_PRODUTO" Height="22px" Width="198px" OnDataBound="ddlCategoria_DataBound">
                    </asp:DropDownList>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Coleção:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO"
                        Height="22px" Width="198px" OnDataBound="ddlColecao_DataBound">
                    </asp:DropDownList>
                </div>
                <div style="width: 400px;" class="alinhamento">
                    <label>
                        Ano/Semana 454:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlSemana454" DataValueField="ANO_SEMANA_454"
                        DataTextField="TEXTO" Height="22px" Width="396px" OnDataBound="ddlSemana454_DataBound">
                    </asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btGravaMovimento" Text="Gravar Movimento Produto"
                OnClick="btGravaMovimento_Click" ValidationGroup="entrada" />
            <asp:ValidationSummary ID="ValidationSummaryEntrada" runat="server" ValidationGroup="entrada"
                ShowMessageBox="true" ShowSummary="false" />
        </div>
        <div>
            <asp:Label runat="server" ID="lblMensagem" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
