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
    public partial class facc_produto_pendente_baixar : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtEmissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtRecebimento.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {

                int codigoEntrada = 0;
                int tela = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Request.QueryString["t"] == null || Request.QueryString["t"] == "" ||
                    Session["USUARIO"] == null)
                    Response.Redirect("facc_menu.aspx");

                codigoEntrada = Convert.ToInt32(Request.QueryString["p"].ToString());
                tela = Convert.ToInt32(Request.QueryString["t"].ToString());

                PROD_HB_ENTRADA _entrada = faccController.ObterEntradaHB(codigoEntrada);
                if (_entrada == null)
                    Response.Redirect("facc_menu.aspx");

                if (_entrada.STATUS == 'B' || _entrada.STATUS == 'R')
                {
                    Response.Write("ENTRADA DE PRODUTO JÁ FOI REALIZADA.");
                    Response.End();
                }

                CarregarFornecedores();
                CarregarServicosProducao();
                hidCodigoEntrada.Value = codigoEntrada.ToString();
                hidTela.Value = tela.ToString();

                txtColecao.Text = new BaseController().BuscaColecaoAtual(_entrada.PROD_HB_SAIDA1.PROD_HB1.COLECAO).DESC_COLECAO.Trim();
                txtHB.Text = _entrada.PROD_HB_SAIDA1.PROD_HB1.HB.ToString();
                txtProduto.Text = _entrada.PROD_HB_SAIDA1.PROD_HB1.CODIGO_PRODUTO_LINX;
                txtNome.Text = _entrada.PROD_HB_SAIDA1.PROD_HB1.NOME;
                txtCor.Text = prodController.ObterCoresBasicas(_entrada.PROD_HB_SAIDA1.PROD_HB1.COR).DESC_COR.Trim();
                txtQtde.Text = _entrada.PROD_HB_SAIDA1.GRADE_TOTAL.ToString();
                txtMostruario.Text = (_entrada.PROD_HB_SAIDA1.PROD_HB1.MOSTRUARIO == 'S') ? "Sim" : "Não";
                ddlFornecedor.SelectedValue = _entrada.PROD_HB_SAIDA1.FORNECEDOR;
                txtFornecedorSub.Text = _entrada.PROD_HB_SAIDA1.FORNECEDOR_SUB;
                ddlTipo.SelectedValue = _entrada.PROD_HB_SAIDA1.TIPO.ToString();
                ddlServico.SelectedValue = _entrada.PROD_HB_SAIDA1.PROD_SERVICO.ToString();
                txtPrecoCusto.Text = _entrada.PROD_HB_SAIDA1.CUSTO.ToString();
                txtPrecoProducao.Text = _entrada.PROD_HB_SAIDA1.PRECO.ToString();
                txtVolume.Text = _entrada.PROD_HB_SAIDA1.VOLUME.ToString();

                txtGradeEXP_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_EXP.ToString();
                txtGradeXP_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_XP.ToString();
                txtGradePP_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_PP.ToString();
                txtGradeP_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_P.ToString();
                txtGradeM_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_M.ToString();
                txtGradeG_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_G.ToString();
                txtGradeGG_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_GG.ToString();
                txtGradeTotal_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_TOTAL.ToString();

                if (_entrada.PROD_HB_SAIDA1.GRADE_EXP <= 0)
                {
                    txtGradeEXP_R.Text = _entrada.PROD_HB_SAIDA1.GRADE_EXP.ToString();
                    txtGradeEXP_R.Text = "0";
                    txtGradeEXP_R.Enabled = false;
                }
                if (_entrada.PROD_HB_SAIDA1.GRADE_XP <= 0)
                {
                    txtGradeXP_R.Text = _entrada.PROD_HB_SAIDA1.GRADE_XP.ToString();
                    txtGradeXP_R.Text = "0";
                    txtGradeXP_R.Enabled = false;
                }
                if (_entrada.PROD_HB_SAIDA1.GRADE_PP <= 0)
                {
                    txtGradePP_R.Text = _entrada.PROD_HB_SAIDA1.GRADE_PP.ToString();
                    txtGradePP_R.Text = "0";
                    txtGradePP_R.Enabled = false;
                }
                if (_entrada.PROD_HB_SAIDA1.GRADE_P <= 0)
                {
                    txtGradeP_R.Text = _entrada.PROD_HB_SAIDA1.GRADE_P.ToString();
                    txtGradeP_R.Text = "0";
                    txtGradeP_R.Enabled = false;
                }
                if (_entrada.PROD_HB_SAIDA1.GRADE_M <= 0)
                {
                    txtGradeM_R.Text = _entrada.PROD_HB_SAIDA1.GRADE_M.ToString();
                    txtGradeM_R.Text = "0";
                    txtGradeM_R.Enabled = false;
                }
                if (_entrada.PROD_HB_SAIDA1.GRADE_G <= 0)
                {
                    txtGradeG_R.Text = _entrada.PROD_HB_SAIDA1.GRADE_G.ToString();
                    txtGradeG_R.Text = "0";
                    txtGradeG_R.Enabled = false;
                }
                if (_entrada.PROD_HB_SAIDA1.GRADE_GG <= 0)
                {
                    txtGradeGG_R.Text = _entrada.PROD_HB_SAIDA1.GRADE_GG.ToString();
                    txtGradeGG_R.Text = "0";
                    txtGradeGG_R.Enabled = false;
                }

                if (_entrada.CODIGO_FILIAL != null)
                    ddlFilial.SelectedValue = _entrada.CODIGO_FILIAL;
                txtNF.Text = _entrada.NF_ENTRADA;
                txtSerie.Text = _entrada.SERIE_NF;
                if (_entrada.EMISSAO != null)
                    txtEmissao.Text = Convert.ToDateTime(_entrada.EMISSAO).ToString("dd/MM/yyyy");

                if (_entrada.PROD_HB_SAIDA1.TIPO == 'I') //SE FOR INTERNO
                {
                    txtNF.Enabled = false;
                    txtSerie.Enabled = false;
                    txtEmissao.Enabled = false;
                }

                if (_entrada.RECEBIMENTO != null)
                    txtRecebimento.Text = Convert.ToDateTime(_entrada.RECEBIMENTO).ToString("dd/MM/yyyy");

                if (tela == 21 || ddlServico.SelectedValue == "6")
                {
                    ddlProdutoAcabado.SelectedValue = "S";
                    ddlProdutoAcabado.Enabled = false;
                }
                else
                {
                    if (_entrada.PRODUTO_ACABADO != null)
                        ddlProdutoAcabado.SelectedValue = (_entrada.PRODUTO_ACABADO == 'S') ? "S" : "N";
                }

                txtGradeTotal_R.Enabled = false;

                if (_entrada.PROD_HB_SAIDA1.PROD_HB1.COLECAO.Trim().Length == 2)
                {
                    txtNF.MaxLength = 6;
                    ddlFilial.SelectedValue = "000029";
                }
                else
                {
                    txtNF.MaxLength = 9;
                    ddlFilial.SelectedValue = "8800  ";
                }

                txtObservacao.Text = _entrada.OBSERVACAO;

                CarregarNomeGrade(_entrada.PROD_HB_SAIDA1.PROD_HB1);
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

        private bool ValidarCampos(PROD_HB_ENTRADA _entrada)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            string tipoRecurso = _entrada.PROD_HB_SAIDA1.TIPO.ToString();
            string status = _entrada.STATUS.ToString();

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            labQtdeNota.ForeColor = _OK;
            if (txtQtdeNota.Text.Trim() == "")
            {
                labQtdeNota.ForeColor = _notOK;
                retorno = false;
            }

            /*labNF.ForeColor = _OK;
            if (txtNF.Text.Trim() == "" && tipoRecurso == "E" && status == "A")
            {
                labNF.ForeColor = _notOK;
                retorno = false;
            }

            labSerie.ForeColor = _OK;
            if (txtSerie.Text.Trim() == "" && tipoRecurso == "E" && status == "A")
            {
                labSerie.ForeColor = _notOK;
                retorno = false;
            }

            labEmissao.ForeColor = _OK;
            if (txtEmissao.Text.Trim() == "" && tipoRecurso == "E" && status == "A")
            {
                labEmissao.ForeColor = _notOK;
                retorno = false;
            }*/

            labRecebimento.ForeColor = _OK;
            if (txtRecebimento.Text.Trim() == "")
            {
                labRecebimento.ForeColor = _notOK;
                retorno = false;
            }

            labProdutoAcabado.ForeColor = _OK;
            if (ddlProdutoAcabado.SelectedValue == "")
            {
                labProdutoAcabado.ForeColor = _notOK;
                retorno = false;
            }

            labStatus.ForeColor = _OK;
            if (ddlStatus.SelectedValue == "")
            {
                labStatus.ForeColor = _notOK;
                retorno = false;
            }

            labPrecoDesconto.ForeColor = _OK;
            if (ddlStatus.SelectedValue == "D" && txtPrecoDesconto.Text.Trim() == "")
            {
                labPrecoDesconto.ForeColor = _notOK;
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
                char statusAnterior = ' ';
                labErro.Text = "";

                PROD_HB_ENTRADA _entrada = faccController.ObterEntradaHB(Convert.ToInt32(hidCodigoEntrada.Value));

                if (_entrada == null)
                {
                    labErro.Text = "Nenhuma entrada de produto encontrada. Entre em contato com TI.";
                    return;
                }

                if (!ValidarCampos(_entrada))
                {
                    labErro.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                if ((ddlStatus.SelectedValue == "D" || ddlStatus.SelectedValue == "R" || ddlStatus.SelectedValue == "B") && (txtGradeTotal_R.Text == "" || txtGradeTotal_R.Text == "0"))
                {
                    labErro.Text = "Informe a grade recebida.";
                    return;
                }

                //Guardar status anterior
                statusAnterior = _entrada.STATUS;

                _entrada.CODIGO_FILIAL = ddlFilial.SelectedValue;

                _entrada.NF_ENTRADA = txtNF.Text.Trim();
                _entrada.SERIE_NF = txtSerie.Text.Trim();
                if (_entrada.PROD_HB_SAIDA1.PROD_HB1.TIPO == 'E')
                {
                    if (txtEmissao.Text.Trim() != "")
                        _entrada.EMISSAO = Convert.ToDateTime(txtEmissao.Text);
                }
                else
                {
                    _entrada.EMISSAO = DateTime.Now;
                }
                _entrada.RECEBIMENTO = Convert.ToDateTime(txtRecebimento.Text);
                _entrada.PRODUTO_ACABADO = Convert.ToChar(ddlProdutoAcabado.SelectedValue);
                _entrada.USUARIO_RECEBIMENTO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                if (txtGradeEXP_R.Text.Trim() != "")
                    _entrada.GRADE_EXP = Convert.ToInt32(txtGradeEXP_R.Text.Trim());
                if (txtGradeXP_R.Text.Trim() != "")
                    _entrada.GRADE_XP = Convert.ToInt32(txtGradeXP_R.Text.Trim());
                if (txtGradePP_R.Text.Trim() != "")
                    _entrada.GRADE_PP = Convert.ToInt32(txtGradePP_R.Text.Trim());
                if (txtGradeP_R.Text.Trim() != "")
                    _entrada.GRADE_P = Convert.ToInt32(txtGradeP_R.Text.Trim());
                if (txtGradeM_R.Text.Trim() != "")
                    _entrada.GRADE_M = Convert.ToInt32(txtGradeM_R.Text.Trim());
                if (txtGradeG_R.Text.Trim() != "")
                    _entrada.GRADE_G = Convert.ToInt32(txtGradeG_R.Text.Trim());
                if (txtGradeGG_R.Text.Trim() != "")
                    _entrada.GRADE_GG = Convert.ToInt32(txtGradeGG_R.Text.Trim());

                int gradeTotal = 0;
                if (txtGradeTotal_R.Text.Trim() != "")
                {
                    gradeTotal = (_entrada.GRADE_EXP + _entrada.GRADE_XP + _entrada.GRADE_PP + _entrada.GRADE_P + _entrada.GRADE_M + _entrada.GRADE_G + _entrada.GRADE_GG);
                    _entrada.GRADE_TOTAL = gradeTotal;
                }

                _entrada.STATUS = Convert.ToChar(ddlStatus.SelectedValue);
                if (txtPrecoDesconto.Text.Trim() != "")
                    _entrada.CUSTO_DESCONTO = Convert.ToDecimal(txtPrecoDesconto.Text.Trim());

                if (_entrada.STATUS == 'B' || _entrada.STATUS == 'R' || _entrada.STATUS == 'D')
                    _entrada.DATA_DISTRIBUICAO = DateTime.Now;

                _entrada.QTDE_NOTA = Convert.ToInt32(txtQtdeNota.Text.Trim());
                _entrada.OBSERVACAO = txtObservacao.Text.Trim();


                if (statusAnterior == 'S' || statusAnterior == 'L' || statusAnterior == 'C')
                    _entrada.STATUS = statusAnterior;
                faccController.AtualizarEntradaHB(_entrada);

                //FALTA ENTREGAR PECA?
                if ((_entrada.GRADE_TOTAL < _entrada.PROD_HB_SAIDA1.GRADE_TOTAL) && (_entrada.STATUS == 'B' || _entrada.STATUS == 'R' || _entrada.STATUS == 'D' || statusAnterior == 'S' || statusAnterior == 'L' || statusAnterior == 'C'))
                {
                    int codigoSaidaNova = 0;
                    var saidaAnterior = faccController.ObterSaidaHB(_entrada.PROD_HB_SAIDA1.CODIGO);
                    PROD_HB_SAIDA _saidaNova = new PROD_HB_SAIDA();
                    _saidaNova.PROD_HB = saidaAnterior.PROD_HB;
                    _saidaNova.CNPJ = saidaAnterior.CNPJ;
                    _saidaNova.FORNECEDOR = saidaAnterior.FORNECEDOR;
                    _saidaNova.FORNECEDOR_SUB = saidaAnterior.FORNECEDOR_SUB;
                    _saidaNova.CODIGO_FILIAL = saidaAnterior.CODIGO_FILIAL;
                    _saidaNova.NF_SAIDA = saidaAnterior.NF_SAIDA;
                    _saidaNova.SERIE_NF = saidaAnterior.SERIE_NF;
                    _saidaNova.EMISSAO = saidaAnterior.EMISSAO;
                    _saidaNova.USUARIO_EMISSAO = saidaAnterior.USUARIO_EMISSAO;
                    _saidaNova.VOLUME = saidaAnterior.VOLUME;
                    _saidaNova.CUSTO = saidaAnterior.CUSTO;
                    _saidaNova.PRECO = saidaAnterior.PRECO;
                    _saidaNova.PROD_SERVICO = saidaAnterior.PROD_SERVICO;
                    _saidaNova.TIPO = saidaAnterior.TIPO;
                    _saidaNova.USUARIO_LIBERACAO = saidaAnterior.USUARIO_LIBERACAO;
                    _saidaNova.DATA_LIBERACAO = saidaAnterior.DATA_LIBERACAO;
                    _saidaNova.DATA_INCLUSAO = saidaAnterior.DATA_INCLUSAO;
                    _saidaNova.PROD_PROCESSO = saidaAnterior.PROD_PROCESSO;

                    _saidaNova.GRADE_EXP = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_EXP - _entrada.GRADE_EXP);
                    _saidaNova.GRADE_XP = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_XP - _entrada.GRADE_XP);
                    _saidaNova.GRADE_PP = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_PP - _entrada.GRADE_PP);
                    _saidaNova.GRADE_P = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_P - _entrada.GRADE_P);
                    _saidaNova.GRADE_M = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_M - _entrada.GRADE_M);
                    _saidaNova.GRADE_G = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_G - _entrada.GRADE_G);
                    _saidaNova.GRADE_GG = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_GG - _entrada.GRADE_GG);
                    _saidaNova.GRADE_TOTAL = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_TOTAL - _entrada.GRADE_TOTAL);
                    _saidaNova.SALDO = 'N';

                    codigoSaidaNova = faccController.InserirSaidaHB(_saidaNova);

                    PROD_HB_ENTRADA _entradaFaltante = new PROD_HB_ENTRADA();
                    _entradaFaltante.PROD_HB_SAIDA = codigoSaidaNova;
                    _entradaFaltante.GRADE_EXP = Convert.ToInt32(_saidaNova.GRADE_EXP);
                    _entradaFaltante.GRADE_XP = Convert.ToInt32(_saidaNova.GRADE_XP);
                    _entradaFaltante.GRADE_PP = Convert.ToInt32(_saidaNova.GRADE_PP);
                    _entradaFaltante.GRADE_P = Convert.ToInt32(_saidaNova.GRADE_P);
                    _entradaFaltante.GRADE_M = Convert.ToInt32(_saidaNova.GRADE_M);
                    _entradaFaltante.GRADE_G = Convert.ToInt32(_saidaNova.GRADE_G);
                    _entradaFaltante.GRADE_GG = Convert.ToInt32(_saidaNova.GRADE_GG);
                    _entradaFaltante.GRADE_TOTAL = Convert.ToInt32(_saidaNova.GRADE_TOTAL);

                    if (statusAnterior == 'S' || statusAnterior == 'L' || statusAnterior == 'C')
                        _entradaFaltante.STATUS = statusAnterior;
                    else
                        _entradaFaltante.STATUS = 'P'; //PENDENTE

                    _entradaFaltante.DATA_INCLUSAO = DateTime.Now;
                    faccController.InserirEntradaHB(_entradaFaltante);
                }

                if (ddlProdutoAcabado.SelectedValue == "N" && (_entrada.STATUS == 'B' || _entrada.STATUS == 'D'))
                {
                    //Gerar registro de Facção
                    //SE JA EXISTIR UM REGISTRO EM ABERTO, ADICIONA
                    int codigoProcesso = 0;
                    codigoProcesso = (_entrada.PROD_HB_SAIDA1.PROD_SERVICO == 1) ? 21 : 20;

                    var _saidaAberto = faccController.ObterSaidaHB(_entrada.PROD_HB_SAIDA1.PROD_HB.ToString()).Where(p =>
                        p.DATA_LIBERACAO == null &&
                        p.PROD_PROCESSO == codigoProcesso
                        ).FirstOrDefault();
                    if (_saidaAberto != null)
                    {
                        _saidaAberto.GRADE_EXP = _saidaAberto.GRADE_EXP + ((_entrada.GRADE_EXP < 0) ? 0 : _entrada.GRADE_EXP);
                        _saidaAberto.GRADE_XP = _saidaAberto.GRADE_XP + ((_entrada.GRADE_XP < 0) ? 0 : _entrada.GRADE_XP);
                        _saidaAberto.GRADE_PP = _saidaAberto.GRADE_PP + ((_entrada.GRADE_PP < 0) ? 0 : _entrada.GRADE_PP);
                        _saidaAberto.GRADE_P = _saidaAberto.GRADE_P + ((_entrada.GRADE_P < 0) ? 0 : _entrada.GRADE_P);
                        _saidaAberto.GRADE_M = _saidaAberto.GRADE_M + ((_entrada.GRADE_M < 0) ? 0 : _entrada.GRADE_M);
                        _saidaAberto.GRADE_G = _saidaAberto.GRADE_G + ((_entrada.GRADE_G < 0) ? 0 : _entrada.GRADE_G);
                        _saidaAberto.GRADE_GG = _saidaAberto.GRADE_GG + ((_entrada.GRADE_GG < 0) ? 0 : _entrada.GRADE_GG);
                        _saidaAberto.GRADE_TOTAL = _saidaAberto.GRADE_TOTAL + ((_entrada.GRADE_TOTAL < 0) ? 0 : _entrada.GRADE_TOTAL);

                        faccController.AtualizarSaidaHB(_saidaAberto);
                    }
                    else
                    {
                        PROD_HB_SAIDA _saida = new PROD_HB_SAIDA();
                        _saida.PROD_HB = _entrada.PROD_HB_SAIDA1.PROD_HB;
                        _saida.PROD_PROCESSO = codigoProcesso;
                        _saida.GRADE_EXP = _entrada.GRADE_EXP;
                        _saida.GRADE_XP = _entrada.GRADE_XP;
                        _saida.GRADE_PP = _entrada.GRADE_PP;
                        _saida.GRADE_P = _entrada.GRADE_P;
                        _saida.GRADE_M = _entrada.GRADE_M;
                        _saida.GRADE_G = _entrada.GRADE_G;
                        _saida.GRADE_GG = _entrada.GRADE_GG;
                        _saida.GRADE_TOTAL = _entrada.GRADE_TOTAL;
                        _saida.DATA_INCLUSAO = DateTime.Now;
                        _saida.SALDO = 'S';
                        faccController.InserirSaidaHB(_saida);
                    }

                }

                //if (_entrada.STATUS == 'D')
                //{
                //    var _saida = faccController.ObterSaidaHB(_entrada.PROD_HB_SAIDA);
                //    if (_saida != null)
                //    {
                //        _saida.PRECO = _entrada.CUSTO_DESCONTO;
                //        _saida.CUSTO = _entrada.CUSTO_DESCONTO;
                //        faccController.AtualizarSaidaHB(_saida);

                //        _entrada.PROD_HB_SAIDA1.CUSTO = _entrada.CUSTO_DESCONTO;
                //        _entrada.PROD_HB_SAIDA1.PRECO = _entrada.CUSTO_DESCONTO;
                //    }
                //}

                if (Constante.enviarEmail)
                    if (_entrada.STATUS == 'B' || _entrada.STATUS == 'D' || _entrada.STATUS == 'R')
                        EnviarEmail(_entrada, Convert.ToInt32(hidTela.Value));

                labErro.Text = "Entrada de Produto salva com sucesso.";

                btSalvar.Enabled = false;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (txtGradeTotal_R.Text.Trim() == "")
                {
                    ddlStatus.SelectedValue = "P";
                    labErro.Text = "Informe a Grade Recebida.";
                    return;
                }

                string status = ddlStatus.SelectedValue.Trim();

                divDesconto.Visible = false;
                txtPrecoDesconto.Text = "";
                if (status == "D")
                    divDesconto.Visible = true;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void txtPrecoDesconto_TextChanged(object sender, EventArgs e)
        {
            if (txtPrecoDesconto.Text.Trim() != "")
            {
                decimal precoDesconto = Convert.ToDecimal(txtPrecoDesconto.Text.Trim());
                int gradeTotal = Convert.ToInt32(txtGradeTotal_R.Text.Trim());

                txtValorTotal.Text = (gradeTotal * precoDesconto).ToString();
            }
        }

        #region "EMAIL"
        private void EnviarEmail(PROD_HB_ENTRADA _entrada, int tela)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            string assunto = "";
            string titulo = "";
            string subTitulo = "";

            if (_entrada.STATUS == 'R')
            {
                titulo = "ENT PRODUTO";
                subTitulo = "PERDA";
            }
            else
            {
                if (_entrada.STATUS == 'D')
                    subTitulo = "DESCONTO";
                if (_entrada.STATUS == 'B')
                    subTitulo = "HANDBOOK";

                if (_entrada.PRODUTO_ACABADO == 'S')
                    titulo = "ENT PRODUTO ACABADO";
                else
                    titulo = "ENT PRODUTO NÃO ACABADO";
            }

            assunto = "Intranet: " + titulo + " - O.P.: " + _entrada.PROD_HB_SAIDA1.PROD_HB1.ORDEM_PRODUCAO + " - " + subTitulo;
            email.ASSUNTO = assunto;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(_entrada, assunto, tela);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails 
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(9, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(PROD_HB_ENTRADA _entrada, string assunto, int tela)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(_entrada.PROD_HB_SAIDA1.PROD_HB1.PROD_GRADE));

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>ENTRADA DE PRODUTO</title>");
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
            sb.Append("                        <h2>" + assunto.Replace("Intranet:", "") + "</h2>");
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
            sb.Append("                                    " + _entrada.PROD_HB_SAIDA1.PROD_HB1.ORDEM_PRODUCAO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    O.C.:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _entrada.PROD_HB_SAIDA1.PROD_HB1.COLECAO.Trim() + _entrada.PROD_HB_SAIDA1.PROD_HB1.HB.ToString() + _entrada.PROD_HB_SAIDA1.PROD_HB1.MOSTRUARIO.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Coleção:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + new BaseController().BuscaColecaoAtual(_entrada.PROD_HB_SAIDA1.PROD_HB1.COLECAO).DESC_COLECAO.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    HB:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _entrada.PROD_HB_SAIDA1.PROD_HB1.HB);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Produto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _entrada.PROD_HB_SAIDA1.PROD_HB1.CODIGO_PRODUTO_LINX);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Nome");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _entrada.PROD_HB_SAIDA1.PROD_HB1.NOME);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _entrada.PROD_HB_SAIDA1.PROD_HB1.COR.Trim() + " - " + prodController.ObterCoresBasicas(_entrada.PROD_HB_SAIDA1.PROD_HB1.COR).DESC_COR.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Produto Acabado:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ((_entrada.PRODUTO_ACABADO == 'S') ? "SIM" : "NÃO"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Quantidade:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _entrada.GRADE_TOTAL.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Mostruário:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ((_entrada.PROD_HB_SAIDA1.PROD_HB1.MOSTRUARIO == 'S') ? "SIM" : "NÃO"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string fase = "";
            var faseLinx = faccController.ObterFase(_entrada.PROD_HB_SAIDA1.PROD_SERVICO1.FASE_LINX);
            if (faseLinx != null)
                fase = faseLinx.FASE_PRODUCAO.Trim() + " - " + faseLinx.DESC_FASE_PRODUCAO.Trim();
            else
                fase = "RETORNO " + _entrada.PROD_HB_SAIDA1.PROD_SERVICO1.DESCRICAO + " (Fase não encontrada no LINX)";

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Fase Atual:");
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
            sb.Append("                                    " + ((_entrada.PROD_HB_SAIDA1.TIPO == 'I') ? "01 - INTERNO" : "02 - EXTERNO"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string fornecedor = "";
            var f = faccController.ObterRecursoProdutivo(_entrada.PROD_HB_SAIDA1.FORNECEDOR);

            if (f != null)
                fornecedor = f.RECURSO_PRODUTIVO.Trim() + " - " + f.DESC_RECURSO.Trim();
            else
                fornecedor = _entrada.PROD_HB_SAIDA1.FORNECEDOR + " " + "(Recurso NÃO cadastrado no LINX)";

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Recurso Produtivo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + fornecedor + " " + _entrada.PROD_HB_SAIDA1.FORNECEDOR_SUB);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string cpfFormatado = _entrada.PROD_HB_SAIDA1.CNPJ.Trim();
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

            fase = "";
            if (_entrada.PRODUTO_ACABADO == 'S')
            {
                fase = "P06 - CONFERENCIA FICHA";
            }
            else
            {
                faseLinx = faccController.ObterFase(_entrada.PROD_HB_SAIDA1.PROD_SERVICO1.FASE_LINX_RET);
                if (faseLinx != null)
                    fase = faseLinx.FASE_PRODUCAO.Trim() + " - " + faseLinx.DESC_FASE_PRODUCAO.Trim();
                else
                    fase = "RETORNO " + _entrada.PROD_HB_SAIDA1.PROD_SERVICO1.DESCRICAO + " (Fase não encontrada no LINX)";
            }

            if (_entrada.STATUS == 'B')
                fase = "A DEFINIR - HANDBOOK";

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Fase Seguinte:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    <font color='red'><b>" + fase + "</b></font>");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Custo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$ " + Convert.ToDecimal(_entrada.PROD_HB_SAIDA1.CUSTO).ToString("###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$ " + Convert.ToDecimal(_entrada.PROD_HB_SAIDA1.PRECO).ToString("###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Volume:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _entrada.PROD_HB_SAIDA1.VOLUME);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Filial:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _entrada.CODIGO_FILIAL + " - " + (new BaseController().BuscaFilialCodigoInt(Convert.ToInt32(_entrada.CODIGO_FILIAL)).FILIAL.Trim()));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Nota Fiscal:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _entrada.NF_ENTRADA);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Série:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + _entrada.SERIE_NF);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Emissão:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + Convert.ToDateTime(_entrada.EMISSAO).ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Recebimento:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + Convert.ToDateTime(_entrada.RECEBIMENTO).ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Observação:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    <font color='red'><b>" + _entrada.OBSERVACAO + "</b></font>");
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
            sb.Append("                                                 Grade Recebida");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_entrada == null) ? 0 : _entrada.GRADE_EXP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_entrada == null) ? 0 : _entrada.GRADE_XP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_entrada == null) ? 0 : _entrada.GRADE_PP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_entrada == null) ? 0 : _entrada.GRADE_P)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_entrada == null) ? 0 : _entrada.GRADE_M)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_entrada == null) ? 0 : _entrada.GRADE_G)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((_entrada == null) ? 0 : _entrada.GRADE_GG)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                 " + _entrada.GRADE_TOTAL.ToString());
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");
            sb.Append("                                     </table>");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                             <tr>");
            sb.Append("                                 <td>");
            sb.Append("                                     &nbsp;");
            sb.Append("                                 </td>");
            sb.Append("                                 <td>");
            sb.Append("                                     &nbsp;");
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
