using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Relatorios
{
    public partial class Login : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        FuncionarioLojaController funcionarioLojaController = new FuncionarioLojaController();
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserName.Attributes.Add("onKeyPress", "doClick('" + LoginButton.ClientID + "',event)");
                Password.Attributes.Add("onKeyPress", "doClick('" + LoginButton.ClientID + "',event)");
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                USUARIO usuario = usuarioController.ValidaUsuario(UserName.Text, Criptografia.Encrypt(Password.Text));

                if (usuario == null)
                    usuario = usuarioController.ValidaUsuario(UserName.Text, Password.Text);

                if (usuario == null || usuario.DATA_EXCLUSAO != null)
                {
                    labErro.Text = "Usuário e/ou Senha Inválida.";
                    return;
                }

                if (usuario != null)
                {
                    USUARIO_LOGIN ul = new USUARIO_LOGIN();

                    ul.CODIGO_USUARIO = usuario.CODIGO_USUARIO;

                    ul.DATA_LOGIN = DateTime.Now;

                    baseController.GravarLogin(ul);

                    LimpaSession();

                    List<Produto> listaProduto = new List<Produto>();

                    Session["PRODUTO"] = listaProduto;
                    Session["USUARIO"] = usuario;

                    Utils.SessionManager.Armazenar<USUARIO>(usuario, "USUARIO_LOGADO");

                    HttpCookie cookieUser = new HttpCookie("_resu");
                    HttpCookie cookiePass = new HttpCookie("_resuss");
                    cookieUser.Value = Criptografia.Encrypt(usuario.USUARIO1);
                    cookiePass.Value = Criptografia.Encrypt(usuario.SENHA);
                    Response.Cookies.Add(cookieUser);
                    Response.Cookies.Add(cookiePass);


                    if (usuario.CODIGO_PERFIL == 1)                          // Administrador
                        Response.Redirect("~/menu/menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 2)                     // Gerente de Loja
                        Response.Redirect("~/DefaultFilial.aspx");
                    else if (usuario.CODIGO_PERFIL == 3)                     // Supervisor de Loja
                        Response.Redirect("~/DefaultFilial.aspx");
                    else if (usuario.CODIGO_PERFIL == 4 || usuario.CODIGO_PERFIL == 47) // Supervisor de Atacado
                        Response.Redirect("~/mod_atacado/atac_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 5)                     // Financeiro
                        Response.Redirect("~/mod_financeiro/fin_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 6)                     // Modulo de Controle de Estoque
                        Response.Redirect("~/mod_estoque/estoque_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 8)                     // Caixa
                        Response.Redirect("~/DefaultCaixaFilial.aspx");
                    else if (usuario.CODIGO_PERFIL == 10)                    // Adm_Nf
                        Response.Redirect("~/DefaultAdmNF.aspx");
                    else if (usuario.CODIGO_PERFIL == 11)                    // Rh
                        Response.Redirect("~/mod_rh/rh_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 12)                    // Gestão
                        Response.Redirect("~/DefaultGestor.aspx");
                    else if (usuario.CODIGO_PERFIL == 13 || usuario.CODIGO_PERFIL == 35) // Fiscal
                        Response.Redirect("~/mod_fiscal/fisc_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 15)                    // Importação
                        Response.Redirect("~/DefaultMenuImportacao.aspx");
                    else if (usuario.CODIGO_PERFIL == 16)                    // Administrador do Site
                        Response.Redirect("~/DefaultMenuAdmSite.aspx");
                    else if (usuario.CODIGO_PERFIL == 17)                    // Administrador Financeiro
                        Response.Redirect("~/mod_contabilidade/contabil_fin_amensal_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 18) // Módulo de Produção
                        Response.Redirect("~/mod_producao/prod_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 19)                    // Módulo de Desenvolvimento Perfil 1
                        Response.Redirect("~/mod_desenvolvimento/desenv_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 20)                    // Módulo de Desenvolvimento Perfil 2
                        Response.Redirect("~/mod_desenvolvimento/desenv_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 21)                    // Módulo de Jurídico
                        Response.Redirect("~/mod_juridico/jur_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 24 || usuario.CODIGO_PERFIL == 34) // Módulo de Contabilidade
                        Response.Redirect("~/mod_contabilidade/contabil_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 25)                    // Módulo Financeiro/Contabilidade
                        Response.Redirect("~/mod_contabilidade/contabil_financeiro_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 26 || usuario.CODIGO_PERFIL == 44 || usuario.CODIGO_PERFIL == 45 || usuario.CODIGO_PERFIL == 46) // FACCAO
                        Response.Redirect("~/mod_faccao/facc_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 27)                    // FINANCEIRO/PRODUCAO
                        Response.Redirect("~/mod_financeiro/fin_prod_fin_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 28)                    // FACCAO RELATORIO
                        Response.Redirect("~/mod_faccao/facc_menu_relatorio.aspx");
                    else if (usuario.CODIGO_PERFIL == 29)                    // SEGUROS
                        Response.Redirect("~/mod_seguro/seg_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 30)                    // CONTROLE DE PRODUTO
                        Response.Redirect("~/mod_controle_produto/cprod_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 31)                    // ADM/FACÇÃO/ESTOQUE
                        Response.Redirect("~/mod_estoque/estoque_adm_facc_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 32)                    // PRODUÇÃO/CONTROLE DE PRODUTO
                        Response.Redirect("~/mod_controle_produto/cprod_producao_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 33)                    // FACCAO ADM
                        Response.Redirect("~/mod_faccao_adm/faccadm_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 33)                    // FACCAO ADM
                        Response.Redirect("~/mod_faccao_adm/faccadm_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 36)                    // CONTAGEM
                        Response.Redirect("~/mod_contagem/cont_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 37)                    // CONTABILIDADE/FINANCEIRO
                        Response.Redirect("~/mod_contabilidade/contabil_faccao_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 38)                    // CONTABILIDADE/SEGURO/DRE
                        Response.Redirect("~/mod_contabilidade/contabil_seguro_dre_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 39)                    // PRODUÇÃO/FACÇÃO
                        Response.Redirect("~/mod_faccao/facc_menu_producao.aspx");
                    else if (usuario.CODIGO_PERFIL == 40)                    // ADM/PRODUÇÃO/FACÇÃO/ESTOQUE/CONTAGEM
                        Response.Redirect("~/mod_estoque/estoque_cont_facc_prod_nf_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 41)                    // ACOMPANHAMENTO MENSAL
                        Response.Redirect("~/mod_acomp_mensal/acomp_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 42)                    // CAD
                        Response.Redirect("~/mod_producao/prod_menu_cad.aspx");
                    else if (usuario.CODIGO_PERFIL == 43)                    // E-COMMERCE
                        Response.Redirect("~/mod_ecom/ecom_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 49)                    // MARKETING
                        Response.Redirect("~/mod_marketing/mkt_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 50)                    // MARKETING/GERENCIAMENTO DE LOJA
                        Response.Redirect("~/mod_marketing/mkt_ger_loja_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 51)                    // MARKETING/ECOM/DESENV PRODUTO
                        Response.Redirect("~/mod_ecom/ecom_mkt_desenv_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 52)                    // ECOM RECEPÇÃO
                        Response.Redirect("~/mod_ecom/ecom_recepcao_menu.aspx");
                    else if (usuario.CODIGO_PERFIL == 53)                    // DRE
                        Response.Redirect("~/mod_acomp_mensal/dre/dre_dre_novo.aspx");
                    else if (usuario.CODIGO_PERFIL == 54)                    // Faccao/Fiscal
                        Response.Redirect("~/mod_faccao/facc_menu_fiscal.aspx");
                    else if (usuario.CODIGO_PERFIL == 55)                    // E-COMMERCE
                        Response.Redirect("~/mod_ecom/ecom_menu_externo.aspx");
                    else if (usuario.CODIGO_PERFIL == 56)                    // VENDAS REPRESENTANTE
                        Response.Redirect("~/mod_atacado/atac_menu_externo.aspx");
                    else if (usuario.CODIGO_PERFIL == 57)                    // WHATSAPP
                        Response.Redirect("~/mod_gerenciamento_loja/gerloja_wapp_niver.aspx");
                    else if (usuario.CODIGO_PERFIL == 58)                    // IGOR
                        Response.Redirect("~/photo_select/cad_phXHD123/photo_menu.aspx");

                    else
                    {
                        labErro.Text = "Usuário não possui Perfil cadastrado.";
                    }
                }
                else
                {
                    labErro.Text = "Usuário e/ou Senha Inválida.";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void LimpaSession()
        {
            Session["USUARIO"] = null;
        }
    }
}