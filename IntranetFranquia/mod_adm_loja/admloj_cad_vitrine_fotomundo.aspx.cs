using DAL;
using Relatorios.mod_ecom.mag;
using Relatorios.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class admloj_cad_vitrine_fotomundo : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["codf"] == null || Request.QueryString["codf"] == "" ||
                    Request.QueryString["codv"] == null || Request.QueryString["codv"] == "" ||
                    Request.QueryString["di"] == null || Request.QueryString["di"] == "" ||
                    Request.QueryString["df"] == null || Request.QueryString["df"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                var codigoFilial = Request.QueryString["codf"].ToString();
                var codigoVitrine = Request.QueryString["codv"].ToString();
                var dataIni = Request.QueryString["di"].ToString();
                var dataFim = Request.QueryString["df"].ToString();

                hidCodigoFilial.Value = codigoFilial;
                hidCodigoVitrine.Value = codigoVitrine;
                hidDataIni.Value = dataIni;
                hidDataFim.Value = dataFim;

                var vitrine = lojaController.ObterVitrinePorFilialEData(codigoFilial, Convert.ToDateTime(dataIni), Convert.ToDateTime(dataFim));
                if (vitrine == null)
                {
                    //Inicializar fotos
                    var codigo = lojaController.GerarVitrineMundos(Convert.ToDateTime(dataIni), Convert.ToDateTime(dataFim), codigoFilial).OK;
                    hidCodigoVitrine.Value = codigo.ToString();
                }

                CarregarMundoFeminino(Convert.ToInt32(hidCodigoVitrine.Value));
                CarregarMundoMasculino(Convert.ToInt32(hidCodigoVitrine.Value));
                CarregarMundoPorta(Convert.ToInt32(hidCodigoVitrine.Value));
                CarregarMundoExtra(Convert.ToInt32(hidCodigoVitrine.Value));

                var codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL;
                var hab = (codigoPerfil == 2) ? false : true;
                HabilitarEdicao(hab);

                var filial = baseController.BuscaFilialCodigo(Convert.ToInt32(codigoFilial)).FILIAL.Trim();
                txtFilial.Text = filial;

            }

            //Evitar duplo clique no botão
            //btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");

        }

        private void CarregarMundoFeminino(int codigoVitrine)
        {
            var mundoFeminino = lojaController.ObterVitrineMundoPorVitrineETipo(codigoVitrine, 1);
            if (mundoFeminino != null)
            {
                hidCodigoVitrineFeminino.Value = mundoFeminino.CODIGO.ToString();

                var fotos = lojaController.ObterVitrineMundoFotoPorMundo(mundoFeminino.CODIGO);

                foreach (var f in fotos)
                {
                    if (f.ORDEM == 0)
                    {
                        btFemininoPrincipal.ImageUrl = f.FOTO;
                        imgEFem0.CommandArgument = f.CODIGO.ToString();
                        imgEFem0SemFoto.CommandArgument = f.CODIGO.ToString();
                    }
                    else if (f.ORDEM == 1)
                    {
                        btFemininoMundo1.ImageUrl = f.FOTO;
                        imgEFem1.CommandArgument = f.CODIGO.ToString();
                        imgEFem2SemFoto.CommandArgument = f.CODIGO.ToString();
                    }
                    else if (f.ORDEM == 2)
                    {
                        btFemininoMundo2.ImageUrl = f.FOTO;
                        imgEFem2.CommandArgument = f.CODIGO.ToString();
                        imgEFem2SemFoto.CommandArgument = f.CODIGO.ToString();
                    }
                    else if (f.ORDEM == 3)
                    {
                        btFemininoMundo3.ImageUrl = f.FOTO;
                        imgEFem3.CommandArgument = f.CODIGO.ToString();
                        imgEFem3SemFoto.CommandArgument = f.CODIGO.ToString();
                    }
                }
            }
        }
        private void CarregarMundoMasculino(int codigoVitrine)
        {
            var mundoMasculino = lojaController.ObterVitrineMundoPorVitrineETipo(codigoVitrine, 2);
            if (mundoMasculino != null)
            {
                hidCodigoVitrineMasculino.Value = mundoMasculino.CODIGO.ToString();

                var fotos = lojaController.ObterVitrineMundoFotoPorMundo(mundoMasculino.CODIGO);

                foreach (var f in fotos)
                {
                    if (f.ORDEM == 0)
                    {
                        btMasculinoPrincipal.ImageUrl = f.FOTO;
                        imgEMasc0.CommandArgument = f.CODIGO.ToString();
                        imgEMasc0SemFoto.CommandArgument = f.CODIGO.ToString();
                    }
                    else if (f.ORDEM == 1)
                    {
                        btMasculinoMundo1.ImageUrl = f.FOTO;
                        imgEMasc1.CommandArgument = f.CODIGO.ToString();
                        imgEMasc1SemFoto.CommandArgument = f.CODIGO.ToString();
                    }
                    else if (f.ORDEM == 2)
                    {
                        btMasculinoMundo2.ImageUrl = f.FOTO;
                        imgEMasc2.CommandArgument = f.CODIGO.ToString();
                        imgEMasc2SemFoto.CommandArgument = f.CODIGO.ToString();
                    }
                }
            }
        }
        private void CarregarMundoPorta(int codigoVitrine)
        {
            var mundoPorta = lojaController.ObterVitrineMundoPorVitrineETipo(codigoVitrine, 3);
            if (mundoPorta != null)
            {
                hidCodigoVitrinePorta.Value = mundoPorta.CODIGO.ToString();

                var fotos = lojaController.ObterVitrineMundoFotoPorMundo(mundoPorta.CODIGO);

                foreach (var f in fotos)
                {
                    if (f.ORDEM == 0)
                    {
                        btPortaPrincipal.ImageUrl = f.FOTO;
                        imgEPorta0.CommandArgument = f.CODIGO.ToString();
                        imgEPorta0SemFoto.CommandArgument = f.CODIGO.ToString();
                    }

                }
            }
        }
        private void CarregarMundoExtra(int codigoVitrine)
        {
            var mundoExtra = lojaController.ObterVitrineMundoPorVitrineETipo(codigoVitrine, 4);
            if (mundoExtra != null)
            {
                hidCodigoVitrineExtra.Value = mundoExtra.CODIGO.ToString();

                var fotos = lojaController.ObterVitrineMundoFotoPorMundo(mundoExtra.CODIGO);

                foreach (var f in fotos)
                {
                    if (f.ORDEM == 0)
                    {
                        btExtraPrincipal.ImageUrl = f.FOTO;
                        imgEExtra0.CommandArgument = f.CODIGO.ToString();
                        imgEExtra0SemFoto.CommandArgument = f.CODIGO.ToString();
                    }
                    else if (f.ORDEM == 1)
                    {
                        btExtraMundo1.ImageUrl = f.FOTO;
                        imgEExtra1.CommandArgument = f.CODIGO.ToString();
                        imgEExtra1SemFoto.CommandArgument = f.CODIGO.ToString();
                    }
                    else if (f.ORDEM == 2)
                    {
                        btExtraMundo2.ImageUrl = f.FOTO;
                        imgEExtra2.CommandArgument = f.CODIGO.ToString();
                        imgEExtra2SemFoto.CommandArgument = f.CODIGO.ToString();
                    }

                }
            }
        }

        protected void btAdicionarFoto_Click(object sender, ImageClickEventArgs e)
        {
            string url = "";

            try
            {
                //labErro.Text = "";

                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    var codigoVitrineMundoFoto = "";
                    var ordem = "";

                    if (bt.ID.ToLower().Contains("feminino"))
                        codigoVitrineMundoFoto = hidCodigoVitrineFeminino.Value;
                    else if (bt.ID.ToLower().Contains("masculino"))
                        codigoVitrineMundoFoto = hidCodigoVitrineMasculino.Value;
                    else if (bt.ID.ToLower().Contains("porta"))
                        codigoVitrineMundoFoto = hidCodigoVitrinePorta.Value;
                    else if (bt.ID.ToLower().Contains("extra"))
                        codigoVitrineMundoFoto = hidCodigoVitrineExtra.Value;

                    ordem = bt.CommandArgument.ToString();

                    //Abrir pop-up
                    url = "fnAbrirTelaCadastro('admloj_cad_vitrine_fotomundoadd.aspx?fot=" + codigoVitrineMundoFoto + "&o=" + ordem + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                }


            }
            catch (Exception ex)
            {
                //labErro.Text = ex.Message;
            }
        }

        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                var codigoVitrine = Convert.ToInt32(hidCodigoVitrine.Value);

                LimparFotos();

                CarregarMundoFeminino(codigoVitrine);
                CarregarMundoMasculino(codigoVitrine);
                CarregarMundoPorta(codigoVitrine);
                CarregarMundoExtra(codigoVitrine);
            }
            catch (Exception)
            {
            }
        }

        protected void imgExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //labErro.Text = "";

                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    if (bt.CommandArgument != "")
                    {
                        int codigoVitrineMundoFoto = Convert.ToInt32(bt.CommandArgument);
                        lojaController.ExcluirVitrineMundoFoto(codigoVitrineMundoFoto);

                        btAtualizar_Click(null, null);
                    }
                }

            }
            catch (Exception ex)
            {
                //labErro.Text = ex.Message;
            }
        }
        private void LimparFotos()
        {
            btFemininoPrincipal.ImageUrl = "";
            btFemininoMundo1.ImageUrl = "";
            btFemininoMundo2.ImageUrl = "";
            btFemininoMundo3.ImageUrl = "";

            btMasculinoPrincipal.ImageUrl = "";
            btMasculinoMundo1.ImageUrl = "";
            btMasculinoMundo2.ImageUrl = "";

            btPortaPrincipal.ImageUrl = "";

            btExtraPrincipal.ImageUrl = "";
            btExtraMundo1.ImageUrl = "";
            btExtraMundo2.ImageUrl = "";
        }

        protected void btFechar_Click(object sender, EventArgs e)
        {
            if (hidCodigoVitrine.Value != "")
            {

                var vitrine = lojaController.ObterVitrinePorCodigo(Convert.ToInt32(hidCodigoVitrine.Value));
                if (vitrine != null)
                {
                    vitrine.DATA_FINALIZADO = DateTime.Today;
                    lojaController.AtualizarVitrine(vitrine);
                    btFechar.Enabled = false;
                }
            }
        }
        protected void btMarcarSugestao_Click(object sender, EventArgs e)
        {
            if (hidCodigoVitrine.Value != "")
            {

                var vitrines = lojaController.ObterVitrinePorData(Convert.ToDateTime(hidDataIni.Value), Convert.ToDateTime(hidDataFim.Value));
                foreach (var v in vitrines)
                {
                    v.SUGESTAO = 'N';
                    lojaController.AtualizarVitrine(v);
                }

                var vitrine = lojaController.ObterVitrinePorCodigo(Convert.ToInt32(hidCodigoVitrine.Value));
                if (vitrine != null)
                {
                    vitrine.SUGESTAO = 'S';
                    lojaController.AtualizarVitrine(vitrine);
                    btMarcarSugestao.Enabled = false;
                }
            }
        }

        private void HabilitarEdicao(bool hab)
        {
            btAtualizar1.Visible = hab;
            btAtualizar2.Visible = hab;
            btAtualizar3.Visible = hab;
            btAtualizar4.Visible = hab;

            btFechar.Visible = hab;
            btMarcarSugestao.Visible = hab;

            imgEFem0.Visible = hab;
            imgEFem1.Visible = hab;
            imgEFem2.Visible = hab;
            imgEFem3.Visible = hab;

            imgEFem0SemFoto.Visible = hab;
            imgEFem1SemFoto.Visible = hab;
            imgEFem2SemFoto.Visible = hab;
            imgEFem3SemFoto.Visible = hab;

            imgEMasc0.Visible = hab;
            imgEMasc1.Visible = hab;
            imgEMasc2.Visible = hab;

            imgEMasc0SemFoto.Visible = hab;
            imgEMasc1SemFoto.Visible = hab;
            imgEMasc2SemFoto.Visible = hab;

            imgEPorta0.Visible = hab;

            imgEPorta0SemFoto.Visible = hab;

            imgEExtra0.Visible = hab;
            imgEExtra1.Visible = hab;
            imgEExtra2.Visible = hab;

            imgEExtra0SemFoto.Visible = hab;
            imgEExtra1SemFoto.Visible = hab;
            imgEExtra2SemFoto.Visible = hab;

            btFemininoPrincipal.Enabled = hab;
            btFemininoMundo1.Enabled = hab;
            btFemininoMundo2.Enabled = hab;
            btFemininoMundo3.Enabled = hab;

            btMasculinoPrincipal.Enabled = hab;
            btMasculinoMundo1.Enabled = hab;
            btMasculinoMundo2.Enabled = hab;

            btPortaPrincipal.Enabled = hab;

            btExtraPrincipal.Enabled = hab;
            btExtraMundo1.Enabled = hab;
            btExtraMundo2.Enabled = hab;


        }

        protected void imgSemFoto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //labErro.Text = "";

                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {

                    var ordem = 0;
                    if (bt.ID.ToLower().Contains("1"))
                        ordem = 1;
                    else if (bt.ID.ToLower().Contains("2"))
                        ordem = 2;
                    else if (bt.ID.ToLower().Contains("3"))
                        ordem = 3;

                    var codigoVitrineMundo = 0;
                    if (bt.ID.ToLower().Contains("fem"))
                        codigoVitrineMundo = Convert.ToInt32(hidCodigoVitrineFeminino.Value);
                    else if (bt.ID.ToLower().Contains("masc"))
                        codigoVitrineMundo = Convert.ToInt32(hidCodigoVitrineMasculino.Value);
                    else if (bt.ID.ToLower().Contains("porta"))
                        codigoVitrineMundo = Convert.ToInt32(hidCodigoVitrinePorta.Value);
                    else if (bt.ID.ToLower().Contains("extra"))
                        codigoVitrineMundo = Convert.ToInt32(hidCodigoVitrineExtra.Value);

                    int codigoVitrineMundoFoto = 0;
                    // ja existe registro
                    if (bt.CommandArgument != "")
                    {
                        codigoVitrineMundoFoto = Convert.ToInt32(bt.CommandArgument);
                    }

                    var vitrineMundoFoto = lojaController.ObterVitrineMundoFotoPorCodigo(codigoVitrineMundoFoto);
                    if (vitrineMundoFoto != null)
                    {
                        vitrineMundoFoto.FOTO = "~/Fotos/sem_foto.png";
                        lojaController.AtualizarVitrineMundoFoto(vitrineMundoFoto);
                    }
                    else
                    {
                        vitrineMundoFoto = new LOJA_VITRINE_MUNDO_FOTO();
                        vitrineMundoFoto.LOJA_VITRINE_MUNDO = codigoVitrineMundo;
                        vitrineMundoFoto.FOTO = "~/Fotos/sem_foto.png";
                        vitrineMundoFoto.PRINCIPAL = (ordem == 0) ? true : false;
                        vitrineMundoFoto.FAVORITO = 'N';
                        vitrineMundoFoto.HARMONIA = 'N';
                        vitrineMundoFoto.POSICAO = 'N';
                        vitrineMundoFoto.MONTAGEM = 'N';
                        vitrineMundoFoto.CUBOS = 'N';
                        vitrineMundoFoto.ACESSORIOS = 'N';
                        vitrineMundoFoto.ORDEM = ordem;
                        lojaController.InserirVitrineMundoFoto(vitrineMundoFoto);
                    }


                    btAtualizar_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                //labErro.Text = ex.Message;
            }
        }



    }
}


