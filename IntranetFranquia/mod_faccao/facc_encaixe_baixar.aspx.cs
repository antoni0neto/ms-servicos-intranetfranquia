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
    public partial class facc_encaixe_baixar : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPrevEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                int codigoSaida = 0;
                int tela = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Request.QueryString["t"] == null || Request.QueryString["t"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("facc_menu.aspx");

                codigoSaida = Convert.ToInt32(Request.QueryString["p"].ToString());
                tela = Convert.ToInt32(Request.QueryString["t"].ToString());

                PROD_HB_SAIDA _saida = faccController.ObterSaidaHB(codigoSaida);
                if (_saida == null)
                    Response.Redirect("facc_menu.aspx");

                if (_saida.DATA_LIBERACAO != null)
                {
                    Response.Write("PRODUTO JÁ ENVIADO PARA EMISSÃO DA NF.");
                    Response.End();
                }

                CarregarFornecedores(tela);
                CarregarServicosProducao(tela, Convert.ToChar(_saida.PROD_HB1.MOSTRUARIO));

                hidCodigoSaida.Value = codigoSaida.ToString();
                hidTela.Value = tela.ToString();
                hidProdHB.Value = _saida.PROD_HB.ToString();
                hidColecao.Value = _saida.PROD_HB1.COLECAO;

                txtColecao.Text = new BaseController().BuscaColecaoAtual(_saida.PROD_HB1.COLECAO).DESC_COLECAO.Trim();
                txtHB.Text = _saida.PROD_HB1.HB.ToString();
                txtProduto.Text = _saida.PROD_HB1.CODIGO_PRODUTO_LINX;
                txtNome.Text = _saida.PROD_HB1.NOME;
                txtCor.Text = prodController.ObterCoresBasicas(_saida.PROD_HB1.COR).DESC_COR.Trim();
                txtQtde.Text = _saida.GRADE_TOTAL.ToString();
                txtMostruario.Text = (_saida.PROD_HB1.MOSTRUARIO == 'S') ? "Sim" : "Não";

                txtGradeEXP_O.Text = _saida.GRADE_EXP.ToString();
                txtGradeXP_O.Text = _saida.GRADE_XP.ToString();
                txtGradePP_O.Text = _saida.GRADE_PP.ToString();
                txtGradeP_O.Text = _saida.GRADE_P.ToString();
                txtGradeM_O.Text = _saida.GRADE_M.ToString();
                txtGradeG_O.Text = _saida.GRADE_G.ToString();
                txtGradeGG_O.Text = _saida.GRADE_GG.ToString();
                txtGradeTotal_O.Text = _saida.GRADE_TOTAL.ToString();
                hidGradeTotal.Value = _saida.GRADE_TOTAL.ToString();

                txtGradeEXP_E.Text = _saida.GRADE_EXP.ToString();
                txtGradeXP_E.Text = _saida.GRADE_XP.ToString();
                txtGradePP_E.Text = _saida.GRADE_PP.ToString();
                txtGradeP_E.Text = _saida.GRADE_P.ToString();
                txtGradeM_E.Text = _saida.GRADE_M.ToString();
                txtGradeG_E.Text = _saida.GRADE_G.ToString();
                txtGradeGG_E.Text = _saida.GRADE_GG.ToString();
                txtGradeTotal_E.Text = _saida.GRADE_TOTAL.ToString();

                if (_saida.GRADE_EXP <= 0)
                    txtGradeEXP_E.Enabled = false;
                if (_saida.GRADE_XP <= 0)
                    txtGradeXP_E.Enabled = false;
                if (_saida.GRADE_PP <= 0)
                    txtGradePP_E.Enabled = false;
                if (_saida.GRADE_P <= 0)
                    txtGradeP_E.Enabled = false;
                if (_saida.GRADE_M <= 0)
                    txtGradeM_E.Enabled = false;
                if (_saida.GRADE_G <= 0)
                    txtGradeG_E.Enabled = false;
                if (_saida.GRADE_GG <= 0)
                    txtGradeGG_E.Enabled = false;

                txtGradeTotal_E.Enabled = false;

                if (tela == 21)
                {
                    ddlServico.SelectedValue = "4";
                    ddlServico.Enabled = false;

                    //if (_saida.PROD_HB1.DATA_IMP_FIC_LOGISTICA == null && _saida.PROD_HB1.MOSTRUARIO == 'N')
                    //{
                    //    btSalvar.Enabled = false;
                    //    labErro.Text = "Produto não foi encaixado para separar Varejo e/ou Atacado.";
                    //}

                    txtPrevEntrega.Text = DateTime.Today.AddDays(3).ToString("dd/MM/yyyy");
                }

                CarregarNomeGrade(_saida.PROD_HB1);
                CarregarHistoricoEncaixe(_saida.PROD_HB.ToString());

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFornecedores(int tela)
        {

            var _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'S') || p.STATUS == 'S').ToList();

                if (tela == 21)
                    _fornecedores = _fornecedores.Where(p => p.CODIGO == 106 || p.CODIGO == 323 || p.CODIGO == 324 || p.CODIGO == 329).ToList();

                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });
                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }

        }
        private void CarregarFornecedoresSub(string fornecedor)
        {

            List<PROD_FORNECEDOR_SUB> fornecedorSub = faccController.ObterFornecedorSub(fornecedor);

            if (fornecedorSub != null)
            {
                fornecedorSub.Insert(0, new PROD_FORNECEDOR_SUB { FORNECEDOR_SUB = "" });
                ddlFornecedorSub.DataSource = fornecedorSub;
                ddlFornecedorSub.DataBind();
            }

        }
        private void CarregarServicosProducao(int tela, char mostruario)
        {
            List<PROD_SERVICO> _servico = new List<PROD_SERVICO>();
            _servico = prodController.ObterServicoProducao().Where(p => p.STATUS == 'A' && p.CODIGO != 5).ToList();

            if (tela == 20)
            {
                if (mostruario == 'N')
                    _servico = _servico.Where(p => p.CODIGO != 4 && p.CODIGO != 6).ToList();
                else
                    _servico = _servico.Where(p => p.CODIGO == 2 || p.CODIGO == 3 || p.CODIGO == 6).ToList();
            }
            else
            {

            }

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

                CarregarFornecedoresSub(ddlFornecedor.SelectedValue.Trim());
                ddlFornecedorSub.Enabled = true;
            }
            else
            {
                ddlTipo.SelectedValue = "E";
                ddlTipo.Enabled = true;

                ddlFornecedorSub.SelectedValue = "";
                ddlFornecedorSub.Enabled = false;
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

            labFornecedorSub.ForeColor = _OK;
            if (ddlFornecedorSub.Enabled && ddlFornecedorSub.SelectedValue == "")
            {
                labFornecedorSub.ForeColor = _notOK;
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

            labPrevEntrega.ForeColor = _OK;
            if (txtPrevEntrega.Text.Trim() == "")
            {
                labPrevEntrega.ForeColor = _notOK;
                retorno = false;
            }

            labGradeTitulo.ForeColor = _OK;
            if (txtGradeTotal_E.Text.Trim() == "" || txtGradeTotal_E.Text.Trim() == "0")
            {
                labGradeTitulo.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarHBCortado(int prod_hb)
        {
            return prodController.ValidarHBCortado(prod_hb);
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
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                labErro.Text = "";

                PROD_HB_SAIDA _saida = faccController.ObterSaidaHB(Convert.ToInt32(hidCodigoSaida.Value));
                if (_saida == null)
                {
                    labErro.Text = "Nenhuma sáida encontrada. Entre em contato com TI.";
                    return;
                }

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                if (_saida.PROD_HB1.MOSTRUARIO == 'N' && ddlTipo.SelectedValue == "E")
                {
                    if (txtPrecoCusto.Text.Trim() != txtPrecoProducao.Text.Trim())
                    {
                        labErro.Text = "Preço Custo e Preço Produção devem ser iguais.";
                        return;
                    }
                }

                if (ddlFornecedor.SelectedValue.Trim().ToUpper() != "HANDBOOK")
                {
                    if (ddlServico.SelectedValue != "1")
                    {
                        //Validar preco maior do que ja encaixado (Usuários informados não precisam de Validação)
                        var precoAnteriorProducao = faccController.ObterCustoProdutoMaior(_saida.PROD_HB1.COLECAO, Convert.ToInt32(ddlServico.SelectedValue), _saida.PROD_HB1.CODIGO_PRODUTO_LINX, 'N');
                        var precoAnteriorMostruario = faccController.ObterCustoProdutoMaior(_saida.PROD_HB1.COLECAO, 6, _saida.PROD_HB1.CODIGO_PRODUTO_LINX, 'S');
                        if ((precoAnteriorProducao != null || precoAnteriorMostruario != null) &&
                            codigoUsuario != 1144 && // Leandro
                            codigoUsuario != 1172 && // Luciana
                            codigoUsuario != 18 && // Tata
                            codigoUsuario != 28 && // Ana Paula
                            codigoUsuario != 1173) // Robson Camargo
                        {
                            decimal valPrecoMostruario = (precoAnteriorMostruario == null) ? -1 : Convert.ToDecimal(precoAnteriorMostruario.PRECO);
                            decimal valPrecoProducao = (precoAnteriorProducao == null) ? -1 : Convert.ToDecimal(precoAnteriorProducao.PRECO);

                            decimal valPrecoAnterior = 0;
                            if (valPrecoMostruario == -1) // SE PRECO MOSTRUARIO == NULL / PRODUCAO
                                valPrecoAnterior = valPrecoProducao;
                            else if (valPrecoProducao == -1) // SE PRECO PRODUCAO == NULL / MOSTRUARIO
                                valPrecoAnterior = valPrecoMostruario;
                            else if (valPrecoMostruario < valPrecoProducao) // SE PRECO MOSTRUARIO MENOR PRECO PRODUCAO / PRECO MOSTRUARIO
                                valPrecoAnterior = valPrecoMostruario;
                            else
                                valPrecoAnterior = valPrecoProducao;


                            if (Convert.ToDecimal(txtPrecoProducao.Text) > valPrecoAnterior)
                            {
                                labErro.Text = "Este produto já foi aprovado por um preço MENOR (R$ " + valPrecoAnterior.ToString() + ") do que o informado. Para encaixar este HB, abaixe o preço ou solicite Aprovação da Diretoria.";
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (codigoUsuario != 1144 && // Leandro
                            codigoUsuario != 1172 && // Luciana
                            codigoUsuario != 18 && // Tata
                            codigoUsuario != 28 && // Ana Paula
                            codigoUsuario != 1173) // Robson Camargo
                        {
                            // Solicitado por Tata - 25/07/2017
                            var prodHB = prodController.ObterHB(_saida.PROD_HB);
                            if (prodHB != null)
                            {
                                if (prodHB.PRECO_FACC_MOSTRUARIO != null)
                                {
                                    if (Convert.ToDecimal(txtPrecoProducao.Text) > prodHB.PRECO_FACC_MOSTRUARIO)
                                    {
                                        labErro.Text = "Este produto somente poderá ser aprovado por um valor menor ou igual a R$ " + prodHB.PRECO_FACC_MOSTRUARIO.ToString() + ". Para encaixar este HB, abaixe o preço ou solicite Aprovação da Diretoria.";
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }

                if (!ValidarHBCortado(_saida.PROD_HB) &&
                    Convert.ToInt32(txtGradeTotal_E.Text.Trim()) < Convert.ToInt32(hidGradeTotal.Value.Trim()) &&
                    ddlServico.SelectedValue == "1")
                {
                    labErro.Text = "Este HB não pode ser dividido entre Facções, pois ainda possui Detalhes para cortar.";
                    return;
                }

                PROD_HB_SAIDA _saidaNova = null;
                if (Convert.ToInt32(txtGradeTotal_E.Text) < Convert.ToInt32(txtGradeTotal_O.Text))
                {

                    _saidaNova = new PROD_HB_SAIDA();
                    _saidaNova.PROD_HB = _saida.PROD_HB;
                    _saidaNova.DATA_INCLUSAO = _saida.DATA_INCLUSAO;
                    _saidaNova.PROD_PROCESSO = Convert.ToInt32(hidTela.Value);
                    _saidaNova.GRADE_EXP = Convert.ToInt32(txtGradeEXP_O.Text) - Convert.ToInt32(txtGradeEXP_E.Text);
                    _saidaNova.GRADE_XP = Convert.ToInt32(txtGradeXP_O.Text) - Convert.ToInt32(txtGradeXP_E.Text);
                    _saidaNova.GRADE_PP = Convert.ToInt32(txtGradePP_O.Text) - Convert.ToInt32(txtGradePP_E.Text);
                    _saidaNova.GRADE_P = Convert.ToInt32(txtGradeP_O.Text) - Convert.ToInt32(txtGradeP_E.Text);
                    _saidaNova.GRADE_M = Convert.ToInt32(txtGradeM_O.Text) - Convert.ToInt32(txtGradeM_E.Text);
                    _saidaNova.GRADE_G = Convert.ToInt32(txtGradeG_O.Text) - Convert.ToInt32(txtGradeG_E.Text);
                    _saidaNova.GRADE_GG = Convert.ToInt32(txtGradeGG_O.Text) - Convert.ToInt32(txtGradeGG_E.Text);
                    _saidaNova.GRADE_TOTAL = Convert.ToInt32(txtGradeTotal_O.Text) - Convert.ToInt32(txtGradeTotal_E.Text);
                    _saidaNova.SALDO = 'S';

                    faccController.InserirSaidaHB(_saidaNova);
                }

                _saida.CNPJ = baseController.ObterCadastroCLIFOR(ddlFornecedor.SelectedValue).CGC_CPF;
                _saida.FORNECEDOR = ddlFornecedor.SelectedValue;
                _saida.FORNECEDOR_SUB = ddlFornecedorSub.SelectedValue;
                _saida.TIPO = Convert.ToChar(ddlTipo.SelectedValue);
                _saida.PROD_SERVICO = Convert.ToInt32(ddlServico.SelectedValue);
                _saida.CUSTO = Convert.ToDecimal(txtPrecoCusto.Text);
                _saida.PRECO = Convert.ToDecimal(txtPrecoProducao.Text);
                _saida.VOLUME = Convert.ToInt32(txtVolume.Text);
                _saida.DATA_LIBERACAO = DateTime.Now;
                _saida.DATA_PREV_ENTREGA = Convert.ToDateTime(txtPrevEntrega.Text);
                _saida.USUARIO_LIBERACAO = codigoUsuario;

                _saida.GRADE_EXP = Convert.ToInt32(txtGradeEXP_E.Text);
                _saida.GRADE_XP = Convert.ToInt32(txtGradeXP_E.Text);
                _saida.GRADE_PP = Convert.ToInt32(txtGradePP_E.Text);
                _saida.GRADE_P = Convert.ToInt32(txtGradeP_E.Text);
                _saida.GRADE_M = Convert.ToInt32(txtGradeM_E.Text);
                _saida.GRADE_G = Convert.ToInt32(txtGradeG_E.Text);
                _saida.GRADE_GG = Convert.ToInt32(txtGradeGG_E.Text);
                _saida.GRADE_TOTAL = Convert.ToInt32(txtGradeTotal_E.Text);

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

                /*INSERE VALORES DOS SERVICOS PARA CUSTO*/
                //Verifica preco aprovado
                if (!ProdutoFoiSimulado(_saida.PROD_HB1.CODIGO_PRODUTO_LINX, Convert.ToChar(_saida.PROD_HB1.MOSTRUARIO), 'S'))
                {
                    bool _inserir = false;
                    PROD_HB_CUSTO_SERVICO _servico = null;
                    _servico = prodController.ObterCustoServicoConexao().Where(p =>
                                                                            p.PRODUTO.Trim() == _saida.PROD_HB1.CODIGO_PRODUTO_LINX.Trim() &&
                                                                            p.MOSTRUARIO == _saida.PROD_HB1.MOSTRUARIO &&
                                                                            p.SERVICO == _saida.PROD_SERVICO).FirstOrDefault();
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

                    if ((_inserir) || (!_inserir && _servico.CUSTO < Convert.ToDecimal(txtPrecoCusto.Text)))
                        _servico.CUSTO = Convert.ToDecimal(txtPrecoCusto.Text);
                    if ((_inserir) || (!_inserir && _servico.CUSTO_PECA < Convert.ToDecimal(txtPrecoProducao.Text)))
                        _servico.CUSTO_PECA = Convert.ToDecimal(txtPrecoProducao.Text);

                    if (_inserir)
                        prodController.InserirCustoServico(_servico);
                    else
                        prodController.AtualizarCustoServico(_servico);

                }
                //VALIDA SE EXISTE PENDENTE NO LOTE, SE SIM, EXCLUI DO LOTE
                var saidaLote = faccController.ObterSaidaHBLote(_saida.CODIGO.ToString());
                if (saidaLote != null)
                {
                    faccController.ExcluirSaidaHBLote(saidaLote.CODIGO);
                }

                InserirAvaliacao(_saida);

                if (Constante.enviarEmail)
                    EnviarEmail(_saida, Convert.ToInt32(hidTela.Value));

                if (_saida.TIPO == 'I')
                    labErro.Text = "Enviado com sucesso para Entrada de Produtos.";
                else
                    labErro.Text = "Enviado com sucesso para Emissão da Nota Fiscal.";

                btSalvar.Enabled = false;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void InserirAvaliacao(PROD_HB_SAIDA _saida)
        {
            if (_saida.PROD_SERVICO == 1)
            {
                var s = faccController.ObterFaccaoAvaliacaoHB(_saida.PROD_HB1.CODIGO).Where(p => p.FORNECEDOR.Trim() == _saida.FORNECEDOR.Trim()).FirstOrDefault();
                if (s == null)
                {
                    PROD_HB_AVALIACAO avaliacao = new PROD_HB_AVALIACAO();
                    avaliacao.PROD_HB = _saida.PROD_HB1.CODIGO;
                    avaliacao.FORNECEDOR = _saida.FORNECEDOR;
                    avaliacao.DATA_INCLUSAO = DateTime.Now;
                    faccController.InserirFaccaoAvaliacao(avaliacao);
                }
            }
        }

        #region "HISTORICO ENCAIXE"

        private void CarregarHistoricoEncaixe(string prod_hb)
        {
            var _saida = faccController.ObterSaidaHB(prod_hb).Where(p => p.NF_SAIDA != null && p.GRADE_TOTAL > 0).OrderBy(p => p.EMISSAO);

            if (_saida == null || _saida.Count() <= 0)
                gvEncaixeHistorico.Visible = false;

            gvEncaixeHistorico.DataSource = _saida;
            gvEncaixeHistorico.DataBind();
        }
        protected void gvEncaixeHistorico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_SAIDA _saida = e.Row.DataItem as PROD_HB_SAIDA;

                    if (_saida != null)
                    {
                        Literal _litEmissao = e.Row.FindControl("litEmissao") as Literal;
                        if (_litEmissao != null)
                            if (_saida.EMISSAO != null)
                                _litEmissao.Text = Convert.ToDateTime(_saida.EMISSAO).ToString("dd/MM/yyyy");

                        Literal _litServico = e.Row.FindControl("litServico") as Literal;
                        if (_litServico != null)
                            if (_saida.PROD_SERVICO != null)
                                _litServico.Text = prodController.ObterServicoProducao(Convert.ToInt32(_saida.PROD_SERVICO)).DESCRICAO.ToUpper();

                        Literal _litRecurso = e.Row.FindControl("litRecurso") as Literal;
                        if (_litRecurso != null)
                            if (_saida.TIPO != null)
                                _litRecurso.Text = (_saida.TIPO == 'I') ? "INTERNO" : "EXTERNO";

                        Literal _litCusto = e.Row.FindControl("litCusto") as Literal;
                        if (_litCusto != null)
                            if (_saida.CUSTO != null)
                                _litCusto.Text = "R$ " + Convert.ToDecimal(_saida.CUSTO).ToString("###,###,##0.00");

                        Literal _litPreco = e.Row.FindControl("litPreco") as Literal;
                        if (_litPreco != null)
                            if (_saida.PRECO != null)
                                _litPreco.Text = "R$ " + Convert.ToDecimal(_saida.PRECO).ToString("###,###,##0.00");

                        Literal _litGrade = e.Row.FindControl("litGrade") as Literal;
                        if (_litGrade != null)
                        {
                            var entrada = faccController.ObterEntradaHB(_saida.CODIGO.ToString()).OrderByDescending(p => p.CODIGO).FirstOrDefault();
                            if (entrada != null)
                            {
                                _litGrade.Text = entrada.GRADE_TOTAL.ToString();
                                if (entrada.STATUS == 'R')
                                    _litGrade.Text = "<font color='red'><b>-" + entrada.GRADE_TOTAL.ToString() + "</b></font>";
                            }
                        }
                    }
                }
            }
        }
        #endregion

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
                titulo = "EMISSÃO NF ";

            if (tela == 21)
                subTitulo = "ACABAMENTO";

            assunto = "Intranet: " + titulo + "" + subTitulo + " - O.P.: " + _saida.PROD_HB1.ORDEM_PRODUCAO;
            email.ASSUNTO = assunto;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(_saida, assunto, tela);

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
            sb.Append("    <title>Encaixe - HB</title>");
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
            sb.Append("                                    " + fornecedor + " " + _saida.FORNECEDOR_SUB);
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
            sb.Append("                                    Preço Custo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$ " + Convert.ToDecimal(_saida.CUSTO).ToString("###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço Produção:");
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

        protected void ddlServico_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtPrecoProducao.Enabled = true;
            if (hidTela.Value == "20" && ddlServico.SelectedValue == "1")
            {
                int volume = 0;
                volume = prodController.ObterVolumeHB(Convert.ToInt32(hidProdHB.Value));
                txtVolume.Text = volume.ToString();

                txtPrecoCusto.Text = "";
                txtPrecoProducao.Text = "";
                var saidaMenor = faccController.ObterCustoProdutoMenor(hidColecao.Value, 6, txtProduto.Text, 'S');
                if (saidaMenor != null)
                {
                    txtPrecoCusto.Text = saidaMenor.PRECO.ToString();
                    txtPrecoProducao.Text = saidaMenor.PRECO.ToString();
                }
            }
            else if (hidTela.Value == "20" && ddlServico.SelectedValue == "6")
            {
                int codigoHB = 0;
                codigoHB = Convert.ToInt32(hidProdHB.Value);

                var prodHB = prodController.ObterHB(codigoHB);
                decimal? precoProducaoMostruario = null;
                if (prodHB != null)
                    precoProducaoMostruario = prodHB.PRECO_FACC_MOSTRUARIO;

                if (precoProducaoMostruario != null)
                {
                    txtPrecoProducao.Text = precoProducaoMostruario.ToString();
                    txtPrecoProducao.Enabled = false;
                }

            }

        }

        private bool ProdutoFoiSimulado(string produto, char mostruario, char finalizado)
        {

            if (prodController.ObterCustoSimulacao().Where(p => p.PRODUTO.Trim() == produto &&
                                                            p.MOSTRUARIO == mostruario &&
                                                            p.CUSTO_MOSTRUARIO == 'N' &&
                                                            p.FINALIZADO == finalizado).Count() > 0)
                return true;

            return false;
        }

    }
}
