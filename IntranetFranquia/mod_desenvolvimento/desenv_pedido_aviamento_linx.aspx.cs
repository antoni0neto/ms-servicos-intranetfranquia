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
    public partial class desenv_pedido_aviamento_linx : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        const string DESENV_FICTEC_PEDCOMPRA_PROD = "DESENV_FICTEC_PEDCOMPRA_PROD";

        decimal qtdeMaterial = 0;
        decimal valTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();

                CarregarFabricante();
                CarregarFilial();
                CarregarGrupos();

                var codigoCarrinhoLinxCab = "";
                var codigoCarrinhoCab = "";
                if (Request.QueryString["p"] != null)
                    codigoCarrinhoCab = Request.QueryString["p"].ToString();

                if (Request.QueryString["l"] != null)
                    codigoCarrinhoLinxCab = Request.QueryString["l"].ToString();

                hidCodigoCarrinhoCab.Value = codigoCarrinhoCab;

                var carrinhoCab = desenvController.ObterCarrinhoMaterialCab(Convert.ToInt32(codigoCarrinhoCab));
                txtPedidoIntra.Text = carrinhoCab.PEDIDO;
                ddlFilial.SelectedValue = carrinhoCab.CODIGO_FILIAL.Trim();
                labPedidoTipo.Text = carrinhoCab.DESENV_MATERIAL_PEDIDO_PERFIL1.PERFIL;

                if (codigoCarrinhoLinxCab == "")
                {
                    var existeCarrinhoAberto = desenvController.ObterCarrinhoMaterialLinxCabPorCarrinhoCab(carrinhoCab.CODIGO).Where(p => p.DATA_FECHAMENTO == null).FirstOrDefault();
                    if (existeCarrinhoAberto == null)
                    {
                        int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                        //abrir carrinho
                        var carrinhoLinxCab = new DESENV_MATERIAL_CARRINHO_LINX_CAB();
                        carrinhoLinxCab.PEDIDO = carrinhoCab.PEDIDO;
                        carrinhoLinxCab.USUARIO = codigoUsuario;
                        carrinhoLinxCab.DATA_ABERTURA = DateTime.Now;
                        carrinhoLinxCab.FILIAL = carrinhoCab.FILIAL;
                        carrinhoLinxCab.CODIGO_FILIAL = carrinhoCab.CODIGO_FILIAL;
                        carrinhoLinxCab.DESENV_MATERIAL_CARRINHO_CAB = carrinhoCab.CODIGO;
                        codigoCarrinhoLinxCab = desenvController.InserirCarrinhoMaterialLinxCab(carrinhoLinxCab).ToString();
                    }
                    else
                    {
                        codigoCarrinhoLinxCab = existeCarrinhoAberto.CODIGO.ToString();
                    }

                    hidCodigoCarrinhoLinxCab.Value = codigoCarrinhoLinxCab;
                }
                else
                {
                    btBuscar.Enabled = false;
                    btAtualizarCarrinho.Enabled = false;
                    btExcluirCarrinho.Enabled = false;
                    btAdicionarTodos.Enabled = false;
                    btGerarPedido.Enabled = false;
                    hidBloqueio.Value = "S";
                }

                CarregarCarrinhoLinx(codigoCarrinhoLinxCab);

                Session[DESENV_FICTEC_PEDCOMPRA_PROD] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                Session["COLECAO"] = ddlColecoes.SelectedValue;
                CarregarOrigem(colecao);
            }

        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                //if (Session["COLECAO"] != null)
                //{
                //    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
                //    CarregarOrigem(Session["COLECAO"].ToString().Trim());
                //    ddlColecoes_SelectedIndexChanged(null, null);
                //}
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = (prodController.ObterGrupoProduto("01"));
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = desenvController.ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList();
            _origem = _origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (_origem != null)
            {
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();

                if (_origem.Count == 2)
                    ddlOrigem.SelectedValue = _origem[1].CODIGO.ToString();

            }
        }
        private void CarregarFabricante()
        {
            var _fornecedores = prodController.ObterFornecedor().ToList();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });
            _fornecedores.Where(p => ((p.STATUS == 'A' && p.TIPO == 'A') || p.STATUS == 'S')).GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (_fornecedores != null)
            {
                ddlFabricante.DataSource = _fornecedores;
                ddlFabricante.DataBind();
            }
        }
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais();

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                filial.Insert(1, new FILIAI { COD_FILIAL = "1055", FILIAL = "C-MAX (NOVA)" });
                filial.Insert(2, new FILIAI { COD_FILIAL = "000029", FILIAL = "CD - LUGZY               " });
                filial.Insert(3, new FILIAI { COD_FILIAL = "000041", FILIAL = "CD MOSTRUARIO            " });
                filial.Insert(4, new FILIAI { COD_FILIAL = "1029", FILIAL = "ATACADO HANDBOOK         " });
                filial.Insert(5, new FILIAI { COD_FILIAL = "1054", FILIAL = "HANDBOOK ONLINE          " });

                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }

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
        private void CarregarMaterialPedido(string colecao, int? desenvOrigem, string produto, string grupoProduto, string grupoMaterial, string subGrupoMaterial, string filial, string fornecedorMaterial, string pedidoIntra)
        {
            var materialPedido = ObterMaterialPrePedido(colecao, desenvOrigem, produto, grupoProduto, grupoMaterial, subGrupoMaterial, filial, fornecedorMaterial, pedidoIntra);

            gvMaterialPedido.DataSource = materialPedido;
            gvMaterialPedido.DataBind();

            Session[DESENV_FICTEC_PEDCOMPRA_PROD] = materialPedido;
        }
        protected void gvMaterialPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_PREPEDIDOResult materialPedido = e.Row.DataItem as SP_OBTER_MATERIAL_PREPEDIDOResult;

                    if (materialPedido != null)
                    {
                        int? codFicTec = (materialPedido.COD_DESENV_PRODUTO_FICTEC == 0) ? null : (int?)materialPedido.COD_DESENV_PRODUTO_FICTEC;
                        var materialNoCarrinho = desenvController.ObterCarrinhoLinxMaterial(materialPedido.PEDIDO_INTRA, materialPedido.MATERIAL, materialPedido.COR_MATERIAL, codFicTec, Convert.ToInt32(hidCodigoCarrinhoLinxCab.Value));
                        if (materialNoCarrinho != null)
                            e.Row.BackColor = Color.SandyBrown;

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvMaterialPedido_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[DESENV_FICTEC_PEDCOMPRA_PROD] != null)
            {
                IEnumerable<SP_OBTER_MATERIAL_PREPEDIDOResult> materialPedido = (IEnumerable<SP_OBTER_MATERIAL_PREPEDIDOResult>)Session[DESENV_FICTEC_PEDCOMPRA_PROD];

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

        private List<SP_OBTER_MATERIAL_PREPEDIDOResult> ObterMaterialPrePedido(string colecao, int? desenvOrigem, string produto, string grupoProduto, string grupoMaterial, string subGrupoMaterial, string filial, string fornecedorMaterial, string pedidoIntra)
        {
            var corMaterial = (ddlCor.SelectedItem != null) ? ddlCor.SelectedValue.Trim() : "";

            var materialPedido = desenvController.ObterMaterialPrePedido(colecao, desenvOrigem, produto, grupoProduto, grupoMaterial, subGrupoMaterial, filial, fornecedorMaterial, pedidoIntra);

            if (corMaterial != "")
                materialPedido = materialPedido.Where(p => p.COR_MATERIAL.Trim() == corMaterial).ToList();

            return materialPedido;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            int? desenvOrigem = null;

            try
            {
                labMsg.Text = "";

                if (ddlFilial.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Filial.";
                    return;
                }

                //if (ddlColecoes.SelectedValue == "")
                //{
                //    labMsg.Text = "Selecione a Coleção.";
                //    return;
                //}

                if (ddlOrigem.SelectedValue != "" && ddlOrigem.SelectedValue != "0")
                    desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

                var subGrupoMaterial = (ddlMaterialSubGrupo.SelectedItem != null) ? ddlMaterialSubGrupo.SelectedItem.Text.Trim() : "";

                CarregarMaterialPedido(ddlColecoes.SelectedValue.Trim(), desenvOrigem, txtProduto.Text.Trim(), ddlGrupo.SelectedValue.Trim(), ddlMaterialGrupo.SelectedItem.Text.Trim(), subGrupoMaterial, ddlFilial.SelectedItem.Text.Trim(), ddlFabricante.SelectedValue.Trim(), txtPedidoIntra.Text.Trim().ToUpper());

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        private void CarregarCarrinhoLinx(string codigoCarrinhoLinxCab)
        {
            var carrinho = desenvController.ObterMaterialCarrinhoLinxPrePedido(Convert.ToInt32(codigoCarrinhoLinxCab));
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_CARRINHO_LINX_PREPEDIDOResult carrinho = e.Row.DataItem as SP_OBTER_MATERIAL_CARRINHO_LINX_PREPEDIDOResult;

                    if (carrinho != null)
                    {
                        Literal litFornecedor = e.Row.FindControl("litFornecedor") as Literal;
                        litFornecedor.Text = carrinho.FORNECEDOR;

                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = carrinho.PRODUTO;

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = carrinho.DESC_PRODUTO;

                        Literal litSubGrupo = e.Row.FindControl("litSubGrupo") as Literal;
                        litSubGrupo.Text = carrinho.SUBGRUPO_MATERIAL;

                        Literal litCorMaterial = e.Row.FindControl("litCorMaterial") as Literal;
                        litCorMaterial.Text = carrinho.COR_FORNECEDOR;

                        TextBox txtQtde = e.Row.FindControl("txtQtde") as TextBox;
                        txtQtde.Text = carrinho.QTDE.ToString();

                        TextBox txtCustoNota = e.Row.FindControl("txtCustoNota") as TextBox;
                        txtCustoNota.Text = carrinho.CUSTO.ToString();

                        TextBox txtDescontoNota = e.Row.FindControl("txtDescontoNota") as TextBox;
                        txtDescontoNota.Text = carrinho.DESCONTO_ITEM.ToString();

                        Literal litConsumoTotal = e.Row.FindControl("litConsumoTotal") as Literal;
                        litConsumoTotal.Text = "R$ " + Convert.ToDecimal(carrinho.VAL_TOTAL).ToString("###,###,###,##0.0000");

                        Button _btExcluirCarrinho = e.Row.FindControl("btExcluirItemCarrinho") as Button;
                        if (_btExcluirCarrinho != null)
                            _btExcluirCarrinho.CommandArgument = carrinho.CODIGO.ToString();

                        qtdeMaterial += carrinho.QTDE;
                        valTotal += Convert.ToDecimal(carrinho.VAL_TOTAL);

                        if (hidBloqueio.Value == "S")
                        {
                            txtDescontoNota.Enabled = false;
                            txtQtde.Enabled = false;
                            txtCustoNota.Enabled = false;
                            _btExcluirCarrinho.Enabled = false;
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
        protected void gvCarrinho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCarrinho.FooterRow;
            if (footer != null)
            {

                footer.Cells[1].Text = "Total";
                footer.Cells[6].Text = qtdeMaterial.ToString();
                footer.Cells[9].Text = "R$ " + valTotal.ToString("###,###,###,##0.0000");
            }
        }
        protected void btExcluirCarrinho_Click(object sender, EventArgs e)
        {
            try
            {

                if (!PodeAlterarPedido())
                {
                    labMsg.Text = "Não é permitido alterar pedido depois que já foi gerado.";
                    CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
                    return;
                }

                int codigoCarrinhoLinxCab = Convert.ToInt32(hidCodigoCarrinhoLinxCab.Value);
                desenvController.ExcluirCarrinhoLinxMaterialPorCab(codigoCarrinhoLinxCab);
                CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
            }
            catch (Exception)
            {
            }
        }
        protected void btExcluirItemCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PodeAlterarPedido())
                {
                    labMsg.Text = "Não é permitido alterar pedido depois que já foi gerado.";
                    CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
                    return;
                }



                Button bt = (Button)(sender);
                int codigoCarrinho = Convert.ToInt32(bt.CommandArgument);
                desenvController.ExcluirCarrinhoLinxMaterialPorCodigo(codigoCarrinho);

                CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
            }
            catch (Exception)
            {
            }
        }
        protected void btAdicionarCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["USUARIO"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                labPedido.Text = "";
                labMsg.Text = "";

                if (!PodeAlterarPedido())
                {
                    labMsg.Text = "Não é permitido alterar pedido depois que já foi gerado.";
                    CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
                    return;
                }

                Button bt = (Button)(sender);

                GridViewRow row = (GridViewRow)bt.NamingContainer;

                string pedido = gvMaterialPedido.DataKeys[row.RowIndex][0].ToString();
                string material = gvMaterialPedido.DataKeys[row.RowIndex][1].ToString();
                string cor = gvMaterialPedido.DataKeys[row.RowIndex][2].ToString();
                int? codFictec = Convert.ToInt32(gvMaterialPedido.DataKeys[row.RowIndex][3].ToString());
                DateTime entrega = Convert.ToDateTime(gvMaterialPedido.DataKeys[row.RowIndex][4].ToString());

                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                codFictec = (codFictec == 0) ? null : codFictec;

                var materialNoCarrinho = desenvController.ObterCarrinhoLinxMaterial(pedido, material, cor, codFictec, Convert.ToInt32(hidCodigoCarrinhoLinxCab.Value));
                if (materialNoCarrinho != null)
                {
                    labPedido.Text = "Material já foi adicionado.";
                    return;
                }

                //var materialTemPedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor);
                //if (produtoTemPedido != null && produtoTemPedido.PEDIDO_LINX != null)
                //{
                //    labPedido.Text = "Produto já possui pedido gerado no LINX: " + produtoTemPedido.PEDIDO_LINX;
                //    return;
                //}

                var carrinho = new DESENV_MATERIAL_CARRINHO_LINX
                {
                    PEDIDO = pedido,
                    MATERIAL = material,
                    COR_MATERIAL = cor,
                    DESENV_PRODUTO_FICTEC = (codFictec == 0) ? null : codFictec,
                    ENTREGA = entrega,
                    USUARIO = codigoUsuario,
                    DESENV_MATERIAL_CARRINHO_LINX_CAB = Convert.ToInt32(hidCodigoCarrinhoLinxCab.Value)
                };
                desenvController.InserirCarrinhoLinxMaterial(carrinho);

                CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void btAdicionarTodos_Click(object sender, EventArgs e)
        {


            labMsg.Text = "";
            if (!PodeAlterarPedido())
            {
                labMsg.Text = "Não é permitido alterar pedido depois que já foi gerado.";
                CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
                return;
            }

            if (Session["USUARIO"] == null)
            {
                Response.Redirect("~/login.aspx");
                return;
            }

            if (ddlFilial.SelectedValue == "")
            {
                labMsg.Text = "Selecione a Filial.";
                return;
            }

            //if (ddlColecoes.SelectedValue == "")
            //{
            //    labMsg.Text = "Selecione a Coleção.";
            //    return;
            //}

            int desenvOrigem = 0;
            //if (ddlOrigem.SelectedValue != "0")
            //    desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

            var subGrupoMaterial = (ddlMaterialSubGrupo.SelectedItem != null) ? ddlMaterialSubGrupo.SelectedItem.Text.Trim() : "";

            labPedido.Text = "";
            labMsg.Text = "";

            var materiais = ObterMaterialPrePedido(ddlColecoes.SelectedValue.Trim(), desenvOrigem, txtProduto.Text.Trim(), ddlGrupo.SelectedValue.Trim(), ddlMaterialGrupo.SelectedItem.Text.Trim(), subGrupoMaterial, ddlFilial.SelectedItem.Text.Trim(), ddlFabricante.SelectedValue.Trim(), txtPedidoIntra.Text.Trim().ToUpper());
            foreach (var m in materiais)
            {
                string pedido = m.PEDIDO_INTRA;
                string material = m.MATERIAL;
                string cor = m.COR_MATERIAL;
                int? codFictec = (m.COD_DESENV_PRODUTO_FICTEC == 0) ? null : (int?)m.COD_DESENV_PRODUTO_FICTEC;
                DateTime entrega = Convert.ToDateTime(m.ENTREGA);

                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;


                var materialNoCarrinho = desenvController.ObterCarrinhoLinxMaterial(pedido, material, cor, codFictec, Convert.ToInt32(hidCodigoCarrinhoLinxCab.Value));
                if (materialNoCarrinho != null)
                {
                    //labPedido.Text = "Produto já foi adicionado.";
                    continue;
                }

                var carrinho = new DESENV_MATERIAL_CARRINHO_LINX
                {
                    PEDIDO = pedido,
                    MATERIAL = material,
                    COR_MATERIAL = cor,
                    DESENV_PRODUTO_FICTEC = codFictec,
                    ENTREGA = entrega,
                    USUARIO = codigoUsuario,
                    DESENV_MATERIAL_CARRINHO_LINX_CAB = Convert.ToInt32(hidCodigoCarrinhoLinxCab.Value)
                };
                desenvController.InserirCarrinhoLinxMaterial(carrinho);
            }

            CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
        }
        protected void btAtualizarCarrinho_Click(object sender, EventArgs e)
        {
            CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
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

                    labMsg.Text = "";
                    if (!PodeAlterarPedido())
                    {
                        labMsg.Text = "Não é permitido alterar pedido depois que já foi gerado.";
                        //CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
                        return;
                    }

                    string codigoCarrinhoLinxCab = hidCodigoCarrinhoLinxCab.Value;

                    if (gvCarrinho.Rows.Count > 0)
                    {
                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('desenv_pedido_aviamento_linx_gerar.aspx?ccc=" + codigoCarrinhoLinxCab + "');";
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

        protected void txtCustoNota_TextChanged(object sender, EventArgs e)
        {

            try
            {
                labMsg.Text = "";
                if (!PodeAlterarPedido())
                {
                    labMsg.Text = "Não é permitido alterar pedido depois que já foi gerado.";
                    CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
                    return;
                }
                TextBox txt = (TextBox)sender;
                GridViewRow row = (GridViewRow)txt.NamingContainer;

                string pedido = gvCarrinho.DataKeys[row.RowIndex][0].ToString();
                string material = gvCarrinho.DataKeys[row.RowIndex][1].ToString();
                string cor = gvCarrinho.DataKeys[row.RowIndex][2].ToString();
                int codFictec = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex][3].ToString());

                if (txt.Text != "")
                {
                    var materiaisPedido = desenvController.ObterComprasMaterialPrePedido(material, cor);
                    foreach (var m in materiaisPedido)
                    {
                        var mCompraPrePedido = desenvController.ObterComprasMaterialPrePedido(m.PEDIDO, m.MATERIAL, m.COR_MATERIAL, m.DESENV_PRODUTO_FICTEC);
                        mCompraPrePedido.CUSTO_NOTA = Convert.ToDecimal(txt.Text);
                        desenvController.AtualizarComprasMaterialPrePedido(mCompraPrePedido);
                    }
                }

                CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }
        protected void txtQtde_TextChanged(object sender, EventArgs e)
        {

            try
            {

                labMsg.Text = "";
                if (!PodeAlterarPedido())
                {
                    labMsg.Text = "Não é permitido alterar pedido depois que já foi gerado.";
                    CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
                    return;
                }

                TextBox txt = (TextBox)sender;
                GridViewRow row = (GridViewRow)txt.NamingContainer;

                string pedido = gvCarrinho.DataKeys[row.RowIndex][0].ToString();
                string material = gvCarrinho.DataKeys[row.RowIndex][1].ToString();
                string cor = gvCarrinho.DataKeys[row.RowIndex][2].ToString();
                int codFictec = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex][3].ToString());

                if (txt.Text != "")
                {
                    var mCompraPrePedido = desenvController.ObterComprasMaterialPrePedido(pedido, material, cor, codFictec);
                    mCompraPrePedido.QTDE_NOTA = Convert.ToDecimal(txt.Text);
                    desenvController.AtualizarComprasMaterialPrePedido(mCompraPrePedido);
                }

                CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }
        }
        protected void txtDescontoNota_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";
                if (!PodeAlterarPedido())
                {
                    labMsg.Text = "Não é permitido alterar pedido depois que já foi gerado.";
                    CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
                    return;
                }

                TextBox txt = (TextBox)sender;
                GridViewRow row = (GridViewRow)txt.NamingContainer;

                string pedido = gvCarrinho.DataKeys[row.RowIndex][0].ToString();
                string material = gvCarrinho.DataKeys[row.RowIndex][1].ToString();
                string cor = gvCarrinho.DataKeys[row.RowIndex][2].ToString();
                int codFictec = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex][3].ToString());

                if (txt.Text != "")
                {
                    var mCompraPrePedido = desenvController.ObterComprasMaterialPrePedido(pedido, material, cor, codFictec);
                    mCompraPrePedido.DESCONTO_ITEM_NOTA = Convert.ToDecimal(txt.Text);
                    desenvController.AtualizarComprasMaterialPrePedido(mCompraPrePedido);
                }

                CarregarCarrinhoLinx(hidCodigoCarrinhoLinxCab.Value);
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }
        }


        private bool PodeAlterarPedido()
        {
            var pedidoLinxCab = desenvController.ObterCarrinhoMaterialLinxCab(Convert.ToInt32(hidCodigoCarrinhoLinxCab.Value));

            if (pedidoLinxCab != null && pedidoLinxCab.DATA_FECHAMENTO != null)
                return false;

            return true;
        }

    }
}

