using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class fotoschi : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                CarregarFotos();


            }

        }

        private void CarregarFotos()
        {

            var lista = new List<ListItem>();

            lista.Add(new ListItem { Value = "FOTO", Text = "ASD" });

            gvProduto.DataSource = lista;
            gvProduto.DataBind();

        }

        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ListItem file = e.Row.DataItem as ListItem;

                    System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    _imgProduto.ImageUrl = file.Value;

                    TextBox _txtRef = e.Row.FindControl("txtRef") as TextBox;
                    _txtRef.Text = file.Text;
                }
            }
        }

        protected void txtRef_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;

                string d = txt.Text.Trim();

            }
            catch (Exception)
            {
            }
        }

    }
}

