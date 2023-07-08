using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;

namespace Relatorios
{
    public partial class prod_cad_molde : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarMolde();

                //Controle de Controles
                btCancelar.Visible = false;
                pnlErro.Visible = false;

                txtMolde.Text = prodController.ObterMoldeUltimo().ToString();
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarMolde()
        {
            try
            {

                var lstMolde = prodController.ObterMoldeProduto();
                gvMolde.DataSource = lstMolde;
                gvMolde.DataBind();

                CarregarFiltros(lstMolde);

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (MOLDE): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                pnlErro.Visible = true;
                pnlErro.Height = Unit.Pixel(100);
            }
        }
        private void CarregarFiltros(List<SP_OBTER_MOLDE_PRODUTOResult> lstMolde)
        {
            CarregarMoldeFiltro(lstMolde);
            CarregarModelagemFiltro(lstMolde);
            CarregarProdutoFiltro(lstMolde);
        }
        private void CarregarMoldeFiltro(List<SP_OBTER_MOLDE_PRODUTOResult> lstMolde)
        {
            List<PROD_HB_MOLDE> moldeFiltro = new List<PROD_HB_MOLDE>();

            var _molde = lstMolde.Select(s => s.MOLDE.Trim()).Distinct().ToList();
            foreach (var item in _molde)
                moldeFiltro.Add(new PROD_HB_MOLDE { MOLDE = item });

            moldeFiltro = moldeFiltro.OrderBy(p => p.MOLDE).ToList();
            ddlMoldeFiltro.DataSource = moldeFiltro;
            ddlMoldeFiltro.DataBind();
            ddlMoldeFiltro.Items.Insert(0, new ListItem { Value = "", Text = "" });
        }
        private void CarregarModelagemFiltro(List<SP_OBTER_MOLDE_PRODUTOResult> lstMolde)
        {
            List<PROD_HB_MOLDE> modelagemFiltro = new List<PROD_HB_MOLDE>();

            var _modelagem = lstMolde.Select(s => s.MODELAGEM.Trim()).Distinct().ToList();
            foreach (var item in _modelagem)
                modelagemFiltro.Add(new PROD_HB_MOLDE { MODELAGEM = item });

            modelagemFiltro = modelagemFiltro.OrderBy(p => p.MODELAGEM).ToList();
            ddlModelagemFiltro.DataSource = modelagemFiltro;
            ddlModelagemFiltro.DataBind();
            ddlModelagemFiltro.Items.Insert(0, new ListItem { Value = "", Text = "" });
        }
        private void CarregarProdutoFiltro(List<SP_OBTER_MOLDE_PRODUTOResult> lstMolde)
        {
            var produtoFiltro = new List<PROD_HB_MOLDE>();
            var nomeProdutoFiltro = new List<SP_OBTER_MOLDE_PRODUTOResult>();

            var _produto = lstMolde.Where(p => p.PRODUTO != null).Select(s => s.PRODUTO.Trim()).Distinct().ToList();
            foreach (var item in _produto)
                produtoFiltro.Add(new PROD_HB_MOLDE { PRODUTO = item });

            var _produtoDesc = lstMolde.Where(p => p.DESC_PRODUTO != null).Select(s => s.DESC_PRODUTO.Trim()).Distinct().ToList();
            foreach (var item in _produtoDesc)
                nomeProdutoFiltro.Add(new SP_OBTER_MOLDE_PRODUTOResult { DESC_PRODUTO = item });

            produtoFiltro = produtoFiltro.OrderBy(p => p.PRODUTO).ToList();
            nomeProdutoFiltro = nomeProdutoFiltro.OrderBy(p => p.DESC_PRODUTO).ToList();

            ddlProdutoFiltro.DataSource = produtoFiltro;
            ddlProdutoFiltro.DataBind();
            ddlProdutoFiltro.Items.Insert(0, new ListItem { Value = "", Text = "" });

            ddlNomeProdutoFiltro.DataSource = nomeProdutoFiltro;
            ddlNomeProdutoFiltro.DataBind();
            ddlNomeProdutoFiltro.Items.Insert(0, new ListItem { Value = "", Text = "" });
        }

        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labMolde.ForeColor = Color.Black;
            labModelagem.ForeColor = Color.Black;
            labProduto.ForeColor = Color.Black;
            btCancelar.Visible = false;
            txtMolde.Text = "";
            txtModelagem.Text = "";
            txtProduto.Text = "";
            txtNome.Text = "";
            pnlErro.Visible = false;

            CarregarMolde();

