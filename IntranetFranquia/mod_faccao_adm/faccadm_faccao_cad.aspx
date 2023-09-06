<%@ Page Title="Cadastro de Facção" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="faccadm_faccao_cad.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.mod_desenvolvimento.faccadm_faccao_cad" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .bodyFaccao {
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

        .alinharDireira {
            text-align: right;
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção Administrativo&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Facção</span>
                <div style="float: right; padding: 0;">
                    <a href="faccadm_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Cadastro de Facção</legend>
                    <div style="padding-left: 0px;">
                        <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                            ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" />
                        <asp:ImageButton ID="ibtSalvar" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                            ToolTip="Salvar" OnClick="ibtSalvar_Click" />
                        <asp:ImageButton ID="ibtCancelar" runat="server" Width="18px" ImageUrl="~/Image/cancel.png"
                            ToolTip="Cancelar" OnClick="ibtCancelar_Click" />
                        <asp:ImageButton ID="ibtEditar" runat="server" Width="20px" ImageUrl="~/Image/edit.jpg"
                            ToolTip="Editar" OnClick="ibtEditar_Click" />
                        <asp:ImageButton ID="ibtLimpar" runat="server" Width="18px" ImageUrl="~/Image/clean.png"
                            ToolTip="Limpar" OnClick="ibtLimpar_Click" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <asp:HiddenField ID="hidAcao" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyFaccao">
                        <tr>
                            <td>
                                <asp:Label ID="labFaccao" runat="server" Text="Facção"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtFaccao" runat="server" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-faccao" id="tabFaccao" runat="server" onclick="MarcarAba(0);">Facção</a></li>
                            <li><a href="#tabs-socio" id="tabSocio" runat="server" onclick="MarcarAba(1);">Sócios</a></li>
                            <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;
                            </li>
                            <li><a href="#tabs-todos" id="tabTodos" runat="server" onclick="MarcarAba(2);">Todos</a></li>
                        </ul>
                        <div id="tabs-faccao">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyFaccao">
                                <tr>
                                    <td>Razão Social
                                    </td>
                                    <td>CNPJ / CPF
                                    </td>
                                    <td>Organização Societária
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 440px;">
                                        <asp:TextBox ID="txtRazaoSocial" runat="server" Width="430px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 240px;">
                                        <asp:TextBox ID="txtCNPJCPF" runat="server" Width="230px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOrgSocietaria" runat="server" Width="240px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Inscrição Estadual
                                    </td>
                                    <td>Qtde Funcionários
                                    </td>
                                    <td>E-Mail
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 440px;">
                                        <asp:TextBox ID="txtInscricaoEstadual" runat="server" Width="430px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 240px;">
                                        <asp:TextBox ID="txtQtdeFuncionario" runat="server" CssClass="alinharDireira" Width="230px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" runat="server" Width="240px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyFaccao">
                                <tr>
                                    <td>
                                        <asp:Label ID="labEndereco" runat="server" Text="Endereço"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labComplemento" runat="server" Text="Complemento"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCidade" runat="server" Text="Cidade"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labUF" runat="server" Text="UF"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCEP" runat="server" Text="CEP"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 320px;">
                                        <asp:TextBox ID="txtEndereco" runat="server" Width="310px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtComplemento" runat="server" Width="110px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 240px;">
                                        <asp:TextBox ID="txtCidade" runat="server" Width="230px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtUF" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCEP" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyFaccao">
                                <tr>
                                    <td>
                                        <div>
                                            <fieldset>
                                                <legend>
                                                    <asp:Label ID="labTipoDocumento" runat="server" Text="Tipos de Documento"></asp:Label></legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvTipoDocumento" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvTipoDocumento_RowDataBound"
                                                        ShowFooter="true" DataKeyNames="COD_FACC_TIPO_DOCUMENTO">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tipo de Documento" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labTipoDocumento" runat="server" Text='<%# Bind("TIPO_DOCUMENTO_DESC") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Obrigatório" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="170px">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cbObrigatorio" runat="server" Checked="false" OnCheckedChanged="cbObrigatorio_CheckedChanged" AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="tabs-socio">
                            <div class="bodyFaccao">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labSocioNome" runat="server" Text="Nome do Sócio"></asp:Label>
                                                        <asp:HiddenField ID="hidCodigoSocio" runat="server" Value="0" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labSocioCPF" runat="server" Text="CPF"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labSocioTelefone" runat="server" Text="Telefone"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labSocioEmail" runat="server" Text="E-Mail"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 325px;">
                                                        <asp:TextBox ID="txtSocioNome" runat="server" Width="315px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 170px;">
                                                        <asp:TextBox ID="txtSocioCPF" runat="server" Width="160px" MaxLength="11"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 170px;">
                                                        <asp:TextBox ID="txtSocioTelefone" runat="server" Width="160px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSocioEmail" runat="server" Width="230px"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;<asp:ImageButton ID="btSocioIncluir" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                                                        ToolTip="Incluir" OnClick="btSocioIncluir_Click" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvSocio" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvSocio_RowDataBound">
                                                    <HeaderStyle BackColor="Gainsboro" />
                                                    <FooterStyle BackColor="Gainsboro" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btSocioEditar" runat="server" Height="15px" ImageUrl="~/Image/edit.jpg"
                                                                    OnClick="btSocioEditar_Click"
                                                                    ToolTip="Editar" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nome" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CPF" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCPF" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Telefone" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litTelefone" runat="server" Text='<%# Bind("TELEFONE") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="E-Mail" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litEmail" runat="server" Text='<%# Bind("EMAIL") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btSocioExcluir" runat="server" Height="15px" ImageUrl="~/Image/delete.png"
                                                                    OnClientClick="return ConfirmarExclusao();" OnClick="btSocioExcluir_Click"
                                                                    ToolTip="Excluir" />
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
                        <div id="tabs-todos">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyFaccao">
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvTodos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvTodos_RowDataBound"
                                                DataKeyNames="NOME_CLIFOR">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex  + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Facção" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labNomeCliFor" runat="server" Text='<%# Bind("NOME_CLIFOR") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CNPJ/CPF" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labCGCCPF" runat="server"></asp:Label>
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
