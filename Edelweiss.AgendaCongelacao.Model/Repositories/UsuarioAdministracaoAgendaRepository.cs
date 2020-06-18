using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edelweiss.AgendaCongelacao.Model.Repositories
{
    public class UsuarioAdministracaoAgendaRepository: ICrud<UsuarioAdministracaoAgenda>
    {
        #region Methods Basic CRUD

        public void Create(UsuarioAdministracaoAgenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [dbo].[UsuarioAdministracaoAgenda]");
            sql.Append(" (");
            sql.Append("[Login]");
            sql.Append(", Senha");
            sql.Append(", Nome");
            sql.Append(")");
            sql.Append(" values ");
            sql.Append("(");
            sql.Append(String.Format("'{0}'", info.Login));
            sql.Append(String.Format(", '{0}'", Cypher.Encrypt(info.Senha)));
            sql.Append(String.Format(", '{0}'", info.Nome));
            sql.Append(")");

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public void Update(UsuarioAdministracaoAgenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("update [dbo].[UsuarioAdministracaoAgenda]");
            sql.Append(" set");
            sql.Append(String.Format(" [Login] = '{0}'", info.Login));
            sql.Append(String.Format(", Senha = '{0}'", Cypher.Encrypt(info.Senha)));
            sql.Append(String.Format(", Nome = '{0}'", info.Nome));
            sql.Append(" where");
            sql.Append(String.Format(" UsuarioAdministracaoAgendaID = {0}", info.UsuarioAdministracaoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public void Delete(UsuarioAdministracaoAgenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("update");
            sql.Append(" [dbo].[UsuarioAdministracaoAgenda]");
            sql.Append(" set");
            sql.Append(String.Format(" Ativo = {0}", 0));
            sql.Append(" where");
            sql.Append(String.Format(" UsuarioAdministracaoAgendaID = {0}", info.UsuarioAdministracaoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public UsuarioAdministracaoAgenda Details(UsuarioAdministracaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            UsuarioAdministracaoAgenda usuario = new UsuarioAdministracaoAgenda();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" uaa.UsuarioAdministracaoAgendaID");
            sql.Append(", uaa.[Login]");
            sql.Append(", uaa.Senha");
            sql.Append(", uaa.Nome");
            sql.Append(", uaa.Ativo");
            sql.Append(" from");
            sql.Append(" [dbo].[UsuarioAdministracaoAgenda] uaa");
            sql.Append(" where");
            sql.Append(String.Format(" uaa.UsuarioAdministracaoAgendaID = {0}", info.UsuarioAdministracaoAgendaID.Value));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            usuario.UsuarioAdministracaoAgendaID = Convert.ToInt32(dr["UsuarioAdministracaoAgendaID"]);
                            usuario.Login = dr["Login"].ToString();
                            usuario.Senha = dr["Senha"].ToString();
                            usuario.Nome = dr["Nome"].ToString();
                            usuario.Ativo = Convert.ToBoolean(dr["Ativo"]);
                        }
                    }
                }
            }

            return usuario;
        }

        public List<UsuarioAdministracaoAgenda> Retreave(UsuarioAdministracaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            List<UsuarioAdministracaoAgenda> usuarios = new List<UsuarioAdministracaoAgenda>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" uaa.UsuarioAdministracaoAgendaID");
            sql.Append(", uaa.Login");
            sql.Append(", uaa.Senha");
            sql.Append(", uaa.Nome");
            sql.Append(", uaa.Ativo");
            sql.Append(" from");
            sql.Append(" [dbo].[UsuarioAdministracaoAgenda] uaa");
            sql.Append(" where");
            sql.Append(" 1 = 1");

            if (info.UsuarioAdministracaoAgendaID.HasValue)
                sql.Append(String.Format(" and uaa.UsuarioAdministracaoAgendaID = {0}", info.UsuarioAdministracaoAgendaID.Value));

            if (info.Login != null)
                sql.Append(String.Format(" and uaa.Login like '%{0}%'", info.Login));

            if (info.Senha != null)
                sql.Append(String.Format(" and uaa.Senha like '%{0}%'", info.Senha));

            if (info.Nome != null)
                sql.Append(String.Format(" and uaa.Nome like '%{0}%'", info.Nome));

            if (info.Ativo.HasValue)
                sql.Append(String.Format(" and uaa.Ativo = {0}", (info.Ativo.Value ? 1 : 0)));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            UsuarioAdministracaoAgenda usuario = new UsuarioAdministracaoAgenda();
                            usuario.UsuarioAdministracaoAgendaID = Convert.ToInt32(dr["UsuarioAdministracaoAgendaID"]);
                            usuario.Login = dr["Login"].ToString();
                            usuario.Senha = dr["Senha"].ToString();
                            usuario.Nome = dr["Nome"].ToString();
                            usuario.Ativo = Convert.ToBoolean(dr["Ativo"]);
                            usuarios.Add(usuario);
                        }
                    }
                }
            }

            return usuarios;
        }

        #endregion

        public UsuarioAdministracaoAgenda Authenticate(UsuarioAdministracaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            UsuarioAdministracaoAgenda usuario = null;

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" uaa.UsuarioAdministracaoAgendaID");
            sql.Append(", uaa.[Login]");
            sql.Append(", uaa.Senha");
            sql.Append(", uaa.Nome");
            sql.Append(", uaa.Ativo");
            sql.Append(" from");
            sql.Append(" [dbo].[UsuarioAdministracaoAgenda] uaa");
            sql.Append(" where");
            sql.Append(String.Format(" uaa.[Login] = '{0}'", info.Login));
            sql.Append(String.Format(" and uaa.Senha = '{0}'", info.Senha));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            usuario = new UsuarioAdministracaoAgenda();
                            usuario.UsuarioAdministracaoAgendaID = Convert.ToInt32(dr["UsuarioAdministracaoAgendaID"]);
                            usuario.Login = dr["Login"].ToString();
                            usuario.Senha = Cypher.Decrypt(dr["Senha"].ToString());
                            usuario.Nome = dr["Nome"].ToString();
                            usuario.Ativo = Convert.ToBoolean(dr["Ativo"]);
                        }
                    }
                }
            }

            return usuario;
        }
    }
}
