using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.Drawing;
using System.Text;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class cont_resultado_media : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();
        BaseController baseController = new BaseController();

        const string RESULTADO_MEDIA = "RESULTADO_MEDIA";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarSupervisor();
                CarregarDataAno();

                Session[RESULTADO_MEDIA] = null;
            }

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
        protected void ddlSupervisor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFilial(ddlSupervisor.SelectedValue);
        }

        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAnoDe.DataSource = dataAno;
                ddlAnoDe.DataBind();

                ddlAnoAte.DataSource = dataAno;
                ddlAnoAte.DataBind();

                if (ddlAnoDe.Items.Count > 0)
                {
                    try
                    {
                        ddlAnoDe.SelectedValue = DateTime.Now.Year.ToString();
                        ddlAnoAte.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                int anoDe = 0;
                int mesDe = 0;
                int anoAte = 0;
                int mesAte = 0;

                anoDe = Convert.ToInt32(ddlAnoDe.SelectedValue);
                mesDe = Convert.ToInt32(ddlMesDe.SelectedValue);

                anoAte = Convert.ToInt32(ddlAnoAte.SelectedValue);
                mesAte = Convert.ToInt32(ddlMesAte.SelectedValue);

                var x = CarregarResultadoContagem(ddlFilial.SelectedValue, anoDe, mesDe, anoAte, mesAte);
                Session[RESULTADO_MEDIA] = x;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "GRID"
        private List<SP_OBTER_CONT_RESULT_LOJAResult> CarregarResultadoContagem(string codigoFilial, int anoDe, int mesDe, int anoAte, int mesAte)
        {
            DateTime periodoIni;
            DateTime periodoFim;

            periodoIni = Convert.ToDateTime(anoDe + "-" + ((mesDe.ToString().Length == 2) ? mesDe.ToString() : ("0" + mesDe.ToString())) + "-01");
            periodoFim = Convert.ToDateTime(anoAte + "-" + ((mesAte.ToString().Length == 2) ? mesAte.ToString() : ("0" + mesAte.ToString())) + "-" + DateTime.DaysInMonth(anoAte, mesAte).ToString());

            var contResultLoja = contController.ObterResultadoPorLoja(codigoFilial, periodoIni, periodoFim);

            contResultLoja = contResultLoja.GroupBy(p => new { CODIGO_FILIAL = p.CODIGO_FILIAL, FILIAL = p.FILIAL, NOME_SUPERVISOR = p.NOME_SUPERVISOR }).Select(g => new SP_OBTER_CONT_RESULT_LOJAResult()
            {
                CODIGO_FILIAL = g.Key.CODIGO_FILIAL,
                FILIAL = g.Key.FILIAL,
                NOME_SUPERVISOR = g.Key.NOME_SUPERVISOR,
                DIAS_ULTIMA_CONTAGEM = g.Sum(x => x.DIAS_ULTIMA_CONTAGEM),
                DATA_CONTAGEM = g.Max(x => x.DATA_CONTAGEM),
                VALOR_ACEITAVEL_PORC = g.Sum(x => x.VALOR_ACEITAVEL_PORC / g.Count()),
                VALOR_ACEITAVEL = g.Sum(x => x.VALOR_ACEITAVEL),
                RESULTADO_PECAS = g.Sum(x => x.RESULTADO_PECAS),
            }).OrderBy(p => p.NOME_SUPERVISOR).ThenBy(x => x.FILIAL).ToList();

            //FILTRO SUPER
            if (ddlSupervisor.SelectedValue != "0")
                contResultLoja = contResultLoja.Where(p => p.NOME_SUPERVISOR.Trim().ToUpper() == ddlSupervisor.SelectedItem.Text.Trim().ToUpper()).OrderBy(p => p.FILIAL).ToList();

            gvResultadoContagem.DataSource = contResultLoja;
            gvResultadoContagem.DataBind();

            if (contResultLoja == null || contResultLoja.Count() <= 0)
            {
                labErro.Text = "Nenhum resultado de Contagem Encontrado.";
                return null;
            }

            return contResultLoja;
        }
        protected void gvResultadoContagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CONT_RESULT_LOJAResult resultadoCont = e.Row.DataItem as SP_OBTER_CONT_RESULT_LOJAResult;

                    if (resultadoCont != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = resultadoCont.DATA_CONTAGEM.ToString("dd/MM/yyyy");

                        Literal _litDias = e.Row.FindControl("litDias") as Literal;
                        if (_litDias != null)
                            _litDias.Text = resultadoCont.DIAS_ULTIMA_CONTAGEM.ToString() + " dias";

                        Literal _litPorcPerda = e.Row.FindControl("litPorcPerda") as Literal;
                        if (_litPorcPerda != null)
                            _litPorcPerda.Text = (resultadoCont.RESULTADO_PECAS * resultadoCont.VALOR_ACEITAVEL_PORC / 100 / resultadoCont.VALOR_ACEITAVEL * 100).ToString("###,###,###,##0.00");

                        Literal _litPecasDia = e.Row.FindControl("litPecasDia") as Literal;
                        if (_litPecasDia != null)
                            _litPecasDia.Text = (resultadoCont.RESULTADO_PECAS / Convert.ToDecimal(resultadoCont.DIAS_ULTIMA_CONTAGEM) * -1).ToString("###,###,###,##0.00");


                    }
                }
            }

        }
        protected void gvResultadoContagem_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_CONT_RESULT_LOJAResult> resultadoLoja = (IEnumerable<SP_OBTER_CONT_RESULT_LOJAResult>)Session[RESULTADO_MEDIA];

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            resultadoLoja = resultadoLoja.OrderBy(e.SortExpression + sortDirection);
            gvResultadoContagem.DataSource = resultadoLoja;
            gvResultadoContagem.DataBind();
        }
        protected void gvResultadoContagem_DataBound(object sender, EventArgs e)
        {

        }
        #endregion




    }
}
