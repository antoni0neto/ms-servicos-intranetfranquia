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
    public partial class desenv_acessorio_manut : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        List<SP_OBTER_DESENV_COLECAO_ACESSResult> g_AcessorioFiltro = new List<SP_OBTER_DESENV_COLECAO_ACESSResult>();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPrevEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

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
                CarregarCores();

                CarregarPrecoFiltro();
                CarregarCustoFiltro();
                CarregarQtdeFiltro();
                CarregarDescricaoSugeridaFiltro();

                //Foco no coleção
                ddlColecoes.Focus();

                //Filtro da TRIPA
                Session["TRIPA_ACESS"] = null;
                ViewState["sortDirection"] = true;
            }

            //Evitar duplo clique no botão
            btAtualizar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizar, null) + ";");
            txtDataPrevEntrega.Attributes.Add("onkeyup", "formataData(this, event);");

        }

        #region "MANUTENÇÃO"
        private void RecarregarAcessorio()
        {
            //Obter lista de acessorios
            List<SP_OBTER_DESENV_COLECAO_ACESSResult> _lstAcessorio = new List<SP_OBTER_DESENV_COLECAO_ACESSResult>();
            _lstAcessorio = desenvController.ObterDesenvolvimentoColecaoAcessorio(ddlColecoes.SelectedValue);

            //Obter tudo
            g_AcessorioFiltro = new List<SP_OBTER_DESENV_COLECAO_ACESSResult>();
            g_AcessorioFiltro.AddRange(_lstAcessorio);

            Session["TRIPA_ACESS"] = _lstAcessorio;
        }
        protected void gvAcessorio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DESENV_COLECAO_ACESSResult _acessorio = e.Row.DataItem as SP_OBTER_DESENV_COLECAO_ACESSResult;

                    if (_acessorio != null)
                    {

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                            _litNome.Text = _acessorio.NOME.Trim();

                        Label _labDataPedido = e.Row.FindControl("labDataPedido") as Label;
                        if (_labDataPedido != null)
                            _labDataPedido.Text = (_acessorio.DATA_PEDIDO == null) ? "" : (Convert.ToDateTime(_acessorio.DATA_PEDIDO).ToString("dd/MM/yyyy"));

                        Label _labPreco = e.Row.FindControl("labPreco") as Label;
                        if (_labPreco != null)
                            _labPreco.Text = (_acessorio.PRECO == null) ? "" : ("R$ " + Convert.ToDecimal(_acessorio.PRECO).ToString(""));

                        Label _labCusto = e.Row.FindControl("labCusto") as Label;
                        if (_labCusto != null)
                            _labCusto.Text = (_acessorio.CUSTO == null) ? "" : ("R$ " + Convert.ToDecimal(_acessorio.CUSTO).ToString(""));

                        ImageButton _btAcessorioExcluir = e.Row.FindControl("btAcessorioExcluir") as ImageButton;
                        if (_btAcessorioExcluir != null)
                        {
                            _btAcessorioExcluir.Attributes.Add("onclick", "return ConfirmarExclusao();");
                            if (_acessorio.STATUS != 'A')
                            {
                                _btAcessorioExcluir.Attributes.Add("onclick", "");
                                e.Row.ForeColor = Color.Red;
                                _btAcessorioExcluir.CommandName = "IN"; //FLAG PARA MARCAR INSERÇÃO
                                _btAcessorioExcluir.ImageUrl = "~/Image/insert.jpg";
                                _btAcessorioExcluir.ToolTip = "Incluir";
                            }
                            _btAcessorioExcluir.CommandArgument = _acessorio.CODIGO.ToString();
                        }

                        ImageButton _btAcessorioCopiar = e.Row.FindControl("btAcessorioCopiar") as ImageButton;
                        if (_btAcessorioCopiar != null)
                            _btAcessorioCopiar.CommandArgument = _acessorio.CODIGO.ToString();

                        ImageButton _btAcessorioImprimir = e.Row.FindControl("btAcessorioImprimir") as ImageButton;
                        if (_btAcessorioImprimir != null)
                            _btAcessorioImprimir.CommandArgument = _acessorio.CODIGO.ToString();

                        ImageButton _btAcessorioVisualizar = e.Row.FindControl("btAcessorioVisualizar") as ImageButton;
                        if (_btAcessorioVisualizar != null)
                        {
                            _btAcessorioVisualizar.CommandArgument = _acessorio.CODIGO.ToString();
                            if (_acessorio.FOTO1 == null || _acessorio.FOTO1.Trim() == "" || _acessorio.FOTO1.Trim().Length < 5)
                            {
                                _btAcessorioVisualizar.ImageUrl = "~/Image/no_image.png";
                                _btAcessorioVisualizar.ToolTip = "Sem Foto";
                            }
                        }

                        ImageButton _btAcessorioEditar = e.Row.FindControl("btAcessorioEditar") as ImageButton;
                        _btAcessorioEditar.CommandArgument = _acessorio.CODIGO.ToString();
                    }
                }
            }
        }
        protected void btAcessorioExcluir_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string header = "";
            if (b != null)
            {
                try
                {
                    labSalvar.Text = "";

                    DESENV_ACESSORIO _acessorio = new DESENV_ACESSORIO();
                    _acessorio = desenvController.ObterAcessorio(Convert.ToInt32(b.CommandArgument));

                    //VALIDA PEDIDO EXISTENTE
                    var pedidoAcessorio = desenvController.ObterAcessorioPedido(_acessorio.CODIGO.ToString());
                    if (pedidoAcessorio != null && pedidoAcessorio.Count() > 0)
                    {
                        msg = "Acessório não pode ser Excluído. Possui PEDIDO.";
                        header = "Aviso";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '" + header + "', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);

                        return;
                    }

                    if (_acessorio != null)
                    {

                        if (b.CommandName == "EX")
                        {
                            msg = "Acessório excluído com sucesso.";
                            header = "Exclusão";

                            _acessorio.STATUS = 'E'; //Excluido
                        }
                        else
                        {
                            _acessorio.STATUS = 'A'; //ATIVO
                            msg = "Acessório adicionado com sucesso.";
                            header = "Inclusão";
                        }

                        //Atualizar produto
                        desenvController.AtualizarAcessorio(_acessorio);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '" + header + "', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                        RecarregarAcessorio();
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
        protected void btAcessorioCopiar_Click(object sender, EventArgs e)
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

                    DESENV_ACESSORIO _acessorio = new DESENV_ACESSORIO();
                    _acessorio = desenvController.ObterAcessorio(Convert.ToInt32(b.CommandArgument));
                    if (_acessorio != null)
                    {

                        if (_acessorio.DESENV_PRODUTO_ORIGEM > 0)
                            ddlOrigem.SelectedValue = _acessorio.DESENV_PRODUTO_ORIGEM.ToString();

                        if (_acessorio.GRUPO != null && _acessorio.GRUPO != "")
                            ddlGrupo.SelectedValue = prodController.ObterGrupoProduto("02").Where(p => p.GRUPO_PRODUTO.Trim() == _acessorio.GRUPO.Trim()).SingleOrDefault().GRUPO_PRODUTO;

                        txtProduto.Text = _acessorio.PRODUTO;

                        if (_acessorio.GRIFFE != null && _acessorio.GRIFFE.Trim() != "")
                            ddlGriffe.SelectedValue = new BaseController().BuscaGriffeProduto(_acessorio.GRIFFE).GRIFFE;

                        if (_acessorio.FORNECEDOR != null)
                        {
                            var fornecedor = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == _acessorio.FORNECEDOR.Trim() && p.TIPO == 'C').SingleOrDefault();
                            if (fornecedor != null)
                                ddlFornecedor.SelectedValue = fornecedor.FORNECEDOR.Trim();
                        }

                        var _cor = prodController.ObterCoresBasicas(_acessorio.COR);
                        if (_cor != null)
                            ddlCor.SelectedValue = _cor.COR.Trim();

                        txtCorFornecedor.Text = _acessorio.COR_FORNECEDOR;

                        txtPreco.Text = _acessorio.PRECO.ToString();
                        txtCusto.Text = _acessorio.CUSTO.ToString();
                        txtQuantidade.Text = _acessorio.QTDE.ToString();
                        txtDescricaoSugerida.Text = _acessorio.DESCRICAO_SUGERIDA;
                        txtReferFabricante.Text = _acessorio.REFER_FABRICANTE;
                        txtDataPrevEntrega.Text = _acessorio.DATA_PREVISAO_ENTREGA;

                        txtObservacao.Text = (_acessorio.OBS == null) ? "" : _acessorio.OBS.ToString();

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
        protected void btAcessorioVisualizar_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labSalvar.Text = "";
                    string codigoAcessorio = b.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('desenv_acessorio_foto.aspx?a=" + codigoAcessorio + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btAcessorioEditar_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labSalvar.Text = "";
                    string codigoAcessorio = b.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('desenv_acessorio_editar.aspx?a=" + codigoAcessorio + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }

        }

        protected void lnkColuna_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            if (lnk != null)
            {
                foreach (GridViewRow r in gvAcessorio.Rows)
                    r.Font.Bold = false;

                GridViewRow row = (GridViewRow)lnk.NamingContainer;
                if (row != null)
                    row.Font.Bold = true;
            }
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

            labFornecedor.ForeColor = _OK;
            if (ddlFornecedor.SelectedValue.Trim() == "")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labCor.ForeColor = _OK;
            if (ddlCor.SelectedValue.Trim() == "")
            {
                labCor.ForeColor = _notOK;
                retorno = false;
            }

            labPreco.ForeColor = _OK;
            if (txtPreco.Text.Trim() == "")
            {
                labPreco.ForeColor = _notOK;
                retorno = false;
            }

            labCusto.ForeColor = _OK;
            if (txtCusto.Text.Trim() == "")
            {
                labCusto.ForeColor = _notOK;
                retorno = false;
            }

            labQuantidade.ForeColor = _OK;
            if (txtQuantidade.Text.Trim() == "")
            {
                labQuantidade.ForeColor = _notOK;
                retorno = false;
            }

            labDescricaoSugerida.ForeColor = _OK;
            if (txtDescricaoSugerida.Text.Trim() == "")
            {
                labDescricaoSugerida.ForeColor = _notOK;
                retorno = false;
            }

            labPrevEntrega.ForeColor = _OK;
            if (txtDataPrevEntrega.Text.Trim() == "")
            {
                labPrevEntrega.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            DESENV_ACESSORIO _acessorio = null;

            try
            {
                labSalvar.Text = "";

                //Validação de nulos
                if (!ValidarCampos())
                {
                    labSalvar.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                _acessorio = new DESENV_ACESSORIO();
                _acessorio.COLECAO = ddlColecoes.Text.Trim();
                _acessorio.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(ddlOrigem.SelectedValue.Trim());
                _acessorio.GRUPO = ddlGrupo.SelectedValue.Trim();
                _acessorio.PRODUTO = txtProduto.Text.Trim().ToUpper();
                _acessorio.GRIFFE = ddlGriffe.SelectedValue.Trim();
                _acessorio.FORNECEDOR = ddlFornecedor.SelectedValue.ToUpper();
                _acessorio.COR = ddlCor.SelectedValue.Trim().ToUpper();
                _acessorio.COR_FORNECEDOR = txtCorFornecedor.Text.Trim().ToUpper();
                _acessorio.OBS = txtObservacao.Text.Trim().ToUpper();

                _acessorio.PRECO = Convert.ToDecimal(txtPreco.Text.Trim());
                _acessorio.CUSTO = Convert.ToDecimal(txtCusto.Text.Trim());
                _acessorio.QTDE = Convert.ToInt32(txtQuantidade.Text.Trim());
                _acessorio.DESCRICAO_SUGERIDA = txtDescricaoSugerida.Text.Trim().ToUpper();
                _acessorio.REFER_FABRICANTE = txtReferFabricante.Text.Trim().ToUpper();
                _acessorio.DATA_PREVISAO_ENTREGA = txtDataPrevEntrega.Text.Trim().ToUpper();

                _acessorio.DATA_INCLUSAO = DateTime.Now;
                _acessorio.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _acessorio.STATUS = 'A';

                desenvController.InserirAcessorio(_acessorio);

                ddlCor.SelectedValue = "";
                txtCorFornecedor.Text = "";

                labSalvar.Text = "Acessório cadastrado com sucesso.";
                labAcao.ForeColor = Color.Gray;

                Session["COLECAO"] = ddlColecoes.SelectedValue;

                btAtualizar_Click(null, null);

                if (Constante.enviarEmail)
                    EnviarEmail(_acessorio);

            }
            catch (Exception ex)
            {
                labSalvar.Text = ex.Message;
            }
        }

        #region "EMAIL"
        private void EnviarEmail(DESENV_ACESSORIO produtos)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Cadastro de Acessórios - " + produtos.PRODUTO;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(produtos);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(37, 13).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(DESENV_ACESSORIO produtos)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            BaseController baseController = new BaseController();

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Cadastro de Acessório</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divPreco' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 600pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:left;'>");
            sb.Append("                        <h3>Cadastro de Acessório - " + produtos.PRODUTO + "</h3>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 600pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    Coleção:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ddlColecoes.SelectedItem.ToString());
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Produto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.PRODUTO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            PRODUTO produto = baseController.BuscaProduto(produtos.PRODUTO);
            if (produto != null)
            {
                if (produto.DESC_PRODUTO != null)
                {
                    sb.Append("                            <tr>");
                    sb.Append("                                <td>");
                    sb.Append("                                    Nome:");
                    sb.Append("                                </td>");
                    sb.Append("                                <td>");
                    sb.Append("                                    " + produto.DESC_PRODUTO);
                    sb.Append("                                </td>");
                    sb.Append("                                <td>");
                    sb.Append("                                    &nbsp;");
                    sb.Append("                                </td>");
                    sb.Append("                            </tr>");
                }
            }

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Grupo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.GRUPO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Griffe:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.GRIFFE);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string cores = "";
            ProducaoController prodController = new ProducaoController();
            cores = produtos.COR + " - " + prodController.ObterCoresBasicas(produtos.COR).DESC_COR.Trim();
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + cores);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.COR_FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$" + produtos.PRECO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Custo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$" + produtos.CUSTO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Quantidade:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.QTDE);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Previsão de Entrega:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.DATA_PREVISAO_ENTREGA);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Descrição Sugerida:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.DESCRICAO_SUGERIDA);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Referência Fabricante:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.REFER_FABRICANTE);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Observação:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.OBS);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + (new BaseController().BuscaUsuario(codigoUsuario).NOME_USUARIO.ToUpper()) + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion

        protected void btLimpar_Click(object sender, EventArgs e)
        {
            ddlGrupo.SelectedValue = "";
            txtProduto.Text = "";
            ddlGriffe.SelectedValue = "";
            ddlFornecedor.SelectedValue = "";

            ddlCor.SelectedValue = "";
            txtCorFornecedor.Text = "";
            txtObservacao.Text = "";

            txtPreco.Text = "";
            txtCusto.Text = "";
            txtQuantidade.Text = "";
            txtDescricaoSugerida.Text = "";
            txtReferFabricante.Text = "";
            txtDataPrevEntrega.Text = "";

            labSalvar.Text = "";
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

                if (_origem.Count == 2)
                    ddlOrigem.SelectedValue = _origem[1].CODIGO.ToString();

            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = (new BaseController().BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
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

        private List<SP_OBTER_GRUPOResult> RetornarGrupo()
        {
            return (prodController.ObterGrupoProduto("02"));
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
            return _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'C') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();
        }
        #endregion

        #region "FILTROS"
        protected void chkFoto_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RecarregarAcessorio();
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
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl != null)
            {
                RecarregarAcessorio();
                FiltroGeral(ddl, (bool)ViewState["sortDirection"], true);
            }
        }
        protected void gvAcessorio_Sorting(object sender, GridViewSortEventArgs e)
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
                coluna = 5;
                gv.Columns[coluna].HeaderText = "Produto";
                gv.Columns[coluna + 1].HeaderText = "Grupo";
                gv.Columns[coluna + 2].HeaderText = "Nome";
                coluna = 7;
                gv.Columns[coluna + 1].HeaderText = "Origem";
                gv.Columns[coluna + 2].HeaderText = "Griffe";
                gv.Columns[coluna + 3].HeaderText = "Cor";
                gv.Columns[coluna + 4].HeaderText = "Cor Fornecedor";
                gv.Columns[coluna + 5].HeaderText = "Fornecedor";
                gv.Columns[coluna + 6].HeaderText = "Preço";
                gv.Columns[coluna + 7].HeaderText = "Custo";
                gv.Columns[coluna + 8].HeaderText = "Quantidade";
                gv.Columns[coluna + 9].HeaderText = "Previsão Entrega";
                gv.Columns[coluna + 10].HeaderText = "Descrição Sugerida";
                gv.Columns[coluna + 11].HeaderText = "Referência Fabricante";
                gv.Columns[coluna + 12].HeaderText = "Pedido";

                coluna = 5;
                if (e.SortExpression == "ddlProdutoFiltro") gv.Columns[coluna].HeaderText = gv.Columns[coluna].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlGrupoFiltro") gv.Columns[coluna + 1].HeaderText = gv.Columns[coluna + 1].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlNomeFiltro") gv.Columns[coluna + 2].HeaderText = gv.Columns[coluna + 2].HeaderText + " " + ((order) ? " - >>" : " - <<");
                coluna = 7;
                if (e.SortExpression == "ddlOrigemFiltro") gv.Columns[coluna + 1].HeaderText = gv.Columns[coluna + 1].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlGriffeFiltro") gv.Columns[coluna + 2].HeaderText = gv.Columns[coluna + 2].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlCorFiltro") gv.Columns[coluna + 3].HeaderText = gv.Columns[coluna + 3].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlCorFornecedorFiltro") gv.Columns[coluna + 4].HeaderText = gv.Columns[coluna + 4].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlFornecedorFiltro") gv.Columns[coluna + 5].HeaderText = gv.Columns[coluna + 5].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlPrecoFiltro") gv.Columns[coluna + 6].HeaderText = gv.Columns[coluna + 6].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlCustoFiltro") gv.Columns[coluna + 7].HeaderText = gv.Columns[coluna + 7].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlQuantidadeFiltro") gv.Columns[coluna + 8].HeaderText = gv.Columns[coluna + 8].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlDataPrevisaoEntregaFiltro") gv.Columns[coluna + 9].HeaderText = gv.Columns[coluna + 9].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlDescricaoSugeridaFiltro") gv.Columns[coluna + 10].HeaderText = gv.Columns[coluna + 10].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlReferenciaFabricanteFiltro") gv.Columns[coluna + 11].HeaderText = gv.Columns[coluna + 11].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "ddlPedidoFiltro") gv.Columns[coluna + 12].HeaderText = gv.Columns[coluna + 12].HeaderText + " " + ((order) ? " - >>" : " - <<");

            }

            DropDownList ddl = new DropDownList();
            ddl.ID = e.SortExpression;

            FiltroGeral(ddl, order, false);
        }
        private void FiltroGeral(DropDownList ddlFiltro, bool pAsc, bool pHeader)
        {
            int coluna = 0;
            const string _VAZIO = "VAZIO";

            if (Session["TRIPA_ACESS"] != null)
            {
                if (g_AcessorioFiltro != null && g_AcessorioFiltro.Count <= 0)
                    g_AcessorioFiltro.AddRange((List<SP_OBTER_DESENV_COLECAO_ACESSResult>)Session["TRIPA_ACESS"]);

                //FILTROS DOS  COMBOS
                if (ddlProdutoFiltro.SelectedValue.Trim() != "0" && ddlProdutoFiltro.SelectedValue.Trim() != "")
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.PRODUTO.Trim().ToUpper() == ddlProdutoFiltro.SelectedValue.Trim().ToUpper()).ToList();

                if (ddlGriffeFiltro.SelectedValue.Trim() != "0" && ddlGriffeFiltro.SelectedValue.Trim() != "")
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.GRIFFE == null) ? "" : p.GRIFFE.ToUpper().Trim()) == ddlGriffeFiltro.SelectedValue.ToUpper().Trim()).ToList();

                //FILTRO NOME
                if (ddlNomeFiltro.SelectedValue.Trim() != "0" && ddlNomeFiltro.SelectedValue.Trim() != "" && ddlNomeFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.NOME.Trim().ToUpper() == ddlNomeFiltro.SelectedValue.Trim().ToUpper()).ToList();
                else if (ddlNomeFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.NOME == null || p.NOME.Trim() == "").ToList();

                //FILTRO ORIGEM
                if (ddlOrigemFiltro.SelectedValue.Trim() != "0" && ddlOrigemFiltro.SelectedValue.Trim() != "")
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.DESENV_PRODUTO_ORIGEM.ToString() == ddlOrigemFiltro.SelectedValue).ToList();

                //FILTRO GRUPO
                if (ddlGrupoFiltro.SelectedValue.Trim() != "0" && ddlGrupoFiltro.SelectedValue.Trim() != "" && ddlGrupoFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.GRUPO == null) ? "" : p.GRUPO.Trim()) == ddlGrupoFiltro.SelectedValue.Trim()).ToList();
                else if (ddlGrupoFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.GRUPO == null || p.GRUPO.Trim() == "").ToList();

                //FILTRO COR
                if (ddlCorFiltro.SelectedValue.Trim() != "0" && ddlCorFiltro.SelectedValue.Trim() != "" && ddlCorFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.COR.ToUpper().Trim() == ddlCorFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlCorFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.COR == null || p.COR.Trim() == "").ToList();

                //FILTRO FORNECEDOR
                if (ddlFornecedorFiltro.SelectedValue.Trim() != "0" && ddlFornecedorFiltro.SelectedValue.Trim() != "" && ddlFornecedorFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.FORNECEDOR == null) ? "" : p.FORNECEDOR.ToUpper().Trim()) == ddlFornecedorFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlFornecedorFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.FORNECEDOR == null || p.FORNECEDOR.Trim() == "").ToList();

                //FILTRO COR FORNECEDOR
                if (ddlCorFornecedorFiltro.SelectedValue.Trim() != "0" && ddlCorFornecedorFiltro.SelectedValue.Trim() != "" && ddlCorFornecedorFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.COR_FORNECEDOR == null) ? "" : p.COR_FORNECEDOR.ToUpper().Trim()) == ddlCorFornecedorFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlCorFornecedorFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.COR_FORNECEDOR == null || p.COR_FORNECEDOR.Trim() == "").ToList();

                //FILTRO PEDIDO
                if (ddlPedidoFiltro.SelectedValue.Trim() != "0" && ddlPedidoFiltro.SelectedValue.Trim() != "" && ddlPedidoFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.PEDIDO == null) ? "" : p.PEDIDO.ToUpper().Trim()) == ddlPedidoFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlPedidoFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.PEDIDO == null || p.PEDIDO.Trim() == "").ToList();


                //PRECO
                if (ddlPrecoFiltro.SelectedValue.Trim() != "0" && ddlPrecoFiltro.SelectedValue.Trim() != "" && ddlPrecoFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.PRECO == null) ? 0 : p.PRECO) == Convert.ToDecimal(ddlPrecoFiltro.SelectedValue)).ToList();
                else if (ddlPrecoFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.PRECO == null || p.PRECO == 0).ToList();

                //CUSTO
                if (ddlCustoFiltro.SelectedValue.Trim() != "0" && ddlCustoFiltro.SelectedValue.Trim() != "" && ddlCustoFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.CUSTO == null) ? 0 : p.CUSTO) == Convert.ToDecimal(ddlCustoFiltro.SelectedValue)).ToList();
                else if (ddlCustoFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.CUSTO == null || p.CUSTO == 0).ToList();

                //QUANTIDADE
                if (ddlQuantidadeFiltro.SelectedValue.Trim() != "0" && ddlQuantidadeFiltro.SelectedValue.Trim() != "" && ddlQuantidadeFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.QTDE == null) ? 0 : p.QTDE) == Convert.ToDecimal(ddlQuantidadeFiltro.SelectedValue)).ToList();
                else if (ddlQuantidadeFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.QTDE == null || p.QTDE == 0).ToList();

                //DATA PREVISÃO ENTREGA
                if (ddlDataPrevisaoEntregaFiltro.SelectedValue.Trim() != "0" && ddlDataPrevisaoEntregaFiltro.SelectedValue.Trim() != "" && ddlDataPrevisaoEntregaFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.DATA_PREVISAO_ENTREGA == null) ? "" : p.DATA_PREVISAO_ENTREGA.ToUpper().Trim()) == ddlDataPrevisaoEntregaFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlDescricaoSugeridaFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.DATA_PREVISAO_ENTREGA == null || p.DATA_PREVISAO_ENTREGA.Trim() == "").ToList();

                //DESCRICAO SUGERIDA
                if (ddlDescricaoSugeridaFiltro.SelectedValue.Trim() != "0" && ddlDescricaoSugeridaFiltro.SelectedValue.Trim() != "" && ddlDescricaoSugeridaFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.DESCRICAO_SUGERIDA == null) ? "" : p.DESCRICAO_SUGERIDA.ToUpper().Trim()) == ddlDescricaoSugeridaFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlDescricaoSugeridaFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.DESCRICAO_SUGERIDA == null || p.DESCRICAO_SUGERIDA.Trim() == "").ToList();

                //REFERENCIA FABRICANTE
                if (ddlReferenciaFabricanteFiltro.SelectedValue.Trim() != "0" && ddlReferenciaFabricanteFiltro.SelectedValue.Trim() != "" && ddlReferenciaFabricanteFiltro.SelectedValue.Trim() != _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.REFER_FABRICANTE == null) ? "" : p.REFER_FABRICANTE.ToUpper().Trim()) == ddlReferenciaFabricanteFiltro.SelectedValue.ToUpper().Trim()).ToList();
                else if (ddlReferenciaFabricanteFiltro.SelectedValue.Trim() == _VAZIO)
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.REFER_FABRICANTE == null || p.REFER_FABRICANTE.Trim() == "").ToList();


                //Filtra os que nao tem foto
                if (ddlFiltro != null && ddlFiltro.ID == "FOTO")
                    g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.FOTO1 == null || p.FOTO1.Trim() == "" || p.FOTO1.Length <= 5).ToList();


                //ORDENAÇÃO
                if (ddlFiltro != null)
                {
                    if (pAsc)
                    {
                        if (ddlFiltro.ID == "ddlProdutoFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.PRODUTO).ToList();
                        if (ddlFiltro.ID == "ddlGriffeFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.GRIFFE).ToList();
                        if (ddlFiltro.ID == "ddlNomeFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.NOME).ToList();
                        if (ddlFiltro.ID == "ddlOrigemFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.DESCRICAO_ORIGEM).ToList();
                        if (ddlFiltro.ID == "ddlGrupoFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.GRUPO).ToList();
                        if (ddlFiltro.ID == "ddlCorFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.COR).ToList();
                        if (ddlFiltro.ID == "ddlFornecedorFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.FORNECEDOR).ToList();
                        if (ddlFiltro.ID == "ddlCorFornecedorFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.COR_FORNECEDOR).ToList();
                        if (ddlFiltro.ID == "ddlPedidoFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.PEDIDO).ToList();

                        if (ddlFiltro.ID == "ddlPrecoFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.PRECO).ToList();
                        if (ddlFiltro.ID == "ddlCustoFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.CUSTO).ToList();
                        if (ddlFiltro.ID == "ddlQuantidadeFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.QTDE).ToList();
                        if (ddlFiltro.ID == "ddlDataPrevisaoEntregaFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.DATA_PREVISAO_ENTREGA).ToList();
                        if (ddlFiltro.ID == "ddlDescricaoSugeridaFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.DESCRICAO_SUGERIDA).ToList();
                        if (ddlFiltro.ID == "ddlReferenciaFabricanteFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.REFER_FABRICANTE).ToList();

                        if (ddlFiltro.ID == "FOTO") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.PRODUTO).ToList();
                    }
                    else
                    {
                        if (ddlFiltro.ID == "ddlProdutoFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.PRODUTO).ToList();
                        if (ddlFiltro.ID == "ddlGriffeFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.GRIFFE).ToList();
                        if (ddlFiltro.ID == "ddlNomeFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.NOME).ToList();
                        if (ddlFiltro.ID == "ddlOrigemFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.DESCRICAO_ORIGEM).ToList();
                        if (ddlFiltro.ID == "ddlGrupoFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.GRUPO).ToList();
                        if (ddlFiltro.ID == "ddlCorFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.COR).ToList();
                        if (ddlFiltro.ID == "ddlFornecedorFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.FORNECEDOR).ToList();
                        if (ddlFiltro.ID == "ddlCorFornecedorFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.COR_FORNECEDOR).ToList();
                        if (ddlFiltro.ID == "ddlPedidoFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.PEDIDO).ToList();

                        if (ddlFiltro.ID == "ddlPrecoFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.PRECO).ToList();
                        if (ddlFiltro.ID == "ddlCustoFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.CUSTO).ToList();
                        if (ddlFiltro.ID == "ddlQuantidadeFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.QTDE).ToList();
                        if (ddlFiltro.ID == "ddlDataPrevisaoEntregaFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.DATA_PREVISAO_ENTREGA).ToList();
                        if (ddlFiltro.ID == "ddlDescricaoSugeridaFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.DESCRICAO_SUGERIDA).ToList();
                        if (ddlFiltro.ID == "ddlReferenciaFabricanteFiltro") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.REFER_FABRICANTE).ToList();

                        if (ddlFiltro.ID == "FOTO") g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenByDescending(p => p.PRODUTO).ToList();
                    }
                }
                else
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(x => x.STATUS).ThenBy(p => p.PRODUTO).ToList();
                }

                if (pHeader)
                {
                    coluna = 5;
                    gvAcessorio.Columns[coluna].HeaderText = "Produto";
                    gvAcessorio.Columns[coluna + 1].HeaderText = "Grupo";
                    gvAcessorio.Columns[coluna + 2].HeaderText = "Nome";
                    coluna = 7;
                    gvAcessorio.Columns[coluna + 1].HeaderText = "Origem";
                    gvAcessorio.Columns[coluna + 2].HeaderText = "Griffe";
                    gvAcessorio.Columns[coluna + 3].HeaderText = "Cor";
                    gvAcessorio.Columns[coluna + 4].HeaderText = "Cor Fornecedor";
                    gvAcessorio.Columns[coluna + 5].HeaderText = "Fornecedor";
                    gvAcessorio.Columns[coluna + 6].HeaderText = "Preço";
                    gvAcessorio.Columns[coluna + 7].HeaderText = "Custo";
                    gvAcessorio.Columns[coluna + 8].HeaderText = "Quantidade";
                    gvAcessorio.Columns[coluna + 9].HeaderText = "Previsão Entrega";
                    gvAcessorio.Columns[coluna + 10].HeaderText = "Descrição Sugerida";
                    gvAcessorio.Columns[coluna + 11].HeaderText = "Referência Fabricante";
                    gvAcessorio.Columns[coluna + 12].HeaderText = "Pedido";

                }

                gvAcessorio.DataSource = g_AcessorioFiltro;
                gvAcessorio.DataBind();

                PreencherValorFiltro(g_AcessorioFiltro);
            }
            else
            {
                RecarregarAcessorio();
            }
        }
        private void PreencherValorFiltro(List<SP_OBTER_DESENV_COLECAO_ACESSResult> lstAcessorios)
        {
            try
            {
                int totalModelo = 0;
                int totalEstilo = 0;

                var estilo = lstAcessorios.Where(p => p.PRODUTO != null && p.PRODUTO.Trim() != "" && p.PRODUTO.Trim().Length >= 4 && p.STATUS == 'A');
                if (estilo != null)
                    totalEstilo = estilo.Count();

                var modelo = estilo.Select(j => j.PRODUTO).Distinct();
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
        private void CarregarProdutoFiltro()
        {
            List<DESENV_ACESSORIO> produtoFiltro = new List<DESENV_ACESSORIO>();

            var _produto = g_AcessorioFiltro.Select(s => s.PRODUTO.Trim()).Distinct().ToList();
            foreach (var item in _produto)
                if (item.Trim() != "")
                    produtoFiltro.Add(new DESENV_ACESSORIO { PRODUTO = item.Trim() });

            produtoFiltro = produtoFiltro.OrderBy(p => p.PRODUTO).ToList();
            produtoFiltro.Insert(0, new DESENV_ACESSORIO { PRODUTO = "" });
            ddlProdutoFiltro.DataSource = produtoFiltro;
            ddlProdutoFiltro.DataBind();
        }
        private void CarregarGriffeFiltro()
        {
            List<DESENV_PRODUTO> griffeFiltro = new List<DESENV_PRODUTO>();

            var _griffe = g_AcessorioFiltro.Where(p => p.GRIFFE != null).Select(s => s.GRIFFE.Trim()).Distinct().ToList();
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
            List<SP_OBTER_DESENV_COLECAO_ACESSResult> nomeFiltro = new List<SP_OBTER_DESENV_COLECAO_ACESSResult>();

            var _nome = g_AcessorioFiltro.Select(s => s.NOME.Trim()).Distinct().ToList();
            foreach (var item in _nome)
                if (item.Trim() != "")
                    nomeFiltro.Add(new SP_OBTER_DESENV_COLECAO_ACESSResult { NOME = item.Trim() });

            nomeFiltro = nomeFiltro.OrderBy(p => p.NOME).ToList();
            nomeFiltro.Insert(0, new SP_OBTER_DESENV_COLECAO_ACESSResult { NOME = "" });
            nomeFiltro.Insert(1, new SP_OBTER_DESENV_COLECAO_ACESSResult { NOME = "VAZIO" });
            ddlNomeFiltro.DataSource = nomeFiltro;
            ddlNomeFiltro.DataBind();
        }
        private void CarregarOrigemFiltro()
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = desenvController.ObterProdutoOrigem(ddlColecoes.SelectedValue);
            if (_origem != null)
            {
                _origem = _origem.Where(i => g_AcessorioFiltro.Any(g => g.DESENV_PRODUTO_ORIGEM == i.CODIGO)).ToList();
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigemFiltro.DataSource = _origem;
                ddlOrigemFiltro.DataBind();
            }
        }
        private void CarregarGrupoFiltro()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("02");
            if (_grupo != null)
            {
                _grupo = _grupo.Where(i => g_AcessorioFiltro.Any(g => g.GRUPO.Trim() == i.GRUPO_PRODUTO.Trim())).ToList();
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                _grupo.Insert(1, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "VAZIO" });
                ddlGrupoFiltro.DataSource = _grupo;
                ddlGrupoFiltro.DataBind();
            }
        }
        private void CarregarCorFiltro()
        {

            List<CORES_BASICA> _coresBasicas = new List<CORES_BASICA>();

            var _cor = g_AcessorioFiltro.Where(p => p.COR != null).Select(s => s.COR.Trim()).Distinct().ToList();
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
            List<PROD_FORNECEDOR> _fornecedor = prodController.ObterFornecedor().Where(p => p.STATUS == 'A' && p.TIPO == 'C').ToList();
            if (_fornecedor != null)
            {
                _fornecedor = _fornecedor.Where(i => g_AcessorioFiltro.Any(g => g.FORNECEDOR.Trim() == i.FORNECEDOR.Trim())).ToList();
                _fornecedor.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "" });
                ddlFornecedorFiltro.DataSource = _fornecedor;
                ddlFornecedorFiltro.DataBind();
            }
        }
        private void CarregarCorFornecedorFiltro()
        {
            List<DESENV_ACESSORIO> corFornecedorFiltro = new List<DESENV_ACESSORIO>();

            var _corFornecedor = g_AcessorioFiltro.Where(p => p.COR_FORNECEDOR != null).Select(s => s.COR_FORNECEDOR.Trim()).Distinct().ToList();
            foreach (var item in _corFornecedor)
                if (item.Trim() != "")
                    corFornecedorFiltro.Add(new DESENV_ACESSORIO { COR_FORNECEDOR = item.Trim() });

            corFornecedorFiltro = corFornecedorFiltro.OrderBy(p => p.COR_FORNECEDOR).ToList();
            corFornecedorFiltro.Insert(0, new DESENV_ACESSORIO { COR_FORNECEDOR = "" });
            corFornecedorFiltro.Insert(1, new DESENV_ACESSORIO { COR_FORNECEDOR = "VAZIO" });
            ddlCorFornecedorFiltro.DataSource = corFornecedorFiltro;
            ddlCorFornecedorFiltro.DataBind();
        }
        private void CarregarPedidoFiltro()
        {
            List<DESENV_ACESSORIO_PEDIDO> _pedido = desenvController.ObterAcessorioPedido();
            if (_pedido != null)
            {
                _pedido.Insert(0, new DESENV_ACESSORIO_PEDIDO { PEDIDO = "" });
                ddlPedidoFiltro.DataSource = _pedido;
                ddlPedidoFiltro.DataBind();
            }
        }
        private void CarregarPrecoFiltro()
        {
            List<DESENV_ACESSORIO> preco = new List<DESENV_ACESSORIO>();

            var _preco = g_AcessorioFiltro.Select(s => s.PRECO).Distinct().ToList();
            foreach (var item in _preco)
                if (item != 0)
                    preco.Add(new DESENV_ACESSORIO { PRECO = item });

            preco = preco.OrderBy(p => p.PRECO).ToList();
            preco.Insert(0, new DESENV_ACESSORIO { PRECO = 0 });
            ddlPrecoFiltro.DataSource = preco;
            ddlPrecoFiltro.DataBind();
        }
        private void CarregarCustoFiltro()
        {
            List<DESENV_ACESSORIO> custo = new List<DESENV_ACESSORIO>();

            var _custo = g_AcessorioFiltro.Select(s => s.CUSTO).Distinct().ToList();
            foreach (var item in _custo)
                if (item != 0)
                    custo.Add(new DESENV_ACESSORIO { CUSTO = item });

            custo = custo.OrderBy(p => p.CUSTO).ToList();
            custo.Insert(0, new DESENV_ACESSORIO { CUSTO = 0 });
            ddlCustoFiltro.DataSource = custo;
            ddlCustoFiltro.DataBind();
        }
        private void CarregarQtdeFiltro()
        {
            List<DESENV_ACESSORIO> quantidade = new List<DESENV_ACESSORIO>();

            var _quantidade = g_AcessorioFiltro.Where(p => p.QTDE != null).Select(s => s.QTDE).Distinct().ToList();
            foreach (var item in _quantidade)
                if (item != 0)
                    quantidade.Add(new DESENV_ACESSORIO { QTDE = item });

            quantidade = quantidade.OrderBy(p => p.QTDE).ToList();
            quantidade.Insert(0, new DESENV_ACESSORIO { QTDE = 0 });
            ddlQuantidadeFiltro.DataSource = quantidade;
            ddlQuantidadeFiltro.DataBind();
        }
        private void CarregarDescricaoSugeridaFiltro()
        {
            List<DESENV_ACESSORIO> descricaoSugerida = new List<DESENV_ACESSORIO>();

            var _descSug = g_AcessorioFiltro.Where(p => p.DESCRICAO_SUGERIDA != null).Select(s => s.DESCRICAO_SUGERIDA.Trim()).Distinct().ToList();
            foreach (var item in _descSug)
                if (item.Trim() != "")
                    descricaoSugerida.Add(new DESENV_ACESSORIO { DESCRICAO_SUGERIDA = item.Trim() });

            descricaoSugerida = descricaoSugerida.OrderBy(p => p.DESCRICAO_SUGERIDA).ToList();
            descricaoSugerida.Insert(0, new DESENV_ACESSORIO { DESCRICAO_SUGERIDA = "" });
            descricaoSugerida.Insert(1, new DESENV_ACESSORIO { DESCRICAO_SUGERIDA = "VAZIO" });
            ddlDescricaoSugeridaFiltro.DataSource = descricaoSugerida;
            ddlDescricaoSugeridaFiltro.DataBind();
        }
        private void CarregarReferenciaFabricanteFiltro()
        {
            List<DESENV_ACESSORIO> referFabricante = new List<DESENV_ACESSORIO>();

            var _refFab = g_AcessorioFiltro.Where(p => p.REFER_FABRICANTE != null).Select(s => s.REFER_FABRICANTE.Trim()).Distinct().ToList();
            foreach (var item in _refFab)
                if (item.Trim() != "")
                    referFabricante.Add(new DESENV_ACESSORIO { REFER_FABRICANTE = item.Trim() });

            referFabricante = referFabricante.OrderBy(p => p.REFER_FABRICANTE).ToList();
            referFabricante.Insert(0, new DESENV_ACESSORIO { REFER_FABRICANTE = "" });
            referFabricante.Insert(1, new DESENV_ACESSORIO { REFER_FABRICANTE = "VAZIO" });
            ddlReferenciaFabricanteFiltro.DataSource = referFabricante;
            ddlReferenciaFabricanteFiltro.DataBind();
        }
        private void CarregarDataPrevisaoEntregaFiltro()
        {
            List<DESENV_ACESSORIO> dataPrevisaoEntrega = new List<DESENV_ACESSORIO>();

            var _dataPrev = g_AcessorioFiltro.Where(p => p.DATA_PREVISAO_ENTREGA != null).Select(s => s.DATA_PREVISAO_ENTREGA.Trim()).Distinct().ToList();
            foreach (var item in _dataPrev)
                if (item.Trim() != "")
                    dataPrevisaoEntrega.Add(new DESENV_ACESSORIO { DATA_PREVISAO_ENTREGA = item.Trim() });

            dataPrevisaoEntrega = dataPrevisaoEntrega.OrderBy(p => p.DATA_PREVISAO_ENTREGA).ToList();
            dataPrevisaoEntrega.Insert(0, new DESENV_ACESSORIO { DATA_PREVISAO_ENTREGA = "" });
            dataPrevisaoEntrega.Insert(1, new DESENV_ACESSORIO { DATA_PREVISAO_ENTREGA = "VAZIO" });
            ddlDataPrevisaoEntregaFiltro.DataSource = dataPrevisaoEntrega;
            ddlDataPrevisaoEntregaFiltro.DataBind();
        }
        private void CarregarFiltros()
        {
            CarregarProdutoFiltro();
            CarregarGriffeFiltro();
            CarregarNomeFiltro();
            CarregarOrigemFiltro();
            CarregarGrupoFiltro();
            CarregarCorFiltro();
            CarregarFornecedorFiltro();
            CarregarCorFornecedorFiltro();
            CarregarPedidoFiltro();
            CarregarPrecoFiltro();
            CarregarCustoFiltro();
            CarregarQtdeFiltro();
            CarregarDescricaoSugeridaFiltro();
            CarregarReferenciaFabricanteFiltro();
            CarregarDataPrevisaoEntregaFiltro();
        }
        #endregion

        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                CarregarOrigem(colecao);

                btLimpar_Click(null, null);
                pnlTripa.Visible = true;

                RecarregarAcessorio();
                FiltroGeral(null, true, false);

                //Carregar filtros do GRID
                CarregarFiltros();
            }
        }
        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            RecarregarAcessorio();
            FiltroGeral(null, true, false);
            CarregarFiltros();
        }

        protected void btObterSeqProduto_Click(object sender, ImageClickEventArgs e)
        {
            labSalvar.Text = "";

            var acessorioSeq = desenvController.ObterAcessorioSequencia();
            if (acessorioSeq != null)
            {
                acessorioSeq.DATA_UTILIZACAO = DateTime.Now;
                desenvController.AtualizarAcessorioSequencia(acessorioSeq);
                var produtoLinxExiste = baseController.BuscaProduto(acessorioSeq.PRODUTO.ToString());
                if (produtoLinxExiste != null)
                {
                    btObterSeqProduto_Click(btObterSeqProduto, null);
                }
                else
                {
                    txtProduto.Text = acessorioSeq.PRODUTO.ToString();
                }
            }
            else
            {
                txtProduto.Text = "";
                labSalvar.Text = "Não existe Sequência cadastrada. Entre em contato com TI.";
            }
        }
    }
}
