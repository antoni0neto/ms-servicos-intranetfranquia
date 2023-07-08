using DAL;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class fisc_baixa_nf_aliq_icms : System.Web.UI.Page
    {
        ContabilidadeController contController = new ContabilidadeController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarNFs();
            }

        }


        #region "NF"
        private List<SP_OBTER_NF_ALIQ_ICMSResult> ObterNFAliqImposto(int tipo)
        {
            var nfAliqImposto = contController.ObterNFAliqImposto(tipo);
            return nfAliqImposto;
        }
        private void CarregarNFs()
        {
            var nfAliqImposto = ObterNFAliqImposto(0);

            gvNFAliquotaICMS.DataSource = nfAliqImposto;
            gvNFAliquotaICMS.DataBind();

            var nfRegimeEspecial = ObterNFAliqImposto(1);

            gvNFREgimeEspecial.DataSource = nfRegimeEspecial;
            gvNFREgimeEspecial.DataBind();

        }
        protected void gvNFAliquotaICMS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_NF_ALIQ_ICMSResult aliqICMS = e.Row.DataItem as SP_OBTER_NF_ALIQ_ICMSResult;

                    if (aliqICMS != null)
                    {
                        Literal _litEmissao = e.Row.FindControl("litEmissao") as Literal;
                        if (_litEmissao != null)
                            _litEmissao.Text = Convert.ToDateTime(aliqICMS.EMISSAO).ToString("dd/MM/yyyy");

                        Literal _litAutorizacaoNFE = e.Row.FindControl("litAutorizacaoNFE") as Literal;
                        if (_litAutorizacaoNFE != null)
                            _litAutorizacaoNFE.Text = Convert.ToDateTime(aliqICMS.DATA_AUTORIZACAO_NFE).ToString("dd/MM/yyyy");

                    }
                }
            }
        }

        protected void btBaixar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {

                        string filial = ((Literal)row.FindControl("litFilial")).Text;
                        string nfSaida = ((Literal)row.FindControl("litNFSaida")).Text;
                        string serieNF = ((Literal)row.FindControl("litSerieNF")).Text;
                        DateTime emissao = Convert.ToDateTime(((Literal)row.FindControl("litEmissao")).Text);

                        CTB_ALIQUOTA_ICMS_BAIXA aliqICMSBAixa = new CTB_ALIQUOTA_ICMS_BAIXA();
                        aliqICMSBAixa.FILIAL = filial;
                        aliqICMSBAixa.NF_SAIDA = nfSaida;
                        aliqICMSBAixa.SERIE_NF = serieNF;
                        aliqICMSBAixa.EMISSAO = emissao;
                        aliqICMSBAixa.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        aliqICMSBAixa.DATA_INCLUSAO = DateTime.Now;
                        contController.InserirNFAliqImposto(aliqICMSBAixa);

                        CarregarNFs();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion


    }
}

