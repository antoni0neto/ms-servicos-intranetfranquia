<%@ Page Title="Cadastro de Grades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="prod_cad_grade.aspx.cs" Inherits="Relatorios.prod_cad_grade"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Grades</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Cadastro de Grades</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>
                                <asp:Label ID="labNomeGrade" runat="server" Text="Nome da Grade"></asp:Label>
                                <asp:HiddenField ID="hidCodigo" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="labGradeEXP" runat="server" Text="Grade 1"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labGradeXP" runat="server" Text="Grade 2"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labGradePP" runat="server" Text="Grade 3"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labGradeP" runat="server" Text="Grade 4"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labGradeM" runat="server" Text="Grade 5"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labGradeG" runat="server" Text="Grade 6"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labGradeGG" runat="server" Text="Grade 7"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">
                                <asp:TextBox ID="txtNomeGrade" runat="server" Width="190px" MaxLength="30"></asp:TextBox>
                            </td>
                            <td style="width: 90px;">
                                <asp:TextBox ID="txtGradeEXP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                    Width="80px"></asp:TextBox>
                            </td>
                            <td style="width: 90px;">
                                <asp:TextBox ID="txtGradeXP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                    Width="80px"></asp:TextBox>
                            </td>
                            <td style="width: 90px;">
                                <asp:TextBox ID="txtGradePP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                    Width="80px"></asp:TextBox>
                            </td>
                            <td style="width: 90px;">
                                <asp:TextBox ID="txtGradeP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                    Width="80px"></asp:TextBox>
                            </td>
                            <td style="width: 90px;">
                                <asp:TextBox ID="txtGradeM" runat="server" MaxLength="6" CssClass="alinharDireita"
                                    Width="80px"></asp:TextBox>
                            </td>
                            <td style="width: 90px;">
                                <asp:TextBox ID="txtGradeG" runat="server" MaxLength="6" CssClass="alinharDireita"
                                    Width="80px"></asp:TextBox>
                            </td>
                            <td style="width: 90px;">
                                <asp:TextBox ID="txtGradeGG" runat="server" MaxLength="6" CssClass="alinharDireita"
                                    Width="80px"></asp:TextBox>
                            </td>
                            <td style="width: 200px;">
                                <asp:Button runat="server" ID="btIncluir" Text="Incluir" OnClick="btIncluir_Click"
                                    Width="100px" Enabled="true" />
                            </td>
                            <td>
                                <div style="float: right; margin-right: 0px;">
                                    <asp:Button runat="server" ID="btCancelar" Text="Cancelar" OnClick="btCancelar_Click"
                                        Enabled="true" Width="100px" />
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="10">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="10">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvGrade" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvGrade_RowDataBound">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="GRADE" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nome da Grade" />
                                            <asp:BoundField DataField="GRADE_EXP" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Grade 1" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="GRADE_XP" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Grade 2" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="GRADE_PP" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Grade 3" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="GRADE_P" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Grade 4" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="GRADE_M" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Grade 5" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="GRADE_G" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Grade 6" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="GRADE_GG" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Grade 7" HeaderStyle-Width="100px" />
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="65px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btAlterar" runat="server" Height="19px" Width="65px" Text="Alterar" OnClick="btAlterar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="65px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btExcluir" runat="server" Height="19px" Width="65px" Text="Excluir" OnClientClick="return ConfirmarExclusao();"
                                                        OnClick="btExcluir_Click" />
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
