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
    public partial class admloj_cad_vitrine_analise : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "admloj_menu.aspx";

                CarregarFilial();
                CarregarSemanas();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        //private void CarregarFilial()
        //{
        //    List<FILIAI> filial = new List<FILIAI>();
        //    List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

        //    var usuario = (USUARIO)Session["USUARIO"];

        //    if (usuario.CODIGO_PERFIL == 3 || usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 51 || usuario.CODIGO_PERFIL == 50)
        //        filial = baseController.BuscaFiliais();
        //    else
        //        filial = baseController.BuscaFiliais(usuario);

        //    filialDePara = baseController.BuscaFilialDePara();
        //    filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
        //    if (filial != null)
        //    {
        //        filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
        //        ddlFilial.DataSource = filial;
        //        ddlFilial.DataBind();

        //        ddlFilial.Enabled = true;
        //        if (filial.Count() == 2)
        //        {
        //            ddlFilial.SelectedIndex = 1;
        //            ddlFilial.Enabled = false;
        //        }
        //    }
        //}
        private void CarregarFilial()
        {

            var filial = baseController.BuscaFiliais_Agora();

            filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
            ddlFilial.DataSource = filial;
            ddlFilial.DataBind();

            if (filial.Count() == 2)
            {
                ddlFilial.SelectedIndex = 1;
                ddlFilial.Enabled = false;
            }

        }
        private void CarregarSemanas()
        {
            var semana = lojaController.ObterSemanaVenda();

            semana.Insert(0, new LOJA_VENDA_SEMANA { CODIGO = 0, SEMANA = "Selecione" });

            ddlSemana.DataSource = semana;
            ddlSemana.DataBind();
        }

        #endregion


        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlSemana.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Semana...";
                    return;
                }

                CarregarVitrineFilial();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private List<SP_OBTER_VITRINE_FILIALResult> ObterVitrineFilial()
        {
            char? favorito = null;
            char? sugestao = null;

            var semanaSel = lojaController.ObterSemanaVenda(Convert.ToInt32(ddlSemana.SelectedValue));

            if (ddlFavorito.SelectedValue != "")
                favorito = Convert.ToChar(ddlFavorito.SelectedValue);

            if (ddlSugestao.SelectedValue != "")
                sugestao = Convert.ToChar(ddlSugestao.SelectedValue);

            var vitrine = lojaController.ObterVitrineFilial(semanaSel.DATA_INI, semanaSel.DATA_FIM, ddlFilial.SelectedValue.Trim(), favorito, sugestao);


            if (ddlFechado.SelectedValue != "")
            {
                if (ddlFechado.SelectedValue == "S")
                    vitrine = vitrine.Where(p => p.DATA_FINALIZADO != null).ToList();
                else
                    vitrine = vitrine.Where(p => p.DATA_FINALIZADO == null).ToList();
            }

            if (ddlAnalisado.SelectedValue != "")
            {
                if (ddlAnalisado.SelectedValue == "S")
                    vitrine = vitrine.Where(p => p.DATA_ANALISADO != null).ToList();
                else
                    vitrine = vitrine.Where(p => p.DATA_ANALISADO == null).ToList();
            }

            return vitrine;
        }
        private void CarregarVitrineFilial()
        {
            var vitrine = ObterVitrineFilial();

            repVitrine.DataSource = vitrine;
            repVitrine.DataBind();
        }


        protected void repVitrine_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SP_OBTER_VITRINE_FILIALResult vitrine = e.Item.DataItem as SP_OBTER_VITRINE_FILIALResult;

            if (vitrine != null)
            {
                var codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL;
                var hab = (codigoPerfil == 2) ? false : true;

                Literal litFilial = e.Item.FindControl("litFilial") as Literal;
                litFilial.Text = vitrine.FILIAL;

                ImageButton btFilialAnalise = e.Item.FindControl("btFilialAnalise") as ImageButton;
                btFilialAnalise.CommandArgument = vitrine.CODIGO_VITRINE.ToString();
                if (vitrine.DATA_ANALISADO == null)
                    btFilialAnalise.ImageUrl = "~/Image/disapprove.png";
                else
                    btFilialAnalise.ImageUrl = "~/Image/approve.png";
                btFilialAnalise.Enabled = hab;

                Label labFemininoFilial = e.Item.FindControl("labFemininoFilial") as Label;
                labFemininoFilial.Text = "FEMININO - " + vitrine.FILIAL;
                Label labMasculinoFilial = e.Item.FindControl("labMasculinoFilial") as Label;
                labMasculinoFilial.Text = "MASCULINO - " + vitrine.FILIAL;
                Label labPortaFilial = e.Item.FindControl("labPortaFilial") as Label;
                labPortaFilial.Text = "PORTA - " + vitrine.FILIAL;
                Label labExtraFilial = e.Item.FindControl("labExtraFilial") as Label;
                labExtraFilial.Text = "EXTRA - " + vitrine.FILIAL;

                if (vitrine.CODIGO_VITRINE > 0)
                {
                    CarregarMundoFeminino(vitrine, hab, e);
                    CarregarMundoMasculino(vitrine, hab, e);
                    CarregarMundoPorta(vitrine, hab, e);
                    CarregarMundoExtra(vitrine, hab, e);
                }

            }
        }
        private void CarregarMundoFeminino(SP_OBTER_VITRINE_FILIALResult vitrine, bool hab, RepeaterItemEventArgs e)
        {
            var mundoFeminino = lojaController.ObterVitrineMundoPorVitrineETipo(vitrine.CODIGO_VITRINE, 1);
            if (mundoFeminino != null)
            {

                var fotos = lojaController.ObterVitrineMundoFotoPorMundo(mundoFeminino.CODIGO);

                foreach (var f in fotos)
                {
                    if (f.ORDEM == 0)
                    {
                        ImageButton btFemininoPrincipalFav = e.Item.FindControl("btFemininoPrincipalFav") as ImageButton;
                        btFemininoPrincipalFav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btFemininoPrincipalFav.CommandArgument = f.CODIGO.ToString();
                        btFemininoPrincipalFav.Enabled = hab;

                        ImageButton btFemininoPrincipal = e.Item.FindControl("btFemininoPrincipal") as ImageButton;
                        btFemininoPrincipal.ImageUrl = f.FOTO;
                        btFemininoPrincipal.CommandArgument = f.CODIGO.ToString();
                        //btFemininoPrincipal.Enabled = hab;

                        ImageButton btFemininoPrincipalHarmonia = e.Item.FindControl("btFemininoPrincipalHarmonia") as ImageButton;
                        MarcarQualificacao(btFemininoPrincipalHarmonia, f, hab);
                        ImageButton btFemininoPrincipalPosicao = e.Item.FindControl("btFemininoPrincipalPosicao") as ImageButton;
                        MarcarQualificacao(btFemininoPrincipalPosicao, f, hab);

                        ImageButton btFemininoPrincipalObs = e.Item.FindControl("btFemininoPrincipalObs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btFemininoPrincipalObs.Visible = true;

                    }
                    else if (f.ORDEM == 1)
                    {
                        ImageButton btFemininoMundo1Fav = e.Item.FindControl("btFemininoMundo1Fav") as ImageButton;
                        btFemininoMundo1Fav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btFemininoMundo1Fav.CommandArgument = f.CODIGO.ToString();
                        btFemininoMundo1Fav.Enabled = hab;

                        ImageButton btFemininoMundo1 = e.Item.FindControl("btFemininoMundo1") as ImageButton;
                        btFemininoMundo1.ImageUrl = f.FOTO;
                        btFemininoMundo1.CommandArgument = f.CODIGO.ToString();
                        //btFemininoMundo1.Enabled = hab;

                        ImageButton btFemininoMundo1Harmonia = e.Item.FindControl("btFemininoMundo1Harmonia") as ImageButton;
                        MarcarQualificacao(btFemininoMundo1Harmonia, f, hab);
                        ImageButton btFemininoMundo1Posicao = e.Item.FindControl("btFemininoMundo1Posicao") as ImageButton;
                        MarcarQualificacao(btFemininoMundo1Posicao, f, hab);
                        ImageButton btFemininoMundo1Montagem = e.Item.FindControl("btFemininoMundo1Montagem") as ImageButton;
                        MarcarQualificacao(btFemininoMundo1Montagem, f, hab);
                        ImageButton btFemininoMundo1Cubos = e.Item.FindControl("btFemininoMundo1Cubos") as ImageButton;
                        MarcarQualificacao(btFemininoMundo1Cubos, f, hab);
                        ImageButton btFemininoMundo1Acessorios = e.Item.FindControl("btFemininoMundo1Acessorios") as ImageButton;
                        MarcarQualificacao(btFemininoMundo1Acessorios, f, hab);

                        ImageButton btFemininoMundo1Obs = e.Item.FindControl("btFemininoMundo1Obs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btFemininoMundo1Obs.Visible = true;


                    }
                    else if (f.ORDEM == 2)
                    {
                        ImageButton btFemininoMundo2Fav = e.Item.FindControl("btFemininoMundo2Fav") as ImageButton;
                        btFemininoMundo2Fav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btFemininoMundo2Fav.CommandArgument = f.CODIGO.ToString();
                        btFemininoMundo2Fav.Enabled = hab;

                        ImageButton btFemininoMundo2 = e.Item.FindControl("btFemininoMundo2") as ImageButton;
                        btFemininoMundo2.ImageUrl = f.FOTO;
                        btFemininoMundo2.CommandArgument = f.CODIGO.ToString();
                        //btFemininoMundo2.Enabled = hab;

                        ImageButton btFemininoMundo2Harmonia = e.Item.FindControl("btFemininoMundo2Harmonia") as ImageButton;
                        MarcarQualificacao(btFemininoMundo2Harmonia, f, hab);
                        ImageButton btFemininoMundo2Posicao = e.Item.FindControl("btFemininoMundo2Posicao") as ImageButton;
                        MarcarQualificacao(btFemininoMundo2Posicao, f, hab);
                        ImageButton btFemininoMundo2Montagem = e.Item.FindControl("btFemininoMundo2Montagem") as ImageButton;
                        MarcarQualificacao(btFemininoMundo2Montagem, f, hab);
                        ImageButton btFemininoMundo2Cubos = e.Item.FindControl("btFemininoMundo2Cubos") as ImageButton;
                        MarcarQualificacao(btFemininoMundo2Cubos, f, hab);
                        ImageButton btFemininoMundo2Acessorios = e.Item.FindControl("btFemininoMundo2Acessorios") as ImageButton;
                        MarcarQualificacao(btFemininoMundo2Acessorios, f, hab);

                        ImageButton btFemininoMundo2Obs = e.Item.FindControl("btFemininoMundo2Obs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btFemininoMundo2Obs.Visible = true;
                    }
                    else if (f.ORDEM == 3)
                    {
                        ImageButton btFemininoMundo3Fav = e.Item.FindControl("btFemininoMundo3Fav") as ImageButton;
                        btFemininoMundo3Fav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btFemininoMundo3Fav.CommandArgument = f.CODIGO.ToString();
                        btFemininoMundo3Fav.Enabled = hab;

                        ImageButton btFemininoMundo3 = e.Item.FindControl("btFemininoMundo3") as ImageButton;
                        btFemininoMundo3.ImageUrl = f.FOTO;
                        btFemininoMundo3.CommandArgument = f.CODIGO.ToString();
                        //btFemininoMundo3.Enabled = hab;

                        ImageButton btFemininoMundo3Harmonia = e.Item.FindControl("btFemininoMundo3Harmonia") as ImageButton;
                        MarcarQualificacao(btFemininoMundo3Harmonia, f, hab);
                        ImageButton btFemininoMundo3Posicao = e.Item.FindControl("btFemininoMundo3Posicao") as ImageButton;
                        MarcarQualificacao(btFemininoMundo3Posicao, f, hab);
                        ImageButton btFemininoMundo3Montagem = e.Item.FindControl("btFemininoMundo3Montagem") as ImageButton;
                        MarcarQualificacao(btFemininoMundo3Montagem, f, hab);
                        ImageButton btFemininoMundo3Cubos = e.Item.FindControl("btFemininoMundo3Cubos") as ImageButton;
                        MarcarQualificacao(btFemininoMundo3Cubos, f, hab);
                        ImageButton btFemininoMundo3Acessorios = e.Item.FindControl("btFemininoMundo3Acessorios") as ImageButton;
                        MarcarQualificacao(btFemininoMundo3Acessorios, f, hab);

                        ImageButton btFemininoMundo3Obs = e.Item.FindControl("btFemininoMundo3Obs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btFemininoMundo3Obs.Visible = true;
                    }
                }
            }
        }
        private void CarregarMundoMasculino(SP_OBTER_VITRINE_FILIALResult vitrine, bool hab, RepeaterItemEventArgs e)
        {
            var mundoMasculino = lojaController.ObterVitrineMundoPorVitrineETipo(vitrine.CODIGO_VITRINE, 2);
            if (mundoMasculino != null)
            {

                var fotos = lojaController.ObterVitrineMundoFotoPorMundo(mundoMasculino.CODIGO);

                foreach (var f in fotos)
                {
                    if (f.ORDEM == 0)
                    {
                        ImageButton btMasculinoPrincipalFav = e.Item.FindControl("btMasculinoPrincipalFav") as ImageButton;
                        btMasculinoPrincipalFav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btMasculinoPrincipalFav.CommandArgument = f.CODIGO.ToString();
                        btMasculinoPrincipalFav.Enabled = hab;

                        ImageButton btMasculinoPrincipal = e.Item.FindControl("btMasculinoPrincipal") as ImageButton;
                        btMasculinoPrincipal.ImageUrl = f.FOTO;
                        btMasculinoPrincipal.CommandArgument = f.CODIGO.ToString();
                        //btMasculinoPrincipal.Enabled = hab;

                        ImageButton btMasculinoPrincipalHarmonia = e.Item.FindControl("btMasculinoPrincipalHarmonia") as ImageButton;
                        MarcarQualificacao(btMasculinoPrincipalHarmonia, f, hab);
                        ImageButton btMasculinoPrincipalPosicao = e.Item.FindControl("btMasculinoPrincipalPosicao") as ImageButton;
                        MarcarQualificacao(btMasculinoPrincipalPosicao, f, hab);

                        ImageButton btMasculinoPrincipalObs = e.Item.FindControl("btMasculinoPrincipalObs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btMasculinoPrincipalObs.Visible = true;

                    }
                    else if (f.ORDEM == 1)
                    {
                        ImageButton btMasculinoMundo1Fav = e.Item.FindControl("btMasculinoMundo1Fav") as ImageButton;
                        btMasculinoMundo1Fav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btMasculinoMundo1Fav.CommandArgument = f.CODIGO.ToString();
                        btMasculinoMundo1Fav.Enabled = hab;

                        ImageButton btMasculinoMundo1 = e.Item.FindControl("btMasculinoMundo1") as ImageButton;
                        btMasculinoMundo1.ImageUrl = f.FOTO;
                        btMasculinoMundo1.CommandArgument = f.CODIGO.ToString();
                        //btMasculinoMundo1.Enabled = hab;

                        ImageButton btMasculinoMundo1Harmonia = e.Item.FindControl("btMasculinoMundo1Harmonia") as ImageButton;
                        MarcarQualificacao(btMasculinoMundo1Harmonia, f, hab);
                        ImageButton btMasculinoMundo1Posicao = e.Item.FindControl("btMasculinoMundo1Posicao") as ImageButton;
                        MarcarQualificacao(btMasculinoMundo1Posicao, f, hab);
                        ImageButton btMasculinoMundo1Montagem = e.Item.FindControl("btMasculinoMundo1Montagem") as ImageButton;
                        MarcarQualificacao(btMasculinoMundo1Montagem, f, hab);
                        ImageButton btMasculinoMundo1Cubos = e.Item.FindControl("btMasculinoMundo1Cubos") as ImageButton;
                        MarcarQualificacao(btMasculinoMundo1Cubos, f, hab);
                        ImageButton btMasculinoMundo1Acessorios = e.Item.FindControl("btMasculinoMundo1Acessorios") as ImageButton;
                        MarcarQualificacao(btMasculinoMundo1Acessorios, f, hab);


                        ImageButton btMasculinoMundo1Obs = e.Item.FindControl("btMasculinoMundo1Obs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btMasculinoMundo1Obs.Visible = true;

                    }
                    else if (f.ORDEM == 2)
                    {
                        ImageButton btMasculinoMundo2Fav = e.Item.FindControl("btMasculinoMundo2Fav") as ImageButton;
                        btMasculinoMundo2Fav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btMasculinoMundo2Fav.CommandArgument = f.CODIGO.ToString();
                        btMasculinoMundo2Fav.Enabled = hab;

                        ImageButton btMasculinoMundo2 = e.Item.FindControl("btMasculinoMundo2") as ImageButton;
                        btMasculinoMundo2.ImageUrl = f.FOTO;
                        btMasculinoMundo2.CommandArgument = f.CODIGO.ToString();
                        //btMasculinoMundo2.Enabled = hab;

                        ImageButton btMasculinoMundo2Harmonia = e.Item.FindControl("btMasculinoMundo2Harmonia") as ImageButton;
                        MarcarQualificacao(btMasculinoMundo2Harmonia, f, hab);
                        ImageButton btMasculinoMundo2Posicao = e.Item.FindControl("btMasculinoMundo2Posicao") as ImageButton;
                        MarcarQualificacao(btMasculinoMundo2Posicao, f, hab);
                        ImageButton btMasculinoMundo2Montagem = e.Item.FindControl("btMasculinoMundo2Montagem") as ImageButton;
                        MarcarQualificacao(btMasculinoMundo2Montagem, f, hab);
                        ImageButton btMasculinoMundo2Cubos = e.Item.FindControl("btMasculinoMundo2Cubos") as ImageButton;
                        MarcarQualificacao(btMasculinoMundo2Cubos, f, hab);
                        ImageButton btMasculinoMundo2Acessorios = e.Item.FindControl("btMasculinoMundo2Acessorios") as ImageButton;
                        MarcarQualificacao(btMasculinoMundo2Acessorios, f, hab);

                        ImageButton btMasculinoMundo2Obs = e.Item.FindControl("btMasculinoMundo2Obs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btMasculinoMundo2Obs.Visible = true;
                    }

                }
            }
        }
        private void CarregarMundoPorta(SP_OBTER_VITRINE_FILIALResult vitrine, bool hab, RepeaterItemEventArgs e)
        {
            var mundoPorta = lojaController.ObterVitrineMundoPorVitrineETipo(vitrine.CODIGO_VITRINE, 3);
            if (mundoPorta != null)
            {
                var fotos = lojaController.ObterVitrineMundoFotoPorMundo(mundoPorta.CODIGO);

                foreach (var f in fotos)
                {
                    if (f.ORDEM == 0)
                    {
                        ImageButton btPortaPrincipalFav = e.Item.FindControl("btPortaPrincipalFav") as ImageButton;
                        btPortaPrincipalFav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btPortaPrincipalFav.CommandArgument = f.CODIGO.ToString();
                        btPortaPrincipalFav.Enabled = hab;

                        ImageButton btPortaPrincipal = e.Item.FindControl("btPortaPrincipal") as ImageButton;
                        btPortaPrincipal.ImageUrl = f.FOTO;
                        btPortaPrincipal.CommandArgument = f.CODIGO.ToString();
                        //btPortaPrincipal.Enabled = hab;

                        ImageButton btPortaPrincipalHarmonia = e.Item.FindControl("btPortaPrincipalHarmonia") as ImageButton;
                        MarcarQualificacao(btPortaPrincipalHarmonia, f, hab);
                        ImageButton btPortaPrincipalPosicao = e.Item.FindControl("btPortaPrincipalPosicao") as ImageButton;
                        MarcarQualificacao(btPortaPrincipalPosicao, f, hab);

                        ImageButton btPortaPrincipalObs = e.Item.FindControl("btPortaPrincipalObs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btPortaPrincipalObs.Visible = true;
                    }


                }
            }
        }
        private void CarregarMundoExtra(SP_OBTER_VITRINE_FILIALResult vitrine, bool hab, RepeaterItemEventArgs e)
        {
            var mundoExtra = lojaController.ObterVitrineMundoPorVitrineETipo(vitrine.CODIGO_VITRINE, 4);
            if (mundoExtra != null)
            {

                var fotos = lojaController.ObterVitrineMundoFotoPorMundo(mundoExtra.CODIGO);

                foreach (var f in fotos)
                {
                    if (f.ORDEM == 0)
                    {
                        ImageButton btExtraPrincipalFav = e.Item.FindControl("btExtraPrincipalFav") as ImageButton;
                        btExtraPrincipalFav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btExtraPrincipalFav.CommandArgument = f.CODIGO.ToString();
                        btExtraPrincipalFav.Enabled = hab;

                        ImageButton btExtraPrincipal = e.Item.FindControl("btExtraPrincipal") as ImageButton;
                        btExtraPrincipal.ImageUrl = f.FOTO;
                        btExtraPrincipal.CommandArgument = f.CODIGO.ToString();
                        //btExtraPrincipal.Enabled = hab;

                        ImageButton btExtraPrincipalHarmonia = e.Item.FindControl("btExtraPrincipalHarmonia") as ImageButton;
                        MarcarQualificacao(btExtraPrincipalHarmonia, f, hab);
                        ImageButton btExtraPrincipalPosicao = e.Item.FindControl("btExtraPrincipalPosicao") as ImageButton;
                        MarcarQualificacao(btExtraPrincipalPosicao, f, hab);

                        ImageButton btExtraPrincipalObs = e.Item.FindControl("btExtraPrincipalObs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btExtraPrincipalObs.Visible = true;


                    }
                    else if (f.ORDEM == 1)
                    {
                        ImageButton btExtraMundo1Fav = e.Item.FindControl("btExtraMundo1Fav") as ImageButton;
                        btExtraMundo1Fav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btExtraMundo1Fav.CommandArgument = f.CODIGO.ToString();
                        btExtraMundo1Fav.Enabled = hab;

                        ImageButton btExtraMundo1 = e.Item.FindControl("btExtraMundo1") as ImageButton;
                        btExtraMundo1.ImageUrl = f.FOTO;
                        btExtraMundo1.CommandArgument = f.CODIGO.ToString();
                        //btExtraMundo1.Enabled = hab;

                        ImageButton btExtraMundo1Harmonia = e.Item.FindControl("btExtraMundo1Harmonia") as ImageButton;
                        MarcarQualificacao(btExtraMundo1Harmonia, f, hab);
                        ImageButton btExtraMundo1Posicao = e.Item.FindControl("btExtraMundo1Posicao") as ImageButton;
                        MarcarQualificacao(btExtraMundo1Posicao, f, hab);
                        ImageButton btExtraMundo1Montagem = e.Item.FindControl("btExtraMundo1Montagem") as ImageButton;
                        MarcarQualificacao(btExtraMundo1Montagem, f, hab);
                        ImageButton btExtraMundo1Cubos = e.Item.FindControl("btExtraMundo1Cubos") as ImageButton;
                        MarcarQualificacao(btExtraMundo1Cubos, f, hab);
                        ImageButton btExtraMundo1Acessorios = e.Item.FindControl("btExtraMundo1Acessorios") as ImageButton;
                        MarcarQualificacao(btExtraMundo1Acessorios, f, hab);

                        ImageButton btExtraMundo1Obs = e.Item.FindControl("btExtraMundo1Obs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btExtraMundo1Obs.Visible = true;

                    }
                    else if (f.ORDEM == 2)
                    {
                        ImageButton btExtraMundo2Fav = e.Item.FindControl("btExtraMundo2Fav") as ImageButton;
                        btExtraMundo2Fav.ImageUrl = (f.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        btExtraMundo2Fav.CommandArgument = f.CODIGO.ToString();
                        btExtraMundo2Fav.Enabled = hab;

                        ImageButton btExtraMundo2 = e.Item.FindControl("btExtraMundo2") as ImageButton;
                        btExtraMundo2.ImageUrl = f.FOTO;
                        btExtraMundo2.CommandArgument = f.CODIGO.ToString();
                        //btExtraMundo2.Enabled = hab;

                        ImageButton btExtraMundo2Harmonia = e.Item.FindControl("btExtraMundo2Harmonia") as ImageButton;
                        MarcarQualificacao(btExtraMundo2Harmonia, f, hab);
                        ImageButton btExtraMundo2Posicao = e.Item.FindControl("btExtraMundo2Posicao") as ImageButton;
                        MarcarQualificacao(btExtraMundo2Posicao, f, hab);
                        ImageButton btExtraMundo2Montagem = e.Item.FindControl("btExtraMundo2Montagem") as ImageButton;
                        MarcarQualificacao(btExtraMundo2Montagem, f, hab);
                        ImageButton btExtraMundo2Cubos = e.Item.FindControl("btExtraMundo2Cubos") as ImageButton;
                        MarcarQualificacao(btExtraMundo2Cubos, f, hab);
                        ImageButton btExtraMundo2Acessorios = e.Item.FindControl("btExtraMundo2Acessorios") as ImageButton;
                        MarcarQualificacao(btExtraMundo2Acessorios, f, hab);

                        ImageButton btExtraMundo2Obs = e.Item.FindControl("btExtraMundo2Obs") as ImageButton;
                        if (f.OBSERVACAO != null && f.OBSERVACAO.Trim() != "")
                            btExtraMundo2Obs.Visible = true;

                    }

                }
            }
        }

        private void MarcarQualificacao(ImageButton imgButton, LOJA_VITRINE_MUNDO_FOTO f, bool hab)
        {

            if (f.DATA_QUALIFICACAO != null)
            {
                if (imgButton.ID.ToLower().Contains("harmonia"))
                    imgButton.ImageUrl = (f.HARMONIA == 'S') ? "~/Image/approve.png" : "~/Image/delete.png";
                if (imgButton.ID.ToLower().Contains("posicao"))
                    imgButton.ImageUrl = (f.POSICAO == 'S') ? "~/Image/approve.png" : "~/Image/delete.png";
                if (imgButton.ID.ToLower().Contains("montagem"))
                    imgButton.ImageUrl = (f.MONTAGEM == 'S') ? "~/Image/approve.png" : "~/Image/delete.png";
                if (imgButton.ID.ToLower().Contains("cubos"))
                    imgButton.ImageUrl = (f.CUBOS == 'S') ? "~/Image/approve.png" : "~/Image/delete.png";
                if (imgButton.ID.ToLower().Contains("acessorios"))
                    imgButton.ImageUrl = (f.ACESSORIOS == 'S') ? "~/Image/approve.png" : "~/Image/delete.png";

                imgButton.Enabled = hab;

            }
        }

        protected void btZoomFoto_Click(object sender, ImageClickEventArgs e)
        {
            string url = "";

            try
            {
                //labErro.Text = "";

                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    if (bt.CommandArgument != "")
                    {
                        var codigoVitrineMundoFoto = Convert.ToInt32(bt.CommandArgument);

                        //Abrir pop-up
                        url = "fnAbrirTelaCadastroMaior2('admloj_cad_vitrine_analise_qual.aspx?fot=" + codigoVitrineMundoFoto + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                    }
                }


            }
            catch (Exception ex)
            {
                //labErro.Text = ex.Message;
            }
        }
        protected void btFavorito_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                //labErro.Text = "";

                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    if (bt.CommandArgument != "")
                    {
                        var codigoVitrineMundoFoto = Convert.ToInt32(bt.CommandArgument);

                        var vitrineFoto = lojaController.ObterVitrineMundoFotoPorCodigo(codigoVitrineMundoFoto);
                        if (vitrineFoto != null)
                        {
                            vitrineFoto.FAVORITO = (vitrineFoto.FAVORITO == 'S') ? 'N' : 'S';
                            lojaController.AtualizarVitrineMundoFoto(vitrineFoto);

                            bt.ImageUrl = (vitrineFoto.FAVORITO == 'S') ? "~/Image/favorito.png" : "~/Image/favorito_n.png";
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                //labErro.Text = ex.Message;
            }
        }

        protected void btFilialAnalise_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //labErro.Text = "";

                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    if (bt.CommandArgument != "")
                    {
                        var codigoVitrine = Convert.ToInt32(bt.CommandArgument);

                        var vitrine = lojaController.ObterVitrinePorCodigo(codigoVitrine);
                        if (vitrine != null)
                        {
                            vitrine.DATA_ANALISADO = DateTime.Now;
                            lojaController.AtualizarVitrine(vitrine);

                            bt.ImageUrl = "~/Image/approve.png";
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                //labErro.Text = ex.Message;
            }
        }
        protected void btFilialAtualizar_Click(object sender, ImageClickEventArgs e)
        {
            btBuscar_Click(null, null);
        }

    }
}
