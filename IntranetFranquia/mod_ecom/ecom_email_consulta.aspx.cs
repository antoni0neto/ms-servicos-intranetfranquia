using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_email_consulta : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";

            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"

        #endregion


        private List<ECOM_EMAIL> ObterEmail()
        {
            var emails = ecomController.ObterEmail();

            if (txtTitulo.Text.Trim() != "")
            {
                emails = emails.Where(p => p.NOME.ToLower().Contains(txtTitulo.Text.Trim().ToLower())).ToList();
            }

            return emails;
        }
        private void CarregarEmail()
        {
            var config = ObterEmail();
            gvEmail.DataSource = config;
            gvEmail.DataBind();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            CarregarEmail();
        }

        protected void gvEmail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_EMAIL email = e.Row.DataItem as ECOM_EMAIL;

                    Literal litTitulo = e.Row.FindControl("litTitulo") as Literal;
                    litTitulo.Text = email.NOME;

                    Literal litDataCriacao = e.Row.FindControl("litDataCriacao") as Literal;
                    litDataCriacao.Text = email.DATA_CRIACAO.ToString("dd/MM/yyyy");

                    Button btAlterar = e.Row.FindControl("btAlterar") as Button;
                    btAlterar.CommandArgument = email.CODIGO.ToString();

                    Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                    btExcluir.CommandArgument = email.CODIGO.ToString();

                }
            }
        }
        protected void gvEmail_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<ECOM_EMAIL> produtos = ObterEmail();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            produtos = produtos.OrderBy(e.SortExpression + sortDirection);
            gvEmail.DataSource = produtos;
            gvEmail.DataBind();
        }
        protected void gvEmail_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvEmail.FooterRow;
            if (_footer != null)
            {
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            try
            {
                labErro.Text = "";

                var codigoEmail = Convert.ToInt32(bt.CommandArgument);
                ecomController.ExcluirEmail(codigoEmail);

                CarregarEmail();

            }
            catch (Exception ex)
            {
                labErro.Text = "Este email não pode ser excluído.";
            }
        }

        protected void btAlterar_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            try
            {
                labErro.Text = "";
                var codigoEmail = Convert.ToInt32(bt.CommandArgument);
                Response.Redirect("ecom_email_criacao.aspx?email=" + codigoEmail.ToString(), "_blank", "");

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btCopiarLayout_Click(object sender, EventArgs e)
        {

        }



    }
}
