<%@ Page Title="SMS" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gest_sms.aspx.cs" Inherits="Relatorios.gest_sms" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Gestão&nbsp;&nbsp;>&nbsp;&nbsp;SMS&nbsp;&nbsp;>&nbsp;&nbsp;SMS</span>
                <div style="float: right; padding: 0;">
                    <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>SMS</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Campanha
                            </td>
                            <td>Enviado
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlCampanha" runat="server" Width="194px" Height="21px"
                                    DataTextField="CAMPANHA" DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlEnviado" runat="server" Width="194px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="accordionP">
                                    <h3>Clientes</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td>Mensagem
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtMensagem" runat="server" Text="" Width="100%" MaxLength="300"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btEnviarTodos" runat="server" Text="Enviar para Todos" Width="150px" OnClick="btEnviarTodos_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja enviar este SMS?');" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvCliente" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCliente_RowDataBound"
                                                            OnDataBound="gvCliente_DataBound" AllowSorting="true" ShowFooter="true"
                                                            DataKeyNames="CODIGO_CLIENTE, CODIGO_CAMPANHA">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CODIGO_CLIENTE" HeaderText="CÓD. Cliente" HeaderStyle-Width="150px" />
                                                                <asp:BoundField DataField="NOME" HeaderText="Nome" />
                                                                <asp:BoundField DataField="DDD" HeaderText="DDD" />
                                                                <asp:BoundField DataField="TELEFONE" HeaderText="Telefone" />
                                                                <asp:BoundField DataField="CAMPANHA" HeaderText="Campanha" />
                                                                <asp:BoundField DataField="QTDE_MSG" HeaderText="Qtde Msg" ItemStyle-HorizontalAlign="Center" />
                                                                <asp:TemplateField HeaderText="Último Envio" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litUltimoEnvio" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btEnviar" runat="server" Text="Enviar" Width="65px" OnClick="btEnviar_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja enviar este SMS?');" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
