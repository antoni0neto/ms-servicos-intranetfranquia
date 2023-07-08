using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;

namespace Relatorios
{
    public partial class estoque_mercadoria_recebimento : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        EstoqueController estoqueController = new EstoqueController();
        ProducaoController prodController = new ProducaoController();

        int coluna = 0, colunaProduto = 0, colunaNovo = 0;
        List<ESTOQUE_LOJA_NF_RECEB_PRODUTO> g_produto = new List<ESTOQUE_LOJA_NF_RECEB_PRODUTO>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarFilial();

                Session["NF_PRODUTO_MAIS"] = null;
                pnlProdutoNovo.Visible = false;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btBaixarProduto.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBaixarProduto, null) + ";");
        }

        #region "GRID INICIAL"
        private void RecarregarNotaMercadoria()
        {
            //Obter Notas no LINX
            var _notaMercadoria = estoqueController.ObterNFMercadoriaLoja(ddlFilial.SelectedItem.Text.Trim());

            if (ddlFilial.SelectedItem.Text.Trim() == "FORTALEZA RIOMAR")
                _notaMercadoria = _notaMercadoria.Where(p => p.NUMERO_NF_TRANSFERENCIA.Trim() != "018503" ||
                                                        p.SERIE_NF_ENTRADA.Trim() != "11" ||
                                                        p.FILIAL_ORIGEM.Trim() != "CD - LUGZY" ||
                                                        p.QTDE_TOTAL.ToString() != "4377").ToList();

            if (_notaMercadoria != null)
            {
                gvNotaMercadoria.DataSource = _notaMercadoria;
                gvNotaMercadoria.DataBind();
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            if (ddlFilial.SelectedValue == "" || ddlFilial.SelectedValue == "0")
            {
                labErro.Text = "Selecione a Filial.";
                return;
            }

            try
            {
                RecarregarNotaMercadoria();
                LimparGVProduto();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        protected void btVerificarNota_Click(object sender, EventArgs e)
        {
            string romaneioProduto = "";
            string filial = "";
            string msg = "";
            Button bt = (Button)sender;
            if (bt != null)
            {

                try
                {
                    labBaixa.Text = "";
                    Session["NF_PRODUTO_MAIS"] = null;

                    GridViewRow rowSelected = (GridViewRow)bt.NamingContainer;
                    if (rowSelected != null)
                    {
                        hidNome.Value = "";
                        hidNome.Value = ObterValorControle(rowSelected, "txtNome", typeof(TextBox));
                        if (hidNome.Value.Trim() == "")
                        {
                            msg = "Informe o campo RECEBIDOR POR.";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        romaneioProduto = bt.CommandArgument.Trim();
                        filial = ddlFilial.SelectedItem.Text.Trim();
                        hidEmissao.Value = rowSelected.Cells[1].Text;
                        hidNF.Value = rowSelected.Cells[2].Text;
                        hidSerie.Value = rowSelected.Cells[3].Text;

                        //WTF??????
                        string sEmpresaEmissora = rowSelected.Cells[4].Text.Trim();
                        sEmpresaEmissora = sEmpresaEmissora.Replace("&#205;", "Í");
                        sEmpresaEmissora = sEmpresaEmissora.Replace("&#201;", "É");
                        sEmpresaEmissora = sEmpresaEmissora.Replace("&#170;", "ª");
                        hidEmpresaEmissora.Value = sEmpresaEmissora;
                        //*******************

                        hidQtdeTotal.Value = rowSelected.Cells[5].Text;
                        hidRomaneio.Value = romaneioProduto;

                        //Recarregar grid de produtos novos
                        RecarregarProdutoNovo();

                        foreach (GridViewRow row in gvNotaMercadoria.Rows)
                            if (row != null)
                                row.ForeColor = Color.Gray;

                        rowSelected.ForeColor = Color.Red;
                        pnlProdutoNovo.Visible = true;

                        //Abrir grid de Produtos para informar as divergências
                        List<SP_OBTER_NF_MERCADORIA_LOJA_PRODUTOResult> notaMercadoriaProduto = new List<SP_OBTER_NF_MERCADORIA_LOJA_PRODUTOResult>();
                        notaMercadoriaProduto = estoqueController.ObterNFMercadoriaLojaProduto(filial, romaneioProduto);

                        gvProduto.DataSource = notaMercadoriaProduto;
                        gvProduto.DataBind();

                        btBaixarProduto.Visible = false;
                        if (notaMercadoriaProduto.Count > 0)
                            btBaixarProduto.Visible = true;

                        CheckBox cb = null;
                        foreach (GridViewRow row in gvProduto.Rows)
                        {
                            if (row != null)
                            {
                                cb = row.FindControl("chkOK") as CheckBox;
                                cb.Checked = true;

                                DesabilitarCamposGrade(row, "txtGradeEXP", false);
                                DesabilitarCamposGrade(row, "txtGradeXP", false);
                                DesabilitarCamposGrade(row, "txtGradePP", false);
                                DesabilitarCamposGrade(row, "txtGradeP", false);
                                DesabilitarCamposGrade(row, "txtGradeM", false);
                                DesabilitarCamposGrade(row, "txtGradeG", false);
                                DesabilitarCamposGrade(row, "txtGradeGG", false);

                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void gvNotaMercadoria_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_NF_MERCADORIA_LOJAResult _notaMercadoria = e.Row.DataItem as SP_OBTER_NF_MERCADORIA_LOJAResult;

                    coluna += 1;
                    if (_notaMercadoria != null)
                    {
                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = coluna.ToString();

                        Literal _litValorNota = e.Row.FindControl("litValorNota") as Literal;
                        if (_litValorNota != null)
                            _litValorNota.Text = "R$ " + Convert.ToDecimal(_notaMercadoria.VALOR_TOTAL).ToString("###,###,###,###,##0.00");

                        Button _btVerificarNota = e.Row.FindControl("btVerificarNota") as Button;
                        if (_btVerificarNota != null)
                            _btVerificarNota.CommandArgument = _notaMercadoria.ROMANEIO_PRODUTO.ToString();
                    }
                }
            }
        }
        #endregion

        #region "GRID PRODUTO"
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_NF_MERCADORIA_LOJA_PRODUTOResult _notaMercadoriaProduto = e.Row.DataItem as SP_OBTER_NF_MERCADORIA_LOJA_PRODUTOResult;

                    colunaProduto += 1;
                    if (_notaMercadoriaProduto != null)
                    {
                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = colunaProduto.ToString();

                        HiddenField _hidCor = e.Row.FindControl("hidCor") as HiddenField;
                        if (_hidCor != null)
                            _hidCor.Value = _notaMercadoriaProduto.COR_PRODUTO;
                    }
                }
            }
        }
        protected void chkOK_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb != null)
            {
                GridViewRow row = (GridViewRow)cb.NamingContainer;
                if (row != null)
                {
                    DesabilitarCamposGrade(row, "txtGradeEXP", !cb.Checked);
                    DesabilitarCamposGrade(row, "txtGradeXP", !cb.Checked);
                    DesabilitarCamposGrade(row, "txtGradePP", !cb.Checked);
                    DesabilitarCamposGrade(row, "txtGradeP", !cb.Checked);
                    DesabilitarCamposGrade(row, "txtGradeM", !cb.Checked);
                    DesabilitarCamposGrade(row, "txtGradeG", !cb.Checked);
                    DesabilitarCamposGrade(row, "txtGradeGG", !cb.Checked);
                }
            }
        }
        private string ObterValorControle(GridViewRow row, string id, Type tipo)
        {
            string retorno = "";

            if (tipo == typeof(TextBox))
            {
                TextBox txt = row.FindControl(id) as TextBox;
                if (txt != null)
                    retorno = txt.Text;
            }
            else if (tipo == typeof(DropDownList))
            {
                DropDownList ddl = row.FindControl(id) as DropDownList;
                if (ddl != null)
                    retorno = ddl.SelectedValue;
            }
            else if (tipo == typeof(HiddenField))
            {
                HiddenField hid = row.FindControl(id) as HiddenField;
                if (hid != null)
                    retorno = hid.Value;
            }
            else if (tipo == typeof(CheckBox))
            {
                CheckBox chk = row.FindControl(id) as CheckBox;
                if (chk != null)
                    retorno = (chk.Checked) ? "S" : "N";
            }
            else if (tipo == typeof(Label))
            {
                Label lab = row.FindControl(id) as Label;
                if (lab != null)
                    retorno = lab.Text;
            }
            else if (tipo == typeof(Literal))
            {
                Literal lit = row.FindControl(id) as Literal;
                if (lit != null)
                    retorno = lit.Text;
            }
            return retorno;
        }
        private void LimparGVProduto()
        {
            hidNF.Value = "";
            hidEmissao.Value = "";
            hidSerie.Value = "";
            hidEmpresaEmissora.Value = "";
            hidQtdeTotal.Value = "";
            hidRomaneio.Value = "";
            hidNome.Value = "";

            pnlProdutoNovo.Visible = false;
            gvProduto.DataSource = null;
            gvProduto.DataBind();

            btBaixarProduto.Visible = false;
        }
        private void DesabilitarCamposGrade(GridViewRow row, string id, bool enable)
        {
            TextBox txt = row.FindControl(id) as TextBox;
            if (txt != null)
            {
                txt.Enabled = enable;
                txt.Text = "";
            }
        }
        #endregion

        #region "GRID PRODUTO NOVO"
        private void RecarregarProdutoNovo()
        {
            //List<ESTOQUE_LOJA_NF_RECEB_PRODUTO> estoqueProduto = new List<ESTOQUE_LOJA_NF_RECEB_PRODUTO>();
            //Obter produtos adicionados nao baixados da nota
            //estoqueProduto = estoqueController.ObterEstoqueLojaRecebProdutoPorNota(filial, romaneioProduto).Where(p => p.STATUS == 'N').ToList();

            if (Session["NF_PRODUTO_MAIS"] != null)
                g_produto = (List<ESTOQUE_LOJA_NF_RECEB_PRODUTO>)Session["NF_PRODUTO_MAIS"];

            if (g_produto.Count <= 0)
                g_produto.Add(new ESTOQUE_LOJA_NF_RECEB_PRODUTO { CODIGO = 0 });
            gvProdutoNovo.DataSource = g_produto;
            gvProdutoNovo.DataBind();
        }
        protected void btIncluirProduto_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            string msg = "";
            string retorno = "";
            if (bt != null)
            {
                try
                {
                    labBaixa.Text = "";

                    int gradeEXP = 0;
                    int gradeXP = 0;
                    int gradePP = 0;
                    int gradeP = 0;
                    int gradeM = 0;
                    int gradeG = 0;
                    int gradeGG = 0;
                    int gradeTotal = 0;

                    GridViewRow row = (GridViewRow)bt.NamingContainer;
                    if (row != null)
                    {
                        string codigoProduto = ObterValorControle(row, "txtProduto", typeof(TextBox));
                        if (codigoProduto.Trim() == "")
                        {
                            msg = "Informe o CÓDIGO do Produto";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }
                        string corProduto = ObterValorControle(row, "ddlCor", typeof(DropDownList));
                        if (corProduto.Trim() == "" || corProduto.Trim() == "0" || corProduto.Trim() == "Selecione")
                        {
                            msg = "Selecione a COR do Produto";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (!ValidarProdutoGridNota(codigoProduto, ((DropDownList)row.FindControl("ddlCor")).SelectedItem.Text.Trim()))
                        {
                            msg = "Este produto já existe na NOTA. <br />Por favor, altere a GRADE.";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 3500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }
                        if (!ValidarProdutoGridNovo(codigoProduto, ((DropDownList)row.FindControl("ddlCor")).SelectedItem.Text.Trim()))
                        {
                            msg = "Este produto já foi adicionado. <br />Por favor, altere a GRADE.";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 3500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        retorno = ObterValorControle(row, "txtGradeEXP", typeof(TextBox));
                        if (retorno.Trim() != "")
                            gradeEXP = Convert.ToInt32(retorno);
                        retorno = ObterValorControle(row, "txtGradeXP", typeof(TextBox));
                        if (retorno.Trim() != "")
                            gradeXP = Convert.ToInt32(retorno);
                        retorno = ObterValorControle(row, "txtGradePP", typeof(TextBox));
                        if (retorno.Trim() != "")
                            gradePP = Convert.ToInt32(retorno);
                        retorno = ObterValorControle(row, "txtGradeP", typeof(TextBox));
                        if (retorno.Trim() != "")
                            gradeP = Convert.ToInt32(retorno);
                        retorno = ObterValorControle(row, "txtGradeM", typeof(TextBox));
                        if (retorno.Trim() != "")
                            gradeM = Convert.ToInt32(retorno);
                        retorno = ObterValorControle(row, "txtGradeG", typeof(TextBox));
                        if (retorno.Trim() != "")
                            gradeG = Convert.ToInt32(retorno);
                        retorno = ObterValorControle(row, "txtGradeGG", typeof(TextBox));
                        if (retorno.Trim() != "")
                            gradeGG = Convert.ToInt32(retorno);

                        //Total inserido
                        gradeTotal = gradeEXP + gradeXP + gradePP + gradeP + gradeM + gradeG + gradeGG;

                        if (gradeTotal <= 0)
                        {
                            msg = "Informe a GRADE do Produto Recebida";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (Session["NF_PRODUTO_MAIS"] != null)
                            g_produto = (List<ESTOQUE_LOJA_NF_RECEB_PRODUTO>)Session["NF_PRODUTO_MAIS"];

                        ESTOQUE_LOJA_NF_RECEB_PRODUTO _novoProduto = new ESTOQUE_LOJA_NF_RECEB_PRODUTO();
                        _novoProduto.CODIGO = -1;
                        _novoProduto.PRODUTO = codigoProduto.Trim();
                        _novoProduto.COR_PRODUTO = corProduto;
                        _novoProduto.QTDE_ENTRADA = gradeTotal;
                        _novoProduto.EXP_RECEBIDO = gradeEXP;
                        _novoProduto.XP_RECEBIDO = gradeXP;
                        _novoProduto.PP_RECEBIDO = gradePP;
                        _novoProduto.P_RECEBIDO = gradeP;
                        _novoProduto.M_RECEBIDO = gradeM;
                        _novoProduto.G_RECEBIDO = gradeG;
                        _novoProduto.GG_RECEBIDO = gradeGG;
                        _novoProduto.STATUS = 'N';

                        g_produto.Add(_novoProduto);
                        Session["NF_PRODUTO_MAIS"] = g_produto;

                        RecarregarProdutoNovo();
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btExcluirProduto_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)bt.NamingContainer;
                    if (row != null)
                    {
                        if (Session["NF_PRODUTO_MAIS"] != null)
                            g_produto = (List<ESTOQUE_LOJA_NF_RECEB_PRODUTO>)Session["NF_PRODUTO_MAIS"];

                        int index = g_produto.FindIndex(p => p.PRODUTO == ObterValorControle(row, "labProduto", typeof(Label)) &&
                                                        p.COR_PRODUTO == ObterValorControle(row, "hidCor", typeof(HiddenField)));
                        if (index >= 0)
                            g_produto.RemoveAt(index);

                        Session["NF_PRODUTO_MAIS"] = g_produto;
                        RecarregarProdutoNovo();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)txt.NamingContainer;
                    if (row != null)
                    {
                        Literal litDescricao = row.FindControl("litDescricao") as Literal;
                        DropDownList ddl = row.FindControl("ddlCor") as DropDownList;

                        string produtoLinx = txt.Text.Trim();
                        CarregarCores(ddl, produtoLinx);
                        PRODUTO _produto = new PRODUTO();
                        _produto = baseController.BuscaProduto(produtoLinx);
                        if (_produto != null)
                        {
                            if (litDescricao != null)
                                litDescricao.Text = _produto.DESC_PRODUTO.Trim();
                        }
                        else
                        {
                            if (litDescricao != null)
                                litDescricao.Text = "";
                            if (ddl != null)
                            {
                                ddl.DataSource = null;
                                ddl.DataBind();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected void gvProdutoNovo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PRODUTO _produto = null;
            Sp_Busca_Produto_CorResult _corProduto = null;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ESTOQUE_LOJA_NF_RECEB_PRODUTO _estRecebidoProduto = e.Row.DataItem as ESTOQUE_LOJA_NF_RECEB_PRODUTO;

                    colunaNovo += 1;
                    if (_estRecebidoProduto != null)
                    {
                        if (_estRecebidoProduto.CODIGO == 0)
                        {
                            e.Row.Visible = false;
                            return;
                        }

                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = colunaNovo.ToString();

                        Literal _litDescricaoItem = e.Row.FindControl("litDescricaoItem") as Literal;
                        if (_litDescricaoItem != null)
                        {
                            _produto = new PRODUTO();
                            _produto = baseController.BuscaProduto(_estRecebidoProduto.PRODUTO.Trim());
                            if (_produto != null)
                                _litDescricaoItem.Text = _produto.DESC_PRODUTO.Trim(); ;
                        }

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                        {
                            _corProduto = new Sp_Busca_Produto_CorResult();
                            _corProduto = baseController.BuscaProdutoCor(_estRecebidoProduto.PRODUTO.Trim(), _estRecebidoProduto.COR_PRODUTO);
                            if (_corProduto != null)
                                _litCor.Text = _corProduto.DESC_COR_PRODUTO.Trim();
                        }

                        HiddenField _hidCor = e.Row.FindControl("hidCor") as HiddenField;
                        if (_hidCor != null)
                            _hidCor.Value = _estRecebidoProduto.COR_PRODUTO;

                        Button _btExcluirProduto = e.Row.FindControl("btExcluirProduto") as Button;
                        if (_btExcluirProduto != null)
                            _btExcluirProduto.CommandArgument = _estRecebidoProduto.CODIGO.ToString();
                    }
                }
            }
        }

        private bool ValidarProdutoGridNota(string produto, string cor)
        {
            bool retorno = true;

            foreach (GridViewRow row in gvProduto.Rows)
                if (row.Cells[1].Text.Trim() == produto && row.Cells[3].Text.Trim() == cor)
                {
                    retorno = false;
                    break;
                }
            return retorno;
        }
        private bool ValidarProdutoGridNovo(string produto, string cor)
        {
            bool retorno = true;
            foreach (GridViewRow row in gvProdutoNovo.Rows)
                if (((Label)row.FindControl("labProduto")).Text.Trim() == produto && ((Literal)row.FindControl("litCor")).Text.Trim() == cor)
                {
                    retorno = false;
                    break;
                }

            return retorno;
        }
        #endregion

        #region "BAIXA"
        protected void btBaixarProduto_Click(object sender, EventArgs e)
        {
            string retorno = "";

            int gradeEXP = 0;
            int gradeXP = 0;
            int gradePP = 0;
            int gradeP = 0;
            int gradeM = 0;
            int gradeG = 0;
            int gradeGG = 0;
            int gradeTotal = 0;

            string status = "";
            string codigoFilial = "";

            Button bt = (Button)sender;
            if (bt != null)
            {
                try
                {
                    //Validar BAIXA 
                    labBaixa.Text = "";
                    if (!ValidarBaixa())
                    {
                        labBaixa.Text = "Clique no botão \"INCLUIR\" ou apague o produto digitado.";
                        return;
                    }

                    codigoFilial = ddlFilial.SelectedValue;

                    //INSERE CABEÇALHO DA NOTA
                    ESTOQUE_LOJA_NF_RECEB _novo = new ESTOQUE_LOJA_NF_RECEB();

                    _novo.FILIAL_ORIGEM = hidEmpresaEmissora.Value;
                    _novo.NUMERO_NF_TRANSFERENCIA = hidNF.Value;
                    _novo.SERIE_NF_ENTRADA = hidSerie.Value;
                    _novo.EMISSAO = Convert.ToDateTime(hidEmissao.Value);
                    _novo.QTDE_TOTAL = Convert.ToInt32(hidQtdeTotal.Value);
                    _novo.ROMANEIO_PRODUTO = hidRomaneio.Value;
                    _novo.FILIAL = ddlFilial.SelectedItem.Text.Trim();
                    _novo.NOME_RECEBIDO = hidNome.Value;
                    _novo.DATA_BAIXA = DateTime.Now;
                    _novo.USUARIO_BAIXA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    int codigoEstoqueRecebido = 0;
                    codigoEstoqueRecebido = estoqueController.InserirEstoqueLojaReceb(_novo);
                    // FIM INSERE CABEÇALHO DA NOTA

                    List<ESTOQUE_LOJA_NF_RECEB_PRODUTO> _listaProdutoDivergente = new List<ESTOQUE_LOJA_NF_RECEB_PRODUTO>();
                    if (codigoEstoqueRecebido > 0)
                    {
                        //INSERE PRODUTOS DA NOTA
                        foreach (GridViewRow row in gvProduto.Rows)
                        {
                            if (row != null)
                            {
                                gradeEXP = 0;
                                gradeXP = 0;
                                gradePP = 0;
                                gradeP = 0;
                                gradeM = 0;
                                gradeG = 0;
                                gradeGG = 0;
                                gradeTotal = 0;

                                retorno = ObterValorControle(row, "txtGradeEXP", typeof(TextBox));
                                if (retorno.Trim() != "")
                                    gradeEXP = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "txtGradeXP", typeof(TextBox));
                                if (retorno.Trim() != "")
                                    gradeXP = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "txtGradePP", typeof(TextBox));
                                if (retorno.Trim() != "")
                                    gradePP = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "txtGradeP", typeof(TextBox));
                                if (retorno.Trim() != "")
                                    gradeP = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "txtGradeM", typeof(TextBox));
                                if (retorno.Trim() != "")
                                    gradeM = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "txtGradeG", typeof(TextBox));
                                if (retorno.Trim() != "")
                                    gradeG = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "txtGradeGG", typeof(TextBox));
                                if (retorno.Trim() != "")
                                    gradeGG = Convert.ToInt32(retorno);

                                //Total inserido
                                gradeTotal = gradeEXP + gradeXP + gradePP + gradeP + gradeM + gradeG + gradeGG;

                                ESTOQUE_LOJA_NF_RECEB_PRODUTO _novoProduto = new ESTOQUE_LOJA_NF_RECEB_PRODUTO();
                                _novoProduto.ESTOQUE_LOJA_NF_RECEB = codigoEstoqueRecebido;
                                _novoProduto.PRODUTO = row.Cells[1].Text.Trim();
                                _novoProduto.COR_PRODUTO = ObterValorControle(row, "hidCor", typeof(HiddenField));
                                _novoProduto.EXP_RECEBIDO = gradeEXP;
                                _novoProduto.XP_RECEBIDO = gradeXP;
                                _novoProduto.PP_RECEBIDO = gradePP;
                                _novoProduto.P_RECEBIDO = gradeP;
                                _novoProduto.M_RECEBIDO = gradeM;
                                _novoProduto.G_RECEBIDO = gradeG;
                                _novoProduto.GG_RECEBIDO = gradeGG;
                                _novoProduto.QTDE_ENTRADA = (row.Cells[5].Text.Trim() == "") ? 0 : Convert.ToInt32(row.Cells[5].Text.Trim());

                                if (ObterValorControle(row, "chkOK", typeof(CheckBox)) == "N")
                                {
                                    _novoProduto.STATUS = 'D';
                                    _novoProduto.OBSERVACAO = "DIVERGENTE";
                                    _listaProdutoDivergente.Add(_novoProduto);
                                    status = "D";
                                }
                                else
                                {
                                    _novoProduto.STATUS = 'B';
                                }

                                estoqueController.InserirEstoqueLojaRecebProduto(_novoProduto);

                            }
                        }
                        //FIM INSERE PRODUTOS DA NOTA

                        //INSERE PRODUTOS NOVOS (NAO ESTAO NA NOTA)
                        foreach (GridViewRow row in gvProdutoNovo.Rows)
                        {
                            if (row != null)
                            {
                                gradeEXP = 0;
                                gradeXP = 0;
                                gradePP = 0;
                                gradeP = 0;
                                gradeM = 0;
                                gradeG = 0;
                                gradeGG = 0;
                                gradeTotal = 0;

                                retorno = ObterValorControle(row, "labGradeEXP", typeof(Label));
                                if (retorno.Trim() != "")
                                    gradeEXP = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "labGradeXP", typeof(Label));
                                if (retorno.Trim() != "")
                                    gradeXP = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "labGradePP", typeof(Label));
                                if (retorno.Trim() != "")
                                    gradePP = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "labGradeP", typeof(Label));
                                if (retorno.Trim() != "")
                                    gradeP = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "labGradeM", typeof(Label));
                                if (retorno.Trim() != "")
                                    gradeM = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "labGradeG", typeof(Label));
                                if (retorno.Trim() != "")
                                    gradeG = Convert.ToInt32(retorno);
                                retorno = ObterValorControle(row, "labGradeGG", typeof(Label));
                                if (retorno.Trim() != "")
                                    gradeGG = Convert.ToInt32(retorno);

                                //Total inserido
                                gradeTotal = gradeEXP + gradeXP + gradePP + gradeP + gradeM + gradeG + gradeGG;

                                ESTOQUE_LOJA_NF_RECEB_PRODUTO _novoProduto = new ESTOQUE_LOJA_NF_RECEB_PRODUTO();

                                _novoProduto.ESTOQUE_LOJA_NF_RECEB = codigoEstoqueRecebido;
                                _novoProduto.PRODUTO = ObterValorControle(row, "labProduto", typeof(Label)).Trim();
                                _novoProduto.COR_PRODUTO = ObterValorControle(row, "hidCor", typeof(HiddenField)).Trim();
                                _novoProduto.QTDE_ENTRADA = 0;// gradeTotal;
                                _novoProduto.EXP_RECEBIDO = gradeEXP;
                                _novoProduto.XP_RECEBIDO = gradeXP;
                                _novoProduto.PP_RECEBIDO = gradePP;
                                _novoProduto.P_RECEBIDO = gradeP;
                                _novoProduto.M_RECEBIDO = gradeM;
                                _novoProduto.G_RECEBIDO = gradeG;
                                _novoProduto.GG_RECEBIDO = gradeGG;
                                if (_novoProduto.PRODUTO.Trim().Length > 4) //Só insere o que tiver mais de 4 caracteres
                                {
                                    _novoProduto.STATUS = 'N'; //Se grade maior que zero, nota está divergente
                                    _novoProduto.OBSERVACAO = "A MAIS";
                                    _listaProdutoDivergente.Add(_novoProduto);
                                    status = "N";
                                    estoqueController.InserirEstoqueLojaRecebProduto(_novoProduto);
                                }
                            }
                        }
                    }

                    if (status == "")
                        status = "B"; //STATUS OK

                    //Enviar E-mail
                    EnviarEmail(status, codigoFilial, _listaProdutoDivergente); //Status para informar se nota está OK ou divergente

                    btBuscar_Click(null, null);
                    Session["NF_PRODUTO_MAIS"] = null;
                }
                catch (Exception ex)
                {
                    labBaixa.Text = ex.Message;
                }
            }
        }
        private bool ValidarBaixa()
        {
            bool retorno = true;

            GridViewRow footer = gvProdutoNovo.FooterRow;
            if (footer != null)
            {
                TextBox fProduto = footer.FindControl("txtProduto") as TextBox;
                if (fProduto != null)
                {
                    if (fProduto.Text.Trim() != "")
                        retorno = false;
                }
            }

            return retorno;
        }
        private void EnviarEmail(string status, string codigoFilial, List<ESTOQUE_LOJA_NF_RECEB_PRODUTO> _listaProdutoDivergente)
        {
            string msg = "";
            email_envio email = new email_envio();
            email.ASSUNTO = "Mercadoria Recebida - " + ddlFilial.SelectedItem.Text.Trim() + " - NF: " + hidNF.Value.Trim();
            email.REMETENTE = (USUARIO)Session["USUARIO"];

            List<string> destinatario = new List<string>();
            List<USUARIO> usuarioDestinatario = new List<USUARIO>();
            if (status.ToUpper() != "B")
            {
                msg = "Mercadoria recebida com DIVERGÊNCIA.";
                usuarioDestinatario = new UsuarioController().ObterEmailUsuarioTela(1, 2).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                foreach (USUARIO usu in usuarioDestinatario)
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);

                //OBTER EMAIL DO SUPERVISOR DA LOJA
                var _supervisorId = baseController.BuscaSupervisorLoja(codigoFilial);
                if (_supervisorId != null)
                {
                    var _supervisorUsuario = baseController.BuscaUsuario(_supervisorId.codigo_usuario);
                    if (_supervisorUsuario != null)
                        destinatario.Add(_supervisorUsuario.EMAIL);
                }

                //OBTER EMAIL DA LOJA
                var _filial = baseController.ObterFilialIntranet(codigoFilial);
                if (_filial != null)
                    destinatario.Add(_filial.email);
            }
            else
            {
                msg = "Mercadoria recebida CORRETAMENTE.";
                usuarioDestinatario = new UsuarioController().ObterEmailUsuarioTela(1, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                foreach (USUARIO usu in usuarioDestinatario)
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);

            }
            email.MENSAGEM = MontarCorpoEmail(msg, hidEmpresaEmissora.Value, hidNF.Value, hidSerie.Value, hidEmissao.Value, ddlFilial.SelectedItem.Text.Trim(), hidNome.Value.ToUpper().Trim(), _listaProdutoDivergente);
            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(string msg, string filial_origem, string nf, string serie, string emissao, string filial, string recebido, List<ESTOQUE_LOJA_NF_RECEB_PRODUTO> _listaProdutoDivergente)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            sb.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>NOTA MERCADORIA</title>");
            sb.Append("    <meta charset='UTF-8'>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <span>" + msg + "</span>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <div id='divProduto' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            sb.Append("                            width: 750pt'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 150px;'>");
            sb.Append("                                    Filial Origem:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                " + filial_origem);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    NF Transferência:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                   " + nf);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Série:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                  " + serie);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Emissão:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                   " + emissao);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Filial:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                   " + filial);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Recebido Por:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + recebido.ToUpper());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                         &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");

            if (_listaProdutoDivergente != null && _listaProdutoDivergente.Count > 0)
            {
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        <table cellpadding='0' cellspacing='0' style='width: 830pt; padding: 0px; color: black;");
                sb.Append("                            font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
                sb.Append("                            border: 1px solid #ccc;'>");
                sb.Append("                            <tr style='background-color: #ccc;'>");
                sb.Append("                                <td>");
                sb.Append("                                    Produto");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    Cor");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    Qtde Nota");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    Status");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                    EXP");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                    XP");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                    PP");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                    P");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                    M");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                    G");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                    GG");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                    Total");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");

                foreach (ESTOQUE_LOJA_NF_RECEB_PRODUTO estLojaRecebimento in _listaProdutoDivergente)
                {
                    int total = estLojaRecebimento.EXP_RECEBIDO +
                                estLojaRecebimento.XP_RECEBIDO +
                                estLojaRecebimento.PP_RECEBIDO +
                                estLojaRecebimento.P_RECEBIDO +
                                estLojaRecebimento.M_RECEBIDO +
                                estLojaRecebimento.G_RECEBIDO +
                                estLojaRecebimento.GG_RECEBIDO;

                    var produto = new BaseController().BuscaProduto(estLojaRecebimento.PRODUTO);
                    string descProduto = "";
                    if (produto != null)
                        descProduto = produto.DESC_PRODUTO.Trim();

                    var cor = new ProducaoController().ObterCoresBasicas(estLojaRecebimento.COR_PRODUTO);
                    string descCor = "";
                    if (cor != null)
                        descCor = cor.DESC_COR.Trim();

                    sb.Append("                            <tr>");
                    sb.Append("                                <td>");
                    sb.Append("                                    " + estLojaRecebimento.PRODUTO + " - " + descProduto);
                    sb.Append("                                </td>");
                    sb.Append("                                <td>");
                    sb.Append("                                    " + descCor);
                    sb.Append("                                </td>");
                    sb.Append("                                <td>");
                    sb.Append("                                    " + estLojaRecebimento.QTDE_ENTRADA);
                    sb.Append("                                </td>");
                    sb.Append("                                <td>");
                    sb.Append("                                    " + estLojaRecebimento.OBSERVACAO);
                    sb.Append("                                </td>");
                    sb.Append("                                <td style='text-align: center; width: 75px;'>");
                    sb.Append("                                    " + estLojaRecebimento.EXP_RECEBIDO.ToString());
                    sb.Append("                                </td>");
                    sb.Append("                                <td style='text-align: center; width: 75px;'>");
                    sb.Append("                                    " + estLojaRecebimento.XP_RECEBIDO.ToString());
                    sb.Append("                                </td>");
                    sb.Append("                                <td style='text-align: center; width: 75px;'>");
                    sb.Append("                                    " + estLojaRecebimento.PP_RECEBIDO.ToString());
                    sb.Append("                                </td>");
                    sb.Append("                                <td style='text-align: center; width: 75px;'>");
                    sb.Append("                                    " + estLojaRecebimento.P_RECEBIDO.ToString());
                    sb.Append("                                </td>");
                    sb.Append("                                <td style='text-align: center; width: 75px;'>");
                    sb.Append("                                    " + estLojaRecebimento.M_RECEBIDO.ToString());
                    sb.Append("                                </td>");
                    sb.Append("                                <td style='text-align: center; width: 75px;'>");
                    sb.Append("                                    " + estLojaRecebimento.G_RECEBIDO.ToString());
                    sb.Append("                                </td>");
                    sb.Append("                                <td style='text-align: center; width: 75px;'>");
                    sb.Append("                                    " + estLojaRecebimento.GG_RECEBIDO.ToString());
                    sb.Append("                                </td>");
                    sb.Append("                                <td style='text-align: center; width: 85px;'>");
                    sb.Append("                                    " + total.ToString());
                    sb.Append("                                </td>");
                    sb.Append("                            </tr>");
                }
                sb.Append("                        </table>");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
            }
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO.ToUpper() + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = baseController.BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();
                //lstFilial = baseController.BuscaFiliais();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                        btBuscar_Click(null, null);
                    }
                }
            }
        }
        private void CarregarCores(DropDownList ddl, string codigoProduto)
        {
            List<PRODUTO_CORE> produtoCor = new List<PRODUTO_CORE>();
            produtoCor = baseController.BuscaProdutoCores(codigoProduto);
            if (produtoCor != null)
            {
                produtoCor.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "0", DESC_COR_PRODUTO = "Selecione" });
                ddl.DataSource = produtoCor;
                ddl.DataBind();

                if (produtoCor.Count == 2)
                    ddl.SelectedValue = produtoCor[1].COR_PRODUTO;
            }
        }
        #endregion
    }
}
