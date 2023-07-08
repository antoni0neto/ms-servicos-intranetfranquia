<%@ Page Title="Alterar Quantidade de Sobra" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gest_alt_qtde_sobra.aspx.cs" Inherits="Relatorios.gest_alt_qtde_sobra" %>

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
            background-color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios
                    de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Alterar Quantidade de Sobra</span>
                <div style="float: right; padding: 0;">
                    <a href="gest_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Alterar Quantidade de Sobra</legend>
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
                        <div style="width: 200px;" class="alinhamento">
                            <label>
                                Griffe:&nbsp;
                            </label>
                            <asp:DropDownList runat="server" ID="ddlGriffe" DataValueField="COD_GRIFFE" DataTextField="GRIFFE"
                                Height="22px" Width="198px" OnDataBound="ddlGriffe_DataBound">
                            </asp:DropDownList>
                        </div>
                    </div>
                </fieldset>
                <div>
                    <asp:Button runat="server" ID="btBuscarSobra" Text="Buscar Sobras" OnClick="btBuscarSobra_Click" />
                    <asp:ValidationSummary ID="ValidationSummarySobra" runat="server" ShowMessageBox="true"
                        ShowSummary="false" />
                </div>
                <fieldset class="login">
                    <div style="float: right; margin-right: 10px; width: 120px; border: 0px solid black;">
                        <asp:CheckBox runat="server" ID="cbMarcarTodos" Text="" TextAlign="Right" AutoPostBack="true"
                            OnCheckedChanged="cbMarcarTodos_CheckedChanged" /><asp:Label ID="labMarcarTodos"
                                runat="server" Text="Marcar Todos"></asp:Label>
                    </div>
                    <table border="1" class="style1">
                        <tr>
                            <asp:GridView ID="GridViewSobras" runat="server" Width="100%" AutoGenerateColumns="False"
                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="GridViewSobras_RowDataBound"
                                DataKeyNames="CODIGO">
                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                <RowStyle HorizontalAlign="Center"></RowStyle>
                                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                                <Columns>
                                    <asp:BoundField DataField="CODIGO" HeaderText="Produto" Visible="false" />
                                    <asp:BoundField DataField="ANO_SEMANA" HeaderText="Semana do Cadastro" />
                                    <asp:BoundField DataField="CATEGORIA" HeaderText="Categoria" />
                                    <asp:BoundField DataField="COLECAO" HeaderText="Coleção" />
                                    <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" />
                                    <asp:BoundField DataField="GRUPO" HeaderText="Grupo" />
                                    <asp:BoundField DataField="QTDE" HeaderText="Qtde Virado" />
                                    <asp:BoundField DataField="VALOR" HeaderText="Valor Virado" />
                                    <asp:TemplateField HeaderText="Qtde">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtQtde" AutoPostBack="false" Width="50" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtValor" AutoPostBack="false" Width="50" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Alterado">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="cbAlterado" AutoPostBack="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </tr>
                    </table>
                </fieldset>
                <div>
                    <asp:Button runat="server" ID="btGravar" Text="Gravar Sobras" OnClick="btGravar_Click"
                        Enabled="False" />
                    <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
