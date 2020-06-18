using Edelweiss.AgendaCongelacao.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edelweiss.AgendaCongelacao.Model.Repositories
{
    public class AgendaRepository : ICrud<Agenda>
    {
        #region Methods (roles)

        /// <summary>
        /// Verifica se existem registros de agendas com data/hora próximas a data/hora da agenda que se quer salvar.
        /// A partir da dataHoraEvento da agenda, verifica (pela quantidade de horas de intervalo) antes e depois.
        /// </summary>
        /// <param name="info">O objeto do tipo Agenda contendo todas as informações.</param>
        /// <param name="horasIntervalo">A quantidade de horas de intervalo para ser verificado antes e depois.</param>
        /// <returns>Retorna a quantidade de agendas aproximadas a data/hora da agenda que se quer salvar</returns>
        public Int32 AgendasProximas(Agenda info, Int32 horasIntervalo)
        {
            DataBase dataBase = new DataBase();
            List<Agenda> agendas = new List<Agenda>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" a.AgendaID");
            sql.Append(", a.DataHoraEvento");
            sql.Append(", a.Local");
            sql.Append(", a.NomeMedico");
            sql.Append(", a.NomePaciente");
            sql.Append(", a.Convenio");
            sql.Append(", a.Procedimento");
            sql.Append(", a.TelefoneContato");
            sql.Append(", a.EstadoAgendaID");
            sql.Append(", a.Ativo");
            sql.Append(" from");
            sql.Append(" [dbo].[Agenda] a");
            sql.Append(" where");
            sql.Append(" a.Ativo = 1");
            /*
             * Com a data/hora do evento da agenda a ser gravada,
             * busca outras agendas no intervalo de horas antes e depois.
             */
            sql.Append
                (
                    String.Format
                        (
                            " and a.DataHoraEvento between dateadd(hour, -{0}, '{1}') and dateadd(hour, {0}, '{1}')"
                            , horasIntervalo
                            , info.DataHoraEvento.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        )
                );

            // Em caso de edição desconsidera a agenda!
            if (info.AgendaID.HasValue && info.AgendaID > 0)
                sql.Append(String.Format(" and a.AgendaID != {0}", info.AgendaID.Value));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Agenda agenda = new Agenda();
                            agenda.AgendaID = Convert.ToInt32(dr["AgendaID"]);
                            agenda.DataHoraEvento = Convert.ToDateTime(dr["DataHoraEvento"]);
                            agenda.Local = dr["Local"].ToString();
                            agenda.NomeMedico = dr["NomeMedico"].ToString();
                            agenda.NomePaciente = dr["NomePaciente"].ToString();
                            agenda.Convenio = dr["Convenio"].ToString();
                            agenda.Procedimento = dr["Convenio"].ToString();
                            agenda.TelefoneContato = dr["TelefoneContato"].ToString();
                            agenda.EstadoAgenda =
                                new EstadoAgendaRepository()
                                    .Details(new EstadoAgenda() { EstadoAgendaID = Convert.ToInt32(dr["EstadoAgendaID"]) });
                            agenda.Ativo = Convert.ToBoolean(dr["Ativo"]);
                            agendas.Add(agenda);
                        }
                    }
                }
            }

            return agendas.Count;
        }

        /// <summary>
        /// A partir da quantidade de horas informada, verifica as X horas adiante
        /// se existem agendas com estado de agenda igual a "Agendado".
        /// </summary>
        /// <param name="horas">A quantidade de horas informada.</param>
        /// <returns>Uma lista contendo as agendas dentro do itervalo de horas e que estão "Agendadas".</returns>
        public List<Agenda> VerificarAgendados(Int32 horas)
        {
            DateTime dataAtual = DateTime.Now;
            DataBase dataBase = new DataBase();
            List<Agenda> agendas = new List<Agenda>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" a.AgendaID");
            sql.Append(", a.DataHoraEvento");
            sql.Append(", a.Local");
            sql.Append(", a.NomeMedico");
            sql.Append(", a.MedicoExecucaoAgendaID");
            sql.Append(", a.NomePaciente");
            sql.Append(", a.Convenio");
            sql.Append(", a.Procedimento");
            sql.Append(", a.TelefoneContato");
            sql.Append(", ea.EstadoAgendaID");
            sql.Append(", ea.Estado");
            sql.Append(", a.Ativo");
            sql.Append(" from");
            sql.Append(" Agenda a");
            sql.Append(" join EstadoAgenda ea on (ea.EstadoAgendaID = a.EstadoAgendaID)");
            sql.Append(" where");
            sql.Append(" ea.Estado = 'Agendado'");
            sql.Append(" and a.Ativo = 1");
            sql.Append(
                String.Format
                        (
                            "and a.DataHoraEvento between '{0}' and dateadd(hour, {1}, '{0}')"
                            , dataAtual.ToString("yyyy-MM-dd HH:mm:ss")
                            , horas
                        )
                    );
            sql.Append(" order by");
            sql.Append(" a.DataHoraEvento asc");

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Agenda agenda = new Agenda();
                            agenda.AgendaID = Convert.ToInt32(dr["AgendaID"]);
                            agenda.DataHoraEvento = Convert.ToDateTime(dr["DataHoraEvento"]);
                            agenda.Local = dr["Local"].ToString();
                            agenda.NomeMedico = dr["NomeMedico"].ToString();
                            agenda.MedicoExecucaoAgenda =
                                new MedicoExecucaoAgendaRepository()
                                    .Details(new MedicoExecucaoAgenda() { MedicoExecucaoAgendaID = Convert.ToInt32(dr["MedicoExecucaoAgendaID"]) });
                            agenda.NomePaciente = dr["NomePaciente"].ToString();
                            agenda.Convenio = dr["Convenio"].ToString();
                            agenda.Procedimento = dr["Procedimento"].ToString();
                            agenda.TelefoneContato = dr["TelefoneContato"].ToString();
                            agenda.EstadoAgenda =
                                new EstadoAgendaRepository()
                                    .Details(new EstadoAgenda() { EstadoAgendaID = Convert.ToInt32(dr["EstadoAgendaID"]) });
                            agenda.Ativo = Convert.ToBoolean(dr["Ativo"]);
                            agendas.Add(agenda);
                        }
                    }
                }
            }

            return agendas;
        }

        #endregion

        #region Methods Basic CRUD

        public void Create(Agenda info)
        {
            throw new NotImplementedException();
        }

        public Int32 CreateWithReturnID(Agenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [dbo].[Agenda]");
            sql.Append(" (");
            sql.Append("DataHoraEvento");
            sql.Append(", [Local]");
            sql.Append(", NomeMedico");
            sql.Append(", MedicoExecucaoAgendaID");
            sql.Append(", NomePaciente");
            sql.Append(", Convenio");
            sql.Append(", Procedimento");
            sql.Append(", TelefoneContato");
            sql.Append(", EstadoAgendaID");
            sql.Append(")");
            sql.Append(" values ");
            sql.Append("(");
            sql.Append(String.Format("'{0}'", info.DataHoraEvento.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            sql.Append(String.Format(",'{0}'", info.Local));
            sql.Append(String.Format(", '{0}'", info.NomeMedico));
            sql.Append(String.Format(", '{0}'", info.MedicoExecucaoAgenda.MedicoExecucaoAgendaID));
            sql.Append(String.Format(", '{0}'", info.NomePaciente));
            sql.Append(String.Format(", '{0}'", info.Convenio));
            sql.Append(String.Format(", '{0}'", info.Procedimento));
            sql.Append(String.Format(", '{0}'", info.TelefoneContato));
            sql.Append(String.Format(", {0}", info.EstadoAgenda.EstadoAgendaID));
            sql.Append(")");
            sql.Append(" select cast(scope_identity() as int)");

            int newID = 0;

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    newID = (int)command.ExecuteScalar();
                }
            }

            return newID;
        }

        public void Update(Agenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("update [dbo].[Agenda]");
            sql.Append(" set");
            sql.Append(String.Format(" DataHoraEvento = '{0}'", info.DataHoraEvento.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            sql.Append(String.Format(", [Local] = '{0}'", info.Local));
            sql.Append(String.Format(", NomeMedico = '{0}'", info.NomeMedico));
            sql.Append(String.Format(", MedicoExecucaoAgendaID = '{0}'", info.MedicoExecucaoAgenda.MedicoExecucaoAgendaID));
            sql.Append(String.Format(", NomePaciente = '{0}'", info.NomePaciente));
            sql.Append(String.Format(", Convenio = '{0}'", info.Convenio));
            sql.Append(String.Format(", Procedimento = '{0}'", info.Procedimento));
            sql.Append(String.Format(", TelefoneContato = '{0}'", info.TelefoneContato));
            sql.Append(String.Format(", EstadoAgendaID = {0}", info.EstadoAgenda.EstadoAgendaID));
            sql.Append(" where");
            sql.Append(String.Format(" AgendaID = {0}", info.AgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public void Delete(Agenda info)
        {
            DataBase dataBase = new DataBase();

            StringBuilder sql = new StringBuilder();
            sql.Append("update");
            sql.Append(" [dbo].[Agenda]");
            sql.Append(" set");
            sql.Append(String.Format(" Ativo = {0}", 0));
            sql.Append(" where");
            sql.Append(String.Format(" AgendaID = {0}", info.AgendaID));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }

        public Agenda Details(Agenda info)
        {
            DataBase dataBase = new DataBase();
            Agenda agenda = new Agenda();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" a.AgendaID");
            sql.Append(", a.DataHoraEvento");
            sql.Append(", a.Local");
            sql.Append(", a.NomeMedico");
            sql.Append(", a.MedicoExecucaoAgendaID");
            sql.Append(", a.NomePaciente");
            sql.Append(", a.Convenio");
            sql.Append(", a.Procedimento");
            sql.Append(", a.TelefoneContato");
            sql.Append(", a.EstadoAgendaID");
            sql.Append(", a.Ativo");
            sql.Append(" from");
            sql.Append(" [dbo].[Agenda] a");
            sql.Append(" where");
            sql.Append(String.Format(" a.AgendaID = {0}", info.AgendaID.Value));

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            agenda.AgendaID = Convert.ToInt32(dr["AgendaID"]);
                            agenda.DataHoraEvento = Convert.ToDateTime(dr["DataHoraEvento"]);
                            agenda.Local = dr["Local"].ToString();
                            agenda.NomeMedico = dr["NomeMedico"].ToString();
                            agenda.MedicoExecucaoAgenda =
                                new MedicoExecucaoAgendaRepository()
                                    .Details(new MedicoExecucaoAgenda() { MedicoExecucaoAgendaID = Convert.ToInt32(dr["MedicoExecucaoAgendaID"]) });
                            agenda.NomePaciente = dr["NomePaciente"].ToString();
                            agenda.Convenio = dr["Convenio"].ToString();
                            agenda.Procedimento = dr["Procedimento"].ToString();
                            agenda.TelefoneContato = dr["TelefoneContato"].ToString();
                            agenda.EstadoAgenda =
                                new EstadoAgendaRepository()
                                    .Details(new EstadoAgenda() { EstadoAgendaID = Convert.ToInt32(dr["EstadoAgendaID"]) });
                            agenda.Ativo = Convert.ToBoolean(dr["Ativo"]);
                        }
                    }
                }
            }

            return agenda;
        }

        public List<Agenda> Retreave(Agenda info)
        {
            throw new NotImplementedException();
        }

        public List<Agenda> Retreave(Agenda info, DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            DataBase dataBase = new DataBase();
            List<Agenda> agendas = new List<Agenda>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" a.AgendaID");
            sql.Append(", a.DataHoraEvento");
            sql.Append(", a.Local");
            sql.Append(", a.NomeMedico");
            sql.Append(", a.MedicoExecucaoAgendaID");
            sql.Append(", a.NomePaciente");
            sql.Append(", a.Convenio");
            sql.Append(", a.Procedimento");
            sql.Append(", a.TelefoneContato");
            sql.Append(", a.EstadoAgendaID");
            sql.Append(", a.Ativo");
            sql.Append(" from");
            sql.Append(" [dbo].[Agenda] a");
            sql.Append(" where");
            sql.Append(" 1 = 1");

            if (info.AgendaID.HasValue)
                sql.Append(String.Format(" and a.AgendaID = {0}", info.AgendaID.Value));

            if (info.DataHoraEvento.HasValue)
                sql.Append(String.Format(" and a.DataHoraEvento = '{0}'", info.DataHoraEvento.Value));

            if (info.Local != null)
                sql.Append(String.Format(" and a.Local like '%{0}%'", info.Local));

            if (info.NomeMedico != null)
                sql.Append(String.Format(" and a.NomeMedico like '%{0}%'", info.NomeMedico));

            if (info.MedicoExecucaoAgenda != null && info.MedicoExecucaoAgenda.MedicoExecucaoAgendaID != null)
                sql.Append(String.Format(" and a.MedicoExecucaoAgendaID = {0}", info.MedicoExecucaoAgenda.MedicoExecucaoAgendaID));

            if (info.NomePaciente != null)
                sql.Append(String.Format(" and a.NomePaciente like '%{0}%'", info.NomePaciente));

            if (info.Convenio != null)
                sql.Append(String.Format(" and a.Convenio like '%{0}%'", info.Convenio));

            if (info.Procedimento != null)
                sql.Append(String.Format(" and a.Procedimento like '%{0}%'", info.Procedimento));

            if (info.TelefoneContato != null)
                sql.Append(String.Format(" and a.TelefoneContato like '%{0}%'", info.TelefoneContato));

            if (info.EstadoAgenda != null && info.EstadoAgenda.EstadoAgendaID != null)
                sql.Append(String.Format(" and a.EstadoAgendaID = {0}", info.EstadoAgenda.EstadoAgendaID));

            if (info.Ativo.HasValue)
                sql.Append(String.Format(" and a.Ativo = {0}", info.Ativo));

            // Busca no intervalo de datas, se ambas as datas forem informadas.
            if (dataInicial != null && dataFinal != null)
                sql.Append
                    (
                        String.Format(" and a.DataHoraEvento between '{0}' and '{1}'"
                        , dataInicial.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        , dataFinal.Value.ToString("yyyy-MM-dd HH:mm:ss"))
                    );
            // Se somente a data inicial foi inforamda, busca a partir desta data para frente.
            else if (dataInicial != null)
                sql.Append
                    (
                        String.Format(
                        " and a.DataHoraEvento >= '{0}'"
                        , dataInicial.Value.ToString("yyyy-MM-dd HH:mm:ss"))
                    );
            // Se somente a data final foi inforamda, busca a partir desta data para trás.
            else if (dataFinal != null)
                sql.Append
                    (
                        String.Format(" and a.DataHoraEvento <= '{0}'"
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
                            Agenda agenda = new Agenda();
                            agenda.AgendaID = Convert.ToInt32(dr["AgendaID"]);
                            agenda.DataHoraEvento = Convert.ToDateTime(dr["DataHoraEvento"]);
                            agenda.Local = dr["Local"].ToString();
                            agenda.NomeMedico = dr["NomeMedico"].ToString();
                            agenda.MedicoExecucaoAgenda =
                                new MedicoExecucaoAgendaRepository()
                                    .Details(new MedicoExecucaoAgenda() { MedicoExecucaoAgendaID = Convert.ToInt32(dr["MedicoExecucaoAgendaID"]) });
                            agenda.NomePaciente = dr["NomePaciente"].ToString();
                            agenda.Convenio = dr["Convenio"].ToString();
                            agenda.Procedimento = dr["Convenio"].ToString();
                            agenda.TelefoneContato = dr["TelefoneContato"].ToString();
                            agenda.EstadoAgenda =
                                new EstadoAgendaRepository()
                                    .Details(new EstadoAgenda() { EstadoAgendaID = Convert.ToInt32(dr["EstadoAgendaID"]) });
                            agenda.Ativo = Convert.ToBoolean(dr["Ativo"]);
                            agendas.Add(agenda);
                        }
                    }
                }
            }

            return agendas;
        }

        #endregion
    }
}
