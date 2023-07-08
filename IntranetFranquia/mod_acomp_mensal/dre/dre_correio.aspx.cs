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
using System.Text;

namespace Relatorios
{
    public partial class dre_correio : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DREController dreController = new DREController();

        decimal dJaneiro = 0;
        decimal dFevereiro = 0;
        decimal dMarco = 0;
        decimal dAbril = 0;
        decimal dMaio = 0;
        decimal dJunho = 0;
        decimal dJulho = 0;
        decimal dAgosto = 0;
        decimal dSetembro = 0;
        decimal dOutubro = 0;
        decimal dNovembro = 0;
        decimal dDezembro = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();
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

            return retorno;
        }
        #endregion

        #region "CORREIO"
        private List<SP_OBTER_DRE_CORREIOResult> ObterCorreio(int ano)
        {
            DateTime v_dataini = Convert.ToDateTime(ano + "-01-01");
            DateTime v_datafim = Convert.ToDateTime(ano + "-12-31");

            return dreController.ObterDRECorreio(v_dataini, v_datafim);
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

                CarregarCorreio();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregarCorreio()
        {
            gvCorreio.DataSource = ObterCorreio(Convert.ToInt32(ddlAno.SelectedValue));
            gvCorreio.DataBind();
        }
        protected void gvCorreio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DRE_CORREIOResult cor = e.Row.DataItem as SP_OBTER_DRE_CORREIOResult;

                    if (cor != null)
                    {
                        if (cor.CODIGO_FILIAL == "999999")
                        {
                            var btnEditar = ((ImageButton)e.Row.FindControl("btEditar"));
                            if (btnEditar != null)
                                btnEditar.Visible = false;

                            var btnSair = ((ImageButton)e.Row.FindControl("btSair"));
                            if (btnSair != null)
                                btnSair.Visible = false;

                            e.Row.ForeColor = Color.Red;
                            e.Row.Font.Bold = true;
                        }
                        else
                        {
                            dJaneiro += cor.JANEIRO;
                            dFevereiro += cor.FEVEREIRO;
                            dMarco += cor.MARCO;
                            dAbril += cor.ABRIL;
                            dMaio += cor.MAIO;
                            dJunho += cor.JUNHO;
                            dJulho += cor.JULHO;
                            dAgosto += cor.AGOSTO;
                            dSetembro += cor.SETEMBRO;
                            dOutubro += cor.OUTUBRO;
                            dNovembro += cor.NOVEMBRO;
                            dDezembro += cor.DEZEMBRO;
                        }
                    }
                }
            }
        }
        protected void gvCorreio_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCorreio.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[3].Text = dJaneiro.ToString("###,###,###,##0.00");
                footer.Cells[4].Text = dFevereiro.ToString("###,###,###,##0.00");
                footer.Cells[5].Text = dMarco.ToString("###,###,###,##0.00");
                footer.Cells[6].Text = dAbril.ToString("###,###,###,##0.00");
                footer.Cells[7].Text = dMaio.ToString("###,###,###,##0.00");
                footer.Cells[8].Text = dJunho.ToString("###,###,###,##0.00");
                footer.Cells[9].Text = dJulho.ToString("###,###,###,##0.00");
                footer.Cells[10].Text = dAgosto.ToString("###,###,###,##0.00");
                footer.Cells[11].Text = dSetembro.ToString("###,###,###,##0.00");
                footer.Cells[12].Text = dOutubro.ToString("###,###,###,##0.00");
                footer.Cells[13].Text = dNovembro.ToString("###,###,###,##0.00");
                footer.Cells[14].Text = dDezembro.ToString("###,###,###,##0.00");
            }
        }

        protected void txtValor_TextChanged(object sender, EventArgs e)
        {
            TextBox _txt = (TextBox)sender;
            if (_txt != null)
            {
                try
                {
                    GridViewRow _row = (GridViewRow)_txt.NamingContainer;
                    if (_row != null)
                    {

                        string codigoFilial = gvCorreio.DataKeys[_row.RowIndex].Value.ToString();
                        string filial = ((Literal)_row.FindControl("litFilial")).Text.Trim();
                        int ano = Convert.ToInt32(ddlAno.SelectedValue);
                        int mes = 0;

                        decimal valor = 0;

                        if (_txt.Text.Trim() != "")
                        {
                            valor = Convert.ToDecimal(_txt.Text.Trim());

                            if (_txt.ID.ToUpper().Contains("JANEIRO"))
                                mes = 1;

                            if (_txt.ID.ToUpper().Contains("FEVEREIRO"))
                                mes = 2;

                            if (_txt.ID.ToUpper().Contains("MARCO"))
                                mes = 3;

                            if (_txt.ID.ToUpper().Contains("ABRIL"))
                                mes = 4;

                            if (_txt.ID.ToUpper().Contains("MAIO"))
                                mes = 5;

                            if (_txt.ID.ToUpper().Contains("JUNHO"))
                                mes = 6;

                            if (_txt.ID.ToUpper().Contains("JULHO"))
                                mes = 7;

                            if (_txt.ID.ToUpper().Contains("AGOSTO"))
                                mes = 8;

                            if (_txt.ID.ToUpper().Contains("SETEMBRO"))
                                mes = 9;

                            if (_txt.ID.ToUpper().Contains("OUTUBRO"))
                                mes = 10;

                            if (_txt.ID.ToUpper().Contains("NOVEMBRO"))
                                mes = 11;

                            if (_txt.ID.ToUpper().Contains("DEZEMBRO"))
                                mes = 12;

                            var dre = dreController.ObterDRECorreio(codigoFilial, ano, mes);
                            if (dre == null)
                            {
                                DRE_CORREIO corNovo = new DRE_CORREIO();
                                corNovo.CODIGO_FILIAL = codigoFilial;
                                corNovo.FILIAL = filial;
                                corNovo.CENTRO_CUSTO = (codigoFilial == "000000") ? "027" : "029";
                                corNovo.ANO = ano;
                                corNovo.MES = mes;
                                corNovo.VALOR = valor;
                                dreController.InserirDRECorreio(corNovo);
                            }
                            else
                            {
                                dre.VALOR = valor;
                                dreController.AtualizarDRECorreio(dre);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        protected void btEditar_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton _imgBtn = (ImageButton)sender;
            if (_imgBtn != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)_imgBtn.NamingContainer;
                    gvCorreio.EditIndex = row.RowIndex;

                    CarregarCorreio();
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void btSair_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton _imgBtn = (ImageButton)sender;
            if (_imgBtn != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)_imgBtn.NamingContainer;
                    gvCorreio.EditIndex = -1;

                    CarregarCorreio();
                }
                catch (Exception ex)
                {
                }
            }
        }

    }
}
