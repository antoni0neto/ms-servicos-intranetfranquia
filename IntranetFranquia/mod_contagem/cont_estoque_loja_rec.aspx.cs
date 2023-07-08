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
using System.Data.OleDb;

namespace Relatorios
{
    public partial class cont_estoque_loja_rec : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataContagem.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                CarregarFilial();

                txtDataContagem.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            }

            //Evitar duplo clique no botão
            btGerar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = baseController.BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                        ddlFilial_SelectedIndexChanged(null, null);
                    }

                }
            }
        }
        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarContagem(ddlFilial.SelectedValue);
        }
        private void CarregarContagem(string codigoFilial)
        {
            var contagem = contController.ObterEstoqueLojaContagem(codigoFilial).Where(p => p.CODIGO_PAI == null).ToList();
            if (contagem != null)
            {
                contagem.Insert(0, new ESTOQUE_LOJA_CONT { CODIGO = 0, DESCRICAO = "Selecione" });

                ddlContagem.DataSource = contagem;
                ddlContagem.DataBind();
            }
        }
        #endregion

        protected void btGerar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            hidCodigoEstoqueLojaCont.Value = "";
            btAbrirRecontagem.Visible = false;

            try
            {
                if (ddlFilial.SelectedValue == "")
                {
                    labErro.Text = "Selecione uma Filial.";
                    return;
                }

                if (ddlFilial.SelectedValue == "0")
                {
                    labErro.Text = "Selecione uma Contagem.";
                    return;
                }

                if (txtDescricao.Text.Trim() == "")
                {
                    labErro.Text = "Informe uma Descrição.";
                    return;
                }

                if (txtDataContagem.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Data da Contagem.";
                    return;
                }

                //inserir cabecalho
                ESTOQUE_LOJA_CONT estoqueLojaCont = new ESTOQUE_LOJA_CONT();
                estoqueLojaCont.CODIGO_FILIAL = ddlFilial.SelectedValue;
                estoqueLojaCont.CODIGO_PAI = Convert.ToInt32(ddlContagem.SelectedValue);
                estoqueLojaCont.DATA_CONTAGEM = Convert.ToDateTime(txtDataContagem.Text);
                estoqueLojaCont.DESCRICAO = txtDescricao.Text.Trim().ToUpper();
                estoqueLojaCont.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                estoqueLojaCont.DATA_INCLUSAO = DateTime.Now;
                int codigoEstoqueLojaCont = contController.InserirEstoqueLojaContagem(estoqueLojaCont);

                //Obter valores do arquivo para recontagem
                ESTOQUE_LOJA_CONT_ARQ arq = null;
                var estRecArq = contController.ObterEstoqueLojaContagemArq(estoqueLojaCont.CODIGO_PAI.Value);
                foreach (var es in estRecArq)
                {
                    arq = new ESTOQUE_LOJA_CONT_ARQ();
                    arq.COR = es.COR;
                    arq.ESTOQUE_LOJA_CONT = codigoEstoqueLojaCont;
                    arq.PRODUTO = es.PRODUTO;
                    arq.QTDE = es.QTDE;
                    arq.TAMANHO = es.TAMANHO;
                    contController.InserirEstoqueLojaContagemArq(arq);
                }

                //INSERE SALDO ESTOQUE PA
                if (contController.GerarSaldoEstoquePA(Convert.ToDateTime(txtDataContagem.Text), ddlFilial.SelectedItem.Text, codigoEstoqueLojaCont))
                {
                    labErro.Text = "Recontagem realizada com sucesso.";
                    hidCodigoEstoqueLojaCont.Value = codigoEstoqueLojaCont.ToString();
                    btAbrirRecontagem.Visible = true;

                    btAbrirRecontagem_Click(btAbrirRecontagem, null);
                }


            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }



        }

        protected void btAbrirRecontagem_Click(object sender, EventArgs e)
        {
            string url = "";
            string msg = "";
            try
            {
                string codigoEstoqueLojaCont = hidCodigoEstoqueLojaCont.Value;

                //Abrir pop-up
                url = "fnAbrirTelaCadastroMaior('cont_estoque_loja_rel_rec_detalhes.aspx?p=" + codigoEstoqueLojaCont + "&e=1');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
            }
            catch (Exception)
            {
                msg = "Erro ao abrir o Relatório.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }
        }



    }
}
