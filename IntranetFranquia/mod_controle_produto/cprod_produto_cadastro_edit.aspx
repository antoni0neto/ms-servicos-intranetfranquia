<%@ Page Title="Enviar Cadastro de Produto" Language="C#" AutoEventWireup="true" CodeBehind="cprod_produto_cadastro_edit.aspx.cs"
    Inherits="Relatorios.cprod_produto_cadastro_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Enviar Cadastro de Produto</title>
    <style type="text/css">
        .fs {
            border: 1px solid #ccc;
            font-family: Calibri;
        }

        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Controle de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Enviar Cadastro de Produto</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Enviar Cadastro de Produto</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 180px;">Coleção:
                                        <asp:HiddenField ID="hidColecao" runat="server" />
                                        <asp:HiddenField ID="hidProduto" runat="server" />
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="labColecao" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Produto:
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="labProduto" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Grupo:
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="labGrupo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Griffe:
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="labGriffe" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Cor:
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="labCor" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Modelagem:
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="labModelagem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tecido:
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="labTecido" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Signed/Estilista:
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="labSigned" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labNome" runat="server" Text="Nome"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labSubGrupo" runat="server" Text="SubGrupo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labLinha" runat="server" Text="Linha"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labGrade" runat="server" Text="Grade"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtNome" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                    <td style="width: 300px;">
                                        <asp:DropDownList ID="ddlSubGrupo" runat="server" Width="294px" Height="22px" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="Selecione" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="PLANO" Text="PLANO"></asp:ListItem>
                                            <asp:ListItem Value="MALHA" Text="MALHA"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 300px;">
                                        <asp:DropDownList ID="ddlLinha" runat="server" Width="294px" DataValueField="LINHA" DataTextField="LINHA" Height="22px"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlGrade" runat="server" Width="348px" DataValueField="GRADE" DataTextField="GRADE" Height="22px"></asp:DropDownList>
                                    </td>
                                </tr>
                                <%--                                <tr>
                                    <td>
                                        <asp:Label ID="labTecidoGrupo" runat="server" Text="Grupo Tecido"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labTecidoSubGrupo" runat="server" Text="SubGrupo Tecido"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labNCM" runat="server" Text="NCM"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labNCMOutro" runat="server" Text="NCM Outro"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="194px" Height="21px"
                                            DataTextField="GRUPO" DataValueField="GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hidMaterial" runat="server" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="294px" Height="21px"
                                            DataTextField="SUBGRUPO" DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNCM" runat="server" Width="294px" Height="21px"
                                            DataTextField="CLASSIF_FISCAL" DataValueField="CLASSIF_FISCAL">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNCM" runat="server" Width="344px" MaxLength="8" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div>
                                            <fieldset>
                                                <legend>Composição</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvComposicao" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                        Style="background: white">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                        <RowStyle HorizontalAlign="Left" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Quantidade" HeaderStyle-Width="190px" />
                                                            <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Descrição" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td colspan="4">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: right;">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>&nbsp;&nbsp;<asp:Button ID="btSalvar" runat="server" Width="122px" Text="Enviar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />
                                    </td>
                                </tr>
                                <%/*
                                <tr>
                                    <td colspan="4">
                                        <div>
                                            <fieldset>
                                                <legend>Lavagem</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvLavagem" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvLavagem_RowDataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                                        <FooterStyle BackColor="GradientActiveCaption" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgLavagem" Width="30px" Height="27px" runat="server"></asp:Image>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </td>
                                </tr>
                                    */ %>
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;                                        
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

