using Edelweiss.AgendaCongelacao.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edelweiss.AgendaCongelacao.Model.Repositories
{
    public class MedicoExecucaoAgendaRepository : ICrud<MedicoExecucaoAgenda>
    {
        #region Methods Basic CRUD

#pragma warning disable CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
        public void Create(MedicoExecucaoAgenda info)
#pragma warning restore CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [dbo].[MedicoExecucaoAgenda]");
            sql.Append(" (");
            sql.Append("Nome");
            sql.Append(", Email");
            sql.Append(", Celular");
            sql.Append(")");
            sql.Append(" values ");
            sql.Append("(");
            sql.Append(String.Format("'{0}'", info.Nome));
            sql.Append(String.Format(", '{0}'", info.Email));
            sql.Append(String.Format(", '{0}'", info.Celular));
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

#pragma warning disable CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
        public void Update(MedicoExecucaoAgenda info)
#pragma warning restore CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("update [dbo].[MedicoExecucaoAgenda]");
            sql.Append(" set");
            sql.Append(String.Format(" Nome = '{0}'", info.Nome));
            sql.Append(String.Format(", Email = '{0}'", info.Email));
            sql.Append(String.Format(", Celular = '{0}'", info.Celular));
            sql.Append(" where");
            sql.Append(String.Format(" MedicoExecucaoAgendaID = {0}", info.MedicoExecucaoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
        public void Delete(MedicoExecucaoAgenda info)
#pragma warning restore CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("delete from [dbo].[MedicoExecucaoAgenda]");
            sql.Append(" where");
            sql.Append(String.Format(" MedicoExecucaoAgendaID = {0}", info.MedicoExecucaoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
        public MedicoExecucaoAgenda Details(MedicoExecucaoAgenda info)
#pragma warning restore CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
        {
            DataBase dataBase = new DataBase();
            MedicoExecucaoAgenda medicoExecucaoAgenda = new MedicoExecucaoAgenda();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" mea.MedicoExecucaoAgendaID");
            sql.Append(", mea.Nome");
            sql.Append(", mea.Email");
            sql.Append(", mea.Celular");
            sql.Append(" from");
            sql.Append(" [dbo].[MedicoExecucaoAgenda] mea");
            sql.Append(" where");
            sql.Append(String.Format(" mea.MedicoExecucaoAgendaID = {0}", info.MedicoExecucaoAgendaID.Value));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            medicoExecucaoAgenda.MedicoExecucaoAgendaID = Convert.ToInt32(dr["MedicoExecucaoAgendaID"]);
                            medicoExecucaoAgenda.Nome = dr["Nome"].ToString();
                            medicoExecucaoAgenda.Email = dr["Email"].ToString();
                            medicoExecucaoAgenda.Celular = dr["Celular"].ToString();
                        }
                    }
                }
            }

            return medicoExecucaoAgenda;
        }

#pragma warning disable CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
        public List<MedicoExecucaoAgenda> Retreave(MedicoExecucaoAgenda info)
#pragma warning restore CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'MedicoExecucaoAgenda' could not be found (are you missing a using directive or an assembly reference?)
        {
            DataBase dataBase = new DataBase();
            List<MedicoExecucaoAgenda> medicoExecucaoAgendas = new List<MedicoExecucaoAgenda>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" mea.MedicoExecucaoAgendaID");
            sql.Append(", mea.Nome");
            sql.Append(", mea.Email");
            sql.Append(", mea.Celular");
            sql.Append(" from");
            sql.Append(" [dbo].[MedicoExecucaoAgenda] mea");
            sql.Append(" where");
            sql.Append(" 1 = 1");

            if (info.MedicoExecucaoAgendaID.HasValue)
                sql.Append(String.Format(" and mea.MedicoExecucaoAgendaID = {0}", info.MedicoExecucaoAgendaID));

            if (info.Nome != null)
                sql.Append(String.Format(" and mea.Nome like '%{0}%'", info.Nome));

            if (info.Email != null)
                sql.Append(String.Format(" and mea.Email like '%{0}%'", info.Email));

            if (info.Celular != null)
                sql.Append(String.Format(" and mea.Celular = {0}", info.Celular));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            MedicoExecucaoAgenda medicoExecucaoAgenda = new MedicoExecucaoAgenda();
                            medicoExecucaoAgenda.MedicoExecucaoAgendaID = Convert.ToInt32(dr["MedicoExecucaoAgendaID"]);
                            medicoExecucaoAgenda.Nome = dr["Nome"].ToString();
                            medicoExecucaoAgenda.Email = dr["Email"].ToString();
                            medicoExecucaoAgenda.Celular = dr["Celular"].ToString();

                            medicoExecucaoAgendas.Add(medicoExecucaoAgenda);
                        }
                    }
                }
            }

            return medicoExecucaoAgendas;
        }

        #endregion
    }
}
