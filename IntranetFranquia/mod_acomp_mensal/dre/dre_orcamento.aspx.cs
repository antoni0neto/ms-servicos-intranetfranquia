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
    public partial class dre_orcamento : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DREController dreController = new DREController();

        List<DRE_ORCAMENTO> gOrcamento = new List<DRE_ORCAMENTO>();

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
                CarregarGrupo();
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
                        labTitulo.Text = ddlAno.SelectedValue;
                        ObterOrcamento(Convert.ToInt32(ddlAno.SelectedValue));
                    }
                    catch (Exception) { }
                }
            }
        }
        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObterOrcamento(Convert.ToInt32(ddlAno.SelectedValue));
            CarregarGrupo();
        }

        private void CarregarGrupo()
        {
            var orcamentoGrupo = gOrcamento.GroupBy(p => new { GRUPO = p.GRUPO.Trim().ToUpper() }).Select(j => new DRE_ORCAMENTO { GRUPO = j.Key.GRUPO.Trim().ToUpper() }).OrderBy(o => o.ID).ToList();
            orcamentoGrupo.Insert(0, new DRE_ORCAMENTO { GRUPO = "" });
            ddlGrupo.DataSource = orcamentoGrupo;
            ddlGrupo.DataBind();

            ddlGrupo.Enabled = true;
            if (ddlGrupo.Items.Count <= 1)
            {
                ddlGrupo.SelectedValue = "";
                ddlGrupo.Enabled = false;

                ddlGrupo_SelectedIndexChanged(null, null);
            }
        }
        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlItem.SelectedValue = "";
            ddlItem.Enabled = false;
            ddlFilial.SelectedValue = "";
            ddlFilial.Enabled = false;

            ObterOrcamento(Convert.ToInt32(ddlAno.SelectedValue));
            CarregarSubGrupo(ddlGrupo.SelectedValue);
        }

        private void CarregarSubGrupo(string grupo)
        {
            var orcamentoSubGrupo = gOrcamento.Where(g =>
                                                        g.GRUPO.Trim().ToUpper() == grupo.Trim().ToUpper()).GroupBy(p =>
                                                            new { LINHA = p.LINHA.Trim().ToUpper() }).Select(j =>
                                                                new DRE_ORCAMENTO
                                                                {
                                                                    LINHA = j.Key.LINHA.Trim().ToUpper()
                                                                }).OrderBy(o => o.LINHA).ToList();

            if (orcamentoSubGrupo != null && ((orcamentoSubGrupo.Count() > 0 && orcamentoSubGrupo[0].LINHA != "") || orcamentoSubGrupo.Count() == 0))
                orcamentoSubGrupo.Insert(0, new DRE_ORCAMENTO { LINHA = "" });

            ddlSubGrupo.DataSource = orcamentoSubGrupo;
            ddlSubGrupo.DataBind();

            ddlSubGrupo.Enabled = true;
            if (ddlSubGrupo.Items.Count <= 1)
            {
                ddlSubGrupo.Enabled = false;
                CarregarItem(ddlGrupo.SelectedValue, ddlSubGrupo.SelectedValue);
            }
        }
        protected void ddlSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFilial.SelectedValue = "";
            ddlFilial.Enabled = false;

            ObterOrcamento(Convert.ToInt32(ddlAno.SelectedValue));
            CarregarItem(ddlGrupo.SelectedValue, ddlSubGrupo.SelectedValue);
        }

        private void CarregarItem(string grupo, string subGrupo)
        {
            var orcamentoItem = gOrcamento.Where(g =>
                                                    g.GRUPO.Trim().ToUpper() == grupo.Trim().ToUpper() &&
                                                    g.LINHA.Trim().ToUpper() == subGrupo.Trim().ToUpper()).GroupBy(p =>
                                                            new { TIPO = p.TIPO.Trim().ToUpper() }).Select(j =>
                                                                new DRE_ORCAMENTO { TIPO = j.Key.TIPO.Trim().ToUpper() }).OrderBy(o => o.TIPO).ToList();

            if (orcamentoItem != null && ((orcamentoItem.Count() > 0 && orcamentoItem[0].TIPO != "") || orcamentoItem.Count() == 0))
                orcamentoItem.Insert(0, new DRE_ORCAMENTO { TIPO = "" });

            ddlItem.DataSource = orcamentoItem;
            ddlItem.DataBind();

            ddlItem.Enabled = true;
            if (ddlItem.Items.Count <= 1)
            {
                ddlItem.Enabled = false;
                CarregarFilial(ddlGrupo.SelectedValue, ddlSubGrupo.SelectedValue, ddlItem.SelectedValue);
            }
            else if (ddlItem.Items.Count == 2)
            {
                ddlItem.SelectedIndex = 1;
                CarregarFilial(ddlGrupo.SelectedValue, ddlSubGrupo.SelectedValue, ddlItem.SelectedValue);
            }
        }
        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObterOrcamento(Convert.ToInt32(ddlAno.SelectedValue));
            CarregarFilial(ddlGrupo.SelectedValue, ddlSubGrupo.SelectedValue, ddlItem.SelectedValue);
        }

        private void CarregarFilial(string grupo, string subGrupo, string item)
        {
            var orcamentoFilial = gOrcamento.Where(g =>
                                                                g.GRUPO.Trim().ToUpper() == grupo.Trim().ToUpper() &&
                                                                g.LINHA.Trim().ToUpper() == subGrupo.Trim().ToUpper() &&
                                                                g.TIPO.Trim().ToUpper() == item.Trim().ToUpper()).GroupBy(p =>
                                                                        new { FILIAL = p.FILIAL.Trim().ToUpper() }).Select(j =>
                                                                            new DRE_ORCAMENTO { FILIAL = j.Key.FILIAL.Trim().ToUpper() }).OrderBy(o => o.FILIAL).ToList();

            if (orcamentoFilial != null && ((orcamentoFilial.Count() > 0 && orcamentoFilial[0].FILIAL != "") || orcamentoFilial.Count() == 0))
                orcamentoFilial.Insert(0, new DRE_ORCAMENTO { FILIAL = "" });

            ddlFilial.DataSource = orcamentoFilial;
            ddlFilial.DataBind();

            ddlFilial.Enabled = true;
            if (ddlFilial.Items.Count <= 1)
            {
                ddlFilial.Enabled = false;
            }
            else if (ddlFilial.Items.Count == 2)
            {
                ddlFilial.SelectedIndex = 1;
            }
        }

        private void ObterOrcamento(int ano)
        {
            gOrcamento = dreController.ObterOrcamento(ano);
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

            labGrupo.ForeColor = _OK;
            if (ddlGrupo.SelectedValue.Trim() == "")
            {
                labGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labSubGrupo.ForeColor = _OK;
            if (ddlSubGrupo.SelectedValue.Trim() == "" && ddlSubGrupo.Enabled)
            {
                labSubGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labItem.ForeColor = _OK;
            if (ddlItem.SelectedValue.Trim() == "" && ddlItem.Enabled)
            {
                labItem.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        #region "ORÇAMENTO"
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

                CarregarOrcamento(ddlGrupo.SelectedValue, ddlSubGrupo.SelectedValue, ddlItem.SelectedValue, ddlFilial.SelectedValue);

                string titulo = "";

                titulo = ddlAno.SelectedValue;
                titulo = titulo + " - " + ddlGrupo.SelectedValue;
                titulo = titulo + ((ddlSubGrupo.SelectedValue == "") ? "" : (" - " + ddlSubGrupo.SelectedValue));
                titulo = titulo + ((ddlItem.SelectedValue == "") ? "" : (" - " + ddlItem.SelectedValue));
                titulo = titulo + ((ddlFilial.SelectedValue == "") ? "" : (" - " + ddlFilial.SelectedValue));

                labTitulo.Text = titulo;


            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregarOrcamento(string grupo, string subGrupo, string item, string filial)
        {
            ObterOrcamento(Convert.ToInt32(ddlAno.SelectedValue));

            if (grupo != "")
                gOrcamento = gOrcamento.Where(p => p.GRUPO.ToUpper().Trim() == grupo.Trim().ToUpper()).ToList();

            if (subGrupo != "")
                gOrcamento = gOrcamento.Where(p => p.LINHA.ToUpper().Trim() == subGrupo.Trim().ToUpper()).ToList();

            if (item != "")
                gOrcamento = gOrcamento.Where(p => p.TIPO.ToUpper().Trim() == item.Trim().ToUpper()).ToList();

            if (filial != "")
            {
                if (ddlFilial.SelectedValue.Trim().ToUpper().Contains("ANALIA FRANCO"))
                    filial = "ANALIA FRANCO";
                if (ddlFilial.SelectedValue.Trim().ToUpper().Contains("ARICANDUVA"))
                    filial = "ARICANDUVA";
                if (ddlFilial.SelectedValue.Trim().ToUpper().Contains("BOURBON"))
                    filial = "BOURBON";
                if (ddlFilial.SelectedValue.Trim().ToUpper().Contains("ELDORADO"))
                    filial = "ELDORADO";
                if (ddlFilial.SelectedValue.Trim().ToUpper().Contains("GOIANIA"))
                    filial = "GOIANIA";
                if (ddlFilial.SelectedValue.Trim().ToUpper().Contains("IBIRAPUERA"))
                    filial = "IBIRAPUERA";
                if (ddlFilial.SelectedValue.Trim().ToUpper().Contains("MORUMBI"))
                    filial = "MORUMBI";
                if (ddlFilial.SelectedValue.Trim().ToUpper().Contains("OSASCO"))
                    filial = "OSASCO";
                if (ddlFilial.SelectedValue.Trim().ToUpper().Contains("SANTANA"))
                    filial = "SANTANA";

                gOrcamento = gOrcamento.Where(p => p.FILIAL.ToUpper().Trim().Contains(filial.Trim().ToUpper())).ToList();
            }

            gOrcamento = gOrcamento.OrderBy(p => p.FILIAL).ToList();

            gOrcamento.Add(new DRE_ORCAMENTO
            {
                CODIGO = 0,
                FILIAL = ""
            });
            gvOrcamento.DataSource = gOrcamento;
            gvOrcamento.DataBind();
        }
        protected void gvOrcamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DRE_ORCAMENTO orcamento = e.Row.DataItem as DRE_ORCAMENTO;

                    if (orcamento != null)
                    {
                        if (orcamento.CODIGO == 0)
                        {
                            ((Literal)e.Row.FindControl("litColuna")).Text = "&nbsp;";

                            ((TextBox)e.Row.FindControl("txtJaneiro")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtFevereiro")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtMarco")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtAbril")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtMaio")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtJunho")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtJulho")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtAgosto")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtSetembro")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtOutubro")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtNovembro")).Visible = false;
                            ((TextBox)e.Row.FindControl("txtDezembro")).Visible = false;
                        }
                        if (orcamento.CODIGO > 0)
                        {
                            dJaneiro += orcamento.JANEIRO;
                            dFevereiro += orcamento.FEVEREIRO;
                            dMarco += orcamento.MARCO;
                            dAbril += orcamento.ABRIL;
                            dMaio += orcamento.MAIO;
                            dJunho += orcamento.JUNHO;
                            dJulho += orcamento.JULHO;
                            dAgosto += orcamento.AGOSTO;
                            dSetembro += orcamento.SETEMBRO;
                            dOutubro += orcamento.OUTUBRO;
                            dNovembro += orcamento.NOVEMBRO;
                            dDezembro += orcamento.DEZEMBRO;
                        }
                    }
                }
            }
        }
        protected void gvOrcamento_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvOrcamento.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[2].Text = dJaneiro.ToString("###,###,###,##0.00");
                footer.Cells[3].Text = dFevereiro.ToString("###,###,###,##0.00");
                footer.Cells[4].Text = dMarco.ToString("###,###,###,##0.00");
                footer.Cells[5].Text = dAbril.ToString("###,###,###,##0.00");
                footer.Cells[6].Text = dMaio.ToString("###,###,###,##0.00");
                footer.Cells[7].Text = dJunho.ToString("###,###,###,##0.00");
                footer.Cells[8].Text = dJulho.ToString("###,###,###,##0.00");
                footer.Cells[9].Text = dAgosto.ToString("###,###,###,##0.00");
                footer.Cells[10].Text = dSetembro.ToString("###,###,###,##0.00");
                footer.Cells[11].Text = dOutubro.ToString("###,###,###,##0.00");
                footer.Cells[12].Text = dNovembro.ToString("###,###,###,##0.00");
                footer.Cells[13].Text = dDezembro.ToString("###,###,###,##0.00");
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
                        int codigo = Convert.ToInt32(gvOrcamento.DataKeys[_row.RowIndex].Value.ToString());
                        decimal valor = 0;

                        if (_txt.Text.Trim() != "")
                        {
                            valor = Convert.ToDecimal(_txt.Text.Trim());

                            //Atualizar
                            var orc = dreController.ObterOrcamentoCodigo(codigo);
                            if (orc != null)
                            {
                                if (_txt.ID.ToUpper().Contains("JANEIRO"))
                                    orc.JANEIRO = valor;

                                if (_txt.ID.ToUpper().Contains("FEVEREIRO"))
                                    orc.FEVEREIRO = valor;

                                if (_txt.ID.ToUpper().Contains("MARCO"))
                                    orc.MARCO = valor;

                                if (_txt.ID.ToUpper().Contains("ABRIL"))
                                    orc.ABRIL = valor;

                                if (_txt.ID.ToUpper().Contains("MAIO"))
                                    orc.MAIO = valor;

                                if (_txt.ID.ToUpper().Contains("JUNHO"))
                                    orc.JUNHO = valor;

                                if (_txt.ID.ToUpper().Contains("JULHO"))
                                    orc.JULHO = valor;

                                if (_txt.ID.ToUpper().Contains("AGOSTO"))
                                    orc.AGOSTO = valor;

                                if (_txt.ID.ToUpper().Contains("SETEMBRO"))
                                    orc.SETEMBRO = valor;

                                if (_txt.ID.ToUpper().Contains("OUTUBRO"))
                                    orc.OUTUBRO = valor;

                                if (_txt.ID.ToUpper().Contains("NOVEMBRO"))
                                    orc.NOVEMBRO = valor;

                                if (_txt.ID.ToUpper().Contains("DEZEMBRO"))
                                    orc.DEZEMBRO = valor;

                                dreController.AtualizarOrcamento(orc);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion

    }
}
