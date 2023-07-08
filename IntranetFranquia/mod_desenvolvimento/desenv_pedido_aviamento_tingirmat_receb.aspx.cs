using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_pedido_aviamento_tingirmat_receb : System.Web.UI.Page
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
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");

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
            materialCores.Insert(0, new MATERIAIS_CORE { REFER_FABRICANTE = "" });

            ddlCorFornecedor.DataSource = materialCores;
            ddlCorFornecedor.DataBind();

            if (materialCores.Count == 2)
            {
                ddlCorFornecedor.SelectedIndex = 1;
            }
        }

        #endregion

        #region "MATERIAL"
        private List<SP_OBTER_MATERIAL_TINGIMENTO_RECEBResult> ObterMaterialTingimento()
        {
            var materiaisHB = desenvController.ObterMaterialTingimentoReceb(txtMaterial.Text.Trim(), ddlMaterialGrupo.SelectedValue.Trim(), ddlMaterialSubGrupo.SelectedValue.Trim(), ddlCor.SelectedValue);

            if (ddlAberto.SelectedValue != "")
            {
                if (ddlAberto.SelectedValue == "S")
                    materiaisHB = materiaisHB.Where(p => p.DATA_RECEB == null).ToList();
                else
                    materiaisHB = materiaisHB.Where(p => p.DATA_RECEB != null).ToList();
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
                    SP_OBTER_MATERIAL_TINGIMENTO_RECEBResult ting = e.Row.DataItem as SP_OBTER_MATERIAL_TINGIMENTO_RECEBResult;

                    if (ting != null)
                    {
                        Literal litColecao = e.Row.FindControl("litColecao") as Literal;
                        litColecao.Text = baseController.BuscaColecaoAtual(ting.COLECAO).DESC_COLECAO.Trim();

                        Literal litDataEnvio = e.Row.FindControl("litDataEnvio") as Literal;
                        litDataEnvio.Text = (ting.DATA_TING == null) ? "-" : Convert.ToDateTime(ting.DATA_TING).ToString("dd/MM/yyyy");

                        Literal litDataReceb = e.Row.FindControl("litDataReceb") as Literal;
                        litDataReceb.Text = (ting.DATA_RECEB == null) ? "-" : Convert.ToDateTime(ting.DATA_RECEB).ToString("dd/MM/yyyy");

                        TextBox txtQtde = e.Row.FindControl("txtQtde") as TextBox;
                        txtQtde.Text = ting.QTDE_SOL.ToString();

                        CheckBox cbTingido = e.Row.FindControl("cbTingido") as CheckBox;
                        if (ting.DATA_RECEB != null)
                        {
                            cbTingido.Checked = true;
                            cbTingido.Enabled = false;
                            txtQtde.Enabled = false;
                            txtQtde.Text = ting.QTDE_RECEB.ToString();
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

                CarregarMateriais();

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
                        Literal litDataReceb = row.FindControl("litDataReceb") as Literal;
                        if (litDataReceb.Text == "-")
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
                    labErroMsg.Text = "Informe a quantidade do material recebido.";
                    return;
                }

                // gerar retirada
                // insere na tabela
                foreach (GridViewRow row in gvMaterial.Rows)
                {
                    CheckBox box = row.FindControl("cbTingido") as CheckBox;
                    if (box.Checked)
                    {
                        TextBox txt = row.FindControl("txtQtde") as TextBox;
                        if (txt.Text.Trim() != "")
                        {
                            Literal litDataReceb = row.FindControl("litDataReceb") as Literal;
                            if (litDataReceb.Text == "-")
                            {
                                var qtde = Convert.ToDecimal(txt.Text.Trim());

                                var codigoTing = Convert.ToInt32(gvMaterial.DataKeys[row.RowIndex][0].ToString().Trim());
                                var material = gvMaterial.DataKeys[row.RowIndex][1].ToString().Trim();
                                var corMaterial = gvMaterial.DataKeys[row.RowIndex][2].ToString().Trim();

                                var ting = desenvController.ObterMaterialTingimento(codigoTing);
                                if (ting != null)
                                {
                                    ting.QTDE_RECEB = qtde;
                                    ting.DATA_RECEB = DateTime.Now;
                                    ting.USUARIO_RECEB = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                                    desenvController.AtualizarMaterialTingimento(ting);

                                    // adicionar do estoque
                                    var matEstoqueRet = new DESENV_MATERIAL_ESTOQUE();
                                    matEstoqueRet.MATERIAL = material;
                                    matEstoqueRet.COR_MATERIAL = corMaterial;
                                    matEstoqueRet.DATA_ENTRADA = DateTime.Now;
                                    matEstoqueRet.QTDE_ESTOQUE = (qtde);

                                    desenvController.InserirMaterialEstoque(matEstoqueRet);
                                }
                            }
                        }
                    }
                }

                labErroMsg.Text = "Os materiais tingidos foram adicionados com sucesso ao estoque.";
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
