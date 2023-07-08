﻿<%@ Page Title="Manutenção de Desenvolvimento de Coleção" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_pocket_manut.aspx.cs" Inherits="Relatorios.desenv_pocket_manut"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla
        {
            background-color: #000;
            color: white;
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
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
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
            xhr.open('POST', "desenv_pocket_manut.aspx");
            // xhr.setRequestHeader("Content-type", "multipart/form-data");
            xhr.send(data);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btCarregarFoto" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
                    de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Manutenção de Desenvolvimento de Coleção</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Manutenção de Desenvolvimento de Coleção"></asp:Label></legend>
                <fieldset style="margin-top: 0px;">
                    <legend>
                        <asp:Label ID="labAcao" runat="server" Text="Produto"></asp:Label>
                    </legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -10px;">
                        <tr>
                            <td>
                                <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labOrigem" runat="server" Text="Origem"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCodigo" runat="server" Text="Código"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labModelo" runat="server" Text="Modelo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labTecido" runat="server" Text="Tecido"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="194px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO" AutoPostBack="true" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                                    Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlOrigem" runat="server" Width="174px" Height="21px" DataTextField="DESCRICAO"
                                    DataValueField="CODIGO" Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtCodigoRef" runat="server" Width="150px" MaxLength="8" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                    DataValueField="GRUPO_PRODUTO" Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtModelo" runat="server" Width="200px" MaxLength="20" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtCor" runat="server" Width="200px" MaxLength="20" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTecido" runat="server" Width="260px" MaxLength="50" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labQtde" runat="server" Text="Qtde Varejo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labPreco" runat="server" Text="Preço Varejo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labQtdeAtacado" runat="server" Text="Qtde Atacado"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labPrecoAtacado" runat="server" Text="Preço Atacado"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCorFornecedor" runat="server" Text="Cor Fornecedor"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labObservacao" runat="server" Text="Observação"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtQtdeVarejo" runat="server" Width="190px" MaxLength="10" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPreco" runat="server" Width="170px" MaxLength="8" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumeroDecimal(event);" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQtdeAtacado" runat="server" Width="150px" MaxLength="10" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrecoAtacado" runat="server" Width="170px" MaxLength="8" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumeroDecimal(event);" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorFornecedor" runat="server" Width="200px" MaxLength="25" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtObservacao" runat="server" Width="200px" MaxLength="25"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                Foto
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top">
                                <asp:FileUpload ID="upFoto" runat="server" />
                                <br />
                                <br />
                                <div id="drop_zone">
                                    Arraste a imagem...</div>
                            </td>
                            <td colspan="2" style="text-align: left; padding-bottom: 10px;" valign="bottom">
                                <asp:Button ID="btCarregarFoto" runat="server" Text=">>" OnClick="btCarregarFoto_Click" />
                                <asp:HiddenField ID="hidAcao" runat="server" />
                            </td>
                            <td colspan="2" style="text-align: center;">
                                <asp:Image ID="imgFoto" runat="server" Width="110px" Height="160px" />
                            </td>
                            <td style="text-align: right;" valign="bottom">
                                <asp:Button ID="btCancelar" runat="server" Text="Cancelar" Width="100px" OnClick="btCancelar_Click" />
                                <asp:Button ID="btSalvar" runat="server" Text="Salvar" Width="100px" OnClick="btSalvar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="labSalvar" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset style="margin-top: 0px;">
                    <legend>Produtos</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -7px;">
                        <tr>
                            <td style="width: 190px;">
                                Coleção
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlColecoesBuscar" runat="server" Width="174px" Height="21px"
                                    DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbModeloFiltro" runat="server" AutoPostBack="false" Checked="false"
                                    TextAlign="Right" Text="Modelo Vazio" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Grupo
                            </td>
                            <td>
                                Modelo
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlGrupoBuscar" runat="server" Width="174px" Height="21px"
                                    DataTextField="GRUPO_PRODUTO" DataValueField="GRUPO_PRODUTO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtModeloBuscar" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btBuscarProduto" runat="server" Text="Buscar Produto" Width="120px"
                                    OnClick="btBuscarProduto_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labProduto" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                        DataKeyNames="CODIGO" OnLoad="gvProduto_Load">
                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                        <FooterStyle BackColor="GradientActiveCaption" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CODIGO_REF" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Código" HeaderStyle-Width="80px" />
                                            <asp:TemplateField HeaderText="Origem" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litOrigem" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="170px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litGrupo" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modelo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litModelo" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="160px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Varejo" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeVarejo" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Atacado" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeAtacado" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Foto" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgFoto" runat="server" Width="50" Height="90" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Button ID="btProdutoExcluir" runat="server" Height="19px" Width="90px" Text="Excluir"
                                                        OnClick="btProdutoExcluir_Click" OnClientClick="return ConfirmarExclusao();" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btProdutoSalvar" runat="server" Height="19px" Width="90px" Text="Editar"
                                                        OnClick="btProdutoEditar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btProdutoAprovar" runat="server" Height="19px" Width="90px" Text="Aprovar"
                                                        OnClick="btProdutoAprovar_Click" OnClientClick="return ConfirmarAprovacao();" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </fieldset>
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
