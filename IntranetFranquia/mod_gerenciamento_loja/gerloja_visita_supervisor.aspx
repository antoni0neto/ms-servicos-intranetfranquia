<%@ Page Title="Agenda de Visitas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_visita_supervisor.aspx.cs" Inherits="Relatorios.gerloja_visita_supervisor" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
        }
    </style>

    <script src="../js/js.js" type="text/javascript"></script>


</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Supervisores&nbsp;&nbsp;>&nbsp;&nbsp;Agenda de Visitas</span>
            <div style="float: right; padding: 0;">
                <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <fieldset class="login">
            <legend>Agenda de Visitas</legend>


            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" style="width: 50%;">
                        <fieldset>

                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="3">
                                        <asp:HiddenField ID="hidAno" runat="server" Value="" />
                                        <asp:HiddenField ID="hidMes" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCodigoUsuario" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 33%;">
                                        <asp:DropDownList ID="ddlSupervisor" runat="server" DataValueField="CODIGO_USUARIO" DataTextField="NOME_USUARIO" Width="200px" OnSelectedIndexChanged="ddlSupervisor_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td style="width: 33%; text-align: center;">
                                        <asp:Label ID="labPeriodo" runat="server" Text="" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                    </td>
                                    <td style="width: 33%; text-align: right;">
                                        <asp:Button ID="btAtualizar" runat="server" Text="Atualizar" Width="70px" OnClick="btAtualizar_Click" />&nbsp;
                            <asp:Button ID="btAnterior" runat="server" Text="<<" Width="70px" OnClick="btAnterior_Click" />&nbsp;
                        <asp:Button ID="btProximo" runat="server" Text=">>" Width="70px" OnClick="btProximo_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="7">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 14%">&nbsp;<asp:Label ID="labSegunda" runat="server" Text="Segunda" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 15%">&nbsp;<asp:Label ID="labTerca" runat="server" Text="Terça" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 14%">&nbsp;<asp:Label ID="labQuarta" runat="server" Text="Quarta" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 15%">&nbsp;<asp:Label ID="labQuinta" runat="server" Text="Quinta" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 14%">&nbsp;<asp:Label ID="labSexta" runat="server" Text="Sexta" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 15%">&nbsp;<asp:Label ID="labSabado" runat="server" Text="Sábado" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 14%">&nbsp;<asp:Label ID="labDomingo" runat="server" Text="Domingo" Font-Bold="true"></asp:Label></td>
                                </tr>
                            </table>
                            <asp:Repeater ID="repAgendaSuper" runat="server" OnItemDataBound="repAgendaSuper_ItemDataBound">
                                <ItemTemplate>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 14%">
                                                <asp:Table ID="tbSegunda" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="8px">
                                                            &nbsp;<asp:ImageButton ID="btImgSegunda" runat="server" ImageUrl="~/Image/add.png" Width="13px" Height="13px" AlternateText=" " OnClick="btImgAdd_Click" />
                                                        </asp:TableCell>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litSegundaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="75px" ColumnSpan="2">
                                                            <asp:Literal ID="litSegundaFilial" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:Table ID="tbTerca" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="8px">
                                                            &nbsp;<asp:ImageButton ID="btImgTerca" runat="server" ImageUrl="~/Image/add.png" Width="13px" Height="13px" AlternateText=" " OnClick="btImgAdd_Click" />
                                                        </asp:TableCell>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litTercaDia" runat="server" Font-Bold="true"></asp:Label>
                                                            &nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="75px" ColumnSpan="2">
                                                            <asp:Literal ID="litTercaFilial" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 14%">
                                                <asp:Table ID="tbQuarta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="8px">
                                                            &nbsp;<asp:ImageButton ID="btImgQuarta" runat="server" ImageUrl="~/Image/add.png" Width="13px" Height="13px" AlternateText=" " OnClick="btImgAdd_Click" />
                                                        </asp:TableCell>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litQuartaDia" runat="server" Font-Bold="true"></asp:Label>
                                                            &nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="75px" ColumnSpan="2">
                                                            <asp:Literal ID="litQuartaFilial" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:Table ID="tbQuinta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="8px">
                                                            &nbsp;<asp:ImageButton ID="btImgQuinta" runat="server" ImageUrl="~/Image/add.png" Width="13px" Height="13px" AlternateText=" " OnClick="btImgAdd_Click" />
                                                        </asp:TableCell>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litQuintaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="75px" ColumnSpan="2">
                                                            <asp:Literal ID="litQuintaFilial" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 14%">
                                                <asp:Table ID="tbSexta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="8px">
                                                            &nbsp;<asp:ImageButton ID="btImgSexta" runat="server" ImageUrl="~/Image/add.png" Width="13px" Height="13px" AlternateText=" " OnClick="btImgAdd_Click" />
                                                        </asp:TableCell>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litSextaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="75px" ColumnSpan="2">
                                                            <asp:Literal ID="litSextaFilial" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:Table ID="tbSabado" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="8px">
                                                            &nbsp;<asp:ImageButton ID="btImgSabado" runat="server" ImageUrl="~/Image/add.png" Width="13px" Height="13px" AlternateText=" " OnClick="btImgAdd_Click" />
                                                        </asp:TableCell>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litSabadoDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="75px" ColumnSpan="2">
                                                            <asp:Literal ID="litSabadoFilial" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 14%">
                                                <asp:Table ID="tbDomingo" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="8px">
                                                            &nbsp;<asp:ImageButton ID="btImgDomingo" runat="server" ImageUrl="~/Image/add.png" Width="13px" Height="13px" AlternateText=" " OnClick="btImgAdd_Click" />
                                                        </asp:TableCell>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litDomingoDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="75px" ColumnSpan="2">
                                                            <asp:Literal ID="litDomingoFilial" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </td>
                    <td valign="top" style="width: 50%;">
                        <fieldset>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCalendarioSemana" runat="server" Width="100%"
                                    AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="gvCalendarioSemana_RowDataBound"
                                    OnDataBound="gvCalendarioSemana_DataBound">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labFilial" runat="server" Text='<%# Bind("FILIAL")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Segunda" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="58px">
                                            <ItemTemplate>
                                                <asp:Label ID="labSegunda" runat="server" Text='<%# Bind("SEGUNDA")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Terça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="58px">
                                            <ItemTemplate>
                                                <asp:Label ID="labTerca" runat="server" Text='<%# Bind("TERCA")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quarta" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="58px">
                                            <ItemTemplate>
                                                <asp:Label ID="labQuarta" runat="server" Text='<%# Bind("QUARTA")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quinta" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="58px">
                                            <ItemTemplate>
                                                <asp:Label ID="labQuinta" runat="server" Text='<%# Bind("QUINTA")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sexta" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="58px">
                                            <ItemTemplate>
                                                <asp:Label ID="labSexta" runat="server" Text='<%# Bind("SEXTA")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sábado" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="58px">
                                            <ItemTemplate>
                                                <asp:Label ID="labSabado" runat="server" Text='<%# Bind("SABADO")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Domingo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:Label ID="labDomingo" runat="server" Text='<%# Bind("DOMINGO")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Intervalo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <asp:Label ID="labIntervalo" runat="server" Text='<%# Bind("ULTIMA_VISITA_DIAS")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <asp:Label ID="labAgendado" runat="server" ForeColor="Red" Text="Azul possui agendamento*"></asp:Label>
                        </fieldset>
                    </td>
                </tr>
            </table>



        </fieldset>
    </div>
</asp:Content>
