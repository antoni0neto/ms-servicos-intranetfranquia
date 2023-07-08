<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CadastroPackGrade.aspx.cs" Inherits="Relatorios.CadastroPackGrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="jquery-1.6.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input').keypress(function (e) {
                var code = null;
                code = (e.keyCode ? e.keyCode : e.which);
                return (code == 13) ? false : true;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Cadastro de Pack Grade</legend>
            <table class="style1">
                <tr>
                    <td>
                        <fieldset class="login">
                            <div style="width: 200px;"  class="alinhamento">
                                <label>Grupo:&nbsp; </label>
                                <asp:DropDownList runat="server" ID="ddlGrupo" DataValueField="CODIGO_GRUPO" DataTextField="GRUPO_PRODUTO" Height="26px" 
                                    Width="200px" ondatabound="ddlGrupo_DataBound"></asp:DropDownList>
                            </div>
                            <div>
                                <label>Tipo:&nbsp; </label>
                                <asp:TextBox ID="txtDescricaoTipo" runat="server" CssClass="textEntry" MaxLength="100" Height="22px" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorTipo" runat="server" SetFocusOnError="true"
                                        Display="None" ErrorMessage="Preencha a descrição do tipo" ControlToValidate="txtDescricaoTipo" ValidationGroup="pack_grade"></asp:RequiredFieldValidator>
                            </div>
                            <div style="width: 200px;"  class="alinhamento">
                                <label>Griffe:&nbsp; </label>
                                <asp:DropDownList runat="server" ID="ddlGriffe" DataValueField="CODIGO_GRIFFE" DataTextField="DESCRICAO" Height="26px" 
                                    Width="200px" ondatabound="ddlGriffe_DataBound"></asp:DropDownList>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="login">
                            <div>
                                <label>Qtde XP - Pack A:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeXpPackA" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde PP - Pack A:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdePpPackA" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde PQ - Pack A:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdePqPackA" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde MD - Pack A:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeMdPackA" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde GD - Pack A:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeGdPackA" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde GG - Pack A:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeGgPackA" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="login">
                            <div>
                                <label>Qtde XP - Pack B:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeXpPackB" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde PP - Pack B:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdePpPackB" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde PQ - Pack B:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdePqPackB" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde MD - Pack B:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeMdPackB" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde GD - Pack B:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeGdPackB" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde GG - Pack B:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeGgPackB" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="login">
                            <div>
                                <label>Qtde XP - Pack C:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeXpPackC" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde PP - Pack C:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdePpPackC" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde PQ - Pack C:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdePqPackC" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde MD - Pack C:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeMdPackC" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde GD - Pack C:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeGdPackC" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde GG - Pack C:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeGgPackC" runat="server" CssClass="textEntry" MaxLength="3" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </fieldset>
        <p style="height: 13px">
            &nbsp;</p>
        <div>
            <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="pack_grade"/>
            <asp:ValidationSummary ID="ValidationSummaryPackGrade" runat="server" ValidationGroup="pack_grade" ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewPackGrade" AutoGenerateColumns="false" onrowdatabound="GridViewPackGrade_RowDataBound">
            <Columns>
                <asp:BoundField DataField="CODIGO_PACK_GRADE" HeaderText="Código" ItemStyle-HorizontalAlign="Center"/>
                <asp:TemplateField HeaderText="Grupo" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LiteralGrupo"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DESCRICAO_TIPO" HeaderText="Tipo" ItemStyle-HorizontalAlign="Left"/>
                <asp:TemplateField HeaderText="Griffe" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LiteralGriffe"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="QTDE_XP_PACK_A" HeaderText="Qtde XP Pack A" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_PP_PACK_A" HeaderText="Qtde PP Pack A" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_PQ_PACK_A" HeaderText="Qtde PQ Pack A" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_MD_PACK_A" HeaderText="Qtde MD Pack A" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_GD_PACK_A" HeaderText="Qtde GD Pack A" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_GG_PACK_A" HeaderText="Qtde GG Pack A" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_XP_PACK_B" HeaderText="Qtde XP Pack B" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_PP_PACK_B" HeaderText="Qtde PP Pack B" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_PQ_PACK_B" HeaderText="Qtde PQ Pack B" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_MD_PACK_B" HeaderText="Qtde MD Pack B" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_GD_PACK_B" HeaderText="Qtde GD Pack B" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_GG_PACK_B" HeaderText="Qtde GG Pack B" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_XP_PACK_C" HeaderText="Qtde XP Pack C" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_PP_PACK_C" HeaderText="Qtde PP Pack C" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_PQ_PACK_C" HeaderText="Qtde PQ Pack C" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_MD_PACK_C" HeaderText="Qtde MD Pack C" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_GD_PACK_C" HeaderText="Qtde GD Pack C" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="QTDE_GG_PACK_C" HeaderText="Qtde GG Pack C" ItemStyle-HorizontalAlign="Center"/>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditarPackGrade_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluirPackGrade_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoPackGrade" Value="0" />
</asp:Content>
