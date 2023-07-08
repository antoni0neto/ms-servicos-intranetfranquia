<%@ Page Title="Entrada Material" Language="C#" AutoEventWireup="true" CodeBehind="desenv_material_compra_entrada.aspx.cs"
    Inherits="Relatorios.desenv_material_compra_entrada" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Entrada Material</title>
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
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
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
            xhr.open('POST', "desenv_material_compra_entrada.aspx");
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
                        de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Entrada Material</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Entrada Material</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Número<asp:HiddenField ID="hidCodigoPedido" runat="server" />
                                    </td>
                                    <td>Grupo
                                    </td>
                                    <td>SubGrupo
                                    </td>
                                    <td>Cor
                                    </td>
                                    <td>Cor Fornecedor
                                    </td>
                                    <td colspan="2">Fornecedor
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtNumeroPedido" runat="server" Enabled="false" Width="130px" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGrupoTecido" runat="server" Enabled="false" Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubGrupoTecido" runat="server" Enabled="false" Width="200px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCor" runat="server" Enabled="false" Width="170px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescricaoCor" runat="server" Enabled="false" Width="190px"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtFornecedor" runat="server" Enabled="false" Width="214px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Data Entrada
                                    </td>
                                    <td>Quantidade
                                    </td>
                                    <td>NF Entrada
                                    </td>
                                    <td>Valor da Nota
                                    </td>
                                    <td>Produto Fornecedor
                                    </td>
                                    <td>Largura
                                    </td>
                                    <td>Volume
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 140px;">
                                        <asp:TextBox ID="txtDataEntrada" runat="server" Width="130px" onkeypress="return fnReadOnly(event);" CssClass="alinharDireita" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtQuantidade" runat="server" Width="190px" MaxLength="10" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtNF" runat="server" Width="200px" MaxLength="15"></asp:TextBox>
                                    </td>
                                    <td style="width: 180px;">
                                        <asp:TextBox ID="txtValorNF" runat="server" Width="170px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" MaxLength="12"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtProdutoFornecedor" runat="server" Width="190px" MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td style="width: 115px;">
                                        <asp:TextBox ID="txtLargura" runat="server" Width="105px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" MaxLength="12"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVolume" runat="server" Width="99px" onkeypress="return fnValidarNumero(event);" CssClass="alinharDireita" MaxLength="12"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>1º Vencimento
                                    </td>
                                    <td>2º Vencimento
                                    </td>
                                    <td>3º Vencimento
                                    </td>
                                    <td>4º Vencimento
                                    </td>
                                    <td>5º Vencimento
                                    </td>
                                    <td colspan="2">6º Vencimento
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtVencimento1" runat="server" Width="130px" onkeypress="return fnReadOnly(event);" CssClass="alinharDireita" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVencimento2" runat="server" Width="190px" onkeypress="return fnReadOnly(event);" CssClass="alinharDireita" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVencimento3" runat="server" Width="200px" onkeypress="return fnReadOnly(event);" CssClass="alinharDireita" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVencimento4" runat="server" Width="170px" onkeypress="return fnReadOnly(event);" CssClass="alinharDireita" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVencimento5" runat="server" Width="190px" onkeypress="return fnReadOnly(event);" CssClass="alinharDireita" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtVencimento6" runat="server" Width="214px" onkeypress="return fnReadOnly(event);" CssClass="alinharDireita" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">Rendimento (Metros)</td>
                                </tr>
                                <td colspan="7">
                                    <asp:TextBox ID="txtRendimento" runat="server" Width="130px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" MaxLength="12"></asp:TextBox>
                                </td>
                                <tr>
                                    <td colspan="7">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7" style="text-align: right;">
                                        <asp:Label ID="labErroAlteracao" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;&nbsp;<asp:Button ID="btAlterar" runat="server" Text="Salvar" Width="100px" OnClick="btAlterar_Click" />
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
                            <%-- DIVISAO - /\ CAMPOS + \/ FOTO--%>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
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
