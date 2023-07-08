using DAL;
using Relatorios.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class desenv_painel_cortefaccao : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        Dictionary<string, string> dicColecoes = new Dictionary<string, string>();
        int tCol = 0;

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
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";

                CarregarColecoesLinx();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");

        }

        private void CarregarColecoesLinx()
        {
            var colecoes = baseController.BuscaColecoesLinx();

            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
            lstColecao.DataSource = colecoes;
            lstColecao.DataBind();

        }

        private void CarregarPainel(string colecaoLinxCon)
        {
            DataTable dt = new DataTable();
            if (gvCorteFaccao.Columns.Count > 0)
            {
                gvCorteFaccao.Columns.Clear();
                gvCorteFaccao.DataSource = dt;
                gvCorteFaccao.DataBind();
            }

            var faccCorte = desenvController.ObterPainelEntregaFaccCorteLoja(colecaoLinxCon);

            if (faccCorte != null)
            {
                var data = faccCorte.ToArray();
                if (data.Length > 0)
                {

                    BoundField b = new BoundField();

                    var tamColuna = 125;
                    var tamGrid = 0;
                    Color backColor = Color.White;
                    int qtdeCol = 0;

                    foreach (var pair in ((IDictionary<string, object>)data[0]))
                    {
                        b = new BoundField();
                        var nomeColuna = ArrumarNomeColuna(pair.Key);
                        b.DataField = nomeColuna;
                        b.HeaderText = nomeColuna;
                        b.HtmlEncode = false;
                        b.HeaderStyle.Width = Unit.Pixel(tamColuna);
                        b.ItemStyle.Width = Unit.Pixel(tamColuna);
                        b.HeaderStyle.HorizontalAlign = (pair.Key.ToLower() == "mes") ? HorizontalAlign.Left : HorizontalAlign.Center;
                        b.ItemStyle.HorizontalAlign = (pair.Key.ToLower() == "mes") ? HorizontalAlign.Left : HorizontalAlign.Center;
                        gvCorteFaccao.Columns.Add(b);
                        dt.Columns.Add(nomeColuna, (pair.Value ?? string.Empty).GetType());

                        if (pair.Key.ToLower() != "mes")
                        {
                            if (qtdeCol > 2)
                            {
                                if (qtdeCol > 5)
                                {
                                    qtdeCol = 0;
                                    backColor = Color.LightYellow;
                                }
                                else
                                {
                                    backColor = Color.WhiteSmoke;
                                }
                            }
                            else
                            {
                                backColor = Color.LightYellow;
                            }

                            b.ItemStyle.BackColor = backColor;

                            qtdeCol += 1;
                        }

                        tamGrid = tamGrid + tamColuna;
                        tCol += 1;

                    }

                    foreach (var d in data)
                    {
                        dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
                    }

                    if (tamGrid > 1370)
                        gvCorteFaccao.Width = Unit.Pixel(tamGrid);
                    else
                        gvCorteFaccao.Width = Unit.Percentage(100);

                    gvCorteFaccao.DataSource = dt;
                    gvCorteFaccao.DataBind();

                }
            }



        }
        private string ArrumarNomeColuna(string col)
        {
            if (col.ToLower() == "mes")
                return "Mês";

            var vals1 = col.Split('-');
            var ano = vals1[0];

            var descColecao = "";
            if (dicColecoes.ContainsKey(vals1[1]))
            {
                descColecao = dicColecoes[vals1[1]];
            }
            else
            {
                var colecaoMod = baseController.BuscaColecaoAtual(vals1[1]);
                dicColecoes.Add(colecaoMod.COLECAO.Trim(), colecaoMod.DESC_COLECAO.Trim());
                descColecao = colecaoMod.DESC_COLECAO.Trim();
            }

            var campo = vals1[2];

            var nomeColuna = campo.Replace("QTDE_CORTE", "CORTE").Replace("QTDE_FACCAO", "FACÇÃO").Replace("QTDE_ENTREGA", "ENTREGA") + "<br />" + ano + "<br />" + descColecao;

            return nomeColuna;
        }

        protected void gvCorteFaccao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvCorteFaccao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCorteFaccao.FooterRow;
            if (footer != null)
            {
                //Soma a porra toda
                int[] d = new int[100];
                foreach (GridViewRow row in gvCorteFaccao.Rows)
                {
                    for (int i = 1; i < tCol; i++)
                        d[i] += ((row.Cells[i].Text != "&nbsp;" && row.Cells[i].Text != "") ? (Convert.ToInt32(row.Cells[i].Text)) : 0);
                }

                //adiciona linha total
                footer.Cells[0].Text = "Total";

                for (int i = 1; i < tCol; i++)
                {
                    footer.Cells[i].Text = d[i].ToString();
                    footer.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                }

            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (lstColecao.GetSelectedIndices().Count() <= 0)
                {
                    labErro.Text = "Selecione pelo menos uma coleção...";
                    return;
                }

                var colecaoLinxCon = "";
                if (lstColecao.GetSelectedIndices().Count() > 0)
                {
                    foreach (var v in lstColecao.GetSelectedIndices())
                    {
                        var item = lstColecao.Items[v].Value.Trim() + ",";
                        colecaoLinxCon = colecaoLinxCon + item;
                    }

                    colecaoLinxCon = colecaoLinxCon + ",";
                    colecaoLinxCon = colecaoLinxCon.Replace(",,", "");
                }


                CarregarPainel(colecaoLinxCon);

                var diasUteis = desenvController.ObterDiasUteis(0, 0);
                gvDiasUteis.DataSource = diasUteis;
                gvDiasUteis.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void gvDiasUteis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DIASUTEISResult dia = e.Row.DataItem as SP_OBTER_DIASUTEISResult;

                    if (dia != null)
                    {
                        Literal litMes = e.Row.FindControl("litMes") as Literal;
                        litMes.Text = StringUtil.ObterMes(dia.MES);
                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvDiasUteis_DataBound(object sender, EventArgs e)
        {

        }
    }
}
