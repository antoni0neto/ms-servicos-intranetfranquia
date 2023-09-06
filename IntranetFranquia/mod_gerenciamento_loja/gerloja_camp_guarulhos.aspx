<%@ Page Title="Handclub Guarulhos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_camp_guarulhos.aspx.cs" Inherits="Relatorios.gerloja_camp_guarulhos" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Handclub Guarulhos</span>
                <div style="float: right; padding: 0;">
                    <a href="gerloja_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Handclub Guarulhos</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="5"><br /></td>
                        </tr>
                        <tr>
                            <td>Filial
                            </td>
                            <td>Sexo
                            </td>
                            <td>Nome
                            </td>
                            <td>CPF
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlFilialCompra" runat="server" Width="194px" Height="21px"
                                    DataTextField="FILIAL_COMPRA" DataValueField="COD_FILIAL_COMPRA">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlSexo" runat="server" Width="194px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="F" Text="FEMININO"></asp:ListItem>
                                    <asp:ListItem Value="M" Text="MASCULINO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px">
                                <asp:TextBox ID="txtNome" runat="server" Width="190px"></asp:TextBox>
                            </td>
                            <td style="width: 200px">
                                <asp:TextBox ID="txtCPF" runat="server" Width="190px"></asp:TextBox>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div id="accordionP">
                                    <h3>Clientes</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvClientes" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvClientes_RowDataBound"
                                                            OnDataBound="gvClientes_DataBound"
                                                            OnSorting="gvClientes_Sorting" AllowSorting="true" ShowFooter="true"
                                                            DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="FILIAL_COMPRA" HeaderText="Filial de Compra" HeaderStyle-Width="" SortExpression="FILIAL_COMPRA" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="NOME" HeaderText="Nome" HeaderStyle-Width="" SortExpression="NOME" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="CPF" HeaderText="CPF" HeaderStyle-Width="130px" SortExpression="CPF" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:TemplateField HeaderText="Aniversário" HeaderStyle-Width="130px" SortExpression="ANIVERSARIO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litAniversario" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Sexo" HeaderStyle-Width="120px" SortExpression="SEXO">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litSexo" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>



                                                                <asp:BoundField DataField="DDD1" HeaderText="DDD" HeaderStyle-Width="60px" SortExpression="DDD1" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="TELEFONE1" HeaderText="Telefone" HeaderStyle-Width="120px" SortExpression="TELEFONE1" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="DDD2" HeaderText="DDD" HeaderStyle-Width="60px" SortExpression="DDD2" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="TELEFONE2" HeaderText="Telefone" HeaderStyle-Width="120px" SortExpression="TELEFONE2" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
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
