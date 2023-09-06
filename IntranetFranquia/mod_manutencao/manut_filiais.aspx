<%@ Page Title="Configuração de Filiais" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="manut_filiais.aspx.cs" Inherits="Relatorios.mod_manutencao.manut_filiais" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .Label_Grid_Vazio {
            font-weight: bold;
            font-size: 8pt;
            color: red;
            font-family: arial;
            text-align: center;
            background-color: #CCCCCC;
            border-style: none;
        }
    </style>
    <script src="../../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Manutenção&nbsp;&nbsp;>&nbsp;&nbsp;Configurações
            &nbsp;&nbsp;>&nbsp;&nbsp;Filiais&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a href="manut_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <asp:Panel ID="panPrincipal" runat="server">
            <div class="accountInfo">
                <fieldset>
                    <legend>Configurações de Filiais</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 1006px;">
                                <div style="width: 1000px;" class="alinhamento">
                                    <div style="width: 600px;" class="alinhamento">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="7%">
                                                    <asp:Label ID="Label4" runat="server" Text="Filial:"></asp:Label>
                                                </td>
                                                <td width="47%">
                                                    <asp:DropDownList ID="ddlFilial" Width="250px" runat="server" DataValueField="COD_FILIAL" DataTextField="FILIAL" OnDataBound="ddlFilial_DataBound" ></asp:DropDownList>
                                                </td>
                                                <td width="7%">
                                                    <asp:Label ID="Label1" runat="server" Text="Tipo:"></asp:Label>
                                                </td>
                                                <td width="39%">
                                                    <asp:DropDownList ID="ddlTipoFilial" Width="200px" runat="server" DataValueField="TIPO_FILIAL" DataTextField="TIPO_FILIAL" OnDataBound="ddlTipoFilial_DataBound" ></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="width: 300px;" class="alinhamento">
                                        <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btSalvar" runat="server" Text="Salvar" Width="100px" OnClick="btSalvar_Click" />&nbsp;&nbsp;&nbsp;
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div class="accountInfo"> 
                <fieldset>
                    <table border="0" width="2500px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div class="rounded_corners" id="scrollDiv">
                                    <asp:GridView ID="gvFiliaisConfiguracao" runat="server" Width="100.6%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white; margin-bottom: 0px;"
                                        ShowFooter="true" DataKeyNames="COD_FILIAL,FILIAL" OnDataBound="gvFiliaisConfiguracao_DataBound" OnRowDataBound="gvFiliaisConfiguracao_RowDataBound" >
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
	                                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                                    <RowStyle HorizontalAlign="Center"></RowStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="250px" SortExpression="FILIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" Text='<%# Bind("FILIAL") %>' runat="server"></asp:Literal>
                                                    <asp:HiddenField ID="hdnFilial" runat="server" Value='<%# Bind("COD_FILIAL") %>'></asp:HiddenField>
                                                    <asp:HiddenField ID="hdnCodigo" runat="server" Value='<%# Bind("CODIGO") %>'></asp:HiddenField>
                                                    <asp:LinkButton ID="lnkSel" runat="server" ForeColor="White">.</asp:LinkButton>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="250px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Tipo" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="200px" SortExpression="TIPO_FILIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAtiva" runat="server" Text='<%# Bind("TIPO_FILIAL") %>'></asp:Literal>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="200px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Pasta de Imagens de Depósito" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="500px" SortExpression="PASTA_IMAGENS_DEPOSITO">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPastaImagens" runat="server" Text='<%# Bind("PASTA_IMAGENS_DEPOSITO") %>' MaxLength="500" Width="500px" BorderStyle="None"></asp:TextBox>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="500px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Agora Rede" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="AGORA_REDE">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAgoraRede" runat="server" OnCheckedChanged="chkAgoraRede_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnAgoraRede" runat="server" Value='<%# Eval("AGORA_REDE") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Agora Supervisores" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="AGORA_SUPERVISORES">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAgoraSupervisores" runat="server" OnCheckedChanged="chkAgoraSupervisores_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnAgoraSupervisores" runat="server" Value='<%# Eval("AGORA_SUPERVISORES") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Administração" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_ADMINISTRACAO">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAdministracao" runat="server" OnCheckedChanged="chkAdministracao_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnAdministracao" runat="server" Value='<%# Eval("MOD_ADMINISTRACAO") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Gestão" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_GESTAO">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkGestao" runat="server" OnCheckedChanged="chkGestao_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnGestao" runat="server" Value='<%# Eval("MOD_GESTAO") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Atacado" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_ATACADO">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAtacado" runat="server" OnCheckedChanged="chkAtacado_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnAtacado" runat="server" Value='<%# Eval("MOD_ATACADO") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Financeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_FINANCEIRO">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkFinanceiro" runat="server" OnCheckedChanged="chkFinanceiro_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnFinanceiro" runat="server" Value='<%# Eval("MOD_FINANCEIRO") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Contabilidade" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_CONTABILIDADE">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkContabilidade" runat="server" OnCheckedChanged="chkContabilidade_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnContabilidade" runat="server" Value='<%# Eval("MOD_CONTABILIDADE") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Fiscal" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_FISCAL">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkFiscal" runat="server" OnCheckedChanged="chkFiscal_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnFiscal" runat="server" Value='<%# Eval("MOD_FISCAL") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Adm Nota Fiscal" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_ADM_NOTA_FISCAL">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAdmNotaFiscal" runat="server" OnCheckedChanged="chkAdmNotaFiscal_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnAdmNotaFiscala" runat="server" Value='<%# Eval("MOD_ADM_NOTA_FISCAL") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Administração Loja" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_ADMINISTRACAO_LOJA">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAdministracaoLoja" runat="server" OnCheckedChanged="chkAdministracaoLoja_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnAdministracaoLoja" runat="server" Value='<%# Eval("MOD_ADMINISTRACAO_LOJA") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Gerenciamento Loja" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_GERENCIAMENTO_LOJA">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkGerenciamentoLoja" runat="server" OnCheckedChanged="chkGerenciamentoLoja_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnGerenciamentoLoja" runat="server" Value='<%# Eval("MOD_GERENCIAMENTO_LOJA") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Acompanhamento Mensal" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_ACOMPANHAMENTO_MENSAL">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAcompanhamentoMensal" runat="server" OnCheckedChanged="chkAcompanhamentoMensal_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnAcompanhamentoMensal" runat="server" Value='<%# Eval("MOD_ACOMPANHAMENTO_MENSAL") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Controle de Estoque" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_CONTROLE_ESTOQUE">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkControleEstoque" runat="server" OnCheckedChanged="chkControleEstoque_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnControleEstoque" runat="server" Value='<%# Eval("MOD_CONTROLE_ESTOQUE") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Contagem" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_CONTAGEM">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkContagem" runat="server" OnCheckedChanged="chkContagem_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnContagem" runat="server" Value='<%# Eval("MOD_CONTAGEM") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="RH" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_RH">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRH" runat="server" OnCheckedChanged="chkRH_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnRH" runat="server" Value='<%# Eval("MOD_RH") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Representante" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="MOD_REPRESENTANTE">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRepresentante" runat="server" OnCheckedChanged="chkRepresentante_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnRepresentante" runat="server" Value='<%# Eval("MOD_REPRESENTANTE") %>'></asp:HiddenField>
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="100px"></ItemStyle>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            </asp:Panel>


            <span id="popMensagem" style="position:fixed; top: 100px; left: 500px;">
            <div id="divMensagem" class="rounded_modals">
                <asp:Panel ID="panMensagem" runat="server" width="505px" Height="200px" HorizontalAlign="left" Visible="false" BackColor="#EFF3FB" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="4px">
                    <table width="500px" cellpadding="0" cellspacing="0">
                        <tr style="height: 10px;">
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td width="18%" align="center" valign="top">
                                <img src="/Image/warning.png" alt="Alerta" />
                            </td>
                            <td width="82%">
                                <asp:TextBox ID="txtMensagemErro" runat="server" Text="" Width="400px" Height="150px" TextMode="MultiLine" ForeColor="Red" BorderStyle="None" ReadOnly="true" BackColor="#EFF3FB"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnFecharMensagem" runat="server" width="80px" 
                                    Text="OK" cssclass="buttonhbf" OnClick="btnFecharMensagem_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            </span>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
