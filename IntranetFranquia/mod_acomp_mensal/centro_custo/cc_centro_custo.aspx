<%@ Page Title="Centro de Custo" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="cc_centro_custo.aspx.cs" Inherits="Relatorios.cc_centro_custo" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
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

    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;Centro
            de Custo&nbsp;&nbsp;>&nbsp;&nbsp;Centro de Custo&nbsp;&nbsp;</span>
        <div style="float: right; padding: 0;">
            <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset>
            <legend>Centro de Custo</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Ano
                    </td>
                    <td>
                        Grupo C.Custo
                    </td>
                    <td>
                        C.Custo
                    </td>
                    <td>
                        Filial
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 156px;">
                        <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="150px" DataTextField="ANO"
                            DataValueField="ANO">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 206px;">
                        <asp:DropDownList runat="server" ID="ddlCCustoGrupo" DataValueField="ID_GRUPO_CENTRO_CUSTO"
                            DataTextField="DESC_GRUPO_CENTRO_CUSTO" Height="22px" Width="200px" OnSelectedIndexChanged="ddlCCustoGrupo_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 206px;">
                        <asp:DropDownList runat="server" ID="ddlCCusto" DataValueField="CENTRO_CUSTO" DataTextField="DESC_CENTRO_CUSTO"
                            Height="22px" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 206px;">
                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                            Height="22px" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0;" colspan="5">
                        <fieldset>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCentroCustoGrupo" runat="server" Width="100%" AutoGenerateColumns="False"
                                    CellPadding="0" ForeColor="#333333" Style="background: white" OnRowDataBound="gvCentroCustoGrupo_RowDataBound"
                                    OnDataBound="gvCentroCustoGrupo_DataBound" ShowFooter="true" BorderWidth="1"
                                    BorderColor="GradientActiveCaption">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                    <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                    <FooterStyle Font-Bold="true" HorizontalAlign="Center" BackColor="GradientActiveCaption" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px" ItemStyle-BackColor="WhiteSmoke">
                                            <ItemTemplate>
                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                    Width="18px" runat="server" />
                                                <asp:Panel ID="pnlCentroCusto" runat="server" Style="display: none" Width="100%">
                                                    <asp:GridView ID="gvCentroCusto" runat="server" CellPadding="0" CellSpacing="0" AutoGenerateColumns="false"
                                                        OnDataBound="gvCentroCusto_DataBound" OnRowDataBound="gvCentroCusto_RowDataBound"
                                                        Width="100%" BorderColor="Red">
                                                        <HeaderStyle BackColor="GradientActiveCaption" BorderColor="WhiteSmoke" Height="5px"
                                                            HorizontalAlign="Center"></HeaderStyle>
                                                        <RowStyle BackColor="White" Height="22px" BorderWidth="1" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                        Width="18px" runat="server" />
                                                                    <asp:Panel ID="pnlContaContabil" runat="server" Style="display: none" Width="100%"
                                                                        BorderColor="Red">
                                                                        <asp:GridView ID="gvContaContabil" runat="server" CellPadding="0" CellSpacing="0"
                                                                            AutoGenerateColumns="false" OnDataBound="gvContaContabil_DataBound" BorderColor="GradientActiveCaption"
                                                                            OnRowDataBound="gvContaContabil_RowDataBound" Width="100%">
                                                                            <RowStyle BorderColor="WhiteSmoke" Height="22px" BorderWidth="1" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                                                            Width="18px" runat="server" />
                                                                                        <asp:Panel ID="pnlFilial" runat="server" Style="display: none" Width="100%" BorderColor="Red">
                                                                                            <asp:GridView ID="gvFilial" runat="server" CellPadding="0" CellSpacing="0" AutoGenerateColumns="false"
                                                                                                OnDataBound="gvFilial_DataBound" BorderColor="GradientActiveCaption" OnRowDataBound="gvFilial_RowDataBound"
                                                                                                Width="100%">
                                                                                                <RowStyle BorderColor="WhiteSmoke" Height="22px" BorderWidth="1" />
                                                                                                <Columns>
                                                                                                    <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                                                        ItemStyle-BorderWidth="1" ItemStyle-Width="120px">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Literal ID="litFilial" runat="server"></asp:Literal>
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
                                                                                <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-BorderWidth="1" ItemStyle-Width="121px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="litContaContabil" runat="server"></asp:Literal>
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
                                                            <asp:TemplateField HeaderText="" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left"
                                                                ItemStyle-Width="143px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCentroCusto" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                ItemStyle-BorderWidth="1px">
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
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="165px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCentroCustoGrupo" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1"
                                            ItemStyle-Width="90px">
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
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
