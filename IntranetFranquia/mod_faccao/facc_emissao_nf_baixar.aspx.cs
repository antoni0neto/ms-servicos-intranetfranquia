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
    public partial class facc_emissao_nf_baixar : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtEmissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {

                int codigoSaida = 0;
                int tela = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Request.QueryString["t"] == null || Request.QueryString["t"] == "" ||
                    Session["USUARIO"] == null)
                    Response.Redirect("facc_menu.aspx");

                codigoSaida = Convert.ToInt32(Request.QueryString["p"].ToString());
                tela = Convert.ToInt32(Request.QueryString["t"].ToString());

                PROD_HB_SAIDA _saida = faccController.ObterSaidaHB(codigoSaida);
                if (_saida == null)
                    Response.Redirect("facc_menu.aspx");

                if (_saida.DATA_LIBERACAO != null && _saida.EMISSAO != null)
                {
                    Response.Write("NOTA FISCAL DESTE PRODUTO JÁ SALVA.");
                    Response.End();
                }

                CarregarFornecedores();
                CarregarServicosProducao();
                hidCodigoSaida.Value = codigoSaida.ToString();
                hidTela.Value = tela.ToString();

                txtColecao.Text = new BaseController().BuscaColecaoAtual(_saida.PROD_HB1.COLECAO).DESC_COLECAO.Trim();
                txtHB.Text = _saida.PROD_HB1.HB.ToString();
                txtProduto.Text = _saida.PROD_HB1.CODIGO_PRODUTO_LINX;
                txtNome.Text = _saida.PROD_HB1.NOME;
                txtCor.Text = prodController.ObterCoresBasicas(_saida.PROD_HB1.COR).DESC_COR.Trim();
                txtQtde.Text = _saida.GRADE_TOTAL.ToString();
                txtMostruario.Text = (_saida.PROD_HB1.MOSTRUARIO == 'S') ? "Sim" : "Não";
                ddlFornecedor.SelectedValue = _saida.FORNECEDOR;
                ddlTipo.SelectedValue = _saida.TIPO.ToString();
                ddlServico.SelectedValue = _saida.PROD_SERVICO.ToString();
                txtPrecoCusto.Text = _saida.CUSTO.ToString();
                txtPrecoProducao.Text = _saida.PRECO.ToString();
                txtVolume.Text = _saida.VOLUME.ToString();

                txtGradeEXP_O.Text = _saida.GRADE_EXP.ToString();
                txtGradeXP_O.Text = _saida.GRADE_XP.ToString();
                txtGradePP_O.Text = _saida.GRADE_PP.ToString();
                txtGradeP_O.Text = _saida.GRADE_P.ToString();
                txtGradeM_O.Text = _saida.GRADE_M.ToString();
                txtGradeG_O.Text = _saida.GRADE_G.ToString();
                txtGradeGG_O.Text = _saida.GRADE_GG.ToString();
                txtGradeTotal_O.Text = _saida.GRADE_TOTAL.ToString();

                txtNF.MaxLength = 6;
                ddlFilial.SelectedValue = "000029";

                CarregarNomeGrade(_saida.PROD_HB1);

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFornecedores()
        {

            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'S') || p.STATUS == 'S');
                ddlFornecedor.DataBind();
            }

        }
        private void CarregarServicosProducao()
        {
            List<PROD_SERVICO> _servico = new List<PROD_SERVICO>();
            _servico = prodController.ObterServicoProducao().Where(p => p.STATUS == 'A').ToList();
            if (_servico != null)
            {
                _servico.Insert(0, new PROD_SERVICO { DESCRICAO = "Selecione", STATUS = 'A' });
                ddlServico.DataSource = _servico;
                ddlServico.DataBind();
            }
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue.Trim() == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            labNF.ForeColor = _OK;
            if (txtNF.Text.Trim() == "")
            {
                labNF.ForeColor = _notOK;
                retorno = false;
            }

            labSerie.ForeColor = _OK;
            if (txtSerie.Text.Trim() == "")
            {
                labSerie.ForeColor = _notOK;
                retorno = false;
            }

            labEmissao.ForeColor = _OK;
            if (txtEmissao.Text.Trim() == "")
            {
                labEmissao.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }

        private void CarregarNomeGrade(PROD_HB prod_hb)
        {
            var _gradeNome = prodController.ObterGradeNome(Convert.ToInt32(prod_hb.PROD_GRADE));
            if (_gradeNome != null)
            {
                labGradeEXP.Text = _gradeNome.GRADE_EXP;
                labGradeXP.Text = _gradeNome.GRADE_XP;
                labGradePP.Text = _gradeNome.GRADE_PP;
                labGradeP.Text = _gradeNome.GRADE_P;
                labGradeM.Text = _gradeNome.GRADE_M;
                labGradeG.Text = _gradeNome.GRADE_G;
                labGradeGG.Text = _gradeNome.GRADE_GG;
            }
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                PROD_HB_SAIDA _saida = faccController.ObterSaidaHB(Convert.ToInt32(hidCodigoSaida.Value));

                if (_saida == null)
                {
                    labErro.Text = "Nenhuma Saída encontrada. Entre em contato com TI.";
                    return;
                }

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                string nfSaida = "";
                nfSaida = "000000" + txtNF.Text.Trim();
                nfSaida = nfSaida.Substring(nfSaida.Length - 6, 6);

                var nfFat = baseController.ObterNFFaturamento(ddlFilial.SelectedItem.Text, nfSaida, txtSerie.Text, Convert.ToDateTime(txtEmissao.Text));
                if (nfFat == null)
                {
                    labErro.Text = "Nota Fiscal informada não foi encontrada no LINX. Verifique os dados informados.";
                    return;
                }

                _saida.CODIGO_FILIAL = ddlFilial.SelectedValue;

                _saida.NF_SAIDA = nfSaida;
                _saida.SERIE_NF = txtSerie.Text.Trim();
                _saida.EMISSAO = Convert.ToDateTime(txtEmissao.Text.Trim());
                _saida.USUARIO_EMISSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                faccController.AtualizarSaidaHB(_saida);

                var _entradaValida = faccController.ObterEntradaHB(_saida.CODIGO.ToString());
                if (_entradaValida == null || _entradaValida.Count() <= 0)
                {
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

                if (Constante.enviarEmail)
                    EnviarEmail(_saida, Convert.ToInt32(hidTela.Value));

                labErro.Text = "Emissão da Nota Fiscal salva com Sucesso.";

                btSalvar.Enabled = false;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "EMAIL"
        private void EnviarEmail(PROD_HB_SAIDA _saida, int tela)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            string assunto = "";
            string titulo = "";
            string subTitulo = "";
            if (_saida.TIPO == 'I')
                titulo = "INTERNO ";
            else
                titulo = "NOTA FISCAL EMITIDA ";

            if (tela == 21)
                subTitulo = "ACABAMENTO";

            assunto = "Intranet: " + titulo + "" + subTitulo + " - O.P.: " + _saida.PROD_HB1.ORDEM_PRODUCAO;
            email.ASSUNTO = assunto;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(_saida, assunto, tela);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails 
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(8, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(PROD_HB_SAIDA _saida, string assunto, int tela)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(_saida.PROD_HB1.PROD_GRADE));

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>NOTA FISCAL EMITIDA</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divSaida' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
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
            sb.Append("                                    O.P.:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saida.PROD_HB1.ORDEM_PRODUCAO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    O.C.:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saida.PROD_HB1.COLECAO.Trim() + _saida.PROD_HB1.HB.ToString() + _saida.PROD_HB1.MOSTRUARIO.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Coleção:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + new BaseController().BuscaColecaoAtual(_saida.PROD_HB1.COLECAO).DESC_COLECAO.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    HB:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saida.PROD_HB1.HB);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Produto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saida.PROD_HB1.CODIGO_PRODUTO_LINX);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Nome");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saida.PROD_HB1.NOME);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + prodController.ObterCoresBasicas(_saida.PROD_HB1.COR).DESC_COR.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Quantidade:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saida.GRADE_TOTAL.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Mostruário:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ((_saida.PROD_HB1.MOSTRUARIO == 'S') ? "SIM" : "NÃO"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string fase = "";
            var faseLinx = faccController.ObterFase(_saida.PROD_SERVICO1.FASE_LINX);
            if (faseLinx != null)
                fase = faseLinx.FASE_PRODUCAO.Trim() + " - " + faseLinx.DESC_FASE_PRODUCAO.Trim();
            else
                fase = _saida.PROD_SERVICO1.DESCRICAO + " (Fase não encontrada no LINX)";

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
            sb.Append("                                    " + ((_saida.TIPO == 'I') ? "01 - INTERNO" : "02 - EXTERNO"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string fornecedor = "";
            var f = faccController.ObterRecursoProdutivo(_saida.FORNECEDOR);

            if (f != null)
                fornecedor = f.RECURSO_PRODUTIVO.Trim() + " - " + f.DESC_RECURSO.Trim();
            else
                fornecedor = _saida.FORNECEDOR + " " + "(Recurso NÃO cadastrado no LINX)";

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Recurso Produtivo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + fornecedor);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string cpfFormatado = _saida.CNPJ.Trim();
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
            sb.Append("                                    Custo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$ " + Convert.ToDecimal(_saida.CUSTO).ToString("###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$ " + Convert.ToDecimal(_saida.PRECO).ToString("###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Volume:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saida.VOLUME);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Filial:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saida.CODIGO_FILIAL + " - " + (new BaseController().BuscaFilialCodigoInt(Convert.ToInt32(_saida.CODIGO_FILIAL)).FILIAL.Trim()));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Nota Fiscal:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saida.NF_SAIDA);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Série:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saida.SERIE_NF);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Emissão:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + Convert.ToDateTime(_saida.EMISSAO).ToString("dd/MM/yyyy"));
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
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='2'>");
            sb.Append("                                     <table cellpadding='0' cellspacing='0' style='width: 550pt; padding: 0px; color: black;");
            sb.Append("                                         font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
            sb.Append("                                         border: 1px solid #ccc;'>");
            sb.Append("                                         <tr style='background-color: #ccc;'>");
            sb.Append("                                             <td>");
            sb.Append("                                                 &nbsp;");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  &nbsp;");
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");
            sb.Append("                                         <tr>");
            sb.Append("                                             <td>");
            sb.Append("                                                 Grade Enviada");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_saida == null) ? 0 : _saida.GRADE_EXP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_saida == null) ? 0 : _saida.GRADE_XP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_saida == null) ? 0 : _saida.GRADE_PP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_saida == null) ? 0 : _saida.GRADE_P)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_saida == null) ? 0 : _saida.GRADE_M)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_saida == null) ? 0 : _saida.GRADE_G)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_saida == null) ? 0 : _saida.GRADE_GG)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                 " + _saida.GRADE_TOTAL.ToString());
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");
            sb.Append("                                     </table>");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
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

    }
}
