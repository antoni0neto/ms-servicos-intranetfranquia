using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_tripa_prepedido_sub : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        List<SP_OBTER_PREPEDIDOResult> gPrePedido = new List<SP_OBTER_PREPEDIDOResult>();

        enum ORIGEM_PREPEDIDO
        {
            BUSCA = 1,
            INCLUSAO = 2,
            FILTRO = 3,
        }

        decimal gTotConsumo = 0;
        decimal gTotConsumoTotal = 0;
        decimal gTotTecido = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                CarregarColecoes();
                CarregarGriffe();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btAssociar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAssociar, null) + ";");

        }

        #region "DADOS INI"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

                ddlColecao.DataSource = colecoes;
                ddlColecao.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecao.SelectedValue = Session["COLECAO"].ToString();
                    CarregarOrigem(ddlColecao.SelectedValue);
                }
            }
        }
        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarOrigem(ddlColecao.SelectedValue);
        }
        private void CarregarOrigem(string colecao)
        {
            var origem = desenvController.ObterProdutoOrigem(colecao);

            if (origem != null)
            {
                origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigemFiltro.DataSource = origem;
                ddlOrigemFiltro.DataBind();

                if (origem.Count == 2)
                {
                    ddlOrigemFiltro.SelectedValue = origem[1].CODIGO.ToString();
                }
            }
        }
        private void CarregarGriffe()
        {
            var griffe = RetornarGriffe();

            if (griffe != null)
            {
                ddlGriffeFiltro.DataSource = griffe;
                ddlGriffeFiltro.DataBind();
            }
        }
        private List<PRODUTOS_GRIFFE> RetornarGriffe()
        {
            var griffes = baseController.BuscaGriffes();
            griffes.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
            return griffes;
        }
        private List<CORES_BASICA> RetornarCorLinx()
        {
            var coresBasicas = prodController.ObterCoresBasicas();
            coresBasicas = coresBasicas.GroupBy(p => new { COR = p.COR.Trim(), DESC_COR = p.DESC_COR.Trim() }).Select(x => new CORES_BASICA { COR = x.Key.COR.Trim(), DESC_COR = x.Key.DESC_COR.Trim() }).ToList();
            return coresBasicas;
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;
            return retorno;
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroAss.Text = "";

                if (ddlMarca.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Marca.";
                    pnlPrePedido.Visible = false;
                    return;
                }

                if (ddlColecao.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    pnlPrePedido.Visible = false;
                    return;
                }

                pnlPrePedido.Visible = true;

                CarregarPrePedido(ORIGEM_PREPEDIDO.BUSCA);
                CarregarGrupos();
                btAssociar.Enabled = true;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "FILTROS"
        private void CarregarFiltros(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            CarregarFiltroProduto(prePedido);
            CarregarFiltroTecido(prePedido);
            CarregarFiltroFornecedor(prePedido);
            CarregarFiltroCorFornecedor(prePedido);
            CarregarFiltroCorLinx(prePedido);
            CarregarFiltroGrupoProduto(prePedido);
        }
        private void CarregarFiltroProduto(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Where(p => p.PRODUTO != null).Select(p => p.PRODUTO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroProduto.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroProduto.DataBind();
        }
        private void CarregarFiltroTecido(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.TECIDO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroTecido.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroTecido.DataBind();
        }
        private void CarregarFiltroFornecedor(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.FORNECEDOR).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroFornecedor.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroFornecedor.DataBind();
        }
        private void CarregarFiltroCorFornecedor(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.COR_FORNECEDOR).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroCorFornecedor.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroCorFornecedor.DataBind();
        }
        private void CarregarFiltroCorLinx(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.DESC_COR).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroCorLinx.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroCorLinx.DataBind();
        }
        private void CarregarFiltroGrupoProduto(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.GRUPO_PRODUTO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroGrupoProduto.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroGrupoProduto.DataBind();
        }

        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarPrePedido(ORIGEM_PREPEDIDO.FILTRO);
        }

        #endregion

        private List<SP_OBTER_PREPEDIDOResult> ObterPrePedido()
        {
            var prePedido = desenvController.ObterPrePedidoMarca(ddlMarca.SelectedValue, ddlColecao.SelectedValue, ddlGriffeFiltro.SelectedValue);

            if (ddlOrigemFiltro.SelectedValue != "0")
                prePedido = prePedido.Where(p => p.DESENV_PRODUTO_ORIGEM == Convert.ToInt32(ddlOrigemFiltro.SelectedValue)).ToList();

            gPrePedido.AddRange(prePedido);
            //FILTROSSSSSSSSS
            var prePedidoFiltro = prePedido.AsEnumerable();

            if (ddlFiltroProduto.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.PRODUTO == ddlFiltroProduto.SelectedValue);

            if (ddlFiltroTecido.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.TECIDO == ddlFiltroTecido.SelectedValue);

            if (ddlFiltroFornecedor.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.FORNECEDOR == ddlFiltroFornecedor.SelectedValue);

            if (ddlFiltroCorFornecedor.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.COR_FORNECEDOR == ddlFiltroCorFornecedor.SelectedValue);

            if (ddlFiltroCorLinx.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.DESC_COR == ddlFiltroCorLinx.SelectedValue);

            if (ddlFiltroGrupoProduto.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.GRUPO_PRODUTO == ddlFiltroGrupoProduto.SelectedValue);

            if (ddlTemPedido.SelectedValue != "")
            {
                if (ddlTemPedido.SelectedValue == "S")
                    prePedidoFiltro = prePedidoFiltro.Where(p => p.CODIGO_PREPEDIDOSUB > 0).ToList();
                else
                    prePedidoFiltro = prePedidoFiltro.Where(p => p.CODIGO_PREPEDIDOSUB <= 0).ToList();
            }

            return prePedidoFiltro.ToList();
        }
        private void CarregarPrePedido(ORIGEM_PREPEDIDO o)
        {
            var prePedido = ObterPrePedido();

            gvPrePedido.DataSource = prePedido;
            gvPrePedido.DataBind();

            if (o != ORIGEM_PREPEDIDO.FILTRO)
                CarregarFiltros(gPrePedido);

        }
        protected void gvPrePedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PREPEDIDOResult pre = e.Row.DataItem as SP_OBTER_PREPEDIDOResult;

                    Literal litConsumo = e.Row.FindControl("litConsumo") as Literal;
                    litConsumo.Text = pre.CONSUMO.ToString("###,###,##0.000");

                    Literal litPrecoTecido = e.Row.FindControl("litPrecoTecido") as Literal;
                    litPrecoTecido.Text = "R$ " + pre.PRECO_TECIDO.ToString("###,###,###,##0.00");

                    Literal litConsumoTotal = e.Row.FindControl("litConsumoTotal") as Literal;
                    litConsumoTotal.Text = pre.CONSUMO_TOTAL.ToString("###,###,###,##0.000");

                    Literal litValorTecido = e.Row.FindControl("litValorTecido") as Literal;
                    litValorTecido.Text = "R$ " + (pre.PRECO_TECIDO * pre.CONSUMO_TOTAL).ToString("###,###,###,##0.00");

                    Literal litPedido = e.Row.FindControl("litPedido") as Literal;
                    litPedido.Text = (pre.NUMERO_PEDIDO == null) ? "-" : (pre.NUMERO_PEDIDO.ToString() + "-" + pre.ITEM.ToString());

                    CheckBox cbAssociar = e.Row.FindControl("cbAssociar") as CheckBox;
                    ImageButton btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                    if (pre.CODIGO_PREPEDIDOSUB == 0)
                    {
                        btExcluir.Visible = false;
                    }
                    else
                    {
                        cbAssociar.Checked = true;
                        cbAssociar.Enabled = false;
                    }



                    gTotConsumo += Convert.ToDecimal(pre.CONSUMO);
                    gTotConsumoTotal += Convert.ToDecimal(pre.CONSUMO_TOTAL);

                    gTotTecido += (pre.PRECO_TECIDO * pre.CONSUMO_TOTAL);
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvPrePedido_DataBound(object sender, EventArgs e)
        {
            var footer = gvPrePedido.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[8].Text = gTotConsumo.ToString("###,###,###,##0.000");
                footer.Cells[10].Text = gTotConsumoTotal.ToString("###,###,###,##0.000");

                footer.Cells[11].Text = "R$ " + gTotTecido.ToString("###,###,###,##0.00");

            }
        }
        protected void gvPrePedido_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_PREPEDIDOResult> prePedido = ObterPrePedido();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            prePedido = prePedido.OrderBy(e.SortExpression + sortDirection);
            gvPrePedido.DataSource = prePedido.ToList();
            gvPrePedido.DataBind();
        }

        private void CarregarGrupos()
        {
            var matGrupo = desenvController.ObterMaterialGrupo();
            if (matGrupo != null)
            {
                matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "Selecione" });
                ddlMaterialGrupo.DataSource = matGrupo;
                ddlMaterialGrupo.DataBind();
            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroAss.Text = "";

                CarregarSubGrupos(ddlMaterialGrupo.SelectedValue.Trim());
                CarregarPedidoItem("", ddlMaterialGrupo.SelectedValue.Trim(), "", "");
                btAssociar.Enabled = true;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarSubGrupos(string grupo)
        {
            var matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();

            matSubGrupo = matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
            matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });

            ddlMaterialSubGrupo.DataSource = matSubGrupo;
            ddlMaterialSubGrupo.DataBind();
        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                CarregarCores(ddlMaterialGrupo.SelectedValue.Trim(), ddlMaterialSubGrupo.SelectedValue.Trim());
                CarregarPedidoItem("", ddlMaterialGrupo.SelectedValue.Trim(), ddlMaterialSubGrupo.SelectedValue.Trim(), "");
                btAssociar.Enabled = true;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private List<CORES_BASICA> ObterCoresBasicas()
        {
            var cores = prodController.ObterCoresBasicas();
            return cores;
        }
        private void CarregarCores(string grupo, string subGrupo)
        {
            var filtroMaterial = desenvController.ObterMaterial(grupo.Trim(), subGrupo.Trim());

            var materialCores = desenvController.ObterMaterialCor();
            materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).ToList();

            var coresBasicas = ObterCoresBasicas();

            //Depois filtrar cores para carregar os filtros
            coresBasicas = coresBasicas.Where(p => materialCores.Any(x => x.COR_MATERIAL.Trim() == p.COR.Trim())).ToList();
            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

            ddlCor.DataSource = coresBasicas;
            ddlCor.DataBind();

            if (coresBasicas.Count == 2)
            {
                ddlCor.SelectedIndex = 1;
                ddlCor_SelectedIndexChanged(ddlCor, null);
            }

        }
        protected void ddlCor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                CarregarCorFornecedor(ddlMaterialGrupo.SelectedValue.Trim(), ddlMaterialSubGrupo.SelectedValue.Trim(), ddlCor.SelectedValue);
                CarregarPedidoItem("", ddlMaterialGrupo.SelectedValue.Trim(), ddlMaterialSubGrupo.SelectedValue.Trim(), ddlCor.SelectedValue);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarCorFornecedor(string grupo, string subGrupo, string cor)
        {
            var filtroMaterial = desenvController.ObterMaterial(grupo, subGrupo);
            var materialCores = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == cor.Trim()).ToList();

            materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).ToList();
            materialCores.Insert(0, new MATERIAIS_CORE { REFER_FABRICANTE = "" });

            ddlCorFornecedor.DataSource = materialCores.OrderBy(p => p.REFER_FABRICANTE);
            ddlCorFornecedor.DataBind();

            if (materialCores.Count == 2)
            {
                ddlCorFornecedor.SelectedIndex = 1;
            }
        }
        protected void cbAssociar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                labErroAss.Text = "";
                labErro.Text = "";

                CheckBox cb = (CheckBox)sender;
                GridViewRow row = (GridViewRow)cb.NamingContainer;
                if (cb.Checked)
                {
                    var codigoPrePedido = Convert.ToInt32(gvPrePedido.DataKeys[row.RowIndex][0].ToString());
                    var desenvPrePedido = desenvController.ObterPrePedido(codigoPrePedido);
                    if (desenvPrePedido != null)
                        CarregarPedidoItem(desenvPrePedido.TECIDO.Trim(), "", "", desenvPrePedido.COR.Trim());

                    row.BackColor = Color.PaleGreen;
                }
                else
                {
                    row.BackColor = Color.White;
                }

                row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(row.BackColor) + "';this.style.cursor='hand'");

                btAssociar.Enabled = true;
            }
            catch (Exception)
            {
            }
        }
        private void CarregarPedidoItem(string tecido, string grupo, string subGrupo, string corProduto)
        {
            var pedidoItem = new List<SP_OBTER_PEDIDOSUB_CONTROLEResult>();
            if (tecido == "")
            {
                pedidoItem = desenvController.ObterPedidoSubControle("", "", 0, grupo, subGrupo);

                if (corProduto != "")
                    pedidoItem = pedidoItem.Where(p => p.COR.Trim() == corProduto.Trim()).ToList();
            }
            else
            {
                pedidoItem = desenvController.ObterPedidoSubControle("", "", 0, "", "").Where(p =>
                    (p.GRUPO.Trim().Contains(tecido) || p.SUBGRUPO.Trim().Contains(tecido))
                    && p.COR.Trim() == corProduto.Trim()
                    ).ToList();
            }

            pedidoItem.Insert(0, new SP_OBTER_PEDIDOSUB_CONTROLEResult { CODIGO = 0, PEDIDO_ITEM_DESC = "Selecione" });
            ddlPedidoItem.DataSource = pedidoItem;
            ddlPedidoItem.DataBind();

            if (pedidoItem.Count == 2)
            {
                ddlPedidoItem.SelectedIndex = 1;
            }
        }

        protected void btAssociar_Click(object sender, EventArgs e)
        {
            try
            {
                labErroAss.Text = "";

                if (ddlPedidoItem.SelectedValue == "0")
                {
                    labErroAss.Text = "Selecione o Pedido para relacionar os produtos...";
                    return;
                }

                var codigoSubPedido = Convert.ToInt32(ddlPedidoItem.SelectedValue);
                foreach (GridViewRow row in gvPrePedido.Rows)
                {
                    var cb = ((CheckBox)row.FindControl("cbAssociar"));
                    var preCheck = cb.Checked;
                    var codigoPrePedido = Convert.ToInt32(gvPrePedido.DataKeys[row.RowIndex][0].ToString());

                    if (cb.Enabled && preCheck)
                    {
                        var existe = desenvController.ObterPrepedidoSub(codigoPrePedido, codigoSubPedido);
                        if (existe == null)
                        {
                            var preSub = new DESENV_PREPEDIDO_SUB();
                            preSub.DESENV_PEDIDO_SUB = codigoSubPedido;
                            preSub.DESENV_PREPEDIDO = codigoPrePedido;
                            preSub.DATA_INCLUSAO = DateTime.Now;
                            desenvController.InserirPrepedidoSub(preSub);
                        }
                    }
                }

                CarregarPrePedido(ORIGEM_PREPEDIDO.FILTRO);

                btAssociar.Enabled = false;
                labErroAss.Text = "Produtos do pré-pedido relacionados com sucesso.";

                ddlPedidoItem.SelectedValue = "0";
                ddlMaterialGrupo.SelectedValue = "Selecione";
                ddlMaterialSubGrupo.SelectedValue = "";
                ddlCor.SelectedValue = "";
                ddlCorFornecedor.SelectedValue = "";

            }
            catch (Exception ex)
            {
                labErroAss.Text = ex.Message;
            }
        }

        protected void btExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroAss.Text = "";

                ImageButton bt = (ImageButton)sender;
                GridViewRow row = (GridViewRow)bt.NamingContainer;

                var codigoPrePedidoSub = Convert.ToInt32(gvPrePedido.DataKeys[row.RowIndex][1].ToString());
                desenvController.ExcluirPrePedidoSubPorCodigo(codigoPrePedidoSub);
                CheckBox cb = row.FindControl("cbAssociar") as CheckBox;
                cb.Checked = false;
                cb.Enabled = true;
                bt.Visible = false;

            }
            catch (Exception ex)
            {
                labErroAss.Text = ex.Message;
            }
        }
    }
}

