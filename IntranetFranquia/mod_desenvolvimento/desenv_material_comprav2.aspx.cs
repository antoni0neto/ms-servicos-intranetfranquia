using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios.mod_desenvolvimento
{
    public partial class desenv_material_comprav2 : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        decimal qtdePedidoIni = 0;
        decimal qtdePedidoEnt = 0;
        decimal qtdePedidoFalt = 0;

        decimal qtdeSubPedido = 0;
        decimal qtdeSubPedidoRecebida = 0;
        decimal valorSubPedido = 0;

        decimal qtdeTotal = 0;
        decimal valTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarGrupos();
                CarregarFornecedores();
                CarregarUnidadeMedida();

                btAbrirSubPedido.Visible = false;

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                CarregarCarrinho();
                CarregarCarrinhoReservado();

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);

            btSalvarPedido.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvarPedido, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupos()
        {
            var matGrupo = desenvController.ObterMaterialGrupo();
            if (matGrupo != null)
            {
                matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "" });
                ddlMaterialGrupo.DataSource = matGrupo;
                ddlMaterialGrupo.DataBind();

                ddlMaterialGrupoPedido.DataSource = matGrupo;
                ddlMaterialGrupoPedido.DataBind();
            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                DropDownList ddl = (DropDownList)sender;
                if (ddl.ID == "ddlMaterialGrupo")
                {
                    CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo);
                }
                else
                {
                    CarregarSubGrupos(ddlMaterialGrupoPedido.SelectedItem.Text.Trim(), ddlMaterialSubGrupoPedido);
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarSubGrupos(string grupo, DropDownList dllMatGrupo)
        {
            var matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();

            matSubGrupo = matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
            matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });

            dllMatGrupo.DataSource = matSubGrupo;
            dllMatGrupo.DataBind();
        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                DropDownList ddl = (DropDownList)sender;
                if (ddl.ID == "ddlMaterialSubGrupo")
                {
                    CarregarCores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim(), ddlCor);
                }
                else
                {
                    CarregarCores(ddlMaterialGrupoPedido.SelectedItem.Text.Trim(), ddlMaterialSubGrupoPedido.SelectedItem.Text.Trim(), ddlCorPedido);
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarCores(string grupo, string subGrupo, DropDownList ddlC)
        {
            var filtroMaterial = desenvController.ObterMaterial(grupo.Trim(), subGrupo.Trim());

            var materialCores = desenvController.ObterMaterialCor();
            materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).ToList();

            var coresBasicas = ObterCoresBasicas();

            //Depois filtrar cores para carregar os filtros
            coresBasicas = coresBasicas.Where(p => materialCores.Any(x => x.COR_MATERIAL.Trim() == p.COR.Trim())).ToList();
            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

            ddlC.DataSource = coresBasicas;
            ddlC.DataBind();

            if (coresBasicas.Count == 2)
            {
                ddlC.SelectedIndex = 1;
                ddlCor_SelectedIndexChanged(ddlC, null);
            }

        }
        protected void ddlCor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                DropDownList ddl = (DropDownList)sender;
                if (ddl.ID == "ddlCor")
                {
                    CarregarCorFornecedor(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim(), ddlCor.SelectedValue, ddlCorFornecedor);
                }
                else
                {
                    CarregarCorFornecedor(ddlMaterialGrupoPedido.SelectedItem.Text.Trim(), ddlMaterialSubGrupoPedido.SelectedItem.Text.Trim(), ddlCorPedido.SelectedValue, ddlCorFornecedorPedido);
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarCorFornecedor(string grupo, string subGrupo, string cor, DropDownList ddlCorForn)
        {
            var filtroMaterial = desenvController.ObterMaterial(grupo, subGrupo);
            var materialCores = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == cor.Trim()).ToList();

            materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).ToList();
            materialCores.Insert(0, new MATERIAIS_CORE { REFER_FABRICANTE = "" });

            ddlCorForn.DataSource = materialCores.OrderBy(p => p.REFER_FABRICANTE);
            ddlCorForn.DataBind();

            if (materialCores.Count == 2)
            {
                ddlCorForn.SelectedIndex = 1;
            }
        }
        private void CarregarUnidadeMedida()
        {
            var unidadeMedida = prodController.ObterUnidadeMedida();

            unidadeMedida.Add(new UNIDADE_MEDIDA { CODIGO = 0, DESCRICAO = "", STATUS = 'A' });

            unidadeMedida = unidadeMedida.OrderBy(l => l.CODIGO).ToList();

            if (unidadeMedida != null)
            {
                ddlUnidade.DataSource = unidadeMedida;
                ddlUnidade.DataBind();
            }
        }

        private void CarregarFornecedores()
        {
            var fornecedores = prodController.ObterFornecedor().Where(p => p.STATUS == 'A' && p.TIPO != 'S').ToList();
            if (fornecedores != null)
            {
                var fornecedoresAux = fornecedores.Where(p => p.FORNECEDOR != null).Select(s => s.FORNECEDOR.Trim()).Distinct().ToList();

                fornecedores = new List<PROD_FORNECEDOR>();
                foreach (var item in fornecedoresAux)
                    if (item.Trim() != "")
                        fornecedores.Add(new PROD_FORNECEDOR { FORNECEDOR = item.Trim() });

                fornecedores.OrderBy(p => p.FORNECEDOR);
                fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "" });

                ddlPedidoFornecedor.DataSource = fornecedores;
                ddlPedidoFornecedor.DataBind();

            }
        }

        private List<CORES_BASICA> ObterCoresBasicas()
        {
            var cores = prodController.ObterCoresBasicas();
            return cores;
        }
        #endregion

        #region "AÇÕES DA TELA"
        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        #endregion

        private int ObterUltimoPedido()
        {
            return desenvController.ObterUltimoPedido();
        }

        #region "PEDIDO"
        private List<DESENV_PEDIDO> ObterPedidos()
        {
            var pedidos = desenvController.ObterPedido1000(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim());

            var lstCores = new List<string>();

            if (ddlCor.SelectedValue.Trim() != "")
                lstCores.Add(ddlCor.SelectedValue.Trim());
            var corFornecedor = ddlCorFornecedor.SelectedValue.Trim();

            if (lstCores.Count() > 0)
                pedidos = pedidos.Where(p => p.COR != null && lstCores.Contains(p.COR.Trim())).ToList();

            if (corFornecedor != "")
                pedidos = pedidos.Where(p => p.COR_FORNECEDOR != null && p.COR_FORNECEDOR.Trim().Contains(corFornecedor)).ToList();

            return pedidos;
        }
        private void CarregarPedidos(List<DESENV_PEDIDO> pedidos)
        {
            if (pedidos == null || pedidos.Count <= 0)
                pedidos = ObterPedidos();

            if (pedidos == null || pedidos.Count <= 0)
                pedidos.Add(new DESENV_PEDIDO { CODIGO = 0, NUMERO_PEDIDO = 0 });

            gvPedidos.DataSource = pedidos;
            gvPedidos.DataBind();
        }
        protected void gvPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO pedido = e.Row.DataItem as DESENV_PEDIDO;

                    if (pedido != null)
                    {
                        ImageButton btEditarPedido = e.Row.FindControl("btEditarPedido") as ImageButton;
                        ImageButton btExcluirPedido = e.Row.FindControl("btExcluirPedido") as ImageButton;

                        if (pedido.CODIGO > 0)
                        {
                            Label labNumeroPedido = e.Row.FindControl("labNumeroPedido") as Label;
                            labNumeroPedido.Text = pedido.NUMERO_PEDIDO.ToString();

                            Label labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                            labFornecedor.Text = pedido.FORNECEDOR;

                            Label labMaterial = e.Row.FindControl("labMaterial") as Label;
                            var materiais = desenvController.ObterMaterial(pedido.GRUPO.Trim(), pedido.SUBGRUPO.Trim());
                            if (materiais != null)
                            {
                                var materialCor = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == pedido.COR.Trim() && p.REFER_FABRICANTE.Trim() == pedido.COR_FORNECEDOR.Trim() && materiais.Any(x => x.MATERIAL == p.MATERIAL)).FirstOrDefault();
                                if (materialCor != null)
                                    labMaterial.Text = materialCor.MATERIAL.Trim();
                            }

                            Label labGrupoMaterial = e.Row.FindControl("labGrupoMaterial") as Label;
                            labGrupoMaterial.Text = pedido.GRUPO.Trim();

                            Label labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                            labSubGrupo.Text = pedido.SUBGRUPO;


                            Label labCor = e.Row.FindControl("labCor") as Label;
                            labCor.Text = pedido.COR + " - " + prodController.ObterCoresBasicas(pedido.COR).DESC_COR.Trim();

                            Label labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                            labCorFornecedor.Text = pedido.COR_FORNECEDOR;

                            Label labQtde = e.Row.FindControl("labQtde") as Label;
                            labQtde.Text = pedido.QTDE.ToString("###,###,###,##0.000");


                            Label labQtdeEntregue = e.Row.FindControl("labQtdeEntregue") as Label;
                            var totEntregue = 0.00M;
                            var qtdeEntregue = desenvController.ObterPedidoQtdePedido(pedido.CODIGO);
                            if (qtdeEntregue != null && qtdeEntregue.Count() > 0)
                                totEntregue = qtdeEntregue.Sum(p => p.QTDE);
                            labQtdeEntregue.Text = totEntregue.ToString("###,###,##0.000");

                            Label labQtdeFalt = e.Row.FindControl("labQtdeFalt") as Label;
                            labQtdeFalt.Text = (pedido.QTDE - totEntregue).ToString();


                            btEditarPedido.Visible = true;

                            qtdePedidoIni += pedido.QTDE;
                            qtdePedidoEnt += totEntregue;
                            qtdePedidoFalt += (pedido.QTDE - totEntregue);

                        }
                    }
                }
            }
        }
        protected void gvPedidos_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPedidos.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[9].Text = qtdePedidoIni.ToString();
                footer.Cells[10].Text = qtdePedidoEnt.ToString();
                footer.Cells[11].Text = qtdePedidoFalt.ToString();
            }
        }
        protected void btIncluirPedido_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {

                        btAbrirSubPedido.Visible = false;
                        CancelarPedido();
                        int numeroPedido = desenvController.ObterUltimoPedido();
                        txtPedidoNumero.Text = numeroPedido.ToString();
                        MoverAba("1");
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btIncluirPedido_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btEditarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    GridViewRow row = (GridViewRow)bt.NamingContainer;
                    if (row != null)
                    {
                        GridView gv = gvPedidos;
                        var codigoPedido = Convert.ToInt32(gv.DataKeys[row.RowIndex].Value.ToString());
                        EditarPedido(codigoPedido);
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btEditarPedido_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        private void EditarPedido(int codigoPedido)
        {
            hidCodigoPedido.Value = codigoPedido.ToString();

            btAbrirSubPedido.Visible = false;

            var pedido = desenvController.ObterPedido(codigoPedido);
            if (pedido != null)
            {

                //controle de edicao do pedido
                var podeEditar = true;
                //validar sub pedido
                var pedidoSub = desenvController.ObterPedidoSubPorDesenvPedido(pedido.CODIGO);
                if (pedidoSub != null && pedidoSub.Count() > 0)
                {
                    podeEditar = false;
                }
                else
                {
                    //validar entregas
                    var qtdePedido = desenvController.ObterPedidoQtdePedido(pedido.CODIGO);
                    if (qtdePedido != null && qtdePedido.Count() > 0)
                        podeEditar = false;
                }

                txtPedidoNumero.Text = pedido.NUMERO_PEDIDO.ToString();

                var g = desenvController.ObterMaterialGrupo(pedido.GRUPO);
                if (g != null)
                {
                    ddlMaterialGrupoPedido.SelectedValue = g.CODIGO_GRUPO;
                    ddlMaterialGrupo_SelectedIndexChanged(ddlMaterialGrupoPedido, null);
                }

                var s = desenvController.ObterMaterialSubGrupo(pedido.GRUPO, pedido.SUBGRUPO);
                if (s != null)
                    ddlMaterialSubGrupoPedido.SelectedValue = s.CODIGO_SUBGRUPO;

                CarregarCores(ddlMaterialGrupoPedido.SelectedItem.Text.Trim(), ddlMaterialSubGrupoPedido.SelectedItem.Text.Trim(), ddlCorPedido);
                var c = prodController.ObterCoresBasicas(pedido.COR);
                if (c != null)
                    ddlCorPedido.SelectedValue = c.COR;

                CarregarCorFornecedor(pedido.GRUPO, pedido.SUBGRUPO, pedido.COR, ddlCorFornecedorPedido);
                var r = desenvController.ObterMaterialCorV2(pedido.GRUPO, pedido.SUBGRUPO, "").Where(p => p.COR_MATERIAL.Trim() == pedido.COR.Trim() && p.REFER_FABRICANTE.Trim() == pedido.COR_FORNECEDOR.Trim()).FirstOrDefault();
                if (r != null)
                    ddlCorFornecedorPedido.SelectedValue = r.REFER_FABRICANTE;

                var f = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == pedido.FORNECEDOR.Trim());
                if (f != null && f.Count() > 0)
                {
                    ddlPedidoFornecedor.SelectedValue = f.Take(1).SingleOrDefault().FORNECEDOR.Trim();
                    ddlPedidoFornecedor_SelectedIndexChanged(ddlPedidoFornecedor, null);
                }

                ddlUnidade.SelectedValue = (pedido.UNIDADE_MEDIDA == null) ? "0" : pedido.UNIDADE_MEDIDA.ToString();

                CarregarSubPedidos(pedido.CODIGO);

                HabilitarCampos(podeEditar);
            }

            btAbrirSubPedido.Visible = true;

            MoverAba("1");
        }

        protected void btExcluirPedido_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {

                        GridView gv = null;
                        if (bt.ID == "btExcluirPedido")
                            gv = gvPedidos;

                        int codigoPedido = 0;
                        codigoPedido = Convert.ToInt32(gv.DataKeys[row.RowIndex].Value.ToString());

                        DESENV_PEDIDO pedido = desenvController.ObterPedido(codigoPedido);
                        if (pedido != null)
                        {
                            //VALIDAR EXCLUSAO DE PEDIDO
                            //VERIFICA SE JA TEM QTDE ENTREGUE
                            var listaPedidoQtde = desenvController.ObterPedidoQtdePedido(pedido.CODIGO);
                            if (listaPedidoQtde != null && listaPedidoQtde.Count() > 0)
                            {
                                labErro.Text = "Não será possível excluir este Pedido. Pedido tem entrega realizada.";
                                return;
                            }

                            var listaPedidoSub = desenvController.ObterPedidoSubPorDesenvPedido(pedido.CODIGO);
                            if (listaPedidoSub != null && listaPedidoSub.Count() > 0)
                            {
                                labErro.Text = "Não será possível excluir este Pedido. Pedido tem sub pedido realizado.";
                                return;
                            }

                            desenvController.ExcluirPedido(pedido);

                            btPesquisarPedido_Click(null, null);
                        }

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btExcluirPedido_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btSalvarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho.";
                    return;
                }

                //validar se é pedido novo, se sim, nao pode incluir pedido do mesmo material

                if (hidCodigoPedido.Value == "" || hidCodigoPedido.Value == "0")
                {
                    var pedidoMaterialExiste = desenvController.ObterPedido1000(ddlMaterialGrupoPedido.SelectedItem.Text.Trim(), ddlMaterialSubGrupoPedido.SelectedItem.Text.Trim())
                        .Where(p => p.COR.Trim() == ddlCorPedido.SelectedValue.Trim() && p.COR_FORNECEDOR.Trim() == ddlCorFornecedorPedido.SelectedValue.Trim()).FirstOrDefault();
                    if (pedidoMaterialExiste != null)
                    {
                        labErro.Text = "O Material informado já possui o Pedido " + pedidoMaterialExiste.NUMERO_PEDIDO.ToString() + ".";
                        return;
                    }
                }

                var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                var pedido = new DESENV_PEDIDO();
                pedido.NUMERO_PEDIDO = Convert.ToInt32(txtPedidoNumero.Text);
                pedido.COLECAO = "";
                pedido.FORNECEDOR = ddlPedidoFornecedor.SelectedValue;
                pedido.GRUPO = ddlMaterialGrupoPedido.SelectedItem.Text.Trim();
                pedido.SUBGRUPO = ddlMaterialSubGrupoPedido.SelectedItem.Text.Trim();
                pedido.STATUS = 'A';
                pedido.COR = ddlCorPedido.SelectedValue;
                pedido.COR_FORNECEDOR = ddlCorFornecedorPedido.SelectedValue.Trim().ToUpper();
                pedido.QTDE = Convert.ToDecimal(txtQtdeTotal.Text);

                pedido.VALOR = 0;
                pedido.CONDICAO_PGTO = "";
                pedido.CONDICAO_PGTO_OUTRO = "";

                pedido.DATA_PEDIDO = DateTime.Now.Date;

                pedido.USUARIO_INCLUSAO = codigoUsuario;
                pedido.DATA_INCLUSAO = DateTime.Now;
                pedido.UNIDADE_MEDIDA = Convert.ToInt32(ddlUnidade.SelectedValue);

                if (hidCodigoPedido.Value == "" || hidCodigoPedido.Value == "0")
                {
                    int codigoPedido = 0;
                    codigoPedido = desenvController.InserirPedido(pedido);

                    hidCodigoPedido.Value = codigoPedido.ToString();
                    btAbrirSubPedido_Click(btAbrirSubPedido, null);
                }
                else
                {
                    pedido.CODIGO = Convert.ToInt32(hidCodigoPedido.Value);
                    desenvController.AtualizarPedido(pedido);
                    //Obter pedido atualizado
                    pedido = desenvController.ObterPedido(Convert.ToInt32(hidCodigoPedido.Value));
                }

                labErro.Text = "Pedido cadastrado com sucesso.";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CancelarPedido()
        {

            System.Drawing.Color _OK = System.Drawing.Color.Gray;

            labNumeroPedido.ForeColor = _OK;
            labFornecedorPedido.ForeColor = _OK;
            labMaterialGrupoPedido.ForeColor = _OK;
            labMaterialSubGrupoPedido.ForeColor = _OK;
            labCorPedido.ForeColor = _OK;
            labCorFornecedorPedido.ForeColor = _OK;
            labQtdeTotal.ForeColor = _OK;
            labQtdeEntregue.ForeColor = _OK;
            labUnidadeMedida.ForeColor = _OK;
            LimparCampos(false);
            HabilitarCampos(true);
        }
        protected void ibtPedidoNovo_Click(object sender, EventArgs e)
        {
            try
            {
                btAbrirSubPedido.Visible = false;
                hidCodigoPedido.Value = "";
                labErro.Text = "";
                CancelarPedido();
                int numeroPedido = desenvController.ObterUltimoPedido();
                txtPedidoNumero.Text = numeroPedido.ToString();
                MoverAba("1");

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void txtPedidoNumero_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt != null && txt.Text.Trim() != "")
                ValidarNumeroPedido(Convert.ToInt32(txt.Text));
        }
        protected void ddlPedidoFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void btPesquisarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (txtPedidoFiltro.Text.Trim() != "" && Convert.ToInt32(txtPedidoFiltro.Text.Trim()) < 1000)
                {
                    labErro.Text = "Número do pedido deve ser acima de 1000.";
                    return;
                }

                var pedidos = new List<DESENV_PEDIDO>();
                if (txtPedidoFiltro.Text.Trim() != "")
                {
                    var pedido = desenvController.ObterPedidoNumero(Convert.ToInt32(txtPedidoFiltro.Text));
                    if (pedido != null)
                    {
                        pedidos.Add(pedido);
                        EditarPedido(pedido.CODIGO);
                    }
                    else
                    {
                        gvPedidos.DataSource = pedidos;
                        gvPedidos.DataBind();

                        labErro.Text = "Pedido não foi encontrado. Refaça sua pesquisa.";
                        return;
                    }
                }
                else
                {
                    if (ddlMaterialGrupo.SelectedValue.Trim() == "")
                    {
                        labErro.Text = "Selecion o Grupo do Tecido.";
                        return;
                    }
                    if (ddlMaterialSubGrupo.SelectedValue.Trim() == "")
                    {
                        labErro.Text = "Selecion o SubGrupo do Tecido.";
                        return;
                    }
                }

                CarregarPedidos(pedidos);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btAbrirSubPedido_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";

                    if (hidCodigoPedido.Value == "" || hidCodigoPedido.Value == "0")
                    {
                        labErro.Text = "Pedido não existe.";
                        return;
                    }

                    string numeroPedido = txtPedidoNumero.Text;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('desenv_material_compra_subpedidov2.aspx?p=" + numeroPedido + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);

                    MoverAba("1");
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }

        }
        #endregion

        #region "SUBPEDIDO"
        private void CarregarSubPedidos(int codigoDesenvPedido)
        {
            var subPedido = desenvController.ObterPedidoSubPorDesenvPedido(codigoDesenvPedido).OrderBy(p => p.CODIGO);

            gvSubPedido.DataSource = subPedido;
            gvSubPedido.DataBind();
        }
        protected void gvSubPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO_SUB subPedido = e.Row.DataItem as DESENV_PEDIDO_SUB;

                    Label labNumeroSubPedido = e.Row.FindControl("labNumeroSubPedido") as Label;
                    labNumeroSubPedido.Text = subPedido.DESENV_PEDIDO1.NUMERO_PEDIDO.ToString() + " - " + ((subPedido.ITEM == null) ? "X" : subPedido.ITEM.ToString());

                    Label labDataPedido = e.Row.FindControl("labDataPedido") as Label;
                    labDataPedido.Text = Convert.ToDateTime(subPedido.DATA_PEDIDO).ToString("dd/MM/yyyy");

                    Label labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                    labFornecedor.Text = subPedido.FORNECEDOR;

                    Label labColecao = e.Row.FindControl("labColecao") as Label;
                    labColecao.Text = (subPedido.COLECAO == null) ? "-" : (baseController.BuscaColecaoAtual(subPedido.COLECAO).DESC_COLECAO.Trim());

                    Label labQtde = e.Row.FindControl("labQtde") as Label;
                    labQtde.Text = subPedido.QTDE.ToString();

                    Label labCusto = e.Row.FindControl("labCusto") as Label;
                    labCusto.Text = subPedido.VALOR.ToString();

                    Label labValorTotal = e.Row.FindControl("labValorTotal") as Label;
                    labValorTotal.Text = "R$ " + Convert.ToDecimal(subPedido.QTDE * subPedido.VALOR).ToString("###,###,###,##0.00");

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

                    ImageButton btEditarPedidoSub = e.Row.FindControl("btEditarPedidoSub") as ImageButton;
                    btEditarPedidoSub.CommandArgument = subPedido.CODIGO.ToString();

                    ImageButton btEntrarQtde = e.Row.FindControl("btEntrarQtde") as ImageButton;
                    btEntrarQtde.CommandArgument = subPedido.CODIGO.ToString();

                    ImageButton btExcluirSubPedido = e.Row.FindControl("btExcluirSubPedido") as ImageButton;
                    btExcluirSubPedido.CommandArgument = subPedido.CODIGO.ToString();

                    ImageButton btBaixarSubPedido = e.Row.FindControl("btBaixarSubPedido") as ImageButton;
                    btBaixarSubPedido.CommandArgument = subPedido.CODIGO.ToString();

                    if (subPedido.DATA_BAIXA == null && (subPedido.DATA_ENTREGA_PREV < DateTime.Now.Date))
                        e.Row.BackColor = Color.LightGoldenrodYellow;

                    if (subPedido.DATA_BAIXA == null && (subPedido.DATA_ENTREGA_PREV < DateTime.Now.Date && subPedido.DATA_ENTREGA_PREV != ((subPedido.DATA_PREVISAO_FINAL == null) ? subPedido.DATA_ENTREGA_PREV : subPedido.DATA_PREVISAO_FINAL)))
                        e.Row.BackColor = Color.Pink;

                    if (subPedido.DATA_BAIXA != null)
                    {
                        btEditarPedidoSub.Visible = false;
                        btExcluirSubPedido.Visible = false;
                        btBaixarSubPedido.Visible = false;
                    }

                    qtdeSubPedido += Convert.ToDecimal(subPedido.QTDE);
                    valorSubPedido += (Convert.ToDecimal(subPedido.QTDE) * Convert.ToDecimal(subPedido.VALOR));

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

                footer.Cells[5].Text = qtdeSubPedido.ToString();
                footer.Cells[7].Text = "R$ " + valorSubPedido.ToString("###,###,###,##0.00");
                footer.Cells[10].Text = qtdeSubPedidoRecebida.ToString();

                txtQtdeTotal.Text = qtdeSubPedido.ToString();
                txtQtdeEntregue.Text = qtdeSubPedidoRecebida.ToString();
            }
        }
        protected void btAtualizarSubPedido_Click(object sender, EventArgs e)
        {
            if (hidCodigoPedido.Value != "")
            {
                CarregarSubPedidos(Convert.ToInt32(hidCodigoPedido.Value));
            }

            MoverAba("1");
        }
        protected void btEditarPedidoSub_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton b = (ImageButton)sender;
                if (b != null)
                {
                    labErro.Text = "";

                    string codigoSubPedido = b.CommandArgument;
                    string numeroPedido = txtPedidoNumero.Text;

                    //Abrir pop-up
                    var url = "fnAbrirTelaCadastroMaior('desenv_material_compra_subpedidov2.aspx?p=" + numeroPedido + "&sub=" + codigoSubPedido + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + ex.Message + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }
        }
        protected void btEntrarQtde_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    string codigoSubPedido = b.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('desenv_material_compra_entradav2.aspx?sub=" + codigoSubPedido + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);

                    MoverAba("1");
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btExcluirSubPedido_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton bt = (ImageButton)sender;

                string codigoSubPedido = bt.CommandArgument;

                var subPedido = desenvController.ObterPedidoSub(Convert.ToInt32(codigoSubPedido));
                if (subPedido == null)
                {
                    labErro.Text = "SubPedido não foi encontrado.";
                    return;
                }

                var pedido = desenvController.ObterPedido(subPedido.DESENV_PEDIDO);
                if (pedido == null)
                {
                    labErro.Text = "Pedido principal não foi encontrado.";
                    return;
                }

                // validar se tem parcelas pagas
                var parcelas = desenvController.ObterPedidoSubPgtoPorDesenvPedidoSub(subPedido.CODIGO);
                if (parcelas != null && parcelas.Count() > 0)
                {

                    var parcelasPagas = parcelas.Where(p => p.DATA_BAIXA != null);
                    if (parcelasPagas != null && parcelasPagas.Count() > 0)
                    {
                        labErro.Text = "Este Pedido possui parcelas pagas. Não será possível excluir este pedido.";
                        return;
                    }
                }


                pedido.QTDE = pedido.QTDE - Convert.ToDecimal(subPedido.QTDE);

                // excluir as parcelas
                foreach (var par in parcelas)
                    desenvController.ExcluirPedidoSubPgto(par.CODIGO);
                // excluir sub pedido
                desenvController.ExcluirPedidoSub(Convert.ToInt32(codigoSubPedido));
                //atualizar qtde no cabecalho
                desenvController.AtualizarPedido(pedido);

                txtQtdeTotal.Text = pedido.QTDE.ToString();

                CarregarSubPedidos(Convert.ToInt32(hidCodigoPedido.Value));

                MoverAba("1");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btBaixarSubPedido_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;

                string codigoSubPedido = bt.CommandArgument;

                var subPedido = desenvController.ObterPedidoSub(Convert.ToInt32(codigoSubPedido));
                if (subPedido == null)
                {
                    labErro.Text = "SubPedido não foi encontrado.";
                    return;
                }

                subPedido.STATUS = 'B';
                subPedido.DATA_BAIXA = DateTime.Now;

                desenvController.AtualizarPedidoSub(subPedido);

                CarregarSubPedidos(Convert.ToInt32(hidCodigoPedido.Value));

                MoverAba("1");
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void CriarPagto(DESENV_PEDIDO_SUB pedidoSub, GridViewRow row)
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

        #region "EMAIL"
        //private void EnviarEmail(DESENV_PEDIDO _pedido)
        //{
        //    string assuntoEmail = "";

        //    assuntoEmail = "Handbook - Pedido de Compra: " + _pedido.NUMERO_PEDIDO.ToString().Trim() + " - " + _pedido.SUBGRUPO + " ";
        //    assuntoEmail = assuntoEmail + _pedido.COR_FORNECEDOR;

        //    email_envio email = new email_envio();
        //    email.ASSUNTO = assuntoEmail;
        //    email.REMETENTE = (USUARIO)Session["USUARIO"];
        //    email.MENSAGEM = MontarCorpoEmail(_pedido, txtCorpoEmail.Text);

        //    List<string> destinatario = new List<string>();
        //    var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(3, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
        //    foreach (var usu in usuarioEmail)
        //        if (usu != null)
        //            destinatario.Add(usu.EMAIL);

        //    if (txtEmail.Text.Trim() != "")
        //    {
        //        string[] sEmail = txtEmail.Text.Trim().Split(';');
        //        if (sEmail != null && sEmail.Count() > 0)
        //            foreach (string s in sEmail)
        //                destinatario.Add(s);
        //    }

        //    email.DESTINATARIOS = destinatario;

        //    if (destinatario.Count > 0)
        //        email.EnviarEmail();
        //}
        //private string MontarCorpoEmail(DESENV_PEDIDO _pedido, string corpoEmail)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("");

        //    sb.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
        //    sb.Append("<html>");
        //    sb.Append("<head>");
        //    sb.Append("    <title>COMPRA DE TECIDOS</title>");
        //    sb.Append("    <meta charset='UTF-8'>");
        //    sb.Append("</head>");
        //    sb.Append("<body>");
        //    sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
        //    sb.Append("        background: white; white-space: nowrap;'>");
        //    sb.Append("        <span style='font-family:Calibri; font-size:medium;'>Compra de Tecidos</span>");
        //    sb.Append("        <br />");
        //    sb.Append("        <br />");
        //    sb.Append("        <div id='divMaterial' align='left'>");
        //    sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 1000pt; padding: 0px;");
        //    sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
        //    sb.Append("                background: white; white-space: nowrap;'>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td style='line-height: 20px;'>");
        //    sb.Append("                        <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        //    sb.Append("                            width: 750pt'>");
        //    sb.Append("                            <tr style='text-align: left;'>");
        //    sb.Append("                                <td style='width: 180px; font-family:Calibri;'>");
        //    sb.Append("                                    Número:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td style='font-family:Calibri;'>");
        //    sb.Append("                                    " + _pedido.NUMERO_PEDIDO);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Fornecedor:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td style='font-family:Calibri;'>");
        //    sb.Append("                                    " + _pedido.FORNECEDOR);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Material:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td style='font-family:Calibri;'>");
        //    sb.Append("                                    " + _pedido.SUBGRUPO);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Cor Fornecedor:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td style='font-family:Calibri;'>");
        //    sb.Append("                                    " + _pedido.COR_FORNECEDOR);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Quantidade:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td style='font-family:Calibri;'>");
        //    sb.Append("                                    " + _pedido.QTDE.ToString("###,###,###,##0.000") + " " + ((_pedido.UNIDADE_MEDIDA1 == null) ? "" : (_pedido.UNIDADE_MEDIDA1.DESCRICAO + "s")));
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Preço:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td style='font-family:Calibri;'>");
        //    sb.Append("                                    R$ " + _pedido.VALOR.ToString("###,###,###,##0.00"));
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Data do Pedido:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td style='font-family:Calibri;'>");
        //    sb.Append("                                    " + _pedido.DATA_PEDIDO.ToString("dd/MM/yyyy"));
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");

        //    List<FORMA_PGTO> _formaPGTO = (new BaseController().ObterFormaPgto());
        //    if (_formaPGTO != null && _formaPGTO.Count > 0)
        //    {
        //        var fp = _formaPGTO.Where(p => p.CONDICAO_PGTO.Trim() == _pedido.CONDICAO_PGTO.Trim()).SingleOrDefault();
        //        if (fp != null)
        //        {
        //            sb.Append("                            <tr>");
        //            sb.Append("                                <td>");
        //            sb.Append("                                    Condição de Pagamento:");
        //            sb.Append("                                </td>");
        //            sb.Append("                                <td style='font-family:Calibri;'>");
        //            sb.Append("                                    " + ((_pedido.CONDICAO_PGTO.Trim() == "##") ? "" : fp.DESC_COND_PGTO.Trim()) + " " + _pedido.CONDICAO_PGTO_OUTRO.Trim());
        //            sb.Append("                                </td>");
        //            sb.Append("                            </tr>");
        //        }
        //    }

        //    if (_pedido.DATA_ENTREGA_PREV != null)
        //    {
        //        sb.Append("                            <tr>");
        //        sb.Append("                                <td>");
        //        sb.Append("                                    Previsão de Entrega:");
        //        sb.Append("                                </td>");
        //        sb.Append("                                <td style='font-family:Calibri;'>");
        //        sb.Append("                                    " + Convert.ToDateTime(_pedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy"));
        //        sb.Append("                                </td>");
        //        sb.Append("                            </tr>");
        //    }
        //    if (_pedido.DATA_RESERVA != null)
        //    {
        //        sb.Append("                            <tr>");
        //        sb.Append("                                <td>");
        //        sb.Append("                                    Data de Reserva:");
        //        sb.Append("                                </td>");
        //        sb.Append("                                <td style='font-family:Calibri;'>");
        //        sb.Append("                                    " + Convert.ToDateTime(_pedido.DATA_RESERVA).ToString("dd/MM/yyyy"));
        //        sb.Append("                                </td>");
        //        sb.Append("                            </tr>");
        //    }
        //    sb.Append("                        </table>");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td>");
        //    sb.Append("                        &nbsp;");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td>");
        //    sb.Append("                        " + corpoEmail.Replace("\n", "<br>"));
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("            </table>");
        //    sb.Append("        </div>");
        //    sb.Append("        <br />");
        //    sb.Append("        <br />");
        //    sb.Append("        <span>Enviado por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
        //    sb.Append("    </div>");
        //    sb.Append("</body>");
        //    sb.Append("</html>");

        //    return sb.ToString();
        //}
        #endregion

        #region "VALIDACAO"
        private bool ValidarNumeroPedido(int numeroPedido)
        {
            labNumeroPedido.ForeColor = Color.Gray;
            labNumeroPedido.ToolTip = "";
            labErro.Text = "";
            if (desenvController.ObterPedidoNumero(numeroPedido) != null)
            {
                txtPedidoNumero.Text = "";
                txtPedidoNumero.Focus();
                labNumeroPedido.ForeColor = Color.Red;
                labNumeroPedido.ToolTip = "Número do Pedido já existe. Informe um número novo.";
                labErro.Text = "Número do Pedido já existe. Informe um número novo.";
                return false;
            }

            return true;
        }
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labNumeroPedido.ForeColor = _OK;
            if (txtPedidoNumero.Text.Trim() == "")
            {
                labNumeroPedido.ForeColor = _notOK;
                retorno = false;
            }

            labFornecedorPedido.ForeColor = _OK;
            if (ddlPedidoFornecedor.SelectedValue.Trim() == "")
            {
                labFornecedorPedido.ForeColor = _notOK;
                retorno = false;
            }

            labMaterialGrupoPedido.ForeColor = _OK;
            if (ddlMaterialGrupoPedido.SelectedValue.Trim() == "")
            {
                labMaterialGrupoPedido.ForeColor = _notOK;
                retorno = false;
            }

            labMaterialSubGrupoPedido.ForeColor = _OK;
            if (ddlMaterialSubGrupoPedido.SelectedValue.Trim() == "")
            {
                labMaterialSubGrupoPedido.ForeColor = _notOK;
                retorno = false;
            }

            labCorPedido.ForeColor = _OK;
            if (ddlCorPedido.SelectedValue.Trim() == "")
            {
                labCorPedido.ForeColor = _notOK;
                retorno = false;
            }

            labCorFornecedorPedido.ForeColor = _OK;
            if (ddlCorFornecedorPedido.SelectedValue.Trim() == "")
            {
                labCorFornecedorPedido.ForeColor = _notOK;
                retorno = false;
            }

            labQtdeTotal.ForeColor = _OK;
            decimal qtdeVal = 0;
            if (txtQtdeTotal.Text.Trim() == "" || !Decimal.TryParse(txtQtdeTotal.Text, out qtdeVal))
            {
                labQtdeTotal.ForeColor = _notOK;
                retorno = false;
            }

            labUnidadeMedida.ForeColor = _OK;
            if (ddlUnidade.SelectedValue.Trim() == "" || ddlUnidade.SelectedValue.Trim() == "0")
            {
                labUnidadeMedida.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private void HabilitarCampos(bool enable)
        {

            //PEDIDOS
            //txtPedidoNumero.Enabled = enable;
            ddlMaterialGrupoPedido.Enabled = enable;
            ddlMaterialSubGrupoPedido.Enabled = enable;
            ddlCorPedido.Enabled = enable;
            ddlCorFornecedorPedido.Enabled = enable;
            ddlPedidoFornecedor.Enabled = enable;
            ddlUnidade.Enabled = enable;
            btSalvarPedido.Visible = enable;

            foreach (GridViewRow row in gvPedidos.Rows)
                if (row != null)
                {
                    string status = "";
                    Label _labStatus = row.FindControl("labStatus") as Label;
                    if (_labStatus != null)
                        status = _labStatus.Text;


                    ImageButton _btEditarPedido = row.FindControl("btEditarPedido") as ImageButton;
                    if (_btEditarPedido != null)
                        _btEditarPedido.Visible = enable;
                    ImageButton _btExcluirPedido = row.FindControl("btExcluirPedido") as ImageButton;
                    if (_btExcluirPedido != null)
                    {
                        if (status.Trim() != "EXCLUÍDO")
                            _btExcluirPedido.Visible = enable;
                    }
                }

            GridViewRow footer = gvPedidos.FooterRow;
            if (footer != null)
            {
                ImageButton _btIncluirPedido = footer.FindControl("btIncluirPedido") as ImageButton;
                if (_btIncluirPedido != null)
                    _btIncluirPedido.Visible = enable;
            }

        }
        private void LimparCampos(bool _cabecalho)
        {
            labErro.Text = "";
            hidCodigoPedido.Value = "";

            txtPedidoNumero.Text = "";
            ddlMaterialGrupoPedido.SelectedValue = "";
            ddlMaterialSubGrupoPedido.SelectedValue = "";
            ddlCorPedido.SelectedValue = "";
            ddlCorFornecedorPedido.Text = "";
            ddlPedidoFornecedor.SelectedValue = "";
            txtQtdeTotal.Text = "0";
            txtQtdeEntregue.Text = "0";
            ddlUnidade.SelectedValue = "0";
            gvSubPedido.DataSource = new List<DESENV_PEDIDO_SUB>();
            gvSubPedido.DataBind();

            if (_cabecalho)
            {
                ddlMaterialGrupo.SelectedValue = "";
                ddlMaterialGrupo_SelectedIndexChanged(ddlMaterialGrupo, null);
                ddlMaterialSubGrupo.SelectedValue = "";
                ddlCor.SelectedValue = "";
                ddlCorFornecedor.SelectedValue = "";
            }

        }

        #endregion

        #region "PEDIDO EM LOTE"
        private void CarregarCarrinho()
        {
            var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var carrinho = desenvController.ObterCarrinhoPedidoPorUsuario(codigoUsuario);
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void btAtualizarCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                labPedido.Text = "";
                CarregarCarrinho();
            }
            catch (Exception)
            {
            }
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO_CARRINHO carrinho = e.Row.DataItem as DESENV_PEDIDO_CARRINHO;

                    if (carrinho != null)
                    {

                        Literal litPedido = e.Row.FindControl("litPedido") as Literal;
                        litPedido.Text = carrinho.DESENV_PEDIDO1.NUMERO_PEDIDO.ToString();

                        Literal litMaterial = e.Row.FindControl("litMaterial") as Literal;
                        var materiais = desenvController.ObterMaterial(carrinho.DESENV_PEDIDO1.GRUPO.Trim(), carrinho.DESENV_PEDIDO1.SUBGRUPO.Trim());
                        if (materiais != null)
                        {
                            var materialCor = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == carrinho.DESENV_PEDIDO1.COR.Trim() && p.REFER_FABRICANTE.Trim() == carrinho.DESENV_PEDIDO1.COR_FORNECEDOR.Trim() && materiais.Any(x => x.MATERIAL == p.MATERIAL)).FirstOrDefault();
                            if (materialCor != null)
                                litMaterial.Text = materialCor.MATERIAL.Trim();
                        }

                        Literal litTecido = e.Row.FindControl("litTecido") as Literal;
                        litTecido.Text = carrinho.DESENV_PEDIDO1.SUBGRUPO.Trim();

                        Literal litCorFornecedor = e.Row.FindControl("litCorFornecedor") as Literal;
                        litCorFornecedor.Text = carrinho.DESENV_PEDIDO1.COR_FORNECEDOR;

                        Literal litUnidadeMedida = e.Row.FindControl("litUnidadeMedida") as Literal;
                        litUnidadeMedida.Text = carrinho.DESENV_PEDIDO1.UNIDADE_MEDIDA1.DESCRICAO;

                        var qtde = (carrinho.QTDE != null) ? carrinho.QTDE : 0.00M;
                        TextBox txtQtde = e.Row.FindControl("txtQtde") as TextBox;
                        txtQtde.Text = qtde.ToString();

                        var preco = (carrinho.PRECO != null) ? carrinho.PRECO : 0.00M;
                        TextBox txtPreco = e.Row.FindControl("txtPreco") as TextBox;
                        txtPreco.Text = preco.ToString();

                        Literal litValTotal = e.Row.FindControl("litValTotal") as Literal;
                        litValTotal.Text = "R$ " + Convert.ToDecimal(qtde * preco).ToString("###,###,###,##0.00");

                        ImageButton btExcluirCarrinho = e.Row.FindControl("btExcluirItemCarrinho") as ImageButton;
                        if (btExcluirCarrinho != null)
                            btExcluirCarrinho.CommandArgument = carrinho.CODIGO.ToString();

                        if (carrinho.QTDE > 0 && carrinho.PRECO > 0)
                            e.Row.BackColor = Color.PaleGreen;

                        qtdeTotal += Convert.ToDecimal(qtde);
                        valTotal += Convert.ToDecimal(qtde * preco);

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvCarrinho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCarrinho.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[5].Text = qtdeTotal.ToString();
                footer.Cells[8].Text = "R$ " + valTotal.ToString("###,###,##0.00");
            }
        }
        protected void btExcluirItemCarrinho_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)(sender);
                var codigoCarrinho = Convert.ToInt32(bt.CommandArgument);
                desenvController.ExcluirCarrinhoPedidoPorCodigo(codigoCarrinho);
                CarregarCarrinho();
            }
            catch (Exception)
            {
            }
        }
        protected void btExcluirCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                desenvController.ExcluirCarrinhoPedidoPorUsuario(codigoUsuario);
                CarregarCarrinho();
            }
            catch (Exception)
            {
            }
        }
        protected void btGerarPedido_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    if (gvCarrinho.Rows.Count > 0)
                    {
                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('desenv_material_comprav2_gerar.aspx');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        protected void txtQtde_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labPedido.Text = "";

                TextBox txt = (TextBox)sender;

                GridViewRow row = (GridViewRow)txt.NamingContainer;
                int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex].Value);

                //atualizar grade do produto no carrinho
                decimal qtde = (txt.Text.Trim() == "") ? 0 : Convert.ToDecimal(txt.Text.Trim());

                var carrinho = desenvController.ObterCarrinhoPedido(codigoItemCarrinho);
                carrinho.QTDE = qtde;
                desenvController.AtualizarCarrinhoPedido(carrinho);

                CarregarCarrinho();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void txtPreco_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labPedido.Text = "";

                TextBox txt = (TextBox)sender;

                GridViewRow row = (GridViewRow)txt.NamingContainer;
                int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex].Value);

                //atualizar grade do produto no carrinho
                decimal preco = (txt.Text.Trim() == "") ? 0 : Convert.ToDecimal(txt.Text.Trim());

                var carrinho = desenvController.ObterCarrinhoPedido(codigoItemCarrinho);
                carrinho.PRECO = preco;
                desenvController.AtualizarCarrinhoPedido(carrinho);

                CarregarCarrinho();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btPesquisarLote_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (txtPedidoLote.Text.Trim() != "" && Convert.ToInt32(txtPedidoLote.Text.Trim()) < 1000)
                {
                    labErro.Text = "Número do pedido deve ser acima de 1000.";
                    return;
                }

                var pedidos = new List<DESENV_PEDIDO>();
                if (txtPedidoLote.Text.Trim() != "")
                {
                    var pedido = desenvController.ObterPedidoNumero(Convert.ToInt32(txtPedidoLote.Text));
                    if (pedido != null)
                    {
                        pedidos.Add(pedido);
                    }
                    else
                    {
                        gvPedidoLote.DataSource = pedidos;
                        gvPedidoLote.DataBind();

                        labErro.Text = "Pedido não foi encontrado. Refaça sua pesquisa.";
                        return;
                    }
                }
                else
                {
                    if (ddlMaterialGrupo.SelectedValue.Trim() == "")
                    {
                        labErro.Text = "Selecion o Grupo do Tecido.";
                        return;
                    }
                    if (ddlMaterialSubGrupo.SelectedValue.Trim() == "")
                    {
                        labErro.Text = "Selecion o SubGrupo do Tecido.";
                        return;
                    }
                }

                CarregarPedidosLote(pedidos);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void gvPedidoLote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO pedido = e.Row.DataItem as DESENV_PEDIDO;

                    if (pedido != null)
                    {
                        if (pedido.CODIGO > 0)
                        {
                            Label labNumeroPedido = e.Row.FindControl("labNumeroPedido") as Label;
                            labNumeroPedido.Text = pedido.NUMERO_PEDIDO.ToString();

                            Label labMaterial = e.Row.FindControl("labMaterial") as Label;
                            var materiais = desenvController.ObterMaterial(pedido.GRUPO.Trim(), pedido.SUBGRUPO.Trim());
                            if (materiais != null)
                            {
                                var materialCor = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == pedido.COR.Trim() && p.REFER_FABRICANTE.Trim() == pedido.COR_FORNECEDOR.Trim() && materiais.Any(x => x.MATERIAL == p.MATERIAL)).FirstOrDefault();
                                if (materialCor != null)
                                    labMaterial.Text = materialCor.MATERIAL.Trim();
                            }

                            Label labGrupoMaterial = e.Row.FindControl("labGrupoMaterial") as Label;
                            labGrupoMaterial.Text = pedido.GRUPO.Trim();

                            Label labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                            labSubGrupo.Text = pedido.SUBGRUPO;

                            Label labCor = e.Row.FindControl("labCor") as Label;
                            labCor.Text = pedido.COR + " - " + prodController.ObterCoresBasicas(pedido.COR).DESC_COR.Trim();

                            Label labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                            labCorFornecedor.Text = pedido.COR_FORNECEDOR;

                            Button btAdicionarCarrinho = e.Row.FindControl("btAdicionarCarrinho") as Button;
                            btAdicionarCarrinho.CommandArgument = pedido.CODIGO.ToString();
                        }
                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvPedidoLote_DataBound(object sender, EventArgs e)
        {
        }
        protected void btAdicionarCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labPedido.Text = "";

                if (Session["USUARIO"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                Button bt = (Button)(sender);
                GridViewRow row = (GridViewRow)bt.NamingContainer;

                int codigoPedido = Convert.ToInt32(bt.CommandArgument);
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                var pedidoNoCarrinho = desenvController.ObterCarrinhoPedido(codigoPedido, codigoUsuario);
                if (pedidoNoCarrinho != null)
                {
                    labPedido.Text = "Pedido já foi adicionado.";
                    return;
                }

                var carrinho = new DESENV_PEDIDO_CARRINHO
                {
                    DESENV_PEDIDO = codigoPedido,
                    USUARIO = codigoUsuario,
                    QTDE = 0,
                    PRECO = 0,
                    DATA_INCLUSAO = DateTime.Now
                };
                desenvController.InserirCarrinhoPedido(carrinho);

                row.BackColor = Color.PaleGreen;
                row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(row.BackColor) + "';this.style.cursor='hand'");

                CarregarCarrinho();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarPedidosLote(List<DESENV_PEDIDO> pedidos)
        {
            if (pedidos == null || pedidos.Count <= 0)
                pedidos = ObterPedidos();

            gvPedidoLote.DataSource = pedidos;
            gvPedidoLote.DataBind();
        }
        #endregion

        #region "PEDIDO EM LOTE RESERVADOS"
        protected void btPesquisarLoteReserva_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErroReserva.Text = "";
                labErro.Text = "";
                if (txtPedidoLoteReserva.Text.Trim() != "" && Convert.ToInt32(txtPedidoLoteReserva.Text.Trim()) < 1000)
                {
                    labErro.Text = "Número do pedido deve ser acima de 1000.";
                    return;
                }

                var pedidos = new List<SP_OBTER_PEDIDOSUB_RESERVADOResult>();
                if (txtPedidoLoteReserva.Text.Trim() != "")
                {
                    var pedidosSub = desenvController.ObterPedidoSubReservado(Convert.ToInt32(txtPedidoLoteReserva.Text), "", "", "", "");
                    if (pedidosSub != null && pedidosSub.Count() > 0)
                    {
                        pedidos.AddRange(pedidosSub);
                    }
                    else
                    {
                        gvPedidoLoteRes.DataSource = pedidos;
                        gvPedidoLoteRes.DataBind();

                        labErro.Text = "Sub Pedido não foi encontrado. Refaça sua pesquisa.";
                        return;
                    }
                }
                else
                {
                    if (ddlMaterialGrupo.SelectedValue.Trim() == "")
                    {
                        labErro.Text = "Selecion o Grupo do Tecido.";
                        return;
                    }
                    if (ddlMaterialSubGrupo.SelectedValue.Trim() == "")
                    {
                        labErro.Text = "Selecion o SubGrupo do Tecido.";
                        return;
                    }
                }

                CarregarPedidosLoteReserva(pedidos);
            }
            catch (Exception ex)
            {
                labErroReserva.Text = ex.Message;
            }
        }
        private void CarregarPedidosLoteReserva(List<SP_OBTER_PEDIDOSUB_RESERVADOResult> pedidosSub)
        {
            if (pedidosSub == null || pedidosSub.Count <= 0)
            {
                var subGrupo = (ddlMaterialSubGrupo.SelectedItem == null) ? "" : ddlMaterialSubGrupo.SelectedItem.Text.Trim();
                pedidosSub = desenvController.ObterPedidoSubReservado(0, ddlMaterialGrupo.SelectedItem.Text.Trim(), subGrupo, ddlCor.SelectedValue, ddlCorFornecedor.SelectedValue.Trim());
            }

            gvPedidoLoteRes.DataSource = pedidosSub;
            gvPedidoLoteRes.DataBind();
        }
        protected void gvPedidoLoteRes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PEDIDOSUB_RESERVADOResult pedido = e.Row.DataItem as SP_OBTER_PEDIDOSUB_RESERVADOResult;

                    if (pedido != null)
                    {
                        if (pedido.CODIGO > 0)
                        {
                            Label labNumeroPedido = e.Row.FindControl("labNumeroPedido") as Label;
                            labNumeroPedido.Text = pedido.NUMERO_PEDIDO.ToString() + "-" + pedido.ITEM.ToString();

                            Label labMaterial = e.Row.FindControl("labMaterial") as Label;
                            var materiais = desenvController.ObterMaterial(pedido.GRUPO.Trim(), pedido.SUBGRUPO.Trim());
                            if (materiais != null)
                            {
                                var materialCor = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == pedido.COR.Trim() && p.REFER_FABRICANTE.Trim() == pedido.COR_FORNECEDOR.Trim() && materiais.Any(x => x.MATERIAL == p.MATERIAL)).FirstOrDefault();
                                if (materialCor != null)
                                    labMaterial.Text = materialCor.MATERIAL.Trim();
                            }

                            Label labGrupoMaterial = e.Row.FindControl("labGrupoMaterial") as Label;
                            labGrupoMaterial.Text = pedido.GRUPO.Trim();

                            Label labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                            labSubGrupo.Text = pedido.SUBGRUPO;

                            Label labCor = e.Row.FindControl("labCor") as Label;
                            labCor.Text = pedido.COR + " - " + prodController.ObterCoresBasicas(pedido.COR).DESC_COR.Trim();

                            Label labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                            labCorFornecedor.Text = pedido.COR_FORNECEDOR;

                            Button btAdicionarCarrinhoReserva = e.Row.FindControl("btAdicionarCarrinhoReserva") as Button;
                            btAdicionarCarrinhoReserva.CommandArgument = pedido.CODIGO.ToString();
                        }
                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvPedidoLoteRes_DataBound(object sender, EventArgs e)
        {
        }
        protected void btAdicionarCarrinhoReserva_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labPedido.Text = "";
                labErroReserva.Text = "";

                if (Session["USUARIO"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                Button bt = (Button)(sender);
                GridViewRow row = (GridViewRow)bt.NamingContainer;

                int codigoPedidoSub = Convert.ToInt32(bt.CommandArgument);
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                var pedidoNoCarrinho = desenvController.ObterCarrinhoPedidoSub(codigoPedidoSub, codigoUsuario);
                if (pedidoNoCarrinho != null)
                {
                    labErroReserva.Text = "Pedido já foi adicionado.";
                    return;
                }

                var pedidoSub = desenvController.ObterPedidoSub(codigoPedidoSub);
                if (pedidoSub != null)
                {
                    var carrinho = new DESENV_PEDIDO_CARRINHO_SUB
                    {
                        DESENV_PEDIDO_SUB = codigoPedidoSub,
                        USUARIO = codigoUsuario,
                        QTDE = pedidoSub.QTDE,
                        PRECO = pedidoSub.VALOR,
                        DATA_INCLUSAO = DateTime.Now
                    };
                    desenvController.InserirCarrinhoPedidoSub(carrinho);

                    row.BackColor = Color.PaleGreen;
                    row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                    row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(row.BackColor) + "';this.style.cursor='hand'");

                    CarregarCarrinhoReservado();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarCarrinhoReservado()
        {
            var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var carrinho = desenvController.ObterCarrinhoPedidoSubPorUsuario(codigoUsuario);
            gvCarrinhoReserva.DataSource = carrinho;
            gvCarrinhoReserva.DataBind();
        }
        protected void btAtualizarCarrinhoReserva_Click(object sender, EventArgs e)
        {
            try
            {
                labPedido.Text = "";
                labErroReserva.Text = "";
                labErro.Text = "";
                CarregarCarrinhoReservado();
            }
            catch (Exception)
            {
            }
        }

        protected void gvCarrinhoReserva_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO_CARRINHO_SUB carrinho = e.Row.DataItem as DESENV_PEDIDO_CARRINHO_SUB;

                    if (carrinho != null)
                    {

                        Literal litPedido = e.Row.FindControl("litPedido") as Literal;
                        litPedido.Text = carrinho.DESENV_PEDIDO_SUB1.DESENV_PEDIDO1.NUMERO_PEDIDO.ToString() + "-" + carrinho.DESENV_PEDIDO_SUB1.ITEM.ToString();

                        Literal litMaterial = e.Row.FindControl("litMaterial") as Literal;
                        var materiais = desenvController.ObterMaterial(carrinho.DESENV_PEDIDO_SUB1.DESENV_PEDIDO1.GRUPO.Trim(), carrinho.DESENV_PEDIDO_SUB1.DESENV_PEDIDO1.SUBGRUPO.Trim());
                        if (materiais != null)
                        {
                            var materialCor = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == carrinho.DESENV_PEDIDO_SUB1.DESENV_PEDIDO1.COR.Trim() && p.REFER_FABRICANTE.Trim() == carrinho.DESENV_PEDIDO_SUB1.DESENV_PEDIDO1.COR_FORNECEDOR.Trim() && materiais.Any(x => x.MATERIAL == p.MATERIAL)).FirstOrDefault();
                            if (materialCor != null)
                                litMaterial.Text = materialCor.MATERIAL.Trim();
                        }

                        Literal litTecido = e.Row.FindControl("litTecido") as Literal;
                        litTecido.Text = carrinho.DESENV_PEDIDO_SUB1.DESENV_PEDIDO1.SUBGRUPO.Trim();

                        Literal litCorFornecedor = e.Row.FindControl("litCorFornecedor") as Literal;
                        litCorFornecedor.Text = carrinho.DESENV_PEDIDO_SUB1.DESENV_PEDIDO1.COR_FORNECEDOR;

                        Literal litUnidadeMedida = e.Row.FindControl("litUnidadeMedida") as Literal;
                        litUnidadeMedida.Text = carrinho.DESENV_PEDIDO_SUB1.DESENV_PEDIDO1.UNIDADE_MEDIDA1.DESCRICAO;

                        var qtde = (carrinho.QTDE != null) ? carrinho.QTDE : 0.00M;
                        TextBox txtQtdeRes = e.Row.FindControl("txtQtdeRes") as TextBox;
                        txtQtdeRes.Text = qtde.ToString();

                        var preco = (carrinho.PRECO != null) ? carrinho.PRECO : 0.00M;
                        TextBox txtPrecoRes = e.Row.FindControl("txtPrecoRes") as TextBox;
                        txtPrecoRes.Text = preco.ToString();

                        Literal litValTotal = e.Row.FindControl("litValTotal") as Literal;
                        litValTotal.Text = "R$ " + Convert.ToDecimal(qtde * preco).ToString("###,###,###,##0.00");

                        ImageButton btExcluirItemCarrinhoReserva = e.Row.FindControl("btExcluirItemCarrinhoReserva") as ImageButton;
                        if (btExcluirItemCarrinhoReserva != null)
                            btExcluirItemCarrinhoReserva.CommandArgument = carrinho.CODIGO.ToString();

                        if (carrinho.QTDE > 0 && carrinho.PRECO > 0)
                            e.Row.BackColor = Color.PaleGreen;

                        qtdeTotal += Convert.ToDecimal(qtde);
                        valTotal += Convert.ToDecimal(qtde * preco);

                    }
                }
            }
        }
        protected void gvCarrinhoReserva_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCarrinhoReserva.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[5].Text = qtdeTotal.ToString();
                footer.Cells[8].Text = "R$ " + valTotal.ToString("###,###,##0.00");
            }
        }
        protected void txtQtdeRes_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labPedido.Text = "";
                labErroReserva.Text = "";

                TextBox txt = (TextBox)sender;

                GridViewRow row = (GridViewRow)txt.NamingContainer;
                int codigoItemCarrinho = Convert.ToInt32(gvCarrinhoReserva.DataKeys[row.RowIndex].Value);

                //atualizar grade do produto no carrinho
                decimal qtde = (txt.Text.Trim() == "") ? 0 : Convert.ToDecimal(txt.Text.Trim());

                var carrinho = desenvController.ObterCarrinhoPedidoSub(codigoItemCarrinho);
                carrinho.QTDE = qtde;
                desenvController.AtualizarCarrinhoPedidoSub(carrinho);

                CarregarCarrinhoReservado();
            }
            catch (Exception ex)
            {
                labErroReserva.Text = ex.Message;
            }
        }
        protected void txtPrecoRes_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labPedido.Text = "";
                labErroReserva.Text = "";

                TextBox txt = (TextBox)sender;

                GridViewRow row = (GridViewRow)txt.NamingContainer;
                int codigoItemCarrinho = Convert.ToInt32(gvCarrinhoReserva.DataKeys[row.RowIndex].Value);

                //atualizar grade do produto no carrinho
                decimal preco = (txt.Text.Trim() == "") ? 0 : Convert.ToDecimal(txt.Text.Trim());

                var carrinho = desenvController.ObterCarrinhoPedidoSub(codigoItemCarrinho);
                carrinho.PRECO = preco;
                desenvController.AtualizarCarrinhoPedidoSub(carrinho);

                CarregarCarrinhoReservado();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btExcluirCarrinhoReserva_Click(object sender, EventArgs e)
        {
            try
            {
                var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                desenvController.ExcluirCarrinhoPedidoSubPorUsuario(codigoUsuario);
                CarregarCarrinhoReservado();
            }
            catch (Exception)
            {
            }
        }
        protected void btExcluirItemCarrinhoReserva_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)(sender);
                var codigoCarrinho = Convert.ToInt32(bt.CommandArgument);
                desenvController.ExcluirCarrinhoPedidoSubPorCodigo(codigoCarrinho);
                CarregarCarrinhoReservado();
            }
            catch (Exception)
            {
            }
        }
        protected void btGerarPedidoReserva_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    if (gvCarrinhoReserva.Rows.Count > 0)
                    {
                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('desenv_material_comprav2_gerarres.aspx');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }


        #endregion
    }
}
