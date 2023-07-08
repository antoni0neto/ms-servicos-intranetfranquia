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
    public partial class desenv_pedido_aviamento_hb_saq : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        const string MATERIAL_PEDCOMPRA_CON_RESERVA = "MATERIAL_PEDCOMPRA_CON_RESERVA";

        decimal qtdeTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarGrupos();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"

        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "" });
                ddlMaterialGrupo.DataSource = _matGrupo;
                ddlMaterialGrupo.DataBind();
            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }
        }
        private void CarregarSubGrupos(string grupo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = null;
            if (grupo.Trim() != "")
                _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();
            else
                _matSubGrupo = desenvController.ObterMaterialSubGrupo();

            if (_matSubGrupo != null)
            {
                _matSubGrupo = _matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });

                ddlMaterialSubGrupo.DataSource = _matSubGrupo;
                ddlMaterialSubGrupo.DataBind();
            }
        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim());
        }
        private void CarregarCores(string grupo, string subGrupo)
        {
            var materiais = desenvController.ObterMaterialCorV2(grupo, subGrupo, "");
            var coresBasicas = prodController.ObterCoresBasicas();

            coresBasicas = coresBasicas.Where(p => materiais.Any(x => x.COR_MATERIAL.Trim() == p.COR.Trim())).ToList();
            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

            ddlCor.DataSource = coresBasicas;
            ddlCor.DataBind();

            if (coresBasicas.Count == 2)
                ddlCor.SelectedIndex = 1;

        }

        #endregion

        #region "MATERIAL"
        private void CarregarMaterialPedidoReserva(string pedidoIntranet, string pedidoOrigem, string produto, int hb, string grupoMaterial, string subGrupoMaterial, string corMaterial)
        {
            var materialPedido = ObterMaterialPedidoReserva(pedidoIntranet, pedidoOrigem, produto, hb, grupoMaterial, subGrupoMaterial, corMaterial);

            gvMaterialPedido.DataSource = materialPedido;
            gvMaterialPedido.DataBind();

            Session[MATERIAL_PEDCOMPRA_CON_RESERVA] = materialPedido;
        }
        protected void gvMaterialPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_PEDIDO_RESERVADOResult matPedido = e.Row.DataItem as SP_OBTER_MATERIAL_PEDIDO_RESERVADOResult;

                    if (matPedido != null)
                    {
                        Button btnBaixar = e.Row.FindControl("btnBaixar") as Button;
                        btnBaixar.CommandArgument = matPedido.CODIGO_DESENV_MATERIAL_PEDIDO.ToString();

                        TextBox txtQtdeRetirada = e.Row.FindControl("txtQtdeRetirada") as TextBox;
                        var qtde = 0M;
                        if (matPedido.QTDE_RETIRADA == null)
                        {
                            if (matPedido.QTDE_UTILIZADA <= matPedido.QTDE_RESERVADA)
                            {
                                qtde = Convert.ToDecimal(matPedido.QTDE_UTILIZADA);
                            }
                            else
                            {
                                qtde = Convert.ToDecimal(matPedido.QTDE_RESERVADA);
                            }
                        }
                        else
                        {
                            qtde = Convert.ToDecimal(matPedido.QTDE_RETIRADA);
                        }

                        txtQtdeRetirada.Text = qtde.ToString();

                        if (matPedido.DATA_BAIXA != null)
                            btnBaixar.Enabled = false;

                        if (matPedido.QTDE_VAREJO <= 0 || matPedido.STATUS == 'E')
                            e.Row.BackColor = Color.LightSalmon;

                    }
                }
            }
        }
        protected void gvMaterialPedido_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[MATERIAL_PEDCOMPRA_CON_RESERVA] != null)
            {
                IEnumerable<SP_OBTER_MATERIAL_PEDIDO_RESERVADOResult> materialPedido = (IEnumerable<SP_OBTER_MATERIAL_PEDIDO_RESERVADOResult>)Session[MATERIAL_PEDCOMPRA_CON_RESERVA];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvMaterialPedido, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvMaterialPedido, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvMaterialPedido, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                materialPedido = materialPedido.OrderBy(e.SortExpression + sortDirection);
                gvMaterialPedido.DataSource = materialPedido;
                gvMaterialPedido.DataBind();
            }
        }

        #endregion

        private List<SP_OBTER_MATERIAL_PEDIDO_RESERVADOResult> ObterMaterialPedidoReserva(string pedidoIntranet, string pedidoOrigem, string produto, int hb, string grupoMaterial, string subGrupoMaterial, string corMaterial)
        {
            var materialPedido = desenvController.ObterMaterialPedidoReserva(pedidoIntranet, pedidoOrigem, produto, hb, grupoMaterial, subGrupoMaterial, corMaterial, 0);

            if (ddlSeparado.SelectedValue != "")
            {
                if (ddlSeparado.SelectedValue == "S")
                    materialPedido = materialPedido.Where(p => p.DATA_BAIXA != null).ToList();
                else
                    materialPedido = materialPedido.Where(p => p.DATA_BAIXA == null).ToList();
            }

            if (ddlCancelado.SelectedValue != "")
            {
                if (ddlCancelado.SelectedValue == "S")
                    materialPedido = materialPedido.Where(p => p.QTDE_VAREJO <= 0 || p.STATUS == 'E').ToList();
                else if (ddlCancelado.SelectedValue == "N")
                    materialPedido = materialPedido.Where(p => p.QTDE_VAREJO > 0 && p.STATUS == 'A').ToList();
            }

            return materialPedido;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                int hb = 0;
                if (txtHB.Text != "")
                    hb = Convert.ToInt32(txtHB.Text.Trim());

                var subGrupoMaterial = (ddlMaterialSubGrupo.SelectedItem != null) ? ddlMaterialSubGrupo.SelectedItem.Text.Trim() : "";
                var corMaterial = (ddlCor.SelectedItem != null) ? ddlCor.SelectedValue.Trim() : "";

                CarregarMaterialPedidoReserva(txtPedidoIntranet.Text.Trim().ToUpper(), ddlPedidoOrigem.SelectedValue, txtProduto.Text.Trim(), hb, ddlMaterialGrupo.SelectedItem.Text.Trim(), subGrupoMaterial, corMaterial);
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        protected void btnBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnBaixar = (Button)sender;

                string codigoMatPedido = btnBaixar.CommandArgument;
                //atualizar pedido

                GridViewRow row = (GridViewRow)btnBaixar.NamingContainer;
                var qtde = ((TextBox)row.FindControl("txtQtdeRetirada")).Text.ToString().Trim();
                var qtdeConsumida = ((Literal)row.FindControl("litQtdeConsumida")).Text.ToString().Trim();
                if (qtde != "")
                {
                    var matPedido = desenvController.ObterMaterialPedidoExp(Convert.ToInt32(codigoMatPedido));
                    matPedido.QTDE_RETIRADA = Convert.ToDecimal(qtde);

                    if (matPedido.QTDE_RETIRADA > matPedido.QTDE)
                    {
                        labMsg.Text = "A quantidade de retirada não pode ser maior que a quantidade reservada.";
                        return;
                    }

                    matPedido.QTDE_SOBRA = (matPedido.QTDE - matPedido.QTDE_RETIRADA);
                    matPedido.DATA_BAIXA = DateTime.Now;
                    desenvController.AtualizarMaterialPedidoExp(matPedido);

                    //obter dados do material e produto
                    var materialPedido = desenvController.ObterMaterialPedidoReserva("", "", "", 0, "", "", "", matPedido.CODIGO).SingleOrDefault();


                    var qtdeSobra = (matPedido.QTDE - Convert.ToDecimal(matPedido.QTDE_RETIRADA));
                    // se sobra positiva vai para o estoque
                    if (qtdeSobra > 0)
                    {

                        //inserir sobra no estoque
                        var matEstoque = new DESENV_MATERIAL_ESTOQUE();
                        matEstoque.MATERIAL = materialPedido.MATERIAL;
                        matEstoque.COR_MATERIAL = materialPedido.COR_MATERIAL;
                        matEstoque.DATA_ENTRADA = DateTime.Now;
                        matEstoque.QTDE_ESTOQUE = qtdeSobra;
                        desenvController.InserirMaterialEstoque(matEstoque);
                    }

                    //////// NAO É MAIS UTILIZADO
                    //// inserir qtde faltante para "Sonete" realizar o pedido
                    //if (Convert.ToDecimal(qtdeConsumida) > matPedido.QTDE)
                    //{
                    //    var qtdeFaltante = (Convert.ToDecimal(qtdeConsumida) - matPedido.QTDE);
                    //    //inserir qtde faltante

                    //    var matPedidoFalta = new DESENV_MATERIAL_PEDIDO_FALTA();
                    //    matPedidoFalta.DESENV_PRODUTO_FICTEC = matPedido.DESENV_PRODUTO_FICTEC;
                    //    matPedidoFalta.QTDE_FALTANTE = qtdeFaltante;
                    //    matPedidoFalta.DATA_INCLUSAO = DateTime.Now;
                    //    desenvController.InserirMaterialPedidoFaltante(matPedidoFalta);
                    //}


                    btnBaixar.Enabled = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

