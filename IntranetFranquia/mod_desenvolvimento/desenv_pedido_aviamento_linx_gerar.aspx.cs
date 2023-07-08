using System;
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
    public partial class desenv_pedido_aviamento_linx_gerar : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        decimal qtdeMaterial = 0;
        decimal valTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                CarregarFilial();
                CarregarFabricante();


                var codigoCarrinhoLinxCab = "";
                if (Request.QueryString["ccc"] != null)
                    codigoCarrinhoLinxCab = Request.QueryString["ccc"].ToString();

                hidCodigoCarrinhoLinxCab.Value = codigoCarrinhoLinxCab;

                CarregarCarrinhoLinx(codigoCarrinhoLinxCab);

                var carrinhoLinxCab = desenvController.ObterCarrinhoMaterialLinxCab(Convert.ToInt32(codigoCarrinhoLinxCab));

                labPedidoTipo.Text = carrinhoLinxCab.DESENV_MATERIAL_CARRINHO_CAB1.DESENV_MATERIAL_PEDIDO_PERFIL1.PERFIL;
                hidPedidoTipo.Value = carrinhoLinxCab.DESENV_MATERIAL_CARRINHO_CAB1.DESENV_MATERIAL_PEDIDO_PERFIL.ToString();
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFabricante()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor().ToList();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });
            _fornecedores.Where(p => ((p.STATUS == 'A' && p.TIPO == 'A') || p.STATUS == 'S')).GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

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
                filial.Insert(1, new FILIAI { COD_FILIAL = "1055", FILIAL = "C-MAX (NOVA)" });
                filial.Insert(2, new FILIAI { COD_FILIAL = "000029", FILIAL = "CD - LUGZY               " });
                filial.Insert(3, new FILIAI { COD_FILIAL = "000041", FILIAL = "CD MOSTRUARIO            " });
                filial.Insert(4, new FILIAI { COD_FILIAL = "1029", FILIAL = "ATACADO HANDBOOK         " });
                filial.Insert(5, new FILIAI { COD_FILIAL = "1054", FILIAL = "HANDBOOK ONLINE          " });

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
            var carrinho = desenvController.ObterMaterialCarrinhoLinxPrePedido(Convert.ToInt32(hidCodigoCarrinhoLinxCab.Value));
            if (carrinho == null || carrinho.Count() <= 0)
                return false;
            return true;
        }
        private bool ValidarCusto()
        {
            // validar custo // pendnete

            return true;
        }
        #endregion

        private void CarregarCarrinhoLinx(string codigoCarrinhoLinxCab)
        {
            var carrinho = desenvController.ObterMaterialCarrinhoLinxPrePedido(Convert.ToInt32(codigoCarrinhoLinxCab));

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
                    SP_OBTER_MATERIAL_CARRINHO_LINX_PREPEDIDOResult carrinho = e.Row.DataItem as SP_OBTER_MATERIAL_CARRINHO_LINX_PREPEDIDOResult;

                    if (carrinho != null)
                    {
                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = carrinho.PRODUTO;

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = carrinho.DESC_PRODUTO;

                        Literal litSubGrupo = e.Row.FindControl("litSubGrupo") as Literal;
                        litSubGrupo.Text = carrinho.SUBGRUPO_MATERIAL;

                        Literal litCorMaterial = e.Row.FindControl("litCorMaterial") as Literal;
                        litCorMaterial.Text = carrinho.COR_FORNECEDOR;

                        Literal litQtde = e.Row.FindControl("litQtde") as Literal;
                        litQtde.Text = carrinho.QTDE.ToString();

                        Literal litCusto = e.Row.FindControl("litCusto") as Literal;
                        litCusto.Text = "R$ " + carrinho.CUSTO.ToString();

                        Literal litDesconto = e.Row.FindControl("litDesconto") as Literal;
                        litDesconto.Text = "R$ " + carrinho.DESCONTO_ITEM.ToString();

                        Literal litConsumoTotal = e.Row.FindControl("litConsumoTotal") as Literal;
                        litConsumoTotal.Text = "R$" + Convert.ToDecimal(carrinho.VAL_TOTAL).ToString("###,###,###,##0.0000");

                        qtdeMaterial += carrinho.QTDE;
                        valTotal += Convert.ToDecimal(carrinho.VAL_TOTAL);

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
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                footer.Cells[5].Text = qtdeMaterial.ToString();
                footer.Cells[8].Text = "R$ " + valTotal.ToString("###,###,###,##0.0000");
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

                if (!ValidarCusto())
                {
                    labErro.Text = "Este pedido possui o mesmo material com custo diferente. Por favor, valide os custos dos materiais.";
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

                var pedidoCompra = desenvController.GerarLINXPedidoCompraMaterialPorPrePedido(Convert.ToInt32(hidCodigoCarrinhoLinxCab.Value), fornecedor, filial, codigoFilialRateio, 'A', aprovadoPor, usuario.CODIGO_USUARIO);

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
