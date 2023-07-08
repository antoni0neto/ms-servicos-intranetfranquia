<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManualDepositoBanco.aspx.cs" Inherits="Relatorios.ManualDepositoBanco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
 				

COMO REGISTRAR NA INTRANET O DEPÓSITO EM BANCO OU A RETIRADA DE VALORES</br>
______________________________________________________________________________________________________</br></br>

1. Acionar a opção "3. Depósito em Banco" do menu no módulo "Financeiro";</br>
2. Selecionar no calendário as datas: Inicio e Fim, datas essas, referente a entradas de dinheiro na loja através de faturamento;</br>
3. Selecionar a filial e acionar o botão "Busca Movimento Dinheiro";</br>
4. No lado direito do vídeo será apresentado um calendário que terá marcada, na côr cinza, às datas com depósitos realizados nos últimos 30 dias;</br>
5. No quadro abaixo do botão "Busca Movimento Dinheiro" será apresentado um relatório diário dos valores que compõem o depósito;</br>
6. Abaixo do relatório diário o usuário deverá preencher as seguintes informações:</br>
 6.1. Valor do depósito;</br>
 6.2. Preencher seu nome no campo "Assinatura";</br>
 6.3. Selecionar a data do depósito;</br>
 6.4. Atenção !!!</br>
  6.4.1. O usuário deve fotografar o comprovante de depósito usando, de preferência, um celular;</br>
  6.4.2. Copiar a imagem gerada para o computador;</br>
  6.4.3. Acionar o botão "Procurar" para achar a imagem copiada;</br>
  6.4.4. Acionar o botão "Atualizar Imagem" para carregar a imagem para a Intranet. Podem ser carregadas até 4 imagens;</br>
7. O sistema vai apresentar ainda, às seguintes informações:</br>
 7.1. Valor a Depositar, valor este, que é a soma dos valores do relatório diário;</br>
 7.2. Diferença, que é a diferença entre o valor de depósito calculado pelo sistema e o valor digitado pelo usuário;</br>
 7.3. Diferença Anterior, que é a diferença nos valores provocados pelo depósito anterior;</br>
8. Para finalizar a rotina de depósito o usuário deve acionar o botão "Gravar Depósito" se o depósito foi feito em banco ou "Gravar Retirada" se os valores foram retirados por funcionário autorizado;</br>
9. Será apresentada a mensagem "Deseja confirmar o envio do depósito ?" ou "Deseja confirmar a retirada de dinheiro ?"; se o usuário responder que sim, o depósito será gravado e enviado à Fábrica.</br>  
        </fieldset>
    </div>
</asp:Content>
