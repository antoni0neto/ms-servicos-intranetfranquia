<%@ Page Title="Ranking de Vendedores 2020" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_vendedorranknv.aspx.cs" Inherits="Relatorios.gerloja_vendedorranknv" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }

        .imgGray {
            -webkit-filter: grayscale(100%);
            filter: grayscale(100%);
        }

        .pageStyl a, .pageStyl span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: left;
            text-decoration: none;
        }

        .pageStyl a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .pageStyl span {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vendedores&nbsp;&nbsp;>&nbsp;&nbsp;Ranking de Vendedores 2020</span>
                <div style="float: right; padding: 0;">
                    <a href="gerloja_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <table border="0" class="table">
                    <tr>
                        <td>
                            <fieldset style="padding-top: 0;">
                                <legend>Ranking de Vendedores 2020</legend>
                                <br />
                                <div id="accordionR">
                                    <h6>
                                        <asp:Label ID="labPontoSemana" runat="server" Text="Pontos da Semana - 01/01/2000 à 31/12/2099"></asp:Label>
                                    </h6>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvPontoSemana" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvPontoSemana_RowDataBound"
                                                            OnDataBound="gvPontoSemana_DataBound" ShowFooter="true">
                                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="" HeaderStyle-Width="150px">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgCaricatura" runat="server" ImageAlign="AbsMiddle" Width="150px" AlternateText=" " ToolTip="Caricatura" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Vendedor" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litNomeVendedor" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Filial" HeaderStyle-Width="250px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="VENDA_PONTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Vendas" HeaderStyle-Width="100px" />
                                                                <asp:BoundField DataField="GOLDEN_PONTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Golden" HeaderStyle-Width="100px" />
                                                                <asp:BoundField DataField="DIGITAL_PONTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Digital" HeaderStyle-Width="100px" />
                                                                <asp:BoundField DataField="CALCADO_PONTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Calçado" HeaderStyle-Width="100px" />
                                                                <asp:BoundField DataField="PONTO_TOTAL" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Total" HeaderStyle-Width="100px" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                                <div id="accordionA">
                                    <h3>Pontos Acumulados - Início - 01/01/0001</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvPontoAcumulado" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvPontoAcumulado_RowDataBound"
                                                            OnDataBound="gvPontoAcumulado_DataBound" ShowFooter="true"
                                                            OnPageIndexChanging="gvPontoAcumulado_PageIndexChanging" AllowPaging="true" PageSize="40">
                                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                            <PagerStyle HorizontalAlign="Left" CssClass="pageStyl" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="" HeaderStyle-Width="150px">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgCaricatura" runat="server" ImageAlign="AbsMiddle" Width="150px" AlternateText=" " ToolTip="Caricatura" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Vendedor" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litNomeVendedor" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Filial" HeaderStyle-Width="300px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PONTO_ACUMULADO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Pontos Acumulados" HeaderStyle-Width="200px" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
