using Edelweiss.Utils;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Edelweiss.AgendaCongelacao.Model
{
    /// <summary>
    /// Classe de conexão a banco de dados
    /// </summary>
    public class DataBase
    {
        /// <summary>
        /// Conexão Sql
        /// </summary>
        public SqlConnection sqlConnection { get; set; }

        /// <summary>
        /// Retorna a conexão do banco de dados da Rastreabilidade LabEdelweiss
        /// </summary>
        /// <returns>SqlConnection</returns>
        public SqlConnection RetornaConexaoRastreabilidade()
        {
            sqlConnection = new SqlConnection();

            sqlConnection.ConnectionString = RetornaConnectionString("dbRastreabilidade");

            return sqlConnection;
        }

        /// <summary>
        /// Retorna a conexão do banco de dados do Diagnostico Web
        /// </summary>
        /// <returns>SqlConnection</returns>
        public SqlConnection RetornaConexaoDiagnosticoWeb()
        {
            sqlConnection = new SqlConnection();

            sqlConnection.ConnectionString = RetornaConnectionString("dbDiagnosticoWeb");

            return sqlConnection;
        }

        /// <summary>
        /// Retorna a conexão de banco de dados do LabEdelweiss
        /// (base global com dados sobre Area, Feriados, Filial, Perfil, Sistemas e Usuário)
        /// </summary>
        /// <returns></returns>
        public SqlConnection RetornaConexaoLabEldeweiss()
        {
            sqlConnection = new SqlConnection();

            sqlConnection.ConnectionString = RetornaConnectionString("dbLabEdelweiss");

            return sqlConnection;
        }

        /// <summary>
        /// Retorna a connection string de uma base de dados
        /// </summary>
        /// <param name="Database"></param>
        /// <returns>String</returns>
        private static string RetornaConnectionString(string Database)
        {
            string connectioString = ConfigurationManager.ConnectionStrings[Database].ToString();
            return connectioString;
        }

        /// <summary>
        /// Testa a conexão com a base de dados do SQL e retorna em caso de sucesso
        /// </summary>
        /// <returns>Retorna true ou false</returns>
        public static Boolean TestarConexaoSQLServer()
        {
            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["rastreabilidade"].ToString()))
            {
                //Teste de conexão com o SQL
                try
                {
                    sqlConn.Open();
                    return true;
                }
                catch (Exception e)
                {
                    Log.Create(e);
                    Email.Send("Notificador - falha na aplicação", e);
                    return false;
                }
            }
        }
    }
}
