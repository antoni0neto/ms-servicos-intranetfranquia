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
    public partial class gerloja_flash_varejo : System.Web.UI.Page
    {
        DesempenhoController desempController = new DesempenhoController();
        BaseController baseController = new BaseController();

        const string FASHION_VAREJO = "FASHION_VAREJO";

        const string FASHION_VAREJO_ABERTA = "FASHION_VAREJO_ABERTA";
        const string FASHION_VAREJO_FECHADA = "FASHION_VAREJO_FECHADA";

        //INDICADORES
        int tTotalLinha = 0;
        int tQtdePeca = 0;
        decimal tPA = 0;
        decimal tPrecoMedio = 0;
        decimal tTicketMedio = 0;
        decimal tValorVenda = 0;
        int tAtendimento = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarSupervisor();
                CarregarDia();
                CarregarDataAno();
                CarregarFilial();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarSupervisor()
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];
            if (usuario != null)
            {
                var super = baseController.BuscaUsuarioPerfil(3);
                if (usuario.CODIGO_PERFIL == 3)
                {
                    super = super.Where(p => p.CODIGO_USUARIO == usuario.CODIGO_USUARIO).ToList();
                }

                super.Insert(0, new USUARIO { CODIGO_USUARIO = 0, NOME_USUARIO = "" });
                ddlSupervisor.DataSource = super;
                ddlSupervisor.DataBind();

                if (super != null && super.Count() == 2)
                {
                    ddlSupervisor.SelectedIndex = 1;
                    ddlSupervisor.Enabled = false;
                    ddlSupervisor_SelectedIndexChanged(null, null);
                }
                else
                {
                    CarregarFilial("0");
                }
            }

        }
        private void CarregarDia()
        {
            List<ListItem> itens = new List<ListItem>();
            itens.Add(new ListItem { Value = "", Text = "" });
            for (int i = 1; i <= 31; i++)
            {
                itens.Add(new ListItem { Value = i.ToString(), Text = i.ToString() });
            }

            ddlDiaInicio.DataSource = itens;
            ddlDiaInicio.DataBind();

            ddlDiaFim.DataSource = itens;
            ddlDiaFim.DataBind();

        }
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                ddlAnoComparativo.DataSource = dataAno;
                ddlAnoComparativo.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                        ddlAnoComparativo.SelectedValue = (DateTime.Now.Year - 1).ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        private void CarregarFilial(string codigoSupervisor)
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            if (codigoSupervisor != "0")
            {
                var usu = baseController.BuscaUsuario(Convert.ToInt32(codigoSupervisor));
                filial = baseController.BuscaFiliais(usu).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").OrderBy(p => p.FILIAL).ToList();
            }
            else
            {
                filial = baseController.BuscaFiliais();
            }

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            var usuario = (USUARIO)Session["USUARIO"];
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
                    ddlSupervisor.Enabled = false;
                    ddlTipoFilial.Enabled = false;
                }
            }
        }
        protected void ddlSupervisor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFilial(ddlSupervisor.SelectedValue);
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

            labAnoComparativo.ForeColor = _OK;
            if (ddlAnoComparativo.SelectedValue.Trim() == "")
            {
                labAnoComparativo.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        #region "FLASH VAREJO"

        private List<SP_OBTER_FLASH_DIARIO_SUPERResult> ObterFlashVarejo(int ano, int anoComparativo, int? mes, int? diaIni, int? diaFim, string supervisor, string tipoLoja, string filial)
        {
            var flashVarejo = desempController.ObterFlashVarejo(ano, anoComparativo, mes, diaIni, diaFim, supervisor, tipoLoja, filial);
            return flashVarejo;
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

                int ano = 0;
                int anoComparativo = 0;
                int? mes = null;
                int? diaIni = null;
                int? diaFim = null;
                string supervisor = "";
                string tipoLoja = "";
                string filial = "";

                ano = Convert.ToInt32(ddlAno.SelectedValue);
                anoComparativo = Convert.ToInt32(ddlAnoComparativo.SelectedValue);

                if (ddlMes.SelectedValue != "")
                    mes = Convert.ToInt32(ddlMes.SelectedValue);

                if (ddlDiaInicio.SelectedValue != "")
                    diaIni = Convert.ToInt32(ddlDiaInicio.SelectedValue);

                if (ddlDiaFim.SelectedValue != "")
                    diaFim = Convert.ToInt32(ddlDiaFim.SelectedValue);

                if (ddlSupervisor.SelectedValue != "0")
                    supervisor = ddlSupervisor.SelectedItem.Text.ToUpper().Trim();

                tipoLoja = ddlTipoFilial.SelectedValue;
                filial = ddlFilial.SelectedItem.Text.Trim();

                Session[FASHION_VAREJO] = null;
                Session[FASHION_VAREJO_ABERTA] = null;
                Session[FASHION_VAREJO_FECHADA] = null;

                CarregarFlashVarejo(ano, anoComparativo, mes, diaIni, diaFim, supervisor, tipoLoja, filial);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregarFlashVarejo(int ano, int anoComparativo, int? mes, int? diaIni, int? diaFim, string supervisor, string tipoLoja, string filial)
        {
            var flashVarejoGeral = ObterFlashVarejo(ano, anoComparativo, mes, diaIni, diaFim, supervisor, tipoLoja, "");

            var flashComp = flashVarejoGeral.Where(p => p.QTDE_PECA_IND_PORC != 0 && p.QTDE_PECA_IND_PORC != -100).ToList();
            var flashAberta = flashVarejoGeral.Where(p => p.QTDE_PECA_IND_PORC == 0).ToList();
            var flashFechada = flashVarejoGeral.Where(p => p.QTDE_PECA_IND_PORC == -100).ToList();

            //Carregar total primeiro
            CarregarFlashVarejoTotal(flashComp);

            if (filial != "")
            {
                flashComp = flashComp.Where(p => p.FILIAL.Trim() == filial.Trim()).ToList();
                flashAberta = flashAberta.Where(p => p.FILIAL.Trim() == filial.Trim()).ToList();
                flashFechada = flashFechada.Where(p => p.FILIAL.Trim() == filial.Trim()).ToList();
            }

            gvFlashVarejo.DataSource = flashComp;
            gvFlashVarejo.DataBind();

            gvLojaAberta.DataSource = flashAberta;
            gvLojaAberta.DataBind();

            gvLojaFechada.DataSource = flashFechada;
            gvLojaFechada.DataBind();

            Session[FASHION_VAREJO] = flashComp;
            Session[FASHION_VAREJO_ABERTA] = flashAberta;
            Session[FASHION_VAREJO_FECHADA] = flashFechada;


        }
        protected void gvFlashVarejo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FLASH_DIARIO_SUPERResult flashVarejo = e.Row.DataItem as SP_OBTER_FLASH_DIARIO_SUPERResult;

                    if (flashVarejo != null)
                    {
                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = AlterarFonte(flashVarejo.FILIAL);

                        /* INDICADORES VALORES*/
                        Literal _litQtdePecaInd = e.Row.FindControl("litQtdePecaInd") as Literal;
                        if (_litQtdePecaInd != null)
                            _litQtdePecaInd.Text = AlterarFonte(flashVarejo.QTDE_PECA_IND.ToString());
                        Literal _litPAInd = e.Row.FindControl("litPAInd") as Literal;
                        if (_litPAInd != null)
                            _litPAInd.Text = AlterarFonte(flashVarejo.PA_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litPrecoMedioInd = e.Row.FindControl("litPrecoMedioInd") as Literal;
                        if (_litPrecoMedioInd != null)
                            _litPrecoMedioInd.Text = AlterarFonte("R$ " + flashVarejo.PRECO_MEDIO_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litTicketMedioInd = e.Row.FindControl("litTicketMedioInd") as Literal;
                        if (_litTicketMedioInd != null)
                            _litTicketMedioInd.Text = AlterarFonte("R$ " + flashVarejo.TICKET_MEDIO_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litValorVendaInd = e.Row.FindControl("litValorVendaInd") as Literal;
                        if (_litValorVendaInd != null)
                            _litValorVendaInd.Text = AlterarFonte("R$ " + flashVarejo.VALOR_VENDA_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litQtdeAtendimentoInd = e.Row.FindControl("litQtdeAtendimentoInd") as Literal;
                        if (_litQtdeAtendimentoInd != null)
                            _litQtdeAtendimentoInd.Text = AlterarFonte(flashVarejo.ATENDIMENTO_IND.ToString());

                        /* INDICADORES PENCERTUAL*/
                        Literal _litQtdePecaIndPorc = e.Row.FindControl("litQtdePecaIndPorc") as Literal;
                        if (_litQtdePecaIndPorc != null)
                            _litQtdePecaIndPorc.Text = AlterarFonte(flashVarejo.QTDE_PECA_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litPAIndPorc = e.Row.FindControl("litPAIndPorc") as Literal;
                        if (_litPAIndPorc != null)
                            _litPAIndPorc.Text = AlterarFonte(flashVarejo.PA_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litPrecoMedioIndPorc = e.Row.FindControl("litPrecoMedioIndPorc") as Literal;
                        if (_litPrecoMedioIndPorc != null)
                            _litPrecoMedioIndPorc.Text = AlterarFonte(flashVarejo.PRECO_MEDIO_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litTicketMedioIndPorc = e.Row.FindControl("litTicketMedioIndPorc") as Literal;
                        if (_litTicketMedioIndPorc != null)
                            _litTicketMedioIndPorc.Text = AlterarFonte(flashVarejo.TICKET_MEDIO_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litValorVendaIndPorc = e.Row.FindControl("litValorVendaIndPorc") as Literal;
                        if (_litValorVendaIndPorc != null)
                            _litValorVendaIndPorc.Text = AlterarFonte(flashVarejo.VALOR_VENDA_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litQtdeAtendimentoIndPorc = e.Row.FindControl("litQtdeAtendimentoIndPorc") as Literal;
                        if (_litQtdeAtendimentoIndPorc != null)
                            _litQtdeAtendimentoIndPorc.Text = AlterarFonte(flashVarejo.ATENDIMENTO_IND_PORC.ToString("###,###,###,###,##0.00") + "%");

                        //tTotalLinha += 1;
                        //tQtdePeca += (flashVarejo.QTDE_PECA_2 - flashVarejo.QTDE_PECA_1);
                        //tPA += (flashVarejo.PA_2 - flashVarejo.PA_1);
                        //tPrecoMedio += (flashVarejo.PRECO_MEDIO_2 - flashVarejo.PRECO_MEDIO_1);
                        //tTicketMedio += (flashVarejo.TICKET_MEDIO_2 - flashVarejo.TICKET_MEDIO_1);
                        //tValorVenda += (flashVarejo.VALOR_VENDA_2 - flashVarejo.VALOR_VENDA_1); ;
                        //tAtendimento += (flashVarejo.ATENDIMENTO_2 - flashVarejo.ATENDIMENTO_1);

                        /* CORESSSSSSSSS */
                        // Coluna 2 litQtdePecaInd
                        if (flashVarejo.QTDE_PECA_IND >= 0)
                        {
                            e.Row.Cells[2].BackColor = Color.PaleGreen;
                            e.Row.Cells[2].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[2].BackColor = Color.LightPink;
                            e.Row.Cells[2].ForeColor = Color.Maroon;
                        }

                        // Coluna 3 litPAInd
                        if (flashVarejo.PA_IND >= 0)
                        {
                            e.Row.Cells[3].BackColor = Color.PaleGreen;
                            e.Row.Cells[3].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[3].BackColor = Color.LightPink;
                            e.Row.Cells[3].ForeColor = Color.Maroon;
                        }

                        // Coluna 4 litPrecoMedioInd
                        if (flashVarejo.PRECO_MEDIO_IND >= 0)
                        {
                            e.Row.Cells[4].BackColor = Color.PaleGreen;
                            e.Row.Cells[4].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[4].BackColor = Color.LightPink;
                            e.Row.Cells[4].ForeColor = Color.Maroon;
                        }

                        // Coluna 5 litTicketMedioInd
                        if (flashVarejo.TICKET_MEDIO_IND >= 0)
                        {
                            e.Row.Cells[5].BackColor = Color.PaleGreen;
                            e.Row.Cells[5].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[5].BackColor = Color.LightPink;
                            e.Row.Cells[5].ForeColor = Color.Maroon;
                        }

                        // Coluna 6 litValorVendaInd
                        if (flashVarejo.VALOR_VENDA_IND >= 0)
                        {
                            e.Row.Cells[6].BackColor = Color.PaleGreen;
                            e.Row.Cells[6].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[6].BackColor = Color.LightPink;
                            e.Row.Cells[6].ForeColor = Color.Maroon;
                        }

                        // Coluna 7 litQtdeAtendimentoInd
                        if (flashVarejo.ATENDIMENTO_IND >= 0)
                        {
                            e.Row.Cells[7].BackColor = Color.PaleGreen;
                            e.Row.Cells[7].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[7].BackColor = Color.LightPink;
                            e.Row.Cells[7].ForeColor = Color.Maroon;
                        }


                        // Coluna 9 litQtdePecaIndPorc
                        if (flashVarejo.QTDE_PECA_IND_PORC >= 0)
                        {
                            e.Row.Cells[9].BackColor = Color.PaleGreen;
                            e.Row.Cells[9].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[9].BackColor = Color.LightPink;
                            e.Row.Cells[9].ForeColor = Color.Maroon;
                        }

                        // Coluna 10 litPAIndPorc
                        if (flashVarejo.PA_IND_PORC >= 0)
                        {
                            e.Row.Cells[10].BackColor = Color.PaleGreen;
                            e.Row.Cells[10].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[10].BackColor = Color.LightPink;
                            e.Row.Cells[10].ForeColor = Color.Maroon;
                        }

                        // Coluna 11 litPrecoMedioIndPorc
                        if (flashVarejo.PRECO_MEDIO_IND_PORC >= 0)
                        {
                            e.Row.Cells[11].BackColor = Color.PaleGreen;
                            e.Row.Cells[11].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[11].BackColor = Color.LightPink;
                            e.Row.Cells[11].ForeColor = Color.Maroon;
                        }

                        // Coluna 12 litTicketMedioIndPorc
                        if (flashVarejo.TICKET_MEDIO_IND_PORC >= 0)
                        {
                            e.Row.Cells[12].BackColor = Color.PaleGreen;
                            e.Row.Cells[12].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[12].BackColor = Color.LightPink;
                            e.Row.Cells[12].ForeColor = Color.Maroon;
                        }

                        // Coluna 13 litValorVendaIndPorc
                        if (flashVarejo.VALOR_VENDA_IND_PORC >= 0)
                        {
                            e.Row.Cells[13].BackColor = Color.PaleGreen;
                            e.Row.Cells[13].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[13].BackColor = Color.LightPink;
                            e.Row.Cells[13].ForeColor = Color.Maroon;
                        }

                        // Coluna 14 litQtdeAtendimentoIndPorc
                        if (flashVarejo.ATENDIMENTO_IND_PORC >= 0)
                        {
                            e.Row.Cells[14].BackColor = Color.PaleGreen;
                            e.Row.Cells[14].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[14].BackColor = Color.LightPink;
                            e.Row.Cells[14].ForeColor = Color.Maroon;
                        }


                    }
                }
            }
        }
        protected void gvFlashVarejo_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvFlashVarejo.FooterRow;
            if (footer != null)
            {
                //footer.Cells[1].Text = "Total";
                //footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                //footer.Cells[2].Text = tQtdePeca.ToString();
                //if (tQtdePeca >= 0)
                //    footer.Cells[2].ForeColor = Color.Green;
                //else
                //    footer.Cells[2].ForeColor = Color.Maroon;


                //footer.Cells[3].Text = (tPA / tTotalLinha).ToString();
                //footer.Cells[4].Text = (tPrecoMedio / tTotalLinha).ToString();
                //footer.Cells[5].Text = (tTicketMedio / tTotalLinha).ToString();
                //footer.Cells[6].Text = tValorVenda.ToString();
                //footer.Cells[7].Text = tAtendimento.ToString();


            }
        }
        protected void gvFlashVarejo_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[FASHION_VAREJO] != null)
            {
                IEnumerable<SP_OBTER_FLASH_DIARIO_SUPERResult> flashVarejo = (IEnumerable<SP_OBTER_FLASH_DIARIO_SUPERResult>)Session[FASHION_VAREJO];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvFlashVarejo, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvFlashVarejo, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvFlashVarejo, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                flashVarejo = flashVarejo.OrderBy(e.SortExpression + sortDirection);
                gvFlashVarejo.DataSource = flashVarejo;
                gvFlashVarejo.DataBind();
            }
        }

        private void CarregarFlashVarejoTotal(List<SP_OBTER_FLASH_DIARIO_SUPERResult> flashVarejo)
        {
            //List<SP_OBTER_FLASH_DIARIO_SUPERResult> flashVarejoTotal = new List<SP_OBTER_FLASH_DIARIO_SUPERResult>();


            var flashVarejoTotal = flashVarejo.GroupBy(x => new { FILIAL = "REDE" }).Select(p => new SP_OBTER_FLASH_DIARIO_SUPERResult
            {
                FILIAL = p.Key.FILIAL,
                QTDE_PECA_1 = p.Sum(g => g.QTDE_PECA_1),
                QTDE_PECA_2 = p.Sum(g => g.QTDE_PECA_2),
                QTDE_PECA_IND = p.Sum(g => g.QTDE_PECA_2) - p.Sum(f => f.QTDE_PECA_1),

                PA_1 = p.Average(g => g.PA_1),
                PA_2 = p.Average(g => g.PA_2),
                PA_IND = p.Average(g => g.PA_2) - p.Average(f => f.PA_1),

                PRECO_MEDIO_1 = p.Average(g => g.PRECO_MEDIO_1),
                PRECO_MEDIO_2 = p.Average(g => g.PRECO_MEDIO_2),
                PRECO_MEDIO_IND = p.Average(g => g.PRECO_MEDIO_2) - p.Average(f => f.PRECO_MEDIO_1),

                TICKET_MEDIO_1 = p.Average(g => g.TICKET_MEDIO_1),
                TICKET_MEDIO_2 = p.Average(g => g.TICKET_MEDIO_2),
                TICKET_MEDIO_IND = p.Average(g => g.TICKET_MEDIO_2) - p.Average(f => f.TICKET_MEDIO_1),

                VALOR_VENDA_1 = p.Sum(g => g.VALOR_VENDA_1),
                VALOR_VENDA_2 = p.Sum(g => g.VALOR_VENDA_2),
                VALOR_VENDA_IND = p.Sum(g => g.VALOR_VENDA_2) - p.Sum(f => f.VALOR_VENDA_1),

                ATENDIMENTO_1 = p.Sum(g => g.ATENDIMENTO_1),
                ATENDIMENTO_2 = p.Sum(g => g.ATENDIMENTO_2),
                ATENDIMENTO_IND = p.Sum(g => g.ATENDIMENTO_2) - p.Sum(f => f.ATENDIMENTO_1)
            }).ToList();

            if (flashVarejoTotal != null && flashVarejoTotal.Count() > 0)
            {
                flashVarejoTotal[0].QTDE_PECA_IND_PORC = (flashVarejoTotal[0].QTDE_PECA_IND / Convert.ToDecimal(flashVarejoTotal[0].QTDE_PECA_1) * Convert.ToDecimal(100.00));
                flashVarejoTotal[0].PA_IND_PORC = flashVarejoTotal[0].PA_IND / flashVarejoTotal[0].PA_1 * Convert.ToDecimal(100.00);
                flashVarejoTotal[0].PRECO_MEDIO_IND_PORC = flashVarejoTotal[0].PRECO_MEDIO_IND / flashVarejoTotal[0].PRECO_MEDIO_1 * Convert.ToDecimal(100.00);
                flashVarejoTotal[0].TICKET_MEDIO_IND_PORC = flashVarejoTotal[0].TICKET_MEDIO_IND / flashVarejoTotal[0].TICKET_MEDIO_1 * Convert.ToDecimal(100.00);
                flashVarejoTotal[0].VALOR_VENDA_IND_PORC = flashVarejoTotal[0].VALOR_VENDA_IND / flashVarejoTotal[0].VALOR_VENDA_1 * Convert.ToDecimal(100.00);
                flashVarejoTotal[0].ATENDIMENTO_IND_PORC = flashVarejoTotal[0].ATENDIMENTO_IND / Convert.ToDecimal(flashVarejoTotal[0].ATENDIMENTO_1) * Convert.ToDecimal(100.00);
            }

            gvFlashVarejoTotal.DataSource = flashVarejoTotal;
            gvFlashVarejoTotal.DataBind();
        }

        #endregion

        private string AlterarFonte(string valor)
        {
            string retorno = "";

            retorno = "<font size='2' face='Calibri' color=''>" + valor + "</font> ";
            return retorno;
        }



        #region "LOJAS ABERTAS E FECHADAS"

        protected void gvLojaAberta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FLASH_DIARIO_SUPERResult flashVarejo = e.Row.DataItem as SP_OBTER_FLASH_DIARIO_SUPERResult;

                    if (flashVarejo != null)
                    {
                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = AlterarFonte(flashVarejo.FILIAL);

                        /* INDICADORES VALORES*/
                        Literal _litQtdePecaInd = e.Row.FindControl("litQtdePecaInd") as Literal;
                        if (_litQtdePecaInd != null)
                            _litQtdePecaInd.Text = AlterarFonte(flashVarejo.QTDE_PECA_IND.ToString());
                        Literal _litPAInd = e.Row.FindControl("litPAInd") as Literal;
                        if (_litPAInd != null)
                            _litPAInd.Text = AlterarFonte(flashVarejo.PA_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litPrecoMedioInd = e.Row.FindControl("litPrecoMedioInd") as Literal;
                        if (_litPrecoMedioInd != null)
                            _litPrecoMedioInd.Text = AlterarFonte("R$ " + flashVarejo.PRECO_MEDIO_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litTicketMedioInd = e.Row.FindControl("litTicketMedioInd") as Literal;
                        if (_litTicketMedioInd != null)
                            _litTicketMedioInd.Text = AlterarFonte("R$ " + flashVarejo.TICKET_MEDIO_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litValorVendaInd = e.Row.FindControl("litValorVendaInd") as Literal;
                        if (_litValorVendaInd != null)
                            _litValorVendaInd.Text = AlterarFonte("R$ " + flashVarejo.VALOR_VENDA_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litQtdeAtendimentoInd = e.Row.FindControl("litQtdeAtendimentoInd") as Literal;
                        if (_litQtdeAtendimentoInd != null)
                            _litQtdeAtendimentoInd.Text = AlterarFonte(flashVarejo.ATENDIMENTO_IND.ToString());

                        /* INDICADORES PENCERTUAL*/
                        Literal _litQtdePecaIndPorc = e.Row.FindControl("litQtdePecaIndPorc") as Literal;
                        if (_litQtdePecaIndPorc != null)
                            _litQtdePecaIndPorc.Text = AlterarFonte(flashVarejo.QTDE_PECA_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litPAIndPorc = e.Row.FindControl("litPAIndPorc") as Literal;
                        if (_litPAIndPorc != null)
                            _litPAIndPorc.Text = AlterarFonte(flashVarejo.PA_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litPrecoMedioIndPorc = e.Row.FindControl("litPrecoMedioIndPorc") as Literal;
                        if (_litPrecoMedioIndPorc != null)
                            _litPrecoMedioIndPorc.Text = AlterarFonte(flashVarejo.PRECO_MEDIO_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litTicketMedioIndPorc = e.Row.FindControl("litTicketMedioIndPorc") as Literal;
                        if (_litTicketMedioIndPorc != null)
                            _litTicketMedioIndPorc.Text = AlterarFonte(flashVarejo.TICKET_MEDIO_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litValorVendaIndPorc = e.Row.FindControl("litValorVendaIndPorc") as Literal;
                        if (_litValorVendaIndPorc != null)
                            _litValorVendaIndPorc.Text = AlterarFonte(flashVarejo.VALOR_VENDA_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litQtdeAtendimentoIndPorc = e.Row.FindControl("litQtdeAtendimentoIndPorc") as Literal;
                        if (_litQtdeAtendimentoIndPorc != null)
                            _litQtdeAtendimentoIndPorc.Text = AlterarFonte(flashVarejo.ATENDIMENTO_IND_PORC.ToString("###,###,###,###,##0.00") + "%");

                        //tTotalLinha += 1;
                        //tQtdePeca += (flashVarejo.QTDE_PECA_2 - flashVarejo.QTDE_PECA_1);
                        //tPA += (flashVarejo.PA_2 - flashVarejo.PA_1);
                        //tPrecoMedio += (flashVarejo.PRECO_MEDIO_2 - flashVarejo.PRECO_MEDIO_1);
                        //tTicketMedio += (flashVarejo.TICKET_MEDIO_2 - flashVarejo.TICKET_MEDIO_1);
                        //tValorVenda += (flashVarejo.VALOR_VENDA_2 - flashVarejo.VALOR_VENDA_1); ;
                        //tAtendimento += (flashVarejo.ATENDIMENTO_2 - flashVarejo.ATENDIMENTO_1);

                        /* CORESSSSSSSSS */
                        // Coluna 2 litQtdePecaInd
                        if (flashVarejo.QTDE_PECA_IND >= 0)
                        {
                            e.Row.Cells[2].BackColor = Color.PaleGreen;
                            e.Row.Cells[2].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[2].BackColor = Color.LightPink;
                            e.Row.Cells[2].ForeColor = Color.Maroon;
                        }

                        // Coluna 3 litPAInd
                        if (flashVarejo.PA_IND >= 0)
                        {
                            e.Row.Cells[3].BackColor = Color.PaleGreen;
                            e.Row.Cells[3].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[3].BackColor = Color.LightPink;
                            e.Row.Cells[3].ForeColor = Color.Maroon;
                        }

                        // Coluna 4 litPrecoMedioInd
                        if (flashVarejo.PRECO_MEDIO_IND >= 0)
                        {
                            e.Row.Cells[4].BackColor = Color.PaleGreen;
                            e.Row.Cells[4].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[4].BackColor = Color.LightPink;
                            e.Row.Cells[4].ForeColor = Color.Maroon;
                        }

                        // Coluna 5 litTicketMedioInd
                        if (flashVarejo.TICKET_MEDIO_IND >= 0)
                        {
                            e.Row.Cells[5].BackColor = Color.PaleGreen;
                            e.Row.Cells[5].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[5].BackColor = Color.LightPink;
                            e.Row.Cells[5].ForeColor = Color.Maroon;
                        }

                        // Coluna 6 litValorVendaInd
                        if (flashVarejo.VALOR_VENDA_IND >= 0)
                        {
                            e.Row.Cells[6].BackColor = Color.PaleGreen;
                            e.Row.Cells[6].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[6].BackColor = Color.LightPink;
                            e.Row.Cells[6].ForeColor = Color.Maroon;
                        }

                        // Coluna 7 litQtdeAtendimentoInd
                        if (flashVarejo.ATENDIMENTO_IND >= 0)
                        {
                            e.Row.Cells[7].BackColor = Color.PaleGreen;
                            e.Row.Cells[7].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[7].BackColor = Color.LightPink;
                            e.Row.Cells[7].ForeColor = Color.Maroon;
                        }


                        // Coluna 9 litQtdePecaIndPorc
                        if (flashVarejo.QTDE_PECA_IND_PORC >= 0)
                        {
                            e.Row.Cells[9].BackColor = Color.PaleGreen;
                            e.Row.Cells[9].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[9].BackColor = Color.LightPink;
                            e.Row.Cells[9].ForeColor = Color.Maroon;
                        }

                        // Coluna 10 litPAIndPorc
                        if (flashVarejo.PA_IND_PORC >= 0)
                        {
                            e.Row.Cells[10].BackColor = Color.PaleGreen;
                            e.Row.Cells[10].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[10].BackColor = Color.LightPink;
                            e.Row.Cells[10].ForeColor = Color.Maroon;
                        }

                        // Coluna 11 litPrecoMedioIndPorc
                        if (flashVarejo.PRECO_MEDIO_IND_PORC >= 0)
                        {
                            e.Row.Cells[11].BackColor = Color.PaleGreen;
                            e.Row.Cells[11].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[11].BackColor = Color.LightPink;
                            e.Row.Cells[11].ForeColor = Color.Maroon;
                        }

                        // Coluna 12 litTicketMedioIndPorc
                        if (flashVarejo.TICKET_MEDIO_IND_PORC >= 0)
                        {
                            e.Row.Cells[12].BackColor = Color.PaleGreen;
                            e.Row.Cells[12].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[12].BackColor = Color.LightPink;
                            e.Row.Cells[12].ForeColor = Color.Maroon;
                        }

                        // Coluna 13 litValorVendaIndPorc
                        if (flashVarejo.VALOR_VENDA_IND_PORC >= 0)
                        {
                            e.Row.Cells[13].BackColor = Color.PaleGreen;
                            e.Row.Cells[13].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[13].BackColor = Color.LightPink;
                            e.Row.Cells[13].ForeColor = Color.Maroon;
                        }

                        // Coluna 14 litQtdeAtendimentoIndPorc
                        if (flashVarejo.ATENDIMENTO_IND_PORC >= 0)
                        {
                            e.Row.Cells[14].BackColor = Color.PaleGreen;
                            e.Row.Cells[14].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[14].BackColor = Color.LightPink;
                            e.Row.Cells[14].ForeColor = Color.Maroon;
                        }


                    }
                }
            }
        }
        protected void gvLojaAberta_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvLojaAberta.FooterRow;
            if (footer != null)
            {
                //footer.Cells[1].Text = "Total";
                //footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                //footer.Cells[2].Text = tQtdePeca.ToString();
                //if (tQtdePeca >= 0)
                //    footer.Cells[2].ForeColor = Color.Green;
                //else
                //    footer.Cells[2].ForeColor = Color.Maroon;


                //footer.Cells[3].Text = (tPA / tTotalLinha).ToString();
                //footer.Cells[4].Text = (tPrecoMedio / tTotalLinha).ToString();
                //footer.Cells[5].Text = (tTicketMedio / tTotalLinha).ToString();
                //footer.Cells[6].Text = tValorVenda.ToString();
                //footer.Cells[7].Text = tAtendimento.ToString();


            }
        }
        protected void gvLojaAberta_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[FASHION_VAREJO_ABERTA] != null)
            {
                IEnumerable<SP_OBTER_FLASH_DIARIO_SUPERResult> flashVarejo = (IEnumerable<SP_OBTER_FLASH_DIARIO_SUPERResult>)Session[FASHION_VAREJO_ABERTA];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvLojaAberta, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvLojaAberta, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvLojaAberta, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                flashVarejo = flashVarejo.OrderBy(e.SortExpression + sortDirection);
                gvLojaAberta.DataSource = flashVarejo;
                gvLojaAberta.DataBind();
            }
        }

        protected void gvLojaFechada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FLASH_DIARIO_SUPERResult flashVarejo = e.Row.DataItem as SP_OBTER_FLASH_DIARIO_SUPERResult;

                    if (flashVarejo != null)
                    {
                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = AlterarFonte(flashVarejo.FILIAL);

                        /* INDICADORES VALORES*/
                        Literal _litQtdePecaInd = e.Row.FindControl("litQtdePecaInd") as Literal;
                        if (_litQtdePecaInd != null)
                            _litQtdePecaInd.Text = AlterarFonte(flashVarejo.QTDE_PECA_IND.ToString());
                        Literal _litPAInd = e.Row.FindControl("litPAInd") as Literal;
                        if (_litPAInd != null)
                            _litPAInd.Text = AlterarFonte(flashVarejo.PA_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litPrecoMedioInd = e.Row.FindControl("litPrecoMedioInd") as Literal;
                        if (_litPrecoMedioInd != null)
                            _litPrecoMedioInd.Text = AlterarFonte("R$ " + flashVarejo.PRECO_MEDIO_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litTicketMedioInd = e.Row.FindControl("litTicketMedioInd") as Literal;
                        if (_litTicketMedioInd != null)
                            _litTicketMedioInd.Text = AlterarFonte("R$ " + flashVarejo.TICKET_MEDIO_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litValorVendaInd = e.Row.FindControl("litValorVendaInd") as Literal;
                        if (_litValorVendaInd != null)
                            _litValorVendaInd.Text = AlterarFonte("R$ " + flashVarejo.VALOR_VENDA_IND.ToString("###,###,###,###,##0.00"));
                        Literal _litQtdeAtendimentoInd = e.Row.FindControl("litQtdeAtendimentoInd") as Literal;
                        if (_litQtdeAtendimentoInd != null)
                            _litQtdeAtendimentoInd.Text = AlterarFonte(flashVarejo.ATENDIMENTO_IND.ToString());

                        /* INDICADORES PENCERTUAL*/
                        Literal _litQtdePecaIndPorc = e.Row.FindControl("litQtdePecaIndPorc") as Literal;
                        if (_litQtdePecaIndPorc != null)
                            _litQtdePecaIndPorc.Text = AlterarFonte(flashVarejo.QTDE_PECA_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litPAIndPorc = e.Row.FindControl("litPAIndPorc") as Literal;
                        if (_litPAIndPorc != null)
                            _litPAIndPorc.Text = AlterarFonte(flashVarejo.PA_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litPrecoMedioIndPorc = e.Row.FindControl("litPrecoMedioIndPorc") as Literal;
                        if (_litPrecoMedioIndPorc != null)
                            _litPrecoMedioIndPorc.Text = AlterarFonte(flashVarejo.PRECO_MEDIO_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litTicketMedioIndPorc = e.Row.FindControl("litTicketMedioIndPorc") as Literal;
                        if (_litTicketMedioIndPorc != null)
                            _litTicketMedioIndPorc.Text = AlterarFonte(flashVarejo.TICKET_MEDIO_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litValorVendaIndPorc = e.Row.FindControl("litValorVendaIndPorc") as Literal;
                        if (_litValorVendaIndPorc != null)
                            _litValorVendaIndPorc.Text = AlterarFonte(flashVarejo.VALOR_VENDA_IND_PORC.ToString("###,###,###,###,##0.00") + "%");
                        Literal _litQtdeAtendimentoIndPorc = e.Row.FindControl("litQtdeAtendimentoIndPorc") as Literal;
                        if (_litQtdeAtendimentoIndPorc != null)
                            _litQtdeAtendimentoIndPorc.Text = AlterarFonte(flashVarejo.ATENDIMENTO_IND_PORC.ToString("###,###,###,###,##0.00") + "%");

                        //tTotalLinha += 1;
                        //tQtdePeca += (flashVarejo.QTDE_PECA_2 - flashVarejo.QTDE_PECA_1);
                        //tPA += (flashVarejo.PA_2 - flashVarejo.PA_1);
                        //tPrecoMedio += (flashVarejo.PRECO_MEDIO_2 - flashVarejo.PRECO_MEDIO_1);
                        //tTicketMedio += (flashVarejo.TICKET_MEDIO_2 - flashVarejo.TICKET_MEDIO_1);
                        //tValorVenda += (flashVarejo.VALOR_VENDA_2 - flashVarejo.VALOR_VENDA_1); ;
                        //tAtendimento += (flashVarejo.ATENDIMENTO_2 - flashVarejo.ATENDIMENTO_1);

                        /* CORESSSSSSSSS */
                        // Coluna 2 litQtdePecaInd
                        if (flashVarejo.QTDE_PECA_IND >= 0)
                        {
                            e.Row.Cells[2].BackColor = Color.PaleGreen;
                            e.Row.Cells[2].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[2].BackColor = Color.LightPink;
                            e.Row.Cells[2].ForeColor = Color.Maroon;
                        }

                        // Coluna 3 litPAInd
                        if (flashVarejo.PA_IND >= 0)
                        {
                            e.Row.Cells[3].BackColor = Color.PaleGreen;
                            e.Row.Cells[3].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[3].BackColor = Color.LightPink;
                            e.Row.Cells[3].ForeColor = Color.Maroon;
                        }

                        // Coluna 4 litPrecoMedioInd
                        if (flashVarejo.PRECO_MEDIO_IND >= 0)
                        {
                            e.Row.Cells[4].BackColor = Color.PaleGreen;
                            e.Row.Cells[4].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[4].BackColor = Color.LightPink;
                            e.Row.Cells[4].ForeColor = Color.Maroon;
                        }

                        // Coluna 5 litTicketMedioInd
                        if (flashVarejo.TICKET_MEDIO_IND >= 0)
                        {
                            e.Row.Cells[5].BackColor = Color.PaleGreen;
                            e.Row.Cells[5].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[5].BackColor = Color.LightPink;
                            e.Row.Cells[5].ForeColor = Color.Maroon;
                        }

                        // Coluna 6 litValorVendaInd
                        if (flashVarejo.VALOR_VENDA_IND >= 0)
                        {
                            e.Row.Cells[6].BackColor = Color.PaleGreen;
                            e.Row.Cells[6].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[6].BackColor = Color.LightPink;
                            e.Row.Cells[6].ForeColor = Color.Maroon;
                        }

                        // Coluna 7 litQtdeAtendimentoInd
                        if (flashVarejo.ATENDIMENTO_IND >= 0)
                        {
                            e.Row.Cells[7].BackColor = Color.PaleGreen;
                            e.Row.Cells[7].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[7].BackColor = Color.LightPink;
                            e.Row.Cells[7].ForeColor = Color.Maroon;
                        }


                        // Coluna 9 litQtdePecaIndPorc
                        if (flashVarejo.QTDE_PECA_IND_PORC >= 0)
                        {
                            e.Row.Cells[9].BackColor = Color.PaleGreen;
                            e.Row.Cells[9].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[9].BackColor = Color.LightPink;
                            e.Row.Cells[9].ForeColor = Color.Maroon;
                        }

                        // Coluna 10 litPAIndPorc
                        if (flashVarejo.PA_IND_PORC >= 0)
                        {
                            e.Row.Cells[10].BackColor = Color.PaleGreen;
                            e.Row.Cells[10].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[10].BackColor = Color.LightPink;
                            e.Row.Cells[10].ForeColor = Color.Maroon;
                        }

                        // Coluna 11 litPrecoMedioIndPorc
                        if (flashVarejo.PRECO_MEDIO_IND_PORC >= 0)
                        {
                            e.Row.Cells[11].BackColor = Color.PaleGreen;
                            e.Row.Cells[11].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[11].BackColor = Color.LightPink;
                            e.Row.Cells[11].ForeColor = Color.Maroon;
                        }

                        // Coluna 12 litTicketMedioIndPorc
                        if (flashVarejo.TICKET_MEDIO_IND_PORC >= 0)
                        {
                            e.Row.Cells[12].BackColor = Color.PaleGreen;
                            e.Row.Cells[12].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[12].BackColor = Color.LightPink;
                            e.Row.Cells[12].ForeColor = Color.Maroon;
                        }

                        // Coluna 13 litValorVendaIndPorc
                        if (flashVarejo.VALOR_VENDA_IND_PORC >= 0)
                        {
                            e.Row.Cells[13].BackColor = Color.PaleGreen;
                            e.Row.Cells[13].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[13].BackColor = Color.LightPink;
                            e.Row.Cells[13].ForeColor = Color.Maroon;
                        }

                        // Coluna 14 litQtdeAtendimentoIndPorc
                        if (flashVarejo.ATENDIMENTO_IND_PORC >= 0)
                        {
                            e.Row.Cells[14].BackColor = Color.PaleGreen;
                            e.Row.Cells[14].ForeColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[14].BackColor = Color.LightPink;
                            e.Row.Cells[14].ForeColor = Color.Maroon;
                        }


                    }
                }
            }
        }
        protected void gvLojaFechada_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvLojaFechada.FooterRow;
            if (footer != null)
            {
                //footer.Cells[1].Text = "Total";
                //footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                //footer.Cells[2].Text = tQtdePeca.ToString();
                //if (tQtdePeca >= 0)
                //    footer.Cells[2].ForeColor = Color.Green;
                //else
                //    footer.Cells[2].ForeColor = Color.Maroon;


                //footer.Cells[3].Text = (tPA / tTotalLinha).ToString();
                //footer.Cells[4].Text = (tPrecoMedio / tTotalLinha).ToString();
                //footer.Cells[5].Text = (tTicketMedio / tTotalLinha).ToString();
                //footer.Cells[6].Text = tValorVenda.ToString();
                //footer.Cells[7].Text = tAtendimento.ToString();


            }
        }
        protected void gvLojaFechada_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[FASHION_VAREJO_FECHADA] != null)
            {
                IEnumerable<SP_OBTER_FLASH_DIARIO_SUPERResult> flashVarejo = (IEnumerable<SP_OBTER_FLASH_DIARIO_SUPERResult>)Session[FASHION_VAREJO_FECHADA];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvLojaFechada, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvLojaFechada, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvLojaFechada, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                flashVarejo = flashVarejo.OrderBy(e.SortExpression + sortDirection);
                gvLojaFechada.DataSource = flashVarejo;
                gvLojaFechada.DataBind();
            }
        }

        #endregion






    }
}
