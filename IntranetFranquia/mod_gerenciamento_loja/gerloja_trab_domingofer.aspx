<%@ Page Title="Domingos e Feriados Trabalhados" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_trab_domingofer.aspx.cs" Inherits="Relatorios.gerloja_trab_domingofer" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">
            <asp:Label ID="labTitulo" runat="server" Text="Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Domingos e Feriados Trabalhados"></asp:Label></span>
        <div style="float: right; padding: 0;">
            <a href="gerloja_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset>
            <legend>Domingos e Feriados Trabalhados</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>Competência
                    </td>
                    <td>Supervisor
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="ddlSemana" runat="server" Width="194px" DataTextField="SEMANA" DataValueField="SEMANA"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSupervisor" runat="server" DataValueField="CODIGO_USUARIO" DataTextField="NOME_USUARIO" Width="300px"></asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Button runat="server" ID="btBuscar" Text="Buscar" OnClick="btBuscar_Click" Width="90px" />
                        &nbsp;&nbsp;<asp:Button runat="server" ID="btImprimir" Text="Imprimir" OnClick="btImprimir_Click" Width="90px" />
                        &nbsp;&nbsp;<asp:Button runat="server" ID="btEnviarEmail" Text="Email" OnClick="btEnviarEmail_Click" Width="90px" />
                        &nbsp;&nbsp;<asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <fieldset>
                <legend>Gerentes</legend>
                <div class="rounded_corners">
                    <asp:GridView ID="gvTrabDomingo" runat="server" Width="100%"
                        AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="gvTrabDomingo_RowDataBound">
                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                        <Columns>

                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Supervisor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                SortExpression="NOME_USUARIO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="180px">
                                <ItemTemplate>
                                    <asp:Label ID="labSuper" runat="server" Text='<%# Bind("NOME_USUARIO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                SortExpression="FILIAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="labFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gerente" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                SortExpression="NOME" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="300px">
                                <ItemTemplate>
                                    <asp:Label ID="labNomeVendedor" runat="server" Text='<%# Bind("NOME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tot Domingo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                SortExpression="TOT_DOMINGO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTotDomingo" runat="server" Width="140px" CssClass="alinharCentro"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tot Feriado" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                SortExpression="TOT_FERIADO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTotFeriado" runat="server" Width="140px" CssClass="alinharCentro"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Última Atualização" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                SortExpression="DATA_ATUALIZACAO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="">
                                <ItemTemplate>
                                    <asp:Label ID="labDataAtualizacao" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                SortExpression="" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Button ID="btAtualizar" runat="server" Width="100px" Text="Atualizar" OnClick="btAtualizar_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </fieldset>
    </div>
</asp:Content>
