using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Linq.Expressions;
using System.Linq.Dynamic;


namespace Relatorios
{
    public partial class ecom_cad_produto_tratamento : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();

                CarregarJQuery();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btTransferir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btTransferir, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            List<COLECOE> _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        private void CarregarJQuery()
        {
            //GRID CLIENTE
            if (gvProduto.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion


        private List<SP_OBTER_ECOM_PRODUTO_TRATAMENTOResult> ObterProdutoTratamento()
        {

            int hb = 0;
            char mostruario = 'N';

            //if (ddlMostruario.SelectedValue != "")
            //    mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            //if (txtHB.Text != "")
            //    hb = Convert.ToInt32(txtHB.Text.Trim());

            var produtos = eController.ObterProdutoTratamento(ddlColecao.SelectedValue, ddlGrupo.SelectedValue, hb, txtProduto.Text.Trim(), ddlGriffe.SelectedValue, mostruario);

            if (ddlProducaoCortada.SelectedValue != "")
                produtos = produtos.Where(p => p.CORTE_PRODUCAO == ddlProducaoCortada.SelectedValue).ToList();

            if (ddlFotoHandbookOnline.SelectedValue != "")
                produtos = produtos.Where(p => p.FT_HANDBOOKONLINE == ddlFotoHandbookOnline.SelectedValue).ToList();

            if (ddlFotoTratamento.SelectedValue != "")
                produtos = produtos.Where(p => p.FT_TRATAMENTO == ddlFotoTratamento.SelectedValue).ToList();


            return produtos;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvProduto.DataSource = ObterProdutoTratamento();
                gvProduto.DataBind();

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;

                string m1 = ex.Message ?? "";
                string m2 = (ex.InnerException == null) ? "NULOOO" : ex.InnerException.Message;
                string m3 = ex.StackTrace ?? "";

                labErro.Text = m1 + System.Environment.NewLine + m2 + System.Environment.NewLine + m3;

            }
        }

        #region "GRID PRODUTO"
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_TRATAMENTOResult prod = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_TRATAMENTOResult;

                    Literal litMostruario = e.Row.FindControl("litMostruario") as Literal;
                    litMostruario.Text = (prod.CORTE_MOSTRUARIO == "S") ? "SIM" : "NÃO";

                    Literal litProducao = e.Row.FindControl("litProducao") as Literal;
                    litProducao.Text = (prod.CORTE_PRODUCAO == "S") ? "SIM" : "NÃO";

                    Literal litFTHandbookOnline = e.Row.FindControl("litFTHandbookOnline") as Literal;
                    litFTHandbookOnline.Text = (prod.FT_HANDBOOKONLINE == "S") ? "SIM" : "NÃO";

                    Literal litFTTratamento = e.Row.FindControl("litFTTratamento") as Literal;
                    litFTTratamento.Text = (prod.FT_TRATAMENTO == "S") ? "SIM" : "NÃO";

                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvProduto.FooterRow;
            if (_footer != null)
            {
            }
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_ECOM_PRODUTO_TRATAMENTOResult> produtos = ObterProdutoTratamento();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            produtos = produtos.OrderBy(e.SortExpression + sortDirection);
            gvProduto.DataSource = produtos;
            gvProduto.DataBind();

            CarregarJQuery();
        }

        #endregion

        protected void btTransferir_Click(object sender, EventArgs e)
        {
            try
            {
                // transferir fotos para nova pasta
                labErro.Text = "";

                var diretorioBase = Server.MapPath("~/FotosHandbookTratamento") + "\\";
                var diretorioNovo = DateTime.Now.ToString("ddMMyyyyHHmm");
                var diretorioPara = "";
                var diretorioFoto = "";

                var fotosTratamento = ObterProdutoTratamento();
                foreach (var fo in fotosTratamento)
                {
                    diretorioFoto = fo.GRIFFE + "_" + fo.GRUPO_PRODUTO + "-" + fo.PRODUTO + fo.COR;
                    var diretorioDe = diretorioBase + diretorioFoto;
                    if (Directory.Exists(diretorioDe))
                    {

                        diretorioPara = diretorioBase + diretorioNovo;
                        if (!Directory.Exists(diretorioPara))
                            Directory.CreateDirectory(diretorioPara);

                        diretorioPara = Path.Combine(diretorioPara, diretorioFoto);
                        if (!Directory.Exists(diretorioPara))
                            Directory.CreateDirectory(diretorioPara);

                        string[] arquivoFoto = Directory.GetFiles(diretorioDe);
                        foreach (string arq in arquivoFoto)
                        {
                            string nomeFoto = Path.GetFileName(arq);
                            string dest = Path.Combine(diretorioPara, nomeFoto);
                            File.Copy(arq, dest, true);
                        }

                    }
                }


                labErro.Text = "Fotos transferidas com sucesso.";

                CarregarJQuery();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

    }
}

