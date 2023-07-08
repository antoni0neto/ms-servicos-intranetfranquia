using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class contabil_cad_param_linx : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TxtDataFechamento.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        private void CarregaGridViewParamentros()
        {
            List<PARAMETRO> _parametros = baseController.BuscarParametroFechaPeriodo();

            btGravar.Enabled = false;
            LabelFeedBack.Text = "";
            if (_parametros != null)
            {
                GridViewParametros.DataSource = _parametros;
                GridViewParametros.DataBind();

                if (_parametros.Count > 0)
                {

                    if (Session["USUARIO"] != null)
                    {
                        USUARIO _usuario = (Session["USUARIO"] as USUARIO);

                        if (_usuario != null)
                        {
                            if (_usuario.USUARIO1 == "fabio.leite" || _usuario.USUARIO1 == "cristina.reis" || _usuario.USUARIO1 == "valber.custodio" || _usuario.USUARIO1 == "leandrobevilaqua")
                                btGravar.Enabled = true;
                            else
                                btGravar.Enabled = false;
                        }
                    }
                }
            }

        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            CarregaGridViewParamentros();

            foreach (GridViewRow r in GridViewParametros.Rows)
            {
                TextBox _t = (r.FindControl("txtData") as TextBox);
                if (_t != null)
                {
                    if (TxtDataFechamento.Text != "")
                    {
                        _t.Text = TxtDataFechamento.Text;
                    }
                    else
                    {
                        _t.Text = r.Cells[2].Text;
                    }
                }
            }

        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            string _dataNova = "";
            DateTime _data;
            bool _updated = false;

            
            try
            {
                LabelFeedBack.Visible = false;
                foreach (GridViewRow r in GridViewParametros.Rows)
                {
                    TextBox _t = (r.FindControl("txtData") as TextBox);

                    if (_t != null)
                    {
                        if (_t.Text != "")
                        {
                            _dataNova = _t.Text;

                            //Validar campo Data
                            if (DateTime.TryParse(_dataNova, out _data))
                            {
                                //Atualizar campo
                                baseController.AtualizaParametrosFechamento(_data, r.Cells[0].Text);
                                _updated = true;
                            }
                            else
                            {
                                LabelFeedBack.Visible = true;
                                LabelFeedBack.Text = "A data informada do parâmetro \"" + r.Cells[0].Text + "\" é inválida.";
                                return;
                            }
                        }
                    }
                }

                TxtDataFechamento.Text = "";
                CalendarDataInicio.SelectedDates.Clear();

                CarregaGridViewParamentros();

                LabelFeedBack.Visible = true;
                if(_updated)
                    LabelFeedBack.Text = "Parâmetros atualizados.";
                else
                    LabelFeedBack.Text = "Nenhum parâmetro atualizado.";
            }
            catch (Exception ex)
            {
                LabelFeedBack.Visible = true;
                LabelFeedBack.Text = "Erro. " + ex.Message;
            }
        }
    }
}
