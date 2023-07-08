using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Relatorios
{
    public partial class prod_rel_hb_impressao : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
            }

            //Evitar duplo clique no botão
            btImprimir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImprimir, null) + ";");
        }

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

                if (Session["COLECAO"] != null)
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        #endregion

        protected void btImprimir_Click(object sender, EventArgs e)
        {
            labImpressao.Text = "";
            if (ddlColecoesBuscar.SelectedValue == "" || ddlColecoesBuscar.SelectedValue == "0")
            {
                labImpressao.Text = "Selecione a Coleção.";
                return;
            }
            if (txtHBBuscar.Text == "" || Convert.ToInt32(txtHBBuscar.Text) <= 0)
            {
                labImpressao.Text = "Informe o número do HB.";
                return;
            }
            if (ddlFicha.SelectedValue == "" || ddlFicha.SelectedValue == "0")
            {
                labImpressao.Text = "Selecione a Ficha.";
                return;
            }

            try
            {
                List<PROD_HB> lstProdHB = new List<PROD_HB>();
                lstProdHB = prodController.ObterNumeroHB(ddlColecoesBuscar.SelectedValue, Convert.ToInt32(txtHBBuscar.Text));

                PROD_HB _prod_hb = null;
                _prod_hb = lstProdHB.Where(p => p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).SingleOrDefault();

                if (_prod_hb == null)
                {
                    labImpressao.Text = "Nenhum HB encontrado. Refaça a sua pesquisa.";
                    return;
                }

                if (_prod_hb.STATUS.ToString() == "X" || _prod_hb.STATUS.ToString() == "E")
                {
                    labImpressao.Text = "Nenhum HB encontrado. Refaça a sua pesquisa.";
                    return;
                }

                //GERAR RELATORIO
                RelatorioHB _relatorio = new RelatorioHB();
                _relatorio.CODIGO = _prod_hb.CODIGO;
                _relatorio.HB = _prod_hb.HB.ToString();
                _relatorio.COLECAO = (new BaseController().BuscaColecaoAtual(_prod_hb.COLECAO)).DESC_COLECAO;
                var prodCor = (new BaseController().BuscaProdutoCorCodigo(_prod_hb.CODIGO_PRODUTO_LINX.ToString(), _prod_hb.COR));
                if (prodCor != null)
                {
                    _relatorio.DESC_COR = prodCor.DESC_COR_PRODUTO.Trim();
                    _relatorio.COD_COR = _prod_hb.COR.Trim();
                }
                else
                {
                    var prodCorBasica = prodController.ObterCoresBasicas(_prod_hb.COR);
                    if (prodCorBasica != null)
                    {
                        _relatorio.COD_COR = _prod_hb.COR;
                        _relatorio.DESC_COR = prodCorBasica.DESC_COR.Trim();
                    }
                    else
                    {
                        _relatorio.COD_COR = "000";
                        _relatorio.DESC_COR = "SEM COR";
                    }
                }
                _relatorio.COR_FORNECEDOR = _prod_hb.COR_FORNECEDOR;
                _relatorio.GRUPO = _prod_hb.GRUPO.Trim().ToUpper();
                _relatorio.NOME = _prod_hb.NOME.Trim();
                _relatorio.DATA = DateTime.Now.ToString("dd/MM/yyyy");
                _relatorio.TECIDO = _prod_hb.TECIDO.Trim();
                _relatorio.GRUPO_TECIDO = _prod_hb.GRUPO_TECIDO.Trim();
                _relatorio.FORNECEDOR = _prod_hb.FORNECEDOR.Trim().ToUpper();
                _relatorio.LARGURA = _prod_hb.LARGURA.ToString();
                _relatorio.LIQUIDAR = (_prod_hb.LIQUIDAR == 'S') ? "Sim" : "Não";
                _relatorio.LIQUIDAR_PQNO = (_prod_hb.LIQUIDAR_PQNO == 'S') ? "Sim" : "Não";
                _relatorio.MODELAGEM = _prod_hb.MODELAGEM.Trim().ToUpper();
                _relatorio.CUSTO_TECIDO = Convert.ToDecimal(_prod_hb.CUSTO_TECIDO).ToString("###,###,##0.00");
                _relatorio.FOTO_PECA = _prod_hb.FOTO_PECA;
                _relatorio.FOTO_TECIDO = _prod_hb.FOTO_TECIDO;
                _relatorio.COMPOSICAO = prodController.ObterComposicaoHB(_prod_hb.CODIGO);
                _relatorio.DETALHE = prodController.ObterDetalhesHB(_prod_hb.CODIGO);
                _relatorio.OBS = _prod_hb.OBSERVACAO.Trim().ToUpper();
                _relatorio.COMPOSICAO = prodController.ObterComposicaoHB(_prod_hb.CODIGO);
                _relatorio.DETALHE = prodController.ObterDetalhesHB(_prod_hb.CODIGO);
                _relatorio.AVIAMENTO = prodController.ObterAviamentoHB(_prod_hb.CODIGO);
                _relatorio.GRADE_CORTE = prodController.ObterGradeCorteHB(_prod_hb.CODIGO);
                _relatorio.CODIGO_PRODUTO_LINX = _prod_hb.CODIGO_PRODUTO_LINX.ToString();
                _relatorio.NUMERO_PEDIDO = _prod_hb.NUMERO_PEDIDO.ToString();
                _relatorio.PC_VAREJO = (_prod_hb.PC_VAREJO != null) ? _prod_hb.PC_VAREJO.Trim() : "";
                _relatorio.PC_ATACADO = (_prod_hb.PC_ATACADO != null) ? _prod_hb.PC_ATACADO.Trim() : "";
                _relatorio.MOLDE = (_prod_hb.MOLDE != null) ? _prod_hb.MOLDE.ToString() : "";
                _relatorio.AMPLIACAO_OUTRO = _prod_hb.AMPLIACAO_OUTRO;
                _relatorio.MOSTRUARIO = _prod_hb.MOSTRUARIO.ToString();
                _relatorio.PROD_GRADE = _prod_hb.PROD_GRADE.ToString();
                _relatorio.ORDEM_PRODUCAO = _prod_hb.ORDEM_PRODUCAO;
                _relatorio.VOLUME = (_prod_hb.VOLUME == null) ? "" : _prod_hb.VOLUME.ToString();
                _relatorio.GABARITO = (_prod_hb.GABARITO == null) ? "-" : _prod_hb.GABARITO.ToString();
                _relatorio.ESTAMPARIA = (_prod_hb.ESTAMPARIA == null) ? "-" : _prod_hb.ESTAMPARIA.ToString();
                _relatorio.DATA_IMP_FIC_LOGISTICA = (_prod_hb.DATA_IMP_FIC_LOGISTICA == null) ? "" : _prod_hb.DATA_IMP_FIC_LOGISTICA.ToString();
                _relatorio.NOME_AMPLIACAO = _prod_hb.NOME_AMPLIACAO;
                _relatorio.NOME_RISCO = _prod_hb.NOME_RISCO;
                _relatorio.NOME_MODELAGEM = _prod_hb.NOME_MODELAGEM;

                var _produto = desenvController.ObterProduto(ddlColecoesBuscar.SelectedValue, _prod_hb.CODIGO_PRODUTO_LINX.ToString(), _relatorio.COD_COR);
                if (_produto != null)
                {
                    if (_produto.GRIFFE != null)
                        _relatorio.GRIFFE = _produto.GRIFFE;

                    _relatorio.SIGNED = "N";
                    if (_produto.SIGNED != null)
                        _relatorio.SIGNED = _produto.SIGNED.ToString();

                    _relatorio.SIGNED_NOME = "";
                    if (_produto.SIGNED_NOME != null)
                        _relatorio.SIGNED_NOME = _produto.SIGNED_NOME;

                }

                PROD_HB_ROTA _rota = new PROD_HB_ROTA();
                _rota = prodController.ObterRotaOP(_prod_hb.ORDEM_PRODUCAO);
                if (_rota != null)
                {
                    _relatorio.FaseEstamparia = "NÃO";
                    if (_rota.ESTAMPARIA)
                        _relatorio.FaseEstamparia = "SIM";

                    _relatorio.FaseLavanderia = "NÃO";
                    if (_rota.LAVANDERIA)
                        _relatorio.FaseLavanderia = "SIM";
                }
                else
                {
                    _relatorio.FaseEstamparia = "NÃO";
                    _relatorio.FaseLavanderia = "NÃO";
                }

                //OBTER PROCESSO
                _relatorio.PROCESSO = prodController.ObterProcessoHB(_prod_hb.CODIGO, 3); //CORTE

                if (ddlFicha.SelectedValue == "1") //RISCO FICHA
                {
                    _relatorio.GRADE = prodController.ObterGradeHB(_prod_hb.CODIGO, ((_prod_hb.TIPO == 'A') ? 1 : 2));
                    _relatorio.PROCESSO = prodController.ObterProcessoHB(_prod_hb.CODIGO, 2); //RISCO
                    GerarRelatorioRisco(_relatorio, 1, chkMostruario.Checked);
                }

                if (ddlFicha.SelectedValue == "2") //RISCO CORTE
                {
                    _relatorio.GRADEREAL = prodController.ObterGradeHB(_prod_hb.CODIGO, 3);
                    _relatorio.GRADE = prodController.ObterGradeHB(_prod_hb.CODIGO, 2);
                    if (_relatorio.GRADE == null)
                    {
                        labImpressao.Text = "Não será possível imprimir este relatório. Este relatório ainda está na Ampliação.";
                        return;
                    }
                    else
                    {
                        GerarRelatorioCorte(_relatorio, 2);
                    }
                }

                if (ddlFicha.SelectedValue == "3") //FACÇÃO
                {
                    _relatorio.GRADE = prodController.ObterGradeHB(_prod_hb.CODIGO, 3); //GERAR RELATORIO COM A GRADE FINAL DO CORTE
                    _relatorio.GRADEATACADO = prodController.ObterGradeHB(_prod_hb.CODIGO, 99); //GERAR RELATORIO COM A GRADE ATACADO
                    if (_relatorio.GRADE == null)
                    {
                        labImpressao.Text = "Não será possível imprimir este relatório. Não possui grade REAL.";
                        return;
                    }
                    else
                    {
                        GerarRelatorioCorte(_relatorio, 3);
                    }
                }

                if (ddlFicha.SelectedValue == "4") //AVIAMENTO
                {
                    _relatorio.GRADE = prodController.ObterGradeHB(_prod_hb.CODIGO, 3); //GERAR RELATORIO COM A GRADE FINAL DO CORTE
                    _relatorio.GRADEATACADO = prodController.ObterGradeHB(_prod_hb.CODIGO, 99); //GERAR RELATORIO COM A GRADE ATACADO

                    if (_relatorio.GRADE == null)
                    {
                        labImpressao.Text = "Não será possível imprimir este relatório. Não possui grade REAL.";
                        return;
                    }
                    else
                    {
                        GerarRelatorioCorte(_relatorio, 4);
                    }
                }

                if (ddlFicha.SelectedValue == "5") //LOGISTICA
                {
                    if (_prod_hb.MOSTRUARIO == 'N')
                    {
                        if (_prod_hb.DATA_GRADE_ATACADO == null && _prod_hb.ATACADO == 'S')
                        {
                            labImpressao.Text = "Não será possível imprimir este relatório. Logística ainda NÃO foi definida.";
                            return;
                        }
                        if (_prod_hb.DATA_IMP_FIC_LOGISTICA == null)
                        {
                            labImpressao.Text = "Não será possível imprimir este relatório. Este relatório deve ser impresso pela \"Impressão Ficha Logística\".";
                            return;
                        }
                    }

                    _relatorio.GRADE = prodController.ObterGradeHB(_prod_hb.CODIGO, 3); //GERAR RELATORIO COM A GRADE FINAL DO RISCO
                    _relatorio.GRADEATACADO = prodController.ObterGradeHB(_prod_hb.CODIGO, 99); //GERAR RELATORIO COM A GRADE ATACADO

                    if (_relatorio.GRADE == null)
                    {
                        labImpressao.Text = "Não será possível imprimir este relatório. Não possui grade REAL.";
                        return;
                    }
                    else
                    {
                        GerarRelatorioCorte(_relatorio, 5);
                    }
                }

                if (ddlFicha.SelectedValue == "6") //DETALHES
                {

                    _relatorio.GRADEREAL = prodController.ObterGradeHB(_prod_hb.CODIGO, 3);

                    _relatorio.GRADE = prodController.ObterGradeHB(_prod_hb.CODIGO, 2);
                    if (_relatorio.GRADE == null)
                        _relatorio.GRADE = prodController.ObterGradeHB(_prod_hb.CODIGO, 1);

                    if (_relatorio.GRADE == null)
                    {
                        labImpressao.Text = "Não será possível imprimir este relatório. Não possui grade.";
                        return;
                    }
                    else
                    {
                        GerarRelatorioCorte(_relatorio, 6);
                    }
                }

                if (ddlFicha.SelectedValue == "7") //FICHA DE AMPLIAÇÃO
                {
                    _relatorio.AMPLIACAOMEDIDA = prodController.ObterAmpliacaoMedidasHB().Where(p => p.PROD_HB == _prod_hb.CODIGO).ToList();
                    if (_relatorio.AMPLIACAOMEDIDA == null)
                    {
                        labImpressao.Text = "Não será possível imprimir este relatório. Não possui Ampliação.";
                        return;
                    }
                    else
                    {
                        GerarRelatorioRisco(_relatorio, 7, chkMostruario.Checked);
                    }
                }

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;
            }
            catch (Exception ex)
            {
                labImpressao.Text = ex.Message;
            }
        }

        #region "FICHA RISCO"
        private void GerarRelatorioRisco(RelatorioHB _relatorio, int tela, bool mostruario)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "HB_" + _relatorio.HB + "_COL_" + _relatorio.COLECAO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_relatorio, tela, mostruario));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private StringBuilder MontarRelatorioHTML(RelatorioHB _relatorio, int tela, bool mostruario)
        {
            StringBuilder _texto = new StringBuilder();

            if (tela == 1)
            {
                _texto = _relatorio.MontarCabecalho(_texto, "HB - Ordem de Corte - Ficha CAD");
                _texto = _relatorio.MontarRiscoCAD(_texto, _relatorio);
                _texto = _relatorio.MontarReciboFaccao(_texto, _relatorio, "breakafter");

                // Detalhes
                _texto = _relatorio.MontarDetalheRiscoCAD(_texto, _relatorio, "breakbefore");

                //_texto = _relatorio.MontarComposicao(_texto, _relatorio);
                //_texto = _relatorio.MontarGrade(_texto, _relatorio);
                //_texto = _relatorio.MontarGradeCorte(_texto, _relatorio);
                //_texto = MontarLinhasEmBranco(_texto);
                //_texto = MontarDetalhes(_texto, _relatorio);
                //_texto = _relatorio.MontarModelagem(_texto, _relatorio);
                //_texto = MontarPeca(_texto, _relatorio);
                //_texto = _relatorio.MontarOBS(_texto, _relatorio);
                //_texto = _relatorio.MontarDetalhe(_texto, _relatorio, "breakbefore");
            }
            if (tela == 7)
            {
                _texto = _relatorio.MontarCabecalho(_texto, "HB - Ampliação - Ficha");
                _texto = _relatorio.MontarAmpliacao(_texto, _relatorio, "");
            }
            _texto = _relatorio.MontarRodape(_texto);
            return _texto;
        }
        #endregion

        #region "FICHA CORTE"
        private void GerarRelatorioCorte(RelatorioHB _relatorio, int tela)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "";
                if (tela == 2)
                    nomeArquivo = "HB_BAIXADO_R_" + _relatorio.HB + "_COL_" + _relatorio.COLECAO + ".html";
                else
                    nomeArquivo = "HB_BAIXADO_C_" + _relatorio.HB + "_COL_" + _relatorio.COLECAO + ".html";

                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioCorteHTML(_relatorio, tela));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private StringBuilder MontarRelatorioCorteHTML(RelatorioHB _relatorio, int tela)
        {
            StringBuilder _texto = new StringBuilder();

            if (tela == 2) //BAIXA RISCO
            {
                //Cabecalho
                _texto = _relatorio.MontarCabecalho(_texto, "HB - Baixa Risco - Ficha");
                //Montar ORDEM DE CORTE
                _texto = _relatorio.MontarTecido(_texto, _relatorio);
                _texto = _relatorio.MontarComposicao(_texto, _relatorio);
                _texto = _relatorio.MontarGrade(_texto, _relatorio);
                _texto = _relatorio.MontarGradeCorte(_texto, _relatorio);
                _texto = _relatorio.MontarLinhasEmBranco(_texto);
                _texto = _relatorio.MontarDetalhes(_texto, _relatorio);
                _texto = _relatorio.MontarModelagem(_texto, _relatorio);
                _texto = _relatorio.MontarPeca(_texto, _relatorio);
                _texto = _relatorio.MontarOBS(_texto, _relatorio);
                //Rodape
                _texto = _relatorio.MontarRodape(_texto);
            }
            else if (tela == 3) //FACAO
            {
                // Cabecalho
                _texto = _relatorio.MontarCabecalho(_texto, "Facção - Ficha");
                // Facção
                _texto = _relatorio.MontarFaccaoAviamento(_texto, _relatorio, "1", ""); //Facção
                // Rodape
                _texto = _relatorio.MontarRodape(_texto);
            }
            else if (tela == 4) //AVIAMENTO
            {
                // Cabecalho
                _texto = _relatorio.MontarCabecalho(_texto, "Aviamento - Ficha");
                // Aviamento
                _texto = _relatorio.MontarFaccaoAviamento(_texto, _relatorio, "2", ""); //Aviamento
                // Rodape
                _texto = _relatorio.MontarRodape(_texto);
            }
            else if (tela == 5) //LOGISTICA
            {
                // Cabecalho
                _texto = _relatorio.MontarCabecalho(_texto, "Logística - Ficha");
                // Logistica
                _texto = _relatorio.MontarLogisticaV2(_texto, _relatorio, "", Server.MapPath("~")); // Logística
                // Rodape
                _texto = _relatorio.MontarRodape(_texto);
            }
            else if (tela == 6) //DETALHES
            {
                // Cabecalho
                _texto = _relatorio.MontarCabecalho(_texto, "Detalhes - Ficha");
                // Detalhes
                _texto = _relatorio.MontarDetalhe(_texto, _relatorio, "breakafter");
                // Rodape
                _texto = _relatorio.MontarRodape(_texto);
            }

            return _texto;
        }
        #endregion
    }
}
