using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;
using System.Collections;
using Renci.SshNet;
using MySql.Data.MySqlClient;
using System.Data;

namespace DAL
{
    public class EcomMySqlController
    {
        const string sshIP = "104.131.82.141";
        const int sshPort = 9022;
        const string sshUser = "moustache";
        const string sshPass = "m0u5t4ch3%";

        private ConnectionInfo conInfo;
        private MySqlConnection con;
        private SshClient client;

        public EcomMySqlController()
        {
            AbrirConexao();
        }

        private void AbrirConexao()
        {
            conInfo = new ConnectionInfo(sshIP, sshPort, sshUser, new PasswordAuthenticationMethod(sshUser, sshPass));
            conInfo.Timeout = TimeSpan.FromSeconds(30);

            client = new SshClient(conInfo);
            client.Connect();

            var localHost = "127.0.0.3";
            uint localPort = 3307;
            var sqlHostRemote = "10.132.153.78";
            uint sqlPortRemote = 3306;

            var portForwarded = new ForwardedPortLocal(localHost, localPort, sqlHostRemote, sqlPortRemote);
            client.AddForwardedPort(portForwarded);
            portForwarded.Start();

            if (portForwarded.IsStarted)
            {
                var database = "handbook";
                var uid = "leo";
                var password = "@Hand357book*";
                var connectionString = "SERVER=" + localHost + ";port=" + localPort + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";SslMode=none";

                con = new MySqlConnection(connectionString);
                con.Open();
            }
        }
        public void FecharConexao()
        {
            if (con.State == ConnectionState.Open)
                con.Close();

            client.Disconnect();
        }

        //relevancia
        public void AtualizarRelevanciaProdutoCat(string position, string category_id, string produtc_id)
        {
            string queryUp = "UPDATE handbook.catalog_category_product SET position='" + position + "' WHERE category_id='" + category_id + "' and product_id='" + produtc_id + "';";
            using (MySqlCommand com = new MySqlCommand(queryUp, con))
            {
                com.CommandType = CommandType.Text;
                var retReader = com.ExecuteReader();
            }
        }

        //produtos relacionados
        public void ExcluirProdutoRelacionado(string product_id)
        {
            string queryDelPos = "DELETE FROM handbook.catalog_product_link_attribute_int where link_id in (SELECT link_id FROM handbook.catalog_product_link where product_id = '" + product_id + "');";
            using (MySqlCommand com = new MySqlCommand(queryDelPos, con))
            {
                com.CommandType = CommandType.Text;
                var ret = com.ExecuteReader();
            }

            string queryDelLinked = "DELETE FROM handbook.catalog_product_link where product_id = '" + product_id + "';";
            using (MySqlCommand com = new MySqlCommand(queryDelLinked, con))
            {
                com.CommandType = CommandType.Text;
                var ret = com.ExecuteReader();
            }
        }
        public void InserirProdutoRelacionado(string product_id, string linked_product_id, string link_type_id, string position)
        {
            string queryIn = "INSERT INTO handbook.catalog_product_link (product_id, linked_product_id, link_type_id) VALUES ('" + product_id + "', '" + linked_product_id + "', '" + link_type_id + "'); select last_insert_id();";
            var link_id = 0;
            using (MySqlCommand com = new MySqlCommand(queryIn, con))
            {
                com.CommandType = CommandType.Text;
                var retReader = com.ExecuteReader();
                if (retReader.HasRows)
                {
                    if (retReader.Read())
                    {
                        link_id = Convert.ToInt32(retReader.GetValue(0));
                    }
                }
            }

            if (link_id > 0)
            {
                string queryInPos = "INSERT INTO handbook.catalog_product_link_attribute_int (product_link_attribute_id, link_id, value) values ('" + link_type_id + "', '" + link_id + "', '" + position + "');";
                using (MySqlCommand com = new MySqlCommand(queryInPos, con))
                {
                    com.CommandType = CommandType.Text;
                    var ret = com.ExecuteReader();
                }
            }
        }


        public void InserirProdutoCategoria(string product_id, string category_id, string position = "1")
        {
            string queryDelPos = "DELETE FROM handbook.catalog_category_product where product_id = '" + product_id + "' and category_id = '" + category_id + "'  ;";
            using (MySqlCommand com = new MySqlCommand(queryDelPos, con))
            {
                com.CommandType = CommandType.Text;
                var ret = com.ExecuteReader();
            }

            string queryInPos = "INSERT INTO handbook.catalog_category_product (category_id, product_id, position) values ('" + category_id + "', '" + product_id + "', '" + position + "');";
            using (MySqlCommand com = new MySqlCommand(queryInPos, con))
            {
                com.CommandType = CommandType.Text;
                var ret = com.ExecuteReader();
            }
            

        }


    }
}
