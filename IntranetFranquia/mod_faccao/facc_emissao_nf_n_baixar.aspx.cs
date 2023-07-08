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
    public partial class facc_emissao_nf_n_baixar : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtEmissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {

                int lote = 0;
                int tela = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Request.QueryString["t"] == null || Request.QueryString["t"] == "" ||
                    Session["USUARIO"] == null)
                    Response.Redirect("facc_menu.aspx");

                lote = Convert.ToInt32(Request.QueryString["p"].ToString());
                tela = Convert.ToInt32(Request.QueryString["t"].ToString());

                List<PROD_HB_SAIDA_LOTE> _saidaLote = new List<PROD_HB_SAIDA_LOTE>();
                _saidaLote.AddRange(faccController.ObterListaSaidaEmissaoLote(tela).Where(p => p.LOTE == lote));

                if (_saidaLote == null || _saidaLote.Count() <= 0)
                    Response.Redirect("facc_menu.aspx");

                if (_saidaLote[0].DATA_EMISSAO != null)
                {
                    Response.Write("NOTA FISCAL DESTE LOTE JÁ SALVA.");
                    Response.End();
                }

                CarregarFornecedores();
                CarregarServicosProducao();
                hidLote.Value = lote.ToString();
                hidTela.Value = tela.ToString();

                txtLote.Text = _saidaLote[0].LOTE.ToString();
                ddlFornecedor.SelectedValue = _saidaLote[0].PROD_HB_SAIDA1.FORNECEDOR;
                ddlTipo.SelectedValue = _saidaLote[0].PROD_HB_SAIDA1.TIPO.ToString();
                ddlServico.SelectedValue = _saidaLote[0].PROD_HB_SAIDA1.PROD_SERVICO.ToString();
                txtPrecoCusto.Text = _saidaLote[0].PROD_HB_SAIDA1.CUSTO.ToString();
                txtPrecoProducao.Text = _saidaLote[0].PROD_HB_SAIDA1.PRECO.ToString();
                txtVolume.Text = _saidaLote[0].PROD_HB_SAIDA1.VOLUME.ToString();

                if (_saidaLote[0].PROD_HB_SAIDA1.PROD_HB1.COLECAO.Trim().Length == 2)
                {
                    txtNF.MaxLength = 6;
                    ddlFilial.SelectedValue = "000029";
                }
                else
                {
                    txtNF.MaxLength = 9;
                    ddlFilial.SelectedValue = "8800  ";
                }

                gvPrincipal.DataSource = _saidaLote;
                gvPrincipal.DataBind();

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
        #endregion

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

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(_saidaLote.PROD_HB_SAIDA1.PROD_HB1.COR).DESC_COR.Trim();

                        Literal _litLiberado = e.Row.FindControl("litLiberado") as Literal;
                        if (_litLiberado != null)
                            _litLiberado.Text = Convert.ToDateTime(_saidaLote.PROD_HB_SAIDA1.DATA_LIBERACAO).ToString("dd/MM/yyyy");
                    }
                }
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                string nfSaida = "";

                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                nfSaida = "000000" + txtNF.Text.Trim();
                nfSaida = nfSaida.Substring(nfSaida.Length - 6, 6);


                var nfFat = baseController.ObterNFFaturamento(ddlFilial.SelectedItem.Text, nfSaida, txtSerie.Text, Convert.ToDateTime(txtEmissao.Text));
                if (nfFat == null)
                {
                    labErro.Text = "Nota Fiscal informada não foi encontrada no LINX. Verifique os dados informados.";
                    return;
                }


                int codigoSaidaLote = 0;
                string colecao = "";
                PROD_HB_SAIDA _saida = null;
                PROD_HB_SAIDA_LOTE _saidaLote = null;
                PROD_HB_ENTRADA _entrada = null;
                List<PROD_HB_SAIDA_LOTE> listaSaidaLote = new List<PROD_HB_SAIDA_LOTE>();
                foreach (GridViewRow row in gvPrincipal.Rows)
                {
                    codigoSaidaLote = Convert.ToInt32(gvPrincipal.DataKeys[row.RowIndex].Value);
                    colecao = ((Literal)row.FindControl("litColecao")).Text.ToUpper().Trim();

                    _saidaLote = faccController.ObterSaidaHBLote(codigoSaidaLote);
                    _saidaLote.DATA_EMISSAO = Convert.ToDateTime(txtEmissao.Text.Trim());
                    faccController.AtualizarSaidaHBLote(_saidaLote);

                    _saida = faccController.ObterSaidaHB(_saidaLote.PROD_HB_SAIDA);
                    _saida.CODIGO_FILIAL = ddlFilial.SelectedValue;
                    _saida.NF_SAIDA = nfSaida;
                    _saida.SERIE_NF = txtSerie.Text.Trim();
                    _saida.EMISSAO = Convert.ToDateTime(txtEmissao.Text.Trim());
                    _saida.USUARIO_EMISSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    faccController.AtualizarSaidaHB(_saida);

                    //GERAR REGISTRO ENTRADA
                    _entrada = new PROD_HB_ENTRADA();
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

                    listaSaidaLote.Add(_saidaLote);
                }

                if (Constante.enviarEmail)
                    EnviarEmail(listaSaidaLote, Convert.ToInt32(hidTela.Value));

                labErro.Text = "Emissão da Nota Fiscal salva com Sucesso.";

                btSalvar.Enabled = false;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "EMAIL"
        private void EnviarEmail(List<PROD_HB_SAIDA_LOTE> _saidaLote, int tela)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            string assunto = "";
            string titulo = "";
            string subTitulo = "";
            if (_saidaLote[0].PROD_HB_SAIDA1.TIPO == 'I')
                titulo = "INTERNO ";
            else
                titulo = "NOTA FISCAL EMITIDA ";

            if (tela == 21)
                subTitulo = "ACABAMENTO";

            assunto = "Intranet: " + titulo + "" + subTitulo + " - LOTE: " + _saidaLote[0].LOTE.ToString();
            email.ASSUNTO = assunto;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(_saidaLote, assunto, tela);

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
        private string MontarCorpoEmail(List<PROD_HB_SAIDA_LOTE> _saidaLote, string assunto, int tela)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            //var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(_saida.PROD_HB1.PROD_GRADE), Convert.ToInt32(hidConexao.Value));

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>NF Emitida - HB</title>");
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
            sb.Append("                                    " + _saidaLote[0].LOTE.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string fase = "";
            var faseLinx = faccController.ObterFase(_saidaLote[0].PROD_HB_SAIDA1.PROD_SERVICO1.FASE_LINX);
            if (faseLinx != null)
                fase = faseLinx.FASE_PRODUCAO.Trim() + " - " + faseLinx.DESC_FASE_PRODUCAO.Trim();
            else
                fase = _saidaLote[0].PROD_HB_SAIDA1.PROD_SERVICO1.DESCRICAO + " (Fase não encontrada no LINX)";

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
            sb.Append("                                    " + ((_saidaLote[0].PROD_HB_SAIDA1.TIPO == 'I') ? "01 - INTERNO" : "02 - EXTERNO"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string fornecedor = "";
            var f = faccController.ObterRecursoProdutivo(_saidaLote[0].PROD_HB_SAIDA1.FORNECEDOR);

            if (f != null)
                fornecedor = f.RECURSO_PRODUTIVO.Trim() + " - " + f.DESC_RECURSO.Trim();
            else
                fornecedor = _saidaLote[0].PROD_HB_SAIDA1.FORNECEDOR + " " + "(Recurso NÃO cadastrado no LINX)";

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Recurso Produtivo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + fornecedor);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string cpfFormatado = _saidaLote[0].PROD_HB_SAIDA1.CNPJ.Trim();
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
            sb.Append("                                    R$ " + Convert.ToDecimal(_saidaLote[0].PROD_HB_SAIDA1.CUSTO).ToString("###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço Produção:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$ " + Convert.ToDecimal(_saidaLote[0].PROD_HB_SAIDA1.PRECO).ToString("###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Volume:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _saidaLote[0].PROD_HB_SAIDA1.VOLUME);
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

            foreach (var s in _saidaLote)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td style='text-align:center;'>");
                sb.Append("                                    " + s.PROD_HB_SAIDA1.PROD_HB1.ORDEM_PRODUCAO);
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align:center;'>");
                sb.Append("                                    " + s.PROD_HB_SAIDA1.PROD_HB1.COLECAO.Trim() + "" + s.PROD_HB_SAIDA1.PROD_HB1.HB.ToString() + "" + s.PROD_HB_SAIDA1.PROD_HB1.MOSTRUARIO.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + new BaseController().BuscaColecaoAtual(s.PROD_HB_SAIDA1.PROD_HB1.COLECAO).DESC_COLECAO.Trim());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align:center;'>");
                sb.Append("                                    " + s.PROD_HB_SAIDA1.PROD_HB1.HB.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align:center;'>");
                sb.Append("                                    " + s.PROD_HB_SAIDA1.PROD_HB1.CODIGO_PRODUTO_LINX);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + s.PROD_HB_SAIDA1.PROD_HB1.NOME);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + prodController.ObterCoresBasicas(s.PROD_HB_SAIDA1.PROD_HB1.COR).DESC_COR.Trim());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align:center;'>");
                sb.Append("                                    " + s.PROD_HB_SAIDA1.GRADE_TOTAL.ToString());
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

    }
}
