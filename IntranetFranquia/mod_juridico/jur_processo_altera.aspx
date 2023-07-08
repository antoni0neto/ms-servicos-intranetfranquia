<%@ Page Title="Alterar Processo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="jur_processo_altera.aspx.cs" Inherits="Relatorios.jur_processo_altera"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo Jurídico&nbsp;&nbsp;>&nbsp;&nbsp;Processos&nbsp;&nbsp;>&nbsp;&nbsp;Alterar
                    Processo</span>
                <div style="float: right; padding: 0;">
                    <a href="jur_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div>
                <fieldset>
                    <legend>Alterar Processo</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Requerente
                            </td>
                            <td>Número
                            </td>
                            <td>Tipo de Processo
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 280px;">
                                <asp:TextBox ID="txtRequerente" runat="server" Width="270px" MaxLength="30" Text=""></asp:TextBox>
                            </td>
                            <td style="width: 273px;">
                                <asp:TextBox ID="txtNumeroProcesso" runat="server" Width="263px" MaxLength="30" Text=""></asp:TextBox>
                            </td>
                            <td style="width: 270px;">
                                <asp:DropDownList ID="ddlTipoProcesso" Height="21px" runat="server" DataValueField="CODIGO"
                                    DataTextField="DESCRICAO" Width="264px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscarProcesso" runat="server" Text="Buscar" Width="100px" OnClick="btBuscarProcesso_Click" />&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="labProcesso" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProcesso" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvProcesso_RowDataBound">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="REQUERENTE" HeaderText="Requerente" HeaderStyle-Width="300px"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CARGO" HeaderText="Cargo" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="NUMERO" HeaderText="Número" HeaderStyle-Width="330px"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Tipo de Processo" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTipoProcesso" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="35px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btBuscar" runat="server" Height="19px" Width="35px" Text=">>" OnClick="btBuscar_Click" />
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
