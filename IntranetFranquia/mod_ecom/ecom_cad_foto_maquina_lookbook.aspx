<%@ Page Title="Lookbook - Cadastro de Fotos Máquina" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_foto_maquina_lookbook.aspx.cs" Inherits="Relatorios.ecom_cad_foto_maquina_lookbook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
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
    <script type="text/javascript" src="../jquery-1.6.min.js"></script>
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
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Lookbook&nbsp;&nbsp;>&nbsp;&nbsp;Lookbook - Cadastro de Fotos Máquina</span>
        <div style="float: right; padding: 0;">
            <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset class="login">
            <legend>Lookbook - Cadastro de Fotos Máquina</legend>
            <div class="login">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>Data
                        </td>
                        <td>
                            <asp:Label ID="labCodigoBarra" runat="server" Text="Código de Barra do Produto"></asp:Label>
                        </td>
                        <td>Descrição Produto Sem Etiqueta</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtData" runat="server" Text="" Width="100px" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="width: 250px;">
                            <asp:TextBox ID="txtCodigoBarra" runat="server" Width="240px"></asp:TextBox>
                        </td>
                        <td style="width: 400px;">
                            <asp:TextBox ID="txtDescOutro" runat="server" Width="390px"></asp:TextBox>

                        </td>
                        <td>
                            <asp:Button runat="server" ID="btGravar" Text=">>" Width="40px" OnClick="btGravar_Click" />
                            &nbsp&nbsp
                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button runat="server" ID="btProduto1" Text="1 Peça" Width="104px" OnClick="btProduto1_Click" />
                            <asp:Button runat="server" ID="btProduto2" Text="2 Peças" Width="104px" OnClick="btProduto2_Click" />
                            <asp:Button runat="server" ID="btProduto3" Text="3 Peças" Width="104px" OnClick="btProduto3_Click" />
                        </td>
                        <td style="text-align: right;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvFotoMaquina" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white;" OnRowDataBound="gvFotoMaquina_RowDataBound"
                                    ShowFooter="true" DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="btSalvar" runat="server" Width="40px" Text=">>" OnClick="btSalvar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sequência Inicial" HeaderStyle-Width="115px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSeqInicial" runat="server" Width="115px"
                                                    onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharCentro" MaxLength="4"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sequência Final" HeaderStyle-Width="115px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSeqFinal" runat="server" Width="115px"
                                                    onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharCentro" MaxLength="4"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Produto&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Nome&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cor&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Descrição Cor&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Descrição Produto" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:GridView ID="gvFotoMaquinaProduto" runat="server" Width="100%" AutoGenerateColumns="False" ShowHeader="false" CellPadding="0" CellSpacing="0"
                                                    ForeColor="#333333" Style="background: white;" OnRowDataBound="gvFotoMaquinaProduto_RowDataBound" DataKeyNames="CODIGO" BorderWidth="0">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" ItemStyle-Width="145px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0" />
                                                        <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Nome" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0" />
                                                        <asp:BoundField DataField="COR" HeaderText="Cor" ItemStyle-Width="145px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0" />
                                                        <asp:BoundField DataField="DESC_COR" HeaderText="Descrição Cor" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0" />
                                                        <asp:BoundField DataField="DESC_OUTRO" HeaderText="Descrição Produto" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0" />
                                                    </Columns>
                                                </asp:GridView>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="btExcluir" runat="server" Width="75px" Text="Excluir" OnClick="btExcluir_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
</asp:Content>
