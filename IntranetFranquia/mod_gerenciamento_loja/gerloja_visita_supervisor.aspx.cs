using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using System.Drawing;

namespace Relatorios
{
    public partial class gerloja_visita_supervisor : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        int segunda = 0;
        int terca = 0;
        int quarta = 0;
        int quinta = 0;
        int sexta = 0;
        int sabado = 0;
        int domingo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";

                var usuario = ((USUARIO)Session["USUARIO"]);

                var ano = DateTime.Now.Year.ToString();
                var mes = DateTime.Now.Month.ToString();
                mes = (mes.Length == 1) ? "0" + mes : mes;
                var codigoUsuario = usuario.CODIGO_USUARIO;

                hidAno.Value = ano;
                hidMes.Value = mes;
                CarregarSupervisores(usuario);

                if (usuario.CODIGO_PERFIL == 3 || usuario.CODIGO_PERFIL == 50)
                {
                    hidCodigoUsuario.Value = codigoUsuario.ToString();
                    CarregarCalendario(ano, mes, codigoUsuario);
                    CarregarCalendarioSemana(ano, codigoUsuario);
                }

            }

            btAtualizar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizar, null) + ";");
            btProximo.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btProximo, null) + ";");
            btAnterior.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAnterior, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarSupervisores(USUARIO usuario)
        {
            List<USUARIO> supers = new List<USUARIO>();

            if (usuario.CODIGO_PERFIL == 3 || usuario.CODIGO_PERFIL == 50)
            {
                supers = baseController.BuscaUsuarioPerfil(usuario.CODIGO_PERFIL);

                ddlSupervisor.DataSource = supers;
                ddlSupervisor.DataBind();

                ddlSupervisor.SelectedValue = usuario.CODIGO_USUARIO.ToString();
                ddlSupervisor.Enabled = false;

                //ddlSupervisor_SelectedIndexChanged(null, null);
            }
            else
            {
                var super1 = baseController.BuscaUsuarioPerfil(3);
                var super2 = baseController.BuscaUsuarioPerfil(50);

                supers.AddRange(super1);
                supers.AddRange(super2);

                supers = supers.OrderBy(p => p.NOME_USUARIO).ToList();

                supers.Insert(0, new USUARIO { CODIGO_USUARIO = 0, NOME_USUARIO = "Selecione" });

                ddlSupervisor.DataSource = supers;
                ddlSupervisor.DataBind();
            }

        }
        #endregion


        private void CarregarCalendario(string ano, string mes, int codigoUsuario)
        {

            //var usuario = baseController.BuscaUsuario(codigoUsuario);
            //labSupervisor.Text = usuario.NOME_USUARIO.ToUpper();

            var mesAux = "";

            switch (mes)
            {
                case "01":
                    mesAux = "Janeiro";
                    break;
                case "02":
                    mesAux = "Fevereiro";
                    break;
                case "03":
                    mesAux = "Março";
                    break;
                case "04":
                    mesAux = "Abril";
                    break;
                case "05":
                    mesAux = "Maio";
                    break;
                case "06":
                    mesAux = "Junho";
                    break;
                case "07":
                    mesAux = "Julho";
                    break;
                case "08":
                    mesAux = "Agosto";
                    break;
                case "09":
                    mesAux = "Setembro";
                    break;
                case "10":
                    mesAux = "Outubro";
                    break;
                case "11":
                    mesAux = "Novembro";
                    break;
                case "12":
                    mesAux = "Dezembro";
                    break;

            }

            labPeriodo.Text = (mesAux + " de " + ano.ToString()).ToUpper();

            var agendaVisitas = lojaController.ObterCalendarioVisitaSuper(ano, mes, codigoUsuario);

            repAgendaSuper.DataSource = agendaVisitas;
            repAgendaSuper.DataBind();

        }
        private void CarregarCalendarioSemana(string ano, int codigoUsuario)
        {
            var agendaVisitaSEmana = lojaController.ObterCalendarioVisitaSuperSemana(Convert.ToInt32(ano), codigoUsuario);

            gvCalendarioSemana.DataSource = agendaVisitaSEmana;
            gvCalendarioSemana.DataBind();
        }

        protected void repAgendaSuper_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SP_OBTER_CALENDARIO_VISITASUPERResult agenda = e.Item.DataItem as SP_OBTER_CALENDARIO_VISITASUPERResult;

            if (agenda != null)
            {
                // Amarelo #EEE8AA;
                // Vermelho #FFA07A;
                // Verde #98FB98;
                // Azul #ADD8E6;

                Table tbSegunda = e.Item.FindControl("tbSegunda") as Table;
                Table tbTerca = e.Item.FindControl("tbTerca") as Table;
                Table tbQuarta = e.Item.FindControl("tbQuarta") as Table;
                Table tbQuinta = e.Item.FindControl("tbQuinta") as Table;
                Table tbSexta = e.Item.FindControl("tbSexta") as Table;
                Table tbSabado = e.Item.FindControl("tbSabado") as Table;
                Table tbDomingo = e.Item.FindControl("tbDomingo") as Table;


                var corSegunda = lojaController.ObterAgendaVisitaCor(Convert.ToInt32(hidCodigoUsuario.Value), Convert.ToInt32(hidAno.Value), Convert.ToInt32(hidMes.Value), agenda.SEGUNDA_DIA);
                if (corSegunda != null)
                    tbSegunda.Style.Add("background-color", corSegunda.COR);

                var corTerca = lojaController.ObterAgendaVisitaCor(Convert.ToInt32(hidCodigoUsuario.Value), Convert.ToInt32(hidAno.Value), Convert.ToInt32(hidMes.Value), agenda.TERCA_DIA);
                if (corTerca != null)
                    tbTerca.Style.Add("background-color", corTerca.COR);

                var corQuarta = lojaController.ObterAgendaVisitaCor(Convert.ToInt32(hidCodigoUsuario.Value), Convert.ToInt32(hidAno.Value), Convert.ToInt32(hidMes.Value), agenda.QUARTA_DIA);
                if (corQuarta != null)
                    tbQuarta.Style.Add("background-color", corQuarta.COR);

                var corQuinta = lojaController.ObterAgendaVisitaCor(Convert.ToInt32(hidCodigoUsuario.Value), Convert.ToInt32(hidAno.Value), Convert.ToInt32(hidMes.Value), agenda.QUINTA_DIA);
                if (corQuinta != null)
                    tbQuinta.Style.Add("background-color", corQuinta.COR);

                var corSexta = lojaController.ObterAgendaVisitaCor(Convert.ToInt32(hidCodigoUsuario.Value), Convert.ToInt32(hidAno.Value), Convert.ToInt32(hidMes.Value), agenda.SEXTA_DIA);
                if (corSexta != null)
                    tbSexta.Style.Add("background-color", corSexta.COR);

                var corSabado = lojaController.ObterAgendaVisitaCor(Convert.ToInt32(hidCodigoUsuario.Value), Convert.ToInt32(hidAno.Value), Convert.ToInt32(hidMes.Value), agenda.SABADO_DIA);
                if (corSabado != null)
                    tbSabado.Style.Add("background-color", corSabado.COR);

                var corDomingo = lojaController.ObterAgendaVisitaCor(Convert.ToInt32(hidCodigoUsuario.Value), Convert.ToInt32(hidAno.Value), Convert.ToInt32(hidMes.Value), agenda.DOMINGO_DIA);
                if (corDomingo != null)
                    tbDomingo.Style.Add("background-color", corDomingo.COR);


                Label litSegundaDia = e.Item.FindControl("litSegundaDia") as Label;
                litSegundaDia.Text = (agenda.SEGUNDA_DIA == 0) ? "" : agenda.SEGUNDA_DIA.ToString();
                Literal litSegundaFilial = e.Item.FindControl("litSegundaFilial") as Literal;
                litSegundaFilial.Text = FormatarTextoFilial(agenda.SEGUNDA);
                ImageButton btImgSegunda = e.Item.FindControl("btImgSegunda") as ImageButton;
                btImgSegunda.CommandArgument = agenda.SEGUNDA_DIA.ToString();
                if (agenda.SEGUNDA_DIA == 0) btImgSegunda.Visible = false;


                Label litTercaDia = e.Item.FindControl("litTercaDia") as Label;
                litTercaDia.Text = (agenda.TERCA_DIA == 0) ? "" : agenda.TERCA_DIA.ToString();
                Literal litTercaFilial = e.Item.FindControl("litTercaFilial") as Literal;
                litTercaFilial.Text = FormatarTextoFilial(agenda.TERCA);
                ImageButton btImgTerca = e.Item.FindControl("btImgTerca") as ImageButton;
                btImgTerca.CommandArgument = agenda.TERCA_DIA.ToString();
                if (agenda.TERCA_DIA == 0) btImgTerca.Visible = false;


                Label litQuartaDia = e.Item.FindControl("litQuartaDia") as Label;
                litQuartaDia.Text = (agenda.QUARTA_DIA == 0) ? "" : agenda.QUARTA_DIA.ToString();
                Literal litQuartaFilial = e.Item.FindControl("litQuartaFilial") as Literal;
                litQuartaFilial.Text = FormatarTextoFilial(agenda.QUARTA);
                ImageButton btImgQuarta = e.Item.FindControl("btImgQuarta") as ImageButton;
                btImgQuarta.CommandArgument = agenda.QUARTA_DIA.ToString();
                if (agenda.QUARTA_DIA == 0) btImgQuarta.Visible = false;

                Label litQuintaDia = e.Item.FindControl("litQuintaDia") as Label;
                litQuintaDia.Text = (agenda.QUINTA_DIA == 0) ? "" : agenda.QUINTA_DIA.ToString();
                Literal litQuintaFilial = e.Item.FindControl("litQuintaFilial") as Literal;
                litQuintaFilial.Text = FormatarTextoFilial(agenda.QUINTA);
                ImageButton btImgQuinta = e.Item.FindControl("btImgQuinta") as ImageButton;
                btImgQuinta.CommandArgument = agenda.QUINTA_DIA.ToString();
                if (agenda.QUINTA_DIA == 0) btImgQuinta.Visible = false;


                Label litSextaDia = e.Item.FindControl("litSextaDia") as Label;
                litSextaDia.Text = (agenda.SEXTA_DIA == 0) ? "" : agenda.SEXTA_DIA.ToString();
                Literal litSextaFilial = e.Item.FindControl("litSextaFilial") as Literal;
                litSextaFilial.Text = FormatarTextoFilial(agenda.SEXTA);
                ImageButton btImgSexta = e.Item.FindControl("btImgSexta") as ImageButton;
                btImgSexta.CommandArgument = agenda.SEXTA_DIA.ToString();
                if (agenda.SEXTA_DIA == 0) btImgSexta.Visible = false;

                Label litSabadoDia = e.Item.FindControl("litSabadoDia") as Label;
                litSabadoDia.Text = (agenda.SABADO_DIA == 0) ? "" : agenda.SABADO_DIA.ToString();
                Literal litSabadoFilial = e.Item.FindControl("litSabadoFilial") as Literal;
                litSabadoFilial.Text = FormatarTextoFilial(agenda.SABADO);
                ImageButton btImgSabado = e.Item.FindControl("btImgSabado") as ImageButton;
                btImgSabado.CommandArgument = agenda.SABADO_DIA.ToString();
                if (agenda.SABADO_DIA == 0) btImgSabado.Visible = false;

                Label litDomingoDia = e.Item.FindControl("litDomingoDia") as Label;
                litDomingoDia.Text = (agenda.DOMINGO_DIA == 0) ? "" : agenda.DOMINGO_DIA.ToString();
                Literal litDomingoFilial = e.Item.FindControl("litDomingoFilial") as Literal;
                litDomingoFilial.Text = FormatarTextoFilial(agenda.DOMINGO);
                ImageButton btImgDomingo = e.Item.FindControl("btImgDomingo") as ImageButton;
                btImgDomingo.CommandArgument = agenda.DOMINGO_DIA.ToString();
                if (agenda.DOMINGO_DIA == 0) btImgDomingo.Visible = false;


            }
        }
        private string FormatarTextoFilial(string campo)
        {
            return campo.Replace("1@@@", "<font size='2' color=''>").Replace("@@@2", "</font><br />");
        }

        protected void gvCalendarioSemana_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CALENDARIO_VISITASUPER_SEMANAResult agendaSemana = e.Row.DataItem as SP_OBTER_CALENDARIO_VISITASUPER_SEMANAResult;

                    //Label labDataUltimaVisita = e.Row.FindControl("labDataUltimaVisita") as Label;
                    //labDataUltimaVisita.Text = Convert.ToDateTime(agendaSemana.ULTIMA_VISITA).ToString("dd/MM/yyyy");

                    segunda += Convert.ToInt32(agendaSemana.SEGUNDA);
                    terca += Convert.ToInt32(agendaSemana.TERCA);
                    quarta += Convert.ToInt32(agendaSemana.QUARTA);
                    quinta += Convert.ToInt32(agendaSemana.QUINTA);
                    sexta += Convert.ToInt32(agendaSemana.SEXTA);
                    sabado += Convert.ToInt32(agendaSemana.SABADO);
                    domingo += Convert.ToInt32(agendaSemana.DOMINGO);

                    if (agendaSemana.ULTIMA_VISITA > DateTime.Now.Date)
                        e.Row.Cells[0].BackColor = Color.LightCyan;

                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvCalendarioSemana_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCalendarioSemana.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[2].Text = segunda.ToString();
                footer.Cells[3].Text = terca.ToString();
                footer.Cells[4].Text = quarta.ToString();
                footer.Cells[5].Text = quinta.ToString();
                footer.Cells[6].Text = sexta.ToString();
                footer.Cells[7].Text = sabado.ToString();
                footer.Cells[8].Text = domingo.ToString();

            }
        }

        protected void btAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                var ano = Convert.ToInt32(hidAno.Value);
                var mes = Convert.ToInt32(hidMes.Value);
                mes = mes - 1;

                if (mes == 0)
                {
                    ano = ano - 1;
                    mes = 12;
                }

                hidAno.Value = ano.ToString();
                var mesAux = (mes.ToString().Length == 1) ? "0" + mes.ToString() : mes.ToString();
                hidMes.Value = mesAux;

                CarregarCalendario(ano.ToString(), mesAux, Convert.ToInt32(hidCodigoUsuario.Value));
                CarregarCalendarioSemana(ano.ToString(), Convert.ToInt32(hidCodigoUsuario.Value));

            }
            catch (Exception)
            {
            }

        }
        protected void btProximo_Click(object sender, EventArgs e)
        {
            try
            {
                var ano = Convert.ToInt32(hidAno.Value);
                var mes = Convert.ToInt32(hidMes.Value);
                mes = mes + 1;

                if (mes == 13)
                {
                    ano = ano + 1;
                    mes = 1;
                }

                hidAno.Value = ano.ToString();
                var mesAux = (mes.ToString().Length == 1) ? "0" + mes.ToString() : mes.ToString();
                hidMes.Value = mesAux;

                CarregarCalendario(ano.ToString(), mesAux, Convert.ToInt32(hidCodigoUsuario.Value));
                CarregarCalendarioSemana(ano.ToString(), Convert.ToInt32(hidCodigoUsuario.Value));
            }
            catch (Exception)
            {
            }
        }
        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            CarregarCalendario(hidAno.Value, hidMes.Value, Convert.ToInt32(hidCodigoUsuario.Value));
            CarregarCalendarioSemana(hidAno.Value, Convert.ToInt32(hidCodigoUsuario.Value));
        }

        protected void btImgAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;

                var dia = bt.CommandArgument;
                var codigoSupervisor = ddlSupervisor.SelectedValue;

                //Abrir pop-up
                var url = "fnAbrirTelaCadastro('gerloja_visita_supervisor_add.aspx?an=" + hidAno.Value + "&me=" + hidMes.Value + "&di=" + dia + "&sup=" + codigoSupervisor + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
            }
            catch (Exception)
            {
            }
        }

        protected void ddlSupervisor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSupervisor.SelectedValue != "")
            {
                hidCodigoUsuario.Value = ddlSupervisor.SelectedValue;
                btAnterior.Enabled = true;
                btProximo.Enabled = true;
                btAtualizar.Enabled = true;
            }
            else
            {
                hidCodigoUsuario.Value = "0";
                btAnterior.Enabled = false;
                btProximo.Enabled = false;
                btAtualizar.Enabled = false;
            }

            btAtualizar_Click(null, null);
        }


    }
}
