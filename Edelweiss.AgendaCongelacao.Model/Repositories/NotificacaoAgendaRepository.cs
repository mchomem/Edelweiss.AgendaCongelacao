using Edelweiss.AgendaCongelacao.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edelweiss.AgendaCongelacao.Model.Repositories
{
    public class NotificacaoAgendaRepository : ICrud<NotificacaoAgenda>
    {
        #region Methods (roles)

        public void UtilizarNotificacao(NotificacaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            StringBuilder sql = new StringBuilder();
            sql.Append("update");
            sql.Append(" [dbo].[NotificacaoAgenda]");
            sql.Append(" set");
            sql.Append(String.Format(" Utilizado = {0}", info.Ativo.Value ? 1 : 0));
            sql.Append(" where");
            sql.Append(String.Format(" AgendaID = {0}", info.Agenda.AgendaID));
            sql.Append(String.Format(" and ConfiguracaoNotificacaoAgendaID = {0}", info.ConfiguracaoNotificacaoAgenda.ConfiguracaoNotificacaoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public void ReativarNotificacao(NotificacaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            StringBuilder sql = new StringBuilder();
            sql.Append("update");
            sql.Append(" [dbo].[NotificacaoAgenda]");
            sql.Append(" set");
            sql.Append(String.Format(" Utilizado = {0}", info.Utilizado.Value ? 1 : 0));
            sql.Append(String.Format(", Ativo = {0}", info.Ativo.Value ? 1 : 0));
            sql.Append(" where");
            sql.Append(String.Format(" AgendaID = {0}", info.Agenda.AgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        #endregion

        #region Methods Basic CRUD

        public void Create(NotificacaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [dbo].[NotificacaoAgenda]");
            sql.Append("(");
            sql.Append("AgendaID");
            sql.Append(", ConfiguracaoNotificacaoAgendaID");
            sql.Append(")");
            sql.Append("values");
            sql.Append("(");
            sql.Append(String.Format("{0}", info.Agenda.AgendaID.Value));
            sql.Append(String.Format(", {0}", info.ConfiguracaoNotificacaoAgenda.ConfiguracaoNotificacaoAgendaID.Value));
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

        public void Update(NotificacaoAgenda info)
        {
            throw new NotImplementedException();
        }

        public void Delete(NotificacaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            StringBuilder sql = new StringBuilder();
            sql.Append("update");
            sql.Append(" [dbo].[NotificacaoAgenda]");
            sql.Append(" set");
            sql.Append(" Ativo = 0");
            sql.Append(" where");
            sql.Append(String.Format(" AgendaID = {0}", info.Agenda.AgendaID.Value));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public NotificacaoAgenda Details(NotificacaoAgenda info)
        {
            throw new NotImplementedException();
        }

        public List<NotificacaoAgenda> Retreave(NotificacaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            List<NotificacaoAgenda> notificacoesAgendas = new List<NotificacaoAgenda>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" lsa.AgendaID");
            sql.Append(", lsa.ConfiguracaoNotificacaoAgendaID");
            sql.Append(", lsa.Utilizado");
            sql.Append(", lsa.Ativo");
            sql.Append(" from");
            sql.Append(" [dbo].[NotificacaoAgenda] lsa");
            sql.Append(" where");
            sql.Append(" 1 = 1");

            if (info.Agenda != null && info.Agenda.AgendaID.HasValue)
                sql.Append(String.Format(" and lsa.AgendaID = {0}", info.Agenda.AgendaID));

            if (info.ConfiguracaoNotificacaoAgenda != null && info.ConfiguracaoNotificacaoAgenda.ConfiguracaoNotificacaoAgendaID.HasValue)
                sql.Append(String.Format(" and lsa.ConfiguracaoNotificacaoAgendaID = {0}", info.ConfiguracaoNotificacaoAgenda.ConfiguracaoNotificacaoAgendaID));

            if (info.Utilizado.HasValue)
                sql.Append(String.Format(" and lsa.Utilizado = {0}", info.Utilizado.Value ? 1 : 0));

            if (info.Ativo.HasValue)
                sql.Append(String.Format(" and lsa.Ativo = {0}", info.Ativo.Value ? 1 : 0));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            NotificacaoAgenda notificacaoAgenda = new NotificacaoAgenda();
                            notificacaoAgenda.Agenda
                                = new AgendaRepository()
                                    .Details(new Agenda()
                                    {
                                        AgendaID = Convert.ToInt32(dr["AgendaID"])
                                    });
                            notificacaoAgenda.ConfiguracaoNotificacaoAgenda
                                = new ConfiguracaoNotificacaoAgendaRepository()
                                    .Details(new ConfiguracaoNotificacaoAgenda()
                                    {
                                        ConfiguracaoNotificacaoAgendaID = Convert.ToInt32(dr["ConfiguracaoNotificacaoAgendaID"])
                                    });
                            notificacaoAgenda.Utilizado = Convert.ToBoolean(dr["Utilizado"]);
                            notificacaoAgenda.Ativo = Convert.ToBoolean(dr["Ativo"]);
                            notificacoesAgendas.Add(notificacaoAgenda);
                        }
                    }
                }
            }

            return notificacoesAgendas;
        }

        #endregion
    }
}
