using DAL;
using Relatorios.mod_ecom.emailMKT;
using Relatorios.mod_ecom.mag;
using Relatorios.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_email_criacao_conteudo : System.Web.UI.Page
    {

        EcomController ecomController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["emcorp"] == null || Request.QueryString["emcorp"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                var codigoEmailCorpo = Convert.ToInt32(Request.QueryString["emcorp"].ToString());

                var emailCorpo = ecomController.ObterEmailCorpoPorCodigo(codigoEmailCorpo);
                if (emailCorpo == null)
                {
                    Response.Write("Nada foi encontrado.");
                    Response.End();
                }

                hidCodigoCorpo.Value = emailCorpo.CODIGO.ToString();
                hidCodigoBloco.Value = emailCorpo.ECOM_EMAIL_BLOCO.ToString();


                if (emailCorpo.ECOM_EMAIL_BLOCO == 10)
                    fsBanner.Visible = true;
                else if (emailCorpo.ECOM_EMAIL_BLOCO == 11)
                    fsFrase.Visible = true;
                else if (emailCorpo.ECOM_EMAIL_BLOCO == 12)
                    fsCupom.Visible = true;
                else if (emailCorpo.ECOM_EMAIL_BLOCO == 18)
                    fsTitulo.Visible = true;
                else if (emailCorpo.ECOM_EMAIL_BLOCO == 21 || emailCorpo.ECOM_EMAIL_BLOCO == 22)
                    fsLink.Visible = true;
                else if (emailCorpo.ECOM_EMAIL_BLOCO == 13 ||
                        emailCorpo.ECOM_EMAIL_BLOCO == 14 ||
                        emailCorpo.ECOM_EMAIL_BLOCO == 16 ||
                        emailCorpo.ECOM_EMAIL_BLOCO == 17 ||
                        emailCorpo.ECOM_EMAIL_BLOCO == 19 ||
                        emailCorpo.ECOM_EMAIL_BLOCO == 20
                    )
                    fs3Produto.Visible = true;


                if (emailCorpo.ECOM_EMAIL_BLOCO == 13 || emailCorpo.ECOM_EMAIL_BLOCO == 14 || emailCorpo.ECOM_EMAIL_BLOCO == 19 || emailCorpo.ECOM_EMAIL_BLOCO == 20)
                {
                    txtSKU4.Enabled = false;
                    ddlFoto4.Enabled = false;
                    txtSKU5.Enabled = false;
                    ddlFoto5.Enabled = false;
                    txtSKU6.Enabled = false;
                    ddlFoto6.Enabled = false;
                }

                CarregarConteudo(emailCorpo);

            }

        }

        private void CarregarConteudo(ECOM_EMAIL_CORPO emailCorpo)
        {
            if (emailCorpo.ECOM_EMAIL_BLOCO == 10) //banner
            {
                var emailLink = ecomController.ObterEmailCorpoLink(emailCorpo.CODIGO).FirstOrDefault();
                if (emailLink != null)
                {
                    txtLinkBanner.Text = emailLink.LINK_REDIRECT;
                    txtLinkBannerImagem.Text = emailLink.LINK_IMAGEM;
                }
            }

            else if (emailCorpo.ECOM_EMAIL_BLOCO == 21 || emailCorpo.ECOM_EMAIL_BLOCO == 22) //link botao/frase
            {
                var emailLink = ecomController.ObterEmailCorpoLink(emailCorpo.CODIGO).FirstOrDefault();
                if (emailLink != null)
                {
                    txtTextoBotaoFrase.Text = emailLink.TEXTO;
                    txtLinkBotaoFrase.Text = emailLink.LINK_REDIRECT;
                }
            }


            else if (emailCorpo.ECOM_EMAIL_BLOCO == 11) //frase
            {
                var emailLink = ecomController.ObterEmailCorpoLink(emailCorpo.CODIGO).FirstOrDefault();
                if (emailLink != null)
                {
                    txtFrase.Text = emailLink.TEXTO;
                }
            }

            else if (emailCorpo.ECOM_EMAIL_BLOCO == 12) //cupom
            {
                var emailLink = ecomController.ObterEmailCorpoLink(emailCorpo.CODIGO).FirstOrDefault();
                if (emailLink != null)
                {
                    txtCupom.Text = emailLink.TEXTO;
                }
            }
            else if (emailCorpo.ECOM_EMAIL_BLOCO == 18) //titulo
            {
                var emailLink = ecomController.ObterEmailCorpoLink(emailCorpo.CODIGO).FirstOrDefault();
                if (emailLink != null)
                {
                    txtTitulo.Text = emailLink.TEXTO.ToUpper();
                }
            }
            else
            {
                lab3Produto.Text = emailCorpo.ECOM_EMAIL_BLOCO1.NOME;
                var emailLink = ecomController.ObterEmailCorpoLink(emailCorpo.CODIGO);
                foreach (var el in emailLink)
                {
                    if (el.ORDEM == 1)
                    {
                        txtSKU1.Text = el.SKU;
                        txtSKU_TextChanged(txtSKU1, null);
                        imgProduto1.ImageUrl = el.LINK_IMAGEM;
                        labPreco1.Text = "R$ " + el.PRECO.ToString();
                        labPrecoPromo1.Text = "R$ " + el.PRECO_PROMO.ToString();
                    }
                    else if (el.ORDEM == 2)
                    {
                        txtSKU2.Text = el.SKU;
                        txtSKU_TextChanged(txtSKU2, null);
                        imgProduto2.ImageUrl = el.LINK_IMAGEM;
                        labPreco2.Text = "R$ " + el.PRECO.ToString();
                        labPrecoPromo2.Text = "R$ " + el.PRECO_PROMO.ToString();
                    }
                    else if (el.ORDEM == 3)
                    {
                        txtSKU3.Text = el.SKU;
                        txtSKU_TextChanged(txtSKU3, null);
                        imgProduto3.ImageUrl = el.LINK_IMAGEM;
                        labPreco3.Text = "R$ " + el.PRECO.ToString();
                        labPrecoPromo3.Text = "R$ " + el.PRECO_PROMO.ToString();
                    }
                    else if (el.ORDEM == 4)
                    {
                        txtSKU4.Text = el.SKU;
                        txtSKU_TextChanged(txtSKU4, null);
                        imgProduto4.ImageUrl = el.LINK_IMAGEM;
                        labPreco4.Text = "R$ " + el.PRECO.ToString();
                        labPrecoPromo4.Text = "R$ " + el.PRECO_PROMO.ToString();
                    }
                    else if (el.ORDEM == 5)
                    {
                        txtSKU5.Text = el.SKU;
                        txtSKU_TextChanged(txtSKU5, null);
                        imgProduto5.ImageUrl = el.LINK_IMAGEM;
                        labPreco5.Text = "R$ " + el.PRECO.ToString();
                        labPrecoPromo5.Text = "R$ " + el.PRECO_PROMO.ToString();
                    }
                    else if (el.ORDEM == 6)
                    {
                        txtSKU6.Text = el.SKU;
                        txtSKU_TextChanged(txtSKU6, null);
                        imgProduto6.ImageUrl = el.LINK_IMAGEM;
                        labPreco6.Text = "R$ " + el.PRECO.ToString();
                        labPrecoPromo6.Text = "R$ " + el.PRECO_PROMO.ToString();
                    }
                }
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (hidCodigoBloco.Value == "10") //banner
                {
                    var emailLink = ecomController.ObterEmailCorpoLink(Convert.ToInt32(hidCodigoCorpo.Value)).FirstOrDefault();
                    if (emailLink == null)
                    {
                        emailLink = new ECOM_EMAIL_CORPO_LINK();
                        emailLink.ECOM_EMAIL_CORPO = Convert.ToInt32(hidCodigoCorpo.Value);
                        emailLink.LINK_REDIRECT = txtLinkBanner.Text;
                        emailLink.LINK_IMAGEM = txtLinkBannerImagem.Text;
                        emailLink.ORDEM = 1;
                        ecomController.InserirEmailCorpoLink(emailLink);
                    }
                    else
                    {
                        emailLink.LINK_REDIRECT = txtLinkBanner.Text;
                        emailLink.LINK_IMAGEM = txtLinkBannerImagem.Text;
                        ecomController.AtualizarEmailCorpoLink(emailLink);
                    }
                }

                if (hidCodigoBloco.Value == "21" || hidCodigoBloco.Value == "22") //texto botao/frase
                {
                    var emailLink = ecomController.ObterEmailCorpoLink(Convert.ToInt32(hidCodigoCorpo.Value)).FirstOrDefault();
                    if (emailLink == null)
                    {
                        emailLink = new ECOM_EMAIL_CORPO_LINK();
                        emailLink.ECOM_EMAIL_CORPO = Convert.ToInt32(hidCodigoCorpo.Value);
                        emailLink.LINK_REDIRECT = txtLinkBotaoFrase.Text;
                        emailLink.TEXTO = txtTextoBotaoFrase.Text;
                        emailLink.ORDEM = 1;
                        ecomController.InserirEmailCorpoLink(emailLink);
                    }
                    else
                    {
                        emailLink.LINK_REDIRECT = txtLinkBotaoFrase.Text;
                        emailLink.TEXTO = txtTextoBotaoFrase.Text;
                        ecomController.AtualizarEmailCorpoLink(emailLink);
                    }
                }

                else if (hidCodigoBloco.Value == "11") //frase
                {
                    var emailLink = ecomController.ObterEmailCorpoLink(Convert.ToInt32(hidCodigoCorpo.Value)).FirstOrDefault();
                    if (emailLink == null)
                    {
                        emailLink = new ECOM_EMAIL_CORPO_LINK();
                        emailLink.ECOM_EMAIL_CORPO = Convert.ToInt32(hidCodigoCorpo.Value);
                        emailLink.TEXTO = txtFrase.Text.Trim().Replace("\r", "<br/>").Replace("\n", "<br/>");
                        emailLink.ORDEM = 1;
                        ecomController.InserirEmailCorpoLink(emailLink);
                    }
                    else
                    {
                        emailLink.TEXTO = txtFrase.Text.Trim().Replace("\r", "<br/>").Replace("\n", "<br/>");
                        ecomController.AtualizarEmailCorpoLink(emailLink);
                    }
                }

                else if (hidCodigoBloco.Value == "12") //cupom
                {
                    var emailLink = ecomController.ObterEmailCorpoLink(Convert.ToInt32(hidCodigoCorpo.Value)).FirstOrDefault();
                    if (emailLink == null)
                    {
                        emailLink = new ECOM_EMAIL_CORPO_LINK();
                        emailLink.ECOM_EMAIL_CORPO = Convert.ToInt32(hidCodigoCorpo.Value);
                        emailLink.TEXTO = txtCupom.Text.Trim().Replace("\r", "<br/>").Replace("\n", "<br/>");
                        emailLink.ORDEM = 1;
                        ecomController.InserirEmailCorpoLink(emailLink);
                    }
                    else
                    {
                        emailLink.TEXTO = txtCupom.Text.Trim().Replace("\r", "<br/>").Replace("\n", "<br/>");
                        ecomController.AtualizarEmailCorpoLink(emailLink);
                    }
                }
                else if (hidCodigoBloco.Value == "18") //titulo
                {
                    var emailLink = ecomController.ObterEmailCorpoLink(Convert.ToInt32(hidCodigoCorpo.Value)).FirstOrDefault();
                    if (emailLink == null)
                    {
                        emailLink = new ECOM_EMAIL_CORPO_LINK();
                        emailLink.ECOM_EMAIL_CORPO = Convert.ToInt32(hidCodigoCorpo.Value);
                        emailLink.TEXTO = txtTitulo.Text.Trim().Replace("\r", "<br/>").Replace("\n", "<br/>");
                        emailLink.ORDEM = 1;
                        ecomController.InserirEmailCorpoLink(emailLink);
                    }
                    else
                    {
                        emailLink.TEXTO = txtTitulo.Text.Trim().Replace("\r", "<br/>").Replace("\n", "<br/>");
                        ecomController.AtualizarEmailCorpoLink(emailLink);
                    }
                }
                else
                {
                    var codigoCorpoEmail = Convert.ToInt32(hidCodigoCorpo.Value);

                    // validar campos
                    SalvarProduto(txtSKU1, imgProduto1, codigoCorpoEmail, 1);
                    SalvarProduto(txtSKU2, imgProduto2, codigoCorpoEmail, 2);
                    SalvarProduto(txtSKU3, imgProduto3, codigoCorpoEmail, 3);
                    SalvarProduto(txtSKU4, imgProduto4, codigoCorpoEmail, 4);
                    SalvarProduto(txtSKU5, imgProduto5, codigoCorpoEmail, 5);
                    SalvarProduto(txtSKU6, imgProduto6, codigoCorpoEmail, 6);
                }


                labErro.Text = "Bloco atualizado com sucesso. Por favor, atualizar a página do email.";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }


        private void SalvarProduto(TextBox txtSKU, Image imgProduto, int codigoCorpoEmail, int ordem)
        {
            var p = ecomController.ObterMagentoProdutoConfigPorSKU(txtSKU.Text.Trim());
            if (p != null)
            {
                var el = ecomController.ObterEmailCorpoLink(codigoCorpoEmail, ordem);
                if (el != null)
                {
                    el.ECOM_EMAIL_CORPO = codigoCorpoEmail;
                    el.SKU = txtSKU.Text.Trim();
                    el.LINK_REDIRECT = "https://handbookonline.com.br/" + p.URL_PRODUTO.Replace("--", "-").Replace(" ", "-") + ".html";
                    el.LINK_IMAGEM = imgProduto.ImageUrl;
                    el.PRECO = p.PRECO;
                    el.PRECO_PROMO = p.PRECO_PROMO;
                    el.ALT = p.NOME_MAG;

                    ecomController.AtualizarEmailCorpoLink(el);
                }
                else
                {
                    var elN = new ECOM_EMAIL_CORPO_LINK();
                    elN.ECOM_EMAIL_CORPO = codigoCorpoEmail;
                    elN.SKU = txtSKU.Text.Trim();
                    elN.LINK_REDIRECT = "https://handbookonline.com.br/" + p.URL_PRODUTO.Replace("--", "-").Replace(" ", "-") + ".html";
                    elN.LINK_IMAGEM = imgProduto.ImageUrl;
                    elN.PRECO = p.PRECO;
                    elN.PRECO_PROMO = p.PRECO_PROMO;
                    elN.ALT = p.NOME_MAG;
                    elN.ORDEM = ordem;

                    ecomController.InserirEmailCorpoLink(elN);

                }
            }
        }

        protected void txtSKU_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                TextBox txtSKU = (TextBox)sender;

                var sku = txtSKU.Text.Trim();

                if (sku != "")
                {
                    var p = ecomController.ObterMagentoProdutoConfigPorSKU(sku);
                    if (p != null)
                    {

                        List<ListItem> fotosLista = ObterListaDeFotos(p);

                        if (txtSKU.ID.ToLower().Contains("1"))
                        {
                            ddlFoto1.DataSource = fotosLista;
                            ddlFoto1.DataValueField = "Value";
                            ddlFoto1.DataTextField = "Text";
                            ddlFoto1.DataBind();

                            labPreco1.Text = "R$ " + p.PRECO.ToString();
                            labPrecoPromo1.Text = (p.PRECO_PROMO == null) ? "-" : ("R$ " + p.PRECO_PROMO.ToString());
                        }
                        else if (txtSKU.ID.ToLower().Contains("2"))
                        {
                            ddlFoto2.DataSource = fotosLista;
                            ddlFoto2.DataValueField = "Value";
                            ddlFoto2.DataTextField = "Text";
                            ddlFoto2.DataBind();

                            labPreco2.Text = "R$ " + p.PRECO.ToString();
                            labPrecoPromo2.Text = (p.PRECO_PROMO == null) ? "-" : ("R$ " + p.PRECO_PROMO.ToString());
                        }
                        else if (txtSKU.ID.ToLower().Contains("3"))
                        {
                            ddlFoto3.DataSource = fotosLista;
                            ddlFoto3.DataValueField = "Value";
                            ddlFoto3.DataTextField = "Text";
                            ddlFoto3.DataBind();

                            labPreco3.Text = "R$ " + p.PRECO.ToString();
                            labPrecoPromo3.Text = (p.PRECO_PROMO == null) ? "-" : ("R$ " + p.PRECO_PROMO.ToString());
                        }
                        else if (txtSKU.ID.ToLower().Contains("4"))
                        {
                            ddlFoto4.DataSource = fotosLista;
                            ddlFoto4.DataValueField = "Value";
                            ddlFoto4.DataTextField = "Text";
                            ddlFoto4.DataBind();

                            labPreco4.Text = "R$ " + p.PRECO.ToString();
                            labPrecoPromo4.Text = (p.PRECO_PROMO == null) ? "-" : ("R$ " + p.PRECO_PROMO.ToString());
                        }
                        else if (txtSKU.ID.ToLower().Contains("5"))
                        {
                            ddlFoto5.DataSource = fotosLista;
                            ddlFoto5.DataValueField = "Value";
                            ddlFoto5.DataTextField = "Text";
                            ddlFoto5.DataBind();

                            labPreco5.Text = "R$ " + p.PRECO.ToString();
                            labPrecoPromo5.Text = (p.PRECO_PROMO == null) ? "-" : ("R$ " + p.PRECO_PROMO.ToString());
                        }
                        else if (txtSKU.ID.ToLower().Contains("6"))
                        {
                            ddlFoto6.DataSource = fotosLista;
                            ddlFoto6.DataValueField = "Value";
                            ddlFoto6.DataTextField = "Text";
                            ddlFoto6.DataBind();

                            labPreco6.Text = "R$ " + p.PRECO.ToString();
                            labPrecoPromo6.Text = (p.PRECO_PROMO == null) ? "-" : ("R$ " + p.PRECO_PROMO.ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private List<ListItem> ObterListaDeFotos(ECOM_PRODUTO p)
        {
            var items = new List<ListItem>();

            items.Add(new ListItem { Value = "", Text = "" });
            if (p.FOTO_SHOPPING_URLEXT != null)
                items.Add(new ListItem { Value = p.FOTO_SHOPPING_URLEXT, Text = "Foto" });

            return items;
        }

        protected void ddlFoto_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlFoto = (DropDownList)sender;

            var linkImagem = ddlFoto.SelectedValue.Trim();

            if (linkImagem != null && linkImagem != "")
            {
                if (ddlFoto.ID.ToLower().Contains("1"))
                    imgProduto1.ImageUrl = "https://media.handbookonline.com.br/catalog/product" + linkImagem;
                else if (ddlFoto.ID.ToLower().Contains("2"))
                    imgProduto2.ImageUrl = "https://media.handbookonline.com.br/catalog/product" + linkImagem;
                else if (ddlFoto.ID.ToLower().Contains("3"))
                    imgProduto3.ImageUrl = "https://media.handbookonline.com.br/catalog/product" + linkImagem;
                else if (ddlFoto.ID.ToLower().Contains("4"))
                    imgProduto4.ImageUrl = "https://media.handbookonline.com.br/catalog/product" + linkImagem;
                else if (ddlFoto.ID.ToLower().Contains("5"))
                    imgProduto5.ImageUrl = "https://media.handbookonline.com.br/catalog/product" + linkImagem;
                else if (ddlFoto.ID.ToLower().Contains("6"))
                    imgProduto6.ImageUrl = "https://media.handbookonline.com.br/catalog/product" + linkImagem;
            }

        }


    }
}


