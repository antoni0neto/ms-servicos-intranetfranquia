<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CadastroChamadas.aspx.cs" Inherits="Relatorios.CadastroChamadas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Cadastro de Chamadas</legend>
        </fieldset>
        <table border="1" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <div>
                            <label>Requisitante:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </label>
                            <asp:DropDownList runat="server" ID="DropDownListRequisitante" DataValueField="CODIGO_USUARIO" DataTextField="NOME_USUARIO" Height="22px" Width="230px" ondatabound="DropDownListRequisitante_DataBound"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorRequisitante" ControlToValidate="DropDownListRequisitante" InitialValue="0" ValidationGroup="funcionario"
                                ErrorMessage="Selecione um Requisitante">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div style="width: 200px;" class="alinhamento">
                            <label>Data da Chamada:&nbsp; </label>
                            <asp:TextBox ID="TextBoxDataChamada" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                            <asp:Calendar ID="CalendarDataChamada" runat="server" OnSelectionChanged="CalendarDataChamada_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                        </div>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <div>
                            <label>DDD:&nbsp; </label>
                            <asp:TextBox ID="TextBoxDDD" runat="server" CssClass="textEntry" MaxLength="25" Height="22px" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorDDD" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo DDD" ControlToValidate="TextBoxDDD" ValidationGroup="funcionario"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <label>Telefone:&nbsp; </label>
                            <asp:TextBox ID="TextBoxTelefone" runat="server" CssClass="textEntry" MaxLength="25" Height="22px" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorTelefone" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Telefone" ControlToValidate="TextBoxTelefone" ValidationGroup="funcionario"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <label>Destino:&nbsp; </label>
                            <asp:TextBox ID="TextBoxDestino" runat="server" CssClass="textEntry" MaxLength="25" Height="22px" Width="400px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorDestino" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Destino" ControlToValidate="TextBoxDestino" ValidationGroup="funcionario"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <label>Tipo Telefone:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </label>
                            <asp:DropDownList runat="server" ID="DropDownListTipoTelefone" DataValueField="CODIGO" DataTextField="DESCRICAO" Height="22px" Width="230px" ondatabound="DropDownListTipoTelefone_DataBound"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorTipoTelefone" ControlToValidate="DropDownListTipoTelefone" InitialValue="0" ValidationGroup="funcionario" ErrorMessage="Selecione um Tipo de Telefone"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <label>Tipo Destino:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </label>
                            <asp:DropDownList runat="server" ID="DropDownListTipoDestino" DataValueField="CODIGO" DataTextField="DESCRICAO" Height="22px" Width="230px" ondatabound="DropDownListTipoDestino_DataBound"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorTipoDestino" ControlToValidate="DropDownListTipoDestino" InitialValue="0" ValidationGroup="funcionario" ErrorMessage="Selecione um Tipo de Destino"></asp:RequiredFieldValidator>
                        </div>
                    </fieldset>
                </td>
            </tr>
        </table>
        <div>
            <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="funcionario"/>
            <asp:ValidationSummary ID="ValidationSummaryFuncionario" runat="server" ValidationGroup="funcionario" ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewChamadas" AutoGenerateColumns="false" onrowdatabound="GridViewChamadas_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Requisitante">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LiteralRequisitante"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DATA_OCORRENCIA" HeaderText="Data da Ocorrência" />
                <asp:BoundField DataField="DDD" HeaderText="DDD" />
                <asp:BoundField DataField="TELEFONE" HeaderText="Telefone" />
                <asp:BoundField DataField="DESTINO" HeaderText="Destino" />
                <asp:TemplateField HeaderText="Tipo de Telefone">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LiteralTipoTelefone"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo de Destino">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LiteralTipoDestino"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditarChamada_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluirChamada_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoChamada" Value="0" />
</asp:Content>
