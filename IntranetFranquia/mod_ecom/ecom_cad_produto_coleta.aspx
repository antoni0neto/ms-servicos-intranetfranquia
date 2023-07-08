<%@ Page Title="Lista Pedido Coleta" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_produto_coleta.aspx.cs" Inherits="Relatorios.ecom_cad_produto_coleta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
    <script src="../js/js.js" type="text/javascript"></script>
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
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Lista Pedido Coleta</span>
        <div style="float: right; padding: 0;">
            <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset class="login">
            <legend>Coleta</legend>
            <div class="login">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="labTrackNumber" runat="server" Text="Código de Rastreio"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 500px;">
                            <asp:TextBox ID="txtTrackNumber" runat="server" Width="490px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btGravar" Text=">>" Width="40px" OnClick="btGravar_Click" />&nbsp&nbsp
                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btVerHoje" runat="server" Text="Coleta do Dia" Width="150px" OnClick="btVerHoje_Click" />&nbsp;
                            <asp:Button ID="btLimpar" runat="server" Text="Limpar" Width="150px" OnClick="btLimpar_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                </table>

                <fieldset>
                    <legend>Correios</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="cbMarcarTodos" runat="server" Checked="false" TextAlign="Right" OnCheckedChanged="cbMarcarTodos_CheckedChanged" AutoPostBack="true" />Marcar Todos
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvColetaCorreio" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white;" OnRowDataBound="gvColeta_RowDataBound"
                                        ShowFooter="true" DataKeyNames="TRACK_NUMBER">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbMarcar" runat="server" Checked="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PEDIDO" HeaderText="Pedido" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="TRACK_NUMBER" HeaderText="Código de Rastreio" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Data de Coleta" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataColeta" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="" HeaderText="" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btImprimir" runat="server" Text="Imprimir" OnClick="btImprimir_Click" Width="150px" />
                            </td>
                        </tr>
                    </table>
                </fieldset>

                <fieldset>
                    <legend>Loggi</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="cbMarcarTodosLoggi" runat="server" Checked="false" TextAlign="Right" OnCheckedChanged="cbMarcarTodosLoggi_CheckedChanged" AutoPostBack="true" />Marcar Todos
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvColetaLoggi" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white;" OnRowDataBound="gvColeta_RowDataBound"
                                        ShowFooter="true" DataKeyNames="TRACK_NUMBER">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbMarcar" runat="server" Checked="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PEDIDO" HeaderText="Pedido" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="TRACK_NUMBER" HeaderText="Código de Rastreio" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Data de Coleta" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataColeta" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="" HeaderText="" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btImprimirLoggi" runat="server" Text="Imprimir" OnClick="btImprimirLoggi_Click" Width="150px" />
                            </td>
                        </tr>
                    </table>
                </fieldset>

                <fieldset>
                    <legend>Mandae</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="cbMarcarTodosMandae" runat="server" Checked="false" TextAlign="Right" OnCheckedChanged="cbMarcarTodosMandae_CheckedChanged" AutoPostBack="true" />Marcar Todos
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvColetaMandae" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white;" OnRowDataBound="gvColeta_RowDataBound"
                                        ShowFooter="true" DataKeyNames="TRACK_NUMBER">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbMarcar" runat="server" Checked="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PEDIDO" HeaderText="Pedido" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="TRACK_NUMBER" HeaderText="Código de Rastreio" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Data de Coleta" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataColeta" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="" HeaderText="" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btImprimirMandae" runat="server" Text="Imprimir" OnClick="btImprimirMandae_Click" Width="150px" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </fieldset>
    </div>
</asp:Content>
