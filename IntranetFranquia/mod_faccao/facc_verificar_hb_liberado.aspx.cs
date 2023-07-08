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


namespace Relatorios.mod_faccao
{
    public partial class facc_verificar_hb_liberado : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "T", DESC_COLECAO = "Todas" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();
            }
        }

        private void CarregarFornecedores()
        {

            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'S') || p.STATUS == 'S').ToList();

                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Todos", STATUS = 'T' });

                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }

        }

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
                if (tela != "1" && tela != "2" && tela != "3")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "facc_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                if (tela == "3")
                    hrefVoltar.HRef = "facc_menu_relatorio.aspx";

                CarregarColecoes();
                CarregarFornecedores();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_DETALHES_HB_FALTANTEResult> ObterHBsLiberados()
        {
            List<SP_OBTER_DETALHES_HB_FALTANTEResult> listaRetorno = new List<SP_OBTER_DETALHES_HB_FALTANTEResult>();

            Int32 Prod_HB = 0;

            if (txtHB.Text.Trim() != "")
            {
                Prod_HB = Convert.ToInt32(txtHB.Text);
            }

            String Fornecedor = ddlFornecedor.SelectedValue;
            if (Fornecedor.Trim() == "Todos")
                Fornecedor = "T";

            var resumoHBF = faccController.ObterHBLiberado(ddlColecoes.SelectedValue.Trim(), Prod_HB, Fornecedor);
            listaRetorno.AddRange(resumoHBF);

            return listaRetorno.OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ThenBy(p => p.MOSTRUARIO).ToList();
        }

        protected void gvLiberados_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_DETALHES_HB_FALTANTEResult> _faccResumo = ObterHBsLiberados();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _faccResumo = _faccResumo.OrderBy(e.SortExpression + sortDirection);
            gvLiberados.DataSource = _faccResumo;
            gvLiberados.DataBind();
        }

        protected void gvLiberados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        Label _labFORNECEDOR = e.Row.FindControl("labFORNECEDOR") as Label;
                        if (_labFORNECEDOR != null)
                        {
                            if (_labFORNECEDOR.Text == "NÃO ENCAIXADO")
                            {
                                _labFORNECEDOR.Text = "<font color='Red'>" + _labFORNECEDOR.Text.Trim() + "</font>";
                                _labFORNECEDOR.Font.Bold = true;
                            }
                        }

                        HiddenField _hidPROD_DETALHE = e.Row.FindControl("hidPROD_DETALHE") as HiddenField;
                        if (_hidPROD_DETALHE != null)
                        {
                            if (Convert.ToInt32(_hidPROD_DETALHE.Value) == 1)
                            {
                                e.Row.BackColor = Color.Azure;
                            }
                            else if (Convert.ToInt32(_hidPROD_DETALHE.Value) == 2)
                            {
                                e.Row.BackColor = Color.Lavender;
                            }
                            else if (Convert.ToInt32(_hidPROD_DETALHE.Value) == 3)
                            {
                                e.Row.BackColor = Color.Gainsboro;
                            }
                            else if (Convert.ToInt32(_hidPROD_DETALHE.Value) == 4)
                            {
                                e.Row.BackColor = Color.Moccasin;
                            }
                            else if (Convert.ToInt32(_hidPROD_DETALHE.Value) == 5)
                            {
                                e.Row.BackColor = Color.PeachPuff;
                            }
                            else if (Convert.ToInt32(_hidPROD_DETALHE.Value) == 6)
                            {
                                e.Row.BackColor = Color.LightCyan;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labMsg.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvLiberados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandArgument.ToString().Trim() == "")
                { return; }

                int resultado;
                if (!int.TryParse(e.CommandArgument.ToString(), out resultado))
                { return; }

                Int32 iIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow dgItem = gvLiberados.Rows[iIndex];

                if (e.CommandName == "Verificar")
                {
                    HiddenField _hidCodigo = (HiddenField)(dgItem.FindControl("hidCodigo"));
                    Label _labVERIFICADO = (Label)(dgItem.FindControl("labVERIFICADO"));
                    Button _btnVerificar = (Button)(dgItem.FindControl("btnVerificar"));

                    PROD_HB_LIBERADO hbLiberado = new PROD_HB_LIBERADO();
                    hbLiberado.CODIGO = Convert.ToInt32(_hidCodigo.Value);
                    hbLiberado.VERIFICADO = 'S';
                    hbLiberado.DATA_VERIFICADO = DateTime.Now;

                    faccController.AtualizarHBLiberado(hbLiberado);

                    PROD_HB_LIBERADO hbLiberadoAlterado = faccController.ObterHBLiberado(Convert.ToInt32(_hidCodigo.Value));

                    if (hbLiberadoAlterado != null)
                    {
                        if (hbLiberadoAlterado.VERIFICADO == 'S')
                        {
                            _btnVerificar.Visible = false;
                            _labVERIFICADO.Visible = true;
                            _labVERIFICADO.Text = "Sim";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labMsg.Text = "ERRO: " + ex.Message;
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                gvLiberados.DataSource = ObterHBsLiberados();
                gvLiberados.DataBind();
            }
            catch (Exception ex)
            {
                labMsg.Text = "ERRO: " + ex.Message;
            }
        }
    }
}