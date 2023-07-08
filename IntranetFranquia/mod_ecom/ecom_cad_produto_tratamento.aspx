<%@ Page Title="Seleção de Fotos Tratamento" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_produto_tratamento.aspx.cs" Inherits="Relatorios.ecom_cad_produto_tratamento" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro&nbsp;&nbsp;>&nbsp;&nbsp;Seleção de Fotos Tratamento</span>
                <div style="float: right; padding: 0;">
                    <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Seleção de Fotos Tratamento</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Coleção
                            </td>
                            <td>Grupo Produto
                            </td>
                            <%--                            <td>HB
                            </td>--%>
                            <td>Produto
                            </td>
                            <td>Griffe
                            </td>
                            <td>Produção Cortada
                            </td>
                            <td>Foto HandbookOnline</td>
                            <td>Foto Tratamento</td>
                        </tr>
                        <tr>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlColecao" runat="server" Width="194px" Height="21px"
                                    DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="194px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                    DataValueField="GRUPO_PRODUTO">
                                </asp:DropDownList>
                            </td>
                            <%--                            <td style="width: 160px">
                                <asp:TextBox ID="txtHB" runat="server" onkeypress="return fnValidarNumero(event);"
                                    Width="150px"></asp:TextBox>
                            </td>--%>
                            <td style="width: 160px">
                                <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                    DataValueField="GRIFFE">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 160px">
                                <asp:DropDownList ID="ddlProducaoCortada" runat="server" Width="154px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px">
                                <asp:DropDownList ID="ddlFotoHandbookOnline" runat="server" Width="174px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFotoTratamento" runat="server" Width="170px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btTransferir" runat="server" Text="Transferir Fotos" Width="130px" OnClick="btTransferir_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <div id="accordionP">
                                    <h3>Produtos</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                            OnDataBound="gvProduto_DataBound"
                                                            OnSorting="gvProduto_Sorting" AllowSorting="true" ShowFooter="true"
                                                            DataKeyNames="GRUPO_PRODUTO, PRODUTO, COR">
                                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DESC_COLECAO" HeaderText="Coleção" HeaderStyle-Width="120px" SortExpression="DESC_COLECAO" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                <%--<asp:BoundField DataField="HB" HeaderText="HB" HeaderStyle-Width="100px" SortExpression="HB" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />--%>
                                                                <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="100px" SortExpression="PRODUTO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Nome" SortExpression="DESC_PRODUTO" />
                                                                <asp:BoundField DataField="DESC_COR" HeaderText="Cor" SortExpression="DESC_COR" />
                                                                <asp:BoundField DataField="GRUPO_PRODUTO" HeaderText="Grupo Produto" HeaderStyle-Width="" SortExpression="GRUPO_PRODUTO" />
                                                                <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" HeaderStyle-Width="" SortExpression="GRIFFE" />

                                                                <asp:TemplateField HeaderText="Mostruário" ItemStyle-HorizontalAlign="Left" SortExpression="MOSTRUARIO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litMostruario" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Produção" ItemStyle-HorizontalAlign="Left" SortExpression="PRODUCAO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litProducao" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="FT HandbookOnline" ItemStyle-HorizontalAlign="Left" SortExpression="FT_HANDBOOKONLINE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litFTHandbookOnline" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="FT Tratamento" ItemStyle-HorizontalAlign="Left" SortExpression="FT_TRATAMENTO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litFTTratamento" runat="server"></asp:Literal>
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
