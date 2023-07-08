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
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace Relatorios
{
    public partial class adm_con_usuario_ad : System.Web.UI.Page
    {
        ActiveDirectoryController adController = new ActiveDirectoryController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";

            try
            {
                List<UsuarioAD> ad = new List<UsuarioAD>();
                ad = CarregarUsuarioAD().Where(p =>
                                                    p.USUARIO != "SUPPORT_388945A0" &&
                                                    p.USUARIO != "HELPASSISTANT_3E9996" &&
                                                    p.USUARIO != "IUSR_CMAXDADOS" &&
                                                    p.USUARIO != "AVAST" &&
                                                    p.USUARIO != "ASPNET" &&
                                                    p.USUARIO != "BACKUP" &&
                                                    p.USUARIO != "ADMINISTRADOR" &&
                                                    p.USUARIO != "CONVIDADO" &&
                                                    p.USUARIO != "DATASYNC" &&
                                                    p.USUARIO != "FORTIGATE" &&
                                                    p.USUARIO != "GLPI" &&
                                                    p.USUARIO != "KRBTGT" &&
                                                    p.USUARIO != "PANDA" &&
                                                    p.USUARIO != "SQLSERVER" &&
                                                    p.USUARIO != "IWAM_CMAXDADOS").ToList();

                if (txtUsuario.Text.Trim() != "")
                    ad = ad.Where(p => p.USUARIO.Contains(txtUsuario.Text.Trim().ToUpper())).ToList();

                if (txtNome.Text.Trim() != "")
                    ad = ad.Where(p => p.NOME.Contains(txtNome.Text.Trim().ToUpper())).ToList();

                if (txtSetor.Text.Trim() != "")
                    ad = ad.Where(p => p.SETOR.Contains(txtSetor.Text.Trim().ToUpper())).ToList();

                if (ad != null)
                {
                    Session["USUARIO_AD"] = ad;
                    gvUsuario.DataSource = ad;
                    gvUsuario.DataBind();
                }

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + " ......" + ex.StackTrace;
            }
        }
        protected void gvUsuario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    UsuarioAD _usuario = e.Row.DataItem as UsuarioAD;

                    coluna += 1;
                    if (_usuario != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                    }
                }
            }
        }

        private List<UsuarioAD> CarregarUsuarioAD()
        {
            List<UsuarioAD> _lstUsuariosAD = new List<UsuarioAD>();
            List<string> _lstGrupos = new List<string>();

            string uUsuario = "";
            string uNome = "";
            string uSetor = "";
            string uEmail = "";
            string grupoMembro = "";

            int posicao1 = 0;
            int posicao2 = 0;

            var usuarioAD = adController.ObterListaUsuariosAD();
            if (usuarioAD != null)
            {
                foreach (SearchResult sr in usuarioAD)
                {
                    uUsuario = (sr.Properties["sAMAccountName"].Count > 0) ? sr.Properties["sAMAccountName"][0].ToString() : "";
                    uNome = (sr.Properties["displayName"].Count > 0) ? sr.Properties["displayName"][0].ToString() : "Sem Nome";
                    uSetor = (sr.Properties["department"].Count > 0) ? sr.Properties["department"][0].ToString() : "Desativado/Sem Grupo";
                    uEmail = (sr.Properties["mail"].Count > 0) ? sr.Properties["mail"][0].ToString() : "Sem e-mail";

                    if (uSetor.Trim() != "" && uSetor != "Desativado/Sem Grupo")
                    {
                        //Obter outros grupos
                        var grupos = adController.ObterGruposUsuario(uUsuario);
                        foreach (var grupo in grupos.Properties["memberOf"])
                        {
                            grupoMembro = grupo.ToString();
                            if (grupoMembro.Trim() != "")
                            {
                                posicao1 = (grupoMembro.IndexOf("=") + 1);
                                grupoMembro = grupoMembro.Substring(posicao1, grupoMembro.Length - posicao1);
                                posicao2 = (grupoMembro.IndexOf(",") + 1);
                                grupoMembro = grupoMembro.Substring(0, posicao2 - 1);
                            }

                            _lstGrupos.Add(grupoMembro.ToUpper().Replace("GRUPO_", ""));
                        }
                    }

                    _lstUsuariosAD.Add(new UsuarioAD
                    {
                        USUARIO = uUsuario.ToUpper(),
                        NOME = uNome.ToUpper(),
                        SETOR = "G_" + uSetor.ToUpper(),
                        EMAIL = uEmail,
                        MEMBRO_DE = string.Join(", ", _lstGrupos)
                    });

                    _lstGrupos.Clear();
                }
            }

            return _lstUsuariosAD;
        }
        protected void btExcelAD_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["USUARIO_AD"] != null)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "USUARIO_AD_" + DateTime.Today.ToString("yyyy-MM-dd") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvUsuario.AllowPaging = false;
                    gvUsuario.PageSize = 1000;
                    gvUsuario.DataSource = (Session["USUARIO_AD"] as List<UsuarioAD>);
                    gvUsuario.DataBind();

                    gvUsuario.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvUsuario.HeaderRow.Cells.Count; i++)
                    {
                        gvUsuario.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                    }
                    gvUsuario.RenderControl(htw);
                    Response.Write(sw.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                string a = "a";
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}
