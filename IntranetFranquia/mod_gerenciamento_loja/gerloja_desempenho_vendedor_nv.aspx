<%@ Page Title="Desempenho Vendedor Cliente" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_desempenho_vendedor_nv.aspx.cs" Inherits="Relatorios.gerloja_desempenho_vendedor_nv" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
        function openwindow(l) {
            window.open(l, "VENDEDOR_TICKETS", "menubar=1,resizable=0,width=1300,height=700");
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always" EnableViewState="true">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho Vendedor Cliente&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a id="hrefVoltar" runat="server" href="gerloja_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Desempenho Vendedor Cliente</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labVendedor" runat="server" Text="Vendedor"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;">
                                <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="124px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 280px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="274px" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFuncionario" DataValueField="CODIGO" DataTextField="NOME"
                                    Height="22px" Width="394px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td colspan="3">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvVendedor" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white; width: 6400px;" OnRowDataBound="gvVendedor_RowDataBound"
                                        OnDataBound="gvVendedor_DataBound" ShowFooter="true"
                                        OnSorting="gvVendedor_Sorting" AllowSorting="true">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" Font-Size="Smaller" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Vendedor" SortExpression="NOME_VENDEDOR" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNomeVendedor" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Admissão" SortExpression="TEMPO_CASA" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTempoCasa" runat="server" Text='<%# Bind("TEMPO_CASA") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*JANEIRO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Jan (AT)" SortExpression="AT_JANEIRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_JANEIRO" runat="server" Text='<%# Bind("AT_JANEIRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Jan (Novo)" SortExpression="NV_JANEIRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_JANEIRO" runat="server" Text='<%# Bind("NV_JANEIRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Jan (Novo)" SortExpression="NV_JANEIRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_JANEIRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Jan (Rec)" SortExpression="RC_JANEIRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_JANEIRO" runat="server" Text='<%# Bind("RC_JANEIRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Jan (Rec)" SortExpression="RC_JANEIRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_JANEIRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*FEVEREIRO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Fev (AT)" SortExpression="AT_FEVEREIRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_FEVEREIRO" runat="server" Text='<%# Bind("AT_FEVEREIRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Fev (Novo)" SortExpression="NV_FEVEREIRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_FEVEREIRO" runat="server" Text='<%# Bind("NV_FEVEREIRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Fev (Novo)" SortExpression="NV_FEVEREIRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_FEVEREIRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Fev (Rec)" SortExpression="RC_FEVEREIRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_FEVEREIRO" runat="server" Text='<%# Bind("RC_FEVEREIRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Fev (Rec)" SortExpression="RC_FEVEREIRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_FEVEREIRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*MARCO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Mar (AT)" SortExpression="AT_MARCO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_MARCO" runat="server" Text='<%# Bind("AT_MARCO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Mar (Novo)" SortExpression="NV_MARCO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_MARCO" runat="server" Text='<%# Bind("NV_MARCO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Mar (Novo)" SortExpression="NV_MARCO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_MARCO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Mar (Rec)" SortExpression="RC_MARCO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_MARCO" runat="server" Text='<%# Bind("RC_MARCO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Mar (Rec)" SortExpression="RC_MARCO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_MARCO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*ABRIL*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Abr (AT)" SortExpression="AT_ABRIL" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_ABRIL" runat="server" Text='<%# Bind("AT_ABRIL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Abr (Novo)" SortExpression="NV_ABRIL" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_ABRIL" runat="server" Text='<%# Bind("NV_ABRIL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Abr (Novo)" SortExpression="NV_ABRIL_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_ABRIL_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Abr (Rec)" SortExpression="RC_ABRIL" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_ABRIL" runat="server" Text='<%# Bind("RC_ABRIL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Abr (Rec)" SortExpression="RC_ABRIL_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_ABRIL_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*MAIO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Mai (AT)" SortExpression="AT_MAIO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_MAIO" runat="server" Text='<%# Bind("AT_MAIO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Mai (Novo)" SortExpression="NV_MAIO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_MAIO" runat="server" Text='<%# Bind("NV_MAIO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Mai (Novo)" SortExpression="NV_MAIO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_MAIO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Mai (Rec)" SortExpression="RC_MAIO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_MAIO" runat="server" Text='<%# Bind("RC_MAIO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Mai (Rec)" SortExpression="RC_MAIO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_MAIO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*JUNHO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Jun (AT)" SortExpression="AT_JUNHO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_JUNHO" runat="server" Text='<%# Bind("AT_JUNHO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Jun (Novo)" SortExpression="NV_JUNHO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_JUNHO" runat="server" Text='<%# Bind("NV_JUNHO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Jun (Novo)" SortExpression="NV_JUNHO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_JUNHO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Jun (Rec)" SortExpression="RC_JUNHO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_JUNHO" runat="server" Text='<%# Bind("RC_JUNHO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Jun (Rec)" SortExpression="RC_JUNHO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_JUNHO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*JULHO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Jul (AT)" SortExpression="AT_JULHO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_JULHO" runat="server" Text='<%# Bind("AT_JULHO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Jul (Novo)" SortExpression="NV_JULHO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_JULHO" runat="server" Text='<%# Bind("NV_JULHO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Jul (Novo)" SortExpression="NV_JULHO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_JULHO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Jul (Rec)" SortExpression="RC_JULHO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_JULHO" runat="server" Text='<%# Bind("RC_JULHO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Jul (Rec)" SortExpression="RC_JULHO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_JULHO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*AGOSTO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Ago (AT)" SortExpression="AT_AGOSTO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_AGOSTO" runat="server" Text='<%# Bind("AT_AGOSTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Ago (Novo)" SortExpression="NV_AGOSTO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_AGOSTO" runat="server" Text='<%# Bind("NV_AGOSTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Ago (Novo)" SortExpression="NV_AGOSTO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_AGOSTO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Ago (Rec)" SortExpression="RC_AGOSTO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_AGOSTO" runat="server" Text='<%# Bind("RC_AGOSTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Ago (Rec)" SortExpression="RC_AGOSTO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_AGOSTO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*SETEMBRO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Set (AT)" SortExpression="AT_SETEMBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_SETEMBRO" runat="server" Text='<%# Bind("AT_SETEMBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Set (Novo)" SortExpression="NV_SETEMBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_SETEMBRO" runat="server" Text='<%# Bind("NV_SETEMBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Set (Novo)" SortExpression="NV_SETEMBRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_SETEMBRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Set (Rec)" SortExpression="RC_SETEMBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_SETEMBRO" runat="server" Text='<%# Bind("RC_SETEMBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Set (Rec)" SortExpression="RC_SETEMBRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_SETEMBRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*OUTUBRO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Out (AT)" SortExpression="AT_OUTUBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_OUTUBRO" runat="server" Text='<%# Bind("AT_OUTUBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Out (Novo)" SortExpression="NV_OUTUBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_OUTUBRO" runat="server" Text='<%# Bind("NV_OUTUBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Out (Novo)" SortExpression="NV_OUTUBRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_OUTUBRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Out (Rec)" SortExpression="RC_OUTUBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_OUTUBRO" runat="server" Text='<%# Bind("RC_OUTUBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Out (Rec)" SortExpression="RC_OUTUBRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_OUTUBRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*NOVEMBRO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nov (AT)" SortExpression="AT_NOVEMBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_NOVEMBRO" runat="server" Text='<%# Bind("AT_NOVEMBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nov (Novo)" SortExpression="NV_NOVEMBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_NOVEMBRO" runat="server" Text='<%# Bind("NV_NOVEMBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Nov (Novo)" SortExpression="NV_NOVEMBRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_NOVEMBRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nov (Rec)" SortExpression="RC_NOVEMBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_NOVEMBRO" runat="server" Text='<%# Bind("RC_NOVEMBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Nov (Rec)" SortExpression="RC_NOVEMBRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_NOVEMBRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*DEZEMBRO*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Dez (AT)" SortExpression="AT_DEZEMBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_DEZEMBRO" runat="server" Text='<%# Bind("AT_DEZEMBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Dez (Novo)" SortExpression="NV_DEZEMBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_DEZEMBRO" runat="server" Text='<%# Bind("NV_DEZEMBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Dez (Novo)" SortExpression="NV_DEZEMBRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_DEZEMBRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Dez (Rec)" SortExpression="RC_DEZEMBRO" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_DEZEMBRO" runat="server" Text='<%# Bind("RC_DEZEMBRO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Dez (Rec)" SortExpression="RC_DEZEMBRO_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_DEZEMBRO_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--/*TOTAL*/--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Total (AT)" SortExpression="AT_TOTAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAT_TOTAL" runat="server" Text='<%# Bind("AT_TOTAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Total (Novo)" SortExpression="NV_TOTAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_TOTAL" runat="server" Text='<%# Bind("NV_TOTAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Total (Novo)" SortExpression="NV_TOTAL_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNV_TOTAL_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Total (Rec)" SortExpression="RC_TOTAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_TOTAL" runat="server" Text='<%# Bind("RC_TOTAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="% Total (Rec)" SortExpression="RC_TOTAL_PORC" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="90px" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Cornsilk">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRC_TOTAL_PORC" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
