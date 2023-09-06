<%@ Page Title="Editar Acessório" Language="C#" AutoEventWireup="true" CodeBehind="desenv_acessorio_editar.aspx.cs"
    Inherits="Relatorios.desenv_acessorio_editar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Editar Acessório</title>
    <style type="text/css">
        .fs {
            border: 1px solid #ccc;
            font-family: Calibri;
        }

        .alinharDireita {
            text-align: right;
        }

        #drop_zone {
            margin: 10px 0;
            width: 324px;
            min-height: 100px;
            text-align: center;
            text-transform: uppercase;
            font-weight: normal;
            border: 5px dashed #CCC;
            height: 100px;
        }
    </style>
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
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
            xhr.open('POST', "desenv_acessorio_editar.aspx");
            // xhr.setRequestHeader("Content-type", "multipart/form-data");
            xhr.send(data);

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btCarregarFoto" />
            </Triggers>
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento de Coleção de Acessórios&nbsp;&nbsp;>&nbsp;&nbsp;Desenvolvimento de Coleção de Acessórios&nbsp;&nbsp;>&nbsp;&nbsp;Editar Acessório</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Editar Acessório</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="labOrigem" runat="server" Text="Origem"></asp:Label>
                                        <asp:HiddenField ID="hidCodigoAcessorio" runat="server" />
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labProduto" runat="server" Text="Produto"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labGriffe" runat="server" Text="Griffe"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px;">
                                        <asp:DropDownList ID="ddlOrigem" runat="server" Width="194px" Height="21px" DataTextField="DESCRICAO"
                                            DataValueField="CODIGO">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2" style="width: 220px;">
                                        <asp:DropDownList ID="ddlGrupo" runat="server" Width="214px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                            DataValueField="GRUPO_PRODUTO">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 180px;">
                                        <asp:TextBox ID="txtProduto" runat="server" Width="170px" MaxLength="20" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td style="width: 160px;">
                                        <asp:DropDownList ID="ddlGriffe" runat="server" Width="154px" Height="21px" DataTextField="GRIFFE"
                                            DataValueField="GRIFFE">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlFornecedor" runat="server" Width="370px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labCorFornecedor" runat="server" Text="Cor Fornecedor"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labDescricaoSugerida" runat="server" Text="Descrição Sugerida"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labReferFabricante" runat="server" Text="Referência Fabricante"></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlCor" runat="server" Width="194px" Height="21px" DataValueField="COR" DataTextField="DESC_COR">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtCorFornecedor" runat="server" Width="210px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDescricaoSugerida" runat="server" Width="330px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtReferFabricante" runat="server" Width="366px" MaxLength="150"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labQuantidade" runat="server" Text="Quantidade"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labPreco" runat="server" Text="Preço"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCusto" runat="server" Text="Custo"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labObservacao" runat="server" Text="Observação"></asp:Label>
                                    </td>

                                    <td>
                                        <asp:Label ID="labPrevEntrega" runat="server" Text="Previsão de Entrega"></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>


                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtQuantidade" runat="server" Width="190px" MaxLength="5" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPreco" runat="server" Width="100px" MaxLength="10" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCusto" runat="server" Width="100px" MaxLength="10" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtObservacao" runat="server" Width="330px" MaxLength="30"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDataPrevEntrega" runat="server" Width="150px" MaxLength="10" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btAlterar" runat="server" Text="Salvar" Width="100px" OnClick="btAlterar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;<asp:Label ID="labErroAlteracao" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvHistoricoPedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvHistoricoPedido_RowDataBound"
                                                OnDataBound="gvHistoricoPedido_DataBound" ShowFooter="true">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex  +1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pedido" HeaderStyle-Width="125px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPedido" runat="server" Text='<%# Bind("PEDIDO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Data do Pedido" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDataPedido" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantidade" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("QTDE_ORIGINAL") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor Total" ItemStyle-Width="160" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorTotal" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="10">Qtde Fotos
                                    <asp:HiddenField ID="hidCodigo" runat="server" />
                                        <asp:HiddenField ID="hidFoto2" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">
                                        <asp:DropDownList ID="ddlQtdeFoto" runat="server" Width="100px" OnSelectedIndexChanged="ddlQtdeFoto_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="1" Text="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">
                                        <asp:RadioButton ID="rdbFoto1" runat="server" GroupName="Foto" Text="Primeira Foto"
                                            TextAlign="Right" />
                                        <asp:RadioButton ID="rdbFoto2" runat="server" GroupName="Foto" Text="Segunda Foto"
                                            TextAlign="Right" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" style="width: 350px;">
                                        <asp:FileUpload ID="upFoto" runat="server" />
                                        <br />
                                        <br />
                                        <div id="drop_zone">
                                            Arraste a imagem ...
                                        </div>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="3" style="text-align: left; padding-bottom: 10px;" valign="bottom">
                                        <asp:Button ID="btCarregarFoto" runat="server" Text=">>" OnClick="btCarregarFoto_Click" />
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Image ID="imgFoto" runat="server" Width="110px" Height="160px" />
                                        <asp:Image ID="imgFoto2" runat="server" Width="110px" Height="160px" />
                                    </td>
                                    <td colspan="5" style="text-align: right; padding-bottom: 10px;" valign="bottom">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9" style="text-align: left;" valign="bottom">
                                        <asp:Button ID="btSalvar" runat="server" Text="Salvar Foto" Width="100px" OnClick="btSalvar_Click" />&nbsp;&nbsp;
                                    <asp:Label ID="labSalvar" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: right;" valign="bottom">
                                        <asp:Label ID="labExclusao" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        &nbsp;&nbsp;
                                    <asp:Button ID="btExcluir" runat="server" Text="Excluir Foto" CommandArgument="1"
                                        Width="100px" OnClick="btExcluir_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlPocket" runat="server" BackColor="White" BorderWidth="1" BorderColor="White"
                                            Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <br />
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
    </form>
</body>
</html>
