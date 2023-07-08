<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManualUsuarioImportacao.aspx.cs" Inherits="Relatorios.ManualUsuarioImportacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Manual do Usuário</legend>
            1.  Localizar o menu "Importação de Produtos";</br></br></br>
            2.  Gestão</br></br></br>
            2.1 Trazer do LINX os produtos que serão partes integrantes da PROFORMA. Esses produtos devem ser marcados individualmente para importação através da opção 1 do menu;</br>
                Após marcado os produtos, deve-se clicar no botão "Salvar". Acionado este botão o sistema informará quantos produtos foram importados; </br></br></br>
            2.2 Alterar as quantidades dos produtos que integram a PROFORMA. A opção 2 do menu permite esta alteração. Na última coluna deve ser marcado que o produto deve ser alterado. </br>
                Deve-se clicar no botão "Salvar"; apenas os produtos marcados para alteração serão alterados; IMPORTANTE: Se o usuário definir um pack_grade e um pack_group, as quantidades</br>
                da grade serão definidos automaticamente; se não forem definidos pack_grade e pack_group, as quantidades da grade deverão ser digitadas;</br></br></br>
            2.3 Trazer da PROFORMA os produtos que serão partes integrantes da NeL. Esses produtos deverão ser marcados individualmente para vinculação através da opção 3 do menu.</br>
                Após marcado os produtos, deve-se clicar no botão "Salvar". Acionado este botão o sistema informará quantos produtos foram vinculados. Deve-se escolher qual NeL receberá </br>
                esses produtos. Lembre-se, um mesmo produto não poderá fazer parte de uma ou mais NeL dentro da mesma Janela; </br></br></br>
            2.4 Alterar as quantidades dos produtos que integram a NeL. A opção 4 do menu permite estas alterações. Na última coluna deve ser informado que o produto deve ser alterado. </br>
                Deve-se clicar no botão "Salvar". Apenas os produtos marcados para alteração serão alterados; </br></br></br>
            2.5 Alterar as quantidades dos produtos que integram a NeL Original. A opção 5 do menu permite estas alterações. Na última coluna deve ser informado que o produto deve ser alterado.  </br>
                Deve-se clicar no botão "Salvar". Apenas os produtos marcados para alteração serão alterados; </br></br></br>
            2.6 Nesta opção o usuário copia os valores da NeL Final para a NeL Original; </br></br></br>
            2.7 Alguns relatórios estão disponíveis: Por Container, Por NeL, Por Proforma, Por Produto, Por Grupo de Produto. </br></br></br>
            3.  Cadastros</br></br></br> 
            3.1 O cadastro da coleção deve ser feito utilizando-se a opção 1 do menu. O sistema permite ainda, a alteração e exclusão de uma coleção;</br></br></br>
            3.2 O cadastro da proforma deve ser feito utilizando-se a opção 2 do menu. O sistema já possui algumas proformas cadastradas; essas proformas devem ser usadas até que se defina</br>
                o código de identificação da proforma. Proformas pré cadastradas: 1-Atacado/Masculino/Peças, 2-Atacado/Feminino/Peças, 3-Varejo/Masculino/Peças, 4-Varejo/Feminino/Peças, </br>
                5-Varejo/Masculino/Acessórios e 6-Varejo/Feminino/Acessórios;</br></br></br>
            3.3 A opção 3 será usada para definir uma PROFORMA. A opção permite que se escolha a PRÉ e a PROFORMA definitiva. Lembre-se que a PROFORMA deve ser cadastrada antes dessa opção;</br></br></br>
            3.4 A opção 4 será usada para administrar a NeL. Este item deve ser usado para se cadastrar as informações referentes aos tramites legais pertinentes a NeL;</br></br></br>
            3.5 A opção 5 permitirá o cadastro, a alteração e exclusão do container;</br></br></br>
            3.6 A opção 6 permite o cadastro de quantidades por grade dos grupos de produto;</br></br></br>
            3.7 A opção 7 permite o cadastro de quantidades dos Pack´s;</br></br></br>
            3.8 Manual que pode tirar algumas dúvidas do usuário.
        </fieldset>
    </div>
</asp:Content>
