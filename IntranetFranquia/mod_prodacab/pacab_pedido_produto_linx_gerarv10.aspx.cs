﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using System.Text;

namespace Relatorios
{
    public partial class pacab_pedido_produto_linx_gerarv10 : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        int qtdeCar = 0;
        decimal valCar = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                CarregarFilial();
                CarregarFabricante();
                CarregarCarrinhoLinx();

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFabricante()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor().ToList();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });
            _fornecedores.Where(p => ((p.STATUS == 'A' && p.TIPO == 'C') || p.STATUS == 'S')).GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (_fornecedores != null)
            {
                ddlFabricante.DataSource = _fornecedores;
                ddlFabricante.DataBind();
            }
        }
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais();

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                filial.Insert(1, new FILIAI { COD_FILIAL = "000029", FILIAL = "CD - LUGZY               " });
                filial.Insert(2, new FILIAI { COD_FILIAL = "000041", FILIAL = "CD MOSTRUARIO            " });
                filial.Insert(3, new FILIAI { COD_FILIAL = "1029", FILIAL = "ATACADO HANDBOOK         " });
                filial.Insert(4, new FILIAI { COD_FILIAL = "1054", FILIAL = "HANDBOOK ONLINE          " });

                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();

                ddlFilial.Enabled = true;
                if (filial.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFornecedor.ForeColor = _OK;
            if (ddlFabricante.SelectedValue.Trim() == "Selecione")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarGrade()
        {
            var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var carrinho = desenvController.ObterComprasCarrinhoLinx(codigoUsuario);

            if (carrinho == null || carrinho.Count() <= 0)
                return false;
            return true;
        }
        #endregion

        private void CarregarCarrinhoLinx()
        {
            var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var carrinho = desenvController.ObterComprasCarrinhoLinx(codigoUsuario);

            if (carrinho != null && carrinho.Count() > 0)
            {
                var compras = desenvController.ObterComprasPrePedido(carrinho[0].PEDIDO);
                if (compras != null)
                {
                    ddlFabricante.SelectedValue = compras.FORNECEDOR;
                    ddlFilial.SelectedValue = baseController.BuscaFilial(compras.FILIAL_A_ENTREGAR).COD_FILIAL;
                }
            }


            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_ACABADO_CARLINXResult carrinho = e.Row.DataItem as SP_OBTER_PRODUTO_ACABADO_CARLINXResult;

                    if (carrinho != null)
                    {
                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = carrinho.PRODUTO;

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = carrinho.DESC_PRODUTO.Trim();

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = carrinho.DESC_COR;

                        Literal litCusto = e.Row.FindControl("litCusto") as Literal;
                        litCusto.Text = (carrinho.CUSTO == null) ? "-" : ("R$ " + carrinho.CUSTO.ToString());

                        Literal litCustoNota = e.Row.FindControl("litCustoNota") as Literal;
                        litCustoNota.Text = (carrinho.CUSTO_NOTA == null) ? "-" : ("R$ " + carrinho.CUSTO_NOTA.ToString());

                        var pPrePedido = desenvController.ObterComprasProdutoPrePedido(carrinho.PEDIDO, carrinho.PRODUTO, carrinho.COR_PRODUTO, carrinho.ENTREGA);
                        Literal litGradeTotal = e.Row.FindControl("litGradeTotal") as Literal;
                        litGradeTotal.Text = pPrePedido.QTDE_ORIGINAL.ToString();

                        Literal litValTotal = e.Row.FindControl("litValTotal") as Literal;
                        var custo = Convert.ToDecimal((carrinho.CUSTO_NOTA != null) ? carrinho.CUSTO_NOTA : ((carrinho.CUSTO != null) ? carrinho.CUSTO : 0.00M));
                        var valor = Convert.ToDecimal(pPrePedido.QTDE_ORIGINAL) * custo;
                        litValTotal.Text = "R$ " + (valor).ToString("###,###,###,##0.00");

                        qtdeCar += pPrePedido.QTDE_ORIGINAL;
                        valCar += valor;

                    }
                }
            }
        }
        protected void gvCarrinho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCarrinho.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[6].Text = qtdeCar.ToString();
                footer.Cells[7].Text = "R$ " + valCar.ToString("###,###,###,##0.00");
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            string fornecedor;
            DateTime limiteEntrega = DateTime.Now.Date;
            string filial;
            string codigoFilialRateio;
            string aprovadoPor;

            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Por favor, preencher os campos em vermelho.";
                    return;
                }

                if (!ValidarGrade())
                {
                    labErro.Text = "Por favor, preencher a grade de todos os produtos.";
                    return;
                }

                fornecedor = ddlFabricante.SelectedValue.Trim();
                filial = ddlFilial.SelectedItem.Text.Trim();
                codigoFilialRateio = ddlFilial.SelectedValue;

                var usuario = (USUARIO)Session["USUARIO"];
                if (usuario != null)
                    aprovadoPor = usuario.NOME_USUARIO.Trim();
                else
                    aprovadoPor = "USUARIO-INTRANET";

                var pedidoCompra = desenvController.GerarLINXPedidoCompraProdutoPorPrePedidoV10(fornecedor, filial, codigoFilialRateio, 'A', aprovadoPor, usuario.CODIGO_USUARIO, "");

                if (pedidoCompra != null && pedidoCompra.NUMERO_PEDIDO != "")
                {
                    labErro.Text = "Pedido de Compra " + ddlFilial.SelectedItem.Text.Trim() + " gerado com sucesso no LINX: " + pedidoCompra.NUMERO_PEDIDO;

                    btSalvar.Enabled = false;
                }
                else
                {
                    labErro.Text = "Erro ao gerar pedido. Contate o TI.";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }


    }
}
