using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class prod_def_logistica_imp : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarDefinicaoLogistica("", "", "");

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
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

        #endregion

        #region "LOGISTICA"
        private void CarregarDefinicaoLogistica(string colecao, string hb, string produto)
        {
            var prodSemCad = ObterDefinicaoGradeAtacadoImpressao(colecao, hb, produto);

            gvLogistica.DataSource = prodSemCad;
            gvLogistica.DataBind();

        }
        protected void gvLogistica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DEFINICAO_GRADE_ATACADO_IMPResult logistica = e.Row.DataItem as SP_OBTER_DEFINICAO_GRADE_ATACADO_IMPResult;

                    if (logistica != null)
                    {
                        Button _btImprimir = e.Row.FindControl("btImprimir") as Button;
                        if (_btImprimir != null)
                            _btImprimir.CommandArgument = logistica.CODIGO.ToString();

                        //Popular GRID VIEW FILHO
                        if (logistica.FOTO_PECA != null && logistica.FOTO_PECA.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<PROD_HB> _fotoProduto = new List<PROD_HB>();
                                _fotoProduto.Add(new PROD_HB { FOTO_PECA = logistica.FOTO_PECA });
                                gvFoto.DataSource = _fotoProduto;
                                gvFoto.DataBind();
                            }
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                            if (img != null)
                                img.Visible = false;
                        }
                    }
                }
            }
        }
        protected void gvFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _produto = e.Row.DataItem as PROD_HB;
                    if (_produto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _produto.FOTO_PECA;

                    }
                }
            }
        }
        #endregion

        #region "IMPRIMIR"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";

            if (b != null)
            {
                try
                {
                    int codigoHB = Convert.ToInt32(b.CommandArgument);
                    var _prod_hb = prodController.ObterHB(codigoHB);

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
                    _relatorio.FORNECEDOR = _prod_hb.FORNECEDOR.Trim().ToUpper();
                    _relatorio.LARGURA = _prod_hb.LARGURA.ToString();
                    _relatorio.LIQUIDAR = (_prod_hb.LIQUIDAR == 'S') ? "Sim" : "Não";
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
                    _relatorio.DATA_IMP_FIC_LOGISTICA = (_prod_hb.DATA_IMP_FIC_LOGISTICA == null) ? "" : _prod_hb.DATA_IMP_FIC_LOGISTICA.ToString();

                    //OBTER PROCESSO
                    _relatorio.PROCESSO = prodController.ObterProcessoHB(_prod_hb.CODIGO, 3); //CORTE

                    if (_prod_hb.MOSTRUARIO == 'N' && _prod_hb.ATACADO == 'S')
                    {
                        if (_prod_hb.DATA_GRADE_ATACADO == null)
                        {
                            labMsg.Text = "Não será possível imprimir este relatório. Logística ainda NÃO foi definida.";
                            return;
                        }
                    }

                    _relatorio.GRADE = prodController.ObterGradeHB(_prod_hb.CODIGO, 3); //GERAR RELATORIO COM A GRADE FINAL DO RISCO
                    _relatorio.GRADEATACADO = prodController.ObterGradeHB(_prod_hb.CODIGO, 99); //GERAR RELATORIO COM A GRADE ATACADO

                    var desenvProduto = desenvController.ObterProduto(_prod_hb.COLECAO, _prod_hb.CODIGO_PRODUTO_LINX, _prod_hb.COR);
                    if (desenvProduto != null)
                    {
                        if (desenvProduto.GRIFFE != null)
                            _relatorio.GRIFFE = desenvProduto.GRIFFE;

                        _relatorio.SIGNED = "N";
                        if (desenvProduto.SIGNED != null)
                            _relatorio.SIGNED = desenvProduto.SIGNED.ToString();

                        _relatorio.SIGNED_NOME = "";
                        if (desenvProduto.SIGNED_NOME != null)
                            _relatorio.SIGNED_NOME = desenvProduto.SIGNED_NOME;

                    }

                    GerarRelatorioCorte(_relatorio);

                    _prod_hb.DATA_IMP_FIC_LOGISTICA = DateTime.Now;
                    prodController.AtualizarHB(_prod_hb);

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        private void GerarRelatorioCorte(RelatorioHB _relatorio)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "";
                nomeArquivo = "HB_BAIXADO_LOG_" + _relatorio.HB + "_COL_" + _relatorio.COLECAO.Trim() + ".html";

                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioLogisticaHTML(_relatorio));
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
        private StringBuilder MontarRelatorioLogisticaHTML(RelatorioHB _relatorio)
        {
            StringBuilder _texto = new StringBuilder();

            // Cabecalho
            _texto = _relatorio.MontarCabecalho(_texto, "Logística - 1ª Impressão");
            // Logistica
            _texto = _relatorio.MontarLogisticaV2(_texto, _relatorio, "", Server.MapPath("~")); // Logística
            // Rodape
            _texto = _relatorio.MontarRodape(_texto);

            return _texto;
        }
        #endregion

        private List<SP_OBTER_DEFINICAO_GRADE_ATACADO_IMPResult> ObterDefinicaoGradeAtacadoImpressao(string colecao, string hb, string produto)
        {
            int? hbValue = null;
            if (hb != "")
                hbValue = Convert.ToInt32(hb);

            var logistica = prodController.ObterDefinicaoGradeAtacadoImpressao(colecao, hbValue, produto);

            return logistica;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarDefinicaoLogistica(ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim(), txtProduto.Text.Trim().ToUpper());

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

    }
}

