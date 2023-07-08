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
using System.Text;

namespace Relatorios
{
    public partial class dre_lancamento_atual_edit : System.Web.UI.Page
    {
        ContabilidadeController contabilController = new ContabilidadeController();
        DREController dreController = new DREController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                int lancamento = 0;
                int item = 0;
                if (Request.QueryString["l"] == null || Request.QueryString["l"] == "" ||
                    Request.QueryString["i"] == null || Request.QueryString["i"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("dre_menu.aspx");

                lancamento = Convert.ToInt32(Request.QueryString["l"].ToString());
                item = Convert.ToInt32(Request.QueryString["i"].ToString());

                DRE_CTB_LANCAMENTO_ITEM lancItem = dreController.ObterDRELancamento(lancamento, item);
                if (lancItem == null)
                    Response.Redirect("dre_menu.aspx");

                CarregarFilial();
                CarregarContaContabil();
                CarregarCentroCusto();

                txtLancamento.Text = lancItem.LANCAMENTO.ToString();
                txtItem.Text = lancItem.ITEM.ToString();
                txtFilial.Text = baseController.BuscaFilialCodigo(Convert.ToInt32(lancItem.RATEIO_FILIAL)).FILIAL;
                txtCentroCusto.Text = dreController.ObterCentroCusto().Where(p => p.CENTRO_CUSTO.Trim() == lancItem.RATEIO_CENTRO_CUSTO.Trim()).FirstOrDefault().DESC_CENTRO_CUSTO;
                txtContaContabil.Text = contabilController.ObterContasFiltro(lancItem.CONTA_CONTABIL.Trim(), "", "", "", "", "").FirstOrDefault().DESC_CONTA;

                txtDebito.Text = Convert.ToDecimal(lancItem.DEBITO).ToString("###,###,###,##0.00");
                txtHistorico.Text = lancItem.HISTORICO;

                ddlFilial.SelectedValue = lancItem.RATEIO_FILIAL.Trim();
                ddlContaContabil.SelectedValue = lancItem.CONTA_CONTABIL.Trim();
                ddlCCusto.SelectedValue = dreController.ObterCentroCusto().Where(p => p.CENTRO_CUSTO.Trim() == lancItem.RATEIO_CENTRO_CUSTO.Trim()).FirstOrDefault().CENTRO_CUSTO;


                hidLancamento.Value = lancamento.ToString();
                hidItem.Value = item.ToString();

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            List<FILIAI1> filial = new List<FILIAI1>();
            //List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.ListarFiliais();
            //filialDePara = baseController.BuscaFilialDePara();

            //filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI1 { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        private void CarregarContaContabil()
        {
            ddlContaContabil.DataSource = contabilController.ObterContasFiltro("3.%", "", "", "", "", "");

            ddlContaContabil.DataBind();
            ddlContaContabil.Items.Insert(0, new ListItem("", ""));
        }
        private void CarregarCentroCusto()
        {
            var centroCusto = dreController.ObterCentroCusto().Where(p => !p.INATIVA).ToList();
            if (centroCusto != null)
            {
                centroCusto.Insert(0, new CTB_CENTRO_CUSTO { CENTRO_CUSTO = "", DESC_CENTRO_CUSTO = "" });

                ddlCCusto.DataSource = centroCusto;
                ddlCCusto.DataBind();
            }
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            //labFornecedor.ForeColor = _OK;
            //if (ddlFornecedor.SelectedValue.Trim() == "Selecione")
            //{
            //    labFornecedor.ForeColor = _notOK;
            //    retorno = false;
            //}

            return retorno;
        }

        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                int lancamento = Convert.ToInt32(hidLancamento.Value);
                int item = Convert.ToInt32(hidItem.Value);


                var lancItem = dreController.ObterDRELancamento(lancamento, item);
                if (lancItem != null)
                {
                    lancItem.RATEIO_FILIAL = ddlFilial.SelectedValue;
                    lancItem.CONTA_CONTABIL = ddlContaContabil.SelectedValue;
                    lancItem.RATEIO_CENTRO_CUSTO = ddlCCusto.SelectedValue;

                    lancItem.DATA_ALTERACAO = DateTime.Now;
                    lancItem.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    string alt = "";

                    alt = alt + "FILIAL De: " + txtFilial.Text + " Para: " + ddlFilial.SelectedItem.Text.Trim() + System.Environment.NewLine;
                    alt = alt + "CONTA CONTÁBIL De: " + txtContaContabil.Text + " Para: " + ddlContaContabil.SelectedItem.Text.Trim() + System.Environment.NewLine;
                    alt = alt + "CENTRO DE CUSTO De: " + txtCentroCusto.Text + " Para: " + ddlCCusto.SelectedItem.Text.Trim() + System.Environment.NewLine;
                    lancItem.OBS_ALTERACAO = alt;

                    dreController.AtualizarDRELancamento(lancItem);
                    labErro.Text = "Lançamento atualizado com sucesso.";
                }

                btSalvar.Enabled = false;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

    }
}
