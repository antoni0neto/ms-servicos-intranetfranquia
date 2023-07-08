<%@ Page Title="Gerar Notas de Produtos com Defeito" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="GerarNotasDefeito.aspx.cs" Inherits="Relatorios.GerarNotasDefeito" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

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
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Selecione</legend>
            <div style="width: 200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Filial:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px" 
                        Width="200px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btNotaRetirada" Text="Buscar Notas de Defeito" OnClick="btNotaRetirada_Click"/>
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
        <div>
            <asp:GridView runat="server" ID="GridViewNotaRetirada" AutoGenerateColumns="false" onrowdatabound="GridViewNotaRetirada_RowDataBound">
	            <RowStyle HorizontalAlign="Center"></RowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Filial">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralFilial"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NUMERO_NOTA_CMAX" HeaderText="Nota Cmax" />
                    <asp:BoundField DataField="NUMERO_NOTA_HBF" HeaderText="Nota Hbf" />
                    <asp:BoundField DataField="NUMERO_NOTA_CALCADOS" HeaderText="Nota Hbf Calçados" />
                    <asp:BoundField DataField="NUMERO_NOTA_OUTROS" HeaderText="Nota Hbf Outros" />
                    <asp:BoundField DataField="NUMERO_NOTA_LUGZI" HeaderText="Nota Lugzi" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button runat="server" ID="btGerarArquivo" Text="Gerar Arquivo" OnClick="btGerarArquivo_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button runat="server" ID="btRegistrarNota" Text="Registrar Nota" OnClick="btRegistrarNota_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:HiddenField runat="server" ID="HiddenFieldCodigoNotaRetirada" Value="0" />
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="GridViewCmax" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="ITEM_PRODUTO" HeaderText="Item" />
                            <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                            <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor do Produto" />
                            <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
                            <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                            <asp:BoundField DataField="CODIGO_ORIGEM_DEFEITO" HeaderText="Origem do Defeito" />
                            <asp:BoundField DataField="CODIGO_DEFEITO" HeaderText="Defeito" />
                            <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td>
                    <asp:GridView runat="server" ID="GridViewHbf" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="ITEM_PRODUTO" HeaderText="Item" />
                            <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                            <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor do Produto" />
                            <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
                            <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                            <asp:BoundField DataField="CODIGO_ORIGEM_DEFEITO" HeaderText="Origem do Defeito" />
                            <asp:BoundField DataField="CODIGO_DEFEITO" HeaderText="Defeito" />
                            <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="GridViewCalcados" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="ITEM_PRODUTO" HeaderText="Item" />
                            <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                            <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor do Produto" />
                            <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
                            <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                            <asp:BoundField DataField="CODIGO_ORIGEM_DEFEITO" HeaderText="Origem do Defeito" />
                            <asp:BoundField DataField="CODIGO_DEFEITO" HeaderText="Defeito" />
                            <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td>
                    <asp:GridView runat="server" ID="GridViewOutros" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="ITEM_PRODUTO" HeaderText="Item" />
                            <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                            <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor do Produto" />
                            <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
                            <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                            <asp:BoundField DataField="CODIGO_ORIGEM_DEFEITO" HeaderText="Origem do Defeito" />
                            <asp:BoundField DataField="CODIGO_DEFEITO" HeaderText="Defeito" />
                            <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td>
                    <asp:GridView runat="server" ID="GridViewLugzi" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="ITEM_PRODUTO" HeaderText="Item" />
                            <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                            <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor do Produto" />
                            <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
                            <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                            <asp:BoundField DataField="CODIGO_ORIGEM_DEFEITO" HeaderText="Origem do Defeito" />
                            <asp:BoundField DataField="CODIGO_DEFEITO" HeaderText="Defeito" />
                            <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
