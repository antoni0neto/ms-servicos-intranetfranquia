<%@ Page Title="Foto Produto" Language="C#" AutoEventWireup="true" CodeBehind="desenv_produto_foto.aspx.cs"
    Inherits="Relatorios.desenv_produto_foto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foto Produto</title>
    <style type="text/css">
        .fs
        {
            border: 1px solid #ccc;
            font-family: Calibri;
        }
        .alinharDireita
        {
            text-align: right;
        }
        #drop_zone
        {
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
            xhr.open('POST', "desenv_produto_foto.aspx");
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
            <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px;
                background-color: White;">
                <br />
                <div>
                    <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
                        de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Desenvolvimento de Coleção&nbsp;&nbsp;>&nbsp;&nbsp;Foto
                        Produto</span>
                    <div style="float: right; padding: 0;">
                        <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                    </div>
                </div>
                <hr />
                <div>
                    <fieldset>
                        <legend>Foto Produto</legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan="10">
                                    Qtde Fotos
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
                                <td colspan="10">
                                    &nbsp;
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
                                <td colspan="10">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 350px;">
                                    <asp:FileUpload ID="upFoto" runat="server" />
                                    <br />
                                    <br />
                                    <div id="drop_zone">
                                        Arraste a imagem ...</div>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3" style="text-align: left; padding-bottom: 10px;" valign="bottom">
                                    <asp:Button ID="btCarregarFoto" runat="server" Text=">>" OnClick="btCarregarFoto_Click" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:Image ID="imgFoto" runat="server" Width="110px" Height="160px" />
                                    <asp:Image ID="imgFoto2" runat="server" Width="110px" Height="160px" />
                                </td>
                                <td colspan="5" style="text-align: right; padding-bottom: 10px;" valign="bottom">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    &nbsp;
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
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
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
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
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
