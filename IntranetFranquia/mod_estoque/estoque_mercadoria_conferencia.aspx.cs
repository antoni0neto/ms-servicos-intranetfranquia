using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;

namespace Relatorios
{
    public partial class estoque_mercadoria_conferencia : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        EstoqueController estoqueController = new EstoqueController();
        ProducaoController prodController = new ProducaoController();

        List<ESTOQUE_LOJA_NF_RECEB_PRODUTO> g_produto = new List<ESTOQUE_LOJA_NF_RECEB_PRODUTO>();
        List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult> g_produtoRecebidoLoja = new List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                USUARIO usuario = null;

                if (Session["USUARIO"] != null)
                {
                    usuario = new USUARIO();
                    usuario = (USUARIO)Session["USUARIO"];

                    //Validação de Perfil do Usuário
                    if (usuario.CODIGO_PERFIL == 2)
                        Response.Redirect("~/mod_estoque/estoque_menu.aspx");

                }

                //Evitar duplo clique no botão
                btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            }
        }

        #region "GRID INICIAL"
        private List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult> ObterProdutoRecebido(string codigoProduto)
        {
            return estoqueController.ObterProdutoRecebidoLojaNota(codigoProduto, "");
        }
        private void RecarregarNotaMercadoria(string codigoProduto, string numeroNota)
        {
            //Obter Notas no LINX
            List<SP_OBTER_PRODUTO_RECEBIDO_LOJAResult> _produtoRecebido = new List<SP_OBTER_PRODUTO_RECEBIDO_LOJAResult>();
            _produtoRecebido = estoqueController.ObterProdutoRecebidoLoja(codigoProduto, numeroNota).OrderByDescending(p => p.DIV).ToList();

            if (_produtoRecebido != null)
            {
                gvProdutoRecebido.DataSource = _produtoRecebido;
                gvProdutoRecebido.DataBind();
            }
        }
        protected void btVerificarNota_Click(object sender, EventArgs e)
        {
            string codigoProduto = "";
            string msg = "";
            Button bt = (Button)sender;
            if (bt != null)
            {

                try
                {
                    codigoProduto = bt.CommandArgument;

                    //Abrir pop-up
                    string _url = "fnAbrirTelaCadastroMaior('estoque_mercadoria_conferencia_detalhe.aspx?p=" + codigoProduto + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void gvProdutoRecebido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int totalEmTransito = 0, totalRecebido = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_RECEBIDO_LOJAResult _produtoRecebidoLoja = e.Row.DataItem as SP_OBTER_PRODUTO_RECEBIDO_LOJAResult;

                    if (_produtoRecebidoLoja != null)
                    {
                        g_produtoRecebidoLoja = ObterProdutoRecebido(_produtoRecebidoLoja.PRODUTO);

                        Literal _litLojaRecebida = e.Row.FindControl("litLojaRecebida") as Literal;
                        if (_litLojaRecebida != null)
                        {
                            //Bind nos produtos recebidos com divergencia
                            totalRecebido = g_produtoRecebidoLoja.Where(p => p.CODIGO > 0 && (p.STATUS != "OK" && p.STATUS_CONFERENCIA != 'F')).Count();
                            _litLojaRecebida.Text = totalRecebido.ToString();
                        }

                        Literal _litLojaEmTransito = e.Row.FindControl("litLojaEmTransito") as Literal;
                        if (_litLojaEmTransito != null)
                        {
                            //Bind nos produtos em transito
                            totalEmTransito = g_produtoRecebidoLoja.Where(p => p.CODIGO == 0).Count();
                            _litLojaEmTransito.Text = totalEmTransito.ToString();
                        }

                        Button _btVerificarNota = e.Row.FindControl("btVerificarNota") as Button;
                        if (_btVerificarNota != null)
                            _btVerificarNota.CommandArgument = _produtoRecebidoLoja.PRODUTO.ToString();
                    }
                }
            }
        }
        protected void gvProdutoRecebido_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProdutoRecebido.PageIndex = e.NewPageIndex;
            RecarregarNotaMercadoria(txtCodigoLinx.Text.Trim(), txtNota.Text.Trim());
        }
        #endregion

        #region "DADOS INICIAIS"
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (txtCodigoLinx.Text.Trim() == "" && txtNota.Text.Trim() == "")
                {
                    labErro.Text = "Informe um PRODUTO e/ou uma NOTA.";
                    return;
                }

                //Buscar notas das mercadorias
                RecarregarNotaMercadoria(txtCodigoLinx.Text.Trim(), txtNota.Text.Trim());

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion
    }
}
