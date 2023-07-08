<%@ Page Title="Pedido de Transferência/Devolução de Produtos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="admloj_retirada_mercadoria_carr_retirada_pedidos.aspx.cs" Inherits="Relatorios.admloj_retirada_mercadoria_carr_retirada_pedidos" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo ADM Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Logística&nbsp;&nbsp;>&nbsp;&nbsp;Pedido de Transferência/Devolução de Produtos</span>

            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Pedido de Transferência/Devolução de Produtos</legend>
                    <table border="0" class="style1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Filial
                            </td>
                            <td>Pedido Aberto?
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 310px">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="22px"
                                    Width="304px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPedidoAberto" Height="22px"
                                    Width="150px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btBuscar" Text="Buscar" Width="120px" OnClick="btBuscar_Click" />&nbsp;
                                                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </table>
                    <table border="0" class="style1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvPedidoTransferencia" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedidoTransferencia_RowDataBound"
                                        OnDataBound="gvPedidoTransferencia_DataBound" AllowSorting="true" ShowFooter="true">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Data Solicitação" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataSol" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Descrição" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDescricao" runat="server" Text='<%# Bind("OBSERVACAO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Data Envio" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataEnvio" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="85px">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btExcluir" Text="Excluir" Width="85px" OnClick="btExcluir_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir este Pedido?');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="85px">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btVisualizar" Text="Visualizar" Width="85px" OnClick="btVisualizar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="85px">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btAbrir" Text="Separar" Width="85px" OnClick="btAbrir_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>



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
