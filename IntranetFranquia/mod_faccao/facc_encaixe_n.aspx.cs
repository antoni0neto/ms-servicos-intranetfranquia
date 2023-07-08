using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class facc_encaixe_n : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");

                string tela = Request.QueryString["a"].ToString();
                if (tela == "1")
                {
                    hidTela.Value = "20";
                    labTitulo.Text = "Encaixe Lote";
                    labMeioTitulo.Text = "Controle de Facção";
                    labSubTitulo.Text = "Encaixe Lote";
                }
                else
                {
                    hidTela.Value = "21";
                    labTitulo.Text = "Encaixe Lote Acabamento";
                    labMeioTitulo.Text = "Controle de Facção Acabamento";
                    labSubTitulo.Text = "Encaixe Lote Acabamento";

                    ddlServico.SelectedValue = "4";
                    ddlServico.Enabled = false;
                }


                USUARIO usuario = new USUARIO();
                usuario = (USUARIO)Session["USUARIO"];
                hidCodigoUsuario.Value = usuario.CODIGO_USUARIO.ToString();

                CarregarColecoes();
                CarregarFornecedores(Convert.ToInt32(hidTela.Value));
                CarregarServicosProducao(Convert.ToInt32(hidTela.Value));
                CarregarEncaixeLote(usuario.CODIGO_USUARIO);
            }

            //Evitar duplo clique no botão
            btInserir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btInserir, null) + ";");
            btEncaixar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEncaixar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();
            }
        }
        private void CarregarFornecedores(int tela)
        {

            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'S') || p.STATUS == 'S').ToList();

                if (tela == 21)
                    _fornecedores = _fornecedores.Where(p => p.CODIGO == 106 || p.CODIGO == 44).ToList();

                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });
                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }

        }
        private void CarregarServicosProducao(int tela)
        {
            List<PROD_SERVICO> _servico = new List<PROD_SERVICO>();
            _servico = prodController.ObterServicoProducao().Where(p => p.STATUS == 'A' && p.CODIGO != 5).ToList();

            if (tela == 20)
                _servico = _servico.Where(p => p.CODIGO != 4).ToList();

            if (_servico != null)
            {
                _servico.Insert(0, new PROD_SERVICO { DESCRICAO = "Selecione", STATUS = 'A' });
                ddlServico.DataSource = _servico;
                ddlServico.DataBind();
            }
        }
        protected void ddlFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFornecedor.SelectedValue.Trim() == "CD - LUGZY" || ddlFornecedor.SelectedValue.Trim() == "HANDBOOK")
            {
                ddlTipo.SelectedValue = "I";
                ddlTipo.Enabled = false;
            }
            else
            {
                ddlTipo.SelectedValue = "E";
                ddlTipo.Enabled = true;
            }
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFornecedor.ForeColor = _OK;
            if (ddlFornecedor.SelectedValue.Trim() == "Selecione")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labTipo.ForeColor = _OK;
            if (ddlTipo.SelectedValue.Trim() == "")
            {
                labTipo.ForeColor = _notOK;
                retorno = false;
            }

            labServico.ForeColor = _OK;
            if (ddlServico.SelectedValue.Trim() == "0")
            {
                labServico.ForeColor = _notOK;
                retorno = false;
            }

            labPrecoCusto.ForeColor = _OK;
            if (txtPrecoCusto.Text.Trim() == "")
            {
                labPrecoCusto.ForeColor = _notOK;
                retorno = false;
            }

            labPrecoProducao.ForeColor = _OK;
            if (txtPrecoProducao.Text.Trim() == "")
            {
                labPrecoProducao.ForeColor = _notOK;
                retorno = false;
            }

            labVolume.ForeColor = _OK;
            if (txtVolume.Text.Trim() == "")
            {
                labVolume.ForeColor = _notOK;
                retorno = false;
            }

            labTituloLote.ForeColor = _OK;
            if (gvPrincipal.Rows.Count <= 0)
            {
                labTituloLote.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        #region "PRINCIPAL"
        private void CarregarEncaixeLote(int codigoUsuario)
        {
            List<PROD_HB_SAIDA_LOTE> saidaLote = new List<PROD_HB_SAIDA_LOTE>();
            saidaLote.AddRange(faccController.ObterListaSaidaEncaixeLote(Convert.ToInt32(hidTela.Value), codigoUsuario));

            gvPrincipal.DataSource = saidaLote;
            gvPrincipal.DataBind();
        }
        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_SAIDA_LOTE _saidaLote = e.Row.DataItem as PROD_HB_SAIDA_LOTE;

                    if (_saidaLote != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(_saidaLote.PROD_HB_SAIDA1.PROD_HB1.COLECAO)).DESC_COLECAO;

                        Literal _litHB = e.Row.FindControl("litHB") as Literal;
                        if (_litHB != null)
                            _litHB.Text = _saidaLote.PROD_HB_SAIDA1.PROD_HB1.HB.ToString();

                        Literal _litProduto = e.Row.FindControl("litProduto") as Literal;
                        if (_litProduto != null)
                            _litProduto.Text = _saidaLote.PROD_HB_SAIDA1.PROD_HB1.CODIGO_PRODUTO_LINX.ToString();

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                            _litNome.Text = _saidaLote.PROD_HB_SAIDA1.PROD_HB1.NOME;

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(_saidaLote.PROD_HB_SAIDA1.PROD_HB1.COR).DESC_COR.Trim();

                        Literal _qtde = e.Row.FindControl("litQtde") as Literal;
                        if (_qtde != null)
                            _qtde.Text = (Convert.ToInt32(_saidaLote.PROD_HB_SAIDA1.GRADE_EXP) + Convert.ToInt32(_saidaLote.PROD_HB_SAIDA1.GRADE_XP) + Convert.ToInt32(_saidaLote.PROD_HB_SAIDA1.GRADE_PP) + Convert.ToInt32(_saidaLote.PROD_HB_SAIDA1.GRADE_P) + Convert.ToInt32(_saidaLote.PROD_HB_SAIDA1.GRADE_M) + Convert.ToInt32(_saidaLote.PROD_HB_SAIDA1.GRADE_G) + Convert.ToInt32(_saidaLote.PROD_HB_SAIDA1.GRADE_GG)).ToString();

                        Literal _litMostruario = e.Row.FindControl("litMostruario") as Literal;
                        if (_litMostruario != null)
                            _litMostruario.Text = (_saidaLote.PROD_HB_SAIDA1.PROD_HB1.MOSTRUARIO == 'S') ? "Sim" : "Não";
                    }
                }
            }
        }
        protected void btInserir_Click(object sender, EventArgs e)
        {
            List<PROD_HB_SAIDA> lstSaida = new List<PROD_HB_SAIDA>();
            try
            {
                labErro.Text = "";

                if (ddlColecoes.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                if (txtHB.Text.Trim() == "")
                {
                    labErro.Text = "Informe o HB.";
                    return;
                }

                //BUSCAR PROD_HB_SAIDA PARA ENCAIXE...
                lstSaida.AddRange(faccController.ObterListaSaidaEncaixe(Convert.ToInt32(hidTela.Value)).Where(p =>
                    p.PROD_HB1.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim() &&
                    p.PROD_HB1.HB == Convert.ToInt32(txtHB.Text.Trim()) &&
                    p.PROD_HB1.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N') &&
                    p.PROD_HB1.CODIGO_PAI == null
                    ).ToList());

                //VALIDA SE EXISTE
                if (lstSaida == null || lstSaida.Count() <= 0)
                {
                    labErro.Text = "HB não encontrado para Encaixe.";
                    return;
                }

                foreach (var s in faccController.ObterListaSaidaEncaixeLote(Convert.ToInt32(hidTela.Value), null))
                {
                    if (lstSaida.Where(p => p.CODIGO == s.PROD_HB_SAIDA).ToList().Count() > 0)
                    {
                        labErro.Text = "HB já foi inserido. Se NÃO aparecer no quadro abaixo, foi inserido no lote de outro Usuário.";
                        return;
                    }
                }

                //INSERE NA TABELA NOVA POR USUARIO
                int seqSaidaLote = faccController.ObterNumeroLote(Convert.ToInt32(hidCodigoUsuario.Value), Convert.ToInt32(hidTela.Value));
                PROD_HB_SAIDA_LOTE saidaLote = null;
                foreach (var s in lstSaida)
                {
                    saidaLote = new PROD_HB_SAIDA_LOTE();
                    saidaLote.LOTE = seqSaidaLote;
                    saidaLote.PROD_HB_SAIDA = s.CODIGO;
                    saidaLote.USUARIO_LOTE = Convert.ToInt32(hidCodigoUsuario.Value);
                    saidaLote.DATA_INCLUSAO = DateTime.Now;
                    faccController.InserirSaidaHBLote(saidaLote);
                }

                CarregarEncaixeLote(Convert.ToInt32(hidCodigoUsuario.Value));
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    int codigoSaidaLote = 0;
                    int tela = Convert.ToInt32(hidTela.Value);

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        codigoSaidaLote = Convert.ToInt32(gvPrincipal.DataKeys[row.RowIndex].Value.ToString());

                        faccController.ExcluirSaidaHBLote(codigoSaidaLote);
                        CarregarEncaixeLote(Convert.ToInt32(hidCodigoUsuario.Value));
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        #endregion

        protected void btEncaixar_Click(object sender, EventArgs e)
        {
            int codigoUsuario = 0;

            try
            {
                labErroEnxaixe.Text = "";

                if (!ValidarCampos())
                {
                    labErroEnxaixe.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                //LOOP PARA ENCAIXAR OS HBS
                int codigoSaidaLote = 0;
                PROD_HB_SAIDA _saida = null;
                PROD_HB_SAIDA_LOTE _saidaLote = null;
                List<PROD_HB_SAIDA> _lstSaidaEmail = new List<PROD_HB_SAIDA>();
                foreach (GridViewRow row in gvPrincipal.Rows)
                {
                    codigoSaidaLote = Convert.ToInt32(gvPrincipal.DataKeys[row.RowIndex].Value);

                    //OBTER REGISTRO DO SAIDA LOTE PARA BAIXA
                    _saidaLote = faccController.ObterSaidaHBLote(codigoSaidaLote);
                    if (_saidaLote != null)
                    {
                        _saidaLote.DATA_ENCAIXE = DateTime.Now;
                        faccController.AtualizarSaidaHBLote(_saidaLote);
                    }

                    _saida = faccController.ObterSaidaHB(_saidaLote.PROD_HB_SAIDA);

                    _saida.CNPJ = new BaseController().ObterCadastroCLIFOR(ddlFornecedor.SelectedValue).CGC_CPF;
                    _saida.FORNECEDOR = ddlFornecedor.SelectedValue;
                    _saida.TIPO = Convert.ToChar(ddlTipo.SelectedValue);
                    _saida.PROD_SERVICO = Convert.ToInt32(ddlServico.SelectedValue);
                    _saida.CUSTO = Convert.ToDecimal(txtPrecoCusto.Text);
                    _saida.PRECO = Convert.ToDecimal(txtPrecoProducao.Text);
                    _saida.VOLUME = Convert.ToInt32(txtVolume.Text);
                    _saida.DATA_LIBERACAO = DateTime.Now;
                    _saida.USUARIO_LIBERACAO = codigoUsuario;

                    if (_saida.TIPO == 'I')
                    {
                        _saida.CODIGO_FILIAL = "";
                        _saida.NF_SAIDA = "";
                        _saida.SERIE_NF = "";
                        _saida.EMISSAO = DateTime.Now;
                        _saida.USUARIO_EMISSAO = codigoUsuario;

                        PROD_HB_ENTRADA _entrada = new PROD_HB_ENTRADA();
                        _entrada.PROD_HB_SAIDA = _saida.CODIGO;
                        _entrada.GRADE_EXP = 0;
                        _entrada.GRADE_XP = 0;
                        _entrada.GRADE_PP = 0;
                        _entrada.GRADE_P = 0;
                        _entrada.GRADE_M = 0;
                        _entrada.GRADE_G = 0;
                        _entrada.GRADE_GG = 0;
                        _entrada.GRADE_TOTAL = 0;
                        _entrada.STATUS = 'A';
                        _entrada.DATA_INCLUSAO = DateTime.Now;
                        faccController.InserirEntradaHB(_entrada);

                    }

                    faccController.AtualizarSaidaHB(_saida);

                    //adiciona numa lista auxiliar para enviar o email
                    _lstSaidaEmail.Add(_saida);

                    /*INSERE VALORES DOS SERVICOS PARA CUSTO*/
                    //Verifica preco aprovado
                    if (!ProdutoFoiSimulado(_saida.PROD_HB1.CODIGO_PRODUTO_LINX, Convert.ToChar(_saida.PROD_HB1.MOSTRUARIO), 'S'))
                    {
                        bool _inserir = false;
                        PROD_HB_CUSTO_SERVICO _servico = null;
                        _servico = prodController.ObterCustoServicoConexao().Where(p =>
                                                                                p.PRODUTO.Trim() == _saida.PROD_HB1.CODIGO_PRODUTO_LINX.Trim() &&
                                                                                p.MOSTRUARIO == _saida.PROD_HB1.MOSTRUARIO &&
                                                                                p.SERVICO == _saida.PROD_SERVICO).SingleOrDefault();
                        if (_servico == null)
                        {
                            _inserir = true;
                            _servico = new PROD_HB_CUSTO_SERVICO();
                        }
                        _servico.PRODUTO = _saida.PROD_HB1.CODIGO_PRODUTO_LINX;
                        _servico.FORNECEDOR = ddlFornecedor.SelectedValue;
                        _servico.SERVICO = Convert.ToInt32(ddlServico.SelectedValue);
                        _servico.DATA_INCLUSAO = DateTime.Now;
                        _servico.USUARIO_INCLUSAO = codigoUsuario;
                        _servico.MOSTRUARIO = Convert.ToChar(_saida.PROD_HB1.MOSTRUARIO);

                        if (_servico != null && _servico.CUSTO < Convert.ToDecimal(txtPrecoCusto.Text))
                            _servico.CUSTO = Convert.ToDecimal(txtPrecoCusto.Text);
                        if (_servico != null && _servico.CUSTO_PECA < Convert.ToDecimal(txtPrecoCusto.Text))
                            _servico.CUSTO_PECA = Convert.ToDecimal(txtPrecoProducao.Text);

                        if (_inserir)
                            prodController.InserirCustoServico(_servico);
                        else
                            prodController.AtualizarCustoServico(_servico);

                    }
                }

                CarregarEncaixeLote(codigoUsuario);

                //ENVIAR EMAIL
                if (Constante.enviarEmail)
                    EnviarEmail(_lstSaidaEmail, Convert.ToInt32(_saidaLote.LOTE), Convert.ToInt32(hidTela.Value));

                if (ddlTipo.SelectedValue == "I")
                    labErroEnxaixe.Text = "Enviado com sucesso para Entrada de Produtos.";
                else
                    labErroEnxaixe.Text = "Enviado com sucesso para Emissão da Nota Fiscal.";

                ddlFornecedor.SelectedValue = "Selecione";
                ddlTipo.SelectedValue = "";
                ddlServico.SelectedValue = "0";
                txtPrecoCusto.Text = "";
                txtPrecoProducao.Text = "";
                txtVolume.Text = "";

            }
            catch (Exception ex)
            {
                labErroEnxaixe.Text = ex.Message;
            }
        }

        #region "EMAIL"
        private void EnviarEmail(List<PROD_HB_SAIDA> _lstSaidaEmail, int lote, int tela)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            string assunto = "";
            string titulo = "";
            string subTitulo = "";
            if (_lstSaidaEmail[0].TIPO == 'I')
                titulo = "INTERNO ";
            else
                titulo = "EMISSÃO NF ";

            if (tela == 21)
                subTitulo = "ACABAMENTO ";

            assunto = "Intranet: " + titulo + "" + subTitulo + "- " + "LOTE: " + lote.ToString();
            email.ASSUNTO = assunto;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(_lstSaidaEmail, assunto, lote, tela);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails 
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(7, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(List<PROD_HB_SAIDA> _lstSaidaEmail, string assunto, int lote, int tela)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            //var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(_saida.PROD_HB1.PROD_GRADE), Convert.ToInt32(hidConexao.Value));

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Emissão NF Saída - HB</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divSaidaLote' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 650pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <h2> " + assunto.Replace("Intranet:", "") + "</h2>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    Lote");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + lote.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string fase = "";
            var faseLinx = faccController.ObterFase(_lstSaidaEmail[0].PROD_SERVICO1.FASE_LINX);
            if (faseLinx != null)
                fase = faseLinx.FASE_PRODUCAO.Trim() + " - " + faseLinx.DESC_FASE_PRODUCAO.Trim();
            else
                fase = _lstSaidaEmail[0].PROD_SERVICO1.DESCRICAO + " (Fase não encontrada no LINX)";

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Fase:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + fase);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Setor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ((_lstSaidaEmail[0].TIPO == 'I') ? "01 - INTERNO" : "02 - EXTERNO"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string fornecedor = "";
            var f = faccController.ObterRecursoProdutivo(_lstSaidaEmail[0].FORNECEDOR);

            if (f != null)
                fornecedor = f.RECURSO_PRODUTIVO.Trim() + " - " + f.DESC_RECURSO.Trim();
            else
                fornecedor = _lstSaidaEmail[0].FORNECEDOR + " " + "(Recurso NÃO cadastrado no LINX)";

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Recurso Produtivo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + fornecedor);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string cpfFormatado = _lstSaidaEmail[0].CNPJ.Trim();
            if (cpfFormatado.Length == 11)
                cpfFormatado = Convert.ToUInt64(cpfFormatado).ToString(@"000\.000\.000\-00");
            else
                cpfFormatado = Convert.ToUInt64(cpfFormatado).ToString(@"00\.000\.000\/0000\-00");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    CNPJ:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + cpfFormatado);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço Custo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$ " + Convert.ToDecimal(_lstSaidaEmail[0].CUSTO).ToString("###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço Produção:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$ " + Convert.ToDecimal(_lstSaidaEmail[0].PRECO).ToString("###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Volume:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _lstSaidaEmail[0].VOLUME);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");

            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        <table border='1' cellpadding='2' cellspacing='0' style='width: 100%; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='background-color: #FFDAB9'>");
            sb.Append("                                <td style='width:85px;'>");
            sb.Append("                                    O.P.");
            sb.Append("                                </td>");
            sb.Append("                                <td style='width:85px;'>");
            sb.Append("                                    O.C.");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    Coleção");
            sb.Append("                                </td>");
            sb.Append("                                <td style='width:85px;'>");
            sb.Append("                                    HB");
            sb.Append("                                </td>");
            sb.Append("                                <td style='width:95px;'>");
            sb.Append("                                    Produto");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    Nome");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor");
            sb.Append("                                </td>");
            sb.Append("                                <td style='width:90px; text-align:center;'>");
            sb.Append("                                    Quantidade");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            foreach (var s in _lstSaidaEmail)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td style='text-align:center;'>");
                sb.Append("                                    " + s.PROD_HB1.ORDEM_PRODUCAO);
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align:center;'>");
                sb.Append("                                    " + s.PROD_HB1.COLECAO.Trim() + "" + s.PROD_HB1.HB.ToString() + "" + s.PROD_HB1.MOSTRUARIO.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + new BaseController().BuscaColecaoAtual(s.PROD_HB1.COLECAO).DESC_COLECAO.Trim());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align:center;'>");
                sb.Append("                                    " + s.PROD_HB1.HB.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align:center;'>");
                sb.Append("                                    " + s.PROD_HB1.CODIGO_PRODUTO_LINX);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + s.PROD_HB1.NOME);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + prodController.ObterCoresBasicas(s.PROD_HB1.COR).DESC_COR.Trim());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align:center;'>");
                sb.Append("                                    " + s.GRADE_TOTAL.ToString());
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");

            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
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

        private bool ProdutoFoiSimulado(string produto, char mostruario, char finalizado)
        {

            if (prodController.ObterCustoSimulacao().Where(p => p.PRODUTO.Trim() == produto &&
                                                            p.MOSTRUARIO == mostruario &&
                //p.SIMULACAO == 'N' &&
                                                            p.CUSTO_MOSTRUARIO == 'N' &&
                                                            p.FINALIZADO == finalizado).Count() > 0)
                return true;


            return false;
        }
    }
}


