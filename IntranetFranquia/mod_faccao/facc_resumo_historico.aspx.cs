using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using DAL;


namespace Relatorios
{
    public partial class facc_resumo_historico : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        PROD_GRADE _gradeNome = new PROD_GRADE();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "facc_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                if (tela == "3")
                    hrefVoltar.HRef = "facc_menu_relatorio.aspx";

                if (tela == "4")
                    hrefVoltar.HRef = "../mod_fiscal/fisc_menu.aspx";

                CarregarColecoes();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_FACCAO_HISTORICOResult> ObterFaccaoHistorico(string colecao, int prod_processo, int prod_servico)
        {
            char mostruario = 'N';
            if (cbMostruario.Checked)
                mostruario = 'S';

            var historico = faccController.ObterFaccaoHistorico(colecao, Convert.ToInt32(txtHB.Text), mostruario, prod_processo, prod_servico);

            return historico;
        }
        private List<SP_OBTER_FACCAO_HISTORICO_OUTROSResult> ObterFaccaoHistoricoOutros(string colecao, int prod_processo, int prod_servico)
        {
            List<SP_OBTER_FACCAO_HISTORICO_OUTROSResult> listaHistorico = new List<SP_OBTER_FACCAO_HISTORICO_OUTROSResult>();
            char mostruario = 'N';

            if (cbMostruario.Checked)
                mostruario = 'S';

            listaHistorico.AddRange(faccController.ObterFaccaoHistoricoOutros(colecao, Convert.ToInt32(txtHB.Text), mostruario, prod_processo, prod_servico));

            return listaHistorico;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlColecao.SelectedValue.Trim() == "" || ddlColecao.SelectedValue.Trim() == "0")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                if (txtHB.Text.Trim() == "")
                {
                    labErro.Text = "Informe o número do HB.";
                    return;
                }

                LimparGrids();

                PROD_HB_ROTA rotaHB = new PROD_HB_ROTA();
                List<SP_OBTER_FACCAO_HISTORICOResult> listaEstamparia = new List<SP_OBTER_FACCAO_HISTORICOResult>();
                List<SP_OBTER_FACCAO_HISTORICOResult> listaFaccao = new List<SP_OBTER_FACCAO_HISTORICOResult>();
                List<SP_OBTER_FACCAO_HISTORICOResult> listaLavanderia = new List<SP_OBTER_FACCAO_HISTORICOResult>();
                List<SP_OBTER_FACCAO_HISTORICOResult> listaAcabamento = new List<SP_OBTER_FACCAO_HISTORICOResult>();
                List<SP_OBTER_FACCAO_HISTORICO_OUTROSResult> listaOutros = new List<SP_OBTER_FACCAO_HISTORICO_OUTROSResult>();
                List<SP_OBTER_FACCAO_HISTORICOResult> listaLogistica = new List<SP_OBTER_FACCAO_HISTORICOResult>();

                bool temRotaEstamparia = false;
                bool temRotaLavanderia = false;
                bool temRotaFaccao = false;
                bool temRotaAcabamento = false;

                var hb = prodController.ObterNumeroHB(ddlColecao.SelectedValue.Trim(), Convert.ToInt32(txtHB.Text.Trim())).Where(p => p.MOSTRUARIO == ((cbMostruario.Checked) ? 'S' : 'N') && p.CODIGO_PAI == null).FirstOrDefault();
                if (hb != null)
                {
                    imgFoto.ImageUrl = hb.FOTO_PECA;
                    imgFoto.Visible = true;

                    txtProduto.Text = hb.CODIGO_PRODUTO_LINX;
                    txtNome.Text = hb.NOME;
                    txtCor.Text = prodController.ObterCoresBasicas(hb.COR).DESC_COR;

                    if (hb.ORDEM_PRODUCAO != null)
                        rotaHB = prodController.ObterRotaOP(hb.ORDEM_PRODUCAO);

                    _gradeNome = hb.PROD_GRADE1;

                    hidCodigoHB.Value = hb.CODIGO.ToString();
                }

