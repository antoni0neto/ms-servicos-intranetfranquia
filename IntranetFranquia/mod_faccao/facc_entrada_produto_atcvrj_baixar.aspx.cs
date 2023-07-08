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
    public partial class facc_entrada_produto_atcvrj_baixar : System.Web.UI.Page
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

                if (_entrada.STATUS != 'A' && _entrada.STATUS != 'P' && _entrada.STATUS != 'V' && _entrada.STATUS != 'L' && _entrada.STATUS != 'C')
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

                if (_entrada.STATUS == 'A' || _entrada.STATUS == 'P')
                {
                    txtGradeEXP_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_EXP.ToString();
                    txtGradeXP_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_XP.ToString();
                    txtGradePP_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_PP.ToString();
                    txtGradeP_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_P.ToString();
                    txtGradeM_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_M.ToString();
                    txtGradeG_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_G.ToString();
                    txtGradeGG_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_GG.ToString();
                    txtGradeTotal_E.Text = _entrada.PROD_HB_SAIDA1.GRADE_TOTAL.ToString();
                }
                else
                {
                    txtGradeEXP_E.Text = _entrada.GRADE_EXP.ToString();
                    txtGradeXP_E.Text = _entrada.GRADE_XP.ToString();
                    txtGradePP_E.Text = _entrada.GRADE_PP.ToString();
                    txtGradeP_E.Text = _entrada.GRADE_P.ToString();
                    txtGradeM_E.Text = _entrada.GRADE_M.ToString();
                    txtGradeG_E.Text = _entrada.GRADE_G.ToString();
                    txtGradeGG_E.Text = _entrada.GRADE_GG.ToString();
                    txtGradeTotal_E.Text = _entrada.GRADE_TOTAL.ToString();
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

                //if (_entrada.RECEBIMENTO != null)
                //    txtRecebimento.Text = Convert.ToDateTime(_entrada.RECEBIMENTO).ToString("dd/MM/yyyy");
                if (tela == 22)
                    pnlOutro.Visible = false;

                if (tela == 21 || tela == 22 || ddlServico.SelectedValue == "6")
                {
                    ddlProdutoAcabado.SelectedValue = "S";
                    ddlProdutoAcabado.Enabled = false;
                }
                else
                {
                    if (_entrada.PRODUTO_ACABADO != null)
                        ddlProdutoAcabado.SelectedValue = (_entrada.PRODUTO_ACABADO == 'S') ? "S" : "N";
                }

                txtGradeTotal_A.Enabled = false;
                txtGradeTotal_V.Enabled = false;

                if (_entrada.PROD_HB_SAIDA1.PROD_HB1.COLECAO.Trim().Length == 2)
                {
                    txtNF.MaxLength = 6;
                    ddlFilial.SelectedValue = "000029";
                }

                var gradeAtacado = prodController.ObterGradeHB(_entrada.PROD_HB_SAIDA1.PROD_HB, 99);
                if (gradeAtacado != null)
                {
                    var gradeAtacadoRecebida = faccController.ObterEntradaQtdeHB(_entrada.PROD_HB_SAIDA1.PROD_HB, 99, tela);

                    txtGradeEXP_AF.Text = (gradeAtacado.GRADE_EXP - (gradeAtacadoRecebida == null ? 0 : gradeAtacadoRecebida.GRADE_EXP)).ToString();
                    txtGradeXP_AF.Text = (gradeAtacado.GRADE_XP - (gradeAtacadoRecebida == null ? 0 : gradeAtacadoRecebida.GRADE_XP)).ToString();
                    txtGradePP_AF.Text = (gradeAtacado.GRADE_PP - (gradeAtacadoRecebida == null ? 0 : gradeAtacadoRecebida.GRADE_PP)).ToString();
                    txtGradeP_AF.Text = (gradeAtacado.GRADE_P - (gradeAtacadoRecebida == null ? 0 : gradeAtacadoRecebida.GRADE_P)).ToString();
                    txtGradeM_AF.Text = (gradeAtacado.GRADE_M - (gradeAtacadoRecebida == null ? 0 : gradeAtacadoRecebida.GRADE_M)).ToString();
                    txtGradeG_AF.Text = (gradeAtacado.GRADE_G - (gradeAtacadoRecebida == null ? 0 : gradeAtacadoRecebida.GRADE_G)).ToString();
                    txtGradeGG_AF.Text = (gradeAtacado.GRADE_GG - (gradeAtacadoRecebida == null ? 0 : gradeAtacadoRecebida.GRADE_GG)).ToString();
                    if (gradeAtacadoRecebida == null)
                        txtGradeTotal_AF.Text = (gradeAtacado.GRADE_EXP + gradeAtacado.GRADE_XP + gradeAtacado.GRADE_PP + gradeAtacado.GRADE_P + gradeAtacado.GRADE_M + gradeAtacado.GRADE_G + gradeAtacado.GRADE_GG).ToString();
                    else
                        txtGradeTotal_AF.Text = (
                            (gradeAtacado.GRADE_EXP - gradeAtacadoRecebida.GRADE_EXP)
                            + (gradeAtacado.GRADE_XP - gradeAtacadoRecebida.GRADE_XP)
                            + (gradeAtacado.GRADE_PP - gradeAtacadoRecebida.GRADE_PP)
                            + (gradeAtacado.GRADE_P - gradeAtacadoRecebida.GRADE_P)
                            + (gradeAtacado.GRADE_M - gradeAtacadoRecebida.GRADE_M)
                            + (gradeAtacado.GRADE_G - gradeAtacadoRecebida.GRADE_G)
                            + (gradeAtacado.GRADE_GG - gradeAtacadoRecebida.GRADE_GG)).ToString();
                }

                var gradeReal = prodController.ObterGradeHB(_entrada.PROD_HB_SAIDA1.PROD_HB, 3);
                var gradeVarejo = new PROD_HB_GRADE();
                if (gradeReal != null)
                {
                    var gradeVarejoRecebida = faccController.ObterEntradaQtdeHB(_entrada.PROD_HB_SAIDA1.PROD_HB, 98, tela);

                    gradeVarejo.GRADE_EXP = (gradeReal.GRADE_EXP - (gradeAtacado == null ? 0 : gradeAtacado.GRADE_EXP) - (gradeVarejoRecebida == null ? 0 : gradeVarejoRecebida.GRADE_EXP));
                    gradeVarejo.GRADE_XP = (gradeReal.GRADE_XP - (gradeAtacado == null ? 0 : gradeAtacado.GRADE_XP) - (gradeVarejoRecebida == null ? 0 : gradeVarejoRecebida.GRADE_XP));
                    gradeVarejo.GRADE_PP = (gradeReal.GRADE_PP - (gradeAtacado == null ? 0 : gradeAtacado.GRADE_PP) - (gradeVarejoRecebida == null ? 0 : gradeVarejoRecebida.GRADE_PP));
                    gradeVarejo.GRADE_P = (gradeReal.GRADE_P - (gradeAtacado == null ? 0 : gradeAtacado.GRADE_P) - (gradeVarejoRecebida == null ? 0 : gradeVarejoRecebida.GRADE_P));
                    gradeVarejo.GRADE_M = (gradeReal.GRADE_M - (gradeAtacado == null ? 0 : gradeAtacado.GRADE_M) - (gradeVarejoRecebida == null ? 0 : gradeVarejoRecebida.GRADE_M));
                    gradeVarejo.GRADE_G = (gradeReal.GRADE_G - (gradeAtacado == null ? 0 : gradeAtacado.GRADE_G) - (gradeVarejoRecebida == null ? 0 : gradeVarejoRecebida.GRADE_G));
                    gradeVarejo.GRADE_GG = (gradeReal.GRADE_GG - (gradeAtacado == null ? 0 : gradeAtacado.GRADE_GG) - (gradeVarejoRecebida == null ? 0 : gradeVarejoRecebida.GRADE_GG));

                    txtGradeEXP_VF.Text = gradeVarejo.GRADE_EXP.ToString();
                    txtGradeXP_VF.Text = gradeVarejo.GRADE_XP.ToString();
                    txtGradePP_VF.Text = gradeVarejo.GRADE_PP.ToString();
                    txtGradeP_VF.Text = gradeVarejo.GRADE_P.ToString();
                    txtGradeM_VF.Text = gradeVarejo.GRADE_M.ToString();
                    txtGradeG_VF.Text = gradeVarejo.GRADE_G.ToString();
                    txtGradeGG_VF.Text = gradeVarejo.GRADE_GG.ToString();

                    txtGradeTotal_VF.Text = (
                        (gradeVarejo.GRADE_EXP)
                        + (gradeVarejo.GRADE_XP)
                        + (gradeVarejo.GRADE_PP)
                        + (gradeVarejo.GRADE_P)
                        + (gradeVarejo.GRADE_M)
                        + (gradeVarejo.GRADE_G)
                        + (gradeVarejo.GRADE_GG)).ToString();
                }

                DesativarCamposZerados(gradeAtacado, gradeVarejo);
                DesativarCamposZeradosOutros();

                string statusAnterior = "";
                if (_entrada.STATUS == 'A')
                    statusAnterior = "INICIAL";
                else if (_entrada.STATUS == 'P')
                    statusAnterior = "PENDENTE";
                else if (_entrada.STATUS == 'S')
                    statusAnterior = "SEG. QUALIDADE";
                else if (_entrada.STATUS == 'L')
                    statusAnterior = "LAVANDERIA";
                else if (_entrada.STATUS == 'C')
                    statusAnterior = "CONSERTO";
                else if (_entrada.STATUS == 'V')
                    statusAnterior = "REV. SEG. QUALIDADE";
                txtStatusAnterior.Text = statusAnterior;
                hidStatusAnterior.Value = _entrada.STATUS.ToString();

                CarregarStatus(tela);

                // validar definicao de ficha
                var hb = prodController.ObterHB(_entrada.PROD_HB_SAIDA1.PROD_HB);
                if (hb != null)
                {
                    if (hb.DATA_IMP_FIC_LOGISTICA == null)
                    {
                        btSalvar.Enabled = false;
                        labErro.Text = "A Grade do Atacado não foi definida.";
                    }
                }

                CarregarNomeGrade(_entrada.PROD_HB_SAIDA1.PROD_HB1);

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        private void DesativarCamposZerados(PROD_HB_GRADE atacadoGrade, PROD_HB_GRADE varejoGrade)
        {
            //ATACADO
            if (Convert.ToInt32(txtGradeEXP_AF.Text) <= 0)
            {
                txtGradeEXP_A.Text = "0";
                txtGradeEXP_A.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeXP_AF.Text) <= 0)
            {
                txtGradeXP_A.Text = "0";
                txtGradeXP_A.Enabled = false;
            }
            if (Convert.ToInt32(txtGradePP_AF.Text) <= 0)
            {
                txtGradePP_A.Text = "0";
                txtGradePP_A.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeP_AF.Text) <= 0)
            {
                txtGradeP_A.Text = "0";
                txtGradeP_A.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeM_AF.Text) <= 0)
            {
                txtGradeM_A.Text = "0";
                txtGradeM_A.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeG_AF.Text) <= 0)
            {
                txtGradeG_A.Text = "0";
                txtGradeG_A.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeGG_AF.Text) <= 0)
            {
                txtGradeGG_A.Text = "0";
                txtGradeGG_A.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeTotal_AF.Text) <= 0)
            {
                txtGradeTotal_A.Text = "0";
                txtGradeTotal_A.Enabled = false;
            }

            //VAREJO
            if (Convert.ToInt32(txtGradeEXP_VF.Text) <= 0)
            {
                txtGradeEXP_V.Text = "0";
                txtGradeEXP_V.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeXP_VF.Text) <= 0)
            {
                txtGradeXP_V.Text = "0";
                txtGradeXP_V.Enabled = false;
            }
            if (Convert.ToInt32(txtGradePP_VF.Text) <= 0)
            {
                txtGradePP_V.Text = "0";
                txtGradePP_V.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeP_VF.Text) <= 0)
            {
                txtGradeP_V.Text = "0";
                txtGradeP_V.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeM_VF.Text) <= 0)
            {
                txtGradeM_V.Text = "0";
                txtGradeM_V.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeG_VF.Text) <= 0)
            {
                txtGradeG_V.Text = "0";
                txtGradeG_V.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeGG_VF.Text) <= 0)
            {
                txtGradeGG_V.Text = "0";
                txtGradeGG_V.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeTotal_VF.Text) <= 0)
            {
                txtGradeTotal_V.Text = "0";
                txtGradeTotal_V.Enabled = false;
            }

        }
        private void DesativarCamposZeradosOutros()
        {
            if (Convert.ToInt32(txtGradeEXP_E.Text) <= 0)
            {
                txtGradeEXPSeg.Text = "0";
                txtGradeEXPSeg.Enabled = false;
                txtGradeEXPLav.Text = "0";
                txtGradeEXPLav.Enabled = false;
                txtGradeEXPCons.Text = "0";
                txtGradeEXPCons.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeXP_E.Text) <= 0)
            {
                txtGradeXPSeg.Text = "0";
                txtGradeXPSeg.Enabled = false;
                txtGradeXPLav.Text = "0";
                txtGradeXPLav.Enabled = false;
                txtGradeXPCons.Text = "0";
                txtGradeXPCons.Enabled = false;
            }
            if (Convert.ToInt32(txtGradePP_E.Text) <= 0)
            {
                txtGradePPSeg.Text = "0";
                txtGradePPSeg.Enabled = false;
                txtGradePPLav.Text = "0";
                txtGradePPLav.Enabled = false;
                txtGradePPCons.Text = "0";
                txtGradePPCons.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeP_E.Text) <= 0)
            {
                txtGradePSeg.Text = "0";
                txtGradePSeg.Enabled = false;
                txtGradePLav.Text = "0";
                txtGradePLav.Enabled = false;
                txtGradePCons.Text = "0";
                txtGradePCons.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeM_E.Text) <= 0)
            {
                txtGradeMSeg.Text = "0";
                txtGradeMSeg.Enabled = false;
                txtGradeMLav.Text = "0";
                txtGradeMLav.Enabled = false;
                txtGradeMCons.Text = "0";
                txtGradeMCons.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeG_E.Text) <= 0)
            {
                txtGradeGSeg.Text = "0";
                txtGradeGSeg.Enabled = false;
                txtGradeGLav.Text = "0";
                txtGradeGLav.Enabled = false;
                txtGradeGCons.Text = "0";
                txtGradeGCons.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeGG_E.Text) <= 0)
            {
                txtGradeGGSeg.Text = "0";
                txtGradeGGSeg.Enabled = false;
                txtGradeGGLav.Text = "0";
                txtGradeGGLav.Enabled = false;
                txtGradeGGCons.Text = "0";
                txtGradeGGCons.Enabled = false;
            }
            if (Convert.ToInt32(txtGradeTotal_E.Text) <= 0)
            {
                txtGradeTotalSeg.Text = "0";
                txtGradeTotalSeg.Enabled = false;
                txtGradeTotalLav.Text = "0";
                txtGradeTotalLav.Enabled = false;
                txtGradeTotalCons.Text = "0";
                txtGradeTotalCons.Enabled = false;
            }
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
        private void CarregarStatus(int tela)
        {
            //List<ListItem> l = new List<ListItem>();

            //l.Add(new ListItem("Selecione", ""));
            //l.Add(new ListItem("FINALIZADO", "B"));
            //if (tela == 21)
            //{
            //    l.Add(new ListItem("SEGUNDA QUALIDADE", "S"));
            //    l.Add(new ListItem("LAVANDERIA", "L"));
            //    l.Add(new ListItem("CONSERTO", "C"));
            //}

            //ddlStatus.DataSource = l;
            //ddlStatus.DataBind();
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

            //labQtdeNota.ForeColor = _OK;
            //if (txtQtdeNota.Text.Trim() == "")
            //{
            //    labQtdeNota.ForeColor = _notOK;
            //    retorno = false;
            //}

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

        private void CriarSaidaOutro(int tipo, PROD_HB_ENTRADA _entrada)
        {
            //tipo = 1 - Rev Segunda Qualidade
            //tipo = 2 - Lavanderia
            //tipo = 3 - Conserto

            PROD_HB_SAIDA _saidaNova = new PROD_HB_SAIDA();
            var saidaAnterior = faccController.ObterSaidaHB(_entrada.PROD_HB_SAIDA1.CODIGO);

            _saidaNova = new PROD_HB_SAIDA();
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

            //segunda qualidade
            if (tipo == 1)
            {
                _saidaNova.GRADE_EXP = (txtGradeEXPSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeEXPSeg.Text);
                _saidaNova.GRADE_XP = (txtGradeXPSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeXPSeg.Text);
                _saidaNova.GRADE_PP = (txtGradePPSeg.Text == "") ? 0 : Convert.ToInt32(txtGradePPSeg.Text);
                _saidaNova.GRADE_P = (txtGradePSeg.Text == "") ? 0 : Convert.ToInt32(txtGradePSeg.Text);
                _saidaNova.GRADE_M = (txtGradeMSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeMSeg.Text);
                _saidaNova.GRADE_G = (txtGradeGSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeGSeg.Text);
                _saidaNova.GRADE_GG = (txtGradeGGSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeGGSeg.Text);
                _saidaNova.GRADE_TOTAL = (txtGradeTotalSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeTotalSeg.Text);
            }
            //lavanderia
            else if (tipo == 2)
            {
                _saidaNova.GRADE_EXP = (txtGradeEXPLav.Text == "") ? 0 : Convert.ToInt32(txtGradeEXPLav.Text);
                _saidaNova.GRADE_XP = (txtGradeXPLav.Text == "") ? 0 : Convert.ToInt32(txtGradeXPLav.Text);
                _saidaNova.GRADE_PP = (txtGradePPLav.Text == "") ? 0 : Convert.ToInt32(txtGradePPLav.Text);
                _saidaNova.GRADE_P = (txtGradePLav.Text == "") ? 0 : Convert.ToInt32(txtGradePLav.Text);
                _saidaNova.GRADE_M = (txtGradeMLav.Text == "") ? 0 : Convert.ToInt32(txtGradeMLav.Text);
                _saidaNova.GRADE_G = (txtGradeGLav.Text == "") ? 0 : Convert.ToInt32(txtGradeGLav.Text);
                _saidaNova.GRADE_GG = (txtGradeGGLav.Text == "") ? 0 : Convert.ToInt32(txtGradeGGLav.Text);
                _saidaNova.GRADE_TOTAL = (txtGradeTotalLav.Text == "") ? 0 : Convert.ToInt32(txtGradeTotalLav.Text);
            }
            //conserto
            else if (tipo == 3)
            {
                _saidaNova.GRADE_EXP = (txtGradeEXPCons.Text == "") ? 0 : Convert.ToInt32(txtGradeEXPCons.Text);
                _saidaNova.GRADE_XP = (txtGradeXPCons.Text == "") ? 0 : Convert.ToInt32(txtGradeXPCons.Text);
                _saidaNova.GRADE_PP = (txtGradePPCons.Text == "") ? 0 : Convert.ToInt32(txtGradePPCons.Text);
                _saidaNova.GRADE_P = (txtGradePCons.Text == "") ? 0 : Convert.ToInt32(txtGradePCons.Text);
                _saidaNova.GRADE_M = (txtGradeMCons.Text == "") ? 0 : Convert.ToInt32(txtGradeMCons.Text);
                _saidaNova.GRADE_G = (txtGradeGCons.Text == "") ? 0 : Convert.ToInt32(txtGradeGCons.Text);
                _saidaNova.GRADE_GG = (txtGradeGGCons.Text == "") ? 0 : Convert.ToInt32(txtGradeGGCons.Text);
                _saidaNova.GRADE_TOTAL = (txtGradeTotalCons.Text == "") ? 0 : Convert.ToInt32(txtGradeTotalCons.Text);
            }

            _saidaNova.SALDO = 'N';

            int codigoSaidaNova = faccController.InserirSaidaHB(_saidaNova);

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

            if (tipo == 1)
                _entradaFaltante.STATUS = 'V';
            else if (tipo == 2)
                _entradaFaltante.STATUS = 'L';
            else if (tipo == 3)
                _entradaFaltante.STATUS = 'C';

            _entradaFaltante.DATA_INCLUSAO = DateTime.Now;
            faccController.InserirEntradaHB(_entradaFaltante);

        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

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

                if (ddlStatus.SelectedValue == "B" &&
                    (txtGradeTotal_A.Text == "" || txtGradeTotal_A.Text == "0") &&
                    (txtGradeTotal_V.Text == "" || txtGradeTotal_V.Text == "0") &&
                    (txtGradeTotalSeg.Text == "" || txtGradeTotalSeg.Text == "0") &&
                    (txtGradeTotalLav.Text == "" || txtGradeTotalLav.Text == "0") &&
                    (txtGradeTotalCons.Text == "" || txtGradeTotalCons.Text == "0")
                    )
                {
                    labErro.Text = "Informe a grade recebida.";
                    return;
                }


                // NAO VALIDAR QUANDO O USUARIO FOR O RODRIGO - 2201 / 1144
                if(codigoUsuario != 2201 && codigoUsuario != 1144)
                {
                    if (ddlStatus.SelectedValue == "B" && (Convert.ToInt32(txtGradeTotal_A.Text) > Convert.ToInt32(txtGradeTotal_AF.Text)))
                    {
                        labErro.Text = "Grade do Atacado não pode ser maior que a Grade do Atacado Faltante";
                        return;
                    }

                    if (ddlStatus.SelectedValue == "B" && (Convert.ToInt32(txtGradeTotal_V.Text) > Convert.ToInt32(txtGradeTotal_VF.Text)))
                    {
                        labErro.Text = "Grade do Varejo não pode ser maior que a Grade do Varejo Faltante";
                        return;
                    }

                    if ((Convert.ToInt32(txtGradeTotal_V.Text) + Convert.ToInt32(txtGradeTotal_A.Text)) > Convert.ToInt32(txtGradeTotal_E.Text))
                    {
                        labErro.Text = "Grade do Varejo + Grade Atacado não pode ser maior que a Grade a Receber.";
                        return;
                    }
                }


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
                _entrada.USUARIO_RECEBIMENTO = codigoUsuario;

                var gradeAtacado = new PROD_HB_ENTRADA_QTDE();
                if (txtGradeTotal_A.Text.Trim() != "")
                {
                    if (txtGradeEXP_A.Text.Trim() != "")
                        gradeAtacado.GRADE_EXP = Convert.ToInt32(txtGradeEXP_A.Text.Trim());
                    if (txtGradeXP_A.Text.Trim() != "")
                        gradeAtacado.GRADE_XP = Convert.ToInt32(txtGradeXP_A.Text.Trim());
                    if (txtGradePP_A.Text.Trim() != "")
                        gradeAtacado.GRADE_PP = Convert.ToInt32(txtGradePP_A.Text.Trim());
                    if (txtGradeP_A.Text.Trim() != "")
                        gradeAtacado.GRADE_P = Convert.ToInt32(txtGradeP_A.Text.Trim());
                    if (txtGradeM_A.Text.Trim() != "")
                        gradeAtacado.GRADE_M = Convert.ToInt32(txtGradeM_A.Text.Trim());
                    if (txtGradeG_A.Text.Trim() != "")
                        gradeAtacado.GRADE_G = Convert.ToInt32(txtGradeG_A.Text.Trim());
                    if (txtGradeGG_A.Text.Trim() != "")
                        gradeAtacado.GRADE_GG = Convert.ToInt32(txtGradeGG_A.Text.Trim());

                    int gradeTotal = 0;

                    gradeTotal = (gradeAtacado.GRADE_EXP + gradeAtacado.GRADE_XP + gradeAtacado.GRADE_PP + gradeAtacado.GRADE_P + gradeAtacado.GRADE_M + gradeAtacado.GRADE_G + gradeAtacado.GRADE_GG);
                    gradeAtacado.GRADE_TOTAL = gradeTotal;
                }

                var gradeVarejo = new PROD_HB_ENTRADA_QTDE();
                if (txtGradeTotal_V.Text.Trim() != "")
                {
                    if (txtGradeEXP_V.Text.Trim() != "")
                        gradeVarejo.GRADE_EXP = Convert.ToInt32(txtGradeEXP_V.Text.Trim());
                    if (txtGradeXP_V.Text.Trim() != "")
                        gradeVarejo.GRADE_XP = Convert.ToInt32(txtGradeXP_V.Text.Trim());
                    if (txtGradePP_V.Text.Trim() != "")
                        gradeVarejo.GRADE_PP = Convert.ToInt32(txtGradePP_V.Text.Trim());
                    if (txtGradeP_V.Text.Trim() != "")
                        gradeVarejo.GRADE_P = Convert.ToInt32(txtGradeP_V.Text.Trim());
                    if (txtGradeM_V.Text.Trim() != "")
                        gradeVarejo.GRADE_M = Convert.ToInt32(txtGradeM_V.Text.Trim());
                    if (txtGradeG_V.Text.Trim() != "")
                        gradeVarejo.GRADE_G = Convert.ToInt32(txtGradeG_V.Text.Trim());
                    if (txtGradeGG_V.Text.Trim() != "")
                        gradeVarejo.GRADE_GG = Convert.ToInt32(txtGradeGG_V.Text.Trim());

                    int gradeTotal = 0;

                    gradeTotal = (gradeVarejo.GRADE_EXP + gradeVarejo.GRADE_XP + gradeVarejo.GRADE_PP + gradeVarejo.GRADE_P + gradeVarejo.GRADE_M + gradeVarejo.GRADE_G + gradeVarejo.GRADE_GG);
                    gradeVarejo.GRADE_TOTAL = gradeTotal;
                }

                _entrada.GRADE_EXP = gradeAtacado.GRADE_EXP + gradeVarejo.GRADE_EXP;
                _entrada.GRADE_XP = gradeAtacado.GRADE_XP + gradeVarejo.GRADE_XP;
                _entrada.GRADE_PP = gradeAtacado.GRADE_PP + gradeVarejo.GRADE_PP;
                _entrada.GRADE_P = gradeAtacado.GRADE_P + gradeVarejo.GRADE_P;
                _entrada.GRADE_M = gradeAtacado.GRADE_M + gradeVarejo.GRADE_M;
                _entrada.GRADE_G = gradeAtacado.GRADE_G + gradeVarejo.GRADE_G;
                _entrada.GRADE_GG = gradeAtacado.GRADE_GG + gradeVarejo.GRADE_GG;
                _entrada.GRADE_TOTAL = gradeAtacado.GRADE_TOTAL + gradeVarejo.GRADE_TOTAL;

                _entrada.STATUS = Convert.ToChar(ddlStatus.SelectedValue);
                _entrada.QTDE_NOTA = 0;
                _entrada.DATA_DISTRIBUICAO = DateTime.Now;

                faccController.AtualizarEntradaHB(_entrada);

                if (ddlStatus.SelectedValue == "B")
                {
                    if (gradeAtacado.GRADE_TOTAL > 0)
                    {
                        gradeAtacado.PROD_HB_ENTRADA = _entrada.CODIGO;
                        gradeAtacado.PROD_PROCESSO = 99;
                        gradeAtacado.STATUS = Convert.ToChar(ddlStatus.SelectedValue);

                        if (hidControleInsercao.Value == "")
                            faccController.InserirEntradaQtde(gradeAtacado);
                    }
                    if (gradeVarejo.GRADE_TOTAL > 0)
                    {
                        gradeVarejo.PROD_HB_ENTRADA = _entrada.CODIGO;
                        gradeVarejo.PROD_PROCESSO = 98;
                        gradeVarejo.STATUS = Convert.ToChar(ddlStatus.SelectedValue);
                        if (hidControleInsercao.Value == "")
                            faccController.InserirEntradaQtde(gradeVarejo);
                    }

                    // se baixar acabamento, gerar logistica
                    if (hidTela.Value == "21" && ((gradeAtacado.GRADE_TOTAL > 0) || (gradeVarejo.GRADE_TOTAL > 0)))
                    {
                        //inserir processo logistica
                        var saidaLogistica = new PROD_HB_SAIDA();
                        saidaLogistica.PROD_HB = _entrada.PROD_HB_SAIDA1.PROD_HB;
                        saidaLogistica.CNPJ = _entrada.PROD_HB_SAIDA1.CNPJ;
                        saidaLogistica.FORNECEDOR = _entrada.PROD_HB_SAIDA1.FORNECEDOR;
                        saidaLogistica.CODIGO_FILIAL = _entrada.PROD_HB_SAIDA1.CODIGO_FILIAL;
                        saidaLogistica.NF_SAIDA = "";
                        saidaLogistica.SERIE_NF = "";
                        saidaLogistica.EMISSAO = DateTime.Now;
                        saidaLogistica.USUARIO_EMISSAO = _entrada.USUARIO_RECEBIMENTO;
                        saidaLogistica.VOLUME = _entrada.PROD_HB_SAIDA1.VOLUME;
                        saidaLogistica.CUSTO = _entrada.PROD_HB_SAIDA1.CUSTO;
                        saidaLogistica.PRECO = _entrada.PROD_HB_SAIDA1.PRECO;
                        saidaLogistica.PROD_SERVICO = 8;
                        saidaLogistica.TIPO = _entrada.PROD_HB_SAIDA1.TIPO;
                        saidaLogistica.USUARIO_LIBERACAO = _entrada.USUARIO_RECEBIMENTO;
                        saidaLogistica.DATA_LIBERACAO = DateTime.Now;
                        saidaLogistica.DATA_INCLUSAO = DateTime.Now;
                        saidaLogistica.PROD_PROCESSO = 22;
                        saidaLogistica.GRADE_EXP = (gradeAtacado == null ? 0 : gradeAtacado.GRADE_EXP) + (gradeVarejo == null ? 0 : gradeVarejo.GRADE_EXP);
                        saidaLogistica.GRADE_XP = (gradeAtacado == null ? 0 : gradeAtacado.GRADE_XP) + (gradeVarejo == null ? 0 : gradeVarejo.GRADE_XP);
                        saidaLogistica.GRADE_PP = (gradeAtacado == null ? 0 : gradeAtacado.GRADE_PP) + (gradeVarejo == null ? 0 : gradeVarejo.GRADE_PP);
                        saidaLogistica.GRADE_P = (gradeAtacado == null ? 0 : gradeAtacado.GRADE_P) + (gradeVarejo == null ? 0 : gradeVarejo.GRADE_P);
                        saidaLogistica.GRADE_M = (gradeAtacado == null ? 0 : gradeAtacado.GRADE_M) + (gradeVarejo == null ? 0 : gradeVarejo.GRADE_M);
                        saidaLogistica.GRADE_G = (gradeAtacado == null ? 0 : gradeAtacado.GRADE_G) + (gradeVarejo == null ? 0 : gradeVarejo.GRADE_G);
                        saidaLogistica.GRADE_GG = (gradeAtacado == null ? 0 : gradeAtacado.GRADE_GG) + (gradeVarejo == null ? 0 : gradeVarejo.GRADE_GG);
                        saidaLogistica.GRADE_TOTAL = (gradeAtacado == null ? 0 : gradeAtacado.GRADE_TOTAL) + (gradeVarejo == null ? 0 : gradeVarejo.GRADE_TOTAL);
                        saidaLogistica.SALDO = 'S';
                        saidaLogistica.FORNECEDOR_SUB = _entrada.PROD_HB_SAIDA1.FORNECEDOR_SUB;

                        int codigoSaidaLogistica = 0;
                        if (hidControleInsercao.Value == "")
                            codigoSaidaLogistica = faccController.InserirSaidaHB(saidaLogistica);

                        var entradaLogistica = new PROD_HB_ENTRADA();
                        entradaLogistica.PROD_HB_SAIDA = codigoSaidaLogistica;
                        entradaLogistica.GRADE_EXP = 0;
                        entradaLogistica.GRADE_XP = 0;
                        entradaLogistica.GRADE_PP = 0;
                        entradaLogistica.GRADE_P = 0;
                        entradaLogistica.GRADE_M = 0;
                        entradaLogistica.GRADE_G = 0;
                        entradaLogistica.GRADE_GG = 0;
                        entradaLogistica.GRADE_TOTAL = 0;
                        entradaLogistica.STATUS = 'A';
                        entradaLogistica.DATA_INCLUSAO = DateTime.Now;

                        if (hidControleInsercao.Value == "")
                            faccController.InserirEntradaHB(entradaLogistica);
                    }
                }

                //Criar Saida Outros
                if (txtGradeTotalSeg.Text != "" && txtGradeTotalSeg.Text != "0")
                {
                    if (hidControleInsercao.Value == "")
                        CriarSaidaOutro(1, _entrada);
                }

                if (txtGradeTotalLav.Text != "" && txtGradeTotalLav.Text != "0")
                {
                    if (hidControleInsercao.Value == "")
                        CriarSaidaOutro(2, _entrada);
                }

                if (txtGradeTotalCons.Text != "" && txtGradeTotalCons.Text != "0")
                {
                    if (hidControleInsercao.Value == "")
                        CriarSaidaOutro(3, _entrada);
                }

                bool temEntradaVarejoAtacado = false;
                if (_entrada.GRADE_TOTAL > 0)
                    temEntradaVarejoAtacado = true;

                if (
                    (
                    //ACABAMENTO
                    ((_entrada.GRADE_TOTAL < _entrada.PROD_HB_SAIDA1.GRADE_TOTAL) && (hidTela.Value != "22") && ddlStatus.SelectedValue == "B" && (hidStatusAnterior.Value == "A" || hidStatusAnterior.Value == "P")) ||
                    //LOGISTICA
                    ((_entrada.GRADE_TOTAL < Convert.ToInt32(txtGradeTotal_E.Text)) && ddlStatus.SelectedValue == "B" && hidTela.Value == "22")
                    )
                    )
                {
                    //Gera nova saida
                    int codigoSaidaNova = 0;
                    PROD_HB_SAIDA _saidaNova = null;

                    var saidaAnterior = faccController.ObterSaidaHB(_entrada.PROD_HB_SAIDA1.CODIGO);

                    _saidaNova = new PROD_HB_SAIDA();
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

                    //if ((hidStatusAnterior.Value == "S" || hidStatusAnterior.Value == "L" || hidStatusAnterior.Value == "C"))
                    //{
                    //    _saidaNova.GRADE_EXP = Convert.ToInt32(txtGradeEXP_E.Text) - _entrada.GRADE_EXP;
                    //    _saidaNova.GRADE_XP = Convert.ToInt32(txtGradeXP_E.Text) - _entrada.GRADE_XP;
                    //    _saidaNova.GRADE_PP = Convert.ToInt32(txtGradePP_E.Text) - _entrada.GRADE_PP;
                    //    _saidaNova.GRADE_P = Convert.ToInt32(txtGradeP_E.Text) - _entrada.GRADE_P;
                    //    _saidaNova.GRADE_M = Convert.ToInt32(txtGradeM_E.Text) - _entrada.GRADE_M;
                    //    _saidaNova.GRADE_G = Convert.ToInt32(txtGradeG_E.Text) - _entrada.GRADE_G;
                    //    _saidaNova.GRADE_GG = Convert.ToInt32(txtGradeGG_E.Text) - _entrada.GRADE_GG;
                    //    _saidaNova.GRADE_TOTAL = Convert.ToInt32(txtGradeTotal_E.Text) - _entrada.GRADE_TOTAL;
                    //}
                    //else
                    //{
                    _saidaNova.GRADE_EXP = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_EXP) - _entrada.GRADE_EXP -
                        ((txtGradeEXPSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeEXPSeg.Text)) -
                        ((txtGradeEXPLav.Text == "") ? 0 : Convert.ToInt32(txtGradeEXPLav.Text)) -
                        ((txtGradeEXPCons.Text == "") ? 0 : Convert.ToInt32(txtGradeEXPCons.Text));

                    _saidaNova.GRADE_XP = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_XP) - _entrada.GRADE_XP -
                        ((txtGradeXPSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeXPSeg.Text)) -
                        ((txtGradeXPLav.Text == "") ? 0 : Convert.ToInt32(txtGradeXPLav.Text)) -
                        ((txtGradeXPCons.Text == "") ? 0 : Convert.ToInt32(txtGradeXPCons.Text));

                    _saidaNova.GRADE_PP = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_PP) - _entrada.GRADE_PP -
                        ((txtGradePPSeg.Text == "") ? 0 : Convert.ToInt32(txtGradePPSeg.Text)) -
                        ((txtGradePPLav.Text == "") ? 0 : Convert.ToInt32(txtGradePPLav.Text)) -
                        ((txtGradePPCons.Text == "") ? 0 : Convert.ToInt32(txtGradePPCons.Text));

                    _saidaNova.GRADE_P = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_P) - _entrada.GRADE_P -
                        ((txtGradePSeg.Text == "") ? 0 : Convert.ToInt32(txtGradePSeg.Text)) -
                        ((txtGradePLav.Text == "") ? 0 : Convert.ToInt32(txtGradePLav.Text)) -
                        ((txtGradePCons.Text == "") ? 0 : Convert.ToInt32(txtGradePCons.Text));

                    _saidaNova.GRADE_M = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_M) - _entrada.GRADE_M -
                        ((txtGradeMSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeMSeg.Text)) -
                        ((txtGradeMLav.Text == "") ? 0 : Convert.ToInt32(txtGradeMLav.Text)) -
                        ((txtGradeMCons.Text == "") ? 0 : Convert.ToInt32(txtGradeMCons.Text));

                    _saidaNova.GRADE_G = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_G) - _entrada.GRADE_G -
                        ((txtGradeGSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeGSeg.Text)) -
                        ((txtGradeGLav.Text == "") ? 0 : Convert.ToInt32(txtGradeGLav.Text)) -
                        ((txtGradeGCons.Text == "") ? 0 : Convert.ToInt32(txtGradeGCons.Text));

                    _saidaNova.GRADE_GG = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_GG) - _entrada.GRADE_GG -
                        ((txtGradeGGSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeGGSeg.Text)) -
                        ((txtGradeGGLav.Text == "") ? 0 : Convert.ToInt32(txtGradeGGLav.Text)) -
                        ((txtGradeGGCons.Text == "") ? 0 : Convert.ToInt32(txtGradeGGCons.Text));

                    _saidaNova.GRADE_TOTAL = Convert.ToInt32(_entrada.PROD_HB_SAIDA1.GRADE_TOTAL) - _entrada.GRADE_TOTAL -
                        ((txtGradeTotalSeg.Text == "") ? 0 : Convert.ToInt32(txtGradeTotalSeg.Text)) -
                        ((txtGradeTotalLav.Text == "") ? 0 : Convert.ToInt32(txtGradeTotalLav.Text)) -
                        ((txtGradeTotalCons.Text == "") ? 0 : Convert.ToInt32(txtGradeTotalCons.Text));
                    //}

                    _saidaNova.SALDO = 'N';

                    if (hidControleInsercao.Value == "")
                        codigoSaidaNova = faccController.InserirSaidaHB(_saidaNova);

                    PROD_HB_ENTRADA _entradaFaltante = new PROD_HB_ENTRADA();
                    _entradaFaltante.PROD_HB_SAIDA = codigoSaidaNova;

                    //if ((hidStatusAnterior.Value == "S" || hidStatusAnterior.Value == "L" || hidStatusAnterior.Value == "C"))
                    //{
                    //    _entradaFaltante.GRADE_EXP = Convert.ToInt32(txtGradeEXP_E.Text) - _entrada.GRADE_EXP;
                    //    _entradaFaltante.GRADE_XP = Convert.ToInt32(txtGradeXP_E.Text) - _entrada.GRADE_XP;
                    //    _entradaFaltante.GRADE_PP = Convert.ToInt32(txtGradePP_E.Text) - _entrada.GRADE_PP;
                    //    _entradaFaltante.GRADE_P = Convert.ToInt32(txtGradeP_E.Text) - _entrada.GRADE_P;
                    //    _entradaFaltante.GRADE_M = Convert.ToInt32(txtGradeM_E.Text) - _entrada.GRADE_M;
                    //    _entradaFaltante.GRADE_G = Convert.ToInt32(txtGradeG_E.Text) - _entrada.GRADE_G;
                    //    _entradaFaltante.GRADE_GG = Convert.ToInt32(txtGradeGG_E.Text) - _entrada.GRADE_GG;
                    //    _entradaFaltante.GRADE_TOTAL = Convert.ToInt32(txtGradeTotal_E.Text) - _entrada.GRADE_TOTAL;
                    //}
                    //else
                    //{
                    _entradaFaltante.GRADE_EXP = 0;
                    _entradaFaltante.GRADE_XP = 0;
                    _entradaFaltante.GRADE_PP = 0;
                    _entradaFaltante.GRADE_P = 0;
                    _entradaFaltante.GRADE_M = 0;
                    _entradaFaltante.GRADE_G = 0;
                    _entradaFaltante.GRADE_GG = 0;
                    _entradaFaltante.GRADE_TOTAL = 0;
                    //}

                    if (hidStatusAnterior.Value == "A")
                        hidStatusAnterior.Value = "P";
                    _entradaFaltante.STATUS = Convert.ToChar(hidStatusAnterior.Value);
                    _entradaFaltante.DATA_INCLUSAO = DateTime.Now;

                    if (hidControleInsercao.Value == "")
                        faccController.InserirEntradaHB(_entradaFaltante);

                }
                else
                {
                    temEntradaVarejoAtacado = false;


                    //// ENTRADA SOMENTE DE SEGUNDA QUALIDADE/LAVANDERIA/CONSERTO
                    //var saidaAnterior = faccController.ObterSaidaHB(_entrada.PROD_HB_SAIDA1.CODIGO);
                    //if (segQualidade)
                    //{
                    //    saidaAnterior.GRADE_EXP = saidaAnterior.GRADE_EXP - Convert.ToInt32(txtGradeEXPSeg.Text);
                    //    saidaAnterior.GRADE_XP = saidaAnterior.GRADE_XP - Convert.ToInt32(txtGradeXPSeg.Text);
                    //    saidaAnterior.GRADE_PP = saidaAnterior.GRADE_PP - Convert.ToInt32(txtGradePPSeg.Text);
                    //    saidaAnterior.GRADE_P = saidaAnterior.GRADE_P - Convert.ToInt32(txtGradePSeg.Text);
                    //    saidaAnterior.GRADE_M = saidaAnterior.GRADE_M - Convert.ToInt32(txtGradeMSeg.Text);
                    //    saidaAnterior.GRADE_G = saidaAnterior.GRADE_G - Convert.ToInt32(txtGradeGSeg.Text);
                    //    saidaAnterior.GRADE_GG = saidaAnterior.GRADE_GG - Convert.ToInt32(txtGradeGGSeg.Text);
                    //    saidaAnterior.GRADE_TOTAL = saidaAnterior.GRADE_TOTAL - Convert.ToInt32(txtGradeTotalSeg.Text);
                    //}
                    //if (lavanderia)
                    //{
                    //    saidaAnterior.GRADE_EXP = saidaAnterior.GRADE_EXP - Convert.ToInt32(txtGradeEXPLav.Text);
                    //    saidaAnterior.GRADE_XP = saidaAnterior.GRADE_XP - Convert.ToInt32(txtGradeXPLav.Text);
                    //    saidaAnterior.GRADE_PP = saidaAnterior.GRADE_PP - Convert.ToInt32(txtGradePPLav.Text);
                    //    saidaAnterior.GRADE_P = saidaAnterior.GRADE_P - Convert.ToInt32(txtGradePLav.Text);
                    //    saidaAnterior.GRADE_M = saidaAnterior.GRADE_M - Convert.ToInt32(txtGradeMLav.Text);
                    //    saidaAnterior.GRADE_G = saidaAnterior.GRADE_G - Convert.ToInt32(txtGradeGLav.Text);
                    //    saidaAnterior.GRADE_GG = saidaAnterior.GRADE_GG - Convert.ToInt32(txtGradeGGLav.Text);
                    //    saidaAnterior.GRADE_TOTAL = saidaAnterior.GRADE_TOTAL - Convert.ToInt32(txtGradeTotalLav.Text);
                    //}
                    //if (conserto)
                    //{
                    //    saidaAnterior.GRADE_EXP = saidaAnterior.GRADE_EXP - Convert.ToInt32(txtGradeEXPCons.Text);
                    //    saidaAnterior.GRADE_XP = saidaAnterior.GRADE_XP - Convert.ToInt32(txtGradeXPCons.Text);
                    //    saidaAnterior.GRADE_PP = saidaAnterior.GRADE_PP - Convert.ToInt32(txtGradePPCons.Text);
                    //    saidaAnterior.GRADE_P = saidaAnterior.GRADE_P - Convert.ToInt32(txtGradePCons.Text);
                    //    saidaAnterior.GRADE_M = saidaAnterior.GRADE_M - Convert.ToInt32(txtGradeMCons.Text);
                    //    saidaAnterior.GRADE_G = saidaAnterior.GRADE_G - Convert.ToInt32(txtGradeGCons.Text);
                    //    saidaAnterior.GRADE_GG = saidaAnterior.GRADE_GG - Convert.ToInt32(txtGradeGGCons.Text);
                    //    saidaAnterior.GRADE_TOTAL = saidaAnterior.GRADE_TOTAL - Convert.ToInt32(txtGradeTotalCons.Text);
                    //}

                    //faccController.AtualizarSaidaHB(saidaAnterior);
                }

                //Tentativa de controle para nao inserir 2x
                hidControleInsercao.Value = "X";

                if (Constante.enviarEmail)
                    if (ddlStatus.SelectedValue == "B" && temEntradaVarejoAtacado)
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
            //string status = ddlStatus.SelectedValue.Trim();

            //divStatus.Visible = false;
            //if (status == "B")
            //    divStatus.Visible = true;
        }

        #region "EMAIL"
        private void EnviarEmail(PROD_HB_ENTRADA _entrada, int tela)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            string assunto = "";
            string titulo = "";
            string subTitulo = "";

            //if (_entrada.NF_ENTRADA == null || _entrada.NF_ENTRADA.Trim() == "")
            //  subTitulo = " S/ NOTA";

            if (_entrada.PRODUTO_ACABADO == 'S')
                titulo = "ENT PRODUTO ACABADO" + subTitulo;
            else
                titulo = "ENT PRODUTO NÃO ACABADO" + subTitulo;

            if (_entrada.STATUS == 'P')
                titulo = "ENT PRODUTO PENDENTE" + subTitulo;

            assunto = "Intranet: " + titulo + " - O.P.: " + _entrada.PROD_HB_SAIDA1.PROD_HB1.ORDEM_PRODUCAO;
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

            if (subTitulo != "")
            {
                //Adiciona e-mails 
                usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(9, 2).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                foreach (var usu in usuarioEmail)
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);
                //Adicionar remetente
                destinatario.Add(usuario.EMAIL);
            }

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(PROD_HB_ENTRADA _entrada, string assunto, int tela)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(_entrada.PROD_HB_SAIDA1.PROD_HB1.PROD_GRADE));

            var gradeEntrada = faccController.ObterEntradaQtde(_entrada.CODIGO.ToString());
            var gradeAgrupada = gradeEntrada.GroupBy(p => p.PROD_PROCESSO).Select(x => new PROD_HB_ENTRADA_QTDE
            {
                PROD_PROCESSO = x.Key,
                GRADE_EXP = x.Sum(s => s.GRADE_EXP),
                GRADE_XP = x.Sum(s => s.GRADE_XP),
                GRADE_PP = x.Sum(s => s.GRADE_PP),
                GRADE_P = x.Sum(s => s.GRADE_P),
                GRADE_M = x.Sum(s => s.GRADE_M),
                GRADE_G = x.Sum(s => s.GRADE_G),
                GRADE_GG = x.Sum(s => s.GRADE_GG),
                GRADE_TOTAL = x.Sum(s => s.GRADE_TOTAL)
            });

            var gradeAtacado = gradeAgrupada.Where(p => p.PROD_PROCESSO == 99).SingleOrDefault();
            var gradeVarejo = gradeAgrupada.Where(p => p.PROD_PROCESSO == 98).SingleOrDefault();

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

            string retorno = "";
            if (_entrada.PRODUTO_ACABADO == 'S')
            {
                if (_entrada.PROD_HB_SAIDA1.PROD_PROCESSO == 21)
                    retorno = "CONFERÊNCIA DE FICHA - ACABAMENTO";
                else
                    retorno = "CONFERÊNCIA DE FICHA - LOGÍSTICA";
            }
            else
                retorno = "RETORNO " + _entrada.PROD_HB_SAIDA1.PROD_SERVICO1.DESCRICAO.ToUpper();

            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <h2>" + assunto.Replace("Intranet:", "") + " - " + retorno + "</h2>");
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

            //if (subStatus == "P" && _entrada.PROD_HB_SAIDA1.PROD_SERVICO == 1)
            //    fase = "P02B - CONTROLE QUALIDADE";

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
            sb.Append("                                                 Grade Atacado");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_EXP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_XP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_PP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_P)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_M)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_G)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_GG)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                 " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_TOTAL)));
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");
            sb.Append("                                         <tr>");
            sb.Append("                                             <td>");
            sb.Append("                                                 Grade Varejo");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_EXP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_XP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_PP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_P)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_M)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_G)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_GG)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                 " + (((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_TOTAL)));
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");
            sb.Append("                                         <tr>");
            sb.Append("                                             <td>");
            sb.Append("                                                 Total");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_EXP) + ((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_EXP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_XP) + ((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_XP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_PP) + ((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_PP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_P) + ((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_P)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_M) + ((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_M)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_G) + ((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_G)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_GG) + ((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_GG)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 75px;'>");
            sb.Append("                                                  " + (((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_TOTAL) + ((gradeVarejo == null) ? 0 : gradeVarejo.GRADE_TOTAL)));
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
