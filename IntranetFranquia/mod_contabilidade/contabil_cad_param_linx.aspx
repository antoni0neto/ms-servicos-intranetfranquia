<%@ Page Title="Cadastro de Parâmetros de Fechamento" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="contabil_cad_param_linx.aspx.cs" Inherits="Relatorios.contabil_cad_param_linx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
            background-color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Conciliação&nbsp;&nbsp;>&nbsp;&nbsp;Período de Fechamento</span>
                <div style="float: right; padding: 0;">
                    <a href="contabil_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 100%;">
                <fieldset class="">
                    <legend>Período de Fechamento</legend>
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <div style="width: 200px;" class="alinhamento">
                                    <label>
                                        Informe a data do Fechamento:&nbsp;
                                    </label>
                                    <asp:TextBox ID="TxtDataFechamento" runat="server" CssClass="textEntry" Height="22px"
                                        Width="198px"></asp:TextBox>
                                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                                        CaptionAlign="Bottom"></asp:Calendar>
                                </div>
                            </td>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btBuscar" Text="Buscar" OnClick="btBuscar_Click" Enabled="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="GridViewParametros" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ShowFooter="true" ForeColor="#333333" Style="background: white" DataKeyNames="PARAMETRO1">
                                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                        <RowStyle HorizontalAlign="Center"></RowStyle>
                                        <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundField DataField="PARAMETRO1" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Parâmetro" />
                                            <asp:BoundField DataField="DESC_PARAMETRO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Descrição" />
                                            <asp:BoundField DataField="VALOR_ATUAL" HeaderText="Data Atual" />
                                            <asp:TemplateField HeaderText="Nova Data">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtData" AutoPostBack="false" Width="100" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                    </table>
                    <div>
                        <br />
                        <asp:Button runat="server" ID="btGravar" Text="Atualizar" OnClick="btGravar_Click"
                            Enabled="False" />
                        <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
                    </div>
                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
