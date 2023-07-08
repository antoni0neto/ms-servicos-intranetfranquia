using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;

namespace Relatorios
{
    public partial class prod_cad_aviamento : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //Carregar combo de unidade e gv de aviamento
                CarregarUnidadeMedida();
                CarregarAviamento();

                //Controle de Controles
                btCancelar.Visible = false;
                pnlErro.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarUnidadeMedida()
        {
            List<UNIDADE_MEDIDA> _unidadeMedida = prodController.ObterUnidadeMedida();

            _unidadeMedida.Add(new UNIDADE_MEDIDA { CODIGO = 0, DESCRICAO = "Selecione", STATUS = 'A' });

            _unidadeMedida = _unidadeMedida.OrderBy(l => l.CODIGO).ToList();

            if (_unidadeMedida != null)
            {
                ddlUnidade.DataSource = _unidadeMedida;
                ddlUnidade.DataBind();
            }
        }
        private void CarregarAviamento()
        {
            List<PROD_AVIAMENTO> _aviamento = new List<PROD_AVIAMENTO>();

            try
            {
                _aviamento = prodController.ObterAviamento();
                if (_aviamento != null)
                {
                    gvAviamento.DataSource = _aviamento;
                    gvAviamento.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (CarregarAviamento): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                pnlErro.Visible = true;
                pnlErro.Height = Unit.Pixel(100);
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labDescricao.ForeColor = Color.Black;
            labUnidadeMedida.ForeColor = Color.Black;
            labStatus.ForeColor = Color.Black;
            btCancelar.Visible = false;
            txtDescricao.Text = "";
            ddlUnidade.SelectedValue = "0";
            ddlStatus.SelectedValue = "A";
            pnlErro.Visible = false;

            CarregarAviamento();
        }

        #endregion

        #region "CRUD"

        private void Incluir(PROD_AVIAMENTO _aviamento)
        {
            prodController.InserirAviamento(_aviamento);
        }
        private void Editar(PROD_AVIAMENTO _aviamento, string _codigo)
        {
            _aviamento.CODIGO = Convert.ToInt32(_codigo);
            prodController.AtualizarAviamento(_aviamento);
        }
        private void Excluir(int _Codigo)
        {
            prodController.ExcluirAviamento(_Codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            pnlErro.Visible = false;
            if (txtDescricao.Text.Trim() == "")
            {
                labErro.Text = "Informe a descrição do Aviamento.";
                pnlErro.Visible = true;
                return;
            }
            if (ddlUnidade.Text == "0")
            {
                labErro.Text = "Selecione a Unidade de Medida.";
                pnlErro.Visible = true;
                return;
            }
            if (ddlStatus.Text == "0")
            {
                labErro.Text = "Selecione o Status.";
                pnlErro.Visible = true;
                return;
            }

            try
            {
                _Inclusao = true;
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                PROD_AVIAMENTO _novo = new PROD_AVIAMENTO();

                _novo.DESCRICAO = txtDescricao.Text.Trim().ToUpper();
                _novo.UNIDADE_MEDIDA = Convert.ToInt32(ddlUnidade.SelectedValue);
                _novo.STATUS = Convert.ToChar(ddlStatus.Text);
                _novo.DATA_INCLUSAO = DateTime.Now;
                _novo.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                if (_Inclusao)
                    Incluir(_novo);
                else
                    Editar(_novo, hidCodigo.Value);

                RecarregarTela();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                pnlErro.Visible = true;
                pnlErro.Height = Unit.Pixel(100);
            }
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            pnlErro.Visible = false;

            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    PROD_AVIAMENTO _aviamento = prodController.ObterAviamento(Convert.ToInt32(b.CommandArgument));
                    if (_aviamento != null)
                    {
                        hidCodigo.Value = _aviamento.CODIGO.ToString();
                        txtDescricao.Text = _aviamento.DESCRICAO.Trim().ToUpper();
                        ddlUnidade.SelectedValue = _aviamento.UNIDADE_MEDIDA.ToString();
                        ddlStatus.SelectedValue = _aviamento.STATUS.ToString();
                        labDescricao.ForeColor = Color.Red;
                        labUnidadeMedida.ForeColor = Color.Red;
                        labStatus.ForeColor = Color.Red;
                        btCancelar.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btAlterar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    pnlErro.Visible = true;
                    pnlErro.Height = Unit.Pixel(100);
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            pnlErro.Visible = false;
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    //Validar exclusão
                    if (prodController.ObterAviamentoProdHB(Convert.ToInt32(b.CommandArgument)))
                    {
                        labErro.Text = "O Aviamento não pode ser excluído. Este aviamento já está cadastrado em um HB.";
                        pnlErro.Visible = true;
                        return;
                    }

                    Excluir(Convert.ToInt32(b.CommandArgument));
                    RecarregarTela();
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    pnlErro.Visible = true;
                    pnlErro.Height = Unit.Pixel(100);
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion
        protected void gvAviamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_AVIAMENTO _aviamento = e.Row.DataItem as PROD_AVIAMENTO;

                    coluna += 1;
                    if (_aviamento != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _unidadeMedida = e.Row.FindControl("litUnidadeMedida") as Literal;
                        if (_unidadeMedida != null)
                        {
                            string _uMDescricao = prodController.ObterUnidadeMedida(_aviamento.UNIDADE_MEDIDA).DESCRICAO;
                            if (_uMDescricao != "")
                                _unidadeMedida.Text = _uMDescricao;
                        }

                        Literal _status = e.Row.FindControl("litStatus") as Literal;
                        if (_status != null)
                        {
                            if (_aviamento.STATUS == 'A')
                                _status.Text = "Ativo";
                            else
                                _status.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                        {
                            _btAlerar.CommandArgument = _aviamento.CODIGO.ToString();
                        }

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _aviamento.CODIGO.ToString();
                        }

                    }
                }
            }
        }
    }
}
