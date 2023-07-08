<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadImagemProduto.aspx.cs" Inherits="Relatorios.UploadImagemProduto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Cadastro de Imagem de Produto</legend>
            <div>
                <label>Coleção:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" 
                    DataTextField="DESC_COLECAO" Height="22px" 
                    Width="198px" ondatabound="ddlColecao_DataBound" autopostback = "true"
                    onselectedindexchanged="ddlColecao_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div>
                <label>Grupo:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlGrupo" DataValueField="GRUPO" DataTextField="GRUPO" Height="22px" 
                    Width="198px" autopostback = "true" ondatabound="ddlGrupo_DataBound" onselectedindexchanged="ddlGrupo_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div>
                <label>Produto:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlProduto" DataValueField="PRODUTO1" DataTextField="DESC_PRODUTO" Height="22px" 
                    Width="198px" autopostback = "true" ondatabound="ddlProduto_DataBound" onselectedindexchanged="ddlProduto_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div>
                <label>Cor:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlCor" DataValueField="COR_PRODUTO" DataTextField="DESC_COR_PRODUTO" Height="22px" 
                    Width="198px" ondatabound="ddlCor_DataBound"></asp:DropDownList>
            </div>
        </fieldset>
        <fieldset class="login">
            <div>
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" Width="399px" 
                    onprerender="UploadButton_Click" />
            </div>
            <div>
                <label>Ativo: </label>
                <asp:CheckBox ID="CheckBoxAtivo" runat="server" />
            </div>
            <div>
                <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="imagem" />
                <asp:ValidationSummary ID="ValidationSummaryImagem" runat="server" ValidationGroup="imagem" ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewImagem" AutoGenerateColumns="false" onrowdatabound="GridViewImagem_RowDataBound">
            <Columns>
                <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
                <asp:BoundField DataField="LOCAL_IMAGEM_PRODUTO" HeaderText="Local" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluir_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoimagem" Value="0" />
</asp:Content>
