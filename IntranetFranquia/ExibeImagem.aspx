<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExibeImagem.aspx.cs" Inherits="Relatorios.ExibeImagem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Visualizar Imagens</span>
        <div style="float: right; padding: 0;">
            <a href="DefaultFinanceiro.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <fieldset class="login">
        <legend>Visualizar Imagens</legend>
        <div style="width: 200px;" class="alinhamento">
            <div style="width: 200px;"  class="alinhamento">
                <label>Filial:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px" Width="200px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
            </div>
        </div>
    </fieldset>
    <div>
        <asp:Button runat="server" ID="btBuscarImagens" Text="Buscar Imagens" OnClick="btBuscarImagens_Click"/>
    </div>
    <table border="1" class="style1">
        <tr>
            <td>
                <asp:GridView runat="server" ID="GridViewArquivo" onrowdatabound="GridViewArquivo_RowDataBound"> 
                    <Columns>
                        <asp:BoundField DataField="NOME_IMAGEM" HeaderText="Imagem Fechamento" />
                        <asp:TemplateField HeaderText="Arquivo">
                            <ItemTemplate>    
                                <asp:HyperLink ID="Open" Text='<%# Eval("LOCAL_IMAGEM") %>' runat="server" Target="_blank" />        
                            </ItemTemplate>                  
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
