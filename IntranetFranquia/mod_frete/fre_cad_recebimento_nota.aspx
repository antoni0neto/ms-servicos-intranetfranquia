<%@ Page Title="Cadastro de Recebimento de Nota" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="fre_cad_recebimento_nota.aspx.cs" Inherits="Relatorios.mod_frete.fre_cad_recebimento_nota" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .jGrowl .manilla {
            background-color: #000;
            color: red;
            font-weight: bold;
        }

        .imageButtonEditOrDelete {
            width: 25px !important;
            height: 25px !important;
        }

        .ui-widget input {
            font-family: Verdana,Arial,sans-serif;
            font-size: 1em;
            height: 18px !important;
        }

        .legend {
            font-weight: 600;
            padding: 2px 4px 8px 4px;
            font-weight: 600;
            font-size: 1.1em;
            font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
            color: #696969;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $("#tabs").tabs();

            $('#<%=txtDataRecebimento.ClientID %>').datepicker({
                dateFormat: 'dd/mm/yy'
            });

        });

        function MarcarAba(vAba) {
            document.getElementById("MainContent_hidTabSelected").value = vAba;
        }

        function AtualizaCodigoProduto(e) {
            if (e && e.keyCode == 13) {
                __doPostBack('<%= txtProduto.ClientID %>', '');
            }
        };

    </script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Frete&nbsp;&nbsp;>&nbsp;&nbsp;
                    Controle AWB&nbsp;&nbsp;>&nbsp;&nbsp;Recebimento de Nota</span>
                <div style="float: right; padding: 0;">
                    <a href="fre_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Recebimento de Nota</legend>
                    <%--Botoes de Acao--%>
                    <div style="padding-left: 0px;">
                        <asp:ImageButton ID="ibtNovo" runat="server" Width="18px" ImageUrl="~/Image/add.png"
                            ToolTip="Novo" OnClick="ibtNovo_Click" Visible="true" />
                        <asp:ImageButton ID="ibtSalvar" CommandArgument="I" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                            ToolTip="Salvar" OnClick="ibtSalvar_Click" />
                        <asp:ImageButton ID="ibtCancelar" runat="server" Width="18px" ImageUrl="~/Image/cancel.png"
                            ToolTip="Cancelar" OnClick="ibtCancelar_Click" Visible="False" />
                        <%-- <asp:ImageButton ID="ibtEditar" runat="server" Width="20px" ImageUrl="~/Image/edit.jpg"
                            ToolTip="Editar" OnClick="ibtEditar_Click" />--%>
                        <asp:ImageButton ID="ibtExcluir" runat="server" Width="18px" ImageUrl="~/Image/delete.png"
                            ToolTip="Excluir" OnClick="ibtExcluirNotaFormulario" OnClientClick="return ConfirmarExclusao();" Visible="False" />
                        <asp:ImageButton ID="ibtLimpar" runat="server" Width="18px" ImageUrl="~/Image/clean.png"
                            ToolTip="Limpar" OnClick="ibtLimpar_Click" />
                        <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                            ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <asp:HiddenField ID="hidAcao" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErroGeral" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />

                    <br />
                    <%--Abas exibidas na tela--%>
                    <div id="tabs">
                        <ul>
                            <li>
                                <a href="#tabs-nota" id="tabNota" runat="server" onclick="MarcarAba(0);">Inclusão/Alteração de Nota</a>
                            </li>
                            <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </li>
                            <li>
                                <a href="#tabs-notas-salvas" id="tabItems" runat="server" onclick="MarcarAba(1);">Todos</a>
                            </li>
                        </ul>
                        <div id="tabs-nota">

                            <fieldset>
                                <legend class="legend">Cadastro nova Nota</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr style="height: 10px; vertical-align: bottom;">
                                        <td>
                                            <asp:Label ID="labAWB" runat="server" Text="AWB"></asp:Label>
                                            <asp:HiddenField ID="hidPKNota" runat="server" Value="0" />
                                        </td>
                                        <td>
                                            <asp:Label ID="labDataRecebimento" runat="server" Text="Data de Recebimento"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTipoEnvio" runat="server" Text="Tipo de Envio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTipoPagamento" runat="server" Text="Tipo de Pagamento"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtAWB" MaxLength="12" Height="21px" runat="server" Width="170px" Text="" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtDataRecebimento" MaxLength="10" runat="server" CssClass="textEntry" Height="21px" Width="170px"></asp:TextBox>

                                        </td>
                                        <td style="width: 180px;">
                                            <asp:DropDownList ID="ddlTipoEnvio" DataValueField="CODIGO" DataTextField="DESCRICAO" Height="21px" runat="server" Width="174px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoPagamento" DataValueField="CODIGO" DataTextField="DESCRICAO" Height="21px" runat="server" Width="174px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="labObservacoes" runat="server" Text="Observações"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtObsNota" MaxLength="2000" runat="server" TextMode="MultiLine" Height="60px" Width="880px"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td colspan="4">&nbsp;<asp:Label ID="labErroNota" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>

                            <fieldset>
                                <legend class="legend">Cadastro novo Item</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr style="height: 10px; vertical-align: bottom;">
                                        <td>
                                            <asp:Label ID="labTipoAmostra" runat="server" Text="Tipo de Amostra"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labGrupoProduto" runat="server" Text="Grupo do Produto"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labGriffe" runat="server" Text="Griffe"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labQtdePecas" runat="server" Text="Qtde de Peças"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px;">
                                            <asp:DropDownList ID="ddlTipoAmostra" DataValueField="CODIGO" DataTextField="DESCRICAO" Height="21px" runat="server" Width="174px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:DropDownList runat="server" ID="ddlGrupoProduto" DataValueField="GRUPO_PRODUTO" DataTextField="GRUPO_PRODUTO" Height="21px" Width="174px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:DropDownList runat="server" ID="ddlGriffe" DataValueField="GRIFFE" DataTextField="GRIFFE" Height="21px"
                                                Width="174px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQtdePecas" runat="server"  CssClass="alinharDireita" MaxLength="8" Height="21px" Width="120px" Text="" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labValorUnitario" runat="server" Text="Valor Unitário ($)"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="labProduto" runat="server" Text="Produto"></asp:Label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtValorUnitario" MaxLength="8" CssClass="alinharDireita" runat="server" Width="170px" Text="" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatus" Height="21px" runat="server" Width="174px">
                                                <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Ativo" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Inativo" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtProduto" runat="server" MaxLength="5" Width="120px" CssClass="alinharCentro"
                                                onkeypress="return fnValidarNumero(event); AtualizaCodigoProduto(event);" OnTextChanged="txtProduto_TextChanged"
                                                AutoPostBack="true">
                                            </asp:TextBox>
                                            <asp:Label ID="labDescricaoProduto" runat="server"></asp:Label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td colspan="4">Observações
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtObsItem" MaxLength="2000" runat="server" TextMode="MultiLine" Height="60px" Width="880px"></asp:TextBox>

                                            <asp:ImageButton ID="btSalvarItem" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                                                ToolTip="Salvar" OnClick="btSalvarAtualizarItem_Click" />
                                            <asp:ImageButton ID="btCancelarItem" runat="server" Width="18px" ImageUrl="~/Image/cancel.png"
                                                ToolTip="Cancelar" OnClick="btCancelarItem_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">&nbsp;<asp:Label ID="labErroItem" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                            <asp:HiddenField ID="hidKeyItem" runat="server" Value="0" />
                                        </td>
                                    </tr>

                                </table>

                                <fieldset>
                                    <legend class="legend">Items Cadastrados</legend>

                                    <div class="rounded_corners">
                                        <%--Grid com Items da Nota--%>
                                        <asp:GridView ID="gvFreteRecebimentoItem" runat="server"
                                            Width="100%"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            AllowSorting="true"
                                            EnablePaging="true"
                                            GridLines="None"
                                            Style="background: white"
                                            ForeColor="#333333"
                                            ShowFooter="true"
                                            DataKeyNames="CODIGO">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" ImageUrl="~/Image/edit.jpg" Text="Editar" CommandName="Select" />

                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="CODIGO" Visible="false" />

                                                <asp:BoundField DataField="PRODUTO" HeaderText="Produto" />

                                                <asp:BoundField DataField="FRETE_TIPO_AMOSTRA" HeaderText="Tipo Amostra" Visible="false" />

                                                <asp:TemplateField HeaderText="Tipo de Amostra" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("FRETE_TIPO_AMOSTRA1.DESCRICAO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="GRUPO_PRODUTO" HeaderText="Grupo do Produto" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />

                                                <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="VALOR_UNITARIO" HeaderText="Valor Unitário" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:c}" />
                                                <asp:BoundField DataField="QTDE_PECAS" HeaderText="Qtde de Peças" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />

                                                <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litStatus" Text='<%#Eval("STATUS").ToString().Equals("1") ? "ATIVO": "INATIVO" %>' runat="server"></asp:Literal>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btExcluirItem" runat="server" OnClick="btExcluirItem_Click" ImageUrl="~/Image/delete.png" Text="Excluir" OnClientClick="return ConfirmarExclusao();" CommandArgument='<%#Eval("CODIGO") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label runat="server" Text="Nenhum registro encontrado."></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </fieldset>

                            </fieldset>

                        </div>

                        <div id="tabs-notas-salvas">
                            <div class="rounded_corners">
                                <%--Grid com listagem de Notas--%>
                                <asp:GridView ID="gvFreteRecebimentoNota" runat="server"
                                    Width="100%"
                                    AutoGenerateColumns="false"
                                    AllowPaging="true"
                                    AllowSorting="true"
                                    EnablePaging="true"
                                    GridLines="None"
                                    Style="background: white"
                                    ForeColor="#333333"
                                    ShowFooter="true"
                                    DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" Text='<%# (Container.DataItemIndex + 1) %>' runat="server"></asp:Literal>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Image/edit.jpg" Text="Editar" ItemStyle-Height="15px" ItemStyle-Width="15px" CommandName="Select" />
                                        
                                        <asp:BoundField DataField="CODIGO" HeaderText="Código" ItemStyle-Width="30px" Visible="false" />
                                        <asp:BoundField DataField="AWB" HeaderText="AWB" />
                                        <asp:BoundField DataField="DATA_RECEBIMENTO" HeaderText="Data de Recebimento" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="FRETE_TIPO_ENVIO" HeaderText="Tipo de Envio" Visible="false" />

                                        <asp:TemplateField HeaderText="Tipo de Envio">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("FRETE_TIPO_ENVIO1.DESCRICAO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="FRETE_TIPO_PAGAMENTO" HeaderText="Tipo de Pagamento" Visible="false" />

                                        <asp:TemplateField HeaderText="Tipo de Pagamento">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("FRETE_TIPO_PAGAMENTO1.DESCRICAO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btExcluirNotaGridView" runat="server" OnClick="btExcluirNotaGridView_Click" ImageUrl="~/Image/delete.png" Text="Excluir" Height="15px" Width="15px" OnClientClick="return ConfirmarExclusao();" CommandArgument='<%#Eval("CODIGO") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
