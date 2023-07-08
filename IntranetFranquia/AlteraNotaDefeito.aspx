<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AlteraNotaDefeito.aspx.cs" Inherits="Relatorios.AlteraNotaDefeito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
        .style1
        {
            width: 100%;
            background-color:White;
        }
    </style>
    <script type="text/javascript" src="jquery-1.6.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input').keypress(function (e) {
                var code = null;
                code = (e.keyCode ? e.keyCode : e.which);
                return (code == 13) ? false : true;
            });
        });
        function SelectAllCheckboxes1(cbAlterado) {
            $('#<%=GridViewNotaRetirada.ClientID%>').find("input:checkbox").each(function () {
                if (this != cbAlterado) { this.checked = cbAlterado.checked; }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Alteração de Produtos</legend>
            <div>
                <asp:GridView runat="server" ID="GridViewNotaRetirada" AutoGenerateColumns="false" ShowFooter="true" onrowdatabound="GridViewNotaRetirada_RowDataBound" DataKeyNames="CODIGO_NOTA_RETIRADA">
                    <Columns>
                        <asp:BoundField DataField="CODIGO_NOTA_RETIRADA" HeaderText="Código" Visible = "false"/>
                        <asp:TemplateField HeaderText="Nota Cmax">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtNotaCmax" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nota Hbf">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtNotaHbf" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nota Calçados">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtNotaCalcados" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nota Outros">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtNotaOutros" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nota Lugzi">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtNotaLugzi" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alterado">
                            <HeaderTemplate>
                            <asp:CheckBox ID="cbSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes1(this);"  />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbAlterado" AutoPostBack="false"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btAlterar" Text="Alterar Nota" OnClick="btAlterar_Click"/>
            <asp:Label runat="server" ID="lblMensagem" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
