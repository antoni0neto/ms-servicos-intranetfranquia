<%@ Page Title="Novo Pedido" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="desenv_pedido_cad.aspx.cs" Inherits="Relatorios.desenv_pedido_cad"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla
        {
            color: black;
        }
        .jGrowl .redError
        {
            color: red;
        }
        .alinharDireita
        {
            text-align: right;
        }
        #drop_zone
        {
            margin: 10px 0;
            width: 430px;
            min-height: 100px;
            text-align: center;
            text-transform: uppercase;
            font-weight: normal;
            border: 5px dashed #CCC;
            height: 100px;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
        var files;
        function handleDragOver(event) {
            event.stopPropagation();
            event.preventDefault();
            var dropZone = document.getElementById('drop_zone');
            dropZone.innerHTML = "Pode soltar...";
        }

        function handleDnDFileSelect(event) {
            event.stopPropagation();
            event.preventDefault();

            /* Read the list of all the selected files. */
            files = event.dataTransfer.files;

            /* Consolidate the output element. */
            var form = document.getElementById('form1');
            var data = new FormData(form);

            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            var xhr = new XMLHttpRequest();
            debugger;
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200 && xhr.responseText) {
                    alert("Imagem carregada. Clique no botão '>>' para salvar.");

                } else {
                    //alert("ERRO ao carregar foto.");
                }
            };
            xhr.open('POST', "desenv_pedido_cad.aspx");
            // xhr.setRequestHeader("Content-type", "multipart/form-data");
            xhr.send(data);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btCarregarFoto" />
            <asp:PostBackTrigger ControlID="btIncluirPedido" />
            <asp:PostBackTrigger ControlID="ddlFormaPgto" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Controle
                    de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Pedido</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">
                        Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Pedido"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                            <asp:HiddenField ID="hidColecao" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="labPedidoNumero" runat="server" Text="Número do Pedido"></asp:Label>
                            <asp:HiddenField ID="hidPedido" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labCorFornecedor" runat="server" Text="Cor"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labDataPedido" runat="server" Text="Data do Pedido"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labDataPrevisaoEntrega" runat="server" Text="Previsão de Entrega"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labUnidadeMedida" runat="server" Text="Unidade de Medida"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labQtde" runat="server" Text="Quantidade"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labPreco" runat="server" Text="Preço"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labFormaPgto" runat="server" Text="Forma de Pagamento"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px;">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="143px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtPedidoNumero" runat="server" Width="110px" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);" OnTextChanged="txtPedidoNumero_TextChanged"
                                AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td style="width: 190px;">
                            <asp:TextBox ID="txtFornecedor" runat="server" MaxLength="200" Width="180px"></asp:TextBox>
                        </td>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtCorFornecedor" runat="server" MaxLength="20" Width="120px"></asp:TextBox>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtPedidoData" runat="server" Width="150px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td style="width: 140px;">
                            <asp:TextBox ID="txtPedidoPrevEntrega" runat="server" Width="130px" MaxLength="10"
                                Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td style="width: 146px;">
                            <asp:DropDownList ID="ddlUnidade" Height="21px" runat="server" DataValueField="CODIGO"
                                DataTextField="DESCRICAO" Width="140px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtQtde" runat="server" Width="100px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtPreco" runat="server" Width="110px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFormaPgto" runat="server" Width="170px" Height="21px" DataTextField="DESC_COND_PGTO"
                                DataValueField="CONDICAO_PGTO" OnSelectedIndexChanged="ddlFormaPgto_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="line-height: 5px;">
                        <td colspan="10">
                            &nbsp;
                        </td>
                    </tr>
                    <asp:Panel ID="pnlFormaPgtoOutro" runat="server">
                        <tr>
                            <td colspan="9">
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtFormaPgtoOutro" runat="server" Width="166px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
                <fieldset>
                    <legend>Foto Tecido</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="3" valign="top">
                                <asp:FileUpload ID="upFoto" runat="server" />
                                <br />
                                <br />
                                <div id="drop_zone">
                                    Arraste a imagem...</div>
                            </td>
                            <td style="text-align: left; padding-bottom: 10px;" valign="bottom">
                                <asp:Button ID="btCarregarFoto" runat="server" Text=">>" OnClick="btCarregarFoto_Click" />
                                <asp:HiddenField ID="hidAcao" runat="server" />
                            </td>
                            <td colspan="4" style="text-align: left;">
                                <asp:Image ID="imgFoto" runat="server" Width="154px" Height="160px" />
                            </td>
                            <td colspan="2" style="text-align: right; padding-bottom: 10px;" valign="bottom">
                                <asp:Button ID="btIncluirPedido" runat="server" Text="Criar Pedido" Width="110px"
                                    OnClick="btIncluirPedido_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:Panel ID="pnlProduto" runat="server" Visible="false">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="10">
                                <fieldset style="margin-top: -5px;">
                                    <legend>Produtos</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                Grupo
                                            </td>
                                            <td>
                                                Modelo
                                            </td>
                                            <td>
                                                Cor
                                            </td>
                                            <td>
                                                Produtos Adicionados
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td style="text-align: right; color: Red;">
                                                <asp:Label ID="labTotalCarrinho" runat="server" Text="Nenhum produto adicionado"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">
                                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                                    DataValueField="GRUPO_PRODUTO">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 160px;">
                                                <asp:TextBox ID="txtModelo" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                                            </td>
                                            <td style="width: 160px;">
                                                <asp:TextBox ID="txtCor" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                                            </td>
                                            <td style="width: 150px; text-align: center;">
                                                <asp:CheckBox ID="cbProdutoAdicionado" runat="server" Checked="false" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btFiltrar" runat="server" Text="Filtrar" Width="100px" OnClick="btFiltrar_Click" />
                                                <asp:Label ID="labFiltro" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td style="text-align: right; color: Gray; font-size: 12px;">
                                                <asp:LinkButton ID="lnkProdutoResumo" runat="server" CssClass="alink" Text="Resumo do Pedido"
                                                    ToolTip="Resumo do Pedido" OnClick="lnkProdutoResumo_Click"></asp:LinkButton><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                        DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="Gainsboro" />
                                                        <RowStyle ForeColor="Gray" />
                                                        <FooterStyle BackColor="Gainsboro" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Coleção" HeaderStyle-Width="170px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CODIGO_REF" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Código" HeaderStyle-Width="100px" />
                                                            <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Grupo" HeaderStyle-Width="160px" />
                                                            <asp:BoundField DataField="MODELO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Modelo" HeaderStyle-Width="140px" />
                                                            <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Cor" HeaderStyle-Width="140px" />
                                                            <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                                HeaderText="Qtde Varejo" HeaderStyle-Width="120px" />
                                                            <asp:BoundField DataField="PRECO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                                HeaderText="Preço" HeaderStyle-Width="120px" />
                                                            <asp:TemplateField HeaderText="Consumo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Width="140px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtConsumo" runat="server" MaxLength="6" Width="140px" CssClass="alinharDireita"
                                                                        onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btExcluirCarrinho" runat="server" Width="70px" Text="Excluir" OnClick="btExcluirCarrinho_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                HeaderStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btIncluirCarrinho" runat="server" Width="70px" Text="Incluir" OnClick="btIncluirCarrinho_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Button ID="btExcluir" runat="server" Text="Cancelar" Width="110px" OnClientClick="return ConfirmarExclusao();"
                                    OnClick="btExcluir_Click" />
                            </td>
                            <td colspan="9" style="text-align: right;">
                                <asp:HiddenField ID="hidTotalCarrinho" runat="server" />
                                <asp:Label ID="labBuscar" runat="server" ForeColor="Red" Text=""></asp:Label>
                                <asp:Button ID="btSalvar" runat="server" Text="Fechar Pedido" Width="110px" OnClick="btSalvar_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
            <div id="dialogPai" runat="server">
                <div id="dialog" title="Mensagem" class="divPop">
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <strong>PEDIDO
                                    <asp:Label ID="labPopUp" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Label ID="labMensagem" runat="server" Text="CADASTRADO COM SUCESSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Por favor, selecione a tela desejada nos botões abaixo.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        if (window.File && window.FileList && window.FileReader) {
            /************************************ 
            * All the File APIs are supported. * 
            * Entire code goes here.           *
            ************************************/


            /* Setup the Drag-n-Drop listeners. */
            var dropZone = document.getElementById('drop_zone');
            dropZone.addEventListener('dragover', handleDragOver, false);
            dropZone.addEventListener('drop', handleDnDFileSelect, false);

        }
        else {
            alert('Sorry! this browser does not support HTML5 File APIs.');
        }
    </script>
</asp:Content>
