<%@ Page Title="Mover Arquivos XML" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="admfis_leitura_xmlnfe_mover.aspx.cs" Inherits="Relatorios.admfis_leitura_xmlnfe_mover" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">

        function moverArquivosXml() {

            var eleLab = document.getElementById("MainContent_labMsg");
            eleLab.innerHTML = "Movendo arquivos... Aguarde...";

            $.ajax({
                type: 'POST',
                url: 'admfis_leitura_xmlnfe_mover.aspx/TransformarArquivos',
                contentType: "application/json; charset=utf-8",
                //data: '{ produtos: [' + produtos.join() + '], codigoBloco: ' + codigoBloco + ' }',
                dataType: 'json',
                success: function (results) {
                    eleLab.innerHTML = results.d;
                },
                error: function (a, b, c) {
                    eleLab.innerHTML = "Erro ao mover arquivos...";
                    console.log(a);
                    console.log(b);
                    console.log(c);
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo ADM Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;NF-e LINX&nbsp;&nbsp;>&nbsp;&nbsp;Mover Arquivos XML</span>
                <div style="float: right; padding: 0;">
                    <a href="admfis_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Mover Arquivos XML</legend>
                    <br />
                    <div style="width: 100%;">
                        <input type="button" onclick="moverArquivosXml();" value="Mover Arquivos" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="labMsg" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size=""></asp:Label>
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
