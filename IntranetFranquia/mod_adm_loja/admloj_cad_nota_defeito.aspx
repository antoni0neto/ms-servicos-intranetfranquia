<%@ Page Title="Cadastro NF Defeito" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="admloj_cad_nota_defeito.aspx.cs" Inherits="Relatorios.admloj_cad_nota_defeito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function doClick(buttonName, e) {
            //the purpose of this function is to allow the enter key to 
            //point to the correct button to click.
            var key;

            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(buttonName);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labTituloMenu" runat="server" Text="Cadastro NF Defeito"></asp:Label></span>
        <div style="float: right; padding: 0;">
            <a href="admloj_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Cadastro NF Defeito</legend>
            <table class="style1">
                <tr>
                    <td valign="top">
                        <fieldset class="login" style="height: 180px;">
                            <div>
                                <label>Filial:</label>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="21px" Width="200px"></asp:DropDownList>
                            </div>
                            <div>
                                <br />
                                <asp:Button runat="server" ID="btContinue" Text="Continue" Width="100px" OnClick="btContinue_Click" />
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login" style="height: 180px;">
                            <div>
                                <label>Produto:</label>
                                <asp:TextBox ID="txtCodigoProduto" runat="server" CssClass="textEntry" MaxLength="10" Height="20px" Width="100px"></asp:TextBox>&nbsp;
                                <asp:Button runat="server" ID="btBuscar" Text="Buscar" Width="100px" OnClick="btBuscar_Click" ValidationGroup="codigo" />
                            </div>
                            <div>
                                <label>Descrição:</label>
                                <asp:TextBox ID="txtDescricaoProduto" runat="server" CssClass="textEntry" MaxLength="50" Height="20px" Width="300px"></asp:TextBox>
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login" style="height: 180px;">
                            <div>
                                <label>Cor:</label>
                                <asp:DropDownList runat="server" ID="ddlProdutoCor" DataValueField="COR_PRODUTO" DataTextField="DESC_COR_PRODUTO" Height="22px" Width="200px"></asp:DropDownList>
                            </div>
                            <div>
                                <label>Tamanho:</label>
                                <asp:DropDownList runat="server" ID="ddlTamanho" DataValueField="CODIGO" DataTextField="DESCRICAO" Height="22px" Width="200px"></asp:DropDownList>
                            </div>
                            <div>
                                <label>Origem do Defeito:</label>
                                <asp:DropDownList runat="server" ID="ddlOrigemDefeito" DataValueField="CODIGO" DataTextField="DESCRICAO_ORIGEM_DEFEITO" Height="22px" Width="200px"></asp:DropDownList>
                            </div>
                            <div>
                                <label>Defeito:</label>
                                <asp:DropDownList runat="server" ID="ddlDefeito" DataValueField="CODIGO" DataTextField="DESCRICAO_DEFEITO" Height="22px" Width="200px"></asp:DropDownList>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Button runat="server" ID="btSalvar" Text="Salvar" OnClick="btSalvar_Click" Width="120px" Enabled="False" />&nbsp;
                <asp:Label runat="server" ID="labErro" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <div class="rounded_corners">
            <asp:GridView ID="GridViewNotaRetiradaItem" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                Style="background: white" OnRowDataBound="GridViewNotaRetiradaItem_RowDataBound" ShowFooter="true">
                <HeaderStyle BackColor="GradientActiveCaption" />
                <FooterStyle BackColor="GradientActiveCaption" />
                <Columns>
                    <asp:BoundField DataField="ITEM_PRODUTO" HeaderText="Item" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="Origem do Defeito" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralOrigemDefeito" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Defeito" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralDefeito" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Data do Lançamento" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="litDataLancamento" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="FILIAL_ORIGEM" HeaderText="Filial Origem" HeaderStyle-HorizontalAlign="Left" />

                    <asp:TemplateField HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Button runat="server" ID="btExcluir" Text="Excluir" OnClick="btExcluir_Click" Width="70px" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoNotaRetiradaItem" Value="0" />
</asp:Content>
