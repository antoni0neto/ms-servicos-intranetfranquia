using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using System.Collections;

namespace Relatorios
{
    public partial class prod_cad_hb_ampliacao : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int colAmpliacaoElastico, colAmpliacaoGalao, colAmpliacaoAlcaPronta = 0;

        PROD_GRADE _gradeNome = new PROD_GRADE();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["c"] == null || Request.QueryString["c"] == "")
                    Response.Redirect("prod_menu.aspx");

                CarregarGrupo();
                CarregarColecoes();

                //Baixa Ampliacao
                string codigo = Request.QueryString["c"].ToString();
                hidHB.Value = codigo;

                PROD_HB prod_hb = prodController.ObterHB(Convert.ToInt32(codigo));
                if (prod_hb != null)
                {
                    ddlColecoes.SelectedValue = prod_hb.COLECAO;
                    txtHB.Text = prod_hb.HB.ToString();
                    txtCodigoLinx.Text = prod_hb.CODIGO_PRODUTO_LINX.ToString();
                    ddlGrupo.SelectedValue = prod_hb.GRUPO;
                    txtNome.Text = prod_hb.NOME;
                    txtData.Text = prod_hb.DATA_INCLUSAO.ToString("dd/MM/yyyy");
                    txtOutro.Text = prod_hb.AMPLIACAO_OUTRO;

                    hrefVoltar.HRef = "prod_cad_hb_altera_filtro.aspx?d=3";

                    _gradeNome = prodController.ObterGradeNome(Convert.ToInt32(prod_hb.PROD_GRADE));

                    CarregarAmpliacaoMedidas();
                }

                dialogPai.Visible = false;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("01");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "Selecione" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        #endregion

        #region "HB"
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                labErroEnvio.Text = "";

                InserirMedidasAmpliacao();

                labHBPopUp.Text = txtHB.Text;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, Risco: function () { window.open('prod_fila_risco.aspx', '_self'); }, Corte: function () { window.open('prod_fila_corte.aspx', '_self'); }, 'Rel. Aviamento': function () { window.open('prod_rel_aviamento.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labErroEnvio.Text = "Erro (btEnviar_Click): " + ex.Message;
            }
        }
        #endregion

        #region "AMPLIACAO"
        private void InserirMedidasAmpliacao()
        {
            int codigo = 0;
            bool bInserir = false;

            PROD_HB_AMPLIACAO_MEDIDA ampliacaoMedida = null;

            //ELASTICO
            foreach (GridViewRow row in gvElastico.Rows)
            {
                if (row != null)
                {
                    TextBox _txtLocal = row.FindControl("txtLocal") as TextBox;
                    TextBox _txtGradeEXP = row.FindControl("txtGradeEXP") as TextBox;
                    TextBox _txtGradeXP = row.FindControl("txtGradeXP") as TextBox;
                    TextBox _txtGradePP = row.FindControl("txtGradePP") as TextBox;
                    TextBox _txtGradeP = row.FindControl("txtGradeP") as TextBox;
                    TextBox _txtGradeM = row.FindControl("txtGradeM") as TextBox;
                    TextBox _txtGradeG = row.FindControl("txtGradeG") as TextBox;
                    TextBox _txtGradeGG = row.FindControl("txtGradeGG") as TextBox;
                    TextBox _txtLargura = row.FindControl("txtLargura") as TextBox;

                    ampliacaoMedida = new PROD_HB_AMPLIACAO_MEDIDA();

                    //Obter codigo tabela PROD_HB_AMPLIACAO_MEDIDA
                    codigo = Convert.ToInt32(gvElastico.DataKeys[row.RowIndex].Value);
                    ampliacaoMedida.CODIGO = codigo;
                    ampliacaoMedida.PROD_HB = Convert.ToInt32(hidHB.Value);
                    ampliacaoMedida.PROD_HB_AMPLIACAO = 1; //ELASTICO
                    ampliacaoMedida.LOCAL = _txtLocal.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_EXP = _txtGradeEXP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_XP = _txtGradeXP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_PP = _txtGradePP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_P = _txtGradeP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_M = _txtGradeM.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_G = _txtGradeG.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_GG = _txtGradeGG.Text.Trim().ToUpper();
                    ampliacaoMedida.LARGURA = _txtLargura.Text.Trim().ToUpper();
                    ampliacaoMedida.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    ampliacaoMedida.DATA_INCLUSAO = DateTime.Now;

                    bInserir = false;
                    if ((_txtLocal.Text.Trim() != "") || (_txtGradeEXP.Text.Trim() != "") || (_txtGradeXP.Text.Trim() != "") || (_txtGradePP.Text.Trim() != "") || (_txtGradeP.Text.Trim() != "") || (_txtGradeM.Text.Trim() != "") || (_txtGradeG.Text.Trim() != "") || (_txtGradeGG.Text.Trim() != "") || (_txtLargura.Text.Trim() != ""))
                        bInserir = true;

                    if (bInserir) //Inserir/Alterar
                    {
                        if (codigo <= 0)
                            prodController.InserirAmpliacaoMedidasHB(ampliacaoMedida);
                        else
                            prodController.AtualizarAmpliacaoMedidasHB(ampliacaoMedida);
                    }
                    else
                    {
                        prodController.ExcluirAmpliacaoMedidasHB(codigo);
                    }

                }
            }

            //GALÃO
            foreach (GridViewRow row in gvGalao.Rows)
            {
                if (row != null)
                {
                    TextBox _txtQtde = row.FindControl("txtQtde") as TextBox;
                    TextBox _txtLocal = row.FindControl("txtLocal") as TextBox;
                    TextBox _txtComprimento = row.FindControl("txtComprimento") as TextBox;
                    TextBox _txtLargura = row.FindControl("txtLargura") as TextBox;

                    ampliacaoMedida = new PROD_HB_AMPLIACAO_MEDIDA();

                    //Obter codigo tabela PROD_HB_AMPLIACAO_MEDIDA
                    codigo = Convert.ToInt32(gvGalao.DataKeys[row.RowIndex].Value);
                    ampliacaoMedida.CODIGO = codigo;
                    ampliacaoMedida.PROD_HB = Convert.ToInt32(hidHB.Value);
                    ampliacaoMedida.PROD_HB_AMPLIACAO = 2; // GALAO
                    ampliacaoMedida.QTDE = _txtQtde.Text.Trim().ToUpper();
                    ampliacaoMedida.LOCAL = _txtLocal.Text.Trim().ToUpper();
                    ampliacaoMedida.COMPRIMENTO = _txtComprimento.Text.Trim().ToUpper();
                    ampliacaoMedida.LARGURA = _txtLargura.Text.Trim().ToUpper();
                    ampliacaoMedida.DESCRICAO = txtGalaoDescricao.Text.Trim().ToUpper();
                    ampliacaoMedida.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    ampliacaoMedida.DATA_INCLUSAO = DateTime.Now;

                    bInserir = false;
                    if ((_txtQtde.Text.Trim() != "") || (_txtLocal.Text.Trim() != "") || (_txtComprimento.Text.Trim() != "") || (_txtLargura.Text.Trim() != ""))
                        bInserir = true;

                    if (bInserir) //Inserir/Alterar
                    {
                        if (codigo <= 0)
                            prodController.InserirAmpliacaoMedidasHB(ampliacaoMedida);
                        else
                            prodController.AtualizarAmpliacaoMedidasHB(ampliacaoMedida);
                    }
                    else
                    {
                        prodController.ExcluirAmpliacaoMedidasHB(codigo);
                    }

                }
            }

            //ALCA PRONTA
            foreach (GridViewRow row in gvAlcaPronta.Rows)
            {
                if (row != null)
                {
                    TextBox _txtGradeEXP = row.FindControl("txtGradeEXP") as TextBox;
                    TextBox _txtGradeXP = row.FindControl("txtGradeXP") as TextBox;
                    TextBox _txtGradePP = row.FindControl("txtGradePP") as TextBox;
                    TextBox _txtGradeP = row.FindControl("txtGradeP") as TextBox;
                    TextBox _txtGradeM = row.FindControl("txtGradeM") as TextBox;
                    TextBox _txtGradeG = row.FindControl("txtGradeG") as TextBox;
                    TextBox _txtGradeGG = row.FindControl("txtGradeGG") as TextBox;

                    ampliacaoMedida = new PROD_HB_AMPLIACAO_MEDIDA();

                    ///Obter codigo tabela PROD_HB_AMPLIACAO_MEDIDA
                    codigo = Convert.ToInt32(gvAlcaPronta.DataKeys[row.RowIndex].Value);
                    ampliacaoMedida.CODIGO = codigo;
                    ampliacaoMedida.PROD_HB = Convert.ToInt32(hidHB.Value);
                    ampliacaoMedida.PROD_HB_AMPLIACAO = 3; // ALÇA PRONTA
                    ampliacaoMedida.GRADE_EXP = _txtGradeEXP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_XP = _txtGradeXP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_PP = _txtGradePP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_P = _txtGradeP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_M = _txtGradeM.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_G = _txtGradeG.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_GG = _txtGradeGG.Text.Trim().ToUpper();
                    ampliacaoMedida.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    ampliacaoMedida.DATA_INCLUSAO = DateTime.Now;

                    bInserir = false;
                    if ((_txtGradeEXP.Text.Trim() != "") || (_txtGradeXP.Text.Trim() != "") || (_txtGradePP.Text.Trim() != "") || (_txtGradeP.Text.Trim() != "") || (_txtGradeM.Text.Trim() != "") || (_txtGradeG.Text.Trim() != "") || (_txtGradeGG.Text.Trim() != ""))
                        bInserir = true;

                    if (bInserir) //Inserir/Alterar
                    {
                        if (codigo <= 0)
                            prodController.InserirAmpliacaoMedidasHB(ampliacaoMedida);
                        else
                            prodController.AtualizarAmpliacaoMedidasHB(ampliacaoMedida);
                    }
                    else
                    {
                        prodController.ExcluirAmpliacaoMedidasHB(codigo);
                    }

                }
            }

            //ATUALIZAR CAMPO OUTRO
            PROD_HB hbOutro = new PROD_HB();
            hbOutro = prodController.ObterHB(Convert.ToInt32(hidHB.Value));
            if (hbOutro != null)
            {
                hbOutro.AMPLIACAO_OUTRO = txtOutro.Text.Trim();
                //Atualizar HB
                prodController.AtualizarHB(hbOutro);
            }
        }
        private void CarregarAmpliacaoMedidas()
        {
            List<PROD_HB_AMPLIACAO_MEDIDA> lstAmpliacaoMedidaElastico = new List<PROD_HB_AMPLIACAO_MEDIDA>();
            List<PROD_HB_AMPLIACAO_MEDIDA> lstAmpliacaoMedidaGalao = new List<PROD_HB_AMPLIACAO_MEDIDA>();
            List<PROD_HB_AMPLIACAO_MEDIDA> lstAmpliacaoMedidaAlcaPronta = new List<PROD_HB_AMPLIACAO_MEDIDA>();

            /*ELASTICO*/
            lstAmpliacaoMedidaElastico = prodController.ObterAmpliacaoMedidasHB().Where(p => p.PROD_HB == Convert.ToInt32(hidHB.Value) && p.PROD_HB_AMPLIACAO == 1).ToList();
            if (lstAmpliacaoMedidaElastico.Count < 6)
            {
                int i = 6 - lstAmpliacaoMedidaElastico.Count;
                for (; i > 0; i--)
                    lstAmpliacaoMedidaElastico.Add(new PROD_HB_AMPLIACAO_MEDIDA { CODIGO = 0 });
            }
            gvElastico.DataSource = lstAmpliacaoMedidaElastico;
            gvElastico.DataBind();
            /************************************************************************************/

            /*GALAO*/
            lstAmpliacaoMedidaGalao = prodController.ObterAmpliacaoMedidasHB().Where(p => p.PROD_HB == Convert.ToInt32(hidHB.Value) && p.PROD_HB_AMPLIACAO == 2).ToList();
            if (lstAmpliacaoMedidaGalao.Count < 6)
            {
                int i = 6 - lstAmpliacaoMedidaGalao.Count;
                for (; i > 0; i--)
                    lstAmpliacaoMedidaGalao.Add(new PROD_HB_AMPLIACAO_MEDIDA { CODIGO = 0 });
            }
            gvGalao.DataSource = lstAmpliacaoMedidaGalao;
            gvGalao.DataBind();

            txtGalaoDescricao.Text = lstAmpliacaoMedidaGalao[0].DESCRICAO;
            /************************************************************************************/

            /*ALÇA PRONTA*/
            lstAmpliacaoMedidaAlcaPronta = prodController.ObterAmpliacaoMedidasHB().Where(p => p.PROD_HB == Convert.ToInt32(hidHB.Value) && p.PROD_HB_AMPLIACAO == 3).ToList();
            if (lstAmpliacaoMedidaAlcaPronta.Count < 6)
            {
                int i = 6 - lstAmpliacaoMedidaAlcaPronta.Count;
                for (; i > 0; i--)
                    lstAmpliacaoMedidaAlcaPronta.Add(new PROD_HB_AMPLIACAO_MEDIDA { CODIGO = 0 });
            }
            gvAlcaPronta.DataSource = lstAmpliacaoMedidaAlcaPronta;
            gvAlcaPronta.DataBind();
            /************************************************************************************/
        }
        protected void gvElastico_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    PROD_HB_AMPLIACAO_MEDIDA _ampliacaoElastico = e.Row.DataItem as PROD_HB_AMPLIACAO_MEDIDA;

                    colAmpliacaoElastico += 1;
                    if (_ampliacaoElastico != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colAmpliacaoElastico.ToString();

                        TextBox _txtLocal = e.Row.FindControl("txtLocal") as TextBox;
                        if (_txtLocal != null)
                            _txtLocal.Text = _ampliacaoElastico.LOCAL;

                        TextBox _txtGradeEXP = e.Row.FindControl("txtGradeEXP") as TextBox;
                        if (_txtGradeEXP != null)
                            _txtGradeEXP.Text = _ampliacaoElastico.GRADE_EXP;

                        TextBox _txtGradeXP = e.Row.FindControl("txtGradeXP") as TextBox;
                        if (_txtGradeXP != null)
                            _txtGradeXP.Text = _ampliacaoElastico.GRADE_XP;

                        TextBox _txtGradePP = e.Row.FindControl("txtGradePP") as TextBox;
                        if (_txtGradePP != null)
                            _txtGradePP.Text = _ampliacaoElastico.GRADE_PP;

                        TextBox _txtGradeP = e.Row.FindControl("txtGradeP") as TextBox;
                        if (_txtGradeP != null)
                        {
                            _txtGradeP.Text = _ampliacaoElastico.GRADE_P;
                            if (_gradeNome.GRADE_P == "-")
                                _txtGradeP.Enabled = false;
                        }

                        TextBox _txtGradeM = e.Row.FindControl("txtGradeM") as TextBox;
                        if (_txtGradeM != null)
                        {
                            _txtGradeM.Text = _ampliacaoElastico.GRADE_M;
                            if (_gradeNome.GRADE_M == "-")
                                _txtGradeM.Enabled = false;
                        }

                        TextBox _txtGradeG = e.Row.FindControl("txtGradeG") as TextBox;
                        if (_txtGradeG != null)
                        {
                            _txtGradeG.Text = _ampliacaoElastico.GRADE_G;
                            if (_gradeNome.GRADE_G == "-")
                                _txtGradeG.Enabled = false;
                        }

                        TextBox _txtGradeGG = e.Row.FindControl("txtGradeGG") as TextBox;
                        if (_txtGradeGG != null)
                        {
                            _txtGradeGG.Text = _ampliacaoElastico.GRADE_GG;
                            if (_gradeNome.GRADE_GG == "-")
                                _txtGradeGG.Enabled = false;
                        }

                        TextBox _txtLargura = e.Row.FindControl("txtLargura") as TextBox;
                        if (_txtLargura != null)
                            _txtLargura.Text = _ampliacaoElastico.LARGURA;

                    }
                }
            }
        }
        protected void gvGalao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_AMPLIACAO_MEDIDA _ampliacaoGalao = e.Row.DataItem as PROD_HB_AMPLIACAO_MEDIDA;

                    colAmpliacaoGalao += 1;
                    if (_ampliacaoGalao != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colAmpliacaoGalao.ToString();

                        TextBox _txtQtde = e.Row.FindControl("txtQtde") as TextBox;
                        if (_txtQtde != null)
                            _txtQtde.Text = _ampliacaoGalao.QTDE;

                        TextBox _txtLocal = e.Row.FindControl("txtLocal") as TextBox;
                        if (_txtLocal != null)
                            _txtLocal.Text = _ampliacaoGalao.LOCAL;

                        TextBox _txtComprimento = e.Row.FindControl("txtComprimento") as TextBox;
                        if (_txtComprimento != null)
                            _txtComprimento.Text = _ampliacaoGalao.COMPRIMENTO;

                        TextBox _txtLargura = e.Row.FindControl("txtLargura") as TextBox;
                        if (_txtLargura != null)
                            _txtLargura.Text = _ampliacaoGalao.LARGURA;
                    }
                }
            }
        }
        protected void gvAlcaPronta_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (_gradeNome != null)
                {
                    e.Row.Cells[1].Text = _gradeNome.GRADE_EXP;
                    e.Row.Cells[2].Text = _gradeNome.GRADE_XP;
                    e.Row.Cells[3].Text = _gradeNome.GRADE_PP;
                    e.Row.Cells[4].Text = _gradeNome.GRADE_P;
                    e.Row.Cells[5].Text = _gradeNome.GRADE_M;
                    e.Row.Cells[6].Text = _gradeNome.GRADE_G;
                    e.Row.Cells[7].Text = _gradeNome.GRADE_GG;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_AMPLIACAO_MEDIDA _ampliacaoAlcaPronta = e.Row.DataItem as PROD_HB_AMPLIACAO_MEDIDA;

                    colAmpliacaoAlcaPronta += 1;
                    if (_ampliacaoAlcaPronta != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colAmpliacaoAlcaPronta.ToString();

                        TextBox _txtGradeEXP = e.Row.FindControl("txtGradeEXP") as TextBox;
                        if (_txtGradeEXP != null)
                            _txtGradeEXP.Text = _ampliacaoAlcaPronta.GRADE_EXP;

                        TextBox _txtGradeXP = e.Row.FindControl("txtGradeXP") as TextBox;
                        if (_txtGradeXP != null)
                            _txtGradeXP.Text = _ampliacaoAlcaPronta.GRADE_XP;

                        TextBox _txtGradePP = e.Row.FindControl("txtGradePP") as TextBox;
                        if (_txtGradePP != null)
                            _txtGradePP.Text = _ampliacaoAlcaPronta.GRADE_PP;

                        TextBox _txtGradeP = e.Row.FindControl("txtGradeP") as TextBox;
                        if (_txtGradeP != null)
                        {
                            _txtGradeP.Text = _ampliacaoAlcaPronta.GRADE_P;
                            if (_gradeNome.GRADE_P == "-")
                                _txtGradeP.Enabled = false;
                        }

                        TextBox _txtGradeM = e.Row.FindControl("txtGradeM") as TextBox;
                        if (_txtGradeM != null)
                        {
                            _txtGradeM.Text = _ampliacaoAlcaPronta.GRADE_M;
                            if (_gradeNome.GRADE_M == "-")
                                _txtGradeM.Enabled = false;
                        }

                        TextBox _txtGradeG = e.Row.FindControl("txtGradeG") as TextBox;
                        if (_txtGradeG != null)
                        {
                            _txtGradeG.Text = _ampliacaoAlcaPronta.GRADE_G;
                            if (_gradeNome.GRADE_G == "-")
                                _txtGradeG.Enabled = false;
                        }

                        TextBox _txtGradeGG = e.Row.FindControl("txtGradeGG") as TextBox;
                        if (_txtGradeGG != null)
                        {
                            _txtGradeGG.Text = _ampliacaoAlcaPronta.GRADE_GG;
                            if (_gradeNome.GRADE_GG == "-")
                                _txtGradeGG.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    if (b.CommandArgument.Trim().ToString() != "")
                    {
                        prodController.ExcluirAmpliacaoMedidasHB(Convert.ToInt32(b.CommandArgument));
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

    }
}
