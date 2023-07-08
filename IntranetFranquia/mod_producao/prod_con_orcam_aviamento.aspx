<%@ Page Title="Orçamento de Aviamentos" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="prod_con_orcam_aviamento.aspx.cs" Inherits="Relatorios.prod_con_orcam_aviamento"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita
        {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Orçamento
                    de Aviamentos</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>Orçamento de Aviamentos</legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Aviamento
                        </td>
                        <td>
                            HB
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 210px;">
                            <asp:DropDownList ID="ddlAviamento" runat="server" Width="204px" Height="22px" DataValueField="CODIGO"
                                DataTextField="DESCRICAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:TextBox ID="txtHB" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                Width="170px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btComprarExtra" runat="server" Width="120px" Text="Comprar Extra"
                                OnClick="btComprarExtra_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btBuscarAviamentos" runat="server" Text="Buscar" OnClick="btBuscarAviamentos_Click"
                                Width="100px" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="padding: 0;">
                            <br />
                            <div style="border: 0px solid #000;">
                                <div style="float: right; margin-top: -10px; width: 120px; border: 0px solid black;
                                    padding: 0;">
                                    <asp:CheckBox runat="server" ID="cbMarcarTodos" Text="" TextAlign="Right" AutoPostBack="true"
                                        OnCheckedChanged="cbMarcarTodos_CheckedChanged" /><asp:Label ID="labMarcarTodos"
                                            runat="server" Text="Marcar Todos"></asp:Label>
                                </div>
                                <br />
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvAviamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamento_RowDataBound"
                                        DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbCheck" runat="server" Height="19px" AutoPostBack="true" OnCheckedChanged="cbCheck_CheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="HB" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litHB" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Aviamento" HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAviamento" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" HeaderStyle-Width="450px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="QTDE" HeaderText="Quantidade Prevista" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Medida">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litMedida" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Quantidade Real" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeReal" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: right;">
                            <asp:Label ID="labErroImpressao" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <div id="divQtdeSelecionada" runat="server">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Aviamentos selecionados...
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvAviamentoSelecionado" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamentoSelecionado_RowDataBound"
                                        OnDataBound="gvAviamentoSelecionado_DataBound" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="HB" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litHB" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Aviamento" HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAviamento" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" HeaderStyle-Width="600px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="QTDE" HeaderText="Quantidade Prevista" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Button ID="btFechar" runat="server" Text="Preencher Orçamento" OnClick="btFechar_Click"
                                    Width="150px" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
