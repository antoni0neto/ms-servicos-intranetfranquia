using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace DAL
{
    public class ActiveDirectoryController
    {
        private string _filterAttribute;
        private string _connectionStringAD = Constante.ConnectionStringAD;

        public bool UsuarioAutenticadoAD(string p_user, string p_password)
        {
            DirectoryEntry entry = new DirectoryEntry(_connectionStringAD, p_user, p_password);

            //Validar usuário no AD
            try
            {
                //Validar Usuário no AD
                Object obj = entry.NativeObject;
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                //Criar objeto de busca no AD
                DirectorySearcher search = new DirectorySearcher(entry);

                //Relacionar os filtros e buscar
                search.Filter = "(sAMAccountName=" + p_user + ")";

                //search.PropertiesToLoad.Add("ou");
                SearchResult result = search.FindOne();

                if (result == null)
                    return false;

                //Atualizar path
                this._connectionStringAD = result.Path;
                this._filterAttribute = (string)result.Properties["CN"][0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                entry.Close();
            }

            return true;
        }
        public SearchResultCollection ObterListaUsuariosAD()
        {
            DirectoryEntry entry = new DirectoryEntry(_connectionStringAD, "datasync", "cmax2207");

            //Validar usuário no AD
            try
            {
                //Validar Usuário no AD
                Object obj = entry.NativeObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {
                var ds = new DirectorySearcher(entry, "(&(objectClass=user)(objectCategory=person))");

                //Adiciona propriedades para leitura
                ds.PropertiesToLoad.Add("cn");
                ds.PropertiesToLoad.Add("userPrincipalName");
                ds.PropertiesToLoad.Add("sAMAccountName");
                ds.PropertiesToLoad.Add("memberOf");
                ds.PropertiesToLoad.Add("displayName");
                ds.PropertiesToLoad.Add("mail");
                ds.PropertiesToLoad.Add("department");
                ds.Sort.PropertyName = "sAMAccountName";

                SearchResultCollection users = ds.FindAll();
                if (users != null)
                {
                    return users;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                entry.Close();
            }
        }
        public DirectoryEntry ObterGruposUsuario(string p_user)
        {
            DirectoryEntry entry = new DirectoryEntry(_connectionStringAD, "datasync", "cmax2207");

            //Validar usuário no AD
            try
            {
                //Validar Usuário no AD
                Object obj = entry.NativeObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {
                //Criar objeto de busca no AD
                DirectorySearcher search = new DirectorySearcher(entry);

                //Relacionar os filtros e buscar
                search.Filter = "(sAMAccountName=" + p_user + ")";

                //search.PropertiesToLoad.Add("ou");
                SearchResult result = search.FindOne();

                if (result != null)
                    return result.GetDirectoryEntry();

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                entry.Close();
            }

        }

    }
}
