<%@ Page Title="Relatório Comissão" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="rh_rel_folha.aspx.cs" Inherits="Relatorios.rh_rel_folha"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }

        .corTD {
            background-color: #EFEFEF;
        }

        .pageStyl a, .pageStyl span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .pageStyl a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .pageStyl span {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }
    </style>
    <link href="../Styles/gridviewScroll.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript" src="../js/gridviewScroll.min.js"></script>

    <script type="text/javascript">

        function gridviewScroll() {

            gvAtacado = $('#MainContent_gvFolha').gridviewScroll({
                width: 1430,
                height: 550,
                railcolor: "#F0F0F0",
                barcolor: "#CDCDCD",
                barhovercolor: "#606060",
                bgcolor: "#F0F0F0",
                freezesize: 6,
                arrowsize: 30,
                varrowtopimg: "../Image/arrowvt.png",
                varrowbottomimg: "../Image/arrowvb.png",
                harrowleftimg: "../Image/arrowhl.png",
                harrowrightimg: "../Image/arrowhr.png",
                headerrowcount: 1,
                railsize: 16,
                barsize: 12
            });
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Folha&nbsp;&nbsp;>&nbsp;&nbsp;Relatório Comissão</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Relatório Comissão"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Competência
                        </td>
                        <td>Filial
                        </td>
                        <td>CPF
                        </td>
                        <td>CÓD.Vendedor</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 256px;">
                            <asp:DropDownList runat="server" ID="ddlCompetencia" DataValueField="CODIGO_ACOMPANHAMENTO_MESANO" DataTextField="DESCRICAO"
                                Height="22px" Width="250px" OnSelectedIndexChanged="ddlCompetencia_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 256px;">
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                Height="22px" Width="250px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtCPF" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtVendedor" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>

                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvFolha" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white; width: 3500px;" OnRowDataBound="gvFolha_RowDataBound"
                                    OnDataBound="gvFolha_DataBound" OnSorting="gvFolha_Sorting" AllowSorting="true"
                                    ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                    <RowStyle Font-Size="Smaller" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Left" Font-Size="X-Small" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VENDEDOR" HeaderText="CÓD. Vendedor" SortExpression="VENDEDOR" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-CssClass="corTD" />
                                        <asp:TemplateField HeaderText="Nome" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="X-Small"
                                            SortExpression="NOME" ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cargo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                            SortExpression="CARGO" ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCargo" runat="server" Text='<%# Bind("CARGO") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Mínimo Garantido" SortExpression="MINIMO_GARANTIDO" ItemStyle-Width="150px" HeaderStyle-Width="150px"
                                            ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litMinino" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="(%) Comissão" SortExpression="COMISSAO_PORC" ItemStyle-Width="130px" HeaderStyle-Width="130px"
                                            ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litComissaoPorc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="V. Tot. p/ Comissão" SortExpression="VALOR_VENDA_TOTAL_2120" ItemStyle-Width="180px" HeaderStyle-Width="180px"
                                            ItemStyle-BackColor="#99ccff">
                                            <ItemTemplate>
                                                <asp:Literal ID="litVALOR_VENDA_TOTAL_2120" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comissão" SortExpression="COMISSAO_VENDA_TOTAL_2120" ItemStyle-Width="180px" HeaderStyle-Width="180px"
                                            ItemStyle-BackColor="#99ccff">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCOMISSAO_VENDA_TOTAL_2120" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="V. Loja p/ Comissão" SortExpression="VALOR_VENDIDOCOM_2120" ItemStyle-Width="180px" HeaderStyle-Width="180px"
                                            ItemStyle-BackColor="#AFEEEE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litVALOR_VENDIDOCOM_2120" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comissão Loja" SortExpression="COMISSAO_VENDA_2120" ItemStyle-Width="180px" HeaderStyle-Width="180px"
                                            ItemStyle-BackColor="#AFEEEE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCOMISSAO_VENDA_2120" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="V. Ecom p/ Comissão" SortExpression="VALOR_VENDIDO_ECOM_2120" ItemStyle-Width="180px" HeaderStyle-Width="180px"
                                            ItemStyle-BackColor="#99ccff">
                                            <ItemTemplate>
                                                <asp:Literal ID="litVALOR_VENDIDO_ECOM_2120" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comissão Ecom" SortExpression="COMISSAO_ECOM_2120" ItemStyle-Width="180px" HeaderStyle-Width="180px"
                                            ItemStyle-BackColor="#99ccff">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCOMISSAO_ECOM_2120" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="V. Tot Cota" SortExpression="VALOR_VENDIDOCOTA_0130" ItemStyle-Width="150px" HeaderStyle-Width="150px"
                                            ItemStyle-BackColor="#ccffff">
                                            <ItemTemplate>
                                                <asp:Literal ID="litVALOR_VENDIDOCOTA_0130" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comissão Cota" SortExpression="COMISSAO_COTA_0130" ItemStyle-Width="180px" HeaderStyle-Width="180px"
                                            ItemStyle-BackColor="#ccffff">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCOMISSAO_COTA_0130" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Prêmio Cota" SortExpression="PREMIO_COTA" ItemStyle-Width="120px" HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litPREMIO_COTA" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Prêmio Vendedor" SortExpression="PREMIO_VENDEDOR" ItemStyle-Width="145px" HeaderStyle-Width="145px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litPREMIO_VENDEDOR" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prêmio Ponta" SortExpression="PREMIO_PONTA" ItemStyle-Width="130px" HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litPREMIO_PONTA" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cota Total (%)" SortExpression="COTA_TOTAL" ItemStyle-Width="130px" HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCOTA_TOTAL" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cota Semana 1 (%)" SortExpression="COTA_SEM1" ItemStyle-Width="160px" HeaderStyle-Width="160px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCOTA_SEM1" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cota Semana 2 (%)" SortExpression="COTA_SEM2" ItemStyle-Width="160px" HeaderStyle-Width="160px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCOTA_SEM2" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cota Semana 3 (%)" SortExpression="COTA_SEM3" ItemStyle-Width="160px" HeaderStyle-Width="160px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCOTA_SEM3" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cota Semana 4 (%)" SortExpression="COTA_SEM4">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCOTA_SEM4" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
