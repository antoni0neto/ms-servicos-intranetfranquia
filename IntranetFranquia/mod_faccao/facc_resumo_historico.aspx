<%@ Page Title="Ficha Entrada" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="facc_resumo_historico.aspx.cs" Inherits="Relatorios.facc_resumo_historico"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Ficha Entrada</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Ficha Entrada"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção
                        </td>
                        <td>HB
                        </td>
                        <td style="text-align: center;">Mostruário
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="174px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtHB" runat="server" Width="100px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 90px; text-align: center;">
                            <asp:CheckBox ID="cbMostruario" runat="server" Checked="false" Text="" />
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="hidCodigoHB" runat="server" Value="" />
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td>Produto
                        </td>
                        <td colspan="2">Nome
                        </td>
                        <td>Cor
                        </td>
                    </tr>


                    <tr>
                        <td>
                            <asp:TextBox ID="txtProduto" runat="server" Width="170px" MaxLength="5" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNome" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCor" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4">
                            <asp:Image ID="imgFoto" runat="server" Width="150px" BorderColor="Black" Visible="false"
                                BorderWidth="0" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset>
                                <legend>Estamparia</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvEstamparia" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvHistoricoEstamparia_RowDataBound"
                                        ShowFooter="true" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Label ID="labFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Custo" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labCusto" runat="server" Text="-"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGrade" runat="server" Text='<%# Bind("GRADE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NF Entrada" HeaderStyle-Width="105px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labNFEntrada" runat="server" Text='<%# Bind("NF_ENTRADA") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labData" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EXP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeEXP" runat="server" Text='<%# Bind("GRADE_EXP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="XP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeXP" runat="server" Text='<%# Bind("GRADE_XP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradePP" runat="server" Text='<%# Bind("GRADE_PP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeP" runat="server" Text='<%# Bind("GRADE_P") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="M" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeM" runat="server" Text='<%# Bind("GRADE_M") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="G" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeG" runat="server" Text='<%# Bind("GRADE_G") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GG" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeGG" runat="server" Text='<%# Bind("GRADE_GG") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeTotal" runat="server" Text='<%# Bind("GRADE_TOTAL") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrega" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDataEntrega" runat="server" Width="80px" MaxLength="10" Style="text-align: center;"
                                                        onkeypress="return fnValidarData(event);" OnTextChanged="txtDataEntrega_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset>
                                <legend>Facção</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFaccao" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvHistoricoFaccao_RowDataBound"
                                        ShowFooter="true" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" Font-Size="Smaller" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Label ID="labFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Custo" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labCusto" runat="server" Text="-"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGrade" runat="server" Text='<%# Bind("GRADE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NF Entrada" HeaderStyle-Width="105px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labNFEntrada" runat="server" Text='<%# Bind("NF_ENTRADA") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labData" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EXP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeEXP" runat="server" Text='<%# Bind("GRADE_EXP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="XP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeXP" runat="server" Text='<%# Bind("GRADE_XP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradePP" runat="server" Text='<%# Bind("GRADE_PP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeP" runat="server" Text='<%# Bind("GRADE_P") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="M" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeM" runat="server" Text='<%# Bind("GRADE_M") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="G" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeG" runat="server" Text='<%# Bind("GRADE_G") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GG" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeGG" runat="server" Text='<%# Bind("GRADE_GG") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeTotal" runat="server" Text='<%# Bind("GRADE_TOTAL") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrega" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDataEntrega" runat="server" Width="80px" MaxLength="10" Style="text-align: center;"
                                                        onkeypress="return fnValidarData(event);" OnTextChanged="txtDataEntrega_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr style="visibility: visible;">
                        <td colspan="4">
                            <fieldset>
                                <legend>Lavanderia</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvLavanderia" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvHistoricoLavanderia_RowDataBound"
                                        ShowFooter="true" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" Font-Size="Smaller" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Label ID="labFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Custo" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labCusto" runat="server" Text="-"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGrade" runat="server" Text='<%# Bind("GRADE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NF Entrada" HeaderStyle-Width="105px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labNFEntrada" runat="server" Text='<%# Bind("NF_ENTRADA") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labData" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EXP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeEXP" runat="server" Text='<%# Bind("GRADE_EXP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="XP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeXP" runat="server" Text='<%# Bind("GRADE_XP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradePP" runat="server" Text='<%# Bind("GRADE_PP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeP" runat="server" Text='<%# Bind("GRADE_P") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="M" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeM" runat="server" Text='<%# Bind("GRADE_M") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="G" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeG" runat="server" Text='<%# Bind("GRADE_G") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GG" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeGG" runat="server" Text='<%# Bind("GRADE_GG") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeTotal" runat="server" Text='<%# Bind("GRADE_TOTAL") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrega" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDataEntrega" runat="server" Width="80px" MaxLength="10" Style="text-align: center;"
                                                        onkeypress="return fnValidarData(event);" OnTextChanged="txtDataEntrega_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset>
                                <legend>Acabamento</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvGradeCorte" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvGradeCorte_RowDataBound">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade Corte" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGrade" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EXP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeEXP" runat="server" Text='<%# Bind("GRADE_EXP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="XP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeXP" runat="server" Text='<%# Bind("GRADE_XP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PP" HeaderStyle-Width="85px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradePP" runat="server" Text='<%# Bind("GRADE_PP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeP" runat="server" Text='<%# Bind("GRADE_P") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="M" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeM" runat="server" Text='<%# Bind("GRADE_M") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="G" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeG" runat="server" Text='<%# Bind("GRADE_G") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GG" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeGG" runat="server" Text='<%# Bind("GRADE_GG") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeTotal" runat="server" Text=''></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvAcabamento" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvHistoricoAcabamento_RowDataBound"
                                        ShowFooter="true" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                        <FooterStyle Font-Size="Smaller" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Label ID="labFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Custo" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labCusto" runat="server" Text="-"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGrade" runat="server" Text='<%# Bind("GRADE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NF Entrada" HeaderStyle-Width="105px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labNFEntrada" runat="server" Text='<%# Bind("NF_ENTRADA") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labData" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EXP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeEXP" runat="server" Text='<%# Bind("GRADE_EXP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="XP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeXP" runat="server" Text='<%# Bind("GRADE_XP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PP" HeaderStyle-Width="85px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradePP" runat="server" Text='<%# Bind("GRADE_PP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeP" runat="server" Text='<%# Bind("GRADE_P") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="M" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeM" runat="server" Text='<%# Bind("GRADE_M") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="G" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeG" runat="server" Text='<%# Bind("GRADE_G") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GG" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeGG" runat="server" Text='<%# Bind("GRADE_GG") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeTotal" runat="server" Text='<%# Bind("GRADE_TOTAL") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrega" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDataEntrega" runat="server" Width="80px" MaxLength="10" Style="text-align: center;"
                                                        onkeypress="return fnValidarData(event);" OnTextChanged="txtDataEntrega_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset>
                                <legend>Segunda Qualidade/Lavanderia/Conserto</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvOutros" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvHistoricoOutros_RowDataBound"
                                        ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labFornecedor" runat="server" Text="HANDBOOK"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Custo" HeaderStyle-Width="115px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labCusto" runat="server" Text="-"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGrade" runat="server" Text='<%# Bind("GRADE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NF Entrada" HeaderStyle-Width="105px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labNFEntrada" runat="server" Text='<%# Bind("NF_ENTRADA") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labData" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EXP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeEXP" runat="server" Text='<%# Bind("GRADE_EXP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="XP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeXP" runat="server" Text='<%# Bind("GRADE_XP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PP" HeaderStyle-Width="85px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradePP" runat="server" Text='<%# Bind("GRADE_PP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeP" runat="server" Text='<%# Bind("GRADE_P") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="M" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeM" runat="server" Text='<%# Bind("GRADE_M") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="G" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeG" runat="server" Text='<%# Bind("GRADE_G") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GG" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeGG" runat="server" Text='<%# Bind("GRADE_GG") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeTotal" runat="server" Text='<%# Bind("GRADE_TOTAL") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset>
                                <legend>Logística</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvLogistica" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvHistoricoLogistica_RowDataBound"
                                        ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Custo" HeaderStyle-Width="115px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labCusto" runat="server" Text="-"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGrade" runat="server" Text='<%# Bind("GRADE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NF Entrada" HeaderStyle-Width="105px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labNFEntrada" runat="server" Text='<%# Bind("NF_ENTRADA") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labData" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EXP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeEXP" runat="server" Text='<%# Bind("GRADE_EXP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="XP" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeXP" runat="server" Text='<%# Bind("GRADE_XP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PP" HeaderStyle-Width="85px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradePP" runat="server" Text='<%# Bind("GRADE_PP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeP" runat="server" Text='<%# Bind("GRADE_P") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="M" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeM" runat="server" Text='<%# Bind("GRADE_M") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="G" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeG" runat="server" Text='<%# Bind("GRADE_G") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GG" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeGG" runat="server" Text='<%# Bind("GRADE_GG") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeTotal" runat="server" Text='<%# Bind("GRADE_TOTAL") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