                //VERIFICAR ROTAS
                if (rotaHB != null && hb.CODIGO == rotaHB.PROD_HB)
                {
                    temRotaEstamparia = rotaHB.ESTAMPARIA;
                    temRotaFaccao = rotaHB.FACCAO;
                    temRotaLavanderia = rotaHB.LAVANDERIA;
                    temRotaAcabamento = rotaHB.ACABAMENTO;
                }
                else
                {
                    labErro.Text = "Produto não possui Rotas. Verifique se o produto já foi cortado. Se sim, entre em contato com TI.";
                    return;
                }


                if (temRotaEstamparia)
                {
                    listaEstamparia = ObterFaccaoHistorico(ddlColecao.SelectedValue.Trim(), 20, 2);
                    gvEstamparia.DataSource = listaEstamparia;
                    gvEstamparia.DataBind();
                }

                if (temRotaFaccao)
                {
                    listaFaccao = ObterFaccaoHistorico(ddlColecao.SelectedValue.Trim(), 20, 1);
                    gvFaccao.DataSource = listaFaccao;
                    gvFaccao.DataBind();
                }

                if (temRotaLavanderia)
                {
                    listaLavanderia = ObterFaccaoHistorico(ddlColecao.SelectedValue.Trim(), 20, 3);
                    gvLavanderia.DataSource = listaLavanderia;
                    gvLavanderia.DataBind();
                }

                if (temRotaAcabamento)
                {
                    listaAcabamento = ObterFaccaoHistorico(ddlColecao.SelectedValue.Trim(), 21, 4);
                    gvAcabamento.DataSource = listaAcabamento;
                    gvAcabamento.DataBind();

                    CarregarGradeCorte();
                }

                listaOutros = ObterFaccaoHistoricoOutros(ddlColecao.SelectedValue.Trim(), 21, 4);
                gvOutros.DataSource = listaOutros;
                gvOutros.DataBind();

                listaLogistica = ObterFaccaoHistorico(ddlColecao.SelectedValue.Trim(), 22, 8);
                gvLogistica.DataSource = listaLogistica;
                gvLogistica.DataBind();

