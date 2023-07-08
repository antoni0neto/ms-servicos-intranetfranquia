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

namespace Relatorios
{
    public partial class prod_cad_hb_libera_faccao : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                btLiberar.Visible = false;
            }

            //Evitar duplo clique no botão
            btLiberar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btLiberar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            char mostruario = ' ';

            try
            {
                labErro.Text = "";
                labErroLiberar.Text = "";
                btLiberar.Visible = false;
                if (ddlColecao.SelectedValue == "" || ddlColecao.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }
                if ((txtHB.Text == "" || Convert.ToInt32(txtHB.Text) <= 0))
                {
                    labErro.Text = "Informe o número do HB.";
                    return;
                }

                mostruario = (chkMostruario.Checked) ? 'S' : 'N';

                PROD_HB prod_hb = prodController.ObterNumeroHB(ddlColecao.SelectedValue.Trim(), Convert.ToInt32(txtHB.Text.Trim())).Where(p => p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).SingleOrDefault();

                if (prod_hb == null)
                {
                    labErro.Text = "HB não encontrado. Refaça sua pesquisa.";
                    return;
                }

                if (prod_hb.NUMERO_PEDIDO == 2028)
                {
                    labErro.Text = "HB não pode ser enviado para Produção. Pedido do HB 2028. Este pedido não controla Produção.";
                    return;
                }

                if (prod_hb.STATUS != 'B')
                {
                    labErro.Text = "HB ainda não foi Cortado.";
                    return;
                }

                if (prod_hb.ORDEM_PRODUCAO != null && prod_hb.ORDEM_PRODUCAO.Trim() != "")
                {
                    labErro.Text = "HB já possui ORDEM DE PRODUÇÃO. O.P.: " + prod_hb.ORDEM_PRODUCAO + ".";
                    return;
                }

                List<PROD_HB> listaHB = new List<PROD_HB>();
                listaHB.Add(prod_hb);

                gvFaccaoHB.DataSource = listaHB;
                gvFaccaoHB.DataBind();

                btLiberar.Visible = true;

                Session["COLECAO"] = ddlColecao.SelectedValue;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvFaccaoHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB prod_hb = e.Row.DataItem as PROD_HB;

                    coluna += 1;
                    if (prod_hb != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                            _litNome.Text = prod_hb.NOME;

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(prod_hb.COR).DESC_COR;

                        Literal _litQtde = e.Row.FindControl("litQtde") as Literal;
                        if (_litQtde != null)
                            _litQtde.Text = prodController.ObterQtdeGradeHB(prod_hb.CODIGO, 3).ToString();

                    }
                }
            }
        }
        protected void btLiberar_Click(object sender, EventArgs e)
        {
            try
            {
                labErroLiberar.Text = "";
                CheckBox cbEstamparia = null;
                CheckBox cbLavanderia = null;
                CheckBox cbFaccao = null;
                CheckBox cbAcabamento = null;

                FaccaoController faccController = new FaccaoController();

                int codigoHB = 0;
                foreach (GridViewRow row in gvFaccaoHB.Rows)
                {
                    codigoHB = Convert.ToInt32(gvFaccaoHB.DataKeys[row.RowIndex].Value);

                    var prod_hb = prodController.ObterHB(codigoHB);

                    if (prod_hb == null)
                    {
                        labErroLiberar.Text = "HB não encontrado. Entre em contato com TI.";
                        return;
                    }

                    cbEstamparia = row.FindControl("cbEstamparia") as CheckBox;
                    cbLavanderia = row.FindControl("cbLavanderia") as CheckBox;
                    cbFaccao = row.FindControl("cbFaccao") as CheckBox;
                    cbAcabamento = row.FindControl("cbAcabamento") as CheckBox;

                    //Gerar registro de Facção
                    var validaSaida = faccController.ObterSaidaHB(codigoHB.ToString());
                    if (validaSaida == null)
                    {
                        PROD_HB_SAIDA _saida = new PROD_HB_SAIDA();
                        _saida.PROD_HB = codigoHB;
                        _saida.DATA_INCLUSAO = DateTime.Now;
                        _saida.PROD_PROCESSO = 20;
                        var gradePrincipal = prodController.ObterGradeHB(codigoHB, 3);
                        _saida.GRADE_EXP = (gradePrincipal != null) ? gradePrincipal.GRADE_EXP : 0;
                        _saida.GRADE_XP = (gradePrincipal != null) ? gradePrincipal.GRADE_XP : 0;
                        _saida.GRADE_PP = (gradePrincipal != null) ? gradePrincipal.GRADE_PP : 0;
                        _saida.GRADE_P = (gradePrincipal != null) ? gradePrincipal.GRADE_P : 0;
                        _saida.GRADE_M = (gradePrincipal != null) ? gradePrincipal.GRADE_M : 0;
                        _saida.GRADE_G = (gradePrincipal != null) ? gradePrincipal.GRADE_G : 0;
                        _saida.GRADE_GG = (gradePrincipal != null) ? gradePrincipal.GRADE_GG : 0;
                        _saida.GRADE_TOTAL = (_saida.GRADE_EXP + _saida.GRADE_XP + _saida.GRADE_PP + _saida.GRADE_P + _saida.GRADE_M + _saida.GRADE_G + _saida.GRADE_GG);
                        _saida.SALDO = 'S';
                        _saida.CODIGO = faccController.InserirSaidaHB(_saida);

                        PROD_HB_LIBERADO _liberado = new PROD_HB_LIBERADO();
                        _liberado.PROD_HB = codigoHB;
                        _liberado.PROD_HB_SAIDA = _saida.CODIGO;
                        _liberado.CODIGO_USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        _liberado.DATA_LIBERACAO = DateTime.Now;
                        _liberado.VERIFICADO = 'N';
                        _liberado.DATA_VERIFICADO = null;
                        faccController.InserirHBLiberado(_liberado);
                    }

                    var retornoRota = faccController.GerarRotaModeloProduto(prod_hb.CODIGO_PRODUTO_LINX, prod_hb.COR, true, false, cbEstamparia.Checked, cbLavanderia.Checked, cbFaccao.Checked, cbAcabamento.Checked, true, true, false, false, false, false, false);
                    if (retornoRota.MENSAGEM_ERRO.Trim() != "")
                    {
                        throw new Exception("ERRO (GerarRotaModeloProduto): COD. " + retornoRota.CODIGO_ERRO + ", MSG. " + retornoRota.MENSAGEM_ERRO);
                    }

                    var retornoFacc = faccController.GerarOrdemProducao(prod_hb.CODIGO);
                    if (retornoFacc == null || retornoFacc.MENSAGEM_ERRO.Trim() != "")
                    {
                        throw new Exception("ERRO (GerarOrdemProducao): COD. " + retornoFacc.CODIGO_ERRO + ", MSG. " + retornoFacc.MENSAGEM_ERRO);
                    }

                    prod_hb.ORDEM_PRODUCAO = retornoFacc.CODIGO_ERRO.ToString();
                    prodController.AtualizarHB(prod_hb);


                }

                btLiberar.Visible = false;
                gvFaccaoHB.DataSource = new List<PROD_HB>();
                gvFaccaoHB.DataBind();

                labErroLiberar.Text = "HB Liberado com Sucesso.";

            }
            catch (Exception ex)
            {
                labErroLiberar.Text = ex.Message;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecao.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        #endregion

    }
}
