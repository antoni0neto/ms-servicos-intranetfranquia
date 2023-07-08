using DAL;
using System.Web;


public static class UsuarioCookie
{
    public static USUARIO RefreshSession(HttpCookie cookieUser, HttpCookie cookiePass)
    {
        if (cookieUser != null)
        {
            //renovar sessao
            var user = Criptografia.Decrypt(cookieUser.Value);
            var pass = Criptografia.Decrypt(cookiePass.Value);
            return new UsuarioController().ValidaUsuario(user, pass);
        }

        return null;
    }
}
