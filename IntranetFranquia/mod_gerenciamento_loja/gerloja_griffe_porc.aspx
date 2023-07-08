<%@ Page Title="Griffe Porcentagem" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_griffe_porc.aspx.cs" Inherits="Relatorios.gerloja_griffe_porc" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
        }

        .imgX {
            height: 326px;
            width: 100%;
            overflow: hidden;
            vertical-align: middle;
        }

            .imgX img {
                height: auto;
                width: 225px;
            }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Painel&nbsp;&nbsp;>&nbsp;&nbsp;Griffe Porcentagem</span>
            <div style="float: right; padding: 0;">
                <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <fieldset class="login">
            <legend>Griffe Porcentagem</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>Semana</td>
                    <td>Filial ETC</td>
                    <td>Filial</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 350px;">
                        <asp:DropDownList runat="server" ID="ddlSemana" DataValueField="CODIGO" DataTextField="SEMANA"
                            Width="344px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 210px;">
                        <asp:DropDownList runat="server" ID="ddlFilialEtc" Width="204px">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 260px;">
                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Width="254px">
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button runat="server" ID="btBuscar" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;
                        <asp:Label runat="server" ID="labErro" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;</td>
                </tr>
            </table>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="rounded_corners">
                            <asp:GridView ID="gvGriffePorc" runat="server" Width="100%" AutoGenerateColumns="False"
                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvGriffePorc_RowDataBound" OnDataBound="gvGriffePorc_DataBound" OnSorting="gvGriffePorc_Sorting" AllowSorting="true"
                                ShowFooter="true">
                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true"></FooterStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="15px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial">
                                        <ItemTemplate>
                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FILIAL" HeaderText="Filial" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" SortExpression="FILIAL" />

                                    <asp:BoundField DataField="QTDE_VENDA_FEM" HeaderText="Q.Fem" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="LightPink" SortExpression="QTDE_VENDA_FEM" />
                                    <asp:BoundField DataField="QTDE_ESTOQUE_FEM" HeaderText="E.Fem" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="LightPink" SortExpression="QTDE_ESTOQUE_FEM" />
                                    <asp:TemplateField HeaderText="V.Fem ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="60px" ItemStyle-BackColor="LightPink"
                                        SortExpression="VALOR_VENDA_FEM">
                                        <ItemTemplate>
                                            <asp:Label ID="labValVendaFem" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="E.Fem ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="60px" ItemStyle-BackColor="LightPink"
                                        SortExpression="VALOR_ESTOQUE_FEM">
                                        <ItemTemplate>
                                            <asp:Label ID="labValEstoqueFem" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="%V.Fem" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="LightPink"
                                        SortExpression="PORC_VENDA_FEM">
                                        <ItemTemplate>
                                            <asp:Label ID="labPorcVendaFem" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="QTDE_VENDA_PETIT" HeaderText="Q.Pet" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="Wheat" SortExpression="QTDE_VENDA_PETIT" />
                                    <asp:BoundField DataField="QTDE_ESTOQUE_PETIT" HeaderText="E.Pet" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="Wheat" SortExpression="QTDE_ESTOQUE_PETIT" />
                                    <asp:TemplateField HeaderText="V.Pet ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="60px" ItemStyle-BackColor="Wheat"
                                        SortExpression="VALOR_VENDA_PETIT">
                                        <ItemTemplate>
                                            <asp:Label ID="labValVendaPetit" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="E.Pet ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="60px" ItemStyle-BackColor="Wheat"
                                        SortExpression="VALOR_ESTOQUE_PETIT">
                                        <ItemTemplate>
                                            <asp:Label ID="labValEstoquePetit" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="%V.Pet" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="Wheat"
                                        SortExpression="PORC_VENDA_PETIT">
                                        <ItemTemplate>
                                            <asp:Label ID="labPorcVendaPetit" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="QTDE_VENDA_MASC" HeaderText="Q.Masc" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="LightCyan" SortExpression="QTDE_VENDA_MASC" />
                                    <asp:BoundField DataField="QTDE_ESTOQUE_MASC" HeaderText="E.Masc" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="LightCyan" SortExpression="QTDE_ESTOQUE_MASC" />
                                    <asp:TemplateField HeaderText="V.Masc ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="60px" ItemStyle-BackColor="LightCyan"
                                        SortExpression="VALOR_VENDA_MASC">
                                        <ItemTemplate>
                                            <asp:Label ID="labValVendaMasc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="E.Masc ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="60px" ItemStyle-BackColor="LightCyan"
                                        SortExpression="VALOR_ESTOQUE_MASC">
                                        <ItemTemplate>
                                            <asp:Label ID="labValEstoqueMasc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="%V.Masc" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="LightCyan"
                                        SortExpression="PORC_VENDA_MASC">
                                        <ItemTemplate>
                                            <asp:Label ID="labPorcVendaMasc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:BoundField DataField="QTDE_VENDA_CALC" HeaderText="Q.Calc" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="LightYellow" SortExpression="QTDE_VENDA_CALC" />
                                    <asp:BoundField DataField="QTDE_ESTOQUE_CALC" HeaderText="E.Calc" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="LightYellow" SortExpression="QTDE_ESTOQUE_CALC" />
                                    <asp:TemplateField HeaderText="V.Calc ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="60px" ItemStyle-BackColor="LightYellow"
                                        SortExpression="VALOR_VENDA_CALC">
                                        <ItemTemplate>
                                            <asp:Label ID="labValVendaCalc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="E.Calc ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="60px" ItemStyle-BackColor="LightYellow"
                                        SortExpression="VALOR_ESTOQUE_CALC">
                                        <ItemTemplate>
                                            <asp:Label ID="labValEstoqueCalc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="%V.Calc" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="LightYellow"
                                        SortExpression="PORC_VENDA_CALC">
                                        <ItemTemplate>
                                            <asp:Label ID="labPorcVendaCalc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="QTDE_VENDA_OUTRO" HeaderText="Q.Out" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="Linen" SortExpression="QTDE_VENDA_OUTRO" />
                                    <asp:BoundField DataField="QTDE_ESTOQUE_OUTRO" HeaderText="E.Out" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="Linen" SortExpression="QTDE_ESTOQUE_OUTRO" />
                                    <asp:TemplateField HeaderText="V.Out ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="60px" ItemStyle-BackColor="Linen"
                                        SortExpression="VALOR_VENDA_OUTRO">
                                        <ItemTemplate>
                                            <asp:Label ID="labValVendaOut" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="E.Out ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="60px" ItemStyle-BackColor="Linen"
                                        SortExpression="VALOR_ESTOQUE_OUTRO">
                                        <ItemTemplate>
                                            <asp:Label ID="labValEstoqueOut" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="%V.Out" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" HeaderStyle-Width="50px" ItemStyle-BackColor="Linen"
                                        SortExpression="PORC_VENDA_OUTRO">
                                        <ItemTemplate>
                                            <asp:Label ID="labPorcVendaOut" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="V.Tot ($)" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" ItemStyle-Font-Names="Arial" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="PaleGreen"
                                        SortExpression="">
                                        <ItemTemplate>
                                            <asp:Label ID="labValVendaTot" runat="server"></asp:Label>
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
</asp:Content>
