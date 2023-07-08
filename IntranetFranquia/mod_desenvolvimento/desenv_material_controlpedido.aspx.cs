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
    public partial class desenv_material_controlpedido : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        decimal qtdeSubPedido = 0;
        decimal valorSubPedido = 0;
        decimal qtdeSubPedidoRecebida = 0;

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
        private List<SP_OBTER_PEDIDOSUB_CONTROLEResult> ObterPedidos()
        {
            var numeroPedido = 0;
            if (txtNumeroPedido.Text != "")
                numeroPedido = Convert.ToInt32(txtNumeroPedido.Text);

            var subPedidos = desenvController.ObterPedidoSubControle(ddlFornecedor.SelectedValue.Trim(), ddlColecao.SelectedValue.Trim(), numeroPedido, ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim());

            if (ddlStatus.SelectedValue != "")
                subPedidos = subPedidos.Where(p => p.STATUS == Convert.ToChar(ddlStatus.SelectedValue)).ToList();

            var pagamento = ddlPagamento.SelectedValue;
            if (pagamento != "")
            {
                //<asp:ListItem Value="1" Text="Em Aberto"></asp:ListItem>
                //<asp:ListItem Value="2" Text="Pago"></asp:ListItem>
                //<asp:ListItem Value="3" Text="Sem Cadastro"></asp:ListItem>
                if (ddlPagamento.SelectedValue == "1")
                    subPedidos = subPedidos.Where(p => p.QTDE_PARCELA > 0 && p.QTDE_PARCELA_PAGA != p.QTDE_PARCELA).ToList();
                else if (ddlPagamento.SelectedValue == "2")
                    subPedidos = subPedidos.Where(p => p.QTDE_PARCELA > 0 && p.QTDE_PARCELA_PAGA == p.QTDE_PARCELA).ToList();
                else if (ddlPagamento.SelectedValue == "3")
                    subPedidos = subPedidos.Where(p => p.QTDE_PARCELA <= 0 && p.QTDE_PARCELA_PAGA <= 0).ToList();
            }

            return subPedidos;
        }
        private void CarregarPedidos()
        {
            var subPedido = ObterPedidos();

            gvSubPedido.DataSource = subPedido;
            gvSubPedido.DataBind();
        }
        protected void gvSubPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PEDIDOSUB_CONTROLEResult subPedido = e.Row.DataItem as SP_OBTER_PEDIDOSUB_CONTROLEResult;

                    Label labNumeroSubPedido = e.Row.FindControl("labNumeroSubPedido") as Label;
                    labNumeroSubPedido.Text = subPedido.NUMERO_PEDIDO.ToString() + " - " + ((subPedido.ITEM == null) ? "X" : subPedido.ITEM.ToString());

                    Label labDataPedido = e.Row.FindControl("labDataPedido") as Label;
                    labDataPedido.Text = Convert.ToDateTime(subPedido.DATA_PEDIDO).ToString("dd/MM/yyyy");

                    Label labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                    labFornecedor.Text = subPedido.FORNECEDOR;

                    Label labColecao = e.Row.FindControl("labColecao") as Label;
                    labColecao.Text = subPedido.DESC_COLECAO.Trim();

                    Label labQtde = e.Row.FindControl("labQtde") as Label;
                    labQtde.Text = subPedido.QTDE.ToString();

                    Label labTecido = e.Row.FindControl("labTecido") as Label;
                    labTecido.Text = subPedido.SUBGRUPO.ToString();

                    Label labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                    labCorFornecedor.Text = subPedido.COR_FORNECEDOR.ToString();

                    Label labCusto = e.Row.FindControl("labCusto") as Label;
                    labCusto.Text = subPedido.CUSTO.ToString();

                    Label labValorTotal = e.Row.FindControl("labValorTotal") as Label;
                    labValorTotal.Text = "R$ " + Convert.ToDecimal(subPedido.VALOR_PEDIDO).ToString("###,###,###,##0.00");

                    Label labDataPedidoPrevisao = e.Row.FindControl("labDataPedidoPrevisao") as Label;
                    labDataPedidoPrevisao.Text = (subPedido.DATA_ENTREGA_PREV == null) ? "" : Convert.ToDateTime(subPedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy");

                    Label labDataPrevFinal = e.Row.FindControl("labDataPrevFinal") as Label;
                    labDataPrevFinal.Text = (subPedido.DATA_PREVISAO_FINAL == null) ? "-" : Convert.ToDateTime(subPedido.DATA_PREVISAO_FINAL).ToString("dd/MM/yyyy");

                    var status = "";
                    Label labStatus = e.Row.FindControl("labStatus") as Label;
                    if (subPedido.STATUS == 'R')
                        status = "Reserva";
                    else if (subPedido.STATUS == 'P')
                        status = "Pedido";
                    else if (subPedido.STATUS == 'B')
                        status = "Baixado";
                    labStatus.Text = status;

                    ImageButton btEntrarQtde = e.Row.FindControl("btEntrarQtde") as ImageButton;
                    btEntrarQtde.CommandArgument = subPedido.CODIGO.ToString();

                    ImageButton btBaixarSubPedido = e.Row.FindControl("btBaixarSubPedido") as ImageButton;
                    btBaixarSubPedido.CommandArgument = subPedido.CODIGO.ToString();

                    if (subPedido.DATA_BAIXA == null && (subPedido.DATA_ENTREGA_PREV < DateTime.Now.Date))
                        e.Row.BackColor = Color.LightGoldenrodYellow;

                    if (subPedido.DATA_BAIXA == null && (subPedido.DATA_ENTREGA_PREV < DateTime.Now.Date && subPedido.DATA_ENTREGA_PREV != ((subPedido.DATA_PREVISAO_FINAL == null) ? subPedido.DATA_ENTREGA_PREV : subPedido.DATA_PREVISAO_FINAL)))
                        e.Row.BackColor = Color.Pink;

                    if (subPedido.DATA_BAIXA != null)
                        btBaixarSubPedido.Visible = false;

                    qtdeSubPedido += Convert.ToDecimal(subPedido.QTDE);
                    valorSubPedido += (Convert.ToDecimal(subPedido.VALOR_PEDIDO));

                    Label labQtdeEntregue = e.Row.FindControl("labQtdeEntregue") as Label;
                    var pedSubQtdeRec = desenvController.ObterPedidoQtdePedidoPorSub(subPedido.CODIGO);
                    if (pedSubQtdeRec != null)
                    {
                        labQtdeEntregue.Text = pedSubQtdeRec.Sum(p => p.QTDE).ToString();
                        qtdeSubPedidoRecebida += pedSubQtdeRec.Sum(p => p.QTDE);
                    }

                    CriarPagto(subPedido, e.Row);

                }
            }
        }
        protected void gvSubPedido_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvSubPedido.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[7].Text = qtdeSubPedido.ToString();
                footer.Cells[9].Text = "R$ " + valorSubPedido.ToString("###,###,###,##0.00");
                footer.Cells[12].Text = qtdeSubPedidoRecebida.ToString();
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
                    string codigoSubPedido = b.CommandArgument;

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
        protected void btBaixarSubPedido_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;

                var codigoSubPedido = bt.CommandArgument;

                var subPedido = desenvController.ObterPedidoSub(Convert.ToInt32(codigoSubPedido));
                if (subPedido == null)
                    return;

                subPedido.STATUS = 'B';
                subPedido.DATA_BAIXA = DateTime.Now;

                desenvController.AtualizarPedidoSub(subPedido);
                CarregarPedidos();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CriarPagto(SP_OBTER_PEDIDOSUB_CONTROLEResult pedidoSub, GridViewRow row)
        {
            var vermelho = "#B22222";
            var verde = "green";

            var pgto = desenvController.ObterPedidoSubPgtoPorDesenvPedidoSub(pedidoSub.CODIGO);

            Literal litpgto = null;
            var i = 1;
            foreach (var p in pgto)
            {
                var valor = ((p.VALOR <= 0) ? (p.PORC.ToString("##0.00") + "%") : ("R$ " + p.VALOR.ToString("###,###,##0.00")));

                litpgto = row.FindControl("litParc" + i.ToString()) as Literal;

                litpgto.Text = litpgto.Text + " <font color='" + ((p.DATA_BAIXA == null) ? vermelho : verde) + "'>" + valor + " - " + ((p.DATA_PAGAMENTO == null) ? "S/DATA" : Convert.ToDateTime(p.DATA_PAGAMENTO).ToString("dd/MM/yyyy")) + "<br />" + ((p.DATA_BAIXA == null) ? "Pendente" : "Pago") + "</font> ";
                litpgto.Text = litpgto.Text + " <br /> ";

                i = i + 1;
            }

            for (var ii = i; ii <= 6; ii++)
            {
                litpgto = row.FindControl("litParc" + ii.ToString()) as Literal;
                if (litpgto != null)
                    litpgto.Visible = false;
            }

        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarPedidos();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

    }

}

