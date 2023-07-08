using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using DAL;


namespace Relatorios
{
    public partial class facc_fase_alterar : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

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
                if (tela != "1")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "facc_menu.aspx";

                CarregarColecoes();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {

            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecao.DataSource = colecoes;
                ddlColecao.DataBind();
            }
        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                hidCodigoSaida.Value = "";

                if (ddlColecao.SelectedValue.Trim() == "" || ddlColecao.SelectedValue.Trim() == "0")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                if (txtHB.Text.Trim() == "")
                {
                    labErro.Text = "Informe o número do HB.";
                    return;
                }

                var hb = prodController.ObterNumeroHB(ddlColecao.SelectedValue.Trim(), Convert.ToInt32(txtHB.Text.Trim())).Where(p => p.MOSTRUARIO == ((cbMostruario.Checked) ? 'S' : 'N') && p.CODIGO_PAI == null).FirstOrDefault();
                if (hb != null)
                {
                    imgFoto.ImageUrl = hb.FOTO_PECA;
                    imgFoto.Visible = true;

                    txtProduto.Text = hb.CODIGO_PRODUTO_LINX;
                    txtNome.Text = hb.NOME;
                    txtCor.Text = prodController.ObterCoresBasicas(hb.COR).DESC_COR;

                    PROD_HB_ROTA rotaHB = null;
                    if (hb.ORDEM_PRODUCAO != null)
                        rotaHB = prodController.ObterRotaOP(hb.ORDEM_PRODUCAO);

                    hidCodigoHB.Value = hb.CODIGO.ToString();
                }

                var faseAtual = faccController.ObterSaidaEmAberto(hb.CODIGO);
                if (faseAtual == null)
                {
                    labErro.Text = "Este HB não possui nenhuma fase em aberto. Verifique se existe entrada pendente ou se já foi finalizado.";
                    return;
                }

                hidCodigoSaida.Value = faseAtual.CODIGO.ToString();

                if (faseAtual.PROD_SERVICO == null)
                    faseAtual.PROD_SERVICO = 0;
                CarregarServicosFaseAtual(Convert.ToInt32(faseAtual.PROD_SERVICO));
                CarregarServicosParaFase(Convert.ToInt32(faseAtual.PROD_SERVICO));

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        private void CarregarServicosFaseAtual(int codigoServico)
        {

            var servicos = prodController.ObterServicoProducao().Where(p => p.STATUS == 'A' && p.CODIGO != 5).ToList();

            if (servicos != null)
            {
                servicos.Insert(0, new PROD_SERVICO { CODIGO = 0, DESCRICAO = "Sem Fase", STATUS = 'A' });
                ddlFaseAtual.DataSource = servicos;
                ddlFaseAtual.DataBind();

                ddlFaseAtual.SelectedValue = codigoServico.ToString();
            }
        }
        private void CarregarServicosParaFase(int codigoServico)
        {
            var servicos = prodController.ObterServicoProducao().Where(p => p.STATUS == 'A' && p.CODIGO != 5 && p.CODIGO != codigoServico).ToList();

            if (servicos != null)
            {
                servicos.Insert(0, new PROD_SERVICO { CODIGO = 0, DESCRICAO = "Selecione", STATUS = 'A' });
                ddlParaFase.DataSource = servicos;
                ddlParaFase.DataBind();
            }
        }

        protected void btAlterarFase_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlParaFase.SelectedValue == "0")
                {
                    labErro.Text = "Selecione uma fase para alterar.";
                    return;
                }

                if (hidCodigoSaida.Value == "")
                {
                    labErro.Text = "Saída não encontrada. Entre em contato com TI.";
                    return;
                }

                int codigoServico = Convert.ToInt32(ddlParaFase.SelectedValue);
                var saida = faccController.ObterSaidaHB(Convert.ToInt32(hidCodigoSaida.Value));

                saida.PROD_SERVICO = codigoServico;
                if (codigoServico == 1 || codigoServico == 2 || codigoServico == 3 || codigoServico == 6)
                    saida.PROD_PROCESSO = 20;
                else if (codigoServico == 4)
                    saida.PROD_PROCESSO = 21;

                faccController.AtualizarSaidaHB(saida);

                labErro.Text = "Fase alterada com sucesso.";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }







    }
}