                if ((listaEstamparia == null || listaEstamparia.Count() <= 0) && (listaFaccao == null || listaFaccao.Count() <= 0) && (listaAcabamento == null || listaAcabamento.Count() <= 0))
                {
                    labErro.Text = "Nenhum registro encontrado. Refaça sua pesquisa.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        private void LimparGrids()
        {
            //Limpar grids
            gvEstamparia.DataSource = new List<SP_OBTER_FACCAO_HISTORICOResult>();
            gvEstamparia.DataBind();
            gvFaccao.DataSource = new List<SP_OBTER_FACCAO_HISTORICOResult>();
            gvFaccao.DataBind();
            gvLavanderia.DataSource = new List<SP_OBTER_FACCAO_HISTORICOResult>();
            gvLavanderia.DataBind();
            gvAcabamento.DataSource = new List<SP_OBTER_FACCAO_HISTORICOResult>();
            gvAcabamento.DataBind();
            gvOutros.DataSource = new List<SP_OBTER_FACCAO_HISTORICO_OUTROSResult>();
            gvOutros.DataBind();
            gvLogistica.DataSource = new List<SP_OBTER_FACCAO_HISTORICOResult>();
            gvLogistica.DataBind();
            gvGradeCorte.DataSource = new List<PROD_HB_GRADE>();
            gvGradeCorte.DataBind();
        }

        protected void gvHistoricoEstamparia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (_gradeNome != null)
                {
                    e.Row.Cells[6].Text = _gradeNome.GRADE_EXP;
                    e.Row.Cells[7].Text = _gradeNome.GRADE_XP;
                    e.Row.Cells[8].Text = _gradeNome.GRADE_PP;
                    e.Row.Cells[9].Text = _gradeNome.GRADE_P;
                    e.Row.Cells[10].Text = _gradeNome.GRADE_M;
                    e.Row.Cells[11].Text = _gradeNome.GRADE_G;
                    e.Row.Cells[12].Text = _gradeNome.GRADE_GG;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    LoadGridData(e);
                }
            }
        }
        protected void gvHistoricoFaccao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (_gradeNome != null)
                {
                    e.Row.Cells[6].Text = _gradeNome.GRADE_EXP;
                    e.Row.Cells[7].Text = _gradeNome.GRADE_XP;
                    e.Row.Cells[8].Text = _gradeNome.GRADE_PP;
                    e.Row.Cells[9].Text = _gradeNome.GRADE_P;
                    e.Row.Cells[10].Text = _gradeNome.GRADE_M;
                    e.Row.Cells[11].Text = _gradeNome.GRADE_G;
                    e.Row.Cells[12].Text = _gradeNome.GRADE_GG;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    LoadGridData(e);
                }
            }
        }
        protected void gvHistoricoLavanderia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (_gradeNome != null)
                {
                    e.Row.Cells[6].Text = _gradeNome.GRADE_EXP;
                    e.Row.Cells[7].Text = _gradeNome.GRADE_XP;
                    e.Row.Cells[8].Text = _gradeNome.GRADE_PP;
                    e.Row.Cells[9].Text = _gradeNome.GRADE_P;
                    e.Row.Cells[10].Text = _gradeNome.GRADE_M;
                    e.Row.Cells[11].Text = _gradeNome.GRADE_G;
                    e.Row.Cells[12].Text = _gradeNome.GRADE_GG;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    LoadGridData(e);
                }
            }
        }
        protected void gvHistoricoAcabamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (_gradeNome != null)
                {
                    e.Row.Cells[6].Text = _gradeNome.GRADE_EXP;
                    e.Row.Cells[7].Text = _gradeNome.GRADE_XP;
                    e.Row.Cells[8].Text = _gradeNome.GRADE_PP;
                    e.Row.Cells[9].Text = _gradeNome.GRADE_P;
                    e.Row.Cells[10].Text = _gradeNome.GRADE_M;
                    e.Row.Cells[11].Text = _gradeNome.GRADE_G;
                    e.Row.Cells[12].Text = _gradeNome.GRADE_GG;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    LoadGridData(e);
                }
            }
        }
        protected void gvHistoricoOutros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (_gradeNome != null)
                {
                    e.Row.Cells[6].Text = _gradeNome.GRADE_EXP;
                    e.Row.Cells[7].Text = _gradeNome.GRADE_XP;
                    e.Row.Cells[8].Text = _gradeNome.GRADE_PP;
                    e.Row.Cells[9].Text = _gradeNome.GRADE_P;
                    e.Row.Cells[10].Text = _gradeNome.GRADE_M;
                    e.Row.Cells[11].Text = _gradeNome.GRADE_G;
                    e.Row.Cells[12].Text = _gradeNome.GRADE_GG;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_HISTORICO_OUTROSResult histFaccao = e.Row.DataItem as SP_OBTER_FACCAO_HISTORICO_OUTROSResult;

                    if (histFaccao != null)
                    {

                        Label _labData = e.Row.FindControl("labData") as Label;
                        if (_labData != null)
                            _labData.Text = histFaccao.DATA;

                        Label _labServico = e.Row.FindControl("labServico") as Label;
                        if (_labServico != null)
                            _labServico.Text = new ProducaoController().ObterServicoProducao(histFaccao.PROD_SERVICO).DESCRICAO;

                        if (histFaccao.GRADE.Trim().Equals("INICIAL") || histFaccao.GRADE.Trim().Equals("RETRABALHO"))
                        {
                            Label _labCusto = e.Row.FindControl("labCusto") as Label;
                            if (_labCusto != null)
                                _labCusto.Text = "R$ " + histFaccao.CUSTO_PECA.ToString("###,###,###,##0.00");
                        }

                        if (histFaccao.GRADE.Trim().Equals("PERDA"))
                            e.Row.ForeColor = Color.Red;

                        if (histFaccao.GRADE.Trim().Equals("RETRABALHO"))
                        {
                            e.Row.BackColor = Color.MediumPurple;
                            e.Row.ForeColor = Color.White;
                        }

                        if (histFaccao.GRADE.Trim().Equals("INICIAL"))
                            e.Row.BackColor = Color.Plum;


                    }
                }
            }
        }
        protected void gvHistoricoLogistica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (_gradeNome != null)
                {
                    e.Row.Cells[6].Text = _gradeNome.GRADE_EXP;
                    e.Row.Cells[7].Text = _gradeNome.GRADE_XP;
                    e.Row.Cells[8].Text = _gradeNome.GRADE_PP;
                    e.Row.Cells[9].Text = _gradeNome.GRADE_P;
                    e.Row.Cells[10].Text = _gradeNome.GRADE_M;
                    e.Row.Cells[11].Text = _gradeNome.GRADE_G;
                    e.Row.Cells[12].Text = _gradeNome.GRADE_GG;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    LoadGridData(e);
                }
            }
        }

        private void LoadGridData(GridViewRowEventArgs e)
        {
            SP_OBTER_FACCAO_HISTORICOResult histFaccao = e.Row.DataItem as SP_OBTER_FACCAO_HISTORICOResult;

            if (histFaccao != null)
            {

                Label _labData = e.Row.FindControl("labData") as Label;
                if (_labData != null)
                    _labData.Text = histFaccao.DATA;

                Label _labServico = e.Row.FindControl("labServico") as Label;
                if (_labServico != null)
                    _labServico.Text = new ProducaoController().ObterServicoProducao(histFaccao.PROD_SERVICO).DESCRICAO;

                if (histFaccao.GRADE.Trim().Equals("INICIAL") || histFaccao.GRADE.Trim().Equals("RETRABALHO"))
                {
                    Label _labCusto = e.Row.FindControl("labCusto") as Label;
                    if (_labCusto != null)
                        _labCusto.Text = "R$ " + histFaccao.CUSTO_PECA.ToString("###,###,###,##0.00");
                }

                if (histFaccao.GRADE.Trim().Equals("PERDA"))
                    e.Row.ForeColor = Color.Red;

                if (histFaccao.GRADE.Trim().Equals("RETRABALHO"))
                {
                    e.Row.BackColor = Color.MediumPurple;
                    e.Row.ForeColor = Color.White;
                }

                TextBox txtDataEntrega = e.Row.FindControl("txtDataEntrega") as TextBox;
                if (txtDataEntrega != null)
                {
                    txtDataEntrega.Visible = false;
                    if (histFaccao.GRADE.Trim().Equals("INICIAL") || histFaccao.GRADE.Trim().Equals("RETRABALHO"))
                    {
                        txtDataEntrega.Visible = true;
                        if (histFaccao.DATA_PREV_ENTREGA != null)
                            txtDataEntrega.Text = Convert.ToDateTime(histFaccao.DATA_PREV_ENTREGA).ToString("dd/MM/yyyy");
                    }
                }

                if (histFaccao.GRADE.Trim().Equals("INICIAL"))
                    e.Row.BackColor = Color.Plum;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();
            }
        }
        #endregion

        private void CarregarGradeCorte()
        {
            var grade = new List<PROD_HB_GRADE>();

            var gradeCorte = prodController.ObterGradeHB(Convert.ToInt32(hidCodigoHB.Value), 3);
            var gradeAtacado = prodController.ObterGradeHB(Convert.ToInt32(hidCodigoHB.Value), 99);
            var gradeVarejo = new PROD_HB_GRADE();
            if (gradeAtacado != null)
            {
                gradeVarejo.PROD_PROCESSO = 98;
                gradeVarejo.GRADE_EXP = gradeCorte.GRADE_EXP - gradeAtacado.GRADE_EXP;
                gradeVarejo.GRADE_XP = gradeCorte.GRADE_XP - gradeAtacado.GRADE_XP;
                gradeVarejo.GRADE_PP = gradeCorte.GRADE_PP - gradeAtacado.GRADE_PP;
                gradeVarejo.GRADE_P = gradeCorte.GRADE_P - gradeAtacado.GRADE_P;
                gradeVarejo.GRADE_M = gradeCorte.GRADE_M - gradeAtacado.GRADE_M;
                gradeVarejo.GRADE_G = gradeCorte.GRADE_G - gradeAtacado.GRADE_G;
                gradeVarejo.GRADE_GG = gradeCorte.GRADE_GG - gradeAtacado.GRADE_GG;
            }
            else
            {
                gradeVarejo.PROD_PROCESSO = 98;
                gradeVarejo.GRADE_EXP = gradeCorte.GRADE_EXP;
                gradeVarejo.GRADE_XP = gradeCorte.GRADE_XP;
                gradeVarejo.GRADE_PP = gradeCorte.GRADE_PP;
                gradeVarejo.GRADE_P = gradeCorte.GRADE_P;
                gradeVarejo.GRADE_M = gradeCorte.GRADE_M;
                gradeVarejo.GRADE_G = gradeCorte.GRADE_G;
                gradeVarejo.GRADE_GG = gradeCorte.GRADE_GG;
            }

            if (gradeAtacado != null)
                grade.Add(gradeAtacado);
            if (gradeVarejo != null)
                grade.Add(gradeVarejo);

            gvGradeCorte.DataSource = grade;
            gvGradeCorte.DataBind();
        }
        protected void gvGradeCorte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (_gradeNome != null)
                {
                    e.Row.Cells[2].Text = _gradeNome.GRADE_EXP;
                    e.Row.Cells[3].Text = _gradeNome.GRADE_XP;
                    e.Row.Cells[4].Text = _gradeNome.GRADE_PP;
                    e.Row.Cells[5].Text = _gradeNome.GRADE_P;
                    e.Row.Cells[6].Text = _gradeNome.GRADE_M;
                    e.Row.Cells[7].Text = _gradeNome.GRADE_G;
                    e.Row.Cells[8].Text = _gradeNome.GRADE_GG;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_GRADE grade = e.Row.DataItem as PROD_HB_GRADE;

                    if (grade != null)
                    {

                        Label _labGrade = e.Row.FindControl("labGrade") as Label;
                        if (_labGrade != null)
                            _labGrade.Text = (grade.PROD_PROCESSO == 99) ? "Atacado" : "Varejo";

                        Label _labGradeTotal = e.Row.FindControl("labGradeTotal") as Label;
                        if (_labGradeTotal != null)
                            _labGradeTotal.Text = (grade.GRADE_EXP + grade.GRADE_XP + grade.GRADE_PP + grade.GRADE_P + grade.GRADE_M + grade.GRADE_G + grade.GRADE_GG).ToString();
                    }
                }
            }
        }

        protected void txtDataEntrega_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                if (txt != null)
                {
                    GridViewRow row = (GridViewRow)txt.NamingContainer;
                    if (row != null)
                    {
                        GridView gv = (GridView)row.Parent.Parent;
                        var codigoSaida = gv.DataKeys[row.RowIndex].Value.ToString();

                        txt.BackColor = Color.White;
                        DateTime dataEntrega;
                        if (!DateTime.TryParse(txt.Text.Trim(), out dataEntrega))
                        {
                            txt.BackColor = Color.Red;
                            return;
                        }

                        var saida = faccController.ObterSaidaHB(Convert.ToInt32(codigoSaida));
                        if (saida != null)
                        {
                            saida.DATA_PREV_ENTREGA = dataEntrega;
                            faccController.AtualizarSaidaHB(saida);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
