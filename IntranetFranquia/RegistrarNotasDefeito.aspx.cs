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
    public partial class RegistrarNotasDefeito : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CodigoNotaRetirada"] != null)
                {
                    DateTime dataBanco = baseController.BuscaDataBanco().data;

                    NOTA_RETIRADA notaRetirada = baseController.BuscaNotaRetirada(Convert.ToInt32(Request.QueryString["CodigoNotaRetirada"].ToString()));

                    if (notaRetirada != null)
                    {
                        txtNotaCmax.Text = notaRetirada.NUMERO_NOTA_CMAX;

                        if (!txtNotaCmax.Text.Equals(""))
                            txtNotaCmax.Enabled = false;

                        txtNotaHbf.Text = notaRetirada.NUMERO_NOTA_HBF;

                        if (!txtNotaHbf.Text.Equals(""))
                            txtNotaHbf.Enabled = false;

                        txtNotaHbfCalcados.Text = notaRetirada.NUMERO_NOTA_CALCADOS;

                        if (!txtNotaHbfCalcados.Text.Equals(""))
                            txtNotaHbfCalcados.Enabled = false;

                        txtNotaHbfOutros.Text = notaRetirada.NUMERO_NOTA_OUTROS;

                        if (!txtNotaHbfOutros.Text.Equals(""))
                            txtNotaHbfOutros.Enabled = false;
                        
                        txtNotaLugzi.Text = notaRetirada.NUMERO_NOTA_LUGZI;

                        if (!txtNotaLugzi.Text.Equals("") & !txtNotaLugzi.Text.Equals("0"))
                            txtNotaLugzi.Enabled = false;
                    }
                }
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            if (txtNotaCmax.Text.Equals("") & 
                txtNotaHbf.Text.Equals("") & 
                txtNotaHbfCalcados.Text.Equals("") & 
                txtNotaHbfOutros.Text.Equals("") &
                txtNotaLugzi.Text.Equals(""))
                return;

            if (Request.QueryString["CodigoNotaRetirada"] != null)
            {
                DateTime dataBanco = baseController.BuscaDataBanco().data;

                NOTA_RETIRADA notaRetirada = usuarioController.BuscaNotaRetirada(Convert.ToInt32(Request.QueryString["CodigoNotaRetirada"]));

                if (notaRetirada != null)
                {
                    if (txtNotaCmax.Enabled)
                    {
                        if (!txtNotaCmax.Equals(""))
                        {
                            notaRetirada.NUMERO_NOTA_CMAX = txtNotaCmax.Text;
                            notaRetirada.DATA_NOTA_CMAX = dataBanco;
                        }
                    }

                    if (txtNotaHbf.Enabled)
                    {
                        if (!txtNotaHbf.Equals(""))
                        {
                            notaRetirada.NUMERO_NOTA_HBF = txtNotaHbf.Text;
                            notaRetirada.DATA_NOTA_HBF = dataBanco;
                        }
                    }

                    if (txtNotaHbfCalcados.Enabled)
                    {
                        if (!txtNotaHbfCalcados.Equals(""))
                        {
                            notaRetirada.NUMERO_NOTA_CALCADOS = txtNotaHbfCalcados.Text;
                            notaRetirada.DATA_NOTA_CALCADOS = dataBanco;
                        }
                    }

                    if (txtNotaHbfOutros.Enabled)
                    {
                        if (!txtNotaHbfOutros.Equals(""))
                        {
                            notaRetirada.NUMERO_NOTA_OUTROS = txtNotaHbfOutros.Text;
                            notaRetirada.DATA_NOTA_OUTROS = dataBanco;
                        }
                    }

                    if (txtNotaLugzi.Enabled)
                    {
                        if (!txtNotaLugzi.Equals(""))
                        {
                            notaRetirada.NUMERO_NOTA_LUGZI = txtNotaLugzi.Text;
                            notaRetirada.DATA_NOTA_LUGZI = dataBanco;
                        }
                    }

                    usuarioController.AtualizaNotaRetirada(notaRetirada);

                    LabelFeedBack.Text = "Registro Atualizado com Sucesso !!!";
                }
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }
    }
}