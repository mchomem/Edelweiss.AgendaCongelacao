using Edelweiss.AgendaCongelacao.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edelweiss.AgendaCongelacao.Model.Repositories
{
    public class ConfiguracaoNotificacaoAgendaRepository : ICrud<ConfiguracaoNotificacaoAgenda>
    {
        #region Methods Basic CRUD

        public void Create(ConfiguracaoNotificacaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [dbo].[ConfiguracaoNotificacaoAgenda]");
            sql.Append("(");
            sql.Append("Tempo");
            sql.Append(", UnidadeTempoAgendaID");
            sql.Append(")");
            sql.Append("values");
            sql.Append("(");
            sql.Append(String.Format("{0}", info.Tempo));
            sql.Append(String.Format(", {0}", info.UnidadeTempoAgenda.UnidadeTempoAgendaID));
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

        public void Update(ConfiguracaoNotificacaoAgenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("update [dbo].[ConfiguracaoNotificacaoAgenda]");
            sql.Append(" set");
            sql.Append(String.Format(" Tempo = '{0}'", info.Tempo));
            sql.Append(String.Format(", UnidadeTempoAgendaID = {0}", info.UnidadeTempoAgenda.UnidadeTempoAgendaID));
            sql.Append(" where");
            sql.Append(String.Format(" ConfiguracaoNotificacaoAgendaID = {0}", info.ConfiguracaoNotificacaoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public void Delete(ConfiguracaoNotificacaoAgenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("delete");
            sql.Append(" [dbo].[ConfiguracaoNotificacaoAgenda]");
            sql.Append(" where");
            sql.Append(String.Format(" ConfiguracaoNotificacaoAgendaID = {0}", info.ConfiguracaoNotificacaoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public ConfiguracaoNotificacaoAgenda Details(ConfiguracaoNotificacaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            ConfiguracaoNotificacaoAgenda configuracaoNotificacaoAgenda = new ConfiguracaoNotificacaoAgenda();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" cna.ConfiguracaoNotificacaoAgendaID");
            sql.Append(", cna.Tempo");
            sql.Append(", cna.UnidadeTempoAgendaID");
            sql.Append(" from");
            sql.Append(" [dbo].[ConfiguracaoNotificacaoAgenda] cna");
            sql.Append(" where");
            sql.Append(String.Format(" cna.ConfiguracaoNotificacaoAgendaID = {0}", info.ConfiguracaoNotificacaoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            configuracaoNotificacaoAgenda.ConfiguracaoNotificacaoAgendaID = Convert.ToInt32(dr["ConfiguracaoNotificacaoAgendaID"]);
                            configuracaoNotificacaoAgenda.Tempo = Convert.ToInt32(dr["Tempo"]);
                            configuracaoNotificacaoAgenda.UnidadeTempoAgenda =
                                new UnidadeTempoAgendaRepository()
                                    .Details(new UnidadeTempoAgenda()
                                    {
                                        UnidadeTempoAgendaID = Convert.ToInt32(dr["UnidadeTempoAgendaID"])
                                    });
                        }
                    }
                }
            }

            return configuracaoNotificacaoAgenda;
        }

        public List<ConfiguracaoNotificacaoAgenda> Retreave(ConfiguracaoNotificacaoAgenda info)
        {
            DataBase dataBase = new DataBase();
            List<ConfiguracaoNotificacaoAgenda> configuracaoNotificacaoAgendas = new List<ConfiguracaoNotificacaoAgenda>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" cna.ConfiguracaoNotificacaoAgendaID");
            sql.Append(", cna.Tempo");
            sql.Append(", cna.UnidadeTempoAgendaID");
            sql.Append(" from");
            sql.Append(" [dbo].[ConfiguracaoNotificacaoAgenda] cna");
            sql.Append(" where");
            sql.Append(" 1 = 1");

            if (info.ConfiguracaoNotificacaoAgendaID.HasValue)
                sql.Append(String.Format("and cna.ConfiguracaoNotificacaoAgendaID = {0}", info.ConfiguracaoNotificacaoAgendaID));

            if (info.Tempo.HasValue)
                sql.Append(String.Format("and cna.Tempo = {0}", info.Tempo));

            if (info.UnidadeTempoAgenda != null && info.UnidadeTempoAgenda.UnidadeTempoAgendaID.HasValue)
                sql.Append(String.Format("and cna.UnidadeTempoAgendaID = {0}", info.UnidadeTempoAgenda.UnidadeTempoAgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ConfiguracaoNotificacaoAgenda configuracaoNotificacaoAgenda = new ConfiguracaoNotificacaoAgenda();
                            configuracaoNotificacaoAgenda.ConfiguracaoNotificacaoAgendaID = Convert.ToInt32(dr["ConfiguracaoNotificacaoAgendaID"]);
                            configuracaoNotificacaoAgenda.Tempo = Convert.ToInt32(dr["Tempo"]);
                            configuracaoNotificacaoAgenda.UnidadeTempoAgenda =
                                new UnidadeTempoAgendaRepository()
                                    .Details(new UnidadeTempoAgenda() { UnidadeTempoAgendaID = Convert.ToInt32(dr["UnidadeTempoAgendaID"]) });

                            configuracaoNotificacaoAgendas.Add(configuracaoNotificacaoAgenda);
                        }
                    }
                }
            }

            return configuracaoNotificacaoAgendas;
        }

        #endregion
    }
}
