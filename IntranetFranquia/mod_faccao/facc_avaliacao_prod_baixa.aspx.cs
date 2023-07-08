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
    public partial class facc_avaliacao_prod_baixa : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                int codigoAvaliacao = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("facc_menu.aspx");

                codigoAvaliacao = Convert.ToInt32(Request.QueryString["p"].ToString());

                PROD_HB_AVALIACAO aval = faccController.ObterFaccaoAvaliacao(codigoAvaliacao);
                if (aval == null)
                    Response.Redirect("facc_menu.aspx");

                var hb = prodController.ObterHB(aval.PROD_HB);
                if (hb != null)
                {
                    CarregarFornecedores();

                    hidCodigoAvaliacao.Value = codigoAvaliacao.ToString();

                    txtColecao.Text = baseController.BuscaColecaoAtual(hb.COLECAO).DESC_COLECAO;
                    txtHB.Text = hb.HB.ToString();
                    txtProduto.Text = hb.CODIGO_PRODUTO_LINX;
                    txtNome.Text = hb.NOME;
                    txtCor.Text = prodController.ObterCoresBasicas(hb.COR).DESC_COR;
                    ddlFornecedor.SelectedValue = aval.FORNECEDOR;
                    txtDataEnvio.Text = Convert.ToDateTime(aval.DATA_INCLUSAO).ToString("dd/MM/yyyy");

                    txtNota1.Text = (aval.NOTA1 == null) ? "" : aval.NOTA1.ToString();
                    txtObs1.Text = aval.OBS1;
                    txtNota2.Text = (aval.NOTA2 == null) ? "" : aval.NOTA2.ToString();
                    txtObs2.Text = aval.OBS2;
                }
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFornecedores()
        {

            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'S') || p.STATUS == 'S').ToList();

                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });
                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }

        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFornecedor.ForeColor = _OK;
            if (ddlFornecedor.SelectedValue.Trim() == "Selecione")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labNota1.ForeColor = _OK;
            if (txtNota1.Text.Trim() == "")
            {
                labNota1.ForeColor = _notOK;
                retorno = false;
            }

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
                    labErro.Text = "Informe pelo menos a Nota 1.";
                    return;
                }

                var aval = faccController.ObterFaccaoAvaliacao(Convert.ToInt32(hidCodigoAvaliacao.Value));
                if (aval == null)
                {
                    labErro.Text = "Nenhuma avaliação encontrada. Entre em contato com TI.";
                    return;
                }

                if (txtNota1.Text != "")
                {
                    int nota = 0;
                    nota = Convert.ToInt32(txtNota1.Text);
                    if (nota < 0 || nota > 5)
                    {
                        labErro.Text = "Informe a Nota 1 entre 0 e 5.";
                        return;
                    }
                }

                if (txtNota2.Text != "")
                {
                    int nota = 0;
                    nota = Convert.ToInt32(txtNota2.Text);
                    if (nota < 0 || nota > 5)
                    {
                        labErro.Text = "Informe a Nota 2 entre 0 e 5.";
                        return;
                    }
                }

                if (txtNota1.Text.Trim() != "")
                    aval.NOTA1 = Convert.ToInt32(txtNota1.Text.Trim());
                aval.OBS1 = txtObs1.Text;
                if (txtNota2.Text.Trim() != "")
                    aval.NOTA2 = Convert.ToInt32(txtNota2.Text.Trim());
                aval.OBS2 = txtObs2.Text;
                aval.DATA_AVALIACAO = DateTime.Now;
                aval.USUARIO_AVALIACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                faccController.AtualizarFaccaoAvaliacao(aval);


                labErro.Text = "Nota cadastrada com sucesso.";
                btSalvar.Enabled = false;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }





    }
}
