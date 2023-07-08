using DAL;
using Relatorios.mod_ecom.mag;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class ecom_cad_produto : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

        const string extensaoFoto = "jpg";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarSigned();
                CarregarCategoriaMag();

                CarregarJQuery();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
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

                ddlColecaoEcom.DataSource = _colecoes;
                ddlColecaoEcom.DataBind();
            }
        }
        private void CarregarGrupo()
        {
            var _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        private void CarregarSigned()
        {
            List<DESENV_PRODUTO> signedNomeFiltro = new List<DESENV_PRODUTO>();

            var _signednome = desenvController.ObterDesenvolvimentoColecao("").Where(p => p.SIGNED_NOME != null && p.SIGNED_NOME.Trim() != "" && p.STATUS == 'A').Select(s => s.SIGNED_NOME.Trim()).Distinct().ToList();
            foreach (var item in _signednome)
                if (item.Trim() != "")
                    signedNomeFiltro.Add(new DESENV_PRODUTO { SIGNED_NOME = item.Trim() });

            signedNomeFiltro = signedNomeFiltro.OrderBy(p => p.SIGNED_NOME).ToList();
            signedNomeFiltro.Insert(0, new DESENV_PRODUTO { SIGNED_NOME = "" });
            ddlSignedNome.DataSource = signedNomeFiltro;
            ddlSignedNome.DataBind();
        }
        private void CarregarCategoriaMag()
        {
            var catMag = eController.ObterMagentoGrupoProduto().Where(p => p.MOSTRAR_BLOCO == true).OrderBy(p => p.GRUPO).ToList();
            catMag.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "" });

            ddlCategoriaMag.DataSource = catMag;
            ddlCategoriaMag.DataBind();
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

        private List<DIR_PRODUTO_FOTO> ObterProdutoComFoto()
        {
            string pathFotos = Server.MapPath("~/FotosHandbookOnline") + "\\";
            //string pathFotos = @"\\192.168.1.8\d$\Intranet\FotosHandbookOnline\";

            var fotos = new DirectoryInfo(pathFotos).GetFiles(("*." + extensaoFoto));

            string nomeFoto = "";
            string produto = "";
            string cor = "";

            List<DIR_PRODUTO_FOTO> dirProdutoFoto = new List<DIR_PRODUTO_FOTO>();

            foreach (var f in fotos)
            {
                nomeFoto = f.Name.Substring(0, f.Name.Length - 5);

                produto = nomeFoto.Substring(0, 5);
                cor = nomeFoto.Substring(5);

                var produtoCor = dirProdutoFoto.Where(p => p.PRODUTO == produto && p.COR == cor);
                if (produtoCor == null || produtoCor.Count() <= 0)
                    dirProdutoFoto.Add(new DIR_PRODUTO_FOTO { PRODUTO = produto, COR = cor });
            }

            return dirProdutoFoto;
        }

        private List<SP_OBTER_ECOM_PRODUTO_CADResult> ObterProduto()
        {
            //var produtoFotos = ObterProdutoComFoto();

            var produtos = eController.ObterProduto(ddlColecao.SelectedValue, ddlGrupo.SelectedValue, txtProduto.Text.Trim(), ddlGriffe.SelectedValue);

            //filtro Cadastro
            if (ddlStatusProduto.SelectedValue != "")
                produtos = produtos.Where(p => p.STATUS_CADASTRO == Convert.ToChar(ddlStatusProduto.SelectedValue)).ToList();

            //filtro nota faturada
            if (ddlNota.SelectedValue != "")
                produtos = produtos.Where(p => p.TEM_NOTA == ddlNota.SelectedValue).ToList();

            //filtro Estoque
            if (ddlEstoque.SelectedValue != "")
            {
                if (ddlEstoque.SelectedValue == "S")
                    produtos = produtos.Where(p => p.ESTOQUE > 0).ToList();
                else
                    produtos = produtos.Where(p => p.ESTOQUE <= 0).ToList();
            }

            //filtro Colecao Ecom
            if (ddlColecaoEcom.SelectedValue != "")
                produtos = produtos.Where(p => p.COLECAO_ECOM == ddlColecaoEcom.SelectedValue.Trim()).ToList();

            //filtro Foto Direotior
            if (ddlFoto.SelectedValue != "")
                produtos = produtos.Where(p => p.ORDEM_FOTO == Convert.ToInt32(ddlFoto.SelectedValue)).ToList();

            if (ddlPrecoDiff.SelectedValue == "D")
            {
                produtos = produtos.Where(p =>
                                            (p.PRECO > 0 && ((p.PRECO_PROMO != 0 && p.PRECO_TL != p.PRECO_PROMO))

                                            || (p.PRECO_PROMO == 0 && p.PRECO_TL < p.PRECO))).ToList();
            }

            if (ddlFotoCadastrada.SelectedValue != "")
            {
                if (ddlFotoCadastrada.SelectedValue == "S")
                    produtos = produtos.Where(p => p.FOTO_FRENTE_CAB != null).ToList();
                else
                    produtos = produtos.Where(p => p.FOTO_FRENTE_CAB == null).ToList();
            }

            if (ddlSignedNome.SelectedValue != "")
            {
                produtos = produtos.Where(p => p.SIGNED_NOME == ddlSignedNome.SelectedValue).ToList();
            }

            if (ddlFiltroQtde.SelectedValue != "")
            {
                //<asp:ListItem Value="1" Text="Somente Atacado"></asp:ListItem>
                //<asp:ListItem Value="2" Text="Somente Varejo"></asp:ListItem>
                //<asp:ListItem Value="3" Text="Atacado + (Atacado ou Varejo)"></asp:ListItem>
                //<asp:ListItem Value="4" Text="Varejo + (Atacado ou Varejo)"></asp:ListItem>
                //<asp:ListItem Value="5" Text="Atacado E Varejo"></asp:ListItem>

                if (ddlFiltroQtde.SelectedValue == "1")
                    produtos = produtos.Where(p => (p.QTDE_ATACADO > 0 && p.QTDE_VAREJO <= 0) || p.COD_CATEGORIA == "02").ToList();
                else if (ddlFiltroQtde.SelectedValue == "2")
                    produtos = produtos.Where(p => (p.QTDE_VAREJO > 0 && p.QTDE_ATACADO <= 0) || p.COD_CATEGORIA == "02").ToList();
                else if (ddlFiltroQtde.SelectedValue == "3")
                    produtos = produtos.Where(p => (p.QTDE_ATACADO > 0) || p.COD_CATEGORIA == "02").ToList();
                else if (ddlFiltroQtde.SelectedValue == "4")
                    produtos = produtos.Where(p => (p.QTDE_VAREJO > 0) || p.COD_CATEGORIA == "02").ToList();
                else if (ddlFiltroQtde.SelectedValue == "5")
                    produtos = produtos.Where(p => (p.QTDE_VAREJO > 0 && p.QTDE_ATACADO > 0) || p.COD_CATEGORIA == "02").ToList();
            }

            if (ddlB2W.SelectedValue != "")
                produtos = produtos.Where(p => p.CAD_MKTPLACE == ddlB2W.SelectedValue).ToList();

            if (ddlCategoriaMag.SelectedValue != "0")
                produtos = produtos.Where(p => p.ECOM_GRUPO_MOMENTO.Split(',').Contains(ddlCategoriaMag.SelectedValue)).ToList();

            return produtos;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvProduto.DataSource = ObterProduto();
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
                    SP_OBTER_ECOM_PRODUTO_CADResult prod = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_CADResult;

                    System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    _imgProduto.ImageUrl = (prod.FOTO == null) ? "" : prod.FOTO;

                    Literal litCor = e.Row.FindControl("litCor") as Literal;
                    litCor.Text = prod.COR + " - " + prod.DESC_COR_PRODUTO;

                    Literal litDataAlteracao = e.Row.FindControl("litDataAlteracao") as Literal;
                    litDataAlteracao.Text = (prod.DATA_ALTERACAO == null) ? "-" : Convert.ToDateTime(prod.DATA_ALTERACAO).ToString("dd/MM/yyyy HH:mm");

                    ImageButton btAtualizarCAT = e.Row.FindControl("btAtualizarCAT") as ImageButton;
                    if (prod.STATUS_CADASTRO != 'B')
                        btAtualizarCAT.Visible = false;

                    if (prod.TEM_DESC == "N")
                        e.Row.BackColor = Color.PapayaWhip;
                    else
                        e.Row.BackColor = Color.White;

                    if (prod.PRECO_TL <= 0)
                        e.Row.BackColor = Color.LightBlue;
                    else
                        e.Row.BackColor = Color.White;


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
            IEnumerable<SP_OBTER_ECOM_PRODUTO_CADResult> produtos = ObterProduto();

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

        protected void btAbrir_Click(object sender, EventArgs e)
        {
            string url = "";

            try
            {
                labErro.Text = "";

                Button bt = (Button)sender;
                if (bt != null)
                {
                    GridViewRow row = (GridViewRow)bt.NamingContainer;

                    string colecao = gvProduto.DataKeys[row.RowIndex].Values[0].ToString().Trim();
                    string produto = gvProduto.DataKeys[row.RowIndex].Values[1].ToString().Trim();
                    string cor = gvProduto.DataKeys[row.RowIndex].Values[2].ToString().Trim();

                    //Abrir pop-up
                    url = "fnAbrirTelaCadastroMaior('ecom_cad_produto_edit.aspx?co=" + colecao + "&p=" + produto + "&c=" + cor + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                }

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #endregion

        protected void btAtualizarCAT_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = null;
            try
            {
                labErro.Text = "";
                ImageButton bt = (ImageButton)sender;
                row = (GridViewRow)bt.NamingContainer;
                string produto = gvProduto.DataKeys[row.RowIndex].Values[1].ToString().Trim();
                string cor = gvProduto.DataKeys[row.RowIndex].Values[2].ToString().Trim();
                Magento mag = new Magento();
                var ecomProduto = eController.ObterMagentoProdutoConfig(produto, cor);
                mag.AtualizarCategoria(ecomProduto);
                if (row != null) row.BackColor = Color.PaleGreen;
                ecomProduto.DATA_ALTERACAO = DateTime.Now;
                eController.AtualizarMagentoProduto(ecomProduto);

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                if (row != null) row.BackColor = Color.Red;
                labErro.Text = ex.Message;
            }
        }
    }
}

#region "PRODUTO COM FOTO"
class DIR_PRODUTO_FOTO
{
    public string PRODUTO { get; set; }
    public string COR { get; set; }
}
#endregion
