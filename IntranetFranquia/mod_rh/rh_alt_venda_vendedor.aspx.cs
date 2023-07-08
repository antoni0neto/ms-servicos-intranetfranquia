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

namespace Relatorios
{
    public partial class rh_alt_venda_vendedor : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataVenda.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {

                string codigoFilial = Request.QueryString["c"].ToString();
                string dataVenda = Request.QueryString["d"].ToString().Replace("@", "/");
                string ticket = Request.QueryString["tic"].ToString();

                CarregarFilial(codigoFilial);
                txtDataVenda.Text = dataVenda;
                txtTicket.Text = ticket;
                btBuscar_Click(btBuscar, null);

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial(string codigoFilial)
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = baseController.BuscaFiliais_Intermediario(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA" || p.TIPO_FILIAL.Trim() == "INATIVA").ToList();

                var filialDePara = baseController.BuscaFilialDePara();
                if (lstFilial.Count > 0)
                {
                    lstFilial = lstFilial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                        ddlFilial.SelectedIndex = 1;

                    if (codigoFilial != "")
                        ddlFilial.SelectedValue = baseController.BuscaFilialCodigo(Convert.ToInt32(codigoFilial)).COD_FILIAL;
                }
            }
        }
        private List<SP_OBTER_FUNCIONARIOResult> CarregarFuncionario(string filial)
        {
            int codigoPerfil = 0;
            USUARIO usuario = (USUARIO)Session["USUARIO"];
            List<SP_OBTER_FUNCIONARIOResult> funcionario = new List<SP_OBTER_FUNCIONARIOResult>();

            if (usuario != null)
            {
                codigoPerfil = usuario.CODIGO_PERFIL;

                if (codigoPerfil == 1 || codigoPerfil == 11)
                    filial = "";

                funcionario = rhController.ObterFuncionario(null, "", filial, "", "").ToList().Select(c => new SP_OBTER_FUNCIONARIOResult()
                {
                    CODIGO = c.CODIGO,
                    VENDEDOR = c.VENDEDOR,
                    NOME = string.Format("{0} - {1}", c.NOME.Trim(), c.VENDEDOR.Trim()),
                    VENDEDOR_APELIDO = c.NOME //USADO APENAS PARA ORDENACAO
                }).Distinct().OrderBy(c => c.VENDEDOR_APELIDO).ToList(); ;
            }

            funcionario.Insert(0, new SP_OBTER_FUNCIONARIOResult { CODIGO = 0, NOME = "Selecione" });
            return funcionario;
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue.Trim() == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            labDataVenda.ForeColor = _OK;
            if (txtDataVenda.Text.Trim() == "")
            {
                labDataVenda.ForeColor = _notOK;
                retorno = false;
            }

            labTicket.ForeColor = _OK;
            if (txtTicket.Text.Trim() == "")
            {
                labTicket.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarData(string strHora)
        {
            try
            {
                if (strHora.Length == 5)
                    strHora = "1900-01-01 " + strHora;

                Convert.ToDateTime(strHora);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labMsg.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                if (!ValidarData(txtDataVenda.Text.Trim()))
                {
                    labErro.Text = "Informe a Data Venda Válida.";
                    return;
                }

                var lojaVenda = new BaseController().BuscaTicketLojaVenda(ddlFilial.SelectedValue, Convert.ToDateTime(txtDataVenda.Text), txtTicket.Text);
                if (lojaVenda == null || lojaVenda.Count() <= 0)
                {
                    labErro.Text = "Nenhum Ticket encontrado. Refaça sua pesquisa.";
                    return;
                }

                gvTicket.DataSource = lojaVenda;
                gvTicket.DataBind();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        protected void gvTicket_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    LOJA_VENDA lojaVenda = e.Row.DataItem as LOJA_VENDA;

                    if (lojaVenda != null)
                    {
                        Button _btAlterar = e.Row.FindControl("btAlterar") as Button;
                        DropDownList _ddlVendedor = e.Row.FindControl("ddlVendedor") as DropDownList;

                        Literal _litVendedor = e.Row.FindControl("litVendedor") as Literal;
                        if (_litVendedor != null)
                        {
                            var vendedorAtivo = baseController.BuscaVendedorPorCodigoEAtivo(lojaVenda.VENDEDOR);

                            if (vendedorAtivo != null)
                            {
                                _litVendedor.Text = vendedorAtivo.VENDEDOR.Trim() + " - " + vendedorAtivo.NOME_VENDEDOR.Trim();
                            }
                            else
                            {
                                var vendedor = baseController.BuscaVendedorPorCodigo(lojaVenda.VENDEDOR);

                                _litVendedor.Text = String.Format("<span style='color:red'>VENDEDOR {0} NÃO LOCALIZADO OU DESATIVADO</span>", vendedor.VENDEDOR_APELIDO.Trim());

                                _ddlVendedor.Enabled = false;
                                _btAlterar.Enabled = false;
                            }
                        }

                        Literal _litDataVenda = e.Row.FindControl("litDataVenda") as Literal;
                        if (_litDataVenda != null)
                            _litDataVenda.Text = lojaVenda.DATA_VENDA.ToString("dd/MM/yyyy");

                        Literal _litValor = e.Row.FindControl("litValor") as Literal;
                        if (_litValor != null)
                            _litValor.Text = Convert.ToDecimal(lojaVenda.VALOR_TIKET).ToString("###,###,###,##0.00");

                        if (_ddlVendedor != null)
                        {
                            _ddlVendedor.DataSource = CarregarFuncionario("");
                            _ddlVendedor.DataBind();
                        }

                    }
                }
            }
        }

        protected void btAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                if (bt != null)
                {

                    labErro.Text = "";
                    labMsg.Text = "";

                    string codigoFilial = ddlFilial.SelectedValue;
                    DateTime dataVenda = Convert.ToDateTime(txtDataVenda.Text);
                    string ticket = txtTicket.Text.Trim();

                    GridViewRow row = ((GridViewRow)bt.NamingContainer);

                    if (row != null)
                    {
                        string vendedor = gvTicket.DataKeys[row.RowIndex].Value.ToString();
                        string vendedorPara = ((DropDownList)row.FindControl("ddlVendedor")).SelectedValue.Trim();

                        if (vendedorPara == "0" || vendedorPara == "" || vendedorPara == "Selecione")
                        {
                            labErro.Text = "Selecione para qual Vendedor irá o Ticket.";
                            return;
                        }

                        baseController.AtualizarTicketLojaVenda(codigoFilial, dataVenda, ticket, vendedorPara);

                        var tlv = baseController.BuscaTicketLojaVendaVendedores(codigoFilial, dataVenda, ticket, vendedor).SingleOrDefault();
                        LOJA_VENDA_VENDEDORE _novo = new LOJA_VENDA_VENDEDORE();
                        _novo.CODIGO_FILIAL = tlv.CODIGO_FILIAL;
                        _novo.TICKET = tlv.TICKET;
                        _novo.DATA_VENDA = tlv.DATA_VENDA;
                        _novo.ID_VENDEDOR = tlv.ID_VENDEDOR;
                        _novo.VENDEDOR = vendedorPara;
                        _novo.COMISSAO = tlv.COMISSAO;
                        _novo.DATA_PARA_TRANSFERENCIA = DateTime.Now;
                        _novo.TIPO_VENDEDOR = tlv.TIPO_VENDEDOR;
                        _novo.CODIGO_CLIENTE = tlv.CODIGO_CLIENTE;
                        _novo.ACESSO_GERENCIAL = tlv.ACESSO_GERENCIAL;
                        _novo.AUSENTE = tlv.AUSENTE;

                        baseController.ExcluirTicketLojaVendaVendedores(_novo.CODIGO_FILIAL, _novo.DATA_VENDA, _novo.TICKET, _novo.VENDEDOR);

                        baseController.InserirTicketLojaVendaVendedores(_novo);

                        baseController.ExcluirTicketLojaVendaVendedores(codigoFilial, dataVenda, ticket, vendedor);

                        btBuscar_Click(null, null);

                        labMsg.Text = "Ticket alterado com sucesso.";
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
    }
}

