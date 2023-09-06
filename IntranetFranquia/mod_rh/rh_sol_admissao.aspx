<%@ Page Title="Admissão" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="rh_sol_admissao.aspx.cs" Inherits="Relatorios.rh_sol_admissao"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            color: black;
        }

        .jGrowl .redError {
            color: red;
        }

        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Solicitações&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Admissão"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 953px; margin-left: 16%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Admissão"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <asp:Literal ID="litNumeroSolicitacao" runat="server" Text="" Visible="false"></asp:Literal>
                            </td>
                            <td colspan="1" style="text-align: right;">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labFilialAtual" runat="server" Text="Filial"></asp:Label>
                                <asp:HiddenField ID="hidCodigoWF" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="labFuncionario" runat="server" Text="Funcionário"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCPF" runat="server" Text="CPF"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 240px;">
                                <asp:DropDownList runat="server" ID="ddlFilialAtual" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="234px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 400px;">
                                <asp:TextBox ID="txtFuncionario" runat="server" Width="390px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCPF" runat="server" Width="280px" MaxLength="11" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labInicioPrevisto" runat="server" Text="Início Previsto"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCargo" runat="server" Text="Cargo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labTelefone" runat="server" Text="Telefone"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labRG" runat="server" Text="RG"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labSexo" runat="server" Text="Sexo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 240px;">
                                <asp:TextBox ID="txtDataIniPrevisto" runat="server" Width="230px" MaxLength="10" Style="text-align: right;"
                                    onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>
                            <td style="width: 240px;">
                                <asp:TextBox ID="txtCargo" runat="server" Width="230px" MaxLength="40"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtTelefone" runat="server" Width="150px" Style="text-align: right;"
                                    onkeypress="return fnValidarNumero(event);" MaxLength="12"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtRG" runat="server" Width="150px" MaxLength="19"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSexo" runat="server" Width="124px" Height="21px">
                                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                    <asp:ListItem Value="M" Text="Masculino"></asp:ListItem>
                                    <asp:ListItem Value="F" Text="Feminino"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labPeriodoTrabalho" runat="server" Text="Horário de Trabalho"></asp:Label>
                            </td>
                            <td colspan="4" style="text-align: right">
                                <asp:Literal ID="litExemploHorarioTrab" runat="server" Text="<font size='1'>Exemplo: SEG-SEX 09:30-17:30, SAB 10:00-20:00, DOM 12:00-22:00</font>"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="txtPeriodoTrabalho" runat="server" Width="919px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="labObs" runat="server" Text="Observação"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="txtObs" runat="server" TextMode="MultiLine" Height="40px" Width="919px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Panel ID="pnlCriarFuncionario" runat="server" Visible="false">
                                    <fieldset>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labVendedorApelido" runat="server" Text="Apelido Vendedor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labPeriodoExp" runat="server" Text="Período de Experiência"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labComissao" runat="server" Text="Comissão"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labCidade" runat="server" Text="Cidade"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labUF" runat="server" Text="UF"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 230px;">
                                                    <asp:TextBox ID="txtVendedorApelido" runat="server" Width="220px" MaxLength="40"></asp:TextBox>
                                                </td>
                                                <td style="width: 230px;">
                                                    <asp:DropDownList ID="ddlPeriodoExp" runat="server" Width="224px" Height="21px" DataTextField="DESCRICAO"
                                                        DataValueField="CODIGO">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtComissao" runat="server" Width="150px" Style="text-align: right;"
                                                        onkeypress="return fnValidarNumeroDecimal(event);" MaxLength="10"></asp:TextBox>
                                                </td>
                                                <td style="width: 190px;">
                                                    <asp:TextBox ID="txtCidade" runat="server" Width="180px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUF" runat="server" Width="82px" MaxLength="2"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="labCargoFun" runat="server" Text="Cargo"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:DropDownList ID="ddlCargo" runat="server" Width="224px" DataTextField="DESC_CARGO" Height="22px"
                                                        DataValueField="DESC_CARGO">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">&nbsp;
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
                                                            <asp:Label ID="labPeriodoTrabalhoTitulo" runat="server" Text="Período de Trabalho"></asp:Label></legend>
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvPeriodoTrabalho" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvPeriodoTrabalho_RowDataBound"
                                                                OnDataBound="gvPeriodoTrabalho_DataBound" DataKeyNames="CODIGO" ShowFooter="true">
                                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
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
                                    </fieldset>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                        <asp:Panel ID="pnlStatus" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="labMotivo" runat="server" Text="Motivo"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 240px;">
                                    <asp:DropDownList runat="server" ID="ddlStatus" Height="21px" Width="234px" DataTextField="DESCRICAO" DataValueField="CODIGO"
                                        OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtMotivo" runat="server" Width="682px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                                <td style="text-align: right; line-height: 40px; width: 300px;">
                                    <asp:CheckBox ID="cbCriarFunc" runat="server" Text="Criar Funcionário" Checked="false" Visible="false" OnCheckedChanged="cbCriarFunc_CheckedChanged" AutoPostBack="true" />
                                    &nbsp;&nbsp;<asp:Button ID="btEnviar" runat="server" Text="Enviar" Width="110px" OnClick="btEnviar_Click"
                                        OnClientClick="DesabilitarBotao(this);" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="labHistoricoTitulo" runat="server" Visible="false" Text="Histórico da Solicitação" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvStatusHistorico" runat="server" Visible="false" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvStatusHistorico_RowDataBound">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                        <FooterStyle BackColor="Gainsboro" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="145px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Motivo" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litMotivo" runat="server"></asp:Literal>
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
            <div id="dialogPai" runat="server">
                <div id="dialog" title="Mensagem" class="divPop">
                    <table border="0" width="100%">
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <strong>Solicitação:&nbsp;&nbsp;<asp:Label ID="labPopUp" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Label ID="labMensagem" runat="server" Text="ENVIADA COM SUCESSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>Por favor, selecione a tela desejada nos botões abaixo.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
