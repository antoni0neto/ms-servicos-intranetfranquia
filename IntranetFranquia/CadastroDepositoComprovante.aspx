<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CadastroDepositoComprovante.aspx.cs" Inherits="Relatorios.CadastroDepositoComprovante" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Cadastro de Comprovantes de Depósito</legend>
        </fieldset>
        <table border="1" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <div style="width: 200px;" class="alinhamento">
                            <label>Data Início:&nbsp; </label>
                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                            <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                        </div>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <div style="width: 200px;" class="alinhamento">
                            <label>Data Fim:&nbsp; </label>
                            <asp:TextBox ID="txtDataFim" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                            <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                        </div>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <div style="width: 200px;"  class="alinhamento">
                            <label>Filial:&nbsp; </label>
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px" Width="200px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                        </div>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <div>
                            <label>Valor:</label>
                            <asp:TextBox ID="txtValor" runat="server" CssClass="pcRight" MaxLength="20" Height="16px" Width="100px"></asp:TextBox>
                        </div>
                        <div>
                            <label>Observação:</label>
                            <asp:TextBox ID="txtObs" runat="server" CssClass="textEntry"  MaxLength="1000" Height="41px" Width="300px" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </fieldset>
                </td>
            </tr>
        </table>
        <div>
            <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click"/>
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewComprovantes" AutoGenerateColumns="false" onrowdatabound="GridViewComprovantes_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Filial">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LiteralFilial"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DATA_INICIO" HeaderText="Data Início" />
                <asp:BoundField DataField="DATA_FIM" HeaderText="Data Fim" />
                <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                <asp:BoundField DataField="OBS" HeaderText="Observação" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditarComprovante_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluirComprovante_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonAtualizar" Text="Atualizar" OnClick="ButtonAtualizarComprovante_Click"  CausesValidation="false" 
                                    OnClientClick="javascript: return confirm('Atualização com diferença, usar campo Obs. Tem certeza que deseja atualizar o depósito ?');"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoComprovante" Value="0" />
</asp:Content>
