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
    public partial class pacab_pedido_edit : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {

                int codigoDesenvProduto = 0;
                string mostruario = "";
                if (Request.QueryString["d"] == null || Request.QueryString["d"] == "" ||
                    Request.QueryString["m"] == null || Request.QueryString["m"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("facc_menu.aspx");

                codigoDesenvProduto = Convert.ToInt32(Request.QueryString["d"].ToString());
                mostruario = Request.QueryString["m"].ToString();

                DESENV_PRODUTO desenvProduto = desenvController.ObterProduto(codigoDesenvProduto);
                if (desenvProduto == null)
                    Response.Redirect("pacab_menu.aspx");

                hidCodigoProduto.Value = codigoDesenvProduto.ToString();
                hidCor.Value = desenvProduto.COR;
                hidMostruario.Value = mostruario;

                CarregarFornecedor();

                txtColecao.Text = new BaseController().BuscaColecaoAtual(desenvProduto.COLECAO).DESC_COLECAO.Trim();
                txtGrupoProduto.Text = desenvProduto.GRUPO;
                txtProduto.Text = desenvProduto.MODELO;
                txtNome.Text = desenvProduto.DESC_MODELO;
                txtCor.Text = prodController.ObterCoresBasicas(desenvProduto.COR).DESC_COR.Trim();
                txtGriffe.Text = desenvProduto.GRIFFE;
                txtMostruario.Text = (mostruario == "S") ? "Sim" : "Não";

                txtQtdeVarejo.Text = desenvProduto.QTDE.ToString();
                txtQtdeAtacado.Text = desenvProduto.QTDE_ATACADO.ToString();

                ddlFornecedor.SelectedValue = prodController.ObterFornecedor(desenvProduto.FORNECEDOR, 'T').FORNECEDOR;

                CarregarFiliais(mostruario);

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFornecedor()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();
            if (_fornecedores != null)
            {
                _fornecedores = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'T') || p.STATUS == 'S').ToList();

                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });
                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }
        }
        private void CarregarFiliais(string mostruario)
        {
            var f = new List<ListItem>();
            var fR = new List<ListItem>();

            if (mostruario == "S")
            {
                f.Add(new ListItem("CD MOSTRUARIO", "000041"));
                fR.Add(new ListItem("CD MOSTRUARIO 100%", "000041"));
            }
            else
            {
                f.Add(new ListItem("Selecione", ""));

                f.Add(new ListItem("CD - LUGZY", "000029"));
                f.Add(new ListItem("ATACADO HANDBOOK", "1029"));
                f.Add(new ListItem("CD MOSTRUARIO", "000041"));

                fR.Add(new ListItem("Selecione", ""));
                fR.Add(new ListItem("CD - LUGZY 100%", "000029"));
                fR.Add(new ListItem("ATACADO HANDBOOK 100%", "1029"));
                fR.Add(new ListItem("CD MOSTRUARIO 100%", "000041"));

            }

            ddlFilial.DataSource = f;
            ddlFilial.DataBind();

            ddlFilialRateio.DataSource = fR;
            ddlFilialRateio.DataBind();

        }
        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFilialRateio.SelectedValue = ddlFilial.SelectedValue;
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

            labDataEntrega.ForeColor = _OK;
            if (txtDataEntrega.Text.Trim() == "")
            {
                labDataEntrega.ForeColor = _notOK;
                retorno = false;
            }

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            labFilialRateio.ForeColor = _OK;
            if (ddlFilialRateio.SelectedValue == "")
            {
                labFilialRateio.ForeColor = _notOK;
                retorno = false;
            }

            labCusto.ForeColor = _OK;
            if (txtCustoProduto.Text.Trim() == "")
            {
                labCusto.ForeColor = _notOK;
                retorno = false;
            }

            labStatus.ForeColor = _OK;
            if (ddlStatus.SelectedValue.Trim() == "")
            {
                labStatus.ForeColor = _notOK;
                retorno = false;
            }

            labGradeTitulo.ForeColor = _OK;
            if (txtGradeTotal_E.Text.Trim() == "" || txtGradeTotal_E.Text.Trim() == "0")
            {
                labGradeTitulo.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }

        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            string produto;
            string corProduto;
            string fornecedor;
            DateTime limiteEntrega = DateTime.Now.Date;
            string filial;
            string codigoFilialRateio;
            decimal custoProduto = 0;
            char status;
            string aprovadoPor;
            int exp = 0;
            int xp = 0;
            int pp = 0;
            int p = 0;
            int m = 0;
            int g = 0;
            int gg = 0;

            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Por favor, preencher os campos em vermelho.";
                    return;
                }

                var pedidoJaExiste = desenvController.ObterProdutoPedidoLinx(hidCodigoProduto.Value, Convert.ToChar(hidMostruario.Value)).Where(x => x.FILIAL.Trim() == ddlFilial.SelectedItem.Text.Trim()).FirstOrDefault();
                if (pedidoJaExiste != null)
                {
                    labErro.Text = "Produto já possui pedido de compra para " + ddlFilial.SelectedItem.Text.Trim() + ". Pedido: " + pedidoJaExiste.PEDIDO;
                    return;
                }

                produto = txtProduto.Text.Trim();
                corProduto = hidCor.Value.Trim();
                fornecedor = ddlFornecedor.SelectedValue.Trim();
                limiteEntrega = Convert.ToDateTime(txtDataEntrega.Text);
                filial = ddlFilial.SelectedItem.Text.Trim();
                codigoFilialRateio = ddlFilialRateio.SelectedValue.Trim();
                custoProduto = Convert.ToDecimal(txtCustoProduto.Text);
                status = 'A';

                var usuario = (USUARIO)Session["USUARIO"];
                if (usuario != null)
                    aprovadoPor = usuario.NOME_USUARIO.Trim();
                else
                    aprovadoPor = "USUARIO-INTRANET";

                if (txtGradeEXP_E.Text.Trim() != "")
                    exp = Convert.ToInt32(txtGradeEXP_E.Text);

                if (txtGradeXP_E.Text.Trim() != "")
                    xp = Convert.ToInt32(txtGradeXP_E.Text);

                if (txtGradePP_E.Text.Trim() != "")
                    pp = Convert.ToInt32(txtGradePP_E.Text);

                if (txtGradeP_E.Text.Trim() != "")
                    p = Convert.ToInt32(txtGradeP_E.Text);

                if (txtGradeM_E.Text.Trim() != "")
                    m = Convert.ToInt32(txtGradeM_E.Text);

                if (txtGradeG_E.Text.Trim() != "")
                    g = Convert.ToInt32(txtGradeG_E.Text);

                if (txtGradeGG_E.Text.Trim() != "")
                    gg = Convert.ToInt32(txtGradeGG_E.Text);

                var pedidoCompra = desenvController.GerarPedidoCompraProdutoAcabado(produto, corProduto, fornecedor, limiteEntrega, filial, codigoFilialRateio, custoProduto, status, aprovadoPor, exp, xp, pp, p, m, g, gg);

                if (pedidoCompra != null && pedidoCompra.NUMERO_PEDIDO != "")
                {
                    /*GERAR REGISTRO PARA ENTRADA DE PRODUTO ACABADO*/
                    DESENV_PRODUTO_PEDLINX produtoPedLinx = new DESENV_PRODUTO_PEDLINX();
                    produtoPedLinx.DESENV_PRODUTO = Convert.ToInt32(hidCodigoProduto.Value);
                    produtoPedLinx.FILIAL = ddlFilial.SelectedItem.Text;
                    produtoPedLinx.PEDIDO = pedidoCompra.NUMERO_PEDIDO;
                    produtoPedLinx.DATA_INCLUSAO = DateTime.Now;
                    produtoPedLinx.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    produtoPedLinx.MOSTRUARIO = Convert.ToChar(hidMostruario.Value);
                    produtoPedLinx.DATA_ENTREGA = Convert.ToDateTime(txtDataEntrega.Text.Trim());
                    produtoPedLinx.CUSTO = Convert.ToDecimal(txtCustoProduto.Text.Trim());
                    produtoPedLinx.FORNECEDOR = ddlFornecedor.SelectedItem.Text.Trim();

                    int codigoPedLinx = desenvController.InserirProdutoPedidoLinx(produtoPedLinx);

                    //GERAR ENTRADA PEDIDO LINX
                    DESENV_PRODUTO_PEDLINX_ENTRADA produtoPedLinxEntrada = new DESENV_PRODUTO_PEDLINX_ENTRADA();
                    produtoPedLinxEntrada.DESENV_PRODUTO_PEDLINX = codigoPedLinx;
                    produtoPedLinxEntrada.GRADE_EXP = 0;
                    produtoPedLinxEntrada.GRADE_XP = 0;
                    produtoPedLinxEntrada.GRADE_PP = 0;
                    produtoPedLinxEntrada.GRADE_P = 0;
                    produtoPedLinxEntrada.GRADE_M = 0;
                    produtoPedLinxEntrada.GRADE_G = 0;
                    produtoPedLinxEntrada.GRADE_GG = 0;
                    produtoPedLinxEntrada.GRADE_TOTAL = 0;
                    produtoPedLinxEntrada.STATUS = 'A';
                    produtoPedLinxEntrada.DATA_INCLUSAO = DateTime.Now;

                    desenvController.InserirProdutoPedidoLinxEntrada(produtoPedLinxEntrada);

                    labErro.Text = "Pedido de Compra " + ddlFilial.SelectedItem.Text.Trim() + " gerado com sucesso: " + pedidoCompra.NUMERO_PEDIDO;

                    btSalvar.Enabled = false;
                }
                else
                {
                    labErro.Text = "Erro ao gerar pedido. Contate o TI.";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "PEDIDO"
        #endregion

        #region "EMAIL"
        private void EnviarEmail(PROD_HB_SAIDA _saida, int tela)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            string assunto = "";
            string titulo = "";
            string subTitulo = "";
            if (_saida.TIPO == 'I')
                titulo = "INTERNO ";
            else
                titulo = "EMISSÃO NF ";

            if (tela == 21)
                subTitulo = "ACABAMENTO";

            assunto = "Intranet: " + titulo + "" + subTitulo + " - O.P.: " + _saida.PROD_HB1.ORDEM_PRODUCAO;
            email.ASSUNTO = assunto;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(_saida, assunto, tela);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails 
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(7, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(PROD_HB_SAIDA _saida, string assunto, int tela)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");

            return sb.ToString();
        }
        #endregion

        protected void btNovo_Click(object sender, EventArgs e)
        {
            btSalvar.Enabled = true;
            txtGradeEXP_E.Text = "";
            txtGradeXP_E.Text = "";
            txtGradePP_E.Text = "";
            txtGradeP_E.Text = "";
            txtGradeM_E.Text = "";
            txtGradeG_E.Text = "";
            txtGradeGG_E.Text = "";
            txtGradeTotal_E.Text = "";

            ddlFilial.SelectedValue = "";
            ddlFilialRateio.SelectedValue = "";
        }

    }
}
