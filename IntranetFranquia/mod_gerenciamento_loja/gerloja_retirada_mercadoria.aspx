<%@ Page Title="Transferência/Devolução de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_retirada_mercadoria.aspx.cs" Inherits="Relatorios.gerloja_retirada_mercadoria" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="js/js.js" type="text/javascript"></script>
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
    <fieldset class="login">
        <legend>Transferência/Devolução de Produto</legend>
        <div class="accountInfo">
            <fieldset>
                <legend>Produtos</legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Filial<asp:HiddenField ID="hidCodigoTransferencia" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 256px;">
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                Height="22px" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Código de Barra</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCodigoProduto" runat="server" CssClass="textEntry" MaxLength="30" Height="20px" Width="248px"></asp:TextBox>&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btGravar" Text=">>" Width="40px" OnClick="btGravar_Click" ValidationGroup="produto" />
                            &nbsp;&nbsp;
                        <asp:Label runat="server" ID="labErroBip" ForeColor="Red"></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div>
            <fieldset>
                <div class="rounded_corners">
                    <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                        ShowFooter="true" DataKeyNames="CODIGO">
                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CODIGO_BARRA" HeaderText="Código de Barra" HeaderStyle-Width="220px" />
                            <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="220px" />
                            <asp:BoundField DataField="NOME" HeaderText="Nome" />
                            <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="labCor" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TAMANHO" HeaderText="Grade" />
                            <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="labTipo" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FILIAL_ORIGEM" HeaderText="Origem" />
                            <asp:TemplateField HeaderStyle-Width="65px">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btExcluir" Text="Excluir" Width="65px" OnClick="btExcluir_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
            <div>
                <fieldset>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Descrição
                            </td>
                            <td>Volume
                            </td>
                            <td>Motivo
                            </td>
                            <td>
                                <asp:Label ID="labParaFilial" runat="server" Text="Para Filial" Visible="false"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 510px;">
                                <asp:TextBox ID="txtObservacao" runat="server" MaxLength="1000" Height="20px" Width="500px" textAlign="rigth"></asp:TextBox>
                            </td>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtVolume" runat="server" MaxLength="10" Height="20px" Width="240px" textAlign="rigth"></asp:TextBox>
                            </td>
                            <td style="width: 170px;">
                                <asp:DropDownList ID="ddlMotivo" runat="server" Width="164px" Height="22px" OnSelectedIndexChanged="ddlMotivo_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                    <asp:ListItem Value="R" Text="RETIRADA"></asp:ListItem>
                                    <asp:ListItem Value="T" Text="TRANSFERÊNCIA"></asp:ListItem>
                                    <asp:ListItem Value="P" Text="PERMUTA"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 310px;">
                                <asp:DropDownList runat="server" ID="ddlParaFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="21px" Width="304px" Visible="false">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>

                    <br />
                    <asp:Button runat="server" ID="btSalvar" Text="Salvar" Width="120px" OnClick="btSalvar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;
                <asp:Label runat="server" ID="labErro" ForeColor="Red"></asp:Label>
                </fieldset>
            </div>
            <div>
                <br />
                <br />
                <br />
            </div>
        </div>
        <div id="dialogPai" runat="server">
            <div id="dialog" title="Mensagem" class="divPop">
                <table border="0" width="100%">
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; color: red;">
                            <strong>Aviso</strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; color: red;">PRODUTOS ENVIADOS COM SUCESSO
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </fieldset>
</asp:Content>
