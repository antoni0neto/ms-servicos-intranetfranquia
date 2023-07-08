using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;

namespace Relatorios
{
    public partial class AlteraNotaDefeito : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CodigoNotaRetirada"] != null)
                {
                    int codigoNotaRetirada = Convert.ToInt32(Request.QueryString["CodigoNotaRetirada"].ToString());

                    CarregaGridViewNotaRetirada(codigoNotaRetirada);
                }
            }
        }

        protected void btAlterar_Click(object sender, EventArgs e)
        {
            string notaCmax;
            string notaHbf;
            string notaCalcados;
            string notaOutros;
            string notaLugzi;

            foreach (GridViewRow item in GridViewNotaRetirada.Rows)
            {
                notaCmax = "0";
                notaHbf = "0";
                notaCalcados = "0";
                notaOutros = "0";
                notaLugzi = "0";

                CheckBox cbAlterado = item.FindControl("cbAlterado") as CheckBox;

                if (cbAlterado != null)
                {
                    if (cbAlterado.Checked)
                    {
                        TextBox txtNotaCmax = item.FindControl("txtNotaCmax") as TextBox;

                        if (txtNotaCmax != null)
                        {
                            if (!txtNotaCmax.Text.Equals(""))
                                notaCmax = txtNotaCmax.Text;
                        }

                        TextBox txtNotaHbf = item.FindControl("txtNotaHbf") as TextBox;

                        if (txtNotaHbf != null)
                        {
                            if (!txtNotaHbf.Text.Equals(""))
                                notaHbf = txtNotaHbf.Text;
                        }

                        TextBox txtNotaCalcados = item.FindControl("txtNotaCalcados") as TextBox;

                        if (txtNotaCalcados != null)
                        {
                            if (!txtNotaCalcados.Text.Equals(""))
                                notaCalcados = txtNotaCalcados.Text;
                        }

                        TextBox txtNotaOutros = item.FindControl("txtNotaOutros") as TextBox;

                        if (txtNotaOutros != null)
                        {
                            if (!txtNotaOutros.Text.Equals(""))
                                notaOutros = txtNotaOutros.Text;
                        }

                        TextBox txtNotaLugzi = item.FindControl("txtNotaLugzi") as TextBox;

                        if (txtNotaLugzi != null)
                        {
                            if (!txtNotaLugzi.Text.Equals(""))
                                notaLugzi = txtNotaLugzi.Text;
                        }

                        usuarioController.AtualizaNotaRetirada(Convert.ToInt32(GridViewNotaRetirada.DataKeys[item.RowIndex].Value), notaCmax, notaHbf, notaCalcados, notaOutros, notaLugzi);
                    }
                }
            }

            lblMensagem.Text = "Gravados com sucesso !!!";

            CarregaGridViewNotaRetirada(Convert.ToInt32(Request.QueryString["CodigoNotaRetirada"]));
        }

        protected void GridViewNotaRetirada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA notaRetirada = e.Row.DataItem as NOTA_RETIRADA;

            if (notaRetirada != null)
            {
                TextBox txtNotaCmax = e.Row.FindControl("txtNotaCmax") as TextBox;

                if (txtNotaCmax != null)
                    txtNotaCmax.Text = notaRetirada.NUMERO_NOTA_CMAX.ToString();

                TextBox txtNotaHbf = e.Row.FindControl("txtNotaHbf") as TextBox;

                if (txtNotaHbf != null)
                    txtNotaHbf.Text = notaRetirada.NUMERO_NOTA_HBF.ToString();

                TextBox txtNotaCalcados = e.Row.FindControl("txtNotaCalcados") as TextBox;

                if (txtNotaCalcados != null)
                    txtNotaCalcados.Text = notaRetirada.NUMERO_NOTA_CALCADOS.ToString();

                TextBox txtNotaOutros = e.Row.FindControl("txtNotaOutros") as TextBox;

                if (txtNotaOutros != null)
                    txtNotaOutros.Text = notaRetirada.NUMERO_NOTA_OUTROS.ToString();

                TextBox txtNotaLugzi = e.Row.FindControl("txtNotaLugzi") as TextBox;

                if (txtNotaLugzi != null)
                    txtNotaLugzi.Text = notaRetirada.NUMERO_NOTA_LUGZI.ToString();
            }
        }

        private void CarregaGridViewNotaRetirada(int codigoNotaRetirada)
        {
            List<NOTA_RETIRADA> listaNotaRetirada = new List<NOTA_RETIRADA>();

            NOTA_RETIRADA notaRetirada = usuarioController.BuscaNotaRetirada(codigoNotaRetirada);

            if (notaRetirada != null)
            {
                listaNotaRetirada.Add(notaRetirada);

                GridViewNotaRetirada.DataSource = listaNotaRetirada;
                GridViewNotaRetirada.DataBind();
            }
        }

        private void LimpaFeedBack()
        {
            lblMensagem.Text = string.Empty;
        }
    }
}