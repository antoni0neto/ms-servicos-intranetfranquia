<%@ Page Title="Cadastro de Parâmetros de Conta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="contabil_cad_param_folha.aspx.cs" Inherits="Relatorios.mod_financeiro.contabil_cad_param_folha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharEsquerda {
            text-align: left;
        }
    </style>

    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript" src="../js/fixheader/jquery.freezeheader.js"></script>

    <script type="text/javascript">

        function travaHeaderGrid() {
            $("#<%=gvParamContab.ClientID %>")
              .thfloat();
            //.thfloat({
            //    side: "foot"
            //});
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Conciliação&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Parâmetros de Conta</span>
        <div style="float: right; padding: 0;">
            <a href="contabil_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset>
            <legend>Cadastro de Parâmetros de Conta</legend>

            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <table border="0" width="832px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labEvento" runat="server" Text="Evento"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labContaDebito" runat="server" Text="Conta Débito"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labContaCredito" runat="server" Text="Conta Crédito"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 204px;">
                                <asp:DropDownList ID="ddlEvento" DataValueField="COD_EVENTO" DataTextField="DESCRICAO" Height="21px" runat="server" Width="198px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 204px;">
                                <asp:DropDownList ID="ddlContaDebito" AutoPostBack="true" DataValueField="CONTA_CONTABIL" DataTextField="DESC_CONTA" OnSelectedIndexChanged="ddlContaDebito_SelectedIndexChanged" Height="21px" runat="server" Width="198px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 204px;">
                                <asp:DropDownList ID="ddlContaCredito" AutoPostBack="true" DataValueField="CONTA_CONTABIL" DataTextField="DESC_CONTA" Height="21px" runat="server" Width="198px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 110px;">
                                <asp:Button runat="server" ID="btIncluir" CommandArgument="I" Text="Incluir" OnClick="btIncluir_Click"
                                    Width="100px" Enabled="true" />
                            </td>
                            <td style="width: 110px;">
                                <div style="float: right; margin-right: 0px;">
                                    <asp:Button runat="server" ID="btCancelar" Width="100px" Text="Cancelar" OnClick="btCancelar_Click"
                                        Enabled="true" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <div>
                        <div class="rounded_corners">
                            <asp:GridView ID="gvParamContab" runat="server" AutoGenerateColumns="False" ShowFooter="true" AllowSorting="true" OnSorting="gvParamContab_Sorting"
                                ForeColor="#333333" Style="background: white; width: 1450px; border-collapse: collapse;">
                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Evento" HeaderStyle-HorizontalAlign="Left" SortExpression="cod_evento" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Literal ID="litEvento" Text='<%#Eval("COD_EVENTO") + " - " + Eval("DESCRICAO") %>' runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Conta de Débito" HeaderStyle-HorizontalAlign="Left" SortExpression="conta_debito" HeaderStyle-Width="400px" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Literal ID="litContaDebito" Text='<%#Eval("CONTA_DEBITO") + " - " + Eval("DESCRICAO_DEBITO") %>' runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Conta de Crédito" HeaderStyle-HorizontalAlign="Left" SortExpression="conta_credito" HeaderStyle-Width="400px" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Literal ID="litContaCredito" Text='<%#Eval("CONTA_CREDITO") + " - " + Eval("DESCRICAO_CREDITO") %>' runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right"
                                        HeaderStyle-Width="24px">
                                        <ItemTemplate>
                                            <asp:Button ID="btExcluir" runat="server" Height="19px" Text="Excluir"
                                                CommandArgument='<%#Eval("cod_evento") + "|" + Eval("conta_debito")+ "|" + Eval("conta_credito") %>'
                                                OnClientClick="return ConfirmarExclusao();"
                                                OnClick="btExcluir_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div>
                        <br />
                        <br />
                    </div>
                    <div>
                        <div class="rounded_corners">
                            <asp:GridView ID="gvEventoSemContabil" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                ForeColor="#333333" Style="background: white; width: 1450px; border-collapse: collapse;">
                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Evento Sem Contabilização" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Literal ID="litEvento" Text='<%#Eval("COD_EVENTO") + " - " + Eval("DESCRICAO") %>' runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>
