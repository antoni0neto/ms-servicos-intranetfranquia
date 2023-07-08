using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Linq.Expressions;
using System.Linq.Dynamic;


namespace Relatorios
{
    public partial class zold_mkt_con_vitrine_semanal : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //CarregarSemana();
                CarregarSupervisor();
                CarregarFilial("0");

                CarregarJQuery();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        //private void CarregarSemana()
        //{
        //    var vitrineFotoSemana = lojaController.ObterVitrineFotoSemana();


        //    var semanaLista = new List<ListItem>();
        //    foreach (var s in vitrineFotoSemana)
        //    {
        //        if (!semanaLista.Exists(p => p.Value == s.ANOSEMANA.ToString()))
        //            semanaLista.Add(new ListItem { Value = s.ANOSEMANA.ToString(), Text = (s.DIA_INICIAL.ToString("dd/MM/yyyy") + " - " + s.DIA_FINAL.ToString("dd/MM/yyyy")) });
        //    }

        //    semanaLista.Insert(0, new ListItem { Value = "", Text = "Selecione" });
        //    ddlAnoSemana.DataSource = semanaLista;
        //    ddlAnoSemana.DataBind();

        //}
        private void CarregarSupervisor()
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];
            if (usuario != null)
            {
                var super = baseController.BuscaUsuarioPerfil(3);
                if (usuario.CODIGO_PERFIL == 3)
                {
                    super = super.Where(p => p.CODIGO_USUARIO == usuario.CODIGO_USUARIO).ToList();
                }

                super.Insert(0, new USUARIO { CODIGO_USUARIO = 0, NOME_USUARIO = "" });
                ddlSupervisor.DataSource = super;
                ddlSupervisor.DataBind();

                if (super != null && super.Count() == 2)
                {
                    ddlSupervisor.SelectedIndex = 1;
                    ddlSupervisor.Enabled = false;
                    ddlSupervisor_SelectedIndexChanged(null, null);
                }
                else
                {
                    CarregarFilial("0");
                }
            }

        }
        private void CarregarFilial(string codigoSupervisor)
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            if (codigoSupervisor != "0")
            {
                var usu = baseController.BuscaUsuario(Convert.ToInt32(codigoSupervisor));
                filial = baseController.BuscaFiliais(usu).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").OrderBy(p => p.FILIAL).ToList();
            }
            else
            {
                filial = baseController.BuscaFiliais();
            }

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        protected void ddlSupervisor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFilial(ddlSupervisor.SelectedValue);
            CarregarJQuery();
        }

        private void CarregarJQuery()
        {
            //GRID FOTO
            if (gvFotoVitrine.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionV').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionV').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

            //GRID FOTO
            if (gvFotoAcessorio.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionA').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionA').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

            //GRID FOTO
            if (gvFotoArmario.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);


        }
        #endregion

        //private List<SP_OBTER_VITRINE_SEMANAResult> ObterVitrineSemana()
        //{
        //    //var vitrine = lojaController.ObterVitrineSemana(Convert.ToInt32(ddlAnoSemana.SelectedValue), Convert.ToInt32(ddlSupervisor.SelectedValue), ddlFilial.SelectedValue);
        //    return null;
        //}
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlAnoSemana.SelectedValue == "")
                {
                    labErro.Text = "Selecione uma semana...";
                    CarregarJQuery();
                    return;
                }

                //var vitrine = ObterVitrineSemana();

                //var setorSemana = lojaController.ObterVitrineSetorData(Convert.ToInt32(ddlAnoSemana.SelectedValue));

                //if (setorSemana == null)
                //{
                //    labErro.Text = "Marcar setor na Semana....";
                //    CarregarJQuery();
                //    return;
                //}

                //if (setorSemana.MKT_VITRINE_SETOR == 1)
                //{
                //    gvFotoVitrine.DataSource = vitrine;
                //    gvFotoVitrine.DataBind();

                //    gvFotoAcessorio.DataSource = new List<SP_OBTER_VITRINE_SEMANAResult>(); ;
                //    gvFotoAcessorio.DataBind();

                //    gvFotoArmario.DataSource = new List<SP_OBTER_VITRINE_SEMANAResult>(); ;
                //    gvFotoArmario.DataBind();
                //}
                //else if (setorSemana.MKT_VITRINE_SETOR == 2)
                //{
                //    gvFotoAcessorio.DataSource = vitrine;
                //    gvFotoAcessorio.DataBind();

                //    gvFotoVitrine.DataSource = new List<SP_OBTER_VITRINE_SEMANAResult>(); ;
                //    gvFotoVitrine.DataBind();

                //    gvFotoArmario.DataSource = new List<SP_OBTER_VITRINE_SEMANAResult>(); ;
                //    gvFotoArmario.DataBind();
                //}
                //else if (setorSemana.MKT_VITRINE_SETOR == 3)
                //{
                //    gvFotoArmario.DataSource = vitrine;
                //    gvFotoArmario.DataBind();

                //    gvFotoVitrine.DataSource = new List<SP_OBTER_VITRINE_SEMANAResult>(); ;
                //    gvFotoVitrine.DataBind();

                //    gvFotoAcessorio.DataSource = new List<SP_OBTER_VITRINE_SEMANAResult>(); ;
                //    gvFotoAcessorio.DataBind();
                //}

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "GRID FOTOS"
        protected void gvFotoVitrine_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    //SP_OBTER_VITRINE_SEMANAResult vitrineFoto = e.Row.DataItem as SP_OBTER_VITRINE_SEMANAResult;

                    //Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                    //_litFilial.Text = vitrineFoto.FILIAL;

                    //ImageButton _imgFoto1 = e.Row.FindControl("imgFoto1") as ImageButton;
                    //_imgFoto1.ImageUrl = vitrineFoto.FOTO_PRINCIPAL;
                    //_imgFoto1.CommandArgument = vitrineFoto.FOTO_PRINCIPAL;

                    //ImageButton _imgFoto2 = e.Row.FindControl("imgFoto2") as ImageButton;
                    //_imgFoto2.ImageUrl = vitrineFoto.FOTO_PRINCIPAL_ADC;
                    //_imgFoto2.CommandArgument = vitrineFoto.FOTO_PRINCIPAL_ADC;

                    //ImageButton _imgFoto3 = e.Row.FindControl("imgFoto3") as ImageButton;
                    //_imgFoto3.ImageUrl = vitrineFoto.FOTO_MASCULINA;
                    //_imgFoto3.CommandArgument = vitrineFoto.FOTO_MASCULINA;

                    //ImageButton _imgFoto4 = e.Row.FindControl("imgFoto4") as ImageButton;
                    //_imgFoto4.ImageUrl = vitrineFoto.FOTO_VITRINE_EXTRA;
                    //_imgFoto4.CommandArgument = vitrineFoto.FOTO_VITRINE_EXTRA;

                    //ImageButton _imgFoto5 = e.Row.FindControl("imgFoto5") as ImageButton;
                    //_imgFoto5.ImageUrl = vitrineFoto.FOTO_CUBO_1;
                    //_imgFoto5.CommandArgument = vitrineFoto.FOTO_CUBO_1;

                    //ImageButton _imgFoto6 = e.Row.FindControl("imgFoto6") as ImageButton;
                    //_imgFoto6.ImageUrl = vitrineFoto.FOTO_CUBO_2;
                    //_imgFoto6.CommandArgument = vitrineFoto.FOTO_CUBO_2;

                    //ImageButton _imgFoto7 = e.Row.FindControl("imgFoto7") as ImageButton;
                    //_imgFoto7.ImageUrl = vitrineFoto.FOTO_CUBO_3;
                    //_imgFoto7.CommandArgument = vitrineFoto.FOTO_CUBO_3;

                }
            }
        }
        protected void gvFotoVitrine_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvFotoVitrine.FooterRow;
            if (_footer != null)
            {
            }
        }

        protected void gvFotoAcessorio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    //SP_OBTER_VITRINE_SEMANAResult vitrineFoto = e.Row.DataItem as SP_OBTER_VITRINE_SEMANAResult;

                    //Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                    //_litFilial.Text = vitrineFoto.FILIAL;

                    //ImageButton _imgFoto1 = e.Row.FindControl("imgFoto1") as ImageButton;
                    //_imgFoto1.ImageUrl = vitrineFoto.FOTO_OCULOS;
                    //_imgFoto1.CommandArgument = vitrineFoto.FOTO_OCULOS;

                    //ImageButton _imgFoto2 = e.Row.FindControl("imgFoto2") as ImageButton;
                    //_imgFoto2.ImageUrl = vitrineFoto.FOTO_BIJUS;
                    //_imgFoto2.CommandArgument = vitrineFoto.FOTO_BIJUS;

                    //ImageButton _imgFoto3 = e.Row.FindControl("imgFoto3") as ImageButton;
                    //_imgFoto3.ImageUrl = vitrineFoto.FOTO_CALCADO_FEMININO;
                    //_imgFoto3.CommandArgument = vitrineFoto.FOTO_CALCADO_FEMININO;

                    //ImageButton _imgFoto4 = e.Row.FindControl("imgFoto4") as ImageButton;
                    //_imgFoto4.ImageUrl = vitrineFoto.FOTO_CALCADO_FEMININO_EXTRA;
                    //_imgFoto4.CommandArgument = vitrineFoto.FOTO_CALCADO_FEMININO_EXTRA;

                    //ImageButton _imgFoto5 = e.Row.FindControl("imgFoto5") as ImageButton;
                    //_imgFoto5.ImageUrl = vitrineFoto.FOTO_CALCADO_MASCULINO;
                    //_imgFoto5.CommandArgument = vitrineFoto.FOTO_CALCADO_MASCULINO;

                    //ImageButton _imgFoto6 = e.Row.FindControl("imgFoto6") as ImageButton;
                    //_imgFoto6.ImageUrl = vitrineFoto.FOTO_CALCADO_MASCULINO_EXTRA;
                    //_imgFoto6.CommandArgument = vitrineFoto.FOTO_CALCADO_MASCULINO_EXTRA;

                    //ImageButton _imgFoto7 = e.Row.FindControl("imgFoto7") as ImageButton;
                    //_imgFoto7.ImageUrl = vitrineFoto.FOTO_ACESSORIO_EXTRA;
                    //_imgFoto7.CommandArgument = vitrineFoto.FOTO_ACESSORIO_EXTRA;

                }
            }
        }
        protected void gvFotoAcessorio_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvFotoAcessorio.FooterRow;
            if (_footer != null)
            {
            }
        }

        protected void gvFotoArmario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    //SP_OBTER_VITRINE_SEMANAResult vitrineFoto = e.Row.DataItem as SP_OBTER_VITRINE_SEMANAResult;

                    //Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                    //_litFilial.Text = vitrineFoto.FILIAL;

                    //ImageButton _imgFoto1 = e.Row.FindControl("imgFoto1") as ImageButton;
                    //_imgFoto1.ImageUrl = vitrineFoto.FOTO_FEMININO_1;
                    //_imgFoto1.CommandArgument = vitrineFoto.FOTO_FEMININO_1;

                    //ImageButton _imgFoto2 = e.Row.FindControl("imgFoto2") as ImageButton;
                    //_imgFoto2.ImageUrl = vitrineFoto.FOTO_FEMININO_2;
                    //_imgFoto2.CommandArgument = vitrineFoto.FOTO_FEMININO_2;

                    //ImageButton _imgFoto3 = e.Row.FindControl("imgFoto3") as ImageButton;
                    //_imgFoto3.ImageUrl = vitrineFoto.FOTO_FEMININO_3;
                    //_imgFoto3.CommandArgument = vitrineFoto.FOTO_FEMININO_3;

                    //ImageButton _imgFoto4 = e.Row.FindControl("imgFoto4") as ImageButton;
                    //_imgFoto4.ImageUrl = vitrineFoto.FOTO_FEMININO_4;
                    //_imgFoto4.CommandArgument = vitrineFoto.FOTO_FEMININO_4;

                    //ImageButton _imgFoto5 = e.Row.FindControl("imgFoto5") as ImageButton;
                    //_imgFoto5.ImageUrl = vitrineFoto.FOTO_FEMININO_5;
                    //_imgFoto5.CommandArgument = vitrineFoto.FOTO_FEMININO_5;

                    //ImageButton _imgFoto6 = e.Row.FindControl("imgFoto6") as ImageButton;
                    //_imgFoto6.ImageUrl = vitrineFoto.FOTO_FEMININO_6;
                    //_imgFoto6.CommandArgument = vitrineFoto.FOTO_FEMININO_6;

                    //ImageButton _imgFoto7 = e.Row.FindControl("imgFoto7") as ImageButton;
                    //_imgFoto7.ImageUrl = vitrineFoto.FOTO_MASCULINO_1;
                    //_imgFoto7.CommandArgument = vitrineFoto.FOTO_MASCULINO_1;

                    //ImageButton _imgFoto8 = e.Row.FindControl("imgFoto8") as ImageButton;
                    //_imgFoto8.ImageUrl = vitrineFoto.FOTO_MASCULINO_2;
                    //_imgFoto8.CommandArgument = vitrineFoto.FOTO_MASCULINO_2;

                    //ImageButton _imgFoto9 = e.Row.FindControl("imgFoto9") as ImageButton;
                    //_imgFoto9.ImageUrl = vitrineFoto.FOTO_MASCULINO_3;
                    //_imgFoto9.CommandArgument = vitrineFoto.FOTO_MASCULINO_3;

                    //ImageButton _imgFoto10 = e.Row.FindControl("imgFoto10") as ImageButton;
                    //_imgFoto10.ImageUrl = vitrineFoto.FOTO_MASCULINO_4;
                    //_imgFoto10.CommandArgument = vitrineFoto.FOTO_MASCULINO_4;

                    //ImageButton _imgFoto11 = e.Row.FindControl("imgFoto11") as ImageButton;
                    //_imgFoto11.ImageUrl = vitrineFoto.FOTO_MASCULINO_5;
                    //_imgFoto11.CommandArgument = vitrineFoto.FOTO_MASCULINO_5;

                    //ImageButton _imgFoto12 = e.Row.FindControl("imgFoto12") as ImageButton;
                    //_imgFoto12.ImageUrl = vitrineFoto.FOTO_MASCULINO_6;
                    //_imgFoto12.CommandArgument = vitrineFoto.FOTO_MASCULINO_6;


                }
            }
        }
        protected void gvFotoArmario_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvFotoArmario.FooterRow;
            if (_footer != null)
            {
            }
        }

        #endregion

        protected void imgFoto_Click(object sender, ImageClickEventArgs e)
        {

            string url = "";

            try
            {
                labErro.Text = "";

                ImageButton imgButton = (ImageButton)sender;
                if (imgButton != null)
                {
                    //Abrir pop-up
                    url = "fnAbrirTelaCadastroMaior2('mkt_con_vitrine_semanal_foto_obs.aspx?p=" + imgButton.CommandArgument + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                }

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }


    }
}


