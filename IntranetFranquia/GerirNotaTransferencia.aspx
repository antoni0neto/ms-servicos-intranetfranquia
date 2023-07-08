<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="GerirNotaTransferencia.aspx.cs" Inherits="Relatorios.GerirNotaTransferencia" %>

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
    <script type="text/javascript">
        function doClick(buttonName, e) {
            //the purpose of this function is to allow the enter key to 
            //point to the correct button to click.
            var key;

            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(buttonName);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>
    <script type="text/javascript" src="jquery-1.6.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input').keypress(function (e) {
                var code = null;
                code = (e.keyCode ? e.keyCode : e.which);
                return (code == 13) ? false : true;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <div style="width: 600px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Início:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Fim:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Filial:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px" Width="200px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btNotasTransfer" Text="Buscar Notas Transferencia" OnClick="btNotasTransfer_Click"/>
        </div>
        <table class="style1">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="GridViewNotas" AutoGenerateColumns="false" OnRowDataBound="GridViewNotas_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="CODIGO" HeaderText="Código" visible = "true"/>
                            <asp:BoundField DataField="DESCRICAO" HeaderText="Nota" />
                            <asp:BoundField DataField="DATA_INICIO" HeaderText="Inicio Leitura" />
                            <asp:BoundField DataField="DATA_FIM" HeaderText="Fim Leitura" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btAlterarNota" Text="Alterar Nota" OnClick="btAlterarNota_Click" />
                                    <asp:Button runat="server" ID="btResumoNota" Text="Resumo de Nota" OnClick="btResumoNota_Click" />
                                    <asp:Button runat="server" ID="btGerarArquivo" Text="Gerar Arquivo" OnClick="btGerarArquivo_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja gerar arquivo ?');" />
                                    <asp:Button runat="server" ID="btExcluirNotas" Text="Excluir Nota" OnClick="btExcluirNotas_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir nota de transferência ?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
                <td>
                    <asp:GridView runat="server" ID="GridViewResumo" AutoGenerateColumns="false" Visible = "false">
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
                        <Columns>
                            <asp:BoundField DataField="barra" HeaderText="Código de Barra" />
                            <asp:BoundField DataField="produto" HeaderText="Descrição" />
                            <asp:BoundField DataField="cor" HeaderText="Cor" />
                            <asp:BoundField DataField="qtEscaneada" HeaderText="Qtd Escaneada" />
                            <asp:BoundField DataField="qtEstoque" HeaderText="Qtd Estoque" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <asp:Button runat="server" ID="btIncluirNota" Text="Incluir Nota" OnClick="btIncluirNota_Click" Visible = "false" />
        <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>
