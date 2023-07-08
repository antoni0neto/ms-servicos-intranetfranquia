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
    public partial class dre_loja_resultado : System.Web.UI.Page
    {
        DREController dreController = new DREController();
        BaseController baseController = new BaseController();

        Color corTitulo = System.Drawing.SystemColors.GradientActiveCaption;
        Color corFundo = Color.WhiteSmoke;
        string tagCorNegativo = "#CD2626";

        int coluna = 0;

        decimal pJaneiro = 0;
        decimal pFevereiro = 0;
        decimal pMarco = 0;
        decimal pAbril = 0;
        decimal pMaio = 0;
        decimal pJunho = 0;
        decimal pJulho = 0;
        decimal pAgosto = 0;
        decimal pSetembro = 0;
        decimal pOutubro = 0;
        decimal pNovembro = 0;
        decimal pDezembro = 0;
        decimal pTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            string ano = "";
            ano = ddlAno.SelectedValue;

            try
            {
                labErro.Text = "";
                CarregarResultadoLoja(ano);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }

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
        private string FormatarValor(decimal? valor)
        {
            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            retorno = "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") + "</font> ";
            return retorno;
        }
        #endregion

        #region "ACOES"
        private List<FILIAI> ObterFiliais()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliaisAtivaInativa();
            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            return filial;
        }
        private void CarregarResultadoLoja(string ano)
        {
            List<SP_DRE_RESUMOResult> resultadoLoja = new List<SP_DRE_RESUMOResult>();

            //Obter Filiais
            List<FILIAI> _filiais = ObterFiliais();

            foreach (FILIAI f in _filiais)
                resultadoLoja.AddRange(dreController.ObterDRELoja(Convert.ToInt32(ano), f.COD_FILIAL));

            if (resultadoLoja.Count > 0)
            {
                gvResultado.DataSource = resultadoLoja;
                gvResultado.DataBind();
            }
        }
        #endregion

        #region "RESULTADO"
        protected void gvResultado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_RESUMOResult resultado = e.Row.DataItem as SP_DRE_RESUMOResult;

                    coluna += 1;
                    if (resultado != null)
                    {
                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = coluna.ToString();

                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = "<span style='font-weight:bold'><font size='2' face='Calibri'><a href='../dre/dre_dre.aspx?a=" + resultado.ANO + "&f=" + resultado.CODIGO_FILIAL + "' class='adre'>&nbsp;" + resultado.FILIAL + "</a></font></span>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(resultado.JANEIRO).Trim();

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(resultado.FEVEREIRO).Trim();

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(resultado.MARCO).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(resultado.ABRIL).Trim();

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(resultado.MAIO).Trim();

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(resultado.JUNHO).Trim();

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(resultado.JULHO).Trim();

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(resultado.AGOSTO).Trim();

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(resultado.SETEMBRO).Trim();

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(resultado.OUTUBRO).Trim();

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(resultado.NOVEMBRO).Trim();

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(resultado.DEZEMBRO).Trim();

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(resultado.TOTAL).Trim();

                        pJaneiro += Convert.ToDecimal(resultado.JANEIRO);
                        pFevereiro += Convert.ToDecimal(resultado.FEVEREIRO);
                        pMarco += Convert.ToDecimal(resultado.MARCO);
                        pAbril += Convert.ToDecimal(resultado.ABRIL);
                        pMaio += Convert.ToDecimal(resultado.MAIO);
                        pJunho += Convert.ToDecimal(resultado.JUNHO);
                        pJulho += Convert.ToDecimal(resultado.JULHO);
                        pAgosto += Convert.ToDecimal(resultado.AGOSTO);
                        pSetembro += Convert.ToDecimal(resultado.SETEMBRO);
                        pOutubro += Convert.ToDecimal(resultado.OUTUBRO);
                        pNovembro += Convert.ToDecimal(resultado.NOVEMBRO);
                        pDezembro += Convert.ToDecimal(resultado.DEZEMBRO);
                        pTotal += Convert.ToDecimal(resultado.TOTAL);

                    }
                }
            }
        }
        protected void gvResultado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvResultado.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "TOTAL";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[2].Text = FormatarValor(pJaneiro);
                footer.Cells[3].Text = FormatarValor(pFevereiro);
                footer.Cells[4].Text = FormatarValor(pMarco);
                footer.Cells[5].Text = FormatarValor(pAbril);
                footer.Cells[6].Text = FormatarValor(pMaio);
                footer.Cells[7].Text = FormatarValor(pJunho);
                footer.Cells[8].Text = FormatarValor(pJulho);
                footer.Cells[9].Text = FormatarValor(pAgosto);
                footer.Cells[10].Text = FormatarValor(pSetembro);
                footer.Cells[11].Text = FormatarValor(pOutubro);
                footer.Cells[12].Text = FormatarValor(pNovembro);
                footer.Cells[13].Text = FormatarValor(pDezembro);
                footer.Cells[14].Text = FormatarValor(pTotal);
            }
        }
        #endregion

    }
}
