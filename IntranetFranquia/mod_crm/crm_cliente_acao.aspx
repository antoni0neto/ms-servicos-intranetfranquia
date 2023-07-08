<%@ Page Title="Clientes - Ação" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="crm_cliente_acao.aspx.cs" Inherits="Relatorios.crm_cliente_acao"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
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
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript">
        function openwindow(l) {
            window.open(l, "CRM_CLIX1", "menubar=1,resizable=0,width=1600,height=800");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de CRM&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Clientes - Ação</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Clientes - Ação"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Ação</td>
                        <td>CPF</td>
                        <td>Nome</td>
                        <td>Aberto</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 310px;">
                            <asp:DropDownList ID="ddlAcao" runat="server" Width="304px" Height="21px" DataTextField="ACAO" DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 200px;">
                            <asp:TextBox ID="txtCPF" runat="server" Width="190px"></asp:TextBox>
                        </td>
                        <td style="width: 200px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="190px"></asp:TextBox>
                        </td>
                        <td style="width: 170px;">
                            <asp:DropDownList ID="ddlAberto" runat="server" Width="164px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="120px" OnClick="btBuscar_Click" />&nbsp;&nbsp;<asp:Label ID="labErro" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCliente" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvCliente_RowDataBound" OnDataBound="gvCliente_DataBound" ShowFooter="true"
                                    OnSorting="gvCliente_Sorting" AllowSorting="true"
                                    DataKeyNames="">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Size="Smaller"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                    <PagerStyle HorizontalAlign="Left" CssClass="pageStyl" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-Width="25px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbrirCliente" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CPF" HeaderStyle-Width="95px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="CPF">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCPF" runat="server" Text='<%# Bind("CPF") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cliente" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="NOME">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCliente" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="CODIGO_FILIAL">
                                            <ItemTemplate>
                                                <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Telefone" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="TELEFONE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litTelefone" runat="server" Text='<%# Bind("TELEFONE") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btWapp1" runat="server" Width="11px" ImageUrl="~/Image/whatsapp.png" OnClick="btWapp1_Click" CommandArgument='<%# Bind("TELEFONE") %>' ToolTip="Número de Telefone" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Celular" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="CELULAR">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCelular" runat="server" Text='<%# Bind("CELULAR") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btWapp2" runat="server" Width="11px" ImageUrl="~/Image/whatsapp.png" OnClick="btWapp2_Click" CommandArgument='<%# Bind("CELULAR") %>' ToolTip="Número de Celular" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Observação" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="OBSERVACAO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litObs" runat="server" Text='<%# Bind("OBSERVACAO") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Data Baixa" HeaderStyle-Width="125px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="DATA_BAIXA">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDataBaixa" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="">
                                            <ItemTemplate>
                                                <asp:Button ID="btBaixar" runat="server" Width="75px" Text=">>" OnClick="btBaixar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
