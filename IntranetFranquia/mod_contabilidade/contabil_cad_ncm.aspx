<%@ Page Title="Manutenção NCM" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="contabil_cad_ncm.aspx.cs" Inherits="Relatorios.contabil_cad_ncm" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Manutenção NCM</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Manutenção NCM</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Grupo Produto</td>
                            <td>SubGrupo Produto</td>
                            <td>Griffe</td>
                            <td>Composição</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="ddlGrupoProduto" runat="server" Width="244px" Height="21px" DataTextField="GRUPO_PRODUTO" DataValueField="GRUPO_PRODUTO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="ddlSubGrupoProduto" runat="server" Width="244px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="MALHA" Text="MALHA"></asp:ListItem>
                                    <asp:ListItem Value="PLANO" Text="PLANO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="ddlGriffe" runat="server" Width="244px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="FEMININO" Text="FEMININO"></asp:ListItem>
                                    <asp:ListItem Value="MASCULINO" Text="MASCULINO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="ddlComposicao" runat="server" Width="244px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="ALGODAO" Text="ALGODÃO"></asp:ListItem>
                                    <asp:ListItem Value="ARTIFICIAL" Text="ARTIFICIAL"></asp:ListItem>
                                    <asp:ListItem Value="SINTETICO" Text="SINTÉTICO"></asp:ListItem>
                                    <asp:ListItem Value="LA" Text="LÃ/PÊLOS"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvNCM" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvNCM_RowDataBound"
                                        AllowSorting="true" OnSorting="gvNCM_Sorting"
                                        ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                        <RowStyle HorizontalAlign="Left"></RowStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="GRUPO_PRODUTO" HeaderText="Grupo Produto" HeaderStyle-Width="200px"
                                                SortExpression="GRUPO_PRODUTO" />
                                            <asp:BoundField DataField="SUBGRUPO_PRODUTO" HeaderText="SubGrupo Produto" HeaderStyle-Width="200px"
                                                SortExpression="SUBGRUPO_PRODUTO" />
                                            <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" HeaderStyle-Width="200px"
                                                SortExpression="GRIFFE" />
                                            <asp:BoundField DataField="COMPOSICAO" HeaderText="Composição" HeaderStyle-Width="200px"
                                                SortExpression="COMPOSICAO" />
                                            <asp:TemplateField HeaderText="NCM" HeaderStyle-Width="140px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNCM" runat="server" Width="140px" MaxLength="8" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="" HeaderText="" />
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btSalvar" runat="server" Text="Salvar" Height="21px" Width="100px" OnClick="btSalvar_Click" />
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
