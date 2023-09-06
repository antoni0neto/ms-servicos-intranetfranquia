<%@ Page Title="Certificado Digital" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="fisc_cad_cert_digital.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.fisc_cad_cert_digital" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Certificado Digital</span>
                <div style="float: right; padding: 0;">
                    <a href="fisc_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Certificado Digital</legend>
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
                        <asp:HiddenField ID="hidCodigoCertificado" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-certificado" id="tabCertificado" runat="server" onclick="MarcarAba(0);">Certificados</a></li>
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
                        <div id="tabs-certificado">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="labTitular" runat="server" Text="Titular"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labCPF" runat="server" Text="CNPJ/CPF"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtTitular" runat="server" Width="540px" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtCPF" runat="server" Width="370px" MaxLength="14"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="labRespAssinatura" runat="server" Text="Responsável Assinatura"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labDataIni" runat="server" Text="Vigência De"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labDataFim" runat="server" Text="Vigência Até"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtRespAssinatura" runat="server" Width="540px" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td style="width: 160px;">
                                        <asp:TextBox ID="txtDataIni" runat="server" MaxLength="10" Width="180px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDataFim" runat="server" MaxLength="10" Width="180px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labRespCompra" runat="server" Text="Responsável Compra"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCertificado" runat="server" Text="Certificado"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labTipo" runat="server" Text="Tipo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCertificadora" runat="server" Text="Certificadora"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 360px;">
                                        <asp:DropDownList ID="ddlRespCompra" runat="server" Width="354px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="CONTABILIDADE" Text="Contabilidade"></asp:ListItem>
                                            <asp:ListItem Value="FISCAL" Text="Fiscal"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 190px;">
                                        <asp:DropDownList ID="ddlCertificado" runat="server" Width="184px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="E-CNPJ" Text="e-CNPJ"></asp:ListItem>
                                            <asp:ListItem Value="E-CPF" Text="e-CPF"></asp:ListItem>
                                            <asp:ListItem Value="NF-E" Text="NF-e"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 190px;">
                                        <asp:DropDownList ID="ddlTipo" runat="server" Width="184px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="A1" Text="A1"></asp:ListItem>
                                            <asp:ListItem Value="A3" Text="A3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCertificadora" runat="server" Width="184px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="AR PRIME" Text="AR Prime"></asp:ListItem>
                                            <asp:ListItem Value="BOA VISTA" Text="Boa Vista"></asp:ListItem>
                                            <asp:ListItem Value="SERASA EXPERIAN" Text="Serasa Experian"></asp:ListItem>
                                            <asp:ListItem Value="VALID" Text="Valid"></asp:ListItem>
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
                                                    <asp:TemplateField HeaderText="Titular" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTitular" runat="server" Text='<%# Bind("TITULAR") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Certificado" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCertificado" runat="server" Text='<%# Bind("CERTIFICADO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tipo" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTipo" runat="server" Text='<%# Bind("TIPO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Certificadora" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCertificadora" runat="server" Text='<%# Bind("CERTIFICADORA") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="VIGENCIA_ATE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Vigência Até" DataFormatString="{0:dd/MM/yyyy}" />
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
