using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.Text;
using System.IO;

namespace Relatorios
{
    public partial class gerloja_adm_wapp : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        BaseController baseController = new BaseController();


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_rh/rh_menu.aspx";

                CarregarFuncionarios();
            }
        }

        #region "DADOS INICIAIS"
        #endregion

        private void CarregarFuncionarios()
        {
            var funcionarios = rhController.ObterFuncionario().Where(p => p.WAPP_BAIXA == null);

            gvWapp.DataSource = funcionarios;
            gvWapp.DataBind();
        }

        protected void gvWapp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    RH_FUNCIONARIO func = e.Row.DataItem as RH_FUNCIONARIO;

                    Label labFilial = e.Row.FindControl("labFilial") as Label;
                    labFilial.Text = baseController.BuscaFilialCodigo(Convert.ToInt32(func.CODIGO_FILIAL)).FILIAL;

                    Label labTelefone = e.Row.FindControl("labTelefone") as Label;
                    labTelefone.Text = rhController.ObterFuncionario(func.CODIGO, "", "", "", "").FirstOrDefault().TELEFONE;

                    Label labAdmissao = e.Row.FindControl("labAdmissao") as Label;
                    labAdmissao.Text = (func.DATA_ADMISSAO == null) ? "-" : Convert.ToDateTime(func.DATA_ADMISSAO).ToString("dd/MM/yyyy");

                    Label labDemissao = e.Row.FindControl("labDemissao") as Label;
                    labDemissao.Text = (func.DATA_DEMISSAO == null) ? "-" : Convert.ToDateTime(func.DATA_DEMISSAO).ToString("dd/MM/yyyy");

                    Button btAtualizar = e.Row.FindControl("btAtualizar") as Button;
                    btAtualizar.CommandArgument = func.CODIGO.ToString();
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }

        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                //CODIGO_SUPER, CODIGO_FILIAL, VENDEDOR, CODIGO_TRAB
                Button bt = (Button)sender;

                var codigoFuncionario = bt.CommandArgument;

                var funcionario = rhController.ObterFuncionario(Convert.ToInt32(codigoFuncionario));
                if (funcionario != null)
                {
                    funcionario.WAPP_BAIXA = DateTime.Now;
                    rhController.AtualizarFuncionario(funcionario);
                }

                CarregarFuncionarios();

            }
            catch (Exception)
            {
            }
        }



    }
}
