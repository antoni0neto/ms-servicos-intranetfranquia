<%@ Page Title="Funcionário x Custo Fixo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="cc_funcionario_custofixo.aspx.cs" Inherits="Relatorios.mod_acomp_mensal.centro_custo.cc_funcionario_custofixo" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
        .Label_Grid_Vazio {
	        font-weight: bold;
	        font-size: 10pt;
	        color: white;
	        font-family: arial;
	        text-align: center;
	        background-color: #CCCCCC;
	        border-style: none; 
        }
    </style>
    <script src="../../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../../Image/plus.png");
            $(this).closest("tr").next().remove();
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;Centro
            de Custo&nbsp;&nbsp;>&nbsp;&nbsp;Conferências - Custo Fixo vs Funcionários&nbsp;&nbsp;</span>
        <div style="float: right; padding: 0;">
            <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset>
            <legend>Centro de Custo</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Mês/Ano
                    </td>
                    <td>
                        Grupo Centro de Custo
                    </td>
                    <td>
                        Centro de Custo
                    </td>
                    <td>
                        Filial
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 106px;">
                        <asp:DropDownList runat="server" ID="ddlMesAno" Height="22px" Width="100px" DataValueField="CODIGO_ACOMPANHAMENTO_MESANO"
                                        DataTextField="DESCRICAO">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 256px;">
                        <asp:DropDownList runat="server" ID="ddlCCustoGrupo" DataValueField="ID_GRUPO_CENTRO_CUSTO"
                            DataTextField="DESC_GRUPO_CENTRO_CUSTO" Height="22px" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="ddlCCustoGrupo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 256px;">
                        <asp:DropDownList runat="server" ID="ddlCCusto" DataValueField="CENTRO_CUSTO" DataTextField="DESC_CENTRO_CUSTO" Height="22px" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 256px;">
                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="NumeroCnpj" DataTextField="NomeFantasia" Height="22px" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="red" Visible="false"></asp:Label>
                        <asp:Label ID="labSucesso" runat="server" Text="" ForeColor="blue" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0;" colspan="5">
                        <fieldset>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCentroCustoFuncionario" runat="server" Width="100%" AutoGenerateColumns="False"
                                    CellPadding="0" ForeColor="#333333" Style="background: white"
                                    ShowFooter="true" BorderWidth="1"  AllowSorting="true"
                                    BorderColor="Gainsboro" OnRowDataBound="gvCentroCustoFuncionario_RowDataBound" OnDataBound="gvCentroCustoFuncionario_DataBound" OnSorting="gvCentroCustoFuncionario_Sorting">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <RowStyle Font-Bold="true" Height="24px" BackColor="WhiteSmoke" />
                                    <FooterStyle Font-Bold="true" HorizontalAlign="Center" BackColor="Gainsboro" />
                                    <AlternatingRowStyle BackColor="GhostWhite" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Grupo Centro Custo" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="250px" SortExpression="DESC_GRUPO_CENTRO_CUSTO" >
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlCentroCustoGrupo" runat="server" DataValueField="ID_GRUPO_CENTRO_CUSTO"
                                                    DataTextField="DESC_GRUPO_CENTRO_CUSTO" AutoPostBack="True" 
                                                    Width="240px" BorderStyle="Solid" BorderWidth="1px" OnSelectedIndexChanged="ddlCentroCustoGrupo_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnDescricaoCentroCustoGrupo" Value='<%# Bind("DESC_GRUPO_CENTRO_CUSTO") %>' runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="hdnCentroCustoGrupo" runat="server" Value='<%# Bind("GRUPOCC") %>'></asp:HiddenField>
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="250px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Centro Custo" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="250px" SortExpression="DESC_CENTRO_CUSTO" >
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlCentroCusto" runat="server" DataValueField="CENTRO_CUSTO" DataTextField="DESC_CENTRO_CUSTO" AutoPostBack="True" 
                                                    Width="240px" BorderStyle="Solid" BorderWidth="1px" OnSelectedIndexChanged="ddlCentroCusto_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnDescricaoCentroCusto" Value='<%# Bind("DESC_CENTRO_CUSTO") %>' runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="hdnCentroCusto" runat="server" Value='<%# Bind("CCUSTO") %>'></asp:HiddenField>
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="250px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="250px" SortExpression="LOJA" >
                                            <ItemTemplate>
                                                <asp:Literal ID="litFilial" Text='<%# Bind("LOJA") %>' runat="server"></asp:Literal>
                                                <asp:HiddenField ID="hdnFilial" runat="server" Value='<%# Bind("CNPJ") %>'></asp:HiddenField>
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="250px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Funcionário" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="290px" SortExpression="FUNCIONARIO" >
                                            <ItemTemplate>
                                                <asp:Literal ID="litFuncionario" Text='<%# Bind("FUNCIONARIO") %>' runat="server"></asp:Literal>
                                                <asp:HiddenField ID="hdnFuncionario" runat="server" Value='<%# Bind("CPF") %>'></asp:HiddenField>
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="290px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cargo" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="250px" SortExpression="CARGO" >
                                            <ItemTemplate>
                                                <asp:Literal ID="litCargo" Text='<%# Bind("CARGO") %>' runat="server"></asp:Literal>
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="250px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Salário Líquido" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="120px" SortExpression="SALARIO_LIQUIDO" >
                                            <ItemTemplate>
                                                <asp:Literal ID="litSalarioLiquido" Text='<%# Bind("SALARIO_LIQUIDO") %>' runat="server"></asp:Literal>
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="120px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="V.T." ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="BENEFICIO_VT" >
                                            <ItemTemplate>
                                                <asp:Literal ID="litBeneficioVT" Text='<%# Bind("BENEFICIO_VT") %>' runat="server"></asp:Literal>
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Custo Fixo" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                            <HeaderTemplate>
                                                Custo Fixo
                                                <asp:CheckBox ID="chkTodos" runat="server" AutoPostBack="true" OnCheckedChanged="chkTodos_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnCutoFixo" runat="server" Value='<%# Bind("CUSTO_FIXO") %>'></asp:HiddenField>
                                                <asp:CheckBox ID="chkCutoFixo" runat="server" />
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Center" BorderWidth="1px"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lbl_Vazio" runat="server" cssclass="Label_Grid_Vazio" Text="Nenhum Registro Localizado para o Filtro Informado !" Width="100%"></asp:Label>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0;" colspan="5" align="right">
                        <asp:Button ID="btGravar" runat="server" Text="Gravar" Width="100px" OnClick="btGravar_Click" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
