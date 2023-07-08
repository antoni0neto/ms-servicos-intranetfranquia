<%@ Page Title="Relacionados - Consulta de Configurações" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ecom_rel_config_con.aspx.cs" Inherits="Relatorios.ecom_rel_config_con"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Image/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>

    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Produtos Relacionados&nbsp;&nbsp;>&nbsp;&nbsp;Consulta de Configurações</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Consulta de Configurações"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Griffe</td>
                        <td>Grupo Produto</td>
                        <td>Nome
                        </td>
                        <td>Tipo
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px">
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 200px">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="194px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 300px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="290px" MaxLength="100" />
                        </td>
                        <td style="width: 200px;">
                            <asp:DropDownList ID="ddlTipo" runat="server" Width="194px">
                                <asp:ListItem Value="0" Text=""></asp:ListItem>
                                <asp:ListItem Value="1" Text="Regra Entrada"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Regra Saída"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Width="100px" Text="Buscar" OnClick="btBuscar_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="rounded_corners">

                                <asp:GridView ID="gvConfig" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvConfig_RowDataBound"
                                    OnDataBound="gvConfig_DataBound" OnSorting="gvConfig_Sorting" AllowSorting="true" ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                    Width="15px" runat="server" />
                                                <asp:Panel ID="pnlCampos" runat="server" Style="display: none" Width="100%">

                                                    <asp:GridView ID="gvCampos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvCampos_RowDataBound"
                                                        OnDataBound="gvCampos_DataBound" ShowFooter="true"
                                                        DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Campo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="172px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCampo" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Operação" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="172px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litOperacao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Valores" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:ListBox ID="lboxValores" runat="server" SelectionMode="Multiple" Width="600px"
                                                                        Height="80px"></asp:ListBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="NOME" HeaderText="Nome" SortExpression="NOME" />
                                        <asp:BoundField DataField="TIPO" HeaderText="Tipo" SortExpression="TIPO" />
                                        <asp:BoundField DataField="DATA" HeaderText="Data" SortExpression="DATA" />

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="btAlterar" runat="server" Text="Alterar" Width="65px" OnClick="btAlterar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="btExcluir" runat="server" Text="Excluir" Width="65px" OnClick="btExcluir_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
