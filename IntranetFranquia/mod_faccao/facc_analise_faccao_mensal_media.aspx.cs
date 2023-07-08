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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Globalization;


namespace Relatorios
{
    public partial class facc_analise_faccao_mensal_media : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        int qtdeVolumeP = 0;
        int qtdeVolumeM = 0;
        int qtdeVolumeG = 0;

        int qtdeVolumePPORC = 0;
        int qtdeVolumeMPORC = 0;
        int qtdeVolumeGPORC = 0;

        int saldoMercadoria = 0;

        int qtdeTotalHB = 0;
        int qtdeTotalPrazo = 0;
        int qtdeAtraso1 = 0;
        int qtdeAtraso2 = 0;
        int qtdeAtraso3 = 0;
        int qtdeAtraso4 = 0;
        int qtdeAtraso5 = 0;

        int mediaProdutividade = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["f"] == null || Request.QueryString["f"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string fornecedor = Request.QueryString["f"].ToString();
                string colecao = Request.QueryString["c"].ToString();
                string mostruario = Request.QueryString["m"].ToString();

                CarregarFornecedores(fornecedor);
                CarregarColecoes(colecao);

                if (mostruario != "")
                    ddlMostruario.SelectedValue = mostruario;

                btBuscar_Click(null, null);
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlFornecedor.SelectedValue.Trim() == "")
                {
                    labErro.Text = "Selecione o Fornecedor.";
                    return;
                }

                CarregarFaccaoAnalise();
                CarregarFaccaoVolume();
                CarregarFaccaoEntrega();
                CarregarFaccaoEntregaPorc();

                //Tem que vir por ultimo
                CarregarDados();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        private void CarregarDados()
        {
            //QTDE DE FUNCIONARIOS
            var qtdeFun = faccController.ObterFaccaoQtdeFun(ddlFornecedor.SelectedValue);
            if (qtdeFun != null && qtdeFun.QTDE_FUN != "")
            {
                int qtdeFunVal = Convert.ToInt32(qtdeFun.QTDE_FUN);

                //set qtde funcionarios
                txtQtdeFun.Text = qtdeFunVal.ToString();

                //PEÇAS POR FUNCIONARIO
                int qtdePecaPorFun = 0;
                qtdePecaPorFun = (qtdeFunVal <= 0) ? 0 : (mediaProdutividade / qtdeFunVal);
                txtPecaFun.Text = (qtdePecaPorFun <= 0) ? "-" : qtdePecaPorFun.ToString();

                //PEÇAS POR DIA
                txtPecaDia.Text = (qtdePecaPorFun <= 0) ? "-" : (qtdePecaPorFun / 22).ToString();
            }
            else
            {
                txtQtdeFun.Text = "-";
                txtPecaFun.Text = "-";
                txtPecaDia.Text = "-";
            }

            //NOTA DA FACÇÃO
            int qtdeVolumePORC = (qtdeVolumePPORC + qtdeVolumeMPORC + qtdeVolumeGPORC);
            int qtdeVolume = (qtdeVolumeP + qtdeVolumeM + qtdeVolumeG);
            txtNota.Text = (qtdeVolume <= 0) ? "-" : (((qtdeVolumePORC * 1.00) / qtdeVolume) * 10).ToString("0.0");

        }

