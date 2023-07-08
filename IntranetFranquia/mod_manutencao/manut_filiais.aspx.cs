using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using DAL;

namespace Relatorios.mod_manutencao
{
    public partial class manut_filiais : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();
                CarregarGvFiliais("0");
            }

            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            Page.MaintainScrollPositionOnPostBack = true;
        }

        private void CarregaDropDownListFilial()
        {
            List<FILIAI1> listFiliais = baseController.ListarFiliais();

            ddlFilial.DataSource = listFiliais;
            ddlFilial.DataBind();


            List<FILIAI1> listTiposFiliais = new List<FILIAI1>();

            var _listTiposFiliais = listFiliais.Where(p => p.TIPO_FILIAL != null).Where(q => q.TIPO_FILIAL.Trim() != "").Select(s => s.TIPO_FILIAL.Trim()).Distinct().ToList();
            foreach (var item in _listTiposFiliais)
                if (item.Trim() != "")
                    listTiposFiliais.Add(new FILIAI1 { TIPO_FILIAL = item.Trim() });

            listTiposFiliais = listTiposFiliais.OrderBy(p => p.TIPO_FILIAL).ToList();
            ddlTipoFilial.DataSource = listTiposFiliais;
            ddlTipoFilial.DataBind();
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Insert(0, new ListItem("Todos", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarGvFiliais(ddlFilial.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregarGvFiliais(string codFilial)
        {
            List<SP_OBTER_FILIAIS_CONFIGURACAOResult> lsFiliaisConfiguracao = new List<SP_OBTER_FILIAIS_CONFIGURACAOResult>();

            lsFiliaisConfiguracao = baseController.ListarFiliaisConfiguracao(codFilial);

            if (ddlTipoFilial.SelectedValue != "0")
            {
                lsFiliaisConfiguracao = lsFiliaisConfiguracao.Where(p => p.TIPO_FILIAL.Trim() == ddlTipoFilial.SelectedValue.Trim()).ToList(); 
            }

            gvFiliaisConfiguracao.DataSource = lsFiliaisConfiguracao;
            gvFiliaisConfiguracao.DataBind();
        }

        protected void gvFiliaisConfiguracao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvFiliaisConfiguracao.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].ColumnSpan = 3;
                footer.Cells[0].Text = gvFiliaisConfiguracao.Rows.Count.ToString().Trim() + "  Filiais Encontradas";
                footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            }
        }

        protected void gvFiliaisConfiguracao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.DataItemIndex != -1)
                {
                    LinkButton lnk = e.Row.FindControl("lnkSel") as LinkButton;
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(lnk, ""));
                    e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
                } 
                
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        CheckBox _chkAgoraRede = e.Row.FindControl("chkAgoraRede") as CheckBox;
                        HiddenField _hdnAgoraRede = e.Row.FindControl("hdnAgoraRede") as HiddenField;

                        CheckBox _chkAgoraSupervisores = e.Row.FindControl("chkAgoraSupervisores") as CheckBox;
                        HiddenField _hdnAgoraSupervisores = e.Row.FindControl("hdnAgoraSupervisores") as HiddenField;

                        CheckBox _chkAdministracao = e.Row.FindControl("chkAdministracao") as CheckBox;
                        HiddenField _hdnAdministracao = e.Row.FindControl("hdnAdministracao") as HiddenField;

                        CheckBox _chkGestao = e.Row.FindControl("chkGestao") as CheckBox;
                        HiddenField _hdnGestao = e.Row.FindControl("hdnGestao") as HiddenField;

                        CheckBox _chkAtacado = e.Row.FindControl("chkAtacado") as CheckBox;
                        HiddenField _hdnAtacado = e.Row.FindControl("hdnAtacado") as HiddenField;

                        CheckBox _chkFinanceiro = e.Row.FindControl("chkFinanceiro") as CheckBox;
                        HiddenField _hdnFinanceiro = e.Row.FindControl("hdnFinanceiro") as HiddenField;

                        CheckBox _chkContabilidade = e.Row.FindControl("chkContabilidade") as CheckBox;
                        HiddenField _hdnContabilidade = e.Row.FindControl("hdnContabilidade") as HiddenField;

                        CheckBox _chkFiscal = e.Row.FindControl("chkFiscal") as CheckBox;
                        HiddenField _hdnFiscal = e.Row.FindControl("hdnFiscal") as HiddenField;

                        CheckBox _chkAdmNotaFiscal = e.Row.FindControl("chkAdmNotaFiscal") as CheckBox;
                        HiddenField _hdnAdmNotaFiscala = e.Row.FindControl("hdnAdmNotaFiscala") as HiddenField;

                        CheckBox _chkAdministracaoLoja = e.Row.FindControl("chkAdministracaoLoja") as CheckBox;
                        HiddenField _hdnAdministracaoLoja = e.Row.FindControl("hdnAdministracaoLoja") as HiddenField;

                        CheckBox _chkGerenciamentoLoja = e.Row.FindControl("chkGerenciamentoLoja") as CheckBox;
                        HiddenField _hdnGerenciamentoLoja = e.Row.FindControl("hdnGerenciamentoLoja") as HiddenField;

                        CheckBox _chkAcompanhamentoMensal = e.Row.FindControl("chkAcompanhamentoMensal") as CheckBox;
                        HiddenField _hdnAcompanhamentoMensal = e.Row.FindControl("hdnAcompanhamentoMensal") as HiddenField;

                        CheckBox _chkControleEstoque = e.Row.FindControl("chkControleEstoque") as CheckBox;
                        HiddenField _hdnControleEstoque = e.Row.FindControl("hdnControleEstoque") as HiddenField;

                        CheckBox _chkContagem = e.Row.FindControl("chkContagem") as CheckBox;
                        HiddenField _hdnContagem = e.Row.FindControl("hdnContagem") as HiddenField;

                        CheckBox _chkRH = e.Row.FindControl("chkRH") as CheckBox;
                        HiddenField _hdnRH = e.Row.FindControl("hdnRH") as HiddenField;

                        CheckBox _chkRepresentante = e.Row.FindControl("chkRepresentante") as CheckBox;
                        HiddenField _hdnRepresentante = e.Row.FindControl("hdnRepresentante") as HiddenField;

                        if (_hdnAgoraRede != null && _chkAgoraRede != null)
                        {
                            if (_hdnAgoraRede.Value.ToString() == "S")
                            {
                                _chkAgoraRede.Checked = true;
                            }
                            else
                            {
                                _chkAgoraRede.Checked = false;
                            }
                        }

                        if (_hdnAgoraSupervisores != null && _chkAgoraSupervisores != null)
                        {
                            if (_hdnAgoraSupervisores.Value.ToString() == "S")
                            {
                                _chkAgoraSupervisores.Checked = true;
                            }
                            else
                            {
                                _chkAgoraSupervisores.Checked = false;
                            }
                        }

                        if (_hdnAdministracao != null && _chkAdministracao != null)
                        {
                            if (_hdnAdministracao.Value.ToString() == "S")
                            {
                                _chkAdministracao.Checked = true;
                            }
                            else
                            {
                                _chkAdministracao.Checked = false;
                            }
                        }

                        if (_hdnGestao != null && _chkGestao != null)
                        {
                            if (_hdnGestao.Value.ToString() == "S")
                            {
                                _chkGestao.Checked = true;
                            }
                            else
                            {
                                _chkGestao.Checked = false;
                            }
                        }

                        if (_hdnAtacado != null && _chkAtacado != null)
                        {
                            if (_hdnAtacado.Value.ToString() == "S")
                            {
                                _chkAtacado.Checked = true;
                            }
                            else
                            {
                                _chkAtacado.Checked = false;
                            }
                        }

                        if (_hdnFinanceiro != null && _chkFinanceiro != null)
                        {
                            if (_hdnFinanceiro.Value.ToString() == "S")
                            {
                                _chkFinanceiro.Checked = true;
                            }
                            else
                            {
                                _chkFinanceiro.Checked = false;
                            }
                        }

                        if (_hdnContabilidade != null && _chkContabilidade != null)
                        {
                            if (_hdnContabilidade.Value.ToString() == "S")
                            {
                                _chkContabilidade.Checked = true;
                            }
                            else
                            {
                                _chkContabilidade.Checked = false;
                            }
                        }

                        if (_hdnFiscal != null && _chkFiscal != null)
                        {
                            if (_hdnFiscal.Value.ToString() == "S")
                            {
                                _chkFiscal.Checked = true;
                            }
                            else
                            {
                                _chkFiscal.Checked = false;
                            }
                        }

                        if (_hdnAdmNotaFiscala != null && _chkAdmNotaFiscal != null)
                        {
                            if (_hdnAdmNotaFiscala.Value.ToString() == "S")
                            {
                                _chkAdmNotaFiscal.Checked = true;
                            }
                            else
                            {
                                _chkAdmNotaFiscal.Checked = false;
                            }
                        }

                        if (_hdnAdministracaoLoja != null && _chkAdministracaoLoja != null)
                        {
                            if (_hdnAdministracaoLoja.Value.ToString() == "S")
                            {
                                _chkAdministracaoLoja.Checked = true;
                            }
                            else
                            {
                                _chkAdministracaoLoja.Checked = false;
                            }
                        }

                        if (_hdnGerenciamentoLoja != null && _chkGerenciamentoLoja != null)
                        {
                            if (_hdnGerenciamentoLoja.Value.ToString() == "S")
                            {
                                _chkGerenciamentoLoja.Checked = true;
                            }
                            else
                            {
                                _chkGerenciamentoLoja.Checked = false;
                            }
                        }

                        if (_hdnAcompanhamentoMensal != null && _chkAcompanhamentoMensal != null)
                        {
                            if (_hdnAcompanhamentoMensal.Value.ToString() == "S")
                            {
                                _chkAcompanhamentoMensal.Checked = true;
                            }
                            else
                            {
                                _chkAcompanhamentoMensal.Checked = false;
                            }
                        }

                        if (_hdnControleEstoque != null && _chkControleEstoque != null)
                        {
                            if (_hdnControleEstoque.Value.ToString() == "S")
                            {
                                _chkControleEstoque.Checked = true;
                            }
                            else
                            {
                                _chkControleEstoque.Checked = false;
                            }
                        }

                        if (_hdnContagem != null && _chkContagem != null)
                        {
                            if (_hdnContagem.Value.ToString() == "S")
                            {
                                _chkContagem.Checked = true;
                            }
                            else
                            {
                                _chkContagem.Checked = false;
                            }
                        }

                        if (_hdnRH != null && _chkRH != null)
                        {
                            if (_hdnRH.Value.ToString() == "S")
                            {
                                _chkRH.Checked = true;
                            }
                            else
                            {
                                _chkRH.Checked = false;
                            }
                        }

                        if (_hdnRepresentante != null && _chkRepresentante != null)
                        {
                            if (_hdnRepresentante.Value.ToString() == "S")
                            {
                                _chkRepresentante.Checked = true;
                            }
                            else
                            {
                                _chkRepresentante.Checked = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void btnFecharMensagem_Click(object sender, EventArgs e)
        {
            txtMensagemErro.Text = "";
            panMensagem.Visible = false;
            panPrincipal.Enabled = true;
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach(GridViewRow linha in gvFiliaisConfiguracao.Rows)
                {
                    FILIAIS_CONFIGURACAO config = new FILIAIS_CONFIGURACAO();

                    HiddenField _hdnCodigo = (HiddenField)linha.FindControl("hdnCodigo");
                    HiddenField _hdnFilial = (HiddenField)linha.FindControl("hdnFilial");

                    TextBox _txtPastaImagens = (TextBox)linha.FindControl("txtPastaImagens");

                    CheckBox _chkAgoraRede = (CheckBox)linha.FindControl("chkAgoraRede");
                    HiddenField _hdnAgoraRede = (HiddenField)linha.FindControl("hdnAgoraRede");

                    CheckBox _chkAgoraSupervisores = (CheckBox)linha.FindControl("chkAgoraSupervisores");
                    HiddenField _hdnAgoraSupervisores = (HiddenField)linha.FindControl("hdnAgoraSupervisores");

                    CheckBox _chkAdministracao = (CheckBox)linha.FindControl("chkAdministracao");
                    HiddenField _hdnAdministracao = (HiddenField)linha.FindControl("hdnAdministracao");

                    CheckBox _chkGestao = (CheckBox)linha.FindControl("chkGestao");
                    HiddenField _hdnGestao = (HiddenField)linha.FindControl("hdnGestao");

                    CheckBox _chkAtacado = (CheckBox)linha.FindControl("chkAtacado");
                    HiddenField _hdnAtacado = (HiddenField)linha.FindControl("hdnAtacado");

                    CheckBox _chkFinanceiro = (CheckBox)linha.FindControl("chkFinanceiro");
                    HiddenField _hdnFinanceiro = (HiddenField)linha.FindControl("hdnFinanceiro");

                    CheckBox _chkContabilidade = (CheckBox)linha.FindControl("chkContabilidade");
                    HiddenField _hdnContabilidade = (HiddenField)linha.FindControl("hdnContabilidade");

                    CheckBox _chkFiscal = (CheckBox)linha.FindControl("chkFiscal");
                    HiddenField _hdnFiscal = (HiddenField)linha.FindControl("hdnFiscal");

                    CheckBox _chkAdmNotaFiscal = (CheckBox)linha.FindControl("chkAdmNotaFiscal");
                    HiddenField _hdnAdmNotaFiscala = (HiddenField)linha.FindControl("hdnAdmNotaFiscala");

                    CheckBox _chkAdministracaoLoja = (CheckBox)linha.FindControl("chkAdministracaoLoja");
                    HiddenField _hdnAdministracaoLoja = (HiddenField)linha.FindControl("hdnAdministracaoLoja");

                    CheckBox _chkGerenciamentoLoja = (CheckBox)linha.FindControl("chkGerenciamentoLoja");
                    HiddenField _hdnGerenciamentoLoja = (HiddenField)linha.FindControl("hdnGerenciamentoLoja");

                    CheckBox _chkAcompanhamentoMensal = (CheckBox)linha.FindControl("chkAcompanhamentoMensal");
                    HiddenField _hdnAcompanhamentoMensal = (HiddenField)linha.FindControl("hdnAcompanhamentoMensal");

                    CheckBox _chkControleEstoque = (CheckBox)linha.FindControl("chkControleEstoque");
                    HiddenField _hdnControleEstoque = (HiddenField)linha.FindControl("hdnControleEstoque");

                    CheckBox _chkContagem = (CheckBox)linha.FindControl("chkContagem");
                    HiddenField _hdnContagem = (HiddenField)linha.FindControl("hdnContagem");

                    CheckBox _chkRH = (CheckBox)linha.FindControl("chkRH");
                    HiddenField _hdnRH = (HiddenField)linha.FindControl("hdnRH");

                    CheckBox _chkRepresentante = (CheckBox)linha.FindControl("chkRepresentante");
                    HiddenField _hdnRepresentante = (HiddenField)linha.FindControl("hdnRepresentante");


                    if (_hdnFilial != null)
                    {
                        config.CODIGO_FILIAL = _hdnFilial.Value.ToString();
                    }

                    if (_txtPastaImagens != null)
                    {
                        config.PASTA_IMAGENS_DEPOSITO = _txtPastaImagens.Text;
                    }

                    if (_hdnAgoraRede != null && _chkAgoraRede != null)
                    {
                        if (_chkAgoraRede.Checked == true)
                        {
                            config.AGORA_REDE = 'S';
                        }
                        else
                        {
                            config.AGORA_REDE = 'N';
                        }
                    }

                    if (_hdnAgoraSupervisores != null && _chkAgoraSupervisores != null)
                    {
                        if (_chkAgoraSupervisores.Checked == true)
                        {
                            config.AGORA_SUPERVISORES = 'S';
                        }
                        else
                        {
                            config.AGORA_SUPERVISORES = 'N';
                        }
                    }

                    if (_hdnAdministracao != null && _chkAdministracao != null)
                    {
                        if (_chkAdministracao.Checked == true)
                        {
                            config.MOD_ADMINISTRACAO = 'S';
                        }
                        else
                        {
                            config.MOD_ADMINISTRACAO = 'N';
                        }
                    }

                    if (_hdnGestao != null && _chkGestao != null)
                    {
                        if (_chkGestao.Checked == true)
                        {
                            config.MOD_GESTAO = 'S';
                        }
                        else
                        {
                            config.MOD_GESTAO = 'N';
                        }
                    }

                    if (_hdnAtacado != null && _chkAtacado != null)
                    {
                        if (_chkAtacado.Checked == true)
                        {
                            config.MOD_ATACADO = 'S';
                        }
                        else
                        {
                            config.MOD_ATACADO = 'N';
                        }
                    }

                    if (_hdnFinanceiro != null && _chkFinanceiro != null)
                    {
                        if (_chkFinanceiro.Checked == true)
                        {
                            config.MOD_FINANCEIRO = 'S';
                        }
                        else
                        {
                            config.MOD_FINANCEIRO = 'N';
                        }
                    }

                    if (_hdnContabilidade != null && _chkContabilidade != null)
                    {
                        if (_chkContabilidade.Checked == true)
                        {
                            config.MOD_CONTABILIDADE = 'S';
                        }
                        else
                        {
                            config.MOD_CONTABILIDADE = 'N';
                        }
                    }

                    if (_hdnFiscal != null && _chkFiscal != null)
                    {
                        if (_chkFiscal.Checked == true)
                        {
                            config.MOD_FISCAL = 'S';
                        }
                        else
                        {
                            config.MOD_FISCAL = 'N';
                        }
                    }

                    if (_hdnAdmNotaFiscala != null && _chkAdmNotaFiscal != null)
                    {
                        if (_chkAdmNotaFiscal.Checked == true)
                        {
                            config.MOD_ADM_NOTA_FISCAL = 'S';
                        }
                        else
                        {
                            config.MOD_ADM_NOTA_FISCAL = 'N';
                        }
                    }

                    if (_hdnAdministracaoLoja != null && _chkAdministracaoLoja != null)
                    {
                        if (_chkAdministracaoLoja.Checked == true)
                        {
                            config.MOD_ADMINISTRACAO_LOJA = 'S';
                        }
                        else
                        {
                            config.MOD_ADMINISTRACAO_LOJA = 'N';
                        }
                    }

                    if (_hdnGerenciamentoLoja != null && _chkGerenciamentoLoja != null)
                    {
                        if (_chkGerenciamentoLoja.Checked == true)
                        {
                            config.MOD_GERENCIAMENTO_LOJA = 'S';
                        }
                        else
                        {
                            config.MOD_GERENCIAMENTO_LOJA = 'N';
                        }
                    }

                    if (_hdnAcompanhamentoMensal != null && _chkAcompanhamentoMensal != null)
                    {
                        if (_chkAcompanhamentoMensal.Checked == true)
                        {
                            config.MOD_ACOMPANHAMENTO_MENSAL = 'S';
                        }
                        else
                        {
                            config.MOD_ACOMPANHAMENTO_MENSAL = 'N';
                        }
                    }

                    if (_hdnControleEstoque != null && _chkControleEstoque != null)
                    {
                        if (_chkControleEstoque.Checked == true)
                        {
                            config.MOD_CONTROLE_ESTOQUE = 'S';
                        }
                        else
                        {
                            config.MOD_CONTROLE_ESTOQUE = 'N';
                        }
                    }

                    if (_hdnContagem != null && _chkContagem != null)
                    {
                        if (_chkContagem.Checked == true)
                        {
                            config.MOD_CONTAGEM = 'S';
                        }
                        else
                        {
                            config.MOD_CONTAGEM = 'N';
                        }
                    }

                    if (_hdnRH != null && _chkRH != null)
                    {
                        if (_chkRH.Checked == true)
                        {
                            config.MOD_RH = 'S';
                        }
                        else
                        {
                            config.MOD_RH = 'N';
                        }
                    }

                    if (_hdnRepresentante != null && _chkRepresentante != null)
                    {
                        if (_chkRepresentante.Checked == true)
                        {
                            config.MOD_REPRESENTANTE = 'S';
                        }
                        else
                        {
                            config.MOD_REPRESENTANTE = 'N';
                        }
                    }

                    if (Convert.ToInt32(_hdnCodigo.Value) == 0)
                    {
                        baseController.IncluirFiliaisConfiguracao(config);
                    }
                    else
                    {
                        baseController.AtualizarFiliaisConfiguracao(config);
                    }
                }

                CarregarGvFiliais(ddlFilial.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void marcarRow(GridViewRow dgItem)
        {
            dgItem.BorderWidth = 2;
            dgItem.BorderColor = Color.DarkBlue;
        }

        private void desmarcarRow(Int32 iLinha)
        {
            foreach (GridViewRow gvItem in gvFiliaisConfiguracao.Rows)
            {
                if (gvItem.RowIndex != iLinha)
                {
                    gvItem.BorderWidth = 0;
                    gvItem.BorderColor = Color.Empty;
                }
            }
        }

        protected void chkAgoraRede_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkAgoraSupervisores_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkAdministracao_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkGestao_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkAtacado_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkFinanceiro_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkContabilidade_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkFiscal_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkAdmNotaFiscal_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkAdministracaoLoja_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkGerenciamentoLoja_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkAcompanhamentoMensal_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkControleEstoque_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkContagem_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkRH_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void chkRepresentante_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }

        protected void ddlTipoFilial_DataBound(object sender, EventArgs e)
        {
            ddlTipoFilial.Items.Insert(0, new ListItem("Todos", "0"));
            ddlTipoFilial.SelectedValue = "0";
         }
    }
}