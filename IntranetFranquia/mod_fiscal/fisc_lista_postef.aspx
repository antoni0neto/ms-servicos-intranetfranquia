<%@ Page Title="Vendas POS/TEF" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="fisc_lista_postef.aspx.cs"
    Inherits="Relatorios.fisc_lista_postef" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            color: black;
        }

        .jGrowl .redError {
            color: red;
        }

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
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Controle Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Vendas POS/TEF</span>
                <div style="float: right; padding: 0;">
                    <a href="fisc_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 100%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Vendas POS/TEF"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvVendas" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" />
                                        <Columns>
                                            <asp:BoundField DataField="col" HeaderText="" ItemStyle-Width="25px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="cod_filial" HeaderText="Código da Filial" Visible="false" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="filial" HeaderText="Filial" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="terminal" HeaderText="Terminal" ItemStyle-HorizontalAlign="Center"  />
                                            <asp:BoundField DataField="numero_cupom_fiscal" HeaderText="Cupom Fiscal" ItemStyle-HorizontalAlign="Center"  />
                                            <asp:BoundField DataField="caixa_vendedor" HeaderText="Vendedor" ItemStyle-HorizontalAlign="Center"  />
                                            <asp:BoundField DataField="hora_venda" HeaderText="Hora Venda" ItemStyle-HorizontalAlign="Center"  />
                                            <asp:BoundField DataField="vendedor_apelido" HeaderText="Apelido" Visible="false" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="nome_vendedor" HeaderText="Nome" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="cod_forma_pgto" HeaderText="Cód.Forma Pagto" Visible="false" />
                                            <asp:BoundField DataField="forma_pgto" HeaderText="Forma Pagto" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="numero_ccf" HeaderText="Número CCF" ItemStyle-HorizontalAlign="Center"  />
                                            <asp:BoundField DataField="parcela" HeaderText="Parcela" ItemStyle-HorizontalAlign="Center"  />
                                            <asp:BoundField DataField="codigo_administradora" HeaderText="Código Administradora" Visible="false"  />
                                            <asp:BoundField DataField="administradora" HeaderText="Administradora" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="tipo_pgto" HeaderText="Tipo Pgto" Visible="false"  />
                                            <asp:BoundField DataField="desc_tipo_pgto" HeaderText="Tipo Pgto" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="numero_aprovacao_cartao" HeaderText="Aprov. Cartão" ItemStyle-HorizontalAlign="Right"  />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
