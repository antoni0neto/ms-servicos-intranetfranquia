using DAL;
using Relatorios.mod_desenvolvimento.modelo_pocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_painel_producaov2 : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPrevIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPrevFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                var colecao = Request.QueryString["col"].ToString();

                CarregarColecoes(colecao);
                CarregarGriffe();
                CarregarGrupoProduto();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");

        }

        #region "DADOS INI"
        private void CarregarColecoes(string colecao)
        {
            var colecoes = baseController.BuscaColecoes();

            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

            ddlColecao.DataSource = colecoes;
            ddlColecao.DataBind();

            if (colecao != "")
                ddlColecao.SelectedValue = baseController.BuscaColecaoAtual(colecao).COLECAO;
        }
        private void CarregarGrupoProduto()
        {
            var _grupo = (prodController.ObterGrupoProduto("01"));
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        #endregion

        #region "MODULOS"

        private List<VW_CALPRODUCAO> ObterProdutos(string local, string colecao)
        {
            var mod = new List<VW_CALPRODUCAO>();

            if (local == "pre-modelo")
                mod = ObterPreModelo(mod, colecao);
            else if (local == "mod_e_design")
                mod = ObterModelagemEDesign(mod, colecao);
            else if (local == "1ª_peca")
                mod = Obter1Peca(mod, colecao);
            else if (local == "modelagem")
                mod = ObterModelagem(mod, colecao);
            else if (local == "pre-risco")
                mod = ObterPreRisco(mod, colecao);
            else if (local == "risco")
                mod = ObterRisco(mod, colecao);
            else if (local == "corte")
                mod = ObterCorte(mod, colecao);
            else if (local == "encaixe_faccao")
                mod = ObterEnxaixeFaccao(mod, colecao, 20);
            else if (local == "faccao")
                mod = ObterFaccaoFal(mod, colecao, 1);
            else if (local == "encaixe_acabamento")
                mod = ObterEnxaixeFaccao(mod, colecao, 21);
            else if (local == "acabamento")
                mod = ObterFaccaoFal(mod, colecao, 4);

            if (local == "encaixe_faccao" || local == "faccao" || local == "encaixe_acabamento" || local == "acabamento")
            {
                if (txtDataPrevIni.Text.Trim() != "")
                    mod = mod.Where(p => p.DATA_PREVISAO != null && p.DATA_PREVISAO >= Convert.ToDateTime(txtDataPrevIni.Text.Trim())).ToList();

                if (txtDataPrevFim.Text.Trim() != "")
                    mod = mod.Where(p => p.DATA_PREVISAO != null && p.DATA_PREVISAO <= Convert.ToDateTime(txtDataPrevFim.Text.Trim())).ToList();
            }

            if (ddlGriffe.SelectedValue != "")
                mod = mod.Where(p => p.GRIFFE == ddlGriffe.SelectedValue.Trim()).ToList();

            if (ddlGrupo.SelectedValue != "")
                mod = mod.Where(p => p.GRUPO_PRODUTO == ddlGrupo.SelectedValue.Trim()).ToList();

            if (ddl90Porc.SelectedValue != "")
                if (ddl90Porc.SelectedValue == "S")
                    mod = mod.Where(p => p.PORC_QTDE_FALT >= 10).ToList();

            return mod;
        }

        private List<VW_OBTER_PREPEDIDO_TECIDOCOR> ObterPrePedidoTecidoCor(string colecao)
        {
            var mod = desenvController.ObterPrePedidoTecidoCor(colecao);

            if (ddlGriffe.SelectedValue != "")
                mod = mod.Where(p => p.GRIFFE == ddlGriffe.SelectedValue.Trim()).ToList();

            if (ddlGrupo.SelectedValue != "")
                mod = mod.Where(p => p.GRUPO_PRODUTO == ddlGrupo.SelectedValue.Trim()).ToList();

            return mod.OrderBy(p => p.TECIDO).ThenBy(p => p.COR_FORNECEDOR).ToList();
        }
        private List<VW_CALPRODUCAO> ObterPreModelo(List<VW_CALPRODUCAO> mod, string colecao)
        {
            var m = desenvController.ObterCalProducaoMODESTAMPA(colecao, 0);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = vw.QTDE_VAREJO,
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterModelagemEDesign(List<VW_CALPRODUCAO> mod, string colecao)
        {
            var m = desenvController.ObterCalProducaoMODESTAMPA(colecao, -1).Where(p => p.MODDESIGN != 0);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = vw.QTDE_VAREJO,
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> Obter1Peca(List<VW_CALPRODUCAO> mod, string colecao)
        {
            var m = desenvController.ObterCalProducao1PECA(colecao);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = vw.QTDE_VAREJO,
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterModelagem(List<VW_CALPRODUCAO> mod, string colecao)
        {
            var m = desenvController.ObterCalProducaoMODELAGEM(colecao);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = vw.QTDE_VAREJO,
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterPreRisco(List<VW_CALPRODUCAO> mod, string colecao)
        {
            var m = desenvController.ObterCalProducaoPRERISCO(colecao);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = Convert.ToInt32(vw.QTDE_VAREJO),
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterRisco(List<VW_CALPRODUCAO> mod, string colecao)
        {
            var m = desenvController.ObterCalProducaoRISCO(colecao);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = Convert.ToInt32(vw.QTDE_VAREJO),
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterCorte(List<VW_CALPRODUCAO> mod, string colecao)
        {
            var m = desenvController.ObterCalProducaoCORTE(colecao);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = Convert.ToInt32(vw.QTDE_VAREJO),
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterEnxaixeFaccao(List<VW_CALPRODUCAO> mod, string colecao, int codigoProcesso)
        {
            var m = desenvController.ObterCalProducaoEncaixeFACCAO(colecao, codigoProcesso);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = Convert.ToInt32(vw.QTDE_VAREJO),
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = Convert.ToDecimal(vw.PORC_QTDE_FALT)
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterFaccaoFal(List<VW_CALPRODUCAO> mod, string colecao, int codigoServico)
        {
            var m = desenvController.ObterCalProducaoFACCAOFAL(colecao, codigoServico);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.SERVICO,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = Convert.ToInt16(vw.CODIGO_ORIGEM),
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB.ToString(),
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = Convert.ToInt32(vw.QTDE_VAREJO),
                    QTDE_ATACADO = Convert.ToInt32(vw.QTDE_ATACADO),
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = Convert.ToDecimal(vw.PORC_QTDE_FALT),
                    FORNECEDOR = vw.FORNECEDOR_FACCAO
                });
            }

            return mod;
        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlColecao.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção...";
                    return;
                }

                CarregarPainel(ddlColecao.SelectedValue.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarPainel(string colecao)
        {

            try
            {

                divPrePedido.Visible = false;
                divPreModelo.Visible = false;
                divModEDes.Visible = false;
                div1Peca.Visible = false;
                divModelagem.Visible = false;
                divPreRisco.Visible = false;
                divRisco.Visible = false;
                divCorte.Visible = false;
                divEncFaccao.Visible = false;
                divFaccao.Visible = false;
                divEncAcabamento.Visible = false;
                divAcabamento.Visible = false;

                if (cbPrePedido.Checked)
                {
                    divPrePedido.Visible = true;
                    CarregarDIVPrePedido(ObterPrePedidoTecidoCor(colecao));
                }

                if (cbPreModelo.Checked)
                {
                    divPreModelo.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTotPreModelo, "pre-modelo", colecao), pnlPreModelo, false, "PRÉ-MODELO", 1);
                }

                if (cbModEDes.Checked)
                {
                    divModEDes.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTotMODeDES, "mod_e_design", colecao), pnlModEDes, false, "MOD e DESI", 1);
                }

                if (cb1Peca.Checked)
                {
                    div1Peca.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTot1Peca, "1ª_peca", colecao), pnl1Peca, false, "1ª PEÇA", 1);
                }

                if (cbModelagem.Checked)
                {
                    divModelagem.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTotModelagem, "modelagem", colecao), pnlModelagem, false, "MODELAGEM", 1);
                }

                if (cbPreRisco.Checked)
                {
                    divPreRisco.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTotPreRisco, "pre-risco", colecao), pnlPreRisco, false, "PRÉ-RISCO", 1);
                }

                if (cbRisco.Checked)
                {
                    divRisco.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTotRisco, "risco", colecao), pnlRisco, false, "RISCO", 2);
                }

                if (cbCorte.Checked)
                {
                    divCorte.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTotCorte, "corte", colecao), pnlCorte, false, "CORTE", 2);
                }

                if (cbEncFaccao.Checked)
                {
                    divEncFaccao.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTotEncFaccao, "encaixe_faccao", colecao), pnlEncaixeFaccao, true, "ENC FACÇÃO", 3);
                }

                if (cbFaccao.Checked)
                {
                    divFaccao.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTotFaccao, "faccao", colecao), pnlFaccao, true, "FACÇÃO", 4);
                }

                if (cbEncAcabamento.Checked)
                {
                    divEncAcabamento.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTotEncAcabamento, "encaixe_acabamento", colecao), pnlEncaixeAcabamento, true, "ENC ACAB", 3);
                }

                if (cbAcabamento.Checked)
                {
                    divAcabamento.Visible = true;
                    CarregarDIVs(ObterDadosPainel(labTotAcabamento, "acabamento", colecao), pnlAcabamento, true, "ACAB", 4);
                }


            }
            catch (Exception ex)
            {
            }
        }
        private List<DESENV_PRODUTO> ObterDadosPainel(Label labTot, string local, string colecao)
        {
            //ENCAIXE ACABAMENTO 
            var produtos = new List<DESENV_PRODUTO>();
            var vals = ObterProdutos(local, colecao).OrderBy("TEMPO_PREVISAO_DIAS ASC, TEMPO_MODULO_DIAS DESC").ToList();
            foreach (var m in vals)
            {
                var desenvProduto = desenvController.ObterProduto(m.COLECAO, m.PRODUTO, m.COR, m.CODIGO_ORIGEM);
                if (desenvProduto != null)
                {
                    // campos auxiliares, o nome da propriedade nao reflete seu conteudo
                    desenvProduto.GRADE = m.DATA_ENTRADA_MODULO.ToString("dd/MM/yyyy") + " / " + m.TEMPO_MODULO_DIAS.ToString() + " Dias";
                    desenvProduto.LINHA = m.DATA_PREVISAO.ToString("dd/MM/yyyy") + " / " + m.TEMPO_PREVISAO_DIAS.ToString() + " Dias";
                    desenvProduto.QTDE_MOSTRUARIO = m.QTDE_TOTAL;
                    desenvProduto.FORNECEDOR = (m.FORNECEDOR == null || m.FORNECEDOR.Trim() == "") ? desenvProduto.FORNECEDOR : m.FORNECEDOR;
                    desenvProduto.QTDE = m.QTDE_TOTAL;
                    produtos.Add(desenvProduto);
                }
            }

            var qtdeTotal = vals.Select(x => x.QTDE_TOTAL).Sum();

            labTot.Text = produtos.Count().ToString() + " - Qtde " + qtdeTotal.ToString();

            return produtos;
        }


        private void CarregarDIVs(List<DESENV_PRODUTO> produtos, Panel pnl, bool gradeReal, string modulo, int painel)
        {
            var div = new HtmlGenericControl();
            div.InnerHtml = MontarPocket(produtos, gradeReal, modulo, painel).ToString();
            pnl.Controls.Add(div);
        }
        private void CarregarDIVPrePedido(List<VW_OBTER_PREPEDIDO_TECIDOCOR> tecidos)
        {
            var div = new HtmlGenericControl();
            div.InnerHtml = MontarPrePedido(tecidos).ToString();
            pnlPrePedido.Controls.Add(div);

            labTotPrePedido.Text = tecidos.Sum(p => p.SKU).ToString();
        }

        private StringBuilder MontarPocket(List<DESENV_PRODUTO> listaProdutos, bool gradeReal, string modulo, int painel)
        {
            StringBuilder b = new StringBuilder();

            //Carregar produtos
            List<PocketEntity> pocket = new List<PocketEntity>();
            if (listaProdutos != null && listaProdutos.Count > 0)
            {
                string modelo = listaProdutos[0].MODELO.Trim().ToUpper();
                int _index = 0;
                int _cores = 0;

                foreach (DESENV_PRODUTO p in listaProdutos)
                {
                    _cores += 1;
                    if (modelo == p.MODELO.Trim().ToUpper() && _cores < 2)
                    {
                        //adicionar produto
                        pocket.Add(new PocketEntity { index = _index, qtdeCopia = 1, produto = p });
                        //obter modelo
                        modelo = p.MODELO.Trim().ToUpper();
                    }
                    else
                    {
                        _index += 1;
                        _cores = 1;
                        pocket.Add(new PocketEntity { index = _index, qtdeCopia = 1, produto = p });
                        modelo = p.MODELO.Trim().ToUpper();
                    }
                }
            }

            if (pocket.Count > 0)
            {

                int loop = pocket.Select(i => i.index).Distinct().Count();

                int count = 0;
                List<DESENV_PRODUTO> prod = new List<DESENV_PRODUTO>();

                //montar cabeçalho
                b = Pocket.MontarCabecalho(b, false);
                b = Pocket.AbreLinha(b, false, "dInicio");

                int qtdeCopia = 1;
                for (int i = 0; i < loop; i++)
                {
                    List<PocketEntity> pocketProduto = pocket.Where(j => j.index == i).ToList();
                    qtdeCopia = (pocketProduto != null && pocketProduto.Count > 0) ? pocketProduto[0].qtdeCopia : 0;

                    while (qtdeCopia > 0)
                    {
                        count += 1;

                        b = Pocket.MontarConteudoCAL1(b, pocketProduto.Select(j => j.produto).ToList(), gradeReal, modulo, painel, (i + 1));

                        if (count <= 2)
                            b = Pocket.ColunaVazia(b, 0, false, "15");

                        if (count == 3)
                        {
                            //Fecha linha
                            b = Pocket.FechaLinha(b);

                            //Pula Linha
                            b = Pocket.AbreLinha(b, false, ("dHR" + i.ToString()));
                            b = Pocket.ColunaVazia(b, 5, false);
                            b = Pocket.FechaLinha(b);
                            b = Pocket.AbreLinha(b, false, ("dES" + i.ToString()));
                            b = Pocket.ColunaVazia(b, 5, false);
                            b = Pocket.FechaLinha(b);


                            //Abre nova linha
                            b = Pocket.AbreLinha(b, true, ("dLinha" + i.ToString()));
                            count = 0;
                        }

                        qtdeCopia -= 1;
                    }

                }

                if (count == 2)
                    b = Pocket.FechaColuna(b);

                //montar rodape
                b = Pocket.FechaLinha(b);
                b = Pocket.MontarRodape(b);
            }

            return b;
        }
        private StringBuilder MontarPrePedido(List<VW_OBTER_PREPEDIDO_TECIDOCOR> tecidos)
        {
            StringBuilder b = new StringBuilder();

            var tecidoAgrupado = tecidos.GroupBy(p => new { TECIDO = p.TECIDO, COR_FORNECEDOR = p.COR_FORNECEDOR })
                .Select(x => new VW_OBTER_PREPEDIDO_TECIDOCOR()
                {
                    TECIDO = x.Key.TECIDO,
                    COR_FORNECEDOR = x.Key.COR_FORNECEDOR
                }).ToList();

            int count = 0;

            //montar cabeçalho
            b = Pocket.MontarCabecalho(b, false);
            b = Pocket.AbreLinha(b, false, "dInicio");

            int i = 0;
            foreach (var tec in tecidoAgrupado)
            {
                count += 1;

                b = Pocket.MontarConteudoPrePedidoTecido(b, tecidos.Where(p => p.TECIDO == tec.TECIDO && p.COR_FORNECEDOR == tec.COR_FORNECEDOR).ToList());

                if (count <= 2)
                    b = Pocket.ColunaVazia(b, 0, false, "15");

                if (count == 3)
                {
                    //Fecha linha
                    b = Pocket.FechaLinha(b);

                    //Pula Linha
                    b = Pocket.AbreLinha(b, false, ("dHR" + i.ToString()));
                    b = Pocket.ColunaVazia(b, 5, false);
                    b = Pocket.FechaLinha(b);
                    b = Pocket.AbreLinha(b, false, ("dES" + i.ToString()));
                    b = Pocket.ColunaVazia(b, 5, false);
                    b = Pocket.FechaLinha(b);


                    //Abre nova linha
                    b = Pocket.AbreLinha(b, true, ("dLinha" + i.ToString()));
                    count = 0;
                }

                i += 1;
            }

            if (count == 2)
                b = Pocket.FechaColuna(b);

            //montar rodape
            b = Pocket.FechaLinha(b);
            b = Pocket.MontarRodape(b);

            return b;
        }

        protected void cbTodos_CheckedChanged(object sender, EventArgs e)
        {
            var todos = cbTodos.Checked;
            cbPrePedido.Checked = todos;
            cbPreModelo.Checked = todos;
            cbModEDes.Checked = todos;
            cb1Peca.Checked = todos;
            cbModelagem.Checked = todos;
            cbPreRisco.Checked = todos;
            cbRisco.Checked = todos;
            cbCorte.Checked = todos;
            cbEncFaccao.Checked = todos;
            cbFaccao.Checked = todos;
            cbEncAcabamento.Checked = todos;
            cbAcabamento.Checked = todos;
        }
    }
}

