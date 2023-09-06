<%@ Page Title="Relatório - Vitrine da Semana" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="zold_mkt_con_vitrine_semanal.aspx.cs" Inherits="Relatorios.zold_mkt_con_vitrine_semanal" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Marketing&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Vitrine da Semana</span>
                <div style="float: right; padding: 0;">
                    <a href="mkt_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Vitrine da Semana</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Semana
                            </td>
                            <td>Supervisor
                            </td>
                            <td>Filial
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px;">
                                <asp:DropDownList runat="server" ID="ddlAnoSemana" DataValueField="value" DataTextField="text"
                                    Height="21px" Width="244px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList runat="server" ID="ddlSupervisor" DataValueField="CODIGO_USUARIO" DataTextField="NOME_USUARIO"
                                    Height="22px" Width="194px" OnSelectedIndexChanged="ddlSupervisor_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="21px" Width="244px">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="accordionV">
                                    <h3>VITRINES</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvFotoVitrine" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvFotoVitrine_RowDataBound"
                                                            OnDataBound="gvFotoVitrine_DataBound" ShowFooter="true">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litFilial" runat="server" Text=""></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PRINCIPAL" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto1" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PRINCIPAL ADICIONAL" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto2" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="MASCULINA" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto3" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="EXTRA" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto4" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CUBO 1" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto5" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CUBO 2" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto6" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CUBO 3" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto7" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
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

                                <div id="accordionA">
                                    <h3>ACESSÓRIOS</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvFotoAcessorio" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvFotoAcessorio_RowDataBound"
                                                            OnDataBound="gvFotoAcessorio_DataBound" ShowFooter="true">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litFilial" runat="server" Text=""></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="ÓCULOS" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto1" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="BIJUS" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto2" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CALÇADO FEMININO" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto3" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CALÇADO FEMININO EXTRA" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto4" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CALÇADO MASCULINO" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto5" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CALÇADO MASCULINO" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto6" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="EXTRA" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto7" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
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

                                <div id="accordionR">
                                    <h3>ARMARIOS</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvFotoArmario" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvFotoArmario_RowDataBound"
                                                            OnDataBound="gvFotoArmario_DataBound" ShowFooter="true">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litFilial" runat="server" Text=""></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FEMININO 1" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto1" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FEMININO 2" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto2" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FEMININO 3" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto3" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FEMININO 4" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto4" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FEMININO 5" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto5" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FEMININO 6" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto6" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="MASCULINO 1" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto7" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="MASCULINO 2" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto8" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="MASCULINO 3" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto9" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="MASCULINO 4" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto10" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="MASCULINO 5" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto11" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="MASCULINO 6" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgFoto12" runat="server" Width="110px" Height="" AlternateText=" " ImageAlign="AbsMiddle" OnClick="imgFoto_Click" />
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
