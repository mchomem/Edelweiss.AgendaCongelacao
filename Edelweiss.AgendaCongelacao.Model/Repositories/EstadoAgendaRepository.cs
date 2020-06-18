using Edelweiss.AgendaCongelacao.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edelweiss.AgendaCongelacao.Model.Repositories
{
    public class EstadoAgendaRepository: ICrud<EstadoAgenda>
    {
        #region Methods Basic CRUD

        public void Create(EstadoAgenda statusAgenda)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [dbo].[EstadoAgenda]");
            sql.Append("(");
            sql.Append(" Estado");
            sql.Append(", CorHexaCss");
            sql.Append(", Ordem");
            sql.Append(")");
            sql.Append("values");
            sql.Append("(");
            sql.Append(String.Format("Estado = '{0}'", statusAgenda.Estado));
            sql.Append(String.Format("CorHexaCss = '{0}'", statusAgenda.CorHexaCss));
            sql.Append(String.Format("Ordem = '{0}'", statusAgenda.Ordem));
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

        public void Update(EstadoAgenda statusAgenda)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("update");
            sql.Append(" [dbo].[EstadoAgenda]");
            sql.Append(" set");
            sql.Append(String.Format("Estado = '{0}'", statusAgenda.Estado));
            sql.Append(String.Format(", CorHexaCss = '{0}'", statusAgenda.CorHexaCss));
            sql.Append(String.Format(", Ordem = '{0}'", statusAgenda.Ordem));
            sql.Append(" where");
            sql.Append(String.Format(" EstadoAgendaID = {0}", statusAgenda.EstadoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public void Delete(EstadoAgenda statusAgenda)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("delete from [dbo].[EstadoAgenda]");
            sql.Append(" where");
            sql.Append(String.Format(" EstadoAgendaID = {0}", statusAgenda.EstadoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public EstadoAgenda Details(EstadoAgenda info)
        {
            DataBase dataBase = new DataBase();
            EstadoAgenda statusAgenda = new EstadoAgenda();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" ea.EstadoAgendaID");
            sql.Append(", ea.Estado");
            sql.Append(", ea.CorHexaCss");
            sql.Append(", ea.Ordem");
            sql.Append(" from");
            sql.Append(" [dbo].[EstadoAgenda] ea");
            sql.Append(" where");
            sql.Append(String.Format(" ea.EstadoAgendaID = {0}", info.EstadoAgendaID.Value));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            statusAgenda.EstadoAgendaID = Convert.ToInt32(dr["EstadoAgendaID"]);
                            statusAgenda.Estado = dr["Estado"].ToString();
                            statusAgenda.CorHexaCss = dr["CorHexaCss"].ToString();
                            statusAgenda.Ordem = Convert.ToInt32(dr["Ordem"]);
                        }
                    }
                }
            }

            return statusAgenda;
        }

        public List<EstadoAgenda> Retreave(EstadoAgenda statusAgenda)
        {
            DataBase dataBase = new DataBase();
            List<EstadoAgenda> statusAgendas = new List<EstadoAgenda>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" ea.EstadoAgendaID");
            sql.Append(", ea.Estado");
            sql.Append(", ea.CorHexaCss");
            sql.Append(", ea.Ordem");
            sql.Append(" from");
            sql.Append(" [dbo].[EstadoAgenda] ea");
            sql.Append(" where");
            sql.Append(" 1 = 1");

            if (statusAgenda.EstadoAgendaID.HasValue)
                sql.Append(String.Format(" and ea.EstadoAgendaID = {0}", statusAgenda.EstadoAgendaID));

            if (statusAgenda.Estado != null)
                sql.Append(String.Format(" and ea.Estado like '%{0}%'", statusAgenda.Estado));

            if (statusAgenda.CorHexaCss != null)
                sql.Append(String.Format(" and ea.CorHexaCss like '%{0}%'", statusAgenda.CorHexaCss));

            if (statusAgenda.Ordem != null)
                sql.Append(String.Format(" and ea.Ordem = {0}", statusAgenda.Ordem));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            EstadoAgenda sa = new EstadoAgenda();
                            sa.EstadoAgendaID = Convert.ToInt32(dr["EstadoAgendaID"]);
                            sa.Estado = dr["Estado"].ToString();
                            statusAgenda.CorHexaCss = dr["CorHexaCss"].ToString();
                            statusAgenda.Ordem = Convert.ToInt32(dr["Ordem"]);
                            statusAgendas.Add(sa);
                        }
                    }
                }
            }

            return statusAgendas;
        }

        #endregion
    }
}
