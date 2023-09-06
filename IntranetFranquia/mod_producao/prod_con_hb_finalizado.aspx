<%@ Page Title="HB Consulta Finalizado" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="prod_con_hb_finalizado.aspx.cs" Inherits="Relatorios.prod_con_hb_finalizado"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="../js/js.js" type="text/javascript"></script>
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div class="accountInfo">
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios
                    Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;HB Consulta Finalizados</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
                <hr />
                <fieldset>
                    <legend>HB Consulta Finalizados</legend>
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <div style="width: 600px;" class="alinhamento">
                                    <div style="width: 200px;" class="alinhamento">
                                        <label>
                                            <span style="color: Red;"></span>Data Início:&nbsp;
                                        </label>
                                        <asp:TextBox ID="txtDataInicio" runat="server" onkeypress="return fnValidarData(event);"
                                            Height="22px" MaxLength="10" Width="196px"></asp:TextBox>
                                        <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                                            CaptionAlign="Bottom"></asp:Calendar>
                                    </div>
                                    <div style="width: 200px;" class="alinhamento">
                                        <label>
                                            <span style="color: Red;"></span>Data Fim:&nbsp;
                                        </label>
                                        <asp:TextBox ID="txtDataFim" runat="server" onkeypress="return fnValidarData(event);"
                                            Height="22px" MaxLength="10" Width="196px"></asp:TextBox>
                                        <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                                            CaptionAlign="Bottom"></asp:Calendar>
                                    </div>
                                    <div style="width: 200px;" class="alinhamento">
                                        <label>
                                            <span style="color: Red;"></span>Coleção:&nbsp;
                                        </label>
                                        <asp:DropDownList ID="ddlColecoes" runat="server" Width="254px" Height="22px" DataTextField="DESC_COLECAO"
                                            DataValueField="COLECAO">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        <label>
                                            <span style="color: Red;"></span>Grupo:&nbsp;
                                        </label>
                                        <asp:DropDownList ID="ddlGrupo" runat="server" Width="254px" Height="22px" DataTextField="GRUPO_PRODUTO"
                                            DataValueField="GRUPO_PRODUTO">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        <label>
                                            <span style="color: Red;"></span>Mostruário:&nbsp;
                                        </label>
                                        <asp:DropDownList ID="ddlMostruario" runat="server" Width="254px" Height="22px">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <p style="height: 13px">
                                        &nbsp;</p>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;">
                                <fieldset>
                                    <legend>Finalizados</legend>
                                    <div style="border: 0px solid #000; padding: 0;">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvFinalizado" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvFinalizado_RowDataBound"
                                                OnDataBound="gvFinalizado_DataBound">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Coleção" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="HB" HeaderText="HB" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="DETALHE" HeaderText="Detalhe" HeaderStyle-Width="130px"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="GRUPO" HeaderText="Grupo" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="NOME" HeaderText="Nome" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="DT_INI_AMPLIACAO" HeaderText="Início Ampliação" HeaderStyle-Width="130px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="DT_INI_RISCO" HeaderText="Início Risco" HeaderStyle-Width="130px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="DT_INI_CORTE" HeaderText="Início Corte" HeaderStyle-Width="130px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="DT_FINALIZADO" HeaderText="Finalizado" HeaderStyle-Width="130px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="QTDE" HeaderText="Qtde" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; text-align: right;">
                                <div style="float: right; width: 100%; border: 0px solid black; padding: 0;">
                                    <asp:Label ID="labErroImpressao" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Visible="false" OnClick="btImprimir_Click"
                                        Width="100px" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
