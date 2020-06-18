using Edelweiss.AgendaCongelacao.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edelweiss.AgendaCongelacao.Model.Repositories
{
    public class LogSmsAgendaRepository: ICrud<LogSmsAgenda>
    {
        #region Methods Basic CRUD

        public void Create(LogSmsAgenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [dbo].[LogSmsAgenda]");
            sql.Append(" (");
            sql.Append(" AgendaID");
            sql.Append(" , SMSEnviado");
            sql.Append(" , SMSDataProcessamento");
            sql.Append(" , SMSMessageID");
            sql.Append(" , Observacao");
            sql.Append(" )");
            sql.Append(" values");
            sql.Append(" (");
            sql.Append(String.Format("{0}", info.Agenda.AgendaID));
            sql.Append(String.Format(", {0}", info.SMSEnviado.Value ? 1 : 0));
            sql.Append(String.Format(", '{0}'", info.SMSDataProcessamento.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            sql.Append(String.Format(", '{0}'", String.IsNullOrEmpty(info.SMSMessageID) ? "NULL" : info.SMSMessageID));
            sql.Append(String.Format(", '{0}'", info.Observacao));
            sql.Append(" )");

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public void Update(LogSmsAgenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("update [dbo].[LogSmsAgenda]");
            sql.Append(" set");
            sql.Append(String.Format(" AgendaID = {0}", info.Agenda.AgendaID));
            sql.Append(String.Format(", SMSEnviado = {0}", info.SMSEnviado.Value ? 1 : 0));
            sql.Append(String.Format(", SMSDataProcessamento = '{0}'", info.SMSDataProcessamento.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            sql.Append(String.Format(", SMSMessageID = '{0}'", info.SMSMessageID));
            sql.Append(String.Format(", Observacao = '{0}'", info.Observacao));
            sql.Append(" where");
            sql.Append(String.Format(" LogSmsAgendaID = {0}", info.LogSmsAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public void Delete(LogSmsAgenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("delete");
            sql.Append(" from");
            sql.Append(" [dbo].[LogSmsAgenda]");
            sql.Append(" where");
            sql.Append(String.Format(" LogSmsAgendaID = {0}", info.LogSmsAgendaID.Value));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public LogSmsAgenda Details(LogSmsAgenda info)
        {
            DataBase dataBase = new DataBase();
            LogSmsAgenda logSmsAgenda = new LogSmsAgenda();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" lsa.LogSmsAgendaID");
            sql.Append(", lsa.AgendaID");
            sql.Append(", lsa.SMSEnviado");
            sql.Append(", lsa.SMSDataProcessamento");
            sql.Append(", lsa.SMSMessageID");
            sql.Append(", lsa.Observacao");
            sql.Append(" from");
            sql.Append(" [dbo].[LogSmsAgenda] lsa");
            sql.Append(" where");
            sql.Append(String.Format(" lsa.LogSmsAgendaID = {0}", info.LogSmsAgendaID.Value));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            logSmsAgenda.LogSmsAgendaID = Convert.ToInt32(dr["LogSmsAgendaID"]);
                            logSmsAgenda.Agenda = new AgendaRepository().Details(new Agenda() { AgendaID = Convert.ToInt32(dr["AgendaID"]) });
                            logSmsAgenda.SMSEnviado = Convert.ToBoolean(dr["SMSEnviado"]);
                            logSmsAgenda.SMSDataProcessamento = Convert.ToDateTime(dr["SMSDataProcessamento"]);
                            logSmsAgenda.SMSMessageID = dr["SMSMessageID"].ToString();
                            logSmsAgenda.Observacao = dr["Observacao"].ToString();
                        }
                    }
                }
            }

            return logSmsAgenda;
        }

        public List<LogSmsAgenda> Retreave(LogSmsAgenda info)
        {
            throw new NotImplementedException();
        }

        public List<LogSmsAgenda> Retreave(LogSmsAgenda info, DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            DataBase dataBase = new DataBase();
            List<LogSmsAgenda> logSmsAgendas = new List<LogSmsAgenda>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" lsa.LogSmsAgendaID");
            sql.Append(", lsa.AgendaID");
            sql.Append(", lsa.SMSEnviado");
            sql.Append(", lsa.SMSDataProcessamento");
            sql.Append(", lsa.SMSMessageID");
            sql.Append(", lsa.Observacao");
            sql.Append(" from");
            sql.Append(" [dbo].[LogSmsAgenda] lsa");
            sql.Append(" where");
            sql.Append(" 1 = 1");

            if (info.LogSmsAgendaID.HasValue)
                sql.Append(String.Format(" and lsa.LogSmsAgendaID = {0}", info.LogSmsAgendaID.Value));

            if (info.Agenda.AgendaID.HasValue)
                sql.Append(String.Format(" and lsa.AgendaID = {0}", info.Agenda.AgendaID.Value));

            if (info.SMSEnviado.HasValue)
                sql.Append(String.Format(" and lsa.SMSEnviado = {0}", info.SMSEnviado.Value ? 1 : 0));

            if (!String.IsNullOrEmpty(info.SMSMessageID))
                sql.Append(String.Format(" and lsa.SMSMessageID = '{0}'", info.SMSMessageID));

            if (!String.IsNullOrEmpty(info.Observacao))
                sql.Append(String.Format(" and lsa.Observacao like '%{0}%'", info.Observacao));

            // Busca no intervalo de datas, se ambas as datas forem informadas.
            if (dataInicial != null && dataFinal != null)
            {
                sql.Append
                    (
                        String.Format(" and lsa.SMSDataProcessamento between '{0}' and '{1}'"
                        , dataInicial.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        , dataFinal.Value.ToString("yyyy-MM-dd HH:mm:ss"))
                    );
            }
            // Se somente a data inicial foi inforamda, busca a partir desta data para frente.
            else if (dataInicial != null)
                sql.Append
                    (
                        String.Format(
                        " and lsa.SMSDataProcessamento >= '{0}'"
                        , dataInicial.Value.ToString("yyyy-MM-dd HH:mm:ss"))
                    );
            // Se somente a data final foi inforamda, busca a partir desta data para trás.
            else if (dataFinal != null)
                sql.Append
                    (
                        String.Format(" and lsa.SMSDataProcessamento <= '{0}'"
                        , dataFinal.Value.ToString("yyyy-MM-dd HH:mm:ss"))
                    );

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            LogSmsAgenda log = new LogSmsAgenda();
                            log.LogSmsAgendaID = Convert.ToInt32(dr["LogSmsAgendaID"]);
                            log.Agenda = new AgendaRepository().Details(new Agenda() { AgendaID = Convert.ToInt32(dr["AgendaID"]) });
                            log.SMSEnviado = Convert.ToBoolean(dr["SMSEnviado"]);
                            log.SMSDataProcessamento = Convert.ToDateTime(dr["SMSDataProcessamento"]);
                            log.SMSMessageID = dr["SMSMessageID"].ToString();
                            log.Observacao = dr["Observacao"].ToString();
                            logSmsAgendas.Add(log);
                        }
                    }
                }
            }

            return logSmsAgendas;
        }

        #endregion
    }
}
