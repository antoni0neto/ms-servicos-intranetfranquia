<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AtualizaDefeitoAtacado.aspx.cs" Inherits="Relatorios.AtualizaDefeitoAtacado" %>

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
            background-color:White;
        }
    </style>
    <script type="text/javascript" src="jquery-1.6.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input').keypress(function (e) {
                var code = null;
                code = (e.keyCode ? e.keyCode : e.which);
                return (code == 13) ? false : true;
            });
        });
        function SelectAllCheckboxes1(cbAlterado) {
            $('#<%=GridViewNotaRetiradaItem.ClientID%>').find("input:checkbox").each(function () {
                if (this != cbAlterado) { this.checked = cbAlterado.checked; }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Geração de Arquivo - Atacado</legend>
            <table border="1" class="style1">
                <tr>
                    <td>
                        <div style="width: 400px;" class="alinhamento">
                            <div style="width: 200px;" class="alinhamento">
                                <label>Data de Lançamento:&nbsp; </label>
                                <asp:TextBox ID="txtData" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                                <asp:Calendar ID="CalendarData" runat="server" OnSelectionChanged="CalendarData_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <asp:TextBox ID="txtDiretorio" runat="server" CssClass="pcRight" Height="16px" Width="100px" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="txtArquivo" runat="server" CssClass="pcRight" Height="16px" Width="100px" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                        </div>
                        <div>
                            <label>Imagem:</label>
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" />
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btAtualizar" Text="Atualizar Produtos" OnClick="btAtualizar_Click"/>
            <asp:Label runat="server" ID="lblMensagem" ForeColor="Red"></asp:Label>
        </div>
        <fieldset class="login">
            <legend>Produtos Atualizados</legend>
            <div>
                <asp:GridView runat="server" ID="GridViewNotaRetiradaItem" AutoGenerateColumns="false" ShowFooter="true" onrowdatabound="GridViewNotaRetiradaItem_RowDataBound" DataKeyNames="CODIGO_NOTA_RETIRADA_ITEM">
                    <Columns>
                        <asp:BoundField DataField="CODIGO_NOTA_RETIRADA_ITEM" HeaderText="Código" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor do Produto" />
                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
<%--                        <asp:BoundField DataField="ORIGEM_DEFEITO" HeaderText="Origem do Defeito" />
                        <asp:BoundField DataField="DEFEITO" HeaderText="Defeito" />
--%>                        <asp:BoundField DataField="DATA_LANCAMENTO" HeaderText="Data do Lançamento" />
<%--                        <asp:BoundField DataField="DESTINO" HeaderText="Destino" />
--%>                        <asp:BoundField DataField="DATA_DESTINO" HeaderText="Data do Destino" />
                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo de Nota" />
<%--                        <asp:BoundField DataField="NUMERO_NOTA" HeaderText="Número da Nota" />
--%>                        <asp:BoundField DataField="DATA_RETORNO" HeaderText="Data do Retorno" />
                        <asp:TemplateField HeaderText="Item">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralItem"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
</asp:Content>
