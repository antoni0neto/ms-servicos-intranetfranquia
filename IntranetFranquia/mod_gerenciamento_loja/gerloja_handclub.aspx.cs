using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;

namespace Relatorios
{
    public partial class gerloja_handclub : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();
                CarregarFilial();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            var usuario = (USUARIO)Session["USUARIO"];

            if (usuario.CODIGO_PERFIL == 3 || usuario.CODIGO_PERFIL == 1)
                filial = baseController.BuscaFiliais();
            else
                filial = baseController.BuscaFiliais(usuario);

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();

                ddlFilial.Enabled = true;
                if (filial.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labAno.ForeColor = _OK;
            if (ddlAno.SelectedValue.Trim() == "")
            {
                labAno.ForeColor = _notOK;
                retorno = false;
            }

            labMes.ForeColor = _OK;
            if (ddlMes.SelectedValue.Trim() == "")
            {
                labMes.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        #region "FLASH HANDCLUB"

        private List<SP_OBTER_FLASH_HANDCLUBResult> ObterFlashHandclub(string filial)
        {
            DateTime dataIni = DateTime.Now;
            DateTime dataFim = DateTime.Now;

            string ano = ddlAno.SelectedValue;
            string mes = ddlMes.SelectedValue;

            dataIni = Convert.ToDateTime(ano + "-" + mes + "-01");
            dataFim = Convert.ToDateTime(ano + "-" + mes + "-" + DateTime.DaysInMonth(int.Parse(ano), int.Parse(mes)).ToString());

            var flashHandclub = new DesempenhoController().ObterFlashHandclub(dataIni, dataFim, filial);
            return flashHandclub;
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho.";
                    return;
                }

                CarregarFlashHandclub();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregarFlashHandclub()
        {
            string codigoFilial = "";

            var handclub = ObterFlashHandclub("");

            //Carregar total primeiro
            CarregarFlashHandclubTotal(handclub);

            codigoFilial = ddlFilial.SelectedValue.Trim();
            if (codigoFilial != "")
                handclub = handclub.Where(p => p.CODIGO_FILIAL.Trim() == codigoFilial).ToList();

            labTituloAnterior.Text = ddlMes.Items[ddlMes.SelectedIndex - 1].Text;
            labTituloAtual.Text = ddlMes.SelectedItem.Text;

            gvFlashHandclub.DataSource = handclub;
            gvFlashHandclub.DataBind();
        }
        protected void gvFlashHandclub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FLASH_HANDCLUBResult handclub = e.Row.DataItem as SP_OBTER_FLASH_HANDCLUBResult;

                    if (handclub != null)
                    {
                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = AlterarFonte(handclub.FILIAL);

                        /* MES ANTERIOR */
                        Literal _litPHandclub_1 = e.Row.FindControl("litPHandclub_1") as Literal;
                        if (_litPHandclub_1 != null)
                            _litPHandclub_1.Text = AlterarFonte(Convert.ToDecimal(handclub.HANDCLUB_PORC_1).ToString("##0.00") + "%");
                        Literal _litPFatorCrescimento_1 = e.Row.FindControl("litPFatorCrescimento_1") as Literal;
                        if (_litPFatorCrescimento_1 != null)
                            _litPFatorCrescimento_1.Text = AlterarFonte(Convert.ToDecimal(handclub.FATOR_CRESCIMENTO_1).ToString("##0.00"));
                        Literal _litPResgateHandclub_1 = e.Row.FindControl("litPResgateHandclub_1") as Literal;
                        if (_litPResgateHandclub_1 != null)
                            _litPResgateHandclub_1.Text = AlterarFonte(Convert.ToDecimal(handclub.RESGATE_PORC_1).ToString("##0.00") + "%");

                        /* MES ATUAL */
                        Literal _litVendaLiquida_2 = e.Row.FindControl("litVendaLiquida_2") as Literal;
                        if (_litVendaLiquida_2 != null)
                            _litVendaLiquida_2.Text = AlterarFonte(Convert.ToDecimal(handclub.VENDA_LIQUIDA_2).ToString("###,###,###,###,##0.00"));

                        Literal _litVendaHandclub_2 = e.Row.FindControl("litVendaHandclub_2") as Literal;
                        if (_litVendaHandclub_2 != null)
                            _litVendaHandclub_2.Text = AlterarFonte(Convert.ToDecimal(handclub.VENDA_HANDCLUB_2).ToString("###,###,###,###,##0.00"));

                        Literal _litPHandclub_2 = e.Row.FindControl("litPHandclub_2") as Literal;
                        if (_litPHandclub_2 != null)
                            _litPHandclub_2.Text = AlterarFonte(Convert.ToDecimal(handclub.HANDCLUB_PORC_2).ToString("##0.00") + "%");

                        Literal _litHandclubDisp_2 = e.Row.FindControl("litHandclubDisp_2") as Literal;
                        if (_litHandclubDisp_2 != null)
                            _litHandclubDisp_2.Text = AlterarFonte(Convert.ToDecimal(handclub.HANDCLUB_DISPONIVEL_2).ToString("###,###,###,###,##0.00"));

                        Literal _litValVendaResgate_2 = e.Row.FindControl("litValVendaResgate_2") as Literal;
                        if (_litValVendaResgate_2 != null)
                            _litValVendaResgate_2.Text = AlterarFonte(Convert.ToDecimal(handclub.VALOR_VENDA_RESGATADO_2).ToString("###,###,###,###,##0.00"));

                        Literal _litValResgate_2 = e.Row.FindControl("litValResgate_2") as Literal;
                        if (_litValResgate_2 != null)
                            _litValResgate_2.Text = AlterarFonte(Convert.ToDecimal(handclub.VALOR_RESGATADO_2).ToString("###,###,###,###,##0.00"));

                        Literal _litPFatorCrescimento_2 = e.Row.FindControl("litPFatorCrescimento_2") as Literal;
                        if (_litPFatorCrescimento_2 != null)
                            _litPFatorCrescimento_2.Text = AlterarFonte(Convert.ToDecimal(handclub.FATOR_CRESCIMENTO_2).ToString("##0.00"));

                        Literal _litPResgateHandclub_2 = e.Row.FindControl("litPResgateHandclub_2") as Literal;
                        if (_litPResgateHandclub_2 != null)
                            _litPResgateHandclub_2.Text = AlterarFonte(Convert.ToDecimal(handclub.RESGATE_PORC_2).ToString("##0.00") + "%");


                        /* CORESSSSSSSSS */

                        if (handclub.HANDCLUB_PORC_1 < 33)
                        {
                            e.Row.Cells[2].BackColor = Color.LightPink;
                            e.Row.Cells[2].ForeColor = Color.Maroon;

                        }
                        else if (handclub.HANDCLUB_PORC_1 < 67)
                        {
                            e.Row.Cells[2].BackColor = Color.LightGoldenrodYellow;
                            e.Row.Cells[2].ForeColor = Color.DarkGoldenrod;
                        }
                        else
                        {
                            e.Row.Cells[2].BackColor = Color.PaleGreen;
                            e.Row.Cells[2].ForeColor = Color.Green;
                        }
                        //FATOR DE CRESCIMENTO ANTERIOR [3]
                        if (handclub.FATOR_CRESCIMENTO_1 < 4.5M)
                        {
                            e.Row.Cells[3].BackColor = Color.LightPink;
                            e.Row.Cells[3].ForeColor = Color.Maroon;

                        }
                        else if (handclub.FATOR_CRESCIMENTO_1 < 7.0M)
                        {
                            e.Row.Cells[3].BackColor = Color.LightGoldenrodYellow;
                            e.Row.Cells[3].ForeColor = Color.DarkGoldenrod;
                        }
                        else
                        {
                            e.Row.Cells[3].BackColor = Color.PaleGreen;
                            e.Row.Cells[3].ForeColor = Color.Green;
                        }

                        //% HANDCLUB
                        if (handclub.HANDCLUB_PORC_2 < 33)
                        {
                            e.Row.Cells[8].BackColor = Color.LightPink;
                            e.Row.Cells[8].ForeColor = Color.Maroon;

                        }
                        else if (handclub.HANDCLUB_PORC_2 < 67)
                        {
                            e.Row.Cells[8].BackColor = Color.LightGoldenrodYellow;
                            e.Row.Cells[8].ForeColor = Color.DarkGoldenrod;
                        }
                        else
                        {
                            e.Row.Cells[8].BackColor = Color.PaleGreen;
                            e.Row.Cells[8].ForeColor = Color.Green;
                        }
                        //FATOR DE CRESCIMENTO
                        if (handclub.FATOR_CRESCIMENTO_2 < 4.5M)
                        {
                            e.Row.Cells[12].BackColor = Color.LightPink;
                            e.Row.Cells[12].ForeColor = Color.Maroon;

                        }
                        else if (handclub.FATOR_CRESCIMENTO_2 < 7.0M)
                        {
                            e.Row.Cells[12].BackColor = Color.LightGoldenrodYellow;
                            e.Row.Cells[12].ForeColor = Color.DarkGoldenrod;
                        }
                        else
                        {
                            e.Row.Cells[12].BackColor = Color.PaleGreen;
                            e.Row.Cells[12].ForeColor = Color.Green;
                        }

                    }
                }
            }
        }
        protected void gvFlashHandclub_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_FLASH_HANDCLUBResult> handclub = ObterFlashHandclub("");

            string codigoFilial = ddlFilial.SelectedValue.Trim();
            if (codigoFilial != "")
                handclub = handclub.Where(p => p.CODIGO_FILIAL.Trim() == codigoFilial).ToList();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(gvFlashHandclub, e, out sort);

            if (sort == SortDirection.Ascending)
            {
                Utils.WebControls.GetBoundFieldIndexByName(gvFlashHandclub, e.SortExpression, " - >>");
            }
            else
            {
                Utils.WebControls.GetBoundFieldIndexByName(gvFlashHandclub, e.SortExpression, " - <<");
            }

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            handclub = handclub.OrderBy(e.SortExpression + sortDirection);
            gvFlashHandclub.DataSource = handclub;
            gvFlashHandclub.DataBind();

        }

        private void CarregarFlashHandclubTotal(List<SP_OBTER_FLASH_HANDCLUBResult> handclub)
        {
            List<SP_OBTER_FLASH_HANDCLUBResult> handclubTotal = new List<SP_OBTER_FLASH_HANDCLUBResult>();


            handclubTotal = handclub.GroupBy(x => new { FILIAL = "REDE" }).Select(p => new SP_OBTER_FLASH_HANDCLUBResult
            {
                FILIAL = p.Key.FILIAL,
                // MES RETRASADO
                VENDA_LIQUIDA_0 = p.Sum(g => g.VENDA_LIQUIDA_0),
                VENDA_HANDCLUB_0 = p.Sum(g => g.VENDA_HANDCLUB_0),

                // MES ANTERIOR
                VENDA_LIQUIDA_1 = p.Sum(g => g.VENDA_LIQUIDA_1),
                VENDA_HANDCLUB_1 = p.Sum(g => g.VENDA_HANDCLUB_1),
                HANDCLUB_DISPONIVEL_1 = p.Sum(g => g.HANDCLUB_DISPONIVEL_1),
                VALOR_VENDA_RESGATADO_1 = p.Sum(g => g.VALOR_VENDA_RESGATADO_1),
                VALOR_RESGATADO_1 = p.Sum(g => g.VALOR_RESGATADO_1),
                HANDCLUB_PORC_1 = 0,

                VENDA_LIQUIDA_2 = p.Sum(g => g.VENDA_LIQUIDA_2),
                VENDA_HANDCLUB_2 = p.Sum(g => g.VENDA_HANDCLUB_2),
                HANDCLUB_DISPONIVEL_2 = p.Sum(g => g.HANDCLUB_DISPONIVEL_2),
                VALOR_VENDA_RESGATADO_2 = p.Sum(g => g.VALOR_VENDA_RESGATADO_2),
                VALOR_RESGATADO_2 = p.Sum(g => g.VALOR_RESGATADO_2)


            }).ToList();

            if (handclubTotal != null && handclubTotal.Count() > 0)
            {
                if (handclubTotal[0].VENDA_LIQUIDA_1 > 0)
                    handclubTotal[0].HANDCLUB_PORC_1 = (handclubTotal[0].VENDA_HANDCLUB_1 / Convert.ToDecimal(handclubTotal[0].VENDA_LIQUIDA_1) * 100.00M);

                if (handclubTotal[0].VALOR_RESGATADO_1 > 0)
                    handclubTotal[0].FATOR_CRESCIMENTO_1 = (handclubTotal[0].VALOR_VENDA_RESGATADO_1 / Convert.ToDecimal(handclubTotal[0].VALOR_RESGATADO_1));

                if ((handclubTotal[0].VENDA_HANDCLUB_0) > 0)
                    handclubTotal[0].RESGATE_PORC_1 = (handclubTotal[0].VALOR_RESGATADO_1 / Convert.ToDecimal(handclubTotal[0].VENDA_HANDCLUB_0) * 10 * 100.00M);

                if (handclubTotal[0].VENDA_LIQUIDA_2 > 0)
                    handclubTotal[0].HANDCLUB_PORC_2 = (handclubTotal[0].VENDA_HANDCLUB_2 / Convert.ToDecimal(handclubTotal[0].VENDA_LIQUIDA_2) * 100.00M);

                if (handclubTotal[0].VALOR_RESGATADO_2 > 0)
                    handclubTotal[0].FATOR_CRESCIMENTO_2 = (handclubTotal[0].VALOR_VENDA_RESGATADO_2 / Convert.ToDecimal(handclubTotal[0].VALOR_RESGATADO_2));

                if ((handclubTotal[0].VENDA_HANDCLUB_1) > 0)
                    handclubTotal[0].RESGATE_PORC_2 = (handclubTotal[0].VALOR_RESGATADO_2 / Convert.ToDecimal(handclubTotal[0].VENDA_HANDCLUB_1) * 10 * 100.00M);

            }

            gvFlashHandclubTotal.DataSource = handclubTotal;
            gvFlashHandclubTotal.DataBind();
        }

        #endregion

        private string AlterarFonte(string valor)
        {
            string retorno = "";

            retorno = "<font size='2' face='Calibri' color=''>" + valor + "</font> ";
            return retorno;
        }



    }
}
