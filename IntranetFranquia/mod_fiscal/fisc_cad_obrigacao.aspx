<%@ Page Title="Obrigação Fiscal" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="fisc_cad_obrigacao.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.fisc_cad_obrigacao" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .bodyMaterial {
            font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif !important;
            margin: 0px;
            padding: 0px;
            color: #696969;
        }

        .jGrowl .manilla {
            background-color: #000;
            color: red;
            font-weight: bold;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#tabs").tabs();
        });

        function MarcarAba(vAba) {
            document.getElementById("MainContent_hidTabSelected").value = vAba;
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Obrigação Fiscal</span>
                <div style="float: right; padding: 0;">
                    <a href="fisc_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Obrigação Fiscal</legend>
                    <div style="padding-left: 0px;">
                        <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                            ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" />
                        <asp:ImageButton ID="ibtSalvar" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                            ToolTip="Salvar" OnClick="ibtSalvar_Click" />
                        <asp:ImageButton ID="ibtCancelar" runat="server" Width="18px" ImageUrl="~/Image/cancel.png"
                            ToolTip="Cancelar" OnClick="ibtCancelar_Click" />
                        <asp:ImageButton ID="ibtIncluir" runat="server" Width="20px" ImageUrl="~/Image/add.png"
                            ToolTip="Incluir" OnClick="ibtIncluir_Click" Visible="true" />
                        <asp:ImageButton ID="ibtEditar" runat="server" Width="20px" ImageUrl="~/Image/edit.jpg"
                            ToolTip="Editar" OnClick="ibtEditar_Click" />
                        <asp:ImageButton ID="ibtExcluir" runat="server" Width="18px" ImageUrl="~/Image/delete.png"
                            ToolTip="Excluir" OnClick="ibtExcluir_Click" OnClientClick="return ConfirmarExclusao();" />
                        <asp:ImageButton ID="ibtLimpar" runat="server" Width="18px" ImageUrl="~/Image/clean.png"
                            ToolTip="Limpar" OnClick="ibtLimpar_Click" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <asp:HiddenField ID="hidAcao" runat="server" Value="" />
                        <asp:HiddenField ID="hidCodigoObrigacao" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-obrigacao" id="tabObrigacao" runat="server" onclick="MarcarAba(0);">Obrigação</a></li>
                            <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </li>
                            <li><a href="#tabs-todos" id="tabTodos" runat="server" onclick="MarcarAba(1);">Todos</a></li>
                        </ul>
                        <div id="tabs-obrigacao">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="labObrigacao" runat="server" Text="Descrição"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labEsfera" runat="server" Text="Esfera"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labRegimeTributacao" runat="server" Text="Regime Tributação"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtObrigacao" runat="server" Width="380px" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEsfera" runat="server" DataTextField="DESCRICAO" DataValueField="CODIGO" Width="264px" Height="23px"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRegimeTributacao" runat="server" Width="264px" Height="23px"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labPeriodo" runat="server" Text="Período"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labDiaUtil" runat="server" Text="Dia Útil"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labDataIni" runat="server" Text="Data Inicial"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labDataFim" runat="server" Text="Data Final"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 250px;">
                                        <asp:DropDownList ID="ddlPeriodo" runat="server" Width="244px" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="M" Text="MENSAL"></asp:ListItem>
                                            <asp:ListItem Value="A" Text="ANUAL"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 140px;">
                                        <asp:DropDownList ID="ddlDiaUtil" runat="server" Width="134px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="S" Text="SIM"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="NÃO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 270px;">
                                        <asp:TextBox ID="txtDataIni" runat="server" MaxLength="10" Width="260px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDataFim" runat="server" MaxLength="10" Width="260px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labUF" runat="server" Text="UF"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labUtilizaValor" runat="server" Text="Utiliza Valor"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div style="position: relative; float: left;">
                                            <asp:Label ID="labInsEstadual" runat="server" Text="Inscrição Estadual"></asp:Label>
                                        </div>
                                        <div style="text-align: right; float: right;">
                                            <asp:Literal ID="litDiaInsEstadual" runat="server" Text="<font size='1'>Informar os finais das Inscrições Estaduais separadas por <font color='red'>vírgula</font>.</font>"></asp:Literal>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlUF" runat="server" DataTextField="" DataValueField="" Width="244px"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUtilizaValor" runat="server" Width="134px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="S" Text="SIM"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="NÃO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtInsEstadual" runat="server" Width="530px" MaxLength="15" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="labFundamentacao" runat="server" Text="Fundamentação"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtFundamentacao" runat="server" Width="920px" MaxLength="400"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="labObservacao" runat="server" Text="Observação"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtObservacao" runat="server" Width="920px" MaxLength="400"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="244px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="A" Text="Ativo"></asp:ListItem>
                                            <asp:ListItem Value="I" Text="Inativo"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="tabs-todos">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="tableC">
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvTodos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvTodos_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Obrigação" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litObrigacao" runat="server" Text='<%# Bind("DESCRICAO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Esfera" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litEsfera" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Período" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPeriodo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dia" HeaderStyle-Width="148px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDia" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="UF" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litUF" runat="server" Text='<%# Bind("UF") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="25px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btPesquisar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/search.png"
                                                                OnClick="btPesquisarTodos_Click" ToolTip="Pesquisar" />
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
                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
