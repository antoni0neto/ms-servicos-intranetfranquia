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
    public partial class desenv_material_gruposubgrupo : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna, colunaSub = 0;

        List<MATERIAIS_GRUPO> _matGrupo = new List<MATERIAIS_GRUPO>();
        List<MATERIAIS_SUBGRUPO> _matSubGrupo = new List<MATERIAIS_SUBGRUPO>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarGrupos();

                //Controles
                btCancelar.Visible = false;

                Session["MATERIAL_GRUPOSUBGRUPO"] = null;
                Session["MATERIAL_GRUPOGRUPO"] = null;
                ViewState["sortDirection"] = true;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _materialGrupo = desenvController.ObterMaterialGrupo().Where(p => p.CODIGO_GRUPO.Trim() != "01" &&
                                                                                                    p.CODIGO_GRUPO.Trim() != "02" &&
                                                                                                    p.CODIGO_GRUPO.Trim() != "03" &&
                                                                                                    p.CODIGO_GRUPO.Trim() != "04" &&
                                                                                                    p.CODIGO_GRUPO.Trim() != "05").ToList();

            //Controle de ordenação
            Session["MATERIAL_GRUPOGRUPO"] = _materialGrupo;

            gvMaterialGrupo.DataSource = _materialGrupo.OrderBy(p => p.GRUPO.Trim());
            gvMaterialGrupo.DataBind();
        }
        private void CarregarSubGrupos(string grupo)
        {
            List<MATERIAIS_SUBGRUPO> _materialSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();

            //Controle de ordenação
            Session["MATERIAL_GRUPOSUBGRUPO"] = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();

            if (_materialSubGrupo == null || _materialSubGrupo.Count <= 0)
                _materialSubGrupo.Add(new MATERIAIS_SUBGRUPO { GRUPO = "", CODIGO_SUBGRUPO = "", SUBGRUPO = "" });

            gvMaterialSubGrupo.DataSource = _materialSubGrupo.OrderBy(p => p.SUBGRUPO);
            gvMaterialSubGrupo.DataBind();

            fsSubGrupo.Visible = true;
        }
        private List<UNIDADE> CarregarUnidadeLinx()
        {
            List<UNIDADE> _unidade = new List<UNIDADE>();
            _unidade = desenvController.ObterUnidadeMedidaLinx().Where(p => p.UNIDADE1.Trim() == "KG" || p.UNIDADE1.Trim() == "UN" || p.UNIDADE1.Trim() == "MT").ToList();

            return _unidade.OrderBy(p => p.DESC_UNIDADE).ToList();
        }
        #endregion

        #region "CRUD"
        private void Incluir(MATERIAIS_GRUPO _materialGrupo)
        {
            desenvController.InserirMaterialGrupo(_materialGrupo);
        }
        private void Editar(MATERIAIS_GRUPO _materialGrupo)
        {
            desenvController.AtualizarMaterialGrupo(_materialGrupo);
        }
        private void Incluir(MATERIAIS_SUBGRUPO _materialSubGrupo)
        {
            desenvController.InserirMaterialSubGrupo(_materialSubGrupo);
        }
        private void Editar(MATERIAIS_SUBGRUPO _materialSubGrupo, string subGrupoAtual)
        {
            desenvController.AtualizarMaterialSubGrupo(_materialSubGrupo, subGrupoAtual);
        }
        private void ExcluirGrupo(string grupo)
        {
            desenvController.ExcluirMaterialGrupo(grupo);
        }
        private void ExcluirSubGrupo(string grupo, string subGrupo)
        {
            desenvController.ExcluirMaterialSubGrupo(grupo, subGrupo);
        }
        #endregion

        #region "AÇÕES"
        //GRUPO
        protected void gvMaterialGrupo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    MATERIAIS_GRUPO _materialG = e.Row.DataItem as MATERIAIS_GRUPO;

                    coluna += 1;
                    if (_materialG != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Button _btAbrirSubGrupo = e.Row.FindControl("btAbrirSubGrupo") as Button;
                        if (_btAbrirSubGrupo != null)
                            _btAbrirSubGrupo.CommandArgument = _materialG.GRUPO;
                    }
                }
            }
        }
        protected void btIncluirGrupo_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            string grupo = "";
            string codigoGrupo = "";

            labErro.Text = "";
            if (txtCodigo.Text.Trim() == "")
            {
                labErro.Text = "Informe o Código.";
                return;
            }
            if (txtGrupo.Text.Trim() == "")
            {
                labErro.Text = "Informe o Grupo.";
                return;
            }

            grupo = txtGrupo.Text.Trim().ToUpper();
            codigoGrupo = txtCodigo.Text.Trim().ToUpper();

            string valGrupo = ValidarGrupo(grupo, codigoGrupo);
            if (valGrupo != "")
            {
                labErro.Text = valGrupo;
                return;
            }

            try
            {
                _Inclusao = true;
                if (hidGrupo.Value != "")
                    _Inclusao = false;

                //Inserir
                MATERIAIS_GRUPO matGrupo = new MATERIAIS_GRUPO();
                matGrupo.GRUPO = grupo;
                codigoGrupo = codigoGrupo.PadLeft(2, '0');
                matGrupo.CODIGO_GRUPO = codigoGrupo.Substring((codigoGrupo.Length - 2), 2);
                if (_Inclusao)
                    Incluir(matGrupo);
                else
                    Editar(matGrupo);

                //Recarregar grid de grupos
                CarregarGrupos();

                //Carregar SubGrupos
                hidGrupo.Value = grupo;
                btCancelar.Visible = true;
                CarregarSubGrupos(grupo);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluirGrupo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        protected void btAbrirSubGrupo_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    string grupo = "";
                    grupo = gvMaterialGrupo.DataKeys[row.RowIndex].Value.ToString();

                    MATERIAIS_GRUPO mGrupo = desenvController.ObterMaterialGrupo(grupo);
                    if (mGrupo != null)
                    {
                        labCodigo.ForeColor = Color.Red;
                        txtCodigo.Text = mGrupo.CODIGO_GRUPO.Trim();
                        labGrupo.ForeColor = Color.Red;
                        txtGrupo.Text = mGrupo.GRUPO.Trim();
                        hidGrupo.Value = mGrupo.GRUPO.Trim();
                        txtGrupo.Enabled = false;
                        btCancelar.Visible = true;
                        btIncluirGrupo.Visible = false;

                        CarregarSubGrupos(grupo);
                    }
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            labCodigo.ForeColor = Color.Gray;
            txtCodigo.Text = "";
            txtCodigo.Enabled = true;
            labGrupo.ForeColor = Color.Gray;
            txtGrupo.Text = "";
            txtGrupo.Enabled = true;

            hidGrupo.Value = "";
            hidSubGrupo.Value = "";
            hidCodigoSubGrupo.Value = "";

            labErro.Text = "";
            btCancelar.Visible = false;
            btIncluirGrupo.Visible = true;

            fsSubGrupo.Visible = false;
            gvMaterialSubGrupo.EditIndex = -1;
            gvMaterialSubGrupo.DataSource = null;
            gvMaterialSubGrupo.DataBind();
        }
        protected void btExcluirGrupo_Click(object sender, EventArgs e)
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
                        string grupo = "";

                        grupo = gvMaterialGrupo.DataKeys[row.RowIndex].Value.ToString();

                        if (grupo != "")
                        {
                            ExcluirGrupo(grupo);
                            CarregarGrupos();
                            btCancelar_Click(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = ex.Message;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Este registro não pode ser excluído.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    }
                }
            }
        }

        //SUBGRUPO
        protected void gvMaterialSubGrupo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    MATERIAIS_SUBGRUPO _materialSubG = e.Row.DataItem as MATERIAIS_SUBGRUPO;

                    colunaSub += 1;
                    if (_materialSubG != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaSub.ToString();

                        TextBox _txtCodigoItem = e.Row.FindControl("txtCodigoItem") as TextBox;
                        if (_txtCodigoItem != null)
                            _txtCodigoItem.Text = _materialSubG.CODIGO_SUBGRUPO.Trim();

                        TextBox _txtSubGrupoItem = e.Row.FindControl("txtSubGrupoItem") as TextBox;
                        if (_txtSubGrupoItem != null)
                            _txtSubGrupoItem.Text = _materialSubG.SUBGRUPO.Trim();

                        DropDownList _ddlUnidadeMedida = e.Row.FindControl("ddlUnidadeMedidaItem") as DropDownList;
                        if (_ddlUnidadeMedida != null)
                        {
                            _ddlUnidadeMedida.DataSource = CarregarUnidadeLinx();
                            _ddlUnidadeMedida.DataBind();

                            _ddlUnidadeMedida.SelectedValue = _materialSubG.UNIDADE.UNIDADE1;
                        }

                        ImageButton btEditar = e.Row.FindControl("btEditar") as ImageButton;
                        ImageButton btSair = e.Row.FindControl("btSair") as ImageButton;
                        ImageButton btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                        btEditar.Visible = false;
                        btSair.Visible = false;
                        btExcluir.Visible = false;
                        if (_materialSubG.GRUPO.Trim() != "")
                        {
                            btExcluir.Visible = true;
                            if (e.Row.RowState.ToString().ToUpper().Contains("EDIT"))
                            {
                                btSair.Visible = true;
                            }
                            else
                            {
                                btEditar.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        protected void gvMaterialSubGrupo_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvMaterialSubGrupo.FooterRow;
            if (footer != null)
            {
                DropDownList _ddlUnidadeMedida = footer.FindControl("ddlUnidadeMedidaFooter") as DropDownList;
                if (_ddlUnidadeMedida != null)
                {
                    _ddlUnidadeMedida.DataSource = CarregarUnidadeLinx();
                    _ddlUnidadeMedida.DataBind();
                }
            }
        }

        protected void txtSubGrupo_TextChanged(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                TextBox txt = (TextBox)sender;
                if (txt != null)
                {
                    GridViewRow row = (GridViewRow)txt.NamingContainer;

                    TextBox _txtCodigoItem = row.FindControl("txtCodigoItem") as TextBox;
                    if (_txtCodigoItem != null && _txtCodigoItem.Text != "")
                    {
                        string valSubGrupo = ValidarSubGrupo(hidGrupo.Value, _txtCodigoItem.Text, "", false);
                        if (valSubGrupo != "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + valSubGrupo + "', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Informe o código do SubGrupo.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                        return;
                    }

                    TextBox _txtSubGrupoItem = row.FindControl("txtSubGrupoItem") as TextBox;
                    if (_txtSubGrupoItem == null || _txtSubGrupoItem.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Informe o SubGrupo.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                        return;
                    }

                    if (row != null)
                        Atualizar(row);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }
        }
        protected void ddlSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                DropDownList ddl = (DropDownList)sender;
                if (ddl != null)
                {
                    GridViewRow row = (GridViewRow)ddl.NamingContainer;
                    if (row != null)
                        Atualizar(row);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }
        }

        protected void btEditar_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    try
                    {
                        gvMaterialSubGrupo.EditIndex = row.RowIndex;
                        CarregarSubGrupos(hidGrupo.Value);

                        //Botão acionado é escondido
                        bt.Visible = false;

                        ImageButton btSair = row.FindControl("btSair") as ImageButton;
                        if (btSair != null)
                            btSair.Visible = true;

                        Label _labCodigoItem = row.FindControl("labCodigoItem") as Label;
                        Label _labSubGrupoItem = row.FindControl("labSubGrupoItem") as Label;
                        hidCodigoSubGrupo.Value = _labCodigoItem.Text;
                        hidSubGrupo.Value = _labSubGrupoItem.Text;
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btEditar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btSair_Click(object sender, EventArgs e)
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
                        gvMaterialSubGrupo.EditIndex = -1;
                        CarregarSubGrupos(hidGrupo.Value);

                        //Botão acionado é escondido
                        bt.Visible = false;

                        ImageButton btEditar = row.FindControl("btEditar") as ImageButton;
                        if (btEditar != null)
                            btEditar.Visible = true;

                        hidSubGrupo.Value = "";
                        hidCodigoSubGrupo.Value = "";
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btSair_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
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
                        Label _labSubGrupoItem = row.FindControl("labSubGrupoItem") as Label;

                        if (_labSubGrupoItem != null)
                        {
                            ExcluirSubGrupo(hidGrupo.Value, _labSubGrupoItem.Text);
                            CarregarSubGrupos(hidGrupo.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = ex.Message;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Este registro não pode ser excluído.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    }
                }
            }
        }

        protected void btIncluirSubGrupo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    try
                    {
                        string codigo = "";
                        string subGrupo = "";
                        string unidadeMedida = "";

                        TextBox _txtCodigoFooter = row.FindControl("txtCodigoFooter") as TextBox;
                        TextBox _txtSubGrupoFooter = row.FindControl("txtSubGrupoFooter") as TextBox;
                        DropDownList _ddlUnidadeMedidaFooter = row.FindControl("ddlUnidadeMedidaFooter") as DropDownList;

                        if (_txtCodigoFooter != null && _txtCodigoFooter.Text.Trim() != "")
                        {
                            codigo = _txtCodigoFooter.Text.Trim();
                            codigo = codigo.PadLeft(2, '0');
                            codigo = codigo.Substring((codigo.Length - 2), 2);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Informe o Código do SubGrupo.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (_txtSubGrupoFooter != null && _txtSubGrupoFooter.Text.Trim() != "")
                        {
                            subGrupo = _txtSubGrupoFooter.Text.Trim().ToUpper();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Informe o SubGrupo.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (_ddlUnidadeMedidaFooter != null && _ddlUnidadeMedidaFooter.SelectedValue != "")
                        {
                            unidadeMedida = _ddlUnidadeMedidaFooter.SelectedValue;
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Informe a Unidade de Medida.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        string valSubGrupo = ValidarSubGrupo(hidGrupo.Value, codigo, subGrupo, true);
                        if (valSubGrupo != "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + valSubGrupo + "', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        MATERIAIS_SUBGRUPO mSubGrupo = new MATERIAIS_SUBGRUPO();
                        mSubGrupo.GRUPO = hidGrupo.Value;
                        mSubGrupo.CODIGO_SUBGRUPO = codigo;
                        mSubGrupo.SUBGRUPO = subGrupo.ToUpper();
                        mSubGrupo.CODIGO_SEQUENCIAL = "0001";
                        mSubGrupo.UNIDADE_ESTOQUE = unidadeMedida;
                        mSubGrupo.FATOR_CONVERSAO = Convert.ToDecimal(1.000000);
                        mSubGrupo.UNIDADE_FICHA_TEC = unidadeMedida;
                        mSubGrupo.FASE_PRODUCAO = "01";
                        mSubGrupo.SETOR_PRODUCAO = "01";
                        mSubGrupo.MRP_PARTICIPANTE = 0;

                        Incluir(mSubGrupo);
                        CarregarSubGrupos(hidGrupo.Value);
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btEditar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }

        private void Atualizar(GridViewRow row)
        {
            string retorno = "";
            string grupo = "";
            string subGrupo = "";

            MATERIAIS_SUBGRUPO _materialSubGrupo = new MATERIAIS_SUBGRUPO();
            subGrupo = gvMaterialSubGrupo.DataKeys[row.RowIndex].Value.ToString();
            grupo = hidGrupo.Value;
            if (subGrupo.Trim() != "")
            {
                //Obter Material Sub Grupo
                _materialSubGrupo = desenvController.ObterMaterialSubGrupo(grupo, subGrupo);

                //produto.CODIGO = codigoProduto;
                retorno = ObterValorControle(row, "txtCodigoItem", typeof(TextBox));
                retorno = retorno.PadLeft(2, '0');
                _materialSubGrupo.CODIGO_SUBGRUPO = retorno.Substring((retorno.Length - 2), 2);
                _materialSubGrupo.SUBGRUPO = ObterValorControle(row, "txtSubGrupoItem", typeof(TextBox));
                _materialSubGrupo.UNIDADE_ESTOQUE = ObterValorControle(row, "ddlUnidadeMedidaItem", typeof(DropDownList));

                //Atualizar MATERIAL SUB GRUPO
                Editar(_materialSubGrupo, hidSubGrupo.Value);
            }
        }
        private string ObterValorControle(GridViewRow row, string id, Type tipo)
        {
            string retorno = "";

            if (tipo == typeof(TextBox))
            {
                TextBox txt = row.FindControl(id) as TextBox;
                if (txt != null)
                    retorno = txt.Text.Trim();
            }
            else if (tipo == typeof(DropDownList))
            {
                DropDownList ddl = row.FindControl(id) as DropDownList;
                if (ddl != null)
                    retorno = ddl.SelectedValue.Trim();
            }
            return retorno;
        }

        #endregion

        #region "ORDENACAO"
        protected void gvMaterialGrupo_Sorting(object sender, GridViewSortEventArgs e)
        {
            bool order;

            if ((bool)ViewState["sortDirection"])
                ViewState["sortDirection"] = false;
            else
                ViewState["sortDirection"] = true;

            order = (bool)ViewState["sortDirection"];

            GridView gv = (GridView)sender;
            if (gv != null)
            {
                gv.Columns[1].HeaderText = "Código";
                gv.Columns[2].HeaderText = "Grupo";

                if (e.SortExpression == "CodigoGrupo") gv.Columns[1].HeaderText = gv.Columns[1].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "Grupo") gv.Columns[2].HeaderText = gv.Columns[2].HeaderText + " " + ((order) ? " - >>" : " - <<");
            }

            FiltroGeralGrupo(e.SortExpression, order, false);
        }
        private void FiltroGeralGrupo(string pCampo, bool pAsc, bool pHeader)
        {
            if (Session["MATERIAL_GRUPOGRUPO"] != null)
            {
                _matGrupo.AddRange((List<MATERIAIS_GRUPO>)Session["MATERIAL_GRUPOGRUPO"]);

                //ORDENAÇÃO
                if (pAsc)
                {
                    if (pCampo == "CodigoGrupo") _matGrupo = _matGrupo.OrderBy(p => p.CODIGO_GRUPO).ToList();
                    if (pCampo == "Grupo") _matGrupo = _matGrupo.OrderBy(p => p.GRUPO).ToList();
                }
                else
                {
                    if (pCampo == "CodigoGrupo") _matGrupo = _matGrupo.OrderByDescending(p => p.CODIGO_GRUPO).ToList();
                    if (pCampo == "Grupo") _matGrupo = _matGrupo.OrderByDescending(p => p.GRUPO).ToList();
                }

                if (pHeader)
                {
                    gvMaterialSubGrupo.Columns[1].HeaderText = "Código";
                    gvMaterialSubGrupo.Columns[2].HeaderText = "Grupo";
                }
                gvMaterialGrupo.DataSource = _matGrupo;
                gvMaterialGrupo.DataBind();
            }
            else
            {
                CarregarGrupos();
            }
        }

        protected void gvMaterialSubGrupo_Sorting(object sender, GridViewSortEventArgs e)
        {
            bool order;

            if ((bool)ViewState["sortDirection"])
                ViewState["sortDirection"] = false;
            else
                ViewState["sortDirection"] = true;

            order = (bool)ViewState["sortDirection"];

            GridView gv = (GridView)sender;
            if (gv != null)
            {
                gv.Columns[2].HeaderText = "Código";
                gv.Columns[3].HeaderText = "SubGrupo";
                gv.Columns[4].HeaderText = "Unidade de Medida";

                if (e.SortExpression == "CodigoItem") gv.Columns[2].HeaderText = gv.Columns[2].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "SubGrupoItem") gv.Columns[3].HeaderText = gv.Columns[3].HeaderText + " " + ((order) ? " - >>" : " - <<");
            }

            FiltroGeralSubGrupo(e.SortExpression, order, false);
        }
        private void FiltroGeralSubGrupo(string pCampo, bool pAsc, bool pHeader)
        {
            if (Session["MATERIAL_GRUPOSUBGRUPO"] != null)
            {
                _matSubGrupo.AddRange((List<MATERIAIS_SUBGRUPO>)Session["MATERIAL_GRUPOSUBGRUPO"]);

                //ORDENAÇÃO
                if (pAsc)
                {
                    if (pCampo == "CodigoItem") _matSubGrupo = _matSubGrupo.OrderBy(p => p.CODIGO_SUBGRUPO).ToList();
                    if (pCampo == "SubGrupoItem") _matSubGrupo = _matSubGrupo.OrderBy(p => p.SUBGRUPO).ToList();
                }
                else
                {
                    if (pCampo == "CodigoItem") _matSubGrupo = _matSubGrupo.OrderByDescending(p => p.CODIGO_SUBGRUPO).ToList();
                    if (pCampo == "SubGrupoItem") _matSubGrupo = _matSubGrupo.OrderByDescending(p => p.SUBGRUPO).ToList();
                }

                if (pHeader)
                {
                    gvMaterialSubGrupo.Columns[2].HeaderText = "Código";
                    gvMaterialSubGrupo.Columns[3].HeaderText = "SubGrupo";
                }
                gvMaterialSubGrupo.DataSource = _matSubGrupo;
                gvMaterialSubGrupo.DataBind();
            }
            else
            {
                CarregarSubGrupos(txtGrupo.Text.Trim());
            }

        }
        #endregion

        #region "VALIDACAO"
        private string ValidarGrupo(string grupo, string codigoGrupo)
        {
            MATERIAIS_GRUPO valGrupo = null;
            valGrupo = desenvController.ObterMaterialGrupo(grupo);
            if (valGrupo != null)
                return "O Grupo informado já está cadastrado. Informe outro Grupo.";
            valGrupo = desenvController.ObterMaterialGrupo().Where(p => p.CODIGO_GRUPO == codigoGrupo).SingleOrDefault();
            if (valGrupo != null)
                return "O Código do Grupo informado já está cadastrado. Informe outro Código do Grupo.";
            return "";
        }
        private string ValidarSubGrupo(string grupo, string codigoSubGrupo, string subGrupo, bool insert)
        {
            MATERIAIS_SUBGRUPO valSubGrupo = null;
            valSubGrupo = desenvController.ObterMaterialSubGrupo(grupo, subGrupo);
            if (valSubGrupo != null)
                return "O SubGrupo informado já está cadastrado. Informe outro SubGrupo.";
            valSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo && p.CODIGO_SUBGRUPO == codigoSubGrupo).SingleOrDefault();
            if ((valSubGrupo != null && insert) || (valSubGrupo != null && !insert && codigoSubGrupo.Trim() != hidCodigoSubGrupo.Value.Trim()))
                return "O Código do SubGrupo informado já está cadastrado. Informe outro Código do SubGrupo.";
            return "";
        }
        #endregion
    }

}
