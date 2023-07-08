<%@ Page Title="Cálculo Comissão Loja" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="rh_folha_loja_calculo.aspx.cs" Inherits="Relatorios.rh_folha_loja_calculo"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Solicitações de Transferência Temporária</span>
        <div style="float: right; padding: 0;">
            <a href="rh_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div style="float: left; width: 1200px; margin-left: 8%;">
        <fieldset>
            <legend>Calcular Comissão Loja</legend>
            <fieldset>
                <legend>Importação Arquivo</legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Competência
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 256px;">
                            <asp:DropDownList runat="server" ID="ddlCompetencia" DataValueField="CODIGO_ACOMPANHAMENTO_MESANO" DataTextField="DESCRICAO"
                                Height="22px" Width="250px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 200px;">
                            <asp:FileUpload ID="uploadArquivoFolha" runat="server" />
                        </td>
                        <td style="width: 110px;">&nbsp;
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btImportar" runat="server" Text="Importar" Width="100px" OnClick="btImportar_Click" OnClientClick="DesabilitarBotao(this);" />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>Comissões</legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="text-align: left; width: 33%;">
                            <asp:Button ID="btCalcularComissao" runat="server" Text="Calcular" Width="100px" OnClick="btCalcularComissao_Click" OnClientClick="DesabilitarBotao(this);" />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="text-align: center; width: 33%;">
                            <asp:Label ID="labMsg" runat="server" ForeColor="Red" Text="" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="text-align: right; width: 33%;">
                            <asp:Button ID="btExportar" runat="server" Text="Exportar" Width="100px" OnClick="btExportar_Click" OnClientClick="DesabilitarBotao(this);" />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </fieldset>
    </div>
    <div>
        <asp:Panel ID="pnlExcel" runat="server">
            <asp:GridView ID="gvExcel" runat="server" Width="100%" AutoGenerateColumns="False"
                ForeColor="#333333" Style="background: white" ShowFooter="true">
                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                <Columns>
                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="COMPETENCIA" HeaderText="Competência" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CPF" HeaderText="CPF" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="MATRICULA" HeaderText="Matrícula" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="NOME" HeaderText="Nome" ItemStyle-Width="400px" />
                    <asp:BoundField DataField="CARGO" HeaderText="Cargo" />
                    <asp:BoundField DataField="FILIAL" HeaderText="Filial" ItemStyle-Width="250px" />
                    <asp:BoundField DataField="COMISSAO_PORC" HeaderText="% Comissão" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="MINIMO_GARANTIDO" HeaderText="Mínimo Garantido" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="COMISSAO_VENDA_TOTAL_2120" HeaderText="Comissão" ItemStyle-Width="130px" />
                    <asp:BoundField DataField="PREMIO_PONTA" HeaderText="Prêmio Ponta" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="PREMIO_VENDEDOR" HeaderText="Prêmio Vendedor" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="PREMIO_COTA" HeaderText="Prêmio Cota" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="" HeaderText="Prêmio ConvColet" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="SUPERVISOR" HeaderText="Supervisor" ItemStyle-Width="250px" />
                    <asp:BoundField DataField="GERENTE" HeaderText="Gerente" ItemStyle-Width="300px" />
                    <asp:BoundField DataField="EMPRESA" HeaderText="Empresa" ItemStyle-Width="400px" />
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </div>

</asp:Content>
