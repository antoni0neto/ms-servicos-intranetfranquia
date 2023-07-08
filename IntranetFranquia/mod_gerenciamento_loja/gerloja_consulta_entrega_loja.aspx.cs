using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class gerloja_consulta_entrega_loja : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarGrupo();
                CarregarGriffe();
                CarregarFilial();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_PRODUTO_RECEBIDOLOJAResult> ObterProdutoRecebidoLoja()
        {
            var produtos = lojaController.ObterProdutoRecebidoLoja(ddlFilial.SelectedItem.Text.Trim(), ddlGriffe.SelectedValue.Trim(), ddlGrupo.SelectedValue.Trim(), txtProduto.Text.Trim());

            if (ddlCor.SelectedValue != null && ddlCor.SelectedValue != "")
                produtos = produtos.Where(p => p.DESC_COR.Trim() == ddlCor.SelectedItem.Text.Trim()).ToList();

            return produtos;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlFilial.SelectedValue == "")
                {
                    labErro.Text = "Por favor, selecione a Filial.";
                    return;
                }

                var produtoRecebido = ObterProdutoRecebidoLoja();

                gvProdutoRecebido.DataSource = produtoRecebido;
                gvProdutoRecebido.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvProdutoRecebido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_RECEBIDOLOJAResult produto = e.Row.DataItem as SP_OBTER_PRODUTO_RECEBIDOLOJAResult;

                    Label labDataEnviado = e.Row.FindControl("labDataEnviado") as Label;
                    labDataEnviado.Text = produto.ENVIADO.ToString("dd/MM/yyyy");

                    Label labDataRecebido = e.Row.FindControl("labDataRecebido") as Label;
                    labDataRecebido.Text = (produto.RECEBIDO == null) ? "-" : Convert.ToDateTime(produto.RECEBIDO).ToString("dd/MM/yyyy");
                }
            }
        }
        protected void gvProdutoRecebido_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvProdutoRecebido.FooterRow;
            if (_footer != null)
            {
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            var _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = (baseController.BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                var filiais = baseController.BuscaFiliais(usuario);

                filiais.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });

                ddlFilial.DataSource = filiais;
                ddlFilial.DataBind();

                if (filiais.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }

        #endregion

        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var codigoProduto = txtProduto.Text.Trim();

                var produto = baseController.BuscaProduto(codigoProduto);
                if (produto != null)
                {
                    var produtoCores = baseController.BuscaProdutoCores(produto.PRODUTO1);
                    if (produtoCores != null && produtoCores.Count() > 0)
                    {
                        produtoCores.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "", DESC_COR_PRODUTO = "" });

                        ddlCor.DataSource = produtoCores;
                        ddlCor.DataBind();
                    }
                    else
                    {
                        ddlCor.DataSource = new List<PRODUTO_CORE>();
                        ddlCor.DataBind();
                    }
                }
                else
                {
                    ddlCor.DataSource = new List<PRODUTO_CORE>();
                    ddlCor.DataBind();
                }
            }
            catch (Exception)
            {
            }
        }


    }
}
