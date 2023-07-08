<%@ Page Title="Apuração PIS e COFINS" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fisc_apuracao_imposto.aspx.cs" Inherits="Relatorios.fisc_apuracao_imposto" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always" EnableViewState="true">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Apuração PIS e COFINS&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a id="hrefVoltar" runat="server" href="#" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Apuração PIS e COFINS</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labEmpresa" runat="server" Text="Empresa"></asp:Label>
                                <asp:HiddenField ID="hidCTBApuracaoEmpresa" runat="server" />
                                <asp:HiddenField ID="hidCodigoPerfilUsuario" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labMes" runat="server" Text="Mês"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 350px;">
                                <asp:DropDownList runat="server" ID="ddlEmpresa" DataValueField="CODIGO_EMPRESA" DataTextField="NOME"
                                    Height="22px" Width="344px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="144px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlMes" Height="22px" Width="144px">
                                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                    <asp:ListItem Value="01" Text="Janeiro"></asp:ListItem>
                                    <asp:ListItem Value="02" Text="Fevereiro"></asp:ListItem>
                                    <asp:ListItem Value="03" Text="Março"></asp:ListItem>
                                    <asp:ListItem Value="04" Text="Abril"></asp:ListItem>
                                    <asp:ListItem Value="05" Text="Maio"></asp:ListItem>
                                    <asp:ListItem Value="06" Text="Junho"></asp:ListItem>
                                    <asp:ListItem Value="07" Text="Julho"></asp:ListItem>
                                    <asp:ListItem Value="08" Text="Agosto"></asp:ListItem>
                                    <asp:ListItem Value="09" Text="Setembro"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="Dezembro"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btGerar" runat="server" Text="Gerar" OnClick="btGerar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btSair" runat="server" Text="Sair" OnClick="btSair_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="6">
                                <div id="divApuracao" runat="server" visible="false">
                                    <fieldset>
                                        <legend><b>FATURAMENTO / RECEITA BRUTA</b></legend>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -7px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labReceita" runat="server" Text="Receita"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labReceitaValor" runat="server" Text="Valor"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 678px;">
                                                    <asp:DropDownList runat="server" ID="ddlReceitaItem" DataValueField="CODIGO" DataTextField="ITEM"
                                                        Height="22px" Width="672px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 206px;">
                                                    <asp:TextBox ID="txtReceitaValor" runat="server" Width="196px" CssClass="alinharDireita"
                                                        onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btIncluirReceita" runat="server" Text=">>" OnClick="btIncluirReceita_Click" Width="80px" />&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="labErroReceita" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvReceita" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvApuracao_RowDataBound" OnDataBound="gvApuracao_DataBound" ShowFooter="true"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litSinal" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="500px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click" ToolTip="Excluir Item" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                    <fieldset>
                                        <legend><b>EXCLUSÕES DA BASE DE CÁLCULO</b></legend>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -7px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labExclusao" runat="server" Text="Exclusão"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labExclusaoValor" runat="server" Text="Valor"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 678px;">
                                                    <asp:DropDownList runat="server" ID="ddlExclusaoItem" DataValueField="CODIGO" DataTextField="ITEM"
                                                        Height="22px" Width="672px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 206px;">
                                                    <asp:TextBox ID="txtExclusaoValor" runat="server" Width="196px" CssClass="alinharDireita"
                                                        onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btIncluirExclusao" runat="server" Text=">>" OnClick="btIncluirExclusao_Click" Width="80px" />&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="labErroExclusao" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvExclusao" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvApuracao_RowDataBound" OnDataBound="gvApuracao_DataBound" ShowFooter="true"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litSinal" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="500px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click" ToolTip="Excluir Item" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                    <fieldset>
                                        <legend><b>ISENÇÕES</b></legend>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -7px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labIsencao" runat="server" Text="Isenção"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labIsencaoValor" runat="server" Text="Valor"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 678px;">
                                                    <asp:DropDownList runat="server" ID="ddlIsencaoItem" DataValueField="CODIGO" DataTextField="ITEM"
                                                        Height="22px" Width="672px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 206px;">
                                                    <asp:TextBox ID="txtIsencaoValor" runat="server" Width="196px" CssClass="alinharDireita"
                                                        onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btIncluirIsencao" runat="server" Text=">>" OnClick="btIncluirIsencao_Click" Width="80px" />&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="labErroIsencao" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvIsencao" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvApuracao_RowDataBound" OnDataBound="gvApuracao_DataBound" ShowFooter="true"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litSinal" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="500px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click" ToolTip="Excluir Item" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                    <fieldset>
                                        <legend><b>CRÉDITOS</b></legend>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -7px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labCredito" runat="server" Text="Crédito"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labCreditoValor" runat="server" Text="Valor"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 678px;">
                                                    <asp:DropDownList runat="server" ID="ddlCreditoItem" DataValueField="CODIGO" DataTextField="ITEM"
                                                        Height="22px" Width="672px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 206px;">
                                                    <asp:TextBox ID="txtCreditoValor" runat="server" Width="196px" CssClass="alinharDireita"
                                                        onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btIncluirCredito" runat="server" Text=">>" OnClick="btIncluirCredito_Click" Width="80px" />&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="labErroCredito" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvCredito" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvApuracao_RowDataBound" OnDataBound="gvApuracao_DataBound" ShowFooter="true"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litSinal" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="500px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click" ToolTip="Excluir Item" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                    <fieldset>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -7px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labImposto" runat="server" Text="Imposto"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 210px;">
                                                    <asp:DropDownList ID="ddlImposto" runat="server" DataValueField="TIPO" DataTextField="TIPO"
                                                        Height="22px" Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="labErroCalcular" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;<asp:Button ID="btCalcular" runat="server" Text="Calcular" OnClick="btCalcular_Click" Width="100px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
