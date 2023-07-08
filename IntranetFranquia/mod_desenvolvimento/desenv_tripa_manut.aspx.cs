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
using Relatorios.mod_desenvolvimento.modelo_pocket;

namespace Relatorios
{
    public partial class desenv_tripa_manut : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        List<SP_OBTER_DESENV_COLECAOResult> g_ProdutoFiltro = new List<SP_OBTER_DESENV_COLECAOResult>();

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

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                //Carregar combos
                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarFornecedores();
                CarregarLinhas();
                CarregarCores();

                //Foco no coleção
                ddlColecoes.Focus();

                //Filtro da TRIPA
                Session["TRIPA"] = null;
                ViewState["sortDirection"] = true;
            }

            //Evitar duplo clique no botão
            btAtualizar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizar, null) + ";");

        }

        #region "MANUTENÇÃO"
        private void RecarregarProduto()
        {
            var _lstProduto = desenvController.ObterDesenvolvimentoColecao(ddlColecoes.SelectedValue);

            _lstProduto = _lstProduto.Where(p => p.MARCA == ddlMarca.SelectedValue).ToList();

            if (ddlOrigemFiltroIni.SelectedValue != "0")
            {
                _lstProduto = _lstProduto.Where(p => p.DESENV_PRODUTO_ORIGEM == Convert.ToInt32(ddlOrigemFiltroIni.SelectedValue)).ToList();
            }

            //Obter tudo
            g_ProdutoFiltro = new List<SP_OBTER_DESENV_COLECAOResult>();
            g_ProdutoFiltro.AddRange(_lstProduto);

            Session["TRIPA"] = _lstProduto;
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DESENV_COLECAOResult _produto = e.Row.DataItem as SP_OBTER_DESENV_COLECAOResult;

                    if (_produto != null)
                    {

                        Label _labBrinde = e.Row.FindControl("labBrinde") as Label;
                        if (_produto.BRINDE == 'S')
                            _labBrinde.Text = "Sim";
                        else if (_produto.BRINDE == 'N')
                            _labBrinde.Text = "Não";
                        else
                            _labBrinde.Text = "";

                        Label _labProdutoAcabado = e.Row.FindControl("labProdutoAcabado") as Label;
                        if (_produto.PRODUTO_ACABADO == 'S')
                            _labProdutoAcabado.Text = "Sim";
                        else if (_produto.PRODUTO_ACABADO == 'N')
                            _labProdutoAcabado.Text = "Não";
                        else
                            _labProdutoAcabado.Text = "";


                        Label _labSigned = e.Row.FindControl("labSigned") as Label;
                        if (_produto.SIGNED == 'S')
                            _labSigned.Text = "Sim";
                        else if (_produto.SIGNED == 'N')
                            _labSigned.Text = "Não";
                        else
                            _labSigned.Text = "";


                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        _litNome.Text = _produto.NOME;

                        ImageButton _btProdutoExcluir = e.Row.FindControl("btProdutoExcluir") as ImageButton;
                        _btProdutoExcluir.Attributes.Add("onclick", "return ConfirmarExclusao();");
                        _btProdutoExcluir.CommandArgument = _produto.CODIGO.ToString();


                        ImageButton _btProdutoCopiar = e.Row.FindControl("btProdutoCopiar") as ImageButton;
                        _btProdutoCopiar.CommandArgument = _produto.CODIGO.ToString();

                        ImageButton _btProdutoImprimir = e.Row.FindControl("btProdutoImprimir") as ImageButton;
                        _btProdutoImprimir.CommandArgument = _produto.CODIGO.ToString();

                        ImageButton _btProdutoVisualizar = e.Row.FindControl("btProdutoVisualizar") as ImageButton;
                        _btProdutoVisualizar.CommandArgument = _produto.CODIGO.ToString();
                        if (_produto.FOTO == null || _produto.FOTO.Trim() == "" || _produto.FOTO.Trim().Length < 5)
                        {
                            //_btProdutoVisualizar.ImageUrl = "~/Image/no_image.png";
                            _btProdutoVisualizar.ToolTip = "Sem Foto";
                        }


                        ImageButton _btProdutoAprovar = e.Row.FindControl("btProdutoAprovar") as ImageButton;
                        ImageButton _btProdutoDesaprovar = e.Row.FindControl("btProdutoDesaprovar") as ImageButton;
                        if (_produto.DATA_APROVACAO != null || _produto.STATUS == 'E')
                            _btProdutoDesaprovar.Visible = true;
                        else
                            _btProdutoAprovar.Visible = true;

                        _btProdutoAprovar.CommandArgument = _produto.CODIGO.ToString();
                        _btProdutoDesaprovar.CommandArgument = _produto.CODIGO.ToString();


                        ImageButton ibtEditar = e.Row.FindControl("btProdutoEditar") as ImageButton;
                        ibtEditar.CommandArgument = _produto.CODIGO.ToString();

                        //if (
                        //    (_produto.QTDE <= 0 && _produto.CORTE_VAREJO == "OK") ||
                        //    (_produto.QTDE > 0 && _produto.CORTE_VAREJO != "OK") ||
                        //    (_produto.QTDE_ATACADO <= 0 && _produto.CORTE_ATACADO == "OK") ||
                        //    (_produto.QTDE_ATACADO > 0 && _produto.CORTE_ATACADO != "OK") ||
                        //    (_produto.QTDE <= 0 && _produto.CORTE_VAREJO != "OK" && _produto.QTDE_ATACADO <= 0 && _produto.CORTE_ATACADO != "OK")
                        //    )
                        //{
                        //    e.Row.Cells[6].ForeColor = Color.DarkGoldenrod;
                        //    e.Row.Cells[6].Font.Bold = true;
                        //}

                    }
                }
            }
        }
        protected void btProdutoExcluir_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string header = "";
            if (b != null)
            {
                try
                {
                    labSalvar.Text = "";

                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(Convert.ToInt32(b.CommandArgument));

                    var producao = desenvController.ObterProdutoProducao(_produto.CODIGO).Where(p => p.PROD_HB != null);
                    if (b.CommandName == "EX" && producao != null && producao.Count() > 0)
                    {
                        msg = "Não será possível excluir este Produto. Produto CORTADO.";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    }
                    else
                    {

                        if (b.CommandName == "EX")
                        {
                            msg = "Produto excluído com sucesso.";
                            header = "Exclusão";

                            _produto.STATUS = 'E'; //Excluido
                            _produto.DATA_APROVACAO = null;
                            _produto.DATA_FICHATECNICA = null;

                            //Obter Producao do Produto
                            var _produtoProducao = desenvController.ObterProdutoProducao(_produto.CODIGO);
                            foreach (DESENV_PRODUTO_PRODUCAO pp in _produtoProducao)
                                if (pp != null)
                                    desenvController.ExcluirProdutoProducao(pp.CODIGO);

                            //InserirQtdeProdutoProducao(_produto, false);
                        }
                        else
                        {
                            _produto.STATUS = 'A'; //ATIVO
                            msg = "Produto adicionado com sucesso.";
                            header = "Inclusão";
                        }

                        //Atualizar produto
                        desenvController.ExcluirProduto(_produto);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '" + header + "', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                        RecarregarProduto();
                        FiltroGeral(null, (bool)ViewState["sortDirection"], false);
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btProdutoCopiar_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            if (b != null)
            {
                try
                {
                    labSalvar.Text = "";
                    //limpar campos antes de copiar
                    btLimpar_Click(null, null);

                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(Convert.ToInt32(b.CommandArgument));
                    if (_produto != null)
                    {
                        //ddlColecoes.SelectedValue = (new BaseController().BuscaColecaoAtual(_produto.COLECAO)).COLECAO;
                        //ddlColecoes_SelectedIndexChanged(null, null);

                        if (_produto.DESENV_PRODUTO_ORIGEM != null && _produto.DESENV_PRODUTO_ORIGEM > 0)
                            ddlOrigem.SelectedValue = _produto.DESENV_PRODUTO_ORIGEM.ToString();

                        if (_produto.GRUPO != null && _produto.GRUPO != "")
                            ddlGrupo.SelectedValue = prodController.ObterGrupoProduto("01").Where(p => p.GRUPO_PRODUTO.Trim() == _produto.GRUPO.Trim()).SingleOrDefault().GRUPO_PRODUTO;

                        txtProduto.Text = _produto.MODELO;

                        if (_produto.GRIFFE != null && _produto.GRIFFE.Trim() != "")
                            ddlGriffe.SelectedValue = baseController.BuscaGriffeProduto(_produto.GRIFFE).GRIFFE;

                        txtSegmento.Text = _produto.SEGMENTO;
                        txtTecido.Text = _produto.TECIDO_POCKET;
                        if (_produto.FORNECEDOR != null)
                        {
                            var fornecedor = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == _produto.FORNECEDOR.Trim() && p.TIPO == 'T').SingleOrDefault();
                            if (fornecedor != null)
                                ddlFornecedor.SelectedValue = fornecedor.FORNECEDOR.Trim();
                        }
                        txtREFModelagem.Text = _produto.REF_MODELAGEM;
                        txtPrecoVendaVarejo.Text = (_produto.PRECO == null) ? "" : _produto.PRECO.ToString();
                        txtPrecoVendaAtacado.Text = (_produto.PRECO_ATACADO == null) ? "" : _produto.PRECO_ATACADO.ToString();

                        var _cor = prodController.ObterCoresBasicas(_produto.COR);
                        if (_cor != null)
                            ddlCor.SelectedValue = _cor.COR.Trim();

                        if (_produto.LINHA != null && _produto.LINHA != "")
                            ddlLinha.SelectedValue = baseController.BuscaLinhas(_produto.LINHA).LINHA;

                        txtCorFornecedor.Text = _produto.FORNECEDOR_COR;
                        txtQtdeMostruario.Text = (_produto.QTDE_MOSTRUARIO == null) ? "" : _produto.QTDE_MOSTRUARIO.ToString();
                        txtQtdeVarejo.Text = (_produto.QTDE == null) ? "" : _produto.QTDE.ToString();
                        txtQtdeAtacado.Text = (_produto.QTDE_ATACADO == null) ? "" : _produto.QTDE_ATACADO.ToString();
                        txtObservacao.Text = (_produto.OBSERVACAO == null) ? "" : _produto.OBSERVACAO.ToString();
                        txtObsImpressao.Text = (_produto.OBS_IMPRESSAO == null) ? "" : _produto.OBS_IMPRESSAO.ToString();

                        ddlBrinde.SelectedValue = (_produto.BRINDE == null) ? "" : _produto.BRINDE.ToString();
                        ddlProdutoAcabado.SelectedValue = (_produto.PRODUTO_ACABADO == null) ? "" : _produto.PRODUTO_ACABADO.ToString();

                        ddlSigned.SelectedValue = (_produto.SIGNED == null) ? "" : _produto.SIGNED.ToString();

                        txtSignedNome.Text = _produto.SIGNED_NOME.Trim();

                    }

                    //RecarregarProduto();
                    //FiltroGeral(null, (bool)ViewState["sortDirection"], false);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btProdutoVisualizar_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labSalvar.Text = "";
                    string codigoProduto = b.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('desenv_produto_foto.aspx?p=" + codigoProduto + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btProdutoEditar_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labSalvar.Text = "";
                    string codigoProduto = b.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('desenv_tripa_editar.aspx?p=" + codigoProduto + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btProdutoAprovar_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string header = "Erro";
            if (b != null)
            {
                try
                {
                    labSalvar.Text = "";

                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(Convert.ToInt32(b.CommandArgument));

                    if (b.CommandName == "AP") //APROVAR
                    {
                        if (_produto.COR == null || _produto.COR.Trim() == "")
                        {
                            msg = "Informe a COR do Produto.";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '" + header + "', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if ((_produto.TECIDO_POCKET == null || _produto.TECIDO_POCKET.Trim() == "") && _produto.PRODUTO_ACABADO == 'N')
                        {
                            msg = "Informe o TECIDO do Produto.";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '" + header + "', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (_produto.FORNECEDOR == null || _produto.FORNECEDOR.Trim() == "")
                        {
                            msg = "Informe o FORNECEDOR do Produto.";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '" + header + "', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        _produto.DATA_APROVACAO = DateTime.Now;
                        header = "Aviso";
                        msg = "Produto Aprovado.";
                    }
                    else
                    {
                        //VALIDAR DESAPROVAÇÃO
                        var _produtoProducao = desenvController.ObterProdutoProducao(_produto.CODIGO);

                        if ((_produtoProducao != null && _produtoProducao.Count() > 0) || _produto.DATA_FICHATECNICA != null)
                        {
                            msg = "Não será possível desaprovar este Produto. Possui Ficha Técnica Aprovada.";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '" + header + "', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        _produto.DATA_APROVACAO = null;
                        header = "Aviso";
                        msg = "Produto Desaprovado.";
                    }

                    //Aprovar produto
                    desenvController.AprovarProduto(_produto);

                    //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA
                    notificacao_envio.GerarNotificacao(_produto.CODIGO, "DESENV_PRODUTO", 1, msg + " - " + DateTime.Now.ToString("dd/MM/yyyy"));

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '" + header + "', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    RecarregarProduto();
                    FiltroGeral(null, (bool)ViewState["sortDirection"], false);
                }

                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btProdutoImprimir_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string header = "Erro";
            List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();
            StreamWriter wr = null;

            if (b != null)
            {
                try
                {
                    labSalvar.Text = "";

                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(Convert.ToInt32(b.CommandArgument));

                    listaProdutos.Add(_produto);
                    if (listaProdutos.Count > 0)
                    {
                        string nomeArquivo = "POCKET_IMP_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                        wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                        wr.Write(Pocket.MontarPocketHBF(listaProdutos, true, false).ToString());
                        wr.Flush();

                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '" + header + "', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
                finally
                {
                    if (wr != null)
                        wr.Close();
                }
            }
        }

        #endregion


        #region "INCLUSAO"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labMarca.ForeColor = _OK;
            if (ddlMarca.SelectedValue.Trim() == "" || ddlMarca.SelectedValue.Trim() == "0")
            {
                labMarca.ForeColor = _notOK;
                retorno = false;
            }

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


            labGrupo.ForeColor = _OK;
            if (ddlGrupo.SelectedValue.Trim() == "Selecione" || ddlGrupo.SelectedValue.Trim() == "")
            {
                labGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labProduto.ForeColor = _OK;
            if (txtProduto.Text.Trim() == "")
            {
                labProduto.ForeColor = _notOK;
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

                if (txtQtdeAtacado.Text.Trim() == "")
                    txtQtdeAtacado.Text = "0";
                if (txtQtdeVarejo.Text.Trim() == "")
                    txtQtdeVarejo.Text = "0";
                //Validação de qtdes
                //if (!ValidarQtde(Convert.ToInt32(txtQtdeVarejo.Text), txtVarejo.Text, labQtdeVarejo, labVarejo) || !ValidarQtde(Convert.ToInt32(txtQtdeAtacado.Text), txtAtacado.Text, labQtdeAtacado, labAtacado))
                //{
                //    labSalvar.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                //    return;
                //}

                _produto = new DESENV_PRODUTO();
                /*                codigo = desenvController.ObterProdutoCODRef(Convert.ToInt32(txtCodigoRef.Text), ddlColecoes.SelectedValue.Trim());
                                if (codigo > 0)
                                {
                                    labSalvar.Text = "Não será possível inserir este PRODUTO. Este CÓDIGO já existe para esta Coleção.";
                                    return;
                                }*/

                _produto.COLECAO = ddlColecoes.Text.Trim();
                _produto.CODIGO_REF = Convert.ToInt32(txtCodigoRef.Text);
                _produto.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(ddlOrigem.SelectedValue.Trim());
                _produto.GRUPO = ddlGrupo.SelectedValue.Trim();
                _produto.MODELO = txtProduto.Text.Trim().ToUpper();
                _produto.GRIFFE = ddlGriffe.SelectedValue.Trim();

                _produto.SEGMENTO = txtSegmento.Text.Trim().ToUpper();
                _produto.TECIDO_POCKET = txtTecido.Text.Trim().ToUpper();
                _produto.FORNECEDOR = ddlFornecedor.SelectedValue.ToUpper();

                _produto.CORTE_PETIT = "";

                _produto.REF_MODELAGEM = txtREFModelagem.Text.Trim().ToUpper();

                if (txtPrecoVendaVarejo.Text != "")
                    _produto.PRECO = Convert.ToDecimal(txtPrecoVendaVarejo.Text);
                else
                    _produto.PRECO = 0;
                if (txtPrecoVendaAtacado.Text != "")
                    _produto.PRECO_ATACADO = Convert.ToDecimal(txtPrecoVendaAtacado.Text);
                else
                    _produto.PRECO_ATACADO = 0;

                _produto.COR = ddlCor.SelectedValue.Trim().ToUpper();
                _produto.FORNECEDOR_COR = txtCorFornecedor.Text.Trim().ToUpper();

                _produto.CORTE_VAREJO = "";
                if (txtQtdeVarejo.Text != "")
                {
                    _produto.QTDE = Convert.ToInt32(txtQtdeVarejo.Text);
                    if (txtQtdeVarejo.Text != "0")
                        _produto.CORTE_VAREJO = "OK";
                }
                else
                    _produto.QTDE = 0;

                _produto.CORTE_ATACADO = "";
                if (txtQtdeAtacado.Text != "")
                {
                    _produto.QTDE_ATACADO = Convert.ToInt32(txtQtdeAtacado.Text);
                    if (txtQtdeAtacado.Text != "0")
                        _produto.CORTE_ATACADO = "OK";
                }
                else
                    _produto.QTDE_ATACADO = 0;

                if (txtQtdeMostruario.Text != "")
                    _produto.QTDE_MOSTRUARIO = Convert.ToInt32(txtQtdeMostruario.Text);
                else
                    _produto.QTDE_MOSTRUARIO = 0;

                _produto.LINHA = ddlLinha.SelectedValue;

                _produto.OBSERVACAO = txtObservacao.Text.Trim().ToUpper();
                _produto.OBS_IMPRESSAO = txtObsImpressao.Text;

                if (ddlBrinde.SelectedValue != "")
                    _produto.BRINDE = Convert.ToChar(ddlBrinde.SelectedValue);

                if (ddlProdutoAcabado.SelectedValue != "")
                    _produto.PRODUTO_ACABADO = Convert.ToChar(ddlProdutoAcabado.SelectedValue);

                _produto.PLAN_VAREJO = 'N';
                _produto.PLAN_ATACADO = 'N';

                _produto.DATA_INCLUSAO = DateTime.Now;
                _produto.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _produto.STATUS = 'A';

                if (ddlSigned.SelectedValue != "")
                    _produto.SIGNED = Convert.ToChar(ddlSigned.SelectedValue);

                _produto.SIGNED_NOME = txtSignedNome.Text.Trim().ToUpper();

                List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();
                listaProdutos.Add(_produto);
                desenvController.InserirAtualizarProduto(listaProdutos);

                //Limpar tela
                //ddlColecoes_SelectedIndexChanged(null, null);

                ddlCor.SelectedValue = "";
                txtCorFornecedor.Text = "";
                txtQtdeMostruario.Text = "";
                txtQtdeVarejo.Text = "";
                txtQtdeAtacado.Text = "";

                labSalvar.Text = "Produto Cadastrado com sucesso.";

                ObterCodigoREF(_produto.COLECAO);

                //labAcao.ForeColor = Color.Gray;
                labQtdeAtacado.ForeColor = System.Drawing.Color.Gray;
                labQtdeVarejo.ForeColor = System.Drawing.Color.Gray;

                Session["COLECAO"] = ddlColecoes.SelectedValue;

                RecarregarProduto();
                FiltroGeral(null, true, false);
            }
            catch (Exception ex)
            {
                labSalvar.Text = ex.Message;
            }
        }
        protected void btLimpar_Click(object sender, EventArgs e)
        {
            //ddlColecoes.SelectedValue = "0";
            //ddlOrigem.SelectedValue = "0";
            //txtCodigoRef.Text = "";
            ddlGrupo.SelectedValue = "";
            txtProduto.Text = "";
            ddlGriffe.SelectedValue = "";
            txtSegmento.Text = "";
            txtTecido.Text = "";
            ddlFornecedor.SelectedValue = "";
            txtREFModelagem.Text = "";
            txtPrecoVendaVarejo.Text = "";
            txtPrecoVendaAtacado.Text = "";
            ddlBrinde.SelectedValue = "N";
            ddlProdutoAcabado.SelectedValue = "N";
            txtObservacao.Text = "";
            ddlLinha.SelectedValue = "";
            ddlSigned.SelectedValue = "N";
            txtSignedNome.Text = "";

            ddlCor.SelectedValue = "";
            txtCorFornecedor.Text = "";
            txtQtdeMostruario.Text = "";
            txtQtdeVarejo.Text = "";
            txtQtdeAtacado.Text = "";
            txtObsImpressao.Text = "";
            labSalvar.Text = "";

            labQtdeAtacado.ForeColor = System.Drawing.Color.Gray;
            labQtdeVarejo.ForeColor = System.Drawing.Color.Gray;

        }

        protected void txtPreco_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtPrecoVarejo = (TextBox)sender;
                if (txtPrecoVarejo != null)
                {
                    decimal precoAtacado = 0;
                    if (txtPrecoVarejo.Text.Trim() != "" && Convert.ToDecimal(txtPrecoVarejo.Text.Trim()) > 0) //PRECO VAREJO MAIOR QUE ZERO, GERA PRECO ATACADO AUTOMATICAMENTE
                    {
                        precoAtacado = Math.Round((Convert.ToDecimal(txtPrecoVarejo.Text.Trim()) / 2), 0); //PRECO VAREJO INFORMADO DIVIDIDO por 2
                        txtPrecoVendaAtacado.Text = precoAtacado.ToString();
                    }
                }

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
            List<COLECOE> _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });

                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                {
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
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = RetornarOrigem();
            _origem = _origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (_origem != null)
            {
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();

                ddlOrigemFiltroIni.DataSource = _origem;
                ddlOrigemFiltroIni.DataBind();


                if (_origem.Count == 2)
                {
                    ddlOrigem.SelectedValue = _origem[1].CODIGO.ToString();
                    ddlOrigemFiltroIni.SelectedValue = _origem[1].CODIGO.ToString();
                }

            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = RetornarGriffe();

            if (griffe != null)
            {
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        private void CarregarCores()
        {
            List<CORES_BASICA> _cores = RetornarCorLinx();
            if (_cores != null)
            {
                _cores.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

                ddlCor.DataSource = _cores;
                ddlCor.DataBind();
            }
        }
        private void CarregarFornecedores()
        {
            List<PROD_FORNECEDOR> _fornecedores = RetornarFornecedor();

            if (_fornecedores != null)
            {
                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }
        }
        private void CarregarLinhas()
        {
            var linhas = baseController.BuscaLinhas();

            linhas.Insert(0, new PRODUTOS_LINHA { LINHA = "" });
            ddlLinha.DataSource = linhas;
            ddlLinha.DataBind();
        }

        private List<SP_OBTER_GRUPOResult> RetornarGrupo()
        {
            return (prodController.ObterGrupoProduto("01"));
        }
        private List<DESENV_PRODUTO_ORIGEM> RetornarOrigem()
        {
            return (desenvController.ObterProdutoOrigem().Where(i => i.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim() && i.STATUS == 'A').ToList());
        }
        private List<CORES_BASICA> RetornarCorLinx()
        {
            List<CORES_BASICA> _coresBasicas = new List<CORES_BASICA>();
            _coresBasicas = prodController.ObterCoresBasicas().ToList();
            _coresBasicas = _coresBasicas.GroupBy(p => new { COR = p.COR.Trim(), DESC_COR = p.DESC_COR.Trim() }).Select(x => new CORES_BASICA { COR = x.Key.COR.Trim(), DESC_COR = x.Key.DESC_COR.Trim() }).ToList();
            return (_coresBasicas);
        }
        private List<PROD_FORNECEDOR> RetornarFornecedor()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor().ToList();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });
            return _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'T') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();
        }
        private List<PRODUTOS_GRIFFE> RetornarGriffe()
        {
            List<PRODUTOS_GRIFFE> _griffes = baseController.BuscaGriffes();
            _griffes.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
            return _griffes;
        }
        #endregion

        #region "FILTROS"
        protected void chkFoto_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RecarregarProduto();
                if (chkFoto.Checked)
                {
                    DropDownList ddl = new DropDownList();
                    ddl.ID = "FOTO";
                    FiltroGeral(ddl, true, false);
                }
                else
                {
                    FiltroGeral(null, true, false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void chkAprovar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RecarregarProduto();
                if (chkAprovar.Checked)
                {
                    DropDownList ddl = new DropDownList();
                    ddl.ID = "APROVAR";
                    FiltroGeral(ddl, true, false);
                }
                else
                {
                    FiltroGeral(null, true, false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl != null)
            {
                RecarregarProduto();
                FiltroGeral(ddl, (bool)ViewState["sortDirection"], true);
            }
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            bool order;
            int coluna = 0;

            if ((bool)ViewState["sortDirection"])
                ViewState["sortDirection"] = false;
            else
                ViewState["sortDirection"] = true;

            order = (bool)ViewState["sortDirection"];

            GridView gv = (GridView)sender;
            if (gv != null)
            {
                coluna = 7;
                gv.Columns[coluna].HeaderText = "Produto";
                gv.Columns[coluna + 1].HeaderText = "Grupo";
                gv.Columns[coluna + 2].HeaderText = "Nome";
                coluna = 9;
                gv.Columns[coluna + 1].HeaderText = "Origem";
                gv.Columns[coluna + 2].HeaderText = "Griffe";
                gv.Columns[coluna + 3].HeaderText = "Segmento";
                gv.Columns[coluna + 4].HeaderText = "Tecido";
                gv.Columns[coluna + 5].HeaderText = "Cor";
                gv.Columns[coluna + 6].HeaderText = "Cor Fornecedor";
                gv.Columns[coluna + 7].HeaderText = "Linha";
                gv.Columns[coluna + 8].HeaderText = "Quantidade Mostruário";
                gv.Columns[coluna + 13].HeaderText = "Fornecedor"; //POS 20
                gv.Columns[coluna + 14].HeaderText = "Mostruário Corte"; //POS 21
                gv.Columns[coluna + 15].HeaderText = "REF de Modelagem";
                gv.Columns[coluna + 17].HeaderText = "Varejo";
                gv.Columns[coluna + 18].HeaderText = "Atacado";

                gv.Columns[coluna + 22].HeaderText = "Signed Nome";

                coluna = 7;
                if (e.SortExpression == "ddlProdutoFiltro") gv.Columns[coluna].HeaderText = gv.Columns[coluna].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlGrupoFiltro") gv.Columns[coluna + 1].HeaderText = gv.Columns[coluna + 1].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlNomeFiltro") gv.Columns[coluna + 2].HeaderText = gv.Columns[coluna + 2].HeaderText + " " + ((order) ? " - >>" : " - <<");
                coluna = 9;
                if (e.SortExpression == "ddlOrigemFiltro") gv.Columns[coluna + 1].HeaderText = gv.Columns[coluna + 1].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlGriffeFiltro") gv.Columns[coluna + 2].HeaderText = gv.Columns[coluna + 2].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlSegmentoFiltro") gv.Columns[coluna + 3].HeaderText = gv.Columns[coluna + 3].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlTecidoFiltro") gv.Columns[coluna + 4].HeaderText = gv.Columns[coluna + 4].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlCorFiltro") gv.Columns[coluna + 5].HeaderText = gv.Columns[coluna + 5].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlCorFornecedorFiltro") gv.Columns[coluna + 6].HeaderText = gv.Columns[coluna + 6].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlFornecedorFiltro") gv.Columns[coluna + 13].HeaderText = gv.Columns[coluna + 13].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlMostruarioCorteFiltro") gv.Columns[coluna + 14].HeaderText = gv.Columns[coluna + 14].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlRefModelagemFiltro") gv.Columns[coluna + 15].HeaderText = gv.Columns[coluna + 15].HeaderText + " " + ((order) ? " - >>" : " - <<");

                if (e.SortExpression == "ddlVarejoFiltro") gv.Columns[coluna + 17].HeaderText = gv.Columns[coluna + 17].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlAtacadoFiltro") gv.Columns[coluna + 18].HeaderText = gv.Columns[coluna + 18].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlSignedNomeFiltro") gv.Columns[coluna + 22].HeaderText = gv.Columns[coluna + 22].HeaderText + " " + ((order) ? " - >>" : " - <<");

            }

            DropDownList ddl = new DropDownList();
            ddl.ID = e.SortExpression;

            FiltroGeral(ddl, order, false);
        }
        private void FiltroGeral(DropDownList ddlFiltro, bool pAsc, bool pHeader)
        {
            int coluna = 0;
            const string _VAZIO = "VAZIO";

            if (Session["TRIPA"] != null)
            {
                if (g_ProdutoFiltro != null && g_ProdutoFiltro.Count <= 0)
                    g_ProdutoFiltro.AddRange((List<SP_OBTER_DESENV_COLECAOResult>)Session["TRIPA"]);

                //FILTROS DOS  COMBOS
                if (ddlProdutoFiltro.SelectedValue.Trim() != "0" && ddlProdutoFiltro.SelectedValue.Trim() != "")
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.MODELO.Trim().ToUpper() == ddlProdutoFiltro.SelectedValue.Trim().ToUpper()).ToList();
                if (ddlGriffeFiltro.SelectedValue.Trim() != "0" && ddlGriffeFiltro.SelectedValue.Trim() != "")
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => ((p.GRIFFE == null) ? "" : p.GRIFFE.ToUpper().Trim()) == ddlGriffeFiltro.SelectedValue.ToUpper().Trim()).ToList();

                //FILTRO NOME
                if (ddlNomeFiltro.SelectedValue.Trim() != "0" && ddlNomeFiltro.SelectedValue.Trim() != "" && ddlNomeFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.NOME.Trim().ToUpper() == ddlNomeFiltro.SelectedValue.Trim().ToUpper()).ToList();
                else if (ddlNomeFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.NOME == null || p.NOME.Trim() == "").ToList();

                //FILTRO ORIGEM
                if (ddlOrigemFiltro.SelectedValue.Trim() != "0" && ddlOrigemFiltro.SelectedValue.Trim() != "")
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.DESENV_PRODUTO_ORIGEM.ToString() == ddlOrigemFiltro.SelectedValue).ToList();

                //FILTRO GRUPO
                if (ddlGrupoFiltro.SelectedValue.Trim() != "0" && ddlGrupoFiltro.SelectedValue.Trim() != "" && ddlGrupoFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => ((p.GRUPO == null) ? "" : p.GRUPO.Trim()) == ddlGrupoFiltro.SelectedValue.Trim()).ToList();
                else if (ddlGrupoFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.GRUPO == null || p.GRUPO.Trim() == "").ToList();

                //FILTRO SEGMENTO
                if (ddlSegmentoFiltro.SelectedValue.Trim() != "0" && ddlSegmentoFiltro.SelectedValue.Trim() != "" && ddlSegmentoFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => ((p.SEGMENTO == null) ? "" : p.SEGMENTO.ToUpper().Trim()) == ddlSegmentoFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlSegmentoFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.SEGMENTO == null || p.SEGMENTO.Trim() == "").ToList();

                //FILTRO TECIDO
                if (ddlTecidoFiltro.SelectedValue.Trim() != "0" && ddlTecidoFiltro.SelectedValue.Trim() != "" && ddlTecidoFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => ((p.TECIDO_POCKET == null) ? "" : p.TECIDO_POCKET.ToUpper().Trim()) == ddlTecidoFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlTecidoFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.TECIDO_POCKET == null || p.TECIDO_POCKET.Trim() == "").ToList();

                //FILTRO COR
                if (ddlCorFiltro.SelectedValue.Trim() != "0" && ddlCorFiltro.SelectedValue.Trim() != "" && ddlCorFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.COR.ToUpper().Trim() == ddlCorFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlCorFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.COR == null || p.COR.Trim() == "").ToList();

                //FILTRO FORNECEDOR
                if (ddlFornecedorFiltro.SelectedValue.Trim() != "0" && ddlFornecedorFiltro.SelectedValue.Trim() != "" && ddlFornecedorFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => ((p.FORNECEDOR == null) ? "" : p.FORNECEDOR.ToUpper().Trim()) == ddlFornecedorFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlFornecedorFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.FORNECEDOR == null || p.FORNECEDOR.Trim() == "").ToList();


                //FILTRO CORTE VAREJO
                if (ddlVarejoFiltro.SelectedValue.Trim() != "0" && ddlVarejoFiltro.SelectedValue.Trim() != "" && ddlVarejoFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => ((p.CORTE_VAREJO == null) ? "" : p.CORTE_VAREJO.ToUpper().Trim()) == ddlVarejoFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlVarejoFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.CORTE_VAREJO == null || p.CORTE_VAREJO.Trim() == "").ToList();

                //FILTRO CORTE ATACADO
                if (ddlAtacadoFiltro.SelectedValue.Trim() != "0" && ddlAtacadoFiltro.SelectedValue.Trim() != "" && ddlAtacadoFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => ((p.CORTE_ATACADO == null) ? "" : p.CORTE_ATACADO.ToUpper().Trim()) == ddlAtacadoFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlAtacadoFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.CORTE_ATACADO == null || p.CORTE_ATACADO.Trim() == "").ToList();

                //FILTRO COR FORNECEDOR
                if (ddlCorFornecedorFiltro.SelectedValue.Trim() != "0" && ddlCorFornecedorFiltro.SelectedValue.Trim() != "" && ddlCorFornecedorFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => ((p.FORNECEDOR_COR == null) ? "" : p.FORNECEDOR_COR.ToUpper().Trim()) == ddlCorFornecedorFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlCorFornecedorFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.FORNECEDOR_COR == null || p.FORNECEDOR_COR.Trim() == "").ToList();

                //FILTRO REFER MODELAGEM
                if (ddlRefModelagemFiltro.SelectedValue.Trim() != "0" && ddlRefModelagemFiltro.SelectedValue.Trim() != "" && ddlRefModelagemFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => ((p.REF_MODELAGEM == null) ? "" : p.REF_MODELAGEM.ToUpper().Trim()) == ddlRefModelagemFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlRefModelagemFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.REF_MODELAGEM == null || p.REF_MODELAGEM.Trim() == "").ToList();

                //FILTRO MOSTRUARIO CORTE
                if (ddlMostruarioCorteFiltro.SelectedValue.Trim() != "0" && ddlMostruarioCorteFiltro.SelectedValue.Trim() != "" && ddlMostruarioCorteFiltro.SelectedValue.Trim() != _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => ((p.MOSTRUARIOCORTE == null) ? "" : p.MOSTRUARIOCORTE.ToUpper().Trim()) == ddlMostruarioCorteFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlMostruarioCorteFiltro.SelectedValue.Trim() == _VAZIO)
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.MOSTRUARIOCORTE.Trim() == "").ToList();

                //FILTRO BRINDE
                if (ddlBrindeFiltro.SelectedValue.Trim() != "0" && ddlBrindeFiltro.SelectedValue.Trim() != "")
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.BRINDE.ToString() == ddlBrindeFiltro.SelectedValue).ToList();

                //FILTRO PRODUTO ACABADO
                if (ddlProdutoAcabadoFiltro.SelectedValue.Trim() != "0" && ddlProdutoAcabadoFiltro.SelectedValue.Trim() != "")
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.PRODUTO_ACABADO.ToString() == ddlProdutoAcabadoFiltro.SelectedValue).ToList();

                //FILTRO SIGNED
                if (ddlSignedFiltro.SelectedValue.Trim() != "0" && ddlSignedFiltro.SelectedValue.Trim() != "")
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.SIGNED.ToString() == ddlSignedFiltro.SelectedValue).ToList();

                //FILTRO SIGNED NOME
                if (ddlSignedNomeFiltro.SelectedValue.Trim() != "0" && ddlSignedNomeFiltro.SelectedValue.Trim() != "")
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.SIGNED_NOME != null && p.SIGNED_NOME.ToString() == ddlSignedNomeFiltro.SelectedValue).ToList();

                //Filtra os que nao tem foto
                if (ddlFiltro != null && ddlFiltro.ID == "FOTO")
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.FOTO == null || p.FOTO.Trim() == "" || p.FOTO.Length <= 5).ToList();

                //Filtra os que nao tem APROVACAO
                if (ddlFiltro != null && ddlFiltro.ID == "APROVAR")
                    g_ProdutoFiltro = g_ProdutoFiltro.Where(p => p.DATA_APROVACAO == null).ToList();

                //ORDENAÇÃO
                if (ddlFiltro != null)
                {
                    if (pAsc)
                    {
                        if (ddlFiltro.ID == "ddlProdutoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.MODELO).ToList();
                        if (ddlFiltro.ID == "ddlGriffeFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.GRIFFE).ToList();
                        if (ddlFiltro.ID == "ddlNomeFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.NOME).ToList();
                        if (ddlFiltro.ID == "ddlOrigemFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => ((p.DESENV_PRODUTO_ORIGEM == null) ? p.MODELO : p.DESCRICAO_ORIGEM)).ToList();
                        if (ddlFiltro.ID == "ddlGrupoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.GRUPO).ToList();
                        if (ddlFiltro.ID == "ddlSegmentoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.SEGMENTO).ToList();
                        if (ddlFiltro.ID == "ddlTecidoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.TECIDO_POCKET).ToList();
                        if (ddlFiltro.ID == "ddlCorFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.COR).ToList();
                        if (ddlFiltro.ID == "ddlFornecedorFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.FORNECEDOR).ToList();
                        if (ddlFiltro.ID == "ddlPetitFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.CORTE_PETIT).ToList();
                        if (ddlFiltro.ID == "ddlVarejoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.CORTE_VAREJO).ToList();
                        if (ddlFiltro.ID == "ddlAtacadoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.CORTE_ATACADO).ToList();
                        if (ddlFiltro.ID == "ddlCorFornecedorFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.FORNECEDOR_COR).ToList();
                        if (ddlFiltro.ID == "ddlRefModelagemFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.REF_MODELAGEM).ToList();
                        if (ddlFiltro.ID == "ddlMostruarioCorteFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.MOSTRUARIOCORTE).ToList();
                        if (ddlFiltro.ID == "ddlProdutoAcabadoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.MODELO).ToList();
                        if (ddlFiltro.ID == "ddlSignedFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.SIGNED).ToList();
                        if (ddlFiltro.ID == "ddlSignedNomeFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.SIGNED_NOME).ToList();
                        if (ddlFiltro.ID == "APROVAR" || ddlFiltro.ID == "FOTO") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.MODELO).ToList();
                    }
                    else
                    {
                        if (ddlFiltro.ID == "ddlProdutoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.MODELO).ToList();
                        if (ddlFiltro.ID == "ddlGriffeFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.GRIFFE).ToList();
                        if (ddlFiltro.ID == "ddlNomeFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.NOME).ToList();
                        if (ddlFiltro.ID == "ddlOrigemFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => ((p.DESENV_PRODUTO_ORIGEM == null) ? p.MODELO : p.DESCRICAO_ORIGEM)).ToList();
                        if (ddlFiltro.ID == "ddlGrupoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.GRUPO).ToList();
                        if (ddlFiltro.ID == "ddlSegmentoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.SEGMENTO).ToList();
                        if (ddlFiltro.ID == "ddlTecidoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.TECIDO_POCKET).ToList();
                        if (ddlFiltro.ID == "ddlCorFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.COR).ToList();
                        if (ddlFiltro.ID == "ddlFornecedorFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.FORNECEDOR).ToList();
                        if (ddlFiltro.ID == "ddlPetitFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.CORTE_PETIT).ToList();
                        if (ddlFiltro.ID == "ddlVarejoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.CORTE_VAREJO).ToList();
                        if (ddlFiltro.ID == "ddlAtacadoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.CORTE_ATACADO).ToList();
                        if (ddlFiltro.ID == "ddlCorFornecedorFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.FORNECEDOR_COR).ToList();
                        if (ddlFiltro.ID == "ddlRefModelagemFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.REF_MODELAGEM).ToList();
                        if (ddlFiltro.ID == "ddlMostruarioCorteFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.MOSTRUARIOCORTE).ToList();
                        if (ddlFiltro.ID == "ddlProdutoAcabadoFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.MODELO).ToList();
                        if (ddlFiltro.ID == "ddlSignedFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.SIGNED).ToList();
                        if (ddlFiltro.ID == "ddlSignedNomeFiltro") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.SIGNED_NOME).ToList();
                        if (ddlFiltro.ID == "APROVAR" || ddlFiltro.ID == "FOTO") g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.MODELO).ToList();
                    }
                }
                else
                {
                    g_ProdutoFiltro = g_ProdutoFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.MODELO).ToList();
                }

                if (pHeader)
                {
                    coluna = 7;
                    gvProduto.Columns[coluna].HeaderText = "Produto";
                    gvProduto.Columns[coluna + 1].HeaderText = "Grupo";
                    gvProduto.Columns[coluna + 2].HeaderText = "Nome";
                    coluna = 9;
                    gvProduto.Columns[coluna + 1].HeaderText = "Origem";
                    gvProduto.Columns[coluna + 2].HeaderText = "Griffe";
                    gvProduto.Columns[coluna + 3].HeaderText = "Segmento";
                    gvProduto.Columns[coluna + 4].HeaderText = "Tecido";
                    gvProduto.Columns[coluna + 5].HeaderText = "Cor";
                    gvProduto.Columns[coluna + 6].HeaderText = "Cor Fornecedor";
                    gvProduto.Columns[coluna + 8].HeaderText = "Quantidade Mostruário"; //POS 16
                    gvProduto.Columns[coluna + 9].HeaderText = "Quantidade Varejo";
                    gvProduto.Columns[coluna + 10].HeaderText = "Preço Varejo";
                    gvProduto.Columns[coluna + 11].HeaderText = "Quantidade Atacado";
                    gvProduto.Columns[coluna + 12].HeaderText = "Preço Atacado";

                    gvProduto.Columns[coluna + 13].HeaderText = "Fornecedor";
                    gvProduto.Columns[coluna + 14].HeaderText = "Mostruário Corte";
                    gvProduto.Columns[coluna + 15].HeaderText = "REF de Modelagem";

                    gvProduto.Columns[coluna + 17].HeaderText = "Varejo";
                    gvProduto.Columns[coluna + 18].HeaderText = "Atacado";

                    gvProduto.Columns[coluna + 22].HeaderText = "Signed Nome";

                }

                gvProduto.DataSource = g_ProdutoFiltro;
                gvProduto.DataBind();

                PreencherValorFiltro(g_ProdutoFiltro);
            }
            else
            {
                RecarregarProduto();
            }
        }
        private void PreencherValorFiltro(List<SP_OBTER_DESENV_COLECAOResult> lstProdutos)
        {
            try
            {
                int totalModelo = 0;
                int totalEstilo = 0;

                var estilo = lstProdutos.Where(p => p.MODELO != null && p.MODELO.Trim() != "" && p.MODELO.Trim().Length >= 4 && p.STATUS == 'A');
                if (estilo != null)
                    totalEstilo = estilo.Count();

                var modelo = estilo.Select(j => j.MODELO).Distinct();
                if (modelo != null)
                    totalModelo = modelo.Count();

                labModeloFiltroValor.Text = totalModelo.ToString();
                labEstiloFiltroValor.Text = totalEstilo.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CarregarModeloFiltro()
        {
            List<DESENV_PRODUTO> modeloFiltro = new List<DESENV_PRODUTO>();

            var _modelo = g_ProdutoFiltro.Select(s => s.MODELO.Trim()).Distinct().ToList();
            foreach (var item in _modelo)
                if (item.Trim() != "")
                    modeloFiltro.Add(new DESENV_PRODUTO { MODELO = item.Trim() });

            modeloFiltro = modeloFiltro.OrderBy(p => p.MODELO).ToList();
            modeloFiltro.Insert(0, new DESENV_PRODUTO { MODELO = "" });
            ddlProdutoFiltro.DataSource = modeloFiltro;
            ddlProdutoFiltro.DataBind();
        }
        private void CarregarGriffeFiltro()
        {
            List<DESENV_PRODUTO> griffeFiltro = new List<DESENV_PRODUTO>();

            var _griffe = g_ProdutoFiltro.Where(p => p.GRIFFE != null).Select(s => s.GRIFFE.Trim()).Distinct().ToList();
            foreach (var item in _griffe)
                if (item.Trim() != "")
                    griffeFiltro.Add(new DESENV_PRODUTO { GRIFFE = item.Trim() });

            griffeFiltro = griffeFiltro.OrderBy(p => p.MODELO).ToList();
            griffeFiltro.Insert(0, new DESENV_PRODUTO { GRIFFE = "" });
            ddlGriffeFiltro.DataSource = griffeFiltro;
            ddlGriffeFiltro.DataBind();
        }
        private void CarregarNomeFiltro()
        {
            List<SP_OBTER_DESENV_COLECAOResult> nomeFiltro = new List<SP_OBTER_DESENV_COLECAOResult>();

            var _nome = g_ProdutoFiltro.Select(s => s.NOME.Trim()).Distinct().ToList();
            foreach (var item in _nome)
                if (item.Trim() != "")
                    nomeFiltro.Add(new SP_OBTER_DESENV_COLECAOResult { NOME = item.Trim() });

            nomeFiltro = nomeFiltro.OrderBy(p => p.NOME).ToList();
            nomeFiltro.Insert(0, new SP_OBTER_DESENV_COLECAOResult { NOME = "" });
            nomeFiltro.Insert(1, new SP_OBTER_DESENV_COLECAOResult { NOME = "VAZIO" });
            ddlNomeFiltro.DataSource = nomeFiltro;
            ddlNomeFiltro.DataBind();
        }
        private void CarregarOrigemFiltro()
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = desenvController.ObterProdutoOrigem(ddlColecoes.SelectedValue);
            if (_origem != null)
            {
                _origem = _origem.Where(i => g_ProdutoFiltro.Any(g => g.DESENV_PRODUTO_ORIGEM == i.CODIGO)).ToList();
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigemFiltro.DataSource = _origem;
                ddlOrigemFiltro.DataBind();
            }
        }
        private void CarregarGrupoFiltro()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("01");
            if (_grupo != null)
            {
                _grupo = _grupo.Where(i => g_ProdutoFiltro.Any(g => g.GRUPO.Trim() == i.GRUPO_PRODUTO.Trim())).ToList();
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                _grupo.Insert(1, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "VAZIO" });
                ddlGrupoFiltro.DataSource = _grupo;
                ddlGrupoFiltro.DataBind();
            }
        }
        private void CarregarSegmentoFiltro()
        {
            List<DESENV_PRODUTO> segmentoFiltro = new List<DESENV_PRODUTO>();

            var _segmento = g_ProdutoFiltro.Where(p => p.SEGMENTO != null).Select(s => s.SEGMENTO.Trim()).Distinct().ToList();
            foreach (var item in _segmento)
                if (item.Trim() != "")
                    segmentoFiltro.Add(new DESENV_PRODUTO { SEGMENTO = item.Trim() });

            segmentoFiltro = segmentoFiltro.OrderBy(p => p.SEGMENTO).ToList();
            segmentoFiltro.Insert(0, new DESENV_PRODUTO { SEGMENTO = "" });
            segmentoFiltro.Insert(1, new DESENV_PRODUTO { SEGMENTO = "VAZIO" });
            ddlSegmentoFiltro.DataSource = segmentoFiltro;
            ddlSegmentoFiltro.DataBind();
        }
        private void CarregarTecidoFiltro()
        {
            List<DESENV_PRODUTO> tecidoFiltro = new List<DESENV_PRODUTO>();

            var _tecido = g_ProdutoFiltro.Where(p => p.TECIDO_POCKET != null).Select(s => s.TECIDO_POCKET.Trim()).Distinct().ToList();
            foreach (var item in _tecido)
                if (item.Trim() != "")
                    tecidoFiltro.Add(new DESENV_PRODUTO { TECIDO_POCKET = item.Trim() });

            tecidoFiltro = tecidoFiltro.OrderBy(p => p.TECIDO_POCKET).ToList();
            tecidoFiltro.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
            tecidoFiltro.Insert(1, new DESENV_PRODUTO { TECIDO_POCKET = "VAZIO" });
            ddlTecidoFiltro.DataSource = tecidoFiltro;
            ddlTecidoFiltro.DataBind();
        }
        private void CarregarCorFiltro()
        {

            List<CORES_BASICA> _coresBasicas = new List<CORES_BASICA>();

            var _cor = g_ProdutoFiltro.Where(p => p.COR != null).Select(s => s.COR.Trim()).Distinct().ToList();
            foreach (var item in _cor)
                if (item.Trim() != "")
                {
                    var descCor = prodController.ObterCoresBasicas(item);
                    if (descCor != null)
                        _coresBasicas.Add(new CORES_BASICA { COR = item.Trim(), DESC_COR = descCor.DESC_COR });
                }

            _coresBasicas = _coresBasicas.OrderBy(p => p.DESC_COR).ToList();
            _coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
            _coresBasicas.Insert(1, new CORES_BASICA { COR = "VAZIO", DESC_COR = "VAZIO" });
            ddlCorFiltro.DataSource = _coresBasicas;
            ddlCorFiltro.DataBind();
        }
        private void CarregarFornecedorFiltro()
        {
            List<PROD_FORNECEDOR> _fornecedor = prodController.ObterFornecedor().Where(p => p.STATUS == 'A' && p.TIPO == 'T').ToList();
            if (_fornecedor != null)
            {
                _fornecedor = _fornecedor.Where(i => g_ProdutoFiltro.Where(j => j.FORNECEDOR_COR != null).Any(g => g.FORNECEDOR.Trim() == i.FORNECEDOR.Trim())).ToList();
                _fornecedor.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "" });
                _fornecedor.Insert(1, new PROD_FORNECEDOR { FORNECEDOR = "VAZIO" });
                ddlFornecedorFiltro.DataSource = _fornecedor;
                ddlFornecedorFiltro.DataBind();
            }
        }
        private void CarregarMostruarioCorteFiltro()
        {
            List<SP_OBTER_DESENV_COLECAOResult> mostruarioCorteFiltro = new List<SP_OBTER_DESENV_COLECAOResult>();

            var _mostCorte = g_ProdutoFiltro.Select(s => s.MOSTRUARIOCORTE.Trim()).Distinct().ToList();
            foreach (var item in _mostCorte)
                if (item.Trim() != "")
                    mostruarioCorteFiltro.Add(new SP_OBTER_DESENV_COLECAOResult { MOSTRUARIOCORTE = item.Trim() });

            mostruarioCorteFiltro = mostruarioCorteFiltro.OrderBy(p => p.MOSTRUARIOCORTE).ToList();
            mostruarioCorteFiltro.Insert(0, new SP_OBTER_DESENV_COLECAOResult { MOSTRUARIOCORTE = "" });
            mostruarioCorteFiltro.Insert(1, new SP_OBTER_DESENV_COLECAOResult { MOSTRUARIOCORTE = "VAZIO" });
            ddlMostruarioCorteFiltro.DataSource = mostruarioCorteFiltro;
            ddlMostruarioCorteFiltro.DataBind();
        }
        private void CarregarVarejoFiltro()
        {
            List<DESENV_PRODUTO> varejoFiltro = new List<DESENV_PRODUTO>();

            var _varejo = g_ProdutoFiltro.Where(p => p.CORTE_VAREJO != null).Select(s => s.CORTE_VAREJO.Trim()).Distinct().ToList();
            foreach (var item in _varejo)
                if (item.Trim() != "")
                    varejoFiltro.Add(new DESENV_PRODUTO { CORTE_VAREJO = item.Trim() });

            varejoFiltro = varejoFiltro.OrderBy(p => p.CORTE_VAREJO).ToList();
            varejoFiltro.Insert(0, new DESENV_PRODUTO { CORTE_VAREJO = "" });
            varejoFiltro.Insert(1, new DESENV_PRODUTO { CORTE_VAREJO = "VAZIO" });
            ddlVarejoFiltro.DataSource = varejoFiltro;
            ddlVarejoFiltro.DataBind();
        }
        private void CarregarAtacadoFiltro()
        {
            List<DESENV_PRODUTO> atacadoFiltro = new List<DESENV_PRODUTO>();

            var _atacado = g_ProdutoFiltro.Where(p => p.CORTE_ATACADO != null).Select(s => s.CORTE_ATACADO.Trim()).Distinct().ToList();
            foreach (var item in _atacado)
                if (item.Trim() != "")
                    atacadoFiltro.Add(new DESENV_PRODUTO { CORTE_ATACADO = item.Trim() });

            atacadoFiltro = atacadoFiltro.OrderBy(p => p.CORTE_ATACADO).ToList();
            atacadoFiltro.Insert(0, new DESENV_PRODUTO { CORTE_ATACADO = "" });
            atacadoFiltro.Insert(1, new DESENV_PRODUTO { CORTE_ATACADO = "VAZIO" });
            ddlAtacadoFiltro.DataSource = atacadoFiltro;
            ddlAtacadoFiltro.DataBind();
        }
        private void CarregarCorFornecedorFiltro()
        {
            List<DESENV_PRODUTO> corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = g_ProdutoFiltro.Where(p => p.FORNECEDOR_COR != null).Select(s => s.FORNECEDOR_COR.Trim()).Distinct().ToList();
            foreach (var item in _corFornecedor)
                if (item.Trim() != "")
                    corFornecedorFiltro.Add(new DESENV_PRODUTO { FORNECEDOR_COR = item.Trim() });

            corFornecedorFiltro = corFornecedorFiltro.OrderBy(p => p.FORNECEDOR_COR).ToList();
            corFornecedorFiltro.Insert(0, new DESENV_PRODUTO { FORNECEDOR_COR = "" });
            corFornecedorFiltro.Insert(1, new DESENV_PRODUTO { FORNECEDOR_COR = "VAZIO" });
            ddlCorFornecedorFiltro.DataSource = corFornecedorFiltro;
            ddlCorFornecedorFiltro.DataBind();
        }
        private void CarregarRefModelagemFiltro()
        {
            List<DESENV_PRODUTO> refModelagemFiltro = new List<DESENV_PRODUTO>();

            var _refModelagem = g_ProdutoFiltro.Where(p => p.REF_MODELAGEM != null).Select(s => s.REF_MODELAGEM.Trim()).Distinct().ToList();
            foreach (var item in _refModelagem)
                if (item.Trim() != "")
                    refModelagemFiltro.Add(new DESENV_PRODUTO { REF_MODELAGEM = item.Trim() });

            refModelagemFiltro = refModelagemFiltro.OrderBy(p => p.REF_MODELAGEM).ToList();
            refModelagemFiltro.Insert(0, new DESENV_PRODUTO { REF_MODELAGEM = "" });
            refModelagemFiltro.Insert(1, new DESENV_PRODUTO { REF_MODELAGEM = "VAZIO" });
            ddlRefModelagemFiltro.DataSource = refModelagemFiltro;
            ddlRefModelagemFiltro.DataBind();
        }
        private void CarregarBrindeFiltro()
        {
            List<DESENV_PRODUTO> brindeFiltro = new List<DESENV_PRODUTO>();

            var _brinde = g_ProdutoFiltro.Where(p => p.BRINDE != null).Select(s => s.BRINDE).Distinct().ToList();
            foreach (var item in _brinde)
                if (item != ' ')
                    brindeFiltro.Add(new DESENV_PRODUTO { BRINDE = item });

            brindeFiltro = brindeFiltro.OrderBy(p => p.BRINDE).ToList();
            brindeFiltro.Insert(0, new DESENV_PRODUTO { BRINDE = ' ' });
            ddlBrindeFiltro.DataSource = brindeFiltro;
            ddlBrindeFiltro.DataBind();
        }
        private void CarregarProdutoAcabadoFiltro()
        {
            List<DESENV_PRODUTO> produtoAcabadoFiltro = new List<DESENV_PRODUTO>();

            var _produtoAcabado = g_ProdutoFiltro.Where(p => p.PRODUTO_ACABADO != null).Select(s => s.PRODUTO_ACABADO).Distinct().ToList();
            foreach (var item in _produtoAcabado)
                if (item != ' ')
                    produtoAcabadoFiltro.Add(new DESENV_PRODUTO { PRODUTO_ACABADO = item });

            produtoAcabadoFiltro = produtoAcabadoFiltro.OrderBy(p => p.PRODUTO_ACABADO).ToList();
            produtoAcabadoFiltro.Insert(0, new DESENV_PRODUTO { PRODUTO_ACABADO = ' ' });
            ddlProdutoAcabadoFiltro.DataSource = produtoAcabadoFiltro;
            ddlProdutoAcabadoFiltro.DataBind();
        }
        private void CarregarSignedFiltro()
        {
            List<DESENV_PRODUTO> signedFiltro = new List<DESENV_PRODUTO>();

            var _signed = g_ProdutoFiltro.Where(p => p.SIGNED != null).Select(s => s.SIGNED).Distinct().ToList();
            foreach (var item in _signed)
                if (item != ' ')
                    signedFiltro.Add(new DESENV_PRODUTO { SIGNED = item });

            signedFiltro = signedFiltro.OrderBy(p => p.SIGNED).ToList();
            signedFiltro.Insert(0, new DESENV_PRODUTO { SIGNED = ' ' });
            ddlSignedFiltro.DataSource = signedFiltro;
            ddlSignedFiltro.DataBind();
        }
        private void CarregarSignedNomeFiltro()
        {
            List<DESENV_PRODUTO> signedNomeFiltro = new List<DESENV_PRODUTO>();

            var _signedNome = g_ProdutoFiltro.Where(p => p.SIGNED_NOME != null).Select(s => s.SIGNED_NOME).Distinct().ToList();
            foreach (var item in _signedNome)
                if (item != "")
                    signedNomeFiltro.Add(new DESENV_PRODUTO { SIGNED_NOME = item });

            signedNomeFiltro = signedNomeFiltro.OrderBy(p => p.SIGNED_NOME).ToList();
            signedNomeFiltro.Insert(0, new DESENV_PRODUTO { SIGNED_NOME = "" });
            ddlSignedNomeFiltro.DataSource = signedNomeFiltro;
            ddlSignedNomeFiltro.DataBind();
        }
        private void CarregarFiltros()
        {
            CarregarModeloFiltro();
            CarregarGriffeFiltro();
            CarregarNomeFiltro();
            CarregarOrigemFiltro();
            CarregarGrupoFiltro();
            CarregarSegmentoFiltro();
            CarregarTecidoFiltro();
            CarregarCorFiltro();
            CarregarFornecedorFiltro();
            CarregarVarejoFiltro();
            CarregarAtacadoFiltro();
            CarregarCorFornecedorFiltro();
            CarregarRefModelagemFiltro();
            CarregarMostruarioCorteFiltro();
            CarregarBrindeFiltro();
            CarregarProdutoAcabadoFiltro();
            CarregarSignedFiltro();
            CarregarSignedNomeFiltro();
        }
        #endregion

        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                CarregarOrigem(colecao);
                ObterCodigoREF(colecao);
            }
        }
        protected void btAtualizar_Click(object sender, EventArgs e)
        {

            labErro.Text = "";

            if (ddlMarca.SelectedValue.Trim() == "" || ddlMarca.SelectedValue.Trim() == "0")
            {
                labErro.Text = "Selecione a Marca...";
                return;
            }

            if (ddlColecoes.SelectedValue.Trim() == "" || ddlColecoes.SelectedValue.Trim() == "0")
            {
                labErro.Text = "Selecione a Coleção...";
                return;
            }


            btLimpar_Click(null, null);
            pnlTripa.Visible = true;

            RecarregarProduto();
            FiltroGeral(null, true, false);

            //Carregar filtros do GRID
            CarregarFiltros();
        }
        private void ObterCodigoREF(string colecao)
        {
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
