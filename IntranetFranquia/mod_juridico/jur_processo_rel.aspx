<%@ Page Title="Acompanhamento de Processos" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="jur_processo_rel.aspx.cs" Inherits="Relatorios.jur_processo_rel"
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
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Image/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div class="accountInfo">
                <span style="font-family: Calibri; font-size: 14px;">Módulo Jurídico&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Acompanhamento
                    de Processos</span>
                <div style="float: right; padding: 0;">
                    <a href="jur_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
                <hr />
                <fieldset>
                    <legend>Acompanhamento de Processos</legend>
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <div style="width: 600px;" class="alinhamento">
                                    <div style="width: 200px;" class="alinhamento">
                                        <label>
                                            <span style="color: Red;"></span>Data Julgamento&nbsp;
                                        </label>
                                        <asp:TextBox ID="txtDataInicio" runat="server" onkeypress="return fnValidarData(event);"
                                            Height="22px" MaxLength="10" Width="196px"></asp:TextBox>
                                        <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                                            CaptionAlign="Bottom"></asp:Calendar>
                                    </div>
                                    <div style="width: 200px;" class="alinhamento">
                                        <label>
                                            <span style="color: Red;"></span>Data Julgamento&nbsp;
                                        </label>
                                        <asp:TextBox ID="txtDataFim" runat="server" onkeypress="return fnValidarData(event);"
                                            Height="22px" MaxLength="10" Width="196px"></asp:TextBox>
                                        <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                                            CaptionAlign="Bottom"></asp:Calendar>
                                    </div>
                                    <div style="width: 200px;" class="alinhamento">
                                        <label>
                                            <span style="color: Red;"></span>Tipo Processo&nbsp;
                                        </label>
                                        <asp:DropDownList ID="ddlTipoProcesso" runat="server" Width="254px" Height="22px"
                                            DataTextField="DESCRICAO" DataValueField="CODIGO">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        <label>
                                            <span style="color: Red;"></span>Fase&nbsp;
                                        </label>
                                        <asp:DropDownList ID="ddlInstancia" runat="server" Width="254px" Height="22px" DataTextField="DESCRICAO"
                                            DataValueField="CODIGO">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        <label>
                                            <span style="color: Red;"></span>Número&nbsp;
                                        </label>
                                        <br />
                                        <asp:TextBox ID="txtNumero" runat="server" Width="250px" Height="18px"></asp:TextBox>
                                        <br />
                                        <br />
                                        <label>
                                            <span style="color: Red;"></span>Status&nbsp;
                                        </label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="254px" Height="22px">
                                            <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="AGUARDANDO JULGAMENTO"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="AGUARDANDO SENTENÇA"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="FINALIZADO"></asp:ListItem>
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
                                    <legend>Processos</legend>
                                    <div style="border: 0px solid #000; padding: 0;">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvProcesso" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvProcesso_RowDataBound">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <img alt="" style="cursor: pointer" src="../Image/plus.png" width="18px" />
                                                            <asp:Panel ID="pnlInstancia" runat="server" Style="display: none" Width="100%">
                                                                <asp:GridView ID="gvInstancia" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvInstancia_RowDataBound"
                                                                    Width="100%">
                                                                    <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                    <RowStyle BorderColor="Gainsboro" BorderWidth="1px" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="23px" ItemStyle-HorizontalAlign="Center"
                                                                            ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Fase" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderStyle-Width="195px" ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litInstancia" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="DATA_JULGAMENTO" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-BorderColor="Gainsboro"
                                                                            ItemStyle-BorderWidth="1px" HeaderText="Data de Julgamento" HeaderStyle-Width="200px"
                                                                            DataFormatString="{0:d}" />
                                                                        <asp:BoundField DataField="DATA_CONDENACAO" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-BorderColor="Gainsboro"
                                                                            ItemStyle-BorderWidth="1px" HeaderText="Data de Sentença" HeaderStyle-Width="200px"
                                                                            DataFormatString="{0:d}" />
                                                                        <asp:BoundField DataField="FORMA_PGTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                            ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px" HeaderText="Forma de Pagamento"
                                                                            HeaderStyle-Width="230px" />
                                                                        <asp:BoundField DataField="OBSERVACAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                            ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px" HeaderText="Observação" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DESCRICAO" HeaderText="Tipo de Processo" HeaderStyle-Width="195px"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="REQUERENTE" HeaderText="Requerente" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="CARGO" HeaderText="Cargo" HeaderStyle-Width="190px" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="155px"
                                                        ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DATA_CONDENACAO" HeaderText="Data de Sentença" HeaderStyle-Width="125px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="FORMA_PGTO" HeaderText="Forma Pgto" HeaderStyle-Width="150px"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="NUMERO" HeaderText="Número" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-Width="220px" HeaderStyle-HorizontalAlign="Left" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
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
