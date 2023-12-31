﻿<%@ Page Title="Ecommerce +Vendidos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_mais_vendidos.aspx.cs" Inherits="Relatorios.ecom_mais_vendidos" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
        }

        .imgX {
            height: 326px;
            width: 100%;
            overflow: hidden;
            vertical-align: middle;
        }

            .imgX img {
                height: auto;
                width: 225px;
            }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Mais Vendidos</span>
            <div style="float: right; padding: 0;">
                <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <fieldset class="login">
            <legend>Mais Vendidos</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>Semana</td>
                    <td>Coleção</td>
                    <td>Griffe</td>
                    <td>Filial</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 350px;">
                        <asp:DropDownList runat="server" ID="ddlSemana" DataValueField="CODIGO" DataTextField="SEMANA"
                            Width="344px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 210px;">
                        <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Width="204px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 150px;">
                        <asp:DropDownList ID="ddlGriffe" runat="server" Width="144px" Height="21px" DataTextField="GRIFFE"
                            DataValueField="GRIFFE">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 260px;">
                        <asp:DropDownList runat="server" ID="ddlFilial" Width="254px">
                            <asp:ListItem Value="1054" Text="HANDBOOK ONLINE"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="5">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Button runat="server" ID="btBuscar" Text="Buscar 60+ Vendidos" Width="170px" OnClick="btBuscar_Click" />&nbsp;
                        <asp:Label runat="server" ID="labErro" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <fieldset>
                <asp:Repeater ID="repVendidos" runat="server" OnItemDataBound="repVendidos_ItemDataBound">
                    <ItemTemplate>
                        <table border="0" width="100%" cellpadding="2" cellspacing="2" style="background-color: #5F9EA0;">
                            <tr>
                                <td style="width: 20%">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: white;">
                                        <tr>
                                            <td colspan="2" style="text-align: center; border: solid 1px black; width: 20%; height: 330px;">
                                                <div class="imgX">
                                                    <asp:Image runat="server" ID="imgProduto1" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold;">Produto</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold;">Nome</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litProduto1" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black; text-align: left;">
                                                <asp:Literal runat="server" ID="litNome1" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Cor</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">&nbsp;</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litCor1" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litQtdeVendidaRede1" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Venda</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Estoque</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; border-bottom: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litQtdeVendidaLoja1" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-bottom: solid 1px black; border-right: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litEstoqueLoja1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 20%">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: white;">
                                        <tr>
                                            <td colspan="2" style="text-align: center; border: solid 1px black; width: 20%; height: 330px;">
                                                <div class="imgX">
                                                    <asp:Image runat="server" ID="imgProduto2" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold;">Produto</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold;">Nome</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litProduto2" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black; text-align: left;">
                                                <asp:Literal runat="server" ID="litNome2" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Cor</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">&nbsp;</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litCor2" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litQtdeVendidaRede2" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Venda</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Estoque</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; border-bottom: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litQtdeVendidaLoja2" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-bottom: solid 1px black; border-right: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litEstoqueLoja2" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 20%">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: white;">
                                        <tr>
                                            <td colspan="2" style="text-align: center; border: solid 1px black; width: 20%; height: 330px;">
                                                <div class="imgX">
                                                    <asp:Image runat="server" ID="imgProduto3" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold;">Produto</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold;">Nome</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litProduto3" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black; text-align: left;">
                                                <asp:Literal runat="server" ID="litNome3" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Cor</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">&nbsp;</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litCor3" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litQtdeVendidaRede3" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Venda</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Estoque</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; border-bottom: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litQtdeVendidaLoja3" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-bottom: solid 1px black; border-right: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litEstoqueLoja3" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 20%">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: white;">
                                        <tr>
                                            <td colspan="2" style="text-align: center; border: solid 1px black; width: 20%; height: 330px;">
                                                <div class="imgX">
                                                    <asp:Image runat="server" ID="imgProduto4" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold;">Produto</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold;">Nome</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litProduto4" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black; text-align: left;">
                                                <asp:Literal runat="server" ID="litNome4" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Cor</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">&nbsp;</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litCor4" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litQtdeVendidaRede4" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Venda</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Estoque</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; border-bottom: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litQtdeVendidaLoja4" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-bottom: solid 1px black; border-right: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litEstoqueLoja4" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 20%">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: white;">
                                        <tr>
                                            <td colspan="2" style="text-align: center; border: solid 1px black; width: 20%; height: 330px;">
                                                <div class="imgX">
                                                    <asp:Image runat="server" ID="imgProduto5" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold;">Produto</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold;">Nome</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litProduto5" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black; text-align: left;">
                                                <asp:Literal runat="server" ID="litNome5" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Cor</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">&nbsp;</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litCor5" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litQtdeVendidaRede5" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Venda</label>
                                            </td>
                                            <td style="border-left: solid 1px black; border-right: solid 1px black;">
                                                <label style="font-weight: bold; border-top: solid 1px black;">Estoque</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-left: solid 1px black; border-bottom: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litQtdeVendidaLoja5" />
                                            </td>
                                            <td style="border-left: solid 1px black; border-bottom: solid 1px black; border-right: solid 1px black; text-align: center;">
                                                <asp:Literal runat="server" ID="litEstoqueLoja5" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
            </fieldset>
        </fieldset>
    </div>
</asp:Content>
