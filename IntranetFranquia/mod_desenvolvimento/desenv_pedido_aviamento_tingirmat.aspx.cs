using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_pedido_aviamento_tingirmat : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarGrupos();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btCancelar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btCancelar, null) + ";");
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "Selecione" });
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
                labErro.Text = ex.Message;
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
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "Selecione" });

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
            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "Selecione" });

            ddlCor.DataSource = coresBasicas;
            ddlCor.DataBind();

            coresBasicas.RemoveAt(0);
            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
            ddlCorFiltro.DataSource = coresBasicas;
            ddlCorFiltro.DataBind();

            if (coresBasicas.Count == 2)
            {
                ddlCor.SelectedIndex = 1;
                ddlCor_SelectedIndexChanged(ddlCor, null);
            }

        }
        protected void ddlCor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCorFornecedor(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim(), ddlCor.SelectedValue);
        }
        private void CarregarCorFornecedor(string grupo, string subGrupo, string cor)
        {
            var filtroMaterial = desenvController.ObterMaterial(grupo, subGrupo);
            var materialCores = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == cor.Trim()).ToList();

            materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).OrderBy(p => p.REFER_FABRICANTE).ToList();
            materialCores.Insert(0, new MATERIAIS_CORE { REFER_FABRICANTE = "Selecione" });

            ddlCorFornecedor.DataSource = materialCores;
            ddlCorFornecedor.DataBind();

            if (materialCores.Count == 2)
            {
                ddlCorFornecedor.SelectedIndex = 1;
            }
        }

        #endregion

        #region "MATERIAL"
        private List<SP_OBTER_MATERIAL_TINGIMENTOResult> ObterMaterialTingimento()
        {
            var materiaisHB = desenvController.ObterMaterialTingimento(txtMaterial.Text.Trim(), "", "", "");

            if (ddlCorFiltro.SelectedValue != "")
                materiaisHB = materiaisHB.Where(p => p.COR_MATERIAL.Trim() == ddlCorFiltro.SelectedValue.Trim()).ToList();

            if (ddlStatus.SelectedValue != "")
            {
                if (ddlStatus.SelectedValue == "S")
                    materiaisHB = materiaisHB.Where(p => p.DATA_TING == null).ToList();
                else
                    materiaisHB = materiaisHB.Where(p => p.DATA_TING != null).ToList();
            }



            return materiaisHB;
        }
        private void CarregarMateriais()
        {
            try
            {
                labErro.Text = "";

                var materiaisHB = ObterMaterialTingimento();

                gvMaterial.DataSource = materiaisHB;
                gvMaterial.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void gvMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_TINGIMENTOResult ting = e.Row.DataItem as SP_OBTER_MATERIAL_TINGIMENTOResult;

                    if (ting != null)
                    {
                        Literal litColecao = e.Row.FindControl("litColecao") as Literal;
                        litColecao.Text = baseController.BuscaColecaoAtual(ting.COLECAO).DESC_COLECAO.Trim();

                        Literal litDataTing = e.Row.FindControl("litDataTing") as Literal;
                        litDataTing.Text = (ting.DATA_TING == null) ? "-" : Convert.ToDateTime(ting.DATA_TING).ToString("dd/MM/yyyy");

                        TextBox txtQtde = e.Row.FindControl("txtQtde") as TextBox;
                        txtQtde.Text = ting.QTDE.ToString();

                        CheckBox cbTingido = e.Row.FindControl("cbTingido") as CheckBox;
                        if (ting.DATA_TING != null)
                        {
                            cbTingido.Checked = true;
                            cbTingido.Enabled = false;
                            txtQtde.Enabled = false;
                            txtQtde.Text = ting.QTDE_SOL.ToString();
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
        protected void gvMaterial_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvMaterial.FooterRow;
            if (footer != null)
            {
            }
        }
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarMateriais();
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroMsg.Text = "";
                txtQtdeSol.Text = "";
                btSalvar.Enabled = true;

                var grupoMaterial = ddlMaterialGrupo.SelectedItem.Text.Trim();
                var subGrupoMaterial = (ddlMaterialSubGrupo.SelectedItem == null) ? "" : ddlMaterialSubGrupo.SelectedItem.Text.Trim();

                if (txtMaterial.Text.Trim() == "")
                {
                    if (ddlMaterialGrupo.SelectedValue == "Selecione" || ddlMaterialSubGrupo.SelectedValue == "Selecione" || ddlCor.SelectedValue == "" || ddlCorFornecedor.SelectedValue == "Selecione")
                    {
                        labErro.Text = "Selecione o material.";
                        return;
                    }

                    var material = desenvController.ObterMaterialCorV2(grupoMaterial, subGrupoMaterial, "")
                                        .Where(p => p.COR_MATERIAL.Trim() == ddlCor.SelectedValue.Trim() && p.REFER_FABRICANTE.Trim() == ddlCorFornecedor.SelectedValue.Trim()).FirstOrDefault();

                    if (material == null)
                    {
                        labErro.Text = "Material não foi encontrado. Refaça sua pesquisa.";
                        return;
                    }

                    txtMaterial.Text = material.MATERIAL.Split('|')[0].ToString();
                }
                else
                {
                    var material = desenvController.ObterMaterial(txtMaterial.Text.Trim());
                    if (material == null)
                    {
                        labErro.Text = "Material não foi encontrado. Refaça sua pesquisa.";
                        return;
                    }

                    if (ddlMaterialGrupo.SelectedValue == "Selecione" || ddlMaterialSubGrupo.SelectedValue == "Selecione" || ddlCor.SelectedValue == "" || ddlCorFornecedor.SelectedValue == "Selecione")
                    {
                        labErro.Text = "Selecione o material.";
                        return;
                    }
                }

                btCancelar.Visible = true;
                txtMaterial.Enabled = false;
                ddlMaterialGrupo.Enabled = false;
                ddlMaterialSubGrupo.Enabled = false;
                ddlCor.Enabled = false;
                ddlCorFornecedor.Enabled = false;

                var qtdeEstoque = 0M;
                var materialEstoque = desenvController.ObterMaterialEstoque(txtMaterial.Text.Trim(), "", "", ddlCor.SelectedValue.Trim(), "");
                if (materialEstoque != null && materialEstoque.Count() > 0)
                {
                    qtdeEstoque = Convert.ToDecimal(materialEstoque.Sum(p => p.QTDE_ESTOQUE));
                    txtEstoque.Text = qtdeEstoque.ToString();
                }

                if (qtdeEstoque <= 0)
                {
                    labErro.Text = "Material selecionado não possui Estoque para tingimento.";
                    return;
                }

                CarregarMateriais();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroMsg.Text = "";


                txtMaterial.Enabled = true;
                ddlMaterialGrupo.Enabled = true;
                ddlMaterialSubGrupo.Enabled = true;
                ddlCor.Enabled = true;
                ddlCorFornecedor.Enabled = true;

                txtMaterial.Text = "";
                txtEstoque.Text = "";
                ddlMaterialGrupo.SelectedValue = "Selecione";
                ddlMaterialSubGrupo.SelectedValue = "Selecione";
                ddlCor.SelectedValue = "";
                ddlCorFornecedor.SelectedValue = "Selecione";

                txtQtdeSol.Text = "";

                gvMaterial.DataSource = new List<SP_OBTER_MATERIAL_TINGIMENTOResult>();
                gvMaterial.DataBind();

                btCancelar.Visible = false;
                btSalvar.Enabled = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void txtMaterial_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var material = desenvController.ObterMaterial(txtMaterial.Text.Trim());
                if (material == null)
                {
                    labErro.Text = "Material não foi encontrado. Refaça sua pesquisa.";
                    return;
                }

                ddlMaterialGrupo.SelectedValue = material.GRUPO;
                ddlMaterialGrupo_SelectedIndexChanged(ddlMaterialGrupo, null);
                ddlMaterialSubGrupo.SelectedValue = material.SUBGRUPO;
                ddlMaterialSubGrupo_SelectedIndexChanged(ddlMaterialSubGrupo, null);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void txtQtde_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                GridViewRow row = (GridViewRow)txt.NamingContainer;
                CheckBox box = row.FindControl("cbTingido") as CheckBox;
                box.Checked = true;
                cbTingido_CheckedChanged(box, null);
            }
            catch (Exception)
            {
            }
        }

        protected void cbTingido_CheckedChanged(object sender, EventArgs e)
        {
            CalcularQtdeSolicitada();
        }
        private void CalcularQtdeSolicitada()
        {
            var qtdeTotal = 0M;
            foreach (GridViewRow row in gvMaterial.Rows)
            {
                CheckBox box = row.FindControl("cbTingido") as CheckBox;
                if (box.Checked)
                {
                    TextBox txt = row.FindControl("txtQtde") as TextBox;
                    if (txt.Text.Trim() != "")
                    {
                        Literal litDataTing = row.FindControl("litDataTing") as Literal;
                        if (litDataTing.Text == "-")
                        {
                            var qtde = Convert.ToDecimal(txt.Text.Trim());
                            qtdeTotal = qtdeTotal + qtde;
                        }
                    }
                }
            }

            txtQtdeSol.Text = qtdeTotal.ToString();
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErroMsg.Text = "";
                labErro.Text = "";

                if (txtQtdeSol.Text == "" || Convert.ToDecimal(txtQtdeSol.Text) <= 0)
                {
                    labErroMsg.Text = "Informe a quantidade do material a ser tingido.";
                    return;
                }

                var qtdeEstoque = Convert.ToDecimal(txtEstoque.Text);
                var qtdeSol = Convert.ToDecimal(txtQtdeSol.Text);

                if (qtdeEstoque < qtdeSol)
                {
                    labErroMsg.Text = "A quantidade de material solicitada é maior que a quantidade disponível em estoque.";
                    return;
                }

                // gerar retirada
                // insere na tabela
                int mm = 1;
                foreach (GridViewRow row in gvMaterial.Rows)
                {
                    CheckBox box = row.FindControl("cbTingido") as CheckBox;
                    if (box.Checked)
                    {
                        TextBox txt = row.FindControl("txtQtde") as TextBox;
                        if (txt.Text.Trim() != "")
                        {
                            Literal litDataTing = row.FindControl("litDataTing") as Literal;
                            if (litDataTing.Text == "-")
                            {
                                var qtde = Convert.ToDecimal(txt.Text.Trim());

                                var codigoProdAviamento = Convert.ToInt32(gvMaterial.DataKeys[row.RowIndex][0].ToString().Trim());
                                var material = gvMaterial.DataKeys[row.RowIndex][1].ToString().Trim();
                                var corMaterial = gvMaterial.DataKeys[row.RowIndex][2].ToString().Trim();

                                var ting = new DESENV_MATERIAL_TING();
                                ting.PROD_HB_PROD_AVIAMENTO = codigoProdAviamento;
                                ting.MATERIAL = material;
                                ting.COR_MATERIAL = corMaterial;
                                ting.QTDE_SOL = qtde;
                                ting.DATA_INCLUSAO = DateTime.Now;
                                ting.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                                desenvController.InserirMaterialTingimento(ting);

                                // retira do estoque
                                var matEstoqueRet = new DESENV_MATERIAL_ESTOQUE();
                                matEstoqueRet.MATERIAL = txtMaterial.Text.Trim();
                                matEstoqueRet.COR_MATERIAL = ddlCor.SelectedValue.Trim();
                                matEstoqueRet.DATA_ENTRADA = DateTime.Now.AddSeconds(mm);
                                matEstoqueRet.QTDE_ESTOQUE = (qtde * -1.00M);

                                desenvController.InserirMaterialEstoque(matEstoqueRet);

                                //controle para aumentar o segundos
                                mm = mm + 2;
                            }
                        }
                    }
                }


                labErroMsg.Text = "Os materiais foram retirados com sucesso do estoque da Cor " + ddlCor.SelectedItem.Text.Trim() + " para tingimento.";
                btSalvar.Enabled = false;
                // enviar email



            }
            catch (Exception ex)
            {
                labErroMsg.Text = ex.Message;
            }
        }


    }
}
