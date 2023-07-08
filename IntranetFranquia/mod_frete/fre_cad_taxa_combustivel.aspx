<%@ Page Title="Taxas de Combustível" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="fre_cad_taxa_combustivel.aspx.cs" Inherits="Relatorios.mod_frete.fre_cad_taxa_combustivel" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript" src="../js/fixheader/jquery.freezeheader.js"></script>

    <script type="text/javascript">

        function travaHeaderGrid() {
            $("#<%=gvCadTaxaCombustivel.ClientID %>")
              .thfloat();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Frete&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Taxas de Combustível</span>
                <div style="float: right; padding: 0;">
                    <a href="fre_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Cadastro de Taxas de Combustível</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>
                                <asp:Label ID="labValorTaxa" runat="server" Text="Valor Taxa (%)"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labMes" runat="server" Text="Mês"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                            </td>                            
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            
                            <td style="width: 180px;">
                                <asp:TextBox ID="txtValorTaxa" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                    Width="170px"></asp:TextBox>
                            </td>
                            <td style="width: 140px;">
                                <asp:DropDownList ID="ddlMes" Height="21px" runat="server" Width="134px">
                                    <asp:ListItem Text="Selecione" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Janeiro" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Fevereiro" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Março" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Abril" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Maio" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Junho" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Julho" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Agosto" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Setembro" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="Outubro" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="Novembro" Value="11"></asp:ListItem>
                                    <asp:ListItem Text="Dezembro" Value="12"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:DropDownList ID="ddlAno" Height="21px" runat="server" Width="124px">                                        
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btIncluir" CommandArgument="I" Text="Incluir" OnClick="btIncluir_Click"
                                    Width="100px" Enabled="true" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btCancelar" Width="100px" Text="Cancelar" OnClick="btCancelar_Click"
                                    Enabled="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                                <asp:HiddenField ID="hidCodigo" runat="server" Value="0" />
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="5">
                            </td>
                        </tr>
                    </table>
                    <div style="width: 100%; overflow: auto; height: 450px;">
                        <div>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCadTaxaCombustivel" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VALOR_TAXA" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Valor Taxa (%)" />

                                        <asp:TemplateField HeaderText="Mês" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" 
                                            ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litMes" Text='<%#ObterNomeMes(Int32.Parse(Eval("MES").ToString())) %>' runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ANO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Ano" ItemStyle-Width="200px" />
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="55px">
                                            <ItemTemplate>
                                                <asp:Button ID="btAlterar" runat="server" Height="19px" CommandArgument='<%#Eval("CODIGO") %>' Text="Alterar" OnClick="btAlterar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="55px">
                                            <ItemTemplate>
                                                <asp:Button ID="btExcluir" runat="server" Height="19px" Text="Excluir"
                                                    CommandArgument='<%#Eval("CODIGO") %>'
                                                    OnClientClick="return ConfirmarExclusao();"
                                                    OnClick="btExcluir_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
