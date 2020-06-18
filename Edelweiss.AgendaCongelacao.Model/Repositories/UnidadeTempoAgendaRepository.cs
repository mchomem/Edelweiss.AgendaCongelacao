using Edelweiss.AgendaCongelacao.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edelweiss.AgendaCongelacao.Model.Repositories
{
    public class UnidadeTempoAgendaRepository : ICrud<UnidadeTempoAgenda>
    {
        #region Methods Basic CRUD

        public void Create(UnidadeTempoAgenda info)
        {
            DataBase dataBase = new DataBase();
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [dbo].[UnidadeTempoAgenda]");
            sql.Append("(");
            sql.Append("Unidade");
            sql.Append(")");
            sql.Append("values");
            sql.Append("(");
            sql.Append(String.Format("'{0}'", info.Unidade));
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

        public void Update(UnidadeTempoAgenda info)
        {
            DataBase dataBase = new DataBase();
            StringBuilder sql = new StringBuilder();
            sql.Append("update [dbo].[UnidadeTempoAgenda]");
            sql.Append(" set");
            sql.Append(String.Format(" Unidade = '{0}'", info.Unidade));
            sql.Append(" where ");
            sql.Append(String.Format("UnidadeTempoAgendaID = {0}", info.Unidade));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public void Delete(UnidadeTempoAgenda info)
        {
            DataBase dataBase = new DataBase();
            StringBuilder sql = new StringBuilder();
            sql.Append("delete");
            sql.Append(" from");
            sql.Append(" [dbo].[UnidadeTempoAgenda]");
            sql.Append(" where");
            sql.Append(String.Format("UnidadeTempoAgendaID = {0}", info.UnidadeTempoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public UnidadeTempoAgenda Details(UnidadeTempoAgenda info)
        {
            DataBase dataBase = new DataBase();
            UnidadeTempoAgenda unidadeTempoAgenda = new UnidadeTempoAgenda();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" uta.UnidadeTempoAgendaID");
            sql.Append(", uta.Unidade");
            sql.Append(" from");
            sql.Append(" [dbo].[UnidadeTempoAgenda] uta");
            sql.Append(" where");
            sql.Append(String.Format(" UnidadeTempoAgendaID = {0}", info.UnidadeTempoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            unidadeTempoAgenda.UnidadeTempoAgendaID = Convert.ToInt32(dr["UnidadeTempoAgendaID"]);
                            unidadeTempoAgenda.Unidade = dr["Unidade"].ToString();
                        }
                    }
                }
            }

            return unidadeTempoAgenda;
        }

        public List<UnidadeTempoAgenda> Retreave(UnidadeTempoAgenda info)
        {
            DataBase dataBase = new DataBase();
            List<UnidadeTempoAgenda> unidadeTempoAgendas = new List<UnidadeTempoAgenda>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" uta.UnidadeTempoAgendaID");
            sql.Append(", uta.Unidade");
            sql.Append(" from");
            sql.Append(" [dbo].[UnidadeTempoAgenda] uta");
            sql.Append(" where");
            sql.Append(" 1 = 1");

            if (info.UnidadeTempoAgendaID.HasValue)
                sql.Append(String.Format(" and uta.UnidadeTempoAgendaID = {0}", info.UnidadeTempoAgendaID.Value));

            if (info.Unidade != null)
                sql.Append(String.Format(" and uta.Unidade like '%{0}%'", info.Unidade));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            UnidadeTempoAgenda unidadeTempoAgenda = new UnidadeTempoAgenda();
                            unidadeTempoAgenda.UnidadeTempoAgendaID = Convert.ToInt32(dr["UnidadeTempoAgendaID"]);
                            unidadeTempoAgenda.Unidade = dr["Unidade"].ToString();
                            unidadeTempoAgendas.Add(unidadeTempoAgenda);
                        }
                    }
                }
            }

            return unidadeTempoAgendas;
        }

        #endregion
    }
}
