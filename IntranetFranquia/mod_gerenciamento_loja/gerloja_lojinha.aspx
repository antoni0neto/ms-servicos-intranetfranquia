<%@ Page Title="Handbook - Lojinha" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="gerloja_lojinha.aspx.cs" Inherits="Relatorios.gerloja_lojinha" %>

<asp:Content ID="contentLojinhaH" ContentPlaceHolderID="HeadContent" runat="server">
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
    <script type="text/javascript" src="../js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MainContent_txtCodigoProduto').keypress(function (e) {
                var code = null;
                code = (e.keyCode ? e.keyCode : e.which);
                return (code == 13) ? false : true;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="contentLojinhaB" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Handbook - Lojinha</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 600px;" valign="bottom">
                        <asp:Button runat="server" ID="btNovaVenda" Text="Novo" Width="100px" OnClick="btNovo_Click" OnClientClick="javascript: return confirm('Tem certeza que abrir uma nova venda?');" />&nbsp;
                        <asp:Button runat="server" ID="btLimpar" Text="Limpar" Width="100px" OnClick="btLimpar_Click" />
                    </td>
                    <td valign="top">
                        <h2>PREÇO FINAL: R$ 
                            <asp:Label ID="labDesconto" runat="server" Text="00,00" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </h2>
                        <asp:Button ID="btFinalizarCompra" runat="server" Text="Finalizar Compra" Width="203px" OnClick="btFinalizarCompra_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja finalizar a venda?');" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlProduto" runat="server" Visible="false">
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <label>CPF/CNPJ</label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 215px;">
                            <asp:TextBox ID="txtCPF" runat="server" CssClass="textEntry" Height="20px" MaxLength="17" Width="205px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:Button runat="server" ID="btBuscarCPF" Text=">>" Width="50px" Height="23px" OnClick="btBuscarCPF_Click" />&nbsp;
                            <asp:Button runat="server" ID="btAbrirVenda" Text="Abrir Venda" Width="150px" Height="23px" OnClick="btAbrirVenda_Click" Visible="false" />
                            &nbsp;
                            <asp:Label ID="labErroCPF" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <label>Nome Funcionário</label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtNomeFuncionario" runat="server" CssClass="textEntry" Height="20px" Width="597px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <label>Código de Barras</label>
                            <asp:HiddenField ID="hidSEQ" runat="server" Value="1" />
                            <asp:HiddenField ID="hidSalarioFuncionario" runat="server" Value="0" />
                            <asp:HiddenField ID="hidValorVenda" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 215px;">
                            <asp:TextBox ID="txtCodigoProduto" runat="server" CssClass="textEntry" Height="20px" MaxLength="15" Width="205px" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="width: 65px;">
                            <asp:Button runat="server" ID="btGravar" Text=">>" Width="50px" Height="23px" OnClick="btGravar_Click" Enabled="false"
                                OnClientClick="DesabilitarBotao(this);" />
                        </td>
                        <td>
                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox ID="cbSalario30" runat="server" TextAlign="Right" Checked="false" Text="" /> Permitir + 30%
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                </table>
                <div>
                    <div class="rounded_corners">
                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                            OnDataBound="gvProduto_DataBound"
                            ShowFooter="true">
                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Produto" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="170px">
                                    <ItemTemplate>
                                        <asp:Label ID="labProduto" runat="server" Text='<%# Bind("PRODUTO")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="260px">
                                    <ItemTemplate>
                                        <asp:Label ID="labDescProduto" runat="server" Text='<%# Bind("DESC_PRODUTO")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="210px">
                                    <ItemTemplate>
                                        <asp:Label ID="labCor" runat="server" Text='<%# Bind("COR")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Preço Original" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="labPrecoTL" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Preço Lojinha" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="labDesconto" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btExcluir" runat="server" Width="18px" ImageUrl="~/Image/delete.png"
                                            ToolTip="Excluir" OnClick="btExcluir_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
            <div>
                <br />
                <br />
            </div>
        </fieldset>
    </div>
</asp:Content>
