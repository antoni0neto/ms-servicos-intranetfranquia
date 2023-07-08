﻿<%@ Page Title="DRE" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="dre_dre_v50_gerencial.aspx.cs" Inherits="Relatorios.dre_dre_v50_gerencial" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }
    </style>
    <script src="../../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../../Image/plus.png");
            $(this).closest("tr").next().remove();
        });

        function openwindow(l) {
            window.open(l, "DRE", "menubar=1,resizable=0,width=1400,height=800");
        }

    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcel" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;DRE&nbsp;&nbsp;>&nbsp;&nbsp;DRE&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a href="#" class="alink" title="">-</a>
                </div>
            </div>
            <hr />
            <div>
                <fieldset>
                    <legend>DRE</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Ano
                            </td>
                            <td>Mês De
                            </td>
                            <td>Até o Mês
                            </td>
                            <td>Tipo
                            </td>
                            <td>Unidade de Negócio
                            </td>
                            <td>Filial
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="144px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlMesDe" Height="22px" Width="144px" Enabled="true">
                                    <asp:ListItem Value="1" Text="Janeiro" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Fevereiro"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Março"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Maio"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Junho"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="Julho"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="Setembro"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="Dezembro"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlMesAte" Height="22px" Width="144px" Enabled="true">
                                    <asp:ListItem Value="1" Text="Janeiro"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Fevereiro"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Março"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Maio"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Junho"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="Julho"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="Setembro"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="Dezembro" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlTipo" Height="22px" Width="144px" Enabled="true">
                                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                    <asp:ListItem Value="G" Text="GERENCIAL" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlUnidadeNegocio" Height="22px" Width="144px" Enabled="true" OnSelectedIndexChanged="ddlUnidadeNegocio_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="ATACADO" Text="ATACADO"></asp:ListItem>
                                    <asp:ListItem Value="ECOMMERCE" Text="ECOMMERCE"></asp:ListItem>
                                    <asp:ListItem Value="FABRICA" Text="FABRICA"></asp:ListItem>
                                    <asp:ListItem Value="VAREJO" Text="VAREJO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 300px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="CODIGO_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="294px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btExcel" runat="server" Width="100px" Text="Excel" OnClick="btExcel_Click" />
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="8">
                                <fieldset>
                                    <div class="rounded_corners">
                                        <% //RECEITA LIQUIDA %>
                                        <asp:GridView ID="gvReceitaLiquida" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvReceitaLiquida_RowDataBound"
                                            OnDataBound="gvReceitaLiquida_DataBound" ShowFooter="false">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlFatBruto" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvFaturamento" runat="server" CellPadding="0" CellSpacing="0" AutoGenerateColumns="false"
                                                                OnDataBound="gvFaturamento_DataBound" OnRowDataBound="gvFaturamento_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                Width="18px" runat="server" />
                                                                            <asp:Panel ID="pnlSubGrid" runat="server" Style="display: none" Width="100%" BorderColor="Red">
                                                                                <asp:GridView ID="gvSubGrid" runat="server" CellPadding="0" CellSpacing="0" AutoGenerateColumns="false"
                                                                                    OnDataBound="gvSubGrid_DataBound" BorderColor="GradientActiveCaption" OnRowDataBound="gvSubGrid_RowDataBound"
                                                                                    Width="100%">
                                                                                    <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                            ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-BorderWidth="1" ItemStyle-Width="142px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSubItem" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text="111"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFaturamento" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="1"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litReceitaBruta" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM RECEITA LIQUIDA %>
                                        <% //CMV %>
                                        <asp:GridView ID="gvCMV" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvCMV_RowDataBound"
                                            OnDataBound="gvCMV_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlCMVItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvCMVItem" runat="server" CellPadding="0" CellSpacing="0" AutoGenerateColumns="false"
                                                                OnDataBound="gvCMVItem_DataBound" OnRowDataBound="gvCMVItem_RowDataBound" Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                Width="18px" runat="server" />
                                                                            <asp:Panel ID="pnlCMVItemSub" runat="server" Style="display: none" Width="100%" BorderColor="Red">
                                                                                <asp:GridView ID="gvCMVItemSub" runat="server" CellPadding="0" CellSpacing="0" AutoGenerateColumns="false"
                                                                                    OnDataBound="gvCMVItemSub_DataBound" BorderColor="GradientActiveCaption" OnRowDataBound="gvCMVItemSub_RowDataBound"
                                                                                    Width="100%">
                                                                                    <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                            ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-BorderWidth="1" ItemStyle-Width="142px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSubItem" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text="211"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFaturamento" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="2"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litReceitaBruta" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM CMV %>
                                        <% //MARGEM DE CONTRIBUICAO %>
                                        <asp:GridView ID="gvMargem" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvMargem_RowDataBound"
                                            OnDataBound="gvMargem_DataBound" ShowFooter="false">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlMargemItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvMargemItem" runat="server" CellPadding="0" CellSpacing="0" AutoGenerateColumns="false"
                                                                OnDataBound="gvMargemItem_DataBound" OnRowDataBound="gvMargemItem_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                Width="18px" runat="server" />
                                                                            <asp:Panel ID="pnlMargemItemSub" runat="server" Style="display: none" Width="100%"
                                                                                BorderColor="Red">
                                                                                <asp:GridView ID="gvMargemItemSub" runat="server" CellPadding="0" CellSpacing="0"
                                                                                    AutoGenerateColumns="false" OnDataBound="gvMargemItemSub_DataBound" BorderColor="GradientActiveCaption"
                                                                                    OnRowDataBound="gvMargemItemSub_RowDataBound" Width="100%">
                                                                                    <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                            ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-BorderWidth="1" ItemStyle-Width="142px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSubItem" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text="32"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litVenda" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="3"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMargem" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM MARGEM DE CONTRIBUICAO %>
                                        <% //CUSTO FIXO%>
                                        <asp:GridView ID="gvCustoFixo" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvCustoFixo_RowDataBound"
                                            OnDataBound="gvCustoFixo_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlCFItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvCustoFixoItem" runat="server" CellPadding="0" CellSpacing="0"
                                                                AutoGenerateColumns="false" OnDataBound="gvCustoFixoItem_DataBound" OnRowDataBound="gvCustoFixoItem_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                Width="18px" runat="server" />
                                                                            <asp:Panel ID="pnlCustoFixoItemSub" runat="server" Style="display: none" Width="100%"
                                                                                BorderColor="Red">
                                                                                <asp:GridView ID="gvCustoFixoItemSub" runat="server" CellPadding="0" CellSpacing="0"
                                                                                    AutoGenerateColumns="false" OnDataBound="gvCustoFixoItemSub_DataBound" BorderColor="GradientActiveCaption"
                                                                                    OnRowDataBound="gvCustoFixoItemSub_RowDataBound" Width="100%">
                                                                                    <RowStyle BorderColor="GradientActiveCaption" Font-Bold="true" Height="22px" BorderWidth="1" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="leandro" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                                            ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                                    Width="18px" runat="server" />
                                                                                                <asp:Panel ID="pnlCustoFixoItemSubSub" runat="server" Style="display: none" Width="100%"
                                                                                                    BorderColor="Red">
                                                                                                    <asp:GridView ID="gvCustoFixoItemSubSub" runat="server" CellPadding="0" CellSpacing="0"
                                                                                                        AutoGenerateColumns="false" OnDataBound="gvCustoFixoItemSubSub_DataBound" BorderColor="GradientActiveCaption"
                                                                                                        OnRowDataBound="gvCustoFixoItemSubSub_RowDataBound" Width="100%">
                                                                                                        <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                                                ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                                                                ItemStyle-BorderWidth="1" ItemStyle-Width="120px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litSubSubItem" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </asp:Panel>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0" ItemStyle-Font-Bold="true"
                                                                                            ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litHelp" runat="server" Text="4311"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-BorderWidth="1" ItemStyle-Width="121px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSubItem" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text="411"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litLinha" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="4"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px" ItemStyle-ForeColor="MediumPurple">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCustoFixo" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM CUSTO FIXO %>
                                        <% //MARGEM BRUTA %>
                                        <asp:GridView ID="gvMargemBruta" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvMargemBruta_RowDataBound"
                                            OnDataBound="gvMargemBruta_DataBound" ShowFooter="false">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        &nbsp;
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="5"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMargemBruta" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM MARGEM BRUTA %>
                                        <% //DESPESAS CTO %>
                                        <asp:GridView ID="gvDespesaCTO" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvDespesaCTO_RowDataBound"
                                            OnDataBound="gvDespesaCTO_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlDespCTOItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvDespesaCTOItem" runat="server" CellPadding="0" CellSpacing="0"
                                                                AutoGenerateColumns="false" OnDataBound="gvDespesaCTOItem_DataBound" OnRowDataBound="gvDespesaCTOItem_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                Width="18px" runat="server" />
                                                                            <asp:Panel ID="pnlDespCTOItemSub" runat="server" Style="display: none" Width="100%"
                                                                                BorderColor="Red">
                                                                                <asp:GridView ID="gvDespesaCTOItemSub" runat="server" CellPadding="0" CellSpacing="0"
                                                                                    AutoGenerateColumns="false" OnDataBound="gvDespesaCTOItemSub_DataBound" BorderColor="GradientActiveCaption"
                                                                                    OnRowDataBound="gvDespesaCTOItemSub_RowDataBound" Width="100%">
                                                                                    <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                            ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-BorderWidth="1" ItemStyle-Width="142px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSubItem" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDespesaItem" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="6"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px" ItemStyle-ForeColor="MediumPurple">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDespesa" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM DESPESAS CTO %>
                                        <% //DESPESAS LOJAS - ESPECIFICAS %>
                                        <asp:GridView ID="gvDespesaEspecifica" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvDespesaEspecifica_RowDataBound"
                                            OnDataBound="gvDespesaEspecifica_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlDespEspItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvDespesaEspecificaItem" runat="server" CellPadding="0" CellSpacing="0"
                                                                AutoGenerateColumns="false" OnDataBound="gvDespesaEspecificaItem_DataBound" OnRowDataBound="gvDespesaEspecificaItem_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                Width="18px" runat="server" />
                                                                            <asp:Panel ID="pnlDespEspItemSub" runat="server" Style="display: none" Width="100%"
                                                                                BorderColor="Red">
                                                                                <asp:GridView ID="gvDespesaEspecificaItemSub" runat="server" CellPadding="0" CellSpacing="0"
                                                                                    AutoGenerateColumns="false" OnDataBound="gvDespesaEspecificaItemSub_DataBound" BorderColor="GradientActiveCaption"
                                                                                    OnRowDataBound="gvDespesaEspecificaItemSub_RowDataBound" Width="100%">
                                                                                    <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                            ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-BorderWidth="1" ItemStyle-Width="142px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSubItem" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDespesaItem" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="7"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px" ItemStyle-ForeColor="MediumPurple">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDespesa" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM DESPESAS ESPECIFICAs %>
                                        <% //DESPESAS VENDAS %>
                                        <asp:GridView ID="gvDespesaVenda" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvDespesaVenda_RowDataBound"
                                            OnDataBound="gvDespesaVenda_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlDespVendaItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvDespesaVendaItem" runat="server" CellPadding="0" CellSpacing="0"
                                                                AutoGenerateColumns="false" OnDataBound="gvDespesaVendaItem_DataBound" OnRowDataBound="gvDespesaVendaItem_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                Width="18px" runat="server" />
                                                                            <asp:Panel ID="pnlDespVendaItemSub" runat="server" Style="display: none" Width="100%"
                                                                                BorderColor="Red">
                                                                                <asp:GridView ID="gvDespesaVendaItemSub" runat="server" CellPadding="0" CellSpacing="0"
                                                                                    AutoGenerateColumns="false" OnDataBound="gvDespesaVendaItemSub_DataBound" BorderColor="GradientActiveCaption"
                                                                                    OnRowDataBound="gvDespesaVendaItemSub_RowDataBound" Width="100%">
                                                                                    <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                            ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-BorderWidth="1" ItemStyle-Width="142px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSubItem" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text="811"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDespesaItem" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="8"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px" ItemStyle-ForeColor="MediumPurple">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDespesa" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM DESPESAS VENDAS %>
                                        <% //DESPESAS ADM %>
                                        <asp:GridView ID="gvDespesaAdm" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvDespesaAdm_RowDataBound"
                                            OnDataBound="gvDespesaAdm_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlDespAdmItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvDespesaAdmItem" runat="server" CellPadding="0" CellSpacing="0"
                                                                AutoGenerateColumns="false" OnDataBound="gvDespesaAdmItem_DataBound" OnRowDataBound="gvDespesaAdmItem_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                Width="18px" runat="server" />
                                                                            <asp:Panel ID="pnlDespAdmItemSub" runat="server" Style="display: none" Width="100%"
                                                                                BorderColor="Red">
                                                                                <asp:GridView ID="gvDespesaAdmItemSub" runat="server" CellPadding="0" CellSpacing="0"
                                                                                    AutoGenerateColumns="false" OnDataBound="gvDespesaAdmItemSub_DataBound" BorderColor="GradientActiveCaption"
                                                                                    OnRowDataBound="gvDespesaAdmItemSub_RowDataBound" Width="100%">
                                                                                    <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="leandro" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                                            ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                                    Width="18px" runat="server" />
                                                                                                <asp:Panel ID="pnlDespAdmItemSubSub" runat="server" Style="display: none" Width="100%"
                                                                                                    BorderColor="Red">
                                                                                                    <asp:GridView ID="gvDespesaAdmItemSubSub" runat="server" CellPadding="0" CellSpacing="0"
                                                                                                        AutoGenerateColumns="false" OnDataBound="gvDespesaAdmItemSubSub_DataBound" BorderColor="GradientActiveCaption"
                                                                                                        OnRowDataBound="gvDespesaAdmItemSubSub_RowDataBound" Width="100%">
                                                                                                        <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                                                ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                                                                ItemStyle-BorderWidth="1" ItemStyle-Width="120px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litSubSubItem" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                                                ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </asp:Panel>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                            ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-BorderWidth="1" ItemStyle-Width="121px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSubItem" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text="911"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDespesaItem" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="9"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px" ItemStyle-ForeColor="MediumPurple">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDespesa" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM DESPESAS ADM %>
                                        <% //EBTIDA %>
                                        <asp:GridView ID="gvEbtida" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvEbtida_RowDataBound"
                                            OnDataBound="gvEbtida_DataBound" ShowFooter="false">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        &nbsp;
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="10"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litEbtida" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM EBTIDA %>
                                        <% // DESPESAS EVENTUAIS %>
                                        <asp:GridView ID="gvDespesaEventual" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvDespesaEventual_RowDataBound"
                                            OnDataBound="gvDespesaEventual_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlEventualItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvDespesaEventualItem" runat="server" CellPadding="0" CellSpacing="0"
                                                                AutoGenerateColumns="false" OnDataBound="gvDespesaEventualItem_DataBound"
                                                                OnRowDataBound="gvDespesaEventualItem_RowDataBound" Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="164px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litLinha" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="11"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px" ItemStyle-ForeColor="MediumPurple">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litEventual" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM DESPESAS EVENTUAIS %>
                                        <% // DEPRECIACAO E AMORTIZACAO  / SERVICOS NAO RECORRENTES %>
                                        <asp:GridView ID="gvDepreciacaoAmortizacao" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvDepreciacaoAmortizacao_RowDataBound"
                                            OnDataBound="gvDepreciacaoAmortizacao_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlDepreItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvDepreciacaoAmortizacaoItem" runat="server" CellPadding="0" CellSpacing="0"
                                                                AutoGenerateColumns="false" OnDataBound="gvDepreciacaoAmortizacaoItem_DataBound"
                                                                OnRowDataBound="gvDepreciacaoAmortizacaoItem_RowDataBound" Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="164px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litLinha" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="12"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px" ItemStyle-ForeColor="MediumPurple">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDepreciacao" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM DEPRECIACAO E AMORTIZACAO / SERVICOS NAO RECORRENTES %>
                                        <% //LUCRO BRUTO %>
                                        <asp:GridView ID="gvLucroBruto" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvLucroBruto_RowDataBound"
                                            OnDataBound="gvLucroBruto_DataBound" ShowFooter="false">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        &nbsp;
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="13"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litLucroBruto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% // FIM LUCRO BRUTO %>
                                        <% // RECEITAS FINANCEIRAS %>
                                        <asp:GridView ID="gvReceitaFin" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvReceitaFin_RowDataBound"
                                            OnDataBound="gvReceitaFin_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlReceitaItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvReceitaFinItem" runat="server" CellPadding="0" CellSpacing="0"
                                                                AutoGenerateColumns="false" OnDataBound="gvReceitaFinItem_DataBound" OnRowDataBound="gvReceitaFinItem_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                Width="18px" runat="server" />
                                                                            <asp:Panel ID="pnlgvReceitaFinSub" runat="server" Style="display: none" Width="100%"
                                                                                BorderColor="Red">
                                                                                <asp:GridView ID="gvReceitaFinItemSub" runat="server" CellPadding="0" CellSpacing="0"
                                                                                    AutoGenerateColumns="false" OnDataBound="gvReceitaFinItemSub_DataBound" BorderColor="GradientActiveCaption"
                                                                                    OnRowDataBound="gvReceitaFinItemSub_RowDataBound" Width="100%">
                                                                                    <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                            ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-BorderWidth="1" ItemStyle-Width="142px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSubItem" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text="1411"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDespesaItem" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="14"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px" ItemStyle-ForeColor="MediumPurple">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDespesa" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM RECEITAS FINANCEIRAS %>
                                        <% // DESPESAS FINANCEIRAS %>
                                        <asp:GridView ID="gvDespesaFin" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvDespesaFin_RowDataBound"
                                            OnDataBound="gvDespesaFin_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlDespesaItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvDespesaFinItem" runat="server" CellPadding="0" CellSpacing="0"
                                                                AutoGenerateColumns="false" OnDataBound="gvDespesaFinItem_DataBound" OnRowDataBound="gvDespesaFinItem_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                Width="18px" runat="server" />
                                                                            <asp:Panel ID="pnlgvDespesaFinSub" runat="server" Style="display: none" Width="100%"
                                                                                BorderColor="Red">
                                                                                <asp:GridView ID="gvDespesaFinItemSub" runat="server" CellPadding="0" CellSpacing="0"
                                                                                    AutoGenerateColumns="false" OnDataBound="gvDespesaFinItemSub_DataBound" BorderColor="GradientActiveCaption"
                                                                                    OnRowDataBound="gvDespesaFinItemSub_RowDataBound" Width="100%">
                                                                                    <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                                            ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-BorderWidth="1" ItemStyle-Width="142px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSubItem" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                                            ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text="8555"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDespesaItem" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="85"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px" ItemStyle-ForeColor="MediumPurple">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDespesa" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM DESPESAS FINANCEIRAS %>
                                        <% //LAIR %>
                                        <asp:GridView ID="gvLAIR" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvLAIR_RowDataBound"
                                            OnDataBound="gvLAIR_DataBound" ShowFooter="false">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        &nbsp;
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="15"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litLAIR" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% // FIM LAIR %>
                                        <% // IMPOSTOS E TAXAS%>
                                        <asp:GridView ID="gvImpostoTaxa" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvImpostoTaxa_RowDataBound"
                                            OnDataBound="gvImpostoTaxa_DataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                            Width="18px" runat="server" />
                                                        <asp:Panel ID="pnlImpostoItem" runat="server" Style="display: none" Width="100%">
                                                            <asp:GridView ID="gvImpostoTaxaItem" runat="server" CellPadding="0" CellSpacing="0"
                                                                AutoGenerateColumns="false" OnDataBound="gvImpostoTaxaItem_DataBound" OnRowDataBound="gvImpostoTaxaItem_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption"
                                                                    Height="5px" HorizontalAlign="Center"></HeaderStyle>
                                                                <RowStyle BorderColor="GradientActiveCaption" Height="22px" BorderWidth="1" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                                        ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHelp" runat="server" Text=""></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="164px" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litLinha" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="16"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px" ItemStyle-ForeColor="MediumPurple">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litImpostoTaxa" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% //FIM IMPOSTOS E TAXAS %>
                                        <% //RESULTADO %>
                                        <asp:GridView ID="gvResultado" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="GradientActiveCaption" BorderColor="GradientActiveCaption" OnRowDataBound="gvResultado_RowDataBound"
                                            OnDataBound="gvResultado_DataBound" ShowFooter="true">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></FooterStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                    <ItemTemplate>
                                                        &nbsp;
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="15px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHelp" runat="server" Text="17"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="165px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litResultado" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <% // FIM RESULTADO%>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <asp:HiddenField ID="hidFilial" runat="server" Value="" />
                <asp:GridView ID="gvDREExcel" runat="server" Width="100%" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="Id" />
                        <asp:BoundField DataField="GRUPO" HeaderText="Grupo" />
                        <asp:BoundField DataField="LINHA" HeaderText="Linha" />
                        <asp:BoundField DataField="TIPO" HeaderText="Tipo" />
                        <asp:BoundField DataField="FILIAL" HeaderText="Filial" />
                        <asp:BoundField DataField="ANO" HeaderText="Ano" />
                        <asp:BoundField DataField="JANEIRO" HeaderText="Janeiro" />
                        <asp:BoundField DataField="FEVEREIRO" HeaderText="Fevereiro" />
                        <asp:BoundField DataField="MARCO" HeaderText="Marco" />
                        <asp:BoundField DataField="ABRIL" HeaderText="Abril" />
                        <asp:BoundField DataField="MAIO" HeaderText="Maio" />
                        <asp:BoundField DataField="JUNHO" HeaderText="Junho" />
                        <asp:BoundField DataField="JULHO" HeaderText="Julho" />
                        <asp:BoundField DataField="AGOSTO" HeaderText="Agosto" />
                        <asp:BoundField DataField="SETEMBRO" HeaderText="Setembro" />
                        <asp:BoundField DataField="OUTUBRO" HeaderText="Outubro" />
                        <asp:BoundField DataField="NOVEMBRO" HeaderText="Novembro" />
                        <asp:BoundField DataField="DEZEMBRO" HeaderText="Dezembro" />
                        <asp:BoundField DataField="TOTAL" HeaderText="Total" />
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
