<%@ Page Title="DRE - Transporte" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="dre_transporte.aspx.cs" Inherits="Relatorios.dre_transporte" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../../js/js.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always" EnableViewState="true">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;DRE&nbsp;&nbsp;>&nbsp;&nbsp;DRE - Transporte&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>DRE - Transporte</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px;">
                                <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="100px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />
                                &nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="6">
                                <fieldset>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvTransporte" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                            Style="background: white" OnRowDataBound="gvTransporte_RowDataBound" OnDataBound="gvTransporte_DataBound"
                                            ShowFooter="true" DataKeyNames="CODIGO_FILIAL">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <EditItemTemplate>
                                                        <asp:ImageButton ID="btSair" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/cancel.png"
                                                            OnClick="btSair_Click" ToolTip="Sair" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btEditar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/edit.jpg"
                                                            OnClick="btEditar_Click" ToolTip="Editar" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFilial" runat="server" Text='<%#Bind("FILIAL") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Janeiro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtJaneiro" runat="server" Width="85px" Text='<%# Bind("JANEIRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server" Text='<%# Eval("JANEIRO", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fevereiro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtFevereiro" runat="server" Width="85px" Text='<%# Bind("FEVEREIRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server" Text='<%# Eval("FEVEREIRO", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Março" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtMarco" runat="server" Width="85px" Text='<%# Bind("MARCO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server" Text='<%# Eval("MARCO", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Abril" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtAbril" runat="server" Width="85px" Text='<%# Bind("ABRIL") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server" Text='<%# Eval("ABRIL", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maio" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtMaio" runat="server" Width="85px" Text='<%# Bind("MAIO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server" Text='<%# Eval("MAIO", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Junho" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtJunho" runat="server" Width="85px" Text='<%# Bind("JUNHO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server" Text='<%# Eval("JUNHO", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Julho" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtJulho" runat="server" Width="85px" Text='<%# Bind("JULHO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server" Text='<%# Eval("JULHO", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Agosto" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtAgosto" runat="server" Width="85px" Text='<%# Bind("AGOSTO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server" Text='<%# Eval("AGOSTO", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Setembro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtSetembro" runat="server" Width="85px" Text='<%# Bind("SETEMBRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server" Text='<%# Eval("SETEMBRO", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Outubro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtOutubro" runat="server" Width="85px" Text='<%# Bind("OUTUBRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server" Text='<%# Eval("OUTUBRO", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Novembro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtNovembro" runat="server" Width="85px" Text='<%# Bind("NOVEMBRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server" Text='<%# Eval("NOVEMBRO", "R$ {0}") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dezembro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtDezembro" runat="server" Width="85px" Text='<%# Bind("DEZEMBRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server" Text='<%# Eval("DEZEMBRO", "R$ {0}") %>'></asp:Literal>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
