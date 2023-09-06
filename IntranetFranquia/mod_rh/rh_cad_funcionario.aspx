<%@ Page Title="Cadastro de Funcionário" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="rh_cad_funcionario.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.mod_desenvolvimento.rh_cad_funcionario" %>

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

        .imgGray {
            -webkit-filter: grayscale(100%);
            filter: grayscale(100%);
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Funcionário</span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Cadastro de Funcionário</legend>
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
                        <asp:ImageButton ID="ibtExcluir" runat="server" Width="18px" ImageUrl="~/Image/delete.png" Visible="false"
                            ToolTip="Excluir" OnClick="ibtExcluir_Click" OnClientClick="return ConfirmarExclusao();" />
                        <asp:ImageButton ID="ibtLimpar" runat="server" Width="18px" ImageUrl="~/Image/clean.png"
                            ToolTip="Limpar" OnClick="ibtLimpar_Click" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <asp:HiddenField ID="hidAcao" runat="server" Value="" />
                        <asp:HiddenField ID="hidCodigoFuncionario" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                        <tr>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labNome" runat="server" Text="Nome"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCPF" runat="server" Text="CPF"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCodigoVendedor" runat="server" Text="Código" Enabled="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 256px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 360px;">
                                <asp:TextBox ID="txtNome" runat="server" Width="350px"></asp:TextBox>
                            </td>
                            <td style="width: 220px;">
                                <asp:TextBox ID="txtCPF" runat="server" Width="210px" MaxLength="11" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVendedorCodigo" runat="server" Enabled="false" Width="130px" MaxLength="4"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:DropDownList ID="ddlStatus" runat="server" Height="22px" Width="250px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="A" Text="Ativo" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="I" Text="Inativo"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-funcionarios" id="tabFuncionario" runat="server" onclick="MarcarAba(0);">Funcionário</a></li>
                            <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </li>
                            <li><a href="#tabs-todos" id="tabTodos" runat="server" onclick="MarcarAba(1);">Todos</a></li>
                        </ul>
                        <div id="tabs-funcionarios">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td rowspan="7" style="text-align: center;">
                                        <asp:ImageButton ID="imgFoto" runat="server" AlternateText=" " Width="145px" BorderWidth="0px" ImageUrl="~/Image/fun_sfoto.jpg" OnClick="imgFoto_Click" /><br />
                                        <asp:ImageButton ID="imgFotoAtualizar" runat="server" AlternateText="Atualizar" ToolTip="Atualizar" Width="15px" BorderWidth="0px" ImageUrl="~/Image/update.png" OnClick="imgFotoAtualizar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="labVendedorApelido" runat="server" Text="Apelido Vendedor"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="labCargo" runat="server" Text="Cargo"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="labComissao" runat="server" Text="Comissão"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="labRG" runat="server" Text="RG"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="labSexo" runat="server" Text="Sexo"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" style="width: 180px;">
                                        <asp:TextBox ID="txtVendedorApelido" runat="server" Width="170px" MaxLength="40"></asp:TextBox>
                                    </td>
                                    <td valign="top" style="width: 180px;">
                                        <asp:DropDownList ID="ddlCargo" runat="server" Width="174px" Height="23px" AutoPostBack="true" DataTextField="DESC_CARGO"
                                            DataValueField="DESC_CARGO" OnSelectedIndexChanged="ddlCargo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="top" style="width: 140px;">
                                        <asp:TextBox ID="txtComissao" runat="server" Width="130px" Style="text-align: right;"
                                            onkeypress="return fnValidarNumeroDecimal(event);" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td valign="top" style="width: 145px;">
                                        <asp:TextBox ID="txtRG" runat="server" Width="135px" MaxLength="19"></asp:TextBox>
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlSexo" runat="server" Width="115px" Height="23px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="M" Text="Masculino"></asp:ListItem>
                                            <asp:ListItem Value="F" Text="Feminino"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="labPeriodoExp" runat="server" Text="Período de Experiência"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="labDataAdmissao" runat="server" Text="Data Ativação"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="labDataDemissao" runat="server" Text="Data Desativação"></asp:Label>
                                    </td>
                                    <td valign="top" colspan="2">
                                        <asp:Label ID="labTelefone" runat="server" Text="Telefone"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlPeriodoExp" runat="server" Width="174px" Height="23px" DataTextField="DESCRICAO"
                                            DataValueField="CODIGO">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtDataAdmissao" runat="server" Width="170px" MaxLength="10" Style="text-align: right;"
                                            onkeypress="return fnValidarData(event);"></asp:TextBox>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtDataDemissao" runat="server" Width="130px" MaxLength="10" Style="text-align: right;"
                                            onkeypress="return fnValidarData(event);"></asp:TextBox>
                                    </td>
                                    <td valign="top" colspan="2">
                                        <asp:TextBox ID="txtTelefone" runat="server" Width="257px" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
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
                                    <td style="width: 350px;">
                                        <asp:TextBox ID="txtEndereco" runat="server" Width="340px" MaxLength="90"></asp:TextBox>
                                    </td>
                                    <td style="width: 130px;">
                                        <asp:TextBox ID="txtComplemento" runat="server" Width="120px" MaxLength="40"></asp:TextBox>
                                    </td>
                                    <td style="width: 190px;">
                                        <asp:TextBox ID="txtCidade" runat="server" Width="180px"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtUF" runat="server" Width="80px" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCEP" runat="server" Width="155px" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="labBatePonto" runat="server" Text="Bate Ponto"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="labOperaCaixa" runat="server" Text="Operar Caixa"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="labAcessoGerencial" runat="server" Text="Acesso Gerencial"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="labGerenteLoja" runat="server" Text="Pode ser Gerente da Loja"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px; text-align: center;">
                                        <asp:CheckBox ID="cbBatePonto" runat="server" Checked="false" />
                                    </td>
                                    <td style="width: 100px; text-align: center;">
                                        <asp:CheckBox ID="cbOperaCaixa" runat="server" Checked="false" />
                                    </td>
                                    <td style="width: 100px; text-align: center;">
                                        <asp:CheckBox ID="cbAcessoGerencial" runat="server" Checked="false" />
                                    </td>
                                    <td style="width: 100px; text-align: center;">
                                        <asp:CheckBox ID="cbGerenteLoja" runat="server" Checked="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:HiddenField ID="hidDescricaoPeriodo" runat="server" />
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="labPeriodoTrabalho" runat="server" Text="Período de Trabalho"></asp:Label></legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvPeriodoTrabalho" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPeriodoTrabalho_RowDataBound"
                                                    OnDataBound="gvPeriodoTrabalho_DataBound" DataKeyNames="CODIGO" ShowFooter="true">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btEditarPeriodo" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/edit.jpg"
                                                                    OnClick="btEditarPeriodo_Click" ToolTip="Editar" />
                                                                <asp:ImageButton ID="btSalvarPeriodo" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                                                                    ToolTip="Salvar" OnClick="btSalvarPeriodo_Click" />
                                                                <asp:ImageButton ID="btSairPeriodo" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/cancel.png"
                                                                    OnClick="btSairPeriodo_Click" ToolTip="Sair" />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:ImageButton ID="btIncluirPeriodo" runat="server" Height="18px" Width="18px" ImageUrl="~/Image/add.png"
                                                                    OnClick="btIncluirPeriodo_Click" ToolTip="Incluir" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Período" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labPeriodo" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlPeriodo" runat="server" Width="760px" Height="21px" DataTextField="DESCRICAO"
                                                                    DataValueField="CODIGO">
                                                                </asp:DropDownList>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlPeriodoFooter" runat="server" Width="760px" Height="21px" DataTextField="DESCRICAO"
                                                                    DataValueField="CODIGO">
                                                                </asp:DropDownList>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="25px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btExcluirPeriodo" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/delete.png"
                                                                    OnClick="btExcluirPeriodo_Click" OnClientClick="return ConfirmarExclusao();" ToolTip="Excluir" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="tabs-todos">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
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
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="230px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labFilial" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nome" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CPF" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labCPF" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CARGO" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labCARGO" runat="server"></asp:Label>
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
