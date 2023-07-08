using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;
using System.Text;
using System.IO;

namespace Relatorios
{
    public partial class desenv_material_devolcontrol : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        decimal qtdeDev = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarFornecedores();
                CarregarColecoes();
                CarregarGrupos();
                CarregarSubGrupos("");
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFornecedores()
        {
            var fornecedores = prodController.ObterFornecedor().Where(p => p.STATUS == 'A').ToList();
            if (fornecedores != null)
            {
                var fornecedoresAux = fornecedores.Where(p => p.FORNECEDOR != null).Select(s => s.FORNECEDOR.Trim()).Distinct().ToList();

                fornecedores = new List<PROD_FORNECEDOR>();
                foreach (var item in fornecedoresAux)
                    if (item.Trim() != "")
                        fornecedores.Add(new PROD_FORNECEDOR { FORNECEDOR = item.Trim() });

                fornecedores.OrderBy(p => p.FORNECEDOR);
                fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "" });

                ddlFornecedor.DataSource = fornecedores;
                ddlFornecedor.DataBind();
            }
        }
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecao.DataSource = colecoes;
                ddlColecao.DataBind();
            }
        }

        private void CarregarGrupos()
        {
            var matGrupo = desenvController.ObterMaterialGrupo();

            matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "" });
            ddlMaterialGrupo.DataSource = matGrupo;
            ddlMaterialGrupo.DataBind();

        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());
        }
        private void CarregarSubGrupos(string grupo)
        {
            var matSubGrupo = desenvController.ObterMaterialSubGrupo();

            if (grupo != "")
                matSubGrupo = matSubGrupo.Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();

            matSubGrupo = matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
            matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });

            ddlMaterialSubGrupo.DataSource = matSubGrupo;
            ddlMaterialSubGrupo.DataBind();
        }
        #endregion

        #region "TECIDOS"
        private List<DESENV_PEDIDO_QTDE> ObterDevolucoes()
        {
            var devolucoes = desenvController.ObterPedidoQtdeDevolucoes();

            if (ddlFornecedor.SelectedValue != "")
                devolucoes = devolucoes.Where(p => p.DESENV_PEDIDO_SUB1.FORNECEDOR.Trim() == ddlFornecedor.SelectedValue.Trim()).ToList();

            if (ddlColecao.SelectedValue != "")
                devolucoes = devolucoes.Where(p => p.DESENV_PEDIDO_SUB1.COLECAO.Trim() == ddlColecao.SelectedValue.Trim()).ToList();

            if (txtNumeroPedido.Text != "")
                devolucoes = devolucoes.Where(p => p.DESENV_PEDIDO1.NUMERO_PEDIDO == Convert.ToInt32(txtNumeroPedido.Text.Trim())).ToList();

            if (ddlMaterialGrupo.SelectedValue != "")
                devolucoes = devolucoes.Where(p => p.DESENV_PEDIDO1.GRUPO.Trim() == ddlMaterialGrupo.SelectedItem.Text.Trim()).ToList();

            if (ddlMaterialSubGrupo.SelectedValue != "")
                devolucoes = devolucoes.Where(p => p.DESENV_PEDIDO1.SUBGRUPO.Trim() == ddlMaterialSubGrupo.SelectedItem.Text.Trim()).ToList();

            var status = ddlStatus.SelectedValue;
            if (status != "")
            {
                //<asp:ListItem Value="A" Text="Aguardando Aprovação"></asp:ListItem>
                //<asp:ListItem Value="B" Text="Aguardando Emissão NF"></asp:ListItem>
                //<asp:ListItem Value="C" Text="Aguardando Retirada"></asp:ListItem>
                //<asp:ListItem Value="D" Text="Retirado"></asp:ListItem>

                if (status == "A")
                    devolucoes = devolucoes.Where(p => p.DATA_NOTA == null).ToList();
                else if (status == "B")
                    devolucoes = devolucoes.Where(p => p.DATA_NOTA != null && p.EMISSAO == null).ToList();
                else if (status == "C")
                    devolucoes = devolucoes.Where(p => p.DATA_NOTA != null && p.EMISSAO != null && p.DATA_RETIRADA == null).ToList();
                else if (status == "D")
                    devolucoes = devolucoes.Where(p => p.DATA_NOTA != null && p.EMISSAO != null && p.DATA_RETIRADA != null).ToList();
            }

            return devolucoes;
        }
        private void CarregarDevolucoes()
        {
            var devolucoes = ObterDevolucoes();
            gvDevolucao.DataSource = devolucoes;
            gvDevolucao.DataBind();
        }
        protected void gvDevolucao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO_QTDE pedidoQtde = e.Row.DataItem as DESENV_PEDIDO_QTDE;

                    if (pedidoQtde != null)
                    {
                        Label labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                        labFornecedor.Text = pedidoQtde.DESENV_PEDIDO_SUB1.FORNECEDOR;

                        Label labColecao = e.Row.FindControl("labColecao") as Label;
                        labColecao.Text = baseController.BuscaColecaoAtual(pedidoQtde.DESENV_PEDIDO_SUB1.COLECAO).DESC_COLECAO.Trim();

                        Label labTecido = e.Row.FindControl("labTecido") as Label;
                        labTecido.Text = pedidoQtde.DESENV_PEDIDO1.SUBGRUPO.Trim();

                        Label labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                        labCorFornecedor.Text = pedidoQtde.DESENV_PEDIDO1.COR_FORNECEDOR.Trim();

                        Label labDataEntrada = e.Row.FindControl("labDataEntrada") as Label;
                        labDataEntrada.Text = pedidoQtde.DATA.ToString("dd/MM/yyyy");

                        Label labEmissao = e.Row.FindControl("labEmissao") as Label;
                        labEmissao.Text = (pedidoQtde.EMISSAO == null) ? "-" : Convert.ToDateTime(pedidoQtde.EMISSAO).ToString("dd/MM/yyyy");

                        Label labQtdeDev = e.Row.FindControl("labQtdeDev") as Label;
                        labQtdeDev.Text = (pedidoQtde.QTDE * -1.000M).ToString("###,###,###,##0.000");

                        Label labDataNota = e.Row.FindControl("labDataNota") as Label;
                        labDataNota.Text = (pedidoQtde.DATA_NOTA == null) ? "-" : Convert.ToDateTime(pedidoQtde.DATA_NOTA).ToString("dd/MM/yyyy");

                        Label labDataRetirada = e.Row.FindControl("labDataRetirada") as Label;
                        labDataRetirada.Text = (pedidoQtde.DATA_RETIRADA == null) ? "-" : Convert.ToDateTime(pedidoQtde.DATA_RETIRADA).ToString("dd/MM/yyyy");

                        ImageButton btEntrarQtde = e.Row.FindControl("btEntrarQtde") as ImageButton;
                        btEntrarQtde.CommandArgument = pedidoQtde.DESENV_PEDIDO_SUB.ToString();

                        Label labStatus = e.Row.FindControl("labStatus") as Label;
                        var status = "-";
                        if (pedidoQtde.DATA_NOTA == null)
                            status = "Aguardando Aprovação";
                        else if (pedidoQtde.DATA_NOTA != null && pedidoQtde.EMISSAO == null)
                            status = "Aguardando Emissão NF";
                        else if (pedidoQtde.DATA_NOTA != null && pedidoQtde.EMISSAO != null && pedidoQtde.DATA_RETIRADA == null)
                            status = "Aguardando Retirada";
                        else if (pedidoQtde.DATA_NOTA != null && pedidoQtde.EMISSAO != null && pedidoQtde.DATA_RETIRADA != null)
                            status = "Retirado";
                        labStatus.Text = status;


                        qtdeDev += pedidoQtde.QTDE;

                    }
                }
            }
        }
        protected void gvDevolucao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvDevolucao.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[5].Text = (qtdeDev * -1.000M).ToString("###,###,###,##0.000");
            }
        }

        protected void btEntrarQtde_Click(object sender, EventArgs e)
        {
            var msg = "";
            try
            {
                ImageButton b = (ImageButton)sender;
                if (b != null)
                {
                    var codigoSubPedido = b.CommandArgument;

                    //Abrir pop-up
                    var url = "fnAbrirTelaCadastroMaior('desenv_material_compra_entradav2.aspx?sub=" + codigoSubPedido + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }
        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarDevolucoes();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

    }

}