        #region "ANALISE MENSAL"
        private List<SP_OBTER_FACCAO_ANALISE_MENSALResult> ObterFaccaoAnaliseMensal()
        {
            char? mostruario = null;
            if (ddlMostruario.SelectedValue != "")
                mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            return faccController.ObterFaccaoAnaliseMensal(ddlFornecedor.SelectedValue.Trim(), DateTime.Now, DateTime.Now, ddlColecao.SelectedValue, mostruario);
        }
        private void CarregarFaccaoAnalise()
        {
            var analiseMensal = ObterFaccaoAnaliseMensal();
            CriarGVMensal(analiseMensal);
        }
        private void CriarGVMensal(List<SP_OBTER_FACCAO_ANALISE_MENSALResult> analiseMensal)
        {
            List<string> colunas = new List<string>();
            DataTable dt = new DataTable();

            gvFaccaoAnaliseMensal.Columns.Clear();

            //Obter Colunas
            var mesColuna = analiseMensal.Select(p => p.ANOMES).Distinct().ToList().OrderBy(p => p);
            colunas.Add(" ");
            foreach (var mes in mesColuna)
                colunas.Add(mes);

            //Adiciona Colunas no DataTable
            DataColumn dCol = null;
            foreach (var col in colunas)
            {
                dCol = new DataColumn(RetornarMesExtenso(col), typeof(string));
                dt.Columns.Add(dCol);
            }
            dCol = new DataColumn("Total", typeof(string));
            dt.Columns.Add(dCol);
            dCol = new DataColumn("Média", typeof(string));
            dt.Columns.Add(dCol);

            //TIPO ENVIADO
            var tipoEnviado = analiseMensal.Where(p => p.TIPO == "ENVIADO");
            int contCol = 1;
            int total = 0;

            //auxiliares para media e total
            int media = 0;
            int colMedia = 0;
            int totalFinal = 0;
            bool primeiroTotal = true;

            DataRow drow = dt.NewRow();
            foreach (var col in colunas)
            {
                if (col == " ")
                {
                    drow[0] = "ENVIADO";
                }
                else
                {
                    var resp = tipoEnviado.Where(p => p.ANOMES == col).FirstOrDefault();
                    total = (resp != null && resp.TOTAL != null) ? Convert.ToInt32(resp.TOTAL) : 0;
                    drow[contCol] = total;

                    //CONTROLE DE MEDIA E TOTAL
                    if (total > 0)
                    {

                        //soma so depois da primeira coluna para nao alterar a media
                        if (!primeiroTotal && contCol < (colunas.Count() - 1))
                        {
                            colMedia += 1;
                            media += total;
                        }

                        //soma o ultimo apenas se o mes estiver fechado
                        if (contCol == (colunas.Count() - 1) && !primeiroTotal)
                        {
                            int ano = Convert.ToInt32(col.Substring(0, 4));
                            int anoAtual = DateTime.Now.Year;
                            int mes = Convert.ToInt32(col.Substring(4, 2));
                            int mesAtual = DateTime.Now.Month;

                            if ((ano < anoAtual) || ((ano == anoAtual) && mes < mesAtual))
                            {
                                colMedia += 1;
                                media += total;
                            }
                        }

                        primeiroTotal = false;
                    }

                    contCol += 1;
                    totalFinal += total;
                }
            }

            drow[colunas.Count] = totalFinal;
            drow[colunas.Count + 1] = (colMedia <= 0) ? 0 : media / colMedia;
            dt.Rows.Add(drow);

            //Saldo mercadoria
            saldoMercadoria = totalFinal;

            // TIPO PRODUTIVIDADE
            var tipoProdutividade = analiseMensal.Where(p => p.TIPO == "PRODUTIVIDADE");
            contCol = 1;
            media = 0;
            colMedia = 0;
            totalFinal = 0;
            primeiroTotal = true;
            drow = dt.NewRow();
            foreach (var col in colunas)
            {
                if (col == " ")
                {
                    drow[0] = "PRODUTIVIDADE";
                }
                else
                {
                    var resp = tipoProdutividade.Where(p => p.ANOMES == col).FirstOrDefault();
                    total = (resp != null && resp.TOTAL != null) ? Convert.ToInt32(resp.TOTAL) : 0;
                    drow[contCol] = total;

                    //CONTROLE DE MEDIA E TOTAL
                    if (total > 0)
                    {
                        //soma so depois da primeira coluna para nao alterar a media
                        if (!primeiroTotal && contCol < (colunas.Count() - 1))
                        {
                            colMedia += 1;
                            media += total;
                        }

                        //soma o ultimo apenas se o mes estiver fechado
                        if (contCol == (colunas.Count() - 1) && !primeiroTotal)
                        {
                            int ano = Convert.ToInt32(col.Substring(0, 4));
                            int anoAtual = DateTime.Now.Year;
                            int mes = Convert.ToInt32(col.Substring(4, 2));
                            int mesAtual = DateTime.Now.Month;

                            if ((ano < anoAtual) || ((ano == anoAtual) && mes < mesAtual))
                            {
                                colMedia += 1;
                                media += total;
                            }
                        }

                        primeiroTotal = false;
                    }



                    contCol += 1;
                    totalFinal += total;
                }
            }

            drow[colunas.Count] = totalFinal;
            drow[colunas.Count + 1] = (colMedia <= 0) ? 0 : media / colMedia;
            dt.Rows.Add(drow);

            mediaProdutividade = (colMedia <= 0) ? 0 : media / colMedia;

            //Saldo mercadoria
            saldoMercadoria = saldoMercadoria - totalFinal;

            foreach (DataColumn col in dt.Columns)
            {
                //Declare the bound field and allocate memory for the bound field.
                BoundField bfield = new BoundField();

                //alinhamento
                if (col.ColumnName == " ")
                {
                    bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    bfield.HeaderStyle.Width = Unit.Pixel(200);
                }
                else if (col.ColumnName == "Total" || col.ColumnName == "Média")
                {
                    bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    bfield.HeaderStyle.Width = Unit.Pixel(120);

                    bfield.ItemStyle.BackColor = Color.WhiteSmoke;
                }
                else
                {
                    bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                }

                bfield.HeaderStyle.BackColor = Color.LightGray;
                bfield.FooterStyle.BackColor = Color.LightGray;

                //inicializa valor do campo
                bfield.DataField = col.ColumnName;

                // adiciona nome na coluna
                bfield.HeaderText = col.ColumnName;

                //adiciona campo na coluna
                gvFaccaoAnaliseMensal.Columns.Add(bfield);
            }

            gvFaccaoAnaliseMensal.DataSource = dt;
            gvFaccaoAnaliseMensal.DataBind();

            txtSaldoMercadoria.Text = saldoMercadoria.ToString();
        }
        protected void gvFaccaoAnaliseMensal_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
            }
        }

        private string RetornarMesExtenso(string anomes)
        {
            if (anomes == null || anomes.Trim() == "")
                return " ";

            var mes = anomes.Substring(4, 2);
            var mesExtenso = "";
            if (mes == "01")
                mesExtenso = "Janeiro";
            else if (mes == "02")
                mesExtenso = "Fevereiro";
            else if (mes == "03")
                mesExtenso = "Março";
            else if (mes == "04")
                mesExtenso = "Abril";
            else if (mes == "05")
                mesExtenso = "Maio";
            else if (mes == "06")
                mesExtenso = "Junho";
            else if (mes == "07")
                mesExtenso = "Julho";
            else if (mes == "08")
                mesExtenso = "Agosto";
            else if (mes == "09")
                mesExtenso = "Setembro";
            else if (mes == "10")
                mesExtenso = "Outubro";
            else if (mes == "11")
                mesExtenso = "Novembro";
            else if (mes == "12")
                mesExtenso = "Dezembro";

            return mesExtenso + " " + anomes.Substring(0, 4);
        }
        #endregion

        #region "ANALISE VOLUME HB"
        private List<SP_OBTER_FACCAO_ANALISE_VOLUMEResult> ObterFaccaoAnaliseVolume()
        {
            char? mostruario = null;
            if (ddlMostruario.SelectedValue != "")
                mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            return faccController.ObterFaccaoAnaliseVolume(ddlFornecedor.SelectedValue.Trim(), ddlColecao.SelectedValue, mostruario);
        }
        private void CarregarFaccaoVolume()
        {
            var analiseVolume = ObterFaccaoAnaliseVolume();
            if (analiseVolume != null && analiseVolume.Count() > 0)
            {
                CriarGVVolume(analiseVolume);
            }
            else
            {
                gvFaccaoAnaliseVolume.Columns.Clear();
                gvFaccaoAnaliseVolume.DataSource = new DataTable();
                gvFaccaoAnaliseVolume.DataBind();
            }
        }
        private void CriarGVVolume(List<SP_OBTER_FACCAO_ANALISE_VOLUMEResult> analiseVolume)
        {
            List<string> colunas = new List<string>();
            DataTable dt = new DataTable();

            gvFaccaoAnaliseVolume.Columns.Clear();

            //Obter Colunas
            //var volumeColuna = analiseVolume.Select(p => p.VOLUME_LEG).Distinct();
            colunas.Add(" ");
            colunas.Add("P <200");
            colunas.Add("M 201-600");
            colunas.Add("G >600");

            //Adiciona Colunas no DataTable
            DataColumn dCol = null;
            foreach (var col in colunas)
            {
                dCol = new DataColumn(col, typeof(string));
                dt.Columns.Add(dCol);
            }
            dCol = new DataColumn("Total", typeof(string));
            dt.Columns.Add(dCol);

            DataRow drowTotal = dt.NewRow();
            DataRow drowQtdeHB = dt.NewRow();
            int total = 0;
            int totalFinal = 0;
            int qtdeHB = 0;
            int totalQtdeHB = 0;
            int contCol = 1;
            foreach (var col in colunas)
            {
                if (col == " ")
                {
                    drowTotal[0] = "QUANTIDADE";
                    drowQtdeHB[0] = "HB";
                }
                else
                {
                    var resp = analiseVolume.Where(p => p.VOLUME_LEG == col).FirstOrDefault();
                    total = (resp != null && resp.GRADE_TOTAL != null) ? Convert.ToInt32(resp.GRADE_TOTAL) : 0;
                    qtdeHB = (resp != null && resp.TOTAL_HB != null) ? Convert.ToInt32(resp.TOTAL_HB) : 0;
                    drowTotal[contCol] = total;
                    drowQtdeHB[contCol] = qtdeHB;

                    contCol += 1;
                    totalFinal += total;
                    totalQtdeHB += qtdeHB;
                }
            }

            qtdeVolumeP = Convert.ToInt32(drowTotal[1]);
            qtdeVolumeM = (drowTotal[2] != null && drowTotal[2].ToString() != "") ? Convert.ToInt32(drowTotal[2]) : 0;
            qtdeVolumeG = (drowTotal[3] != null && drowTotal[3].ToString() != "") ? Convert.ToInt32(drowTotal[3]) : 0;

            drowTotal[colunas.Count] = totalFinal;
            drowQtdeHB[colunas.Count] = totalQtdeHB;

            dt.Rows.Add(drowTotal);
            dt.Rows.Add(drowQtdeHB);

            foreach (DataColumn col in dt.Columns)
            {
                //Declare the bound field and allocate memory for the bound field.
                BoundField bfield = new BoundField();

                //alinhamento
                if (col.ColumnName == " ")
                {
                    bfield.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    bfield.HeaderStyle.Width = Unit.Pixel(200);
                }
                else if (col.ColumnName == "Total")
                {
                    bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    bfield.HeaderStyle.Width = Unit.Pixel(120);

                    bfield.ItemStyle.BackColor = Color.WhiteSmoke;
                }
                else
                {
                    bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                }

                bfield.HeaderStyle.BackColor = Color.LightGray;
                bfield.FooterStyle.BackColor = Color.LightGray;

                //inicializa valor do campo
                bfield.DataField = col.ColumnName;

                // adiciona nome na coluna
                bfield.HeaderText = col.ColumnName;

                //adiciona campo na coluna
                gvFaccaoAnaliseVolume.Columns.Add(bfield);
            }

            gvFaccaoAnaliseVolume.DataSource = dt;
            gvFaccaoAnaliseVolume.DataBind();
        }
        protected void gvFaccaoAnaliseVolume_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_ANALISE_VOLUMEResult volume = e.Row.DataItem as SP_OBTER_FACCAO_ANALISE_VOLUMEResult;

                    if (volume != null)
                    {
                    }
                }
            }
        }
        #endregion

        #region"ANALISE ENTREGA"
        private List<SP_OBTER_FACCAO_ANALISE_ENTREGAResult> ObterFaccaoAnaliseEntrega()
        {
            char? mostruario = null;
            if (ddlMostruario.SelectedValue != "")
                mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            return faccController.ObterFaccaoAnaliseEntrega(ddlFornecedor.SelectedValue.Trim(), ddlColecao.SelectedValue, mostruario);
        }
        private void CarregarFaccaoEntrega()
        {
            var analiseEntrega = ObterFaccaoAnaliseEntrega();

            gvFaccaoAnaliseEntrega.DataSource = analiseEntrega;
            gvFaccaoAnaliseEntrega.DataBind();
        }
        protected void gvFaccaoAnaliseEntrega_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_ANALISE_ENTREGAResult entrega = e.Row.DataItem as SP_OBTER_FACCAO_ANALISE_ENTREGAResult;

                    if (entrega != null)
                    {
                        if (entrega.QTDE_PRAZO > 0)
                        {
                            e.Row.Cells[3].BackColor = Color.LightGreen;
                            e.Row.Cells[3].Font.Bold = true;
                        }
                        if (entrega.QTDE_1_SEMANA > 0)
                        {
                            e.Row.Cells[4].BackColor = Color.Gold;
                            e.Row.Cells[4].Font.Bold = true;
                        }
                        if (entrega.QTDE_2_SEMANA > 0)
                        {
                            e.Row.Cells[5].BackColor = Color.Gold;
                            e.Row.Cells[5].Font.Bold = true;
                        }
                        if (entrega.QTDE_3_SEMANA > 0)
                        {
                            e.Row.Cells[6].BackColor = Color.LightCoral;
                            e.Row.Cells[6].Font.Bold = true;
                        }
                        if (entrega.QTDE_4_SEMANA > 0)
                        {
                            e.Row.Cells[7].BackColor = Color.MediumPurple;
                            e.Row.Cells[7].ForeColor = Color.White;
                            e.Row.Cells[7].Font.Bold = true;
                        }
                        if (entrega.QTDE_5_SEMANA > 0)
                        {
                            e.Row.Cells[8].BackColor = Color.MediumPurple;
                            e.Row.Cells[8].ForeColor = Color.White;
                            e.Row.Cells[8].Font.Bold = true;
                        }

                        qtdeTotalHB += entrega.QTDE_HB;

                        qtdeTotalPrazo += entrega.QTDE_PRAZO;
                        qtdeAtraso1 += entrega.QTDE_1_SEMANA;
                        qtdeAtraso2 += entrega.QTDE_2_SEMANA;
                        qtdeAtraso3 += entrega.QTDE_3_SEMANA;
                        qtdeAtraso4 += entrega.QTDE_4_SEMANA;
                        qtdeAtraso5 += entrega.QTDE_5_SEMANA;
                    }
                }
            }
        }
        protected void gvFaccaoAnaliseEntrega_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = qtdeTotalHB.ToString();

                footer.Cells[3].Text = qtdeTotalPrazo.ToString();
                footer.Cells[4].Text = qtdeAtraso1.ToString();
                footer.Cells[5].Text = qtdeAtraso2.ToString();
                footer.Cells[6].Text = qtdeAtraso3.ToString();
                footer.Cells[7].Text = qtdeAtraso4.ToString();
                footer.Cells[8].Text = qtdeAtraso5.ToString();

            }
        }
        #endregion

        #region "% ANALISE ENTREGA"
        private void CarregarFaccaoEntregaPorc()
        {
            var analiseEntrega = ObterFaccaoAnaliseEntrega();

            gvFaccaoAnaliseEntregaPorc.DataSource = analiseEntrega;
            gvFaccaoAnaliseEntregaPorc.DataBind();
        }
        protected void gvFaccaoAnaliseEntregaPorc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_ANALISE_ENTREGAResult entrega = e.Row.DataItem as SP_OBTER_FACCAO_ANALISE_ENTREGAResult;

                    if (entrega != null)
                    {
                        Literal _litQtdePrazo = e.Row.FindControl("litQtdePrazo") as Literal;
                        if (_litQtdePrazo != null)
                        {
                            if (entrega.QTDE_HB > 0)
                            {
                                var qtdePrazoPorc = (((entrega.QTDE_PRAZO * 1.00) / entrega.QTDE_HB) * 100.00);
                                _litQtdePrazo.Text = qtdePrazoPorc.ToString("###,##0") + "%";
                                if (entrega.QTDE_PRAZO > 0)
                                {
                                    e.Row.Cells[3].BackColor = Color.LightGreen;
                                    e.Row.Cells[3].Font.Bold = true;

                                    if (entrega.TAMANHO == "P")
                                        qtdeVolumePPORC = Convert.ToInt32((qtdeVolumeP * qtdePrazoPorc) / 100);
                                    else if (entrega.TAMANHO == "M")
                                        qtdeVolumeMPORC = Convert.ToInt32((qtdeVolumeM * qtdePrazoPorc) / 100);
                                    else if (entrega.TAMANHO == "G")
                                        qtdeVolumeGPORC = Convert.ToInt32((qtdeVolumeG * qtdePrazoPorc) / 100);

                                }
                            }
                            else
                            {
                                _litQtdePrazo.Text = "0%";
                            }
                        }

                        Literal _litQtdeAtraso1 = e.Row.FindControl("litQtdeAtraso1") as Literal;
                        if (_litQtdeAtraso1 != null)
                        {
                            if (entrega.QTDE_HB > 0)
                            {
                                _litQtdeAtraso1.Text = (((entrega.QTDE_1_SEMANA * 1.00) / entrega.QTDE_HB) * 100.00).ToString("###,##0") + "%";
                                if (entrega.QTDE_1_SEMANA > 0)
                                {
                                    e.Row.Cells[4].BackColor = Color.Gold;
                                    e.Row.Cells[4].Font.Bold = true;
                                }
                            }
                            else
                            {
                                _litQtdeAtraso1.Text = "0%";
                            }
                        }
                        Literal _litQtdeAtraso2 = e.Row.FindControl("litQtdeAtraso2") as Literal;
                        if (_litQtdeAtraso2 != null)
                        {
                            if (entrega.QTDE_HB > 0)
                            {
                                _litQtdeAtraso2.Text = (((entrega.QTDE_2_SEMANA * 1.00) / entrega.QTDE_HB) * 100.00).ToString("###,##0") + "%";
                                if (entrega.QTDE_2_SEMANA > 0)
                                {
                                    e.Row.Cells[5].BackColor = Color.Gold;
                                    e.Row.Cells[5].Font.Bold = true;
                                }
                            }
                            else
                            {
                                _litQtdeAtraso2.Text = "0%";
                            }
                        }

                        Literal _litQtdeAtraso3 = e.Row.FindControl("litQtdeAtraso3") as Literal;
                        if (_litQtdeAtraso3 != null)
                        {
                            if (entrega.QTDE_HB > 0)
                            {
                                _litQtdeAtraso3.Text = (((entrega.QTDE_3_SEMANA * 1.00) / entrega.QTDE_HB) * 100.00).ToString("###,##0") + "%";
                                if (entrega.QTDE_3_SEMANA > 0)
                                {
                                    e.Row.Cells[6].BackColor = Color.LightCoral;
                                    e.Row.Cells[6].Font.Bold = true;
                                }
                            }
                            else
                            {
                                _litQtdeAtraso3.Text = "0%";
                            }
                        }

                        Literal _litQtdeAtraso4 = e.Row.FindControl("litQtdeAtraso4") as Literal;
                        if (_litQtdeAtraso4 != null)
                        {
                            if (entrega.QTDE_HB > 0)
                            {
                                _litQtdeAtraso4.Text = (((entrega.QTDE_4_SEMANA * 1.00) / entrega.QTDE_HB) * 100.00).ToString("###,##0") + "%";
                                if (entrega.QTDE_4_SEMANA > 0)
                                {
                                    e.Row.Cells[7].BackColor = Color.MediumPurple;
                                    e.Row.Cells[7].ForeColor = Color.White;
                                    e.Row.Cells[7].Font.Bold = true;
                                }
                            }
                            else
                            {
                                _litQtdeAtraso4.Text = "0%";
                            }
                        }

                        Literal _litQtdeAtraso5 = e.Row.FindControl("litQtdeAtraso5") as Literal;
                        if (_litQtdeAtraso5 != null)
                        {
                            if (entrega.QTDE_HB > 0)
                            {
                                _litQtdeAtraso5.Text = (((entrega.QTDE_5_SEMANA * 1.00) / entrega.QTDE_HB) * 100.00).ToString("###,##0") + "%";
                                if (entrega.QTDE_5_SEMANA > 0)
                                {
                                    e.Row.Cells[8].BackColor = Color.MediumPurple;
                                    e.Row.Cells[8].ForeColor = Color.White;
                                    e.Row.Cells[8].Font.Bold = true;
                                }
                            }
                            else
                            {
                                _litQtdeAtraso5.Text = "0%";
                            }
                        }





                    }
                }
            }
        }
        protected void gvFaccaoAnaliseEntregaPorc_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = qtdeTotalHB.ToString();
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[2].Font.Bold = true;
            }
        }
        #endregion

        #region "DADOS INICIAIS"
        private void CarregarFornecedores(string fornecedor)
        {

            List<PROD_FORNECEDOR> _fornecedores = new ProducaoController().ObterFornecedor().Where(p => p.CODIGO != 94).OrderBy(p => p.FORNECEDOR).ToList();

            if (_fornecedores != null)
            {

                if (fornecedor != "")
                    _fornecedores = _fornecedores.Where(p => p.FORNECEDOR.Trim() == fornecedor.Trim()).ToList();

                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'S') || p.STATUS == 'S');
                ddlFornecedor.DataBind();

                ddlFornecedor.SelectedIndex = 1;
            }

        }
        private void CarregarColecoes(string colecao)
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                if (colecao != "")
                    _colecoes = _colecoes.Where(p => p.COLECAO.Trim() == colecao.Trim()).ToList();

                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();

                ddlColecao.SelectedIndex = 1;
            }
        }
        #endregion



    }
}
