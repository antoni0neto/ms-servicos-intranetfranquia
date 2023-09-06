<%@ Page Title="Programação da Contagem" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="cont_programacao.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.cont_programacao" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .bodyCont {
            font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif !important;
            margin: 0px;
            padding: 0px;
            color: #696969;
        }

        .jGrowl .manilla {
            background-color: #000;
            color: red;
            font-weight: bold;
        }

        .alinharDireira {
            text-align: right;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#tabs").tabs();

        });
        function MarcarAba(vAba) {
            document.getElementById("MainContent_hidTabSelected").value = vAba;
        }

    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contagem&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro&nbsp;&nbsp;>&nbsp;&nbsp;Programação da Contagem</span>
                <div style="float: right; padding: 0;">
                    <a href="cont_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Programação da Contagem</legend>
                    <div style="padding-left: 0px;">
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <asp:HiddenField ID="hidAcao" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <div>
                                <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div>
                        <br />
                        <hr />
                        <br />
                    </div>
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-programacao" id="tabProgramacao" runat="server" onclick="MarcarAba(0);">Programação</a></li>
                            <li><a href="#tabs-por-mes" id="tabPorMes" runat="server" onclick="MarcarAba(1);">Por Mês</a></li>
                            <li><a href="#tabs-fechamento-mes" id="tabFechamentoMes" runat="server" onclick="MarcarAba(2);">Fechamento Mês</a></li>
                        </ul>
                        <div id="tabs-programacao">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyCont">
                                <tr>
                                    <td>Ano
                                    </td>
                                    <td>Mês
                                    </td>
                                    <td>Dia
                                    </td>
                                    <td>Filial
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="144px" DataTextField="ANO"
                                            DataValueField="ANO" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:DropDownList runat="server" ID="ddlMes" Height="22px" Width="144px"
                                            OnSelectedIndexChanged="ddlMes_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Janeiro"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Fevereiro"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Março"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Maio"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="Junho"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="Julho"></asp:ListItem>
                                            <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="Setembro"></asp:ListItem>
                                            <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                            <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                            <asp:ListItem Value="12" Text="Dezembro"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:DropDownList runat="server" ID="ddlDia" Height="22px" Width="144px" OnSelectedIndexChanged="ddlDia_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 390px;">
                                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                            Height="22px" Width="384px" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btnSalvar" runat="server" Text=">>" Width="80px" Height="22px" OnClick="btnSalvar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">Responsável
                                    </td>
                                    <td colspan="2">Descrição
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtResponsavel" runat="server" Width="440px"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDescricao" runat="server" Width="470px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvProgramacao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                ShowFooter="true">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        HeaderText="Data" HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Dia da Semana" HeaderStyle-Width="180px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Entrada">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Responsável">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                ToolTip="Excluir" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="tabs-por-mes">
                            <div>
                                <div class="bodyCont" style="margin-bottom: -20px;">
                                    <fieldset>
                                        <legend>Filtro</legend>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyCont">
                                            <tr>
                                                <td>Ano
                                                </td>
                                                <td>Filial
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">
                                                    <asp:DropDownList runat="server" ID="ddlAnoFiltro" Height="22px" Width="144px" DataTextField="ANO"
                                                        DataValueField="ANO" OnSelectedIndexChanged="ddlAnoFiltro_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlFilialFiltro" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                                        Height="22px" Width="384px" OnSelectedIndexChanged="ddlFilialFiltro_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </div>
                            <br />
                            <div>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyCont">
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Janeiro</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvJaneiro" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Fevereiro</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvFevereiro" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Março</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvMarco" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Abril</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvAbril" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Maio</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvMaio" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Junho</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvJunho" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Julho</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvJulho" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Agosto</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvAgosto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Setembro</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvSetembro" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Outubro</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvOutubro" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Novembro</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvNovembro" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Dezembro</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvDezembro" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProgramacao_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Dia da Semana">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaSemana" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Entrada">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Responsável">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="tabs-fechamento-mes">
                            <div class="bodyCont" style="margin-bottom: -20px;">
                                <fieldset>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyCont">
                                        <tr>
                                            <td>Ano
                                            </td>
                                            <td>Mês
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px;">
                                                <asp:DropDownList runat="server" ID="ddlAnoFechamento" Height="22px" Width="144px" DataTextField="ANO"
                                                    DataValueField="ANO" OnSelectedIndexChanged="ddlAnoFechamento_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 150px;">
                                                <asp:DropDownList runat="server" ID="ddlMesFechamento" Height="22px" Width="144px" OnSelectedIndexChanged="ddlAnoFechamento_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Janeiro"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Fevereiro"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Março"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="Maio"></asp:ListItem>
                                                    <asp:ListItem Value="6" Text="Junho"></asp:ListItem>
                                                    <asp:ListItem Value="7" Text="Julho"></asp:ListItem>
                                                    <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                                    <asp:ListItem Value="9" Text="Setembro"></asp:ListItem>
                                                    <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                                    <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                                    <asp:ListItem Value="12" Text="Dezembro"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Button ID="btFecharMes" runat="server" Text="Fechar" Width="80px" Enabled="false" Height="22px" OnClick="btFecharMes_Click"
                                                    OnClientClick="DesabilitarBotao(this); " />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