            txtMolde.Text = prodController.ObterMoldeUltimo().ToString();
        }

        #endregion

        #region "CRUD"

        private void Incluir(PROD_HB_MOLDE molde)
        {
            prodController.InserirMolde(molde);
        }
        private void Editar(PROD_HB_MOLDE molde)
        {
            prodController.AtualizarMolde(molde);
        }
        private void Excluir(int codigo)
        {
            prodController.ExcluirMolde(codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                pnlErro.Visible = false;
                if (txtMolde.Text.Trim() == "")
                {
                    labErro.Text = "Informe o Molde.";
                    pnlErro.Visible = true;
                    return;
                }
                if (txtModelagem.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Modelagem.";
                    pnlErro.Visible = true;
                    return;
                }
                if (txtProduto.Text.Trim() == "")
                {
                    labErro.Text = "Informe o Produto.";
                    pnlErro.Visible = true;
                    return;
                }
                if (txtNome.Text.Trim() == "")
                {
                    labErro.Text = "Informe o Produto Cadastrado no LINX.";
                    pnlErro.Visible = true;
                    return;
                }

                PROD_HB_MOLDE _novo = new PROD_HB_MOLDE();
                var m = prodController.ObterMolde(txtMolde.Text.Trim().ToUpper());
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                {
                    _novo = prodController.ObterMolde(Convert.ToInt32(hidCodigo.Value));
                    if (m != null)
                    {
                        if (m.CODIGO != _novo.CODIGO)
                        {
                            labErro.Text = "Este número de Molde já existe. Informe outro.";
                            pnlErro.Visible = true;
                            return;
                        }
                    }
                }
                else //INCLUSAO
                {
                    _novo.CODIGO = 0;
                    if (m != null)
                    {
                        labErro.Text = "Este número de Molde já existe. Informe outro.";
                        pnlErro.Visible = true;
                        return;

                    }
                }

                _novo.MOLDE = txtMolde.Text.Trim().ToUpper();
                _novo.MODELAGEM = txtModelagem.Text.Trim().ToUpper();
                _novo.PRODUTO = txtProduto.Text.Trim().ToUpper();
                _novo.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _novo.DATA_INCLUSAO = DateTime.Now;

                if (_novo.CODIGO == 0)
                    Incluir(_novo);
                else
                    Editar(_novo);

                RecarregarTela();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                pnlErro.Visible = true;
                pnlErro.Height = Unit.Pixel(100);
            }
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            pnlErro.Visible = false;

            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    PROD_HB_MOLDE molde = prodController.ObterMolde(Convert.ToInt32(b.CommandArgument));
                    if (molde != null)
                    {
                        hidCodigo.Value = molde.CODIGO.ToString();
                        txtMolde.Text = molde.MOLDE.ToString();
                        txtModelagem.Text = molde.MODELAGEM.Trim().ToUpper();
                        txtProduto.Text = molde.PRODUTO.Trim().ToUpper();
                        txtNome.Text = "";
                        var p = baseController.ObterProdutoLinx(molde.PRODUTO.Trim());
                        if (p != null)
                            txtNome.Text = p.DESC_PRODUTO;

                        labModelagem.ForeColor = Color.Red;
                        labProduto.ForeColor = Color.Red;
                        btCancelar.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btAlterar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    pnlErro.Visible = true;
                    pnlErro.Height = Unit.Pixel(100);
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            pnlErro.Visible = false;
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    Excluir(Convert.ToInt32(b.CommandArgument));
                    RecarregarTela();
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    pnlErro.Visible = true;
                    pnlErro.Height = Unit.Pixel(100);
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion
        protected void gvMolde_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MOLDE_PRODUTOResult molde = e.Row.DataItem as SP_OBTER_MOLDE_PRODUTOResult;

                    coluna += 1;
                    if (molde != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                        {
                            _btAlerar.CommandArgument = molde.CODIGO.ToString();
                        }

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = molde.CODIGO.ToString();
                        }

                    }
                }
            }
        }

        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtNome.Text = "";
                var p = baseController.ObterProdutoLinx(txtProduto.Text.Trim());
                if (p != null)
                    txtNome.Text = p.DESC_PRODUTO;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lstMolde = prodController.ObterMoldeProduto();

            if (ddlMoldeFiltro.SelectedValue != "" && ddlMoldeFiltro.SelectedValue != "0")
                lstMolde = lstMolde.Where(p => p.MOLDE.ToString() == ddlMoldeFiltro.SelectedValue).ToList();

            if (ddlModelagemFiltro.SelectedValue != "" && ddlModelagemFiltro.SelectedValue != "0")
                lstMolde = lstMolde.Where(p => p.MODELAGEM.ToString() == ddlModelagemFiltro.SelectedValue).ToList();

            if (ddlProdutoFiltro.SelectedValue != "" && ddlProdutoFiltro.SelectedValue != "0")
                lstMolde = lstMolde.Where(p => p.PRODUTO == ddlProdutoFiltro.SelectedValue).ToList();

            if (ddlNomeProdutoFiltro.SelectedValue != "" && ddlNomeProdutoFiltro.SelectedValue != "0")
                lstMolde = lstMolde.Where(p => p.DESC_PRODUTO == ddlNomeProdutoFiltro.SelectedValue).ToList();

            gvMolde.DataSource = lstMolde.OrderByDescending(p => p.MOLDE);
            gvMolde.DataBind();
        }
    }
}
