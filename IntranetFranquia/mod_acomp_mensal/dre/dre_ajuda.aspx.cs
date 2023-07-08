using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;

namespace Relatorios
{
    public partial class dre_ajuda : System.Web.UI.Page
    {
        DREController dreController = new DREController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] == null || Request.QueryString["id"] == "")
                    Response.Redirect("Login.aspx");

                var ID = Convert.ToInt32(Request.QueryString["id"].ToString());
                var tipo = Request.QueryString["tt"].ToString();
                var grupo = Request.QueryString["g"].ToString();
                var linha = Request.QueryString["l"].ToString();
                var linhaExtra = Request.QueryString["le"].ToString();

                hidID.Value = ID.ToString();
                hidTipo.Value = tipo;

                var ajuda = dreController.ObterAjuda(ID, grupo, linha, linhaExtra, tipo);
                if (ajuda != null)
                {
                    txtGrupo.Text = ajuda.GRUPO;
                    txtLinha.Text = ajuda.LINHA;
                    txtLinhaExtra.Text = ajuda.LINHA_EXTRA;
                    txtDescricao.Text = ajuda.DESCRICAO;
                    txtQuery.Text = ajuda.QUERY;

                    txtGrupo.Enabled = false;
                    txtLinha.Enabled = false;
                    txtLinhaExtra.Enabled = false;
                }

            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            var ajuda = dreController.ObterAjuda(Convert.ToInt32(hidID.Value), txtGrupo.Text, txtLinha.Text, txtLinhaExtra.Text, hidTipo.Value);

            if (ajuda == null)
            {
                ajuda = new DRE_AJUDA();
                ajuda.ID = Convert.ToInt32(hidID.Value);
                ajuda.GRUPO = txtGrupo.Text;
                ajuda.LINHA = txtLinha.Text;
                ajuda.LINHA_EXTRA = txtLinhaExtra.Text;
                ajuda.DESCRICAO = txtDescricao.Text;
                ajuda.QUERY = txtQuery.Text;
                ajuda.TIPO = hidTipo.Value;
                dreController.InserirAjudaDRE(ajuda);
            }
            else
            {
                ajuda.TIPO = hidTipo.Value;
                ajuda.DESCRICAO = txtDescricao.Text;
                ajuda.QUERY = txtQuery.Text;
                dreController.AtualizarAjudaDRE(ajuda);
            }

        }


    }
}
