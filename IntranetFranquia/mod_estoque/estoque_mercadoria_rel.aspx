<%@ Page Title="Relatório de Divergência" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="estoque_mercadoria_rel.aspx.cs" Inherits="Relatorios.estoque_mercadoria_rel"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="../js/js.js" type="text/javascript"></script>
    <style type="text/css">
        .alinhamento {
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Controle de Estoque&nbsp;&nbsp;>&nbsp;&nbsp;Estoque
                    Loja&nbsp;&nbsp;>&nbsp;&nbsp;Relatório de Divergência</span>
                <div style="float: right; padding: 0;">
                    <a href="estoque_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
                <hr />
                <fieldset>
                    <legend>Relatório de Divergência</legend>
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
                                            <span style="color: Red;"></span>Filial:&nbsp;
                                        </label>
                                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                            Height="24px" Width="210px">
                                        </asp:DropDownList>
                                        <br />
                                        <label>
                                            <span style="color: Red;"></span>Nota Fiscal:&nbsp;
                                        </label>
                                        <asp:TextBox ID="txtNF" runat="server" Width="206px" Height="18px" MaxLength="10"></asp:TextBox>
                                        <br />
                                        <label>
                                            <span style="color: Red;"></span>Produto:&nbsp;
                                        </label>
                                        <asp:TextBox ID="txtProduto" runat="server" Width="206px" Height="18px" MaxLength="10"
                                            onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                    </div>
                                    <p style="height: 13px">
                                        &nbsp;
                                    </p>
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
                            <td style="line-height: 5px;">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;">
                                <fieldset>
                                    <legend>Produtos</legend>
                                    <div style="border: 0px solid #000; padding: 0;">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvMercadoria" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvMercadoria_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FILIAL" HeaderText="Filial" HeaderStyle-Width="190px"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="EMISSAO" HeaderText="Emissão" HeaderStyle-Width="80px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NUMERO_NF_TRANSFERENCIA" HeaderText="NF" HeaderStyle-Width="110px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="SERIE_NF_ENTRADA" HeaderText="Série" HeaderStyle-Width="40px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="90px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Descrição" HeaderStyle-Width="140px"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="DESC_COR" HeaderText="Cor" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="QTDE_NOTA" HeaderText="Qtde Nota" HeaderStyle-Width="85px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="QTDE_REAL" HeaderText="Qtde Recebida" HeaderStyle-Width="95px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="STATUS" HeaderText="Situação" HeaderStyle-Width="95px"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="STATUS_CONFERENCIA" HeaderText="Status" HeaderStyle-Width="100px"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="OBSERVACAO" HeaderText="Observação" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        HeaderText="Linx" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbEst" runat="server" Checked="false" OnCheckedChanged="cbEst_CheckedChanged" AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; text-align: right;">&nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
