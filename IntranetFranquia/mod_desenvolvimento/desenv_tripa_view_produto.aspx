<%@ Page Title="Liberação de Produto (View)" Language="C#" AutoEventWireup="true" CodeBehind="desenv_tripa_view_produto.aspx.cs"
    Inherits="Relatorios.desenv_tripa_view_produto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foto Produto</title>
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
            xhr.open('POST', "desenv_tripa_view_produto.aspx");
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
                        de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Liberação de Produto (View)&nbsp;&nbsp;>&nbsp;&nbsp;Produto</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Produto</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="labProduto" runat="server" Text="Produto"></asp:Label>
                                    </td>
                                    <td colspan="7">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtModelo" runat="server" Width="160px" MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td colspan="7">
                                        <asp:Button ID="btCmax" runat="server" OnClick="btProdutoSeq_Click" Width="65px" Text="C-Max" Visible="false" />&nbsp;&nbsp;<asp:Button ID="btHandbook" runat="server" OnClick="btProdutoSeq_Click" Width="80px" Text="Handbook" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labOrigem" runat="server" Text="Origem"></asp:Label>
                                        <asp:HiddenField ID="hidCodigoPrePedido" runat="server" />
                                        <asp:HiddenField ID="hidCodigoProduto" runat="server" />
                                        <asp:HiddenField ID="hidColecao" runat="server" />
                                        <asp:HiddenField ID="hidCodRef" runat="server" />
                                        <asp:HiddenField ID="hidProduto" runat="server" />
                                        <asp:HiddenField ID="hidMarca" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labGriffe" runat="server" Text="Griffe"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labSegmento" runat="server" Text="Segmento"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labTecido" runat="server" Text="Tecido"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlOrigem" runat="server" Width="164px" Height="21px" DataTextField="DESCRICAO"
                                            DataValueField="CODIGO">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlGrupo" runat="server" Width="154px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                            DataValueField="GRUPO_PRODUTO" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlGriffe" runat="server" Width="294px" Height="21px" DataTextField="GRIFFE"
                                            DataValueField="GRIFFE" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSegmento" runat="server" Width="130px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTecido" runat="server" Width="140px" MaxLength="50" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlFornecedor" runat="server" Width="214px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labNomeProduto" runat="server" Text="Nome Produto"></asp:Label>
                                    </td>
                                    <td>REF de Modelagem
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoVendaVarejo" runat="server" Text="Preço Venda Varejo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoVendaAtacado" runat="server" Text="Preço Venda Atacado"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labBrinde" runat="server" Text="Brinde"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labProdutoAcabado" runat="server" Text="Produto Acabado"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labSigned" runat="server" Text="Signed"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labSignedNome" runat="server" Text="Signed Nome"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 170px;">
                                        <asp:TextBox ID="txtNome" runat="server" Width="160px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td style="width: 160px;">
                                        <asp:TextBox ID="txtREFModelagem" runat="server" Width="150px" MaxLength="26"></asp:TextBox>
                                    </td>
                                    <td style="width: 160px;">
                                        <asp:TextBox ID="txtPrecoVendaVarejo" runat="server" Width="150px" MaxLength="10"
                                            CssClass="alinharDireita" OnTextChanged="txtPreco_TextChanged" AutoPostBack="true" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td style="width: 140px;">
                                        <asp:TextBox ID="txtPrecoVendaAtacado" runat="server" Width="130px" MaxLength="10"
                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td style="width: 140px;">
                                        <asp:DropDownList ID="ddlBrinde" runat="server" Width="134px" Height="21px">
                                            <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:DropDownList ID="ddlProdutoAcabado" runat="server" Width="144px" Height="21px" OnSelectedIndexChanged="ddlProdutoAcabado_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 70px;">
                                        <asp:DropDownList ID="ddlSigned" runat="server" Width="64px" Height="21px">
                                            <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSignedNome" runat="server" Width="140px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labCorLinx" runat="server" Text="Cor Linx"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCorFornecedor" runat="server" Text="Cor Fornecedor"></asp:Label>
                                    </td>
                                    <td>Qtde Mostruário
                                    </td>
                                    <td>
                                        <asp:Label ID="labQtdeVarejo" runat="server" Text="Qtde Varejo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labQtdeAtacado" runat="server" Text="Qtde Atacado"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labLinha" runat="server" Text="Linha"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labObservacao" runat="server" Text="Observação"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlCor" runat="server" Width="164px" Height="21px" DataValueField="COR" DataTextField="DESC_COR" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCorFornecedor" runat="server" Width="150px" MaxLength="50" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQtdeMostruario" runat="server" Width="150px" MaxLength="10" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQtdeVarejo" runat="server" Width="130px" MaxLength="10" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQtdeAtacado" runat="server" Width="130px" MaxLength="10" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLinha" runat="server" Width="144px" DataValueField="LINHA" DataTextField="LINHA" Height="22px"></asp:DropDownList>
                                    </td>
                                    <td colspan="2" style="">
                                        <asp:TextBox ID="txtObservacao" runat="server" Width="210px" MaxLength="30"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labSubGrupo" runat="server" Text="Malha/Plano"></asp:Label>
                                    </td>
                                    <td colspan="7">
                                        <asp:Label ID="labModDesign" runat="server" Text="Modelagem ou Design?"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlSubGrupo" runat="server" Width="164px" Height="21px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="PLANO" Text="PLANO"></asp:ListItem>
                                            <asp:ListItem Value="MALHA" Text="MALHA"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="7">
                                        <asp:DropDownList ID="ddlModDesign" runat="server" Width="314px" Height="21px" OnSelectedIndexChanged="ddlModDesign_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Pré-Modelo"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Modelagem de Criação"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Design de Estampas"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Modelagem de Criação e Design de Estampas"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Produto Acabado"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:Label ID="labObsImpressao" runat="server" Text="Observação Impressão"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:TextBox ID="txtObsImpressao" runat="server" Width="1128px" Height="65px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8" style="text-align: right;">
                                        <asp:Button ID="btAlterar" runat="server" Text="Salvar" Width="100px" OnClick="btAlterar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:HiddenField ID="hidQtdeMostruario" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hidQtdeVarejo" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hidQtdeAtacado" runat="server"></asp:HiddenField>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8" style="text-align: right;">
                                        <asp:Label ID="labErroAlteracao" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlFoto" runat="server" Visible="true">
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
                            </asp:Panel>
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
