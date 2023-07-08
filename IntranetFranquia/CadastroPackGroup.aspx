<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CadastroPackGroup.aspx.cs" Inherits="Relatorios.CadastroPackGroup" %>

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
            <legend>Cadastro de Pack Group</legend>
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
                                        Display="None" ErrorMessage="Preencha a descrição do tipo" ControlToValidate="txtDescricaoTipo" ValidationGroup="pack_group"></asp:RequiredFieldValidator>
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
                                <label>Qtde Total:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdeTotal" runat="server" CssClass="textEntry" MaxLength="5" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde Pack A:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdePackA" runat="server" CssClass="textEntry" MaxLength="5" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde Pack B:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdePackB" runat="server" CssClass="textEntry" MaxLength="5" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                            <div>
                                <label>Qtde Pack C:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtQtdePackC" runat="server" CssClass="textEntry" MaxLength="5" Height="22px" Width="100px"></asp:TextBox>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </fieldset>
        <p style="height: 13px">
            &nbsp;</p>
        <div>
            <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="pack_group"/>
            <asp:ValidationSummary ID="ValidationSummaryPackGroup" runat="server" ValidationGroup="pack_group" ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewPackGroup" AutoGenerateColumns="false" onrowdatabound="GridViewPackGroup_RowDataBound">
            <Columns>
                <asp:BoundField DataField="CODIGO_PACK_GROUP" HeaderText="Código" ItemStyle-HorizontalAlign="Center"/>
                <asp:TemplateField HeaderText="Grupo"  ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LiteralGrupo"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DESCRICAO_TIPO" HeaderText="Tipo" ItemStyle-HorizontalAlign="Left"/>
                <asp:TemplateField HeaderText="Griffe" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LiteralGriffe" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="QTDE_TOTAL" HeaderText="Qtde Total" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="QTDE_PACK_A" HeaderText="Qtde Pack A" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="QTDE_PACK_B" HeaderText="Qtde Pack B" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="QTDE_PACK_C" HeaderText="Qtde Pack C" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditarPackGroup_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluirPackGroup_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoPackGroup" Value="0" />
</asp:Content>
