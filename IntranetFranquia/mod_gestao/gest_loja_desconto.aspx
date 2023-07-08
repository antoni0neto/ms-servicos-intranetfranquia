<%@ Page Title="Descontos de Venda" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="gest_loja_desconto.aspx.cs" Inherits="Relatorios.gest_loja_desconto"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }
    </style>
       
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    
    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>   
    <script type="text/javascript" src="../js/fixheader/jquery.freezeheader.js"></script>
    <script type="text/javascript">           
        
        $(document).on("click", "[src*=plus]", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");

            $(c).next().css("display", "block");

        });

        $(document).on("click", "[src*=minus]", function () {
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
                <span style="font-family: Calibri; font-size: 14px;">
                    <asp:Label ID="lblModulo" runat="server"></asp:Label>
                    &nbsp;&nbsp;>&nbsp;&nbsp;
                    <asp:Label ID="lblSubModulo" runat="server"></asp:Label>&nbsp;&nbsp;>&nbsp;&nbsp;Descontos de Venda</span>
                <div style="float: right; padding: 0;">
                    <asp:HyperLink ID="lnkVoltar" runat="server" NavigateUrl="~/mod_gestao/gest_menu.aspx" CssClass="alink" Text="Voltar"></asp:HyperLink>
                </div>
                <hr />
                <fieldset>
                    <legend>Descontos de Venda</legend>
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
                                            Height="26px" Width="200px">
                                        </asp:DropDownList>
                                        <br />
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
                            <td style="padding: 0;">
                                <fieldset>
                                    <legend>Tickets</legend>
                                    <div style="border: 0px solid #000; padding: 0;">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvTicketTotal" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvTicketTotal_RowDataBound"
                                                OnDataBound="gvTicketTotal_DataBound">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="63px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="463px"
                                                        HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTipoDesconto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="TOTAL_TICKETS" HeaderText="Total de Tickets" HeaderStyle-Width="179px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="TOTAL_PECAS" HeaderText="Total de Peças" HeaderStyle-Width="110px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:TemplateField HeaderText="Valor Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="120px"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalValor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desconto Total" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalDesconto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desconto (%)" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litMediaDesconto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <br />
                                    <div style="border: 0px solid #000; padding: 0;">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvTicket" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvTicket_RowDataBound"
                                                OnDataBound="gvTicket_DataBound">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgExpand" alt="" style="cursor: pointer" src="../../Image/plus.png"
                                                                Width="18px" runat="server" />
                                                            <asp:Panel ID="pnlTickets" runat="server" style="display: none;" Width="100%">
                                                                <asp:GridView ID="gvDescontoDetalhe" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvDescontoDetalhe_RowDataBound"
                                                                    Width="100%">
                                                                    <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                    <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                                            ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="TICKET" HeaderText="Ticket" HeaderStyle-Width="86px" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="VENDEDOR_APELIDO" HeaderText="Vendedor" HeaderStyle-Width="175px"
                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="CLIENTE_VAREJO" HeaderText="Cliente" HeaderStyle-Width="260px"
                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                        <asp:TemplateField HeaderText="Mês Aniversário" HeaderStyle-Width="115px" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BorderColor="GradientActiveCaption"
                                                                            ItemStyle-BorderWidth="1px">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litMesAniversario" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="TOTAL_PECAS" HeaderText="Total de Peças" HeaderStyle-Width="110px"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                        <asp:TemplateField HeaderText="Valor (R$)" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right"
                                                                            ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litValorTotal" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Desconto (R$)" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Right"
                                                                            ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litDesconto" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Desconto (%)" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                                                            HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litTotalDescontoPorc" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:BoundField DataField="FORMA_PGTO" HeaderText="Forma Pgto" HeaderStyle-Width="86PX" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-HorizontalAlign="Center" />

                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tipo de Desconto" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-Width="463px" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTipoDesconto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="TOTAL_TICKETS" HeaderText="Total de Tickets" HeaderStyle-Width="179px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="TOTAL_PECAS" HeaderText="Total de Peças" HeaderStyle-Width="110px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:TemplateField HeaderText="Valor Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="120px"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalValor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desconto Total" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalDesconto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desconto (%)" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litMediaDesconto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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
