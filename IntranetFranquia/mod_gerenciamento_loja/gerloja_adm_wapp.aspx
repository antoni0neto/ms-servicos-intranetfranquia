<%@ Page Title="WhatsApp - Gerenciamento de Vendedores" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_adm_wapp.aspx.cs" Inherits="Relatorios.gerloja_adm_wapp" %>

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
            <asp:Label ID="labTitulo" runat="server" Text="Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;WhatsApp - Gerenciamento de Vendedores"></asp:Label></span>
        <div style="float: right; padding: 0;">
            <a href="gerloja_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset>
            <legend>WhatsApp - Gerenciamento de Vendedores</legend>
            <div class="rounded_corners">
                <asp:GridView ID="gvWapp" runat="server" Width="100%"
                    AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="gvWapp_RowDataBound">
                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                    <RowStyle HorizontalAlign="Center"></RowStyle>
                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                    <Columns>

                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            SortExpression="FILIAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="labFilial" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            SortExpression="NOME" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="">
                            <ItemTemplate>
                                <asp:Label ID="labNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cargo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            SortExpression="DESC_CARGO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <asp:Label ID="labCargo" runat="server" Text='<%# Bind("DESC_CARGO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Telefone" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            SortExpression="" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <asp:Label ID="labTelefone" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Admissão" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            SortExpression="DATA_ADMISSAO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <asp:Label ID="labAdmissao" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Demissão" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            SortExpression="DATA_DEMISSAO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <asp:Label ID="labDemissao" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            SortExpression="" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Button ID="btAtualizar" runat="server" Width="100px" Text="Baixar" OnClick="btAtualizar_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
</asp:Content>
