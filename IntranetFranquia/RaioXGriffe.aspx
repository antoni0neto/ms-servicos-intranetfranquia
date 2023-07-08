<%@ Page Title="Raio X de Peças por Griffe de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="RaioXGriffe.aspx.cs" Inherits="Relatorios.RaioXGriffe" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
        .style1
        {
            width: 100%;
        }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <table>
                <tr>
                    <td>
                        <label>Categoria:&nbsp; </label>
                        <asp:DropDownList runat="server" ID="ddlCategoria" DataValueField="COD_CATEGORIA" DataTextField="CATEGORIA_PRODUTO" Height="22px" 
                            Width="198px" ondatabound="ddlCategoria_DataBound"></asp:DropDownList>
                    </td>
                    <td>
                        <label>Coleção:&nbsp; </label>
                        <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" 
                            DataTextField="DESC_COLECAO" Height="22px" 
                            Width="198px" ondatabound="ddlColecao_DataBound" autopostback = "true"
                            onselectedindexchanged="ddlColecao_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>
                        <label>Griffe:&nbsp; </label>
                        <asp:DropDownList runat="server" ID="ddlGriffe" DataValueField="COD_GRIFFE" DataTextField="GRIFFE" Height="22px" 
                            Width="198px" autopostback = "true" ondatabound="ddlGriffe_DataBound"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </fieldset>
        <asp:Button runat="server" ID="btMovimento" Text="Buscar Movimento dos Produtos" OnClick="btMovimento_Click"/>
        <div>
            <br />
        </div>
        <table border="1" class="style1">
            <tr>
                <td>
                   <asp:Image runat="server" ID="ImageProduto" Width="" Height=""/>
                </td>
                <td>
                    <fieldset class="login">
                        <legend>Lucratividade</legend>
                        <table border="1" class="style1">
                            <tr>
                                <td>
                                    <label>% Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralPercVenda"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Dias</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNumeroDias"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Resultado</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralResultado"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Preço</label>
                                </td>
                                <td>
                                    <label>Resultado</label>
                                </td>
                                <td>
                                    <label>% Resultado</label>
                                </td>
                                <td>
                                    <label>Markup</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Inicial Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralPrecoInicialVarejo"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralResultadoInicialVarejo"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralPercResultadoInicialVarejo"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarkupInicial"/>
                                </td>
                                <td>
                                    <label>Markup Atual Varejo</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Atual Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralPrecoAtualVarejo"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralResultadoAtualVarejo"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralPercResultadoAtualVarejo"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarkupAtual"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarkupAtualVarejo"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Atacado</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralPrecoAtacado"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Markup Final Varejo</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Franquia</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralPrecoFranquia"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralResultadoFranquia"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarkupFinalVarejo"/>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                    <legend>Resumo Mensal Varejo</legend>
                        <table border="1" class="style1">
                            <tr>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Venda/Mês</label>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Venda/Mês</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Janeiro</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiro"/>
                                </td>
                                <td>
                                    <label>Julho</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulho"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Fevereiro</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiro"/>
                                </td>
                                <td>
                                    <label>Agosto</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgosto"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Março</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarco"/>
                                </td>
                                <td>
                                    <label>Setembro</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembro"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Abril</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbril"/>
                                </td>
                                <td>
                                    <label>Outubro</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubro"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Maio</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaio"/>
                                </td>
                                <td>
                                    <label>Novembro</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembro"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Junho</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunho"/>
                                </td>
                                <td>
                                    <label>Dezembro</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembro"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralTotalVarejoMensal"/>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="1" class="style1">                
                        <tr>
                            <td>
                                <label>Data Final das Vendas</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDataFinalVenda"/>
                            </td>
                        </tr> 
                        <tr>
                            <td>
                                <label>Data de Início das Vendas</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDataInicioVenda"/>
                            </td>
                        </tr> 
                    </table>                
                    <fieldset class="login">
                        <legend>Produto</legend>
                        <table border="1" class="style1">
                            <tr>
                                <td>
                                    <label>Código</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralCodigoProduto"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Descrição</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralDescricaoProduto"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Grupo</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralGrupo"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Sub Grupo</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralSubGrupo"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Tipo</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralTipo"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Linha</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralLinha"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Cor</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralCor"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Final</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralPrecoFinal"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Coleção</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralColecao"/>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset class="login">
                        <legend>Preço</legend>
                        <table border="1" class="style1">
                            <tr>
                                <td>
                                    <label>US$ FOB</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralUssFob"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>TX CONV.</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralTxConv"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>CUSTO VAREJO</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralCustoVarejo"/>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset class="login">
                        <legend>Testador</legend>
                        <table border="1" class="style1">
                            <tr>
                                <td>
                                    <label>Preço Mín. Var./ Testador</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralPrecoTestador"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Resultado</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralResultadoTestador"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Lucratividade</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralLucratividadeTestador"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Markup</label>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="LiteralMarkupTestador"/>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <legend>Resumo Varejo</legend>
                        <table border="1" class="style1">
                            <tr>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Varejo</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Pedido/Produção</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralPedidoProducao"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Entrada</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralEntrada"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>(+) Transferido Atacado</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralTransferidoAtacado"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>(-) Venda Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralVendasVarejo"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>(-) Transferido Franquia</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralTransferidoFranquia"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>(-) Segunda/Perda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSegundaPerda"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Estoque Final</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralEstoqueFinal"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Peças</label>
                                </td>
                                <td>
                                    <label>Valor</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Venda Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralQtdeVendasVarejo"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralValorVendasVarejo"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Venda Atacado</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralQtdeVendasAtacado"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralValorVendasAtacado"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Venda Franquia</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralQtdeVendasFranquia"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralValorVendasFranquia"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Venda Total</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralQtdeVendasTotal"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralValorVendasTotal"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Estoque Total</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralQtdeEstoqueTotal"/>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset class="login">
                    <legend>Resumo Atacado</legend>
                        <table border="1" class="style1">
                            <tr>
                                <td>
                                    <label>Estoque Inicial</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralEstoqueInicialAtacado"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>(-) Reserva</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralEntregarAtacado"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>(-) Faturado</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralEntregueAtacado"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>(-) Estoque Transferido</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralEstoqueTransferidoAtacado"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Estoque Disponível</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralEstoqueDisponivelAtacado"/>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                    <legend>Detalhe Varejo</legend>
                        <table border="1" class="style1" align="right">
                            <tr>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Janeiro</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>44</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>45</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                       <label>46</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>47</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>48</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Julho</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>18</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>19</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>20</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>21</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>22</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroQtde44"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroQtde45"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroQtde46"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroQtde47"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroQtde48"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroQtdeTotal"/>
                                </td>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoQtde18"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoQtde19"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoQtde20"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoQtde21"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoQtde22"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoQtdeTotal"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroPcVarejo44"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroPcVarejo45"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroPcVarejo46"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroPcVarejo47"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroPcVarejo48"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoPcVarejo18"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoPcVarejo19"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoPcVarejo20"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoPcVarejo21"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoPcVarejo22"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroSemanaVd44"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroSemanaVd45"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroSemanaVd46"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroSemanaVd47"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJaneiroSemanaVd48"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoSemanaVd18"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoSemanaVd19"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoSemanaVd20"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoSemanaVd21"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJulhoSemanaVd22"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Fevereiro</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>49</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>50</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>51</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>52</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>53</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Agosto</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>23</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>24</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>25</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>26</label>
                                    </h8>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroQtde49"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroQtde50"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroQtde51"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroQtde52"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroQtde53"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroQtdeTotal"/>
                                </td>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoQtde23"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoQtde24"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoQtde25"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoQtde26"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoQtdeTotal"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroPcVarejo49"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroPcVarejo50"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroPcVarejo51"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroPcVarejo52"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroPcVarejo53"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoPcVarejo23"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoPcVarejo24"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoPcVarejo25"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoPcVarejo26"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroSemanaVd49"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroSemanaVd50"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroSemanaVd51"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroSemanaVd52"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralFevereiroSemanaVd53"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoSemanaVd23"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoSemanaVd24"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoSemanaVd25"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAgostoSemanaVd26"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Março</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>1</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>2</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>3</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>4</label>
                                    </h8>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Setembro</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>27</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>28</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>29</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>30</label>
                                    </h8>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoQtde1"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoQtde2"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoQtde3"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoQtde4"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoQtdeTotal"/>
                                </td>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroQtde27"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroQtde28"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroQtde29"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroQtde30"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroQtdeTotal"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoPcVarejo1"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoPcVarejo2"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoPcVarejo3"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoPcVarejo4"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroPcVarejo27"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroPcVarejo28"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroPcVarejo29"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroPcVarejo30"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoSemanaVd1"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoSemanaVd2"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoSemanaVd3"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMarcoSemanaVd4"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroSemanaVd27"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroSemanaVd28"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroSemanaVd29"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralSetembroSemanaVd30"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Abril</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>5</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>6</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>7</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>8</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>9</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Outubro</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>31</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>32</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>33</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>34</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>35</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilQtde5"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilQtde6"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilQtde7"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilQtde8"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilQtde9"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilQtdeTotal"/>
                                </td>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroQtde31"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroQtde32"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroQtde33"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroQtde34"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroQtde35"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroQtdeTotal"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilPcVarejo5"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilPcVarejo6"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilPcVarejo7"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilPcVarejo8"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilPcVarejo9"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroPcVarejo31"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroPcVarejo32"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroPcVarejo33"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroPcVarejo34"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroPcVarejo35"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilSemanaVd5"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilSemanaVd6"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilSemanaVd7"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilSemanaVd8"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralAbrilSemanaVd9"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroSemanaVd31"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroSemanaVd32"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroSemanaVd33"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroSemanaVd34"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralOutubroSemanaVd35"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Maio</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>10</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>11</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>12</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>13</label>
                                    </h8>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Novembro</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>36</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>37</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>38</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>39</label>
                                    </h8>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioQtde10"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioQtde11"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioQtde12"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioQtde13"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioQtdeTotal"/>
                                </td>
                                <td align="right">
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroQtde36"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroQtde37"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroQtde38"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroQtde39"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroQtdeTotal"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioPcVarejo10"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioPcVarejo11"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioPcVarejo12"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioPcVarejo13"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroPcVarejo36"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroPcVarejo37"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroPcVarejo38"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroPcVarejo39"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioSemanaVd10"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioSemanaVd11"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioSemanaVd12"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralMaioSemanaVd13"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroSemanaVd36"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroSemanaVd37"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroSemanaVd38"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralNovembroSemanaVd39"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Junho</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>14</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>15</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>16</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>17</label>
                                    </h8>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Dezembro</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>40</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>41</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>42</label>
                                    </h8>
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>43</label>
                                    </h8>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <h8 style="color:Navy">
                                        <label>Total</label>
                                    </h8>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoQtde14"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoQtde15"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoQtde16"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoQtde17"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoQtdeTotal"/>
                                </td>
                                <td>
                                    <label>Quantidade</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroQtde40"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroQtde41"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroQtde42"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroQtde43"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroQtdeTotal"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoPcVarejo14"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoPcVarejo15"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoPcVarejo16"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoPcVarejo17"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Preço Varejo</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroPcVarejo40"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroPcVarejo41"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroPcVarejo42"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroPcVarejo43"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoSemanaVd14"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoSemanaVd15"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoSemanaVd16"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralJunhoSemanaVd17"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <label>Semanas de Venda</label>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroSemanaVd40"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroSemanaVd41"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroSemanaVd42"/>
                                </td>
                                <td align="right">
                                    <asp:Literal runat="server" ID="LiteralDezembroSemanaVd43"/>
                                </td>
                                <td>
                                    <br />
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
