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
    public partial class desenv_produto_manut : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        List<SP_OBTER_GRUPOResult> g_grupo = null;
        List<DESENV_PRODUTO_ORIGEM> g_origem = null;
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Carregar combos
                CarregarColecoes();
                CarregarGrupo();

                //Foco no coleção
                ddlColecoes.Focus();
            }
        }

        #region "MANUTENÇÃO"
        protected void btBuscarProduto_Click(object sender, EventArgs e)
        {
            try
            {
                labSalvar.Text = "";
                labProduto.Text = "";
                if (ddlColecoesBuscar.SelectedValue.Trim() == "" || ddlColecoesBuscar.SelectedValue.Trim() == "0")
                {
                    labProduto.Text = "Selecione a Coleção.";
                    return;
                }
                if (!cbModeloFiltro.Checked)
                {
                    if ((ddlGrupoBuscar.SelectedValue.Trim() == ""
                         || ddlGrupoBuscar.SelectedValue.Trim() == "0"
                         || ddlGrupoBuscar.SelectedValue.Trim() == "Selecione") &&
                         (txtModeloBuscar.Text.Trim() == ""))
                    {
                        labProduto.Text = "Selecione um GRUPO e/ou informe um MODELO.";
                        return;
                    }
                }

                RecarregarProduto();
                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;
            }
            catch (Exception ex)
            {
                labProduto.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;

                    coluna += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        /*
                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_produto.COLECAO)).DESC_COLECAO;
                        */

                        DropDownList _ddlGrupo = e.Row.FindControl("ddlGrupo") as DropDownList;
                        if (_ddlGrupo != null)
                        {
                            if (g_grupo != null)
                            {
                                _ddlGrupo.DataSource = g_grupo;
                                _ddlGrupo.DataBind();

                                if (_produto.GRUPO.Trim() != "")
                                {
                                    string _grupoProduto = (new BaseController().BuscaGrupoProduto(_produto.GRUPO)).GRUPO_PRODUTO;
                                    _ddlGrupo.SelectedValue = _grupoProduto;
                                }
                            }
                        }

                        DropDownList _ddlOrigem = e.Row.FindControl("ddlOrigem") as DropDownList;
                        if (_ddlOrigem != null)
                        {
                            if (_ddlOrigem != null)
                            {
                                _ddlOrigem.DataSource = g_origem.Where(i => i.COLECAO.Trim() == _produto.COLECAO.Trim()).ToList();
                                _ddlOrigem.DataBind();

                                if (_produto.DESENV_PRODUTO_ORIGEM > 0)
                                {
                                    int _produtoOrigem = desenvController.ObterProdutoOrigem(Convert.ToInt32(_produto.DESENV_PRODUTO_ORIGEM)).CODIGO;
                                    _ddlOrigem.SelectedValue = _produtoOrigem.ToString();
                                }
                            }
                        }

                        TextBox _txtModelo = e.Row.FindControl("txtModelo") as TextBox;
                        if (_txtModelo != null)
                            _txtModelo.Text = (_produto.MODELO == null) ? "" : _produto.MODELO.Trim();

                        TextBox _txtCor = e.Row.FindControl("txtCor") as TextBox;
                        if (_txtCor != null)
                            _txtCor.Text = (_produto.COR == null) ? "" : _produto.COR.Trim();

                        TextBox _txtQtdeVarejo = e.Row.FindControl("txtQtdeVarejo") as TextBox;
                        if (_txtQtdeVarejo != null)
                            _txtQtdeVarejo.Text = (_produto.QTDE == null) ? "" : _produto.QTDE.ToString();

                        TextBox _txtPreco = e.Row.FindControl("txtPreco") as TextBox;
                        if (_txtPreco != null)
                            _txtPreco.Text = (_produto.PRECO == null) ? "" : _produto.PRECO.ToString();

                        TextBox _txtQtdeAtacado = e.Row.FindControl("txtQtdeAtacado") as TextBox;
                        if (_txtQtdeAtacado != null)
                            _txtQtdeAtacado.Text = (_produto.QTDE_ATACADO == null) ? "" : _produto.QTDE_ATACADO.ToString();

                        TextBox _txtPrecoAtacado = e.Row.FindControl("txtPrecoAtacado") as TextBox;
                        if (_txtPrecoAtacado != null)
                            _txtPrecoAtacado.Text = (_produto.PRECO_ATACADO == null) ? "" : _produto.PRECO_ATACADO.ToString();

                        Button _btProdutoSalvar = e.Row.FindControl("btProdutoSalvar") as Button;
                        if (_btProdutoSalvar != null)
                            _btProdutoSalvar.CommandArgument = _produto.CODIGO.ToString();

                        Button _btProdutoExcluir = e.Row.FindControl("btProdutoExcluir") as Button;
                        if (_btProdutoExcluir != null)
                            _btProdutoExcluir.CommandArgument = _produto.CODIGO.ToString();
                    }
                }
            }
        }
        protected void btProdutoSalvar_Click(object sender, EventArgs e)
        {
            string msg = "";

            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(Convert.ToInt32(b.CommandArgument));

                    if (_produto.CODIGO > 0)
                    {
                        GridViewRow _row = (GridViewRow)b.NamingContainer;
                        if (_row != null)
                        {
                            DropDownList _grupo = _row.FindControl("ddlGrupo") as DropDownList;
                            DropDownList _origem = _row.FindControl("ddlOrigem") as DropDownList;
                            TextBox _modelo = _row.FindControl("txtModelo") as TextBox;
                            TextBox _cor = _row.FindControl("txtCor") as TextBox;
                            TextBox _qtde = _row.FindControl("txtQtdeVarejo") as TextBox;
                            TextBox _preco = _row.FindControl("txtPreco") as TextBox;
                            TextBox _qtdeAtacado = _row.FindControl("txtQtdeAtacado") as TextBox;
                            TextBox _precoAtacado = _row.FindControl("txtPrecoAtacado") as TextBox;

                            //Validação de Campos


                            //Atualização
                            _produto.CODIGO_REF = Convert.ToInt32(_row.Cells[1].Text);
                            _produto.COLECAO = ddlColecoesBuscar.SelectedValue.Trim();
                            _produto.GRUPO = _grupo.SelectedValue.Trim();
                            _produto.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(_origem.SelectedValue);
                            _produto.MODELO = _modelo.Text.Trim().ToUpper();
                            _produto.COR = _cor.Text.Trim().ToUpper();
                            _produto.QTDE = Convert.ToInt32(_qtde.Text);
                            _produto.PRECO = Convert.ToDecimal(_preco.Text);
                            _produto.QTDE_ATACADO = Convert.ToInt32(_qtdeAtacado.Text);
                            _produto.PRECO_ATACADO = Convert.ToDecimal(_precoAtacado.Text);
                            _produto.DATA_INCLUSAO = DateTime.Now;
                            _produto.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                            _produto.STATUS = 'A';
                            //_produto.DATA_APROVACAO = DateTime.Now;

                            List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();
                            listaProdutos.Add(_produto);
                            desenvController.InserirAtualizarProduto(listaProdutos);

                            msg = _produto.GRUPO + " " + _produto.MODELO + " " + _produto.COR + " alterado com sucesso.";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);

                            RecarregarProduto();
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        protected void btProdutoExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            if (b != null)
            {
                try
                {
                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto.CODIGO = Convert.ToInt32(b.CommandArgument);

                    if (desenvController.ObterProdutoPedidoProduto(_produto.CODIGO) != null)
                    {
                        msg = "Não será possível excluir este Produto. Produto possui Produto relacionado.";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    }
                    else
                    {
                        _produto.STATUS = 'E'; //Excluido
                        desenvController.ExcluirProduto(_produto);

                        msg = "Produto excluído com sucesso.";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Exclusão', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);

                        RecarregarProduto();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        private void RecarregarProduto()
        {
            labProduto.Text = "";

            //Obter lista de produtos
            List<DESENV_PRODUTO> _lstProduto = new List<DESENV_PRODUTO>();
            _lstProduto = desenvController.ObterProduto(ddlColecoesBuscar.SelectedValue).Where(p => p.DATA_APROVACAO != null && p.STATUS == 'A').ToList();

            if (ddlGrupoBuscar.SelectedValue.Trim() != "")
                _lstProduto = _lstProduto.Where(i => i.GRUPO.Trim() == ddlGrupoBuscar.SelectedValue.Trim()).ToList();

            if (txtModeloBuscar.Text.Trim() != "")
                _lstProduto = _lstProduto.Where(i => i.MODELO.Trim().ToUpper().Contains(txtModeloBuscar.Text.Trim().ToUpper())).ToList();

            if (cbModeloFiltro.Checked)
                _lstProduto = _lstProduto.Where(i => i.MODELO == null || i.MODELO.Trim() == "").ToList();

            if (_lstProduto.Count <= 0)
                labProduto.Text = "Nenhum produto encontrado. Refaça sua pesquisa.";

            gvProduto.DataSource = _lstProduto;
            gvProduto.DataBind();
        }
        #endregion

        #region "INCLUSAO"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labColecao.ForeColor = _OK;
            if (ddlColecoes.SelectedValue.Trim() == "" || ddlColecoes.SelectedValue.Trim() == "0")
            {
                labColecao.ForeColor = _notOK;
                retorno = false;
            }

            labOrigem.ForeColor = _OK;
            if (ddlOrigem.SelectedValue.Trim() == "" || ddlOrigem.SelectedValue.Trim() == "0")
            {
                labOrigem.ForeColor = _notOK;
                retorno = false;
            }

            labCodigo.ForeColor = _OK;
            if (txtCodigoRef.Text == "" || Convert.ToInt32(txtCodigoRef.Text) <= 0)
            {
                labCodigo.ForeColor = _notOK;
                retorno = false;
            }

            labGrupo.ForeColor = _OK;
            if (ddlGrupo.SelectedValue.Trim() == "Selecione" || ddlGrupo.SelectedValue.Trim() == "")
            {
                labGrupo.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            //int codigo = 0;
            DESENV_PRODUTO _produto = null;

            try
            {
                labSalvar.Text = "";

                //Validação de nulos
                if (!ValidarCampos())
                {
                    labSalvar.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                /*codigo = desenvController.ObterProdutoCODRef(Convert.ToInt32(txtCodigoRef.Text), ddlColecoes.SelectedValue.Trim());
                if (codigo > 0)
                {
                    labSalvar.Text = "Não será possível inserir este PRODUTO. Este CÓDIGO já existe para esta Coleção.";
                    return;
                }*/

                _produto = new DESENV_PRODUTO();
                _produto.COLECAO = ddlColecoes.Text.Trim();
                _produto.CODIGO_REF = Convert.ToInt32(txtCodigoRef.Text);
                _produto.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(ddlOrigem.SelectedValue.Trim());
                _produto.GRUPO = ddlGrupo.SelectedValue.Trim();
                _produto.MODELO = txtModelo.Text.Trim().ToUpper();
                _produto.COR = txtCor.Text.Trim().ToUpper();

                if (txtQtdeVarejo.Text != "")
                    _produto.QTDE = Convert.ToInt32(txtQtdeVarejo.Text);
                else
                    _produto.QTDE = 0;

                if (txtPreco.Text != "")
                    _produto.PRECO = Convert.ToDecimal(txtPreco.Text);
                else
                    _produto.PRECO = 0;

                if (txtQtdeAtacado.Text != "")
                    _produto.QTDE_ATACADO = Convert.ToInt32(txtQtdeAtacado.Text);
                else
                    _produto.QTDE_ATACADO = 0;

                if (txtPrecoAtacado.Text != "")
                    _produto.PRECO_ATACADO = Convert.ToDecimal(txtPrecoAtacado.Text);
                else
                    _produto.PRECO_ATACADO = 0;

                _produto.PRODUTO_ACABADO = (chkProdutoAcabado.Checked) ? 'S' : 'N';

                _produto.DATA_INCLUSAO = DateTime.Now;
                _produto.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _produto.STATUS = 'A';
                _produto.DATA_APROVACAO = DateTime.Now;
                _produto.CODIGO = 0;

                int codigoProduto = desenvController.InserirProduto(_produto);
                //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA

                if (_produto.PRODUTO_ACABADO == 'N')
                    notificacao_envio.GerarNotificacao(codigoProduto, "DESENV_PRODUTO", 1, "PRODUTO APROVADO");

                //Limpar tela
                txtCodigoRef.Text = "";
                ddlGrupo.SelectedValue = "Selecione";
                txtModelo.Text = "";
                txtCor.Text = "";
                txtQtdeVarejo.Text = "";
                txtPreco.Text = "";
                chkProdutoAcabado.Checked = false;
                txtCodigoRef.Focus();

                labSalvar.Text = "Produto Cadastrado com sucesso.";

                Session["COLECAO"] = ddlColecoes.SelectedValue;

                ddlColecoes_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                labSalvar.Text = ex.Message;
            }
        }
        #endregion

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });

                ddlColecoesBuscar.DataSource = _colecoes;
                ddlColecoesBuscar.DataBind();

                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
                    CarregarOrigem(Session["COLECAO"].ToString().Trim());
                    ddlColecoes_SelectedIndexChanged(null, null);
                }
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = RetornarGrupo();
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "Selecione" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }

            List<SP_OBTER_GRUPOResult> _grupoAux = (prodController.ObterGrupoProduto("01"));
            if (_grupoAux != null)
            {
                _grupoAux.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupoBuscar.DataSource = _grupoAux;
                ddlGrupoBuscar.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = RetornarOrigem();
            _origem = _origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (_origem != null)
            {
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "Selecione" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();

                if (_origem.Count == 2)
                    ddlOrigem.SelectedValue = _origem[1].CODIGO.ToString();

            }
        }

        private List<SP_OBTER_GRUPOResult> RetornarGrupo()
        {
            return (prodController.ObterGrupoProduto("01"));
        }
        private List<DESENV_PRODUTO_ORIGEM> RetornarOrigem()
        {
            return (desenvController.ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList());
        }
        #endregion

        protected void gvProduto_Load(object sender, EventArgs e)
        {
            g_grupo = RetornarGrupo();
            g_origem = RetornarOrigem();
        }
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                CarregarOrigem(colecao);

                var _produto = desenvController.ObterProduto(colecao);
                if (_produto != null)
                {
                    int _codigoMax = 0;
                    if (_produto.Count > 0)
                        _codigoMax = _produto.Max(i => i.CODIGO_REF);

                    txtCodigoRef.Text = (_codigoMax + 1).ToString();
                }

            }
        }
    }
}
