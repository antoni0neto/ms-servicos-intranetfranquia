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
    public partial class rh_cad_feriado : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtData.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                CarregarFilial();

                //Carregar gv gvFeriado
                CarregarFeriados();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarFeriados()
        {
            List<RH_FERIADO> _feriado = new List<RH_FERIADO>();

            try
            {
                _feriado = rhController.ObterFeriado().OrderBy(p => p.DATA_INCLUSAO).ThenBy(o => o.DESCRICAO).ToList();
                if (_feriado != null)
                {
                    gvFeriado.DataSource = _feriado;
                    gvFeriado.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (gvFeriado): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labDescricao.ForeColor = Color.Black;
            labData.ForeColor = Color.Black;
            labFilial.ForeColor = Color.Black;
            labTipo.ForeColor = Color.Black;
            labStatus.ForeColor = Color.Black;

            btCancelar.Visible = false;
            txtDescricao.Text = "";
            txtData.Text = "";
            ddlFilial.SelectedValue = "";
            ddlTipo.SelectedValue = "";
            ddlTrabalhado.SelectedValue = "";
            ddlStatus.SelectedValue = "A";

            CarregarFeriados();
        }
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = new BaseController().BuscaFiliais_Intermediario(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA" || p.TIPO_FILIAL.Trim() == "INATIVA").ToList();
                //lstFilial = baseController.BuscaFiliais();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                    }
                }
            }
        }
        #endregion

        #region "CRUD"
        private void Incluir(RH_FERIADO _feriado)
        {
            rhController.InserirFeriado(_feriado);
        }
        private void Editar(RH_FERIADO _feriado)
        {
            rhController.AtualizarFeriado(_feriado);
        }
        private void Excluir(int _codigo)
        {
            rhController.ExcluirFeriado(_codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            labErro.Text = "";
            if (txtDescricao.Text.Trim() == "")
            {
                labErro.Text = "Informe o nome do Feriado.";
                return;
            }

            if (txtData.Text.Trim() == "")
            {
                labErro.Text = "Informe a Data do Feriado.";
                return;
            }

            if (ddlTipo.SelectedValue.Trim() == "")
            {
                labErro.Text = "Selecione o Tipo do Feriado.";
                return;
            }

            if (ddlTipo.SelectedValue.Trim() == "L" && ddlFilial.SelectedValue == "")
            {
                labErro.Text = "Informe a Filial para o Tipo de Feriado Local.";
                return;
            }

            if (ddlTipo.SelectedValue.Trim() == "N" && ddlFilial.SelectedValue != "")
            {
                labErro.Text = "Não é possível incluir um Feriado Nacional com Filial selecionada.";
                return;
            }

            if (ddlTrabalhado.SelectedValue.Trim() == "")
            {
                labErro.Text = "Selecione se o Feriado é Trabalhado ou NÃO.";
                return;
            }

            if (ddlStatus.SelectedValue.Trim() == "0")
            {
                labErro.Text = "Selecione o Status.";
                return;
            }

            try
            {
                _Inclusao = true;
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                RH_FERIADO _novo = new RH_FERIADO();
                _novo.DESCRICAO = txtDescricao.Text.Trim().ToUpper();
                _novo.DATA = Convert.ToDateTime(txtData.Text.Trim());
                if (ddlFilial.SelectedValue != "")
                    _novo.CODIGO_FILIAL = ddlFilial.SelectedValue;
                _novo.TIPO = Convert.ToChar(ddlTipo.SelectedValue);
                _novo.DATA_INCLUSAO = DateTime.Now;
                _novo.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _novo.STATUS = Convert.ToChar(ddlStatus.Text);
                _novo.TRABALHADO = Convert.ToChar(ddlTrabalhado.SelectedValue);

                if (_Inclusao)
                {
                    Incluir(_novo);
                }
                else
                {
                    _novo.CODIGO = Convert.ToInt32(hidCodigo.Value);
                    Editar(_novo);
                }

                RecarregarTela();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    RH_FERIADO _feriado = rhController.ObterFeriado(Convert.ToInt32(b.CommandArgument));
                    if (_feriado != null)
                    {

                        hidCodigo.Value = _feriado.CODIGO.ToString();

                        txtDescricao.Text = _feriado.DESCRICAO.Trim().ToUpper();
                        txtData.Text = _feriado.DATA.ToString("dd/MM/yyyy");
                        if (_feriado.CODIGO_FILIAL != null)
                            ddlFilial.SelectedValue = _feriado.CODIGO_FILIAL;
                        else
                            ddlFilial.SelectedValue = "";
                        ddlTipo.SelectedValue = _feriado.TIPO.ToString();
                        ddlTrabalhado.SelectedValue = _feriado.TRABALHADO.ToString();
                        ddlStatus.SelectedValue = _feriado.STATUS.ToString();

                        labDescricao.ForeColor = Color.Red;
                        labData.ForeColor = Color.Red;
                        labFilial.ForeColor = Color.Red;
                        labTipo.ForeColor = Color.Red;
                        labTrabalhado.ForeColor = Color.Red;
                        labStatus.ForeColor = Color.Red;
                        btCancelar.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btAlterar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {

            Button b = (Button)sender;
            if (b != null)
            {
                labErro.Text = "";
                try
                {
                    try
                    {
                        Excluir(Convert.ToInt32(b.CommandArgument));
                        RecarregarTela();
                    }
                    catch (Exception)
                    {
                        labErro.Text = "O Feriado não pode ser excluído. Este Feriado foi utilizado.";
                    }

                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion
        protected void gvFeriado_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    RH_FERIADO _feriado = e.Row.DataItem as RH_FERIADO;

                    coluna += 1;
                    if (_feriado != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = _feriado.DATA.ToString("dd/MM/yyyy");

                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            if (_feriado.CODIGO_FILIAL != null)
                                _litFilial.Text = new BaseController().ObterFilialIntranet(_feriado.CODIGO_FILIAL).FILIAL.Trim();
                            else
                                _litFilial.Text = "-";

                        Literal _litTipo = e.Row.FindControl("litTipo") as Literal;
                        if (_litTipo != null)
                            _litTipo.Text = (_feriado.TIPO == 'N') ? "Nacional" : "Local";

                        Literal _litTrabalhado = e.Row.FindControl("litTrabalhado") as Literal;
                        if (_litTrabalhado != null)
                            _litTrabalhado.Text = (_feriado.TRABALHADO == 'N') ? "Não" : "Sim";

                        Literal _status = e.Row.FindControl("litStatus") as Literal;
                        if (_status != null)
                        {
                            if (_feriado.STATUS == 'A')
                                _status.Text = "Ativo";
                            else
                                _status.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _feriado.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _feriado.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
