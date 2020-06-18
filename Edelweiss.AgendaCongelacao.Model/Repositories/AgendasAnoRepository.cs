using Edelweiss.AgendaCongelacao.Model.Dashboard;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edelweiss.AgendaCongelacao.Model.Repositories
{
    public class AgendasAnoRepository : ICrud<AgendasAno>
    {
        #region Methods Basic CRUD

        public void Create(AgendasAno entity)
        {
            throw new NotImplementedException();
        }

        public void Update(AgendasAno entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(AgendasAno entity)
        {
            throw new NotImplementedException();
        }

        public AgendasAno Details(AgendasAno entity)
        {
            throw new NotImplementedException();
        }

        public List<AgendasAno> Retreave(AgendasAno entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods (queries)

        public AgendasAno ObterAgendasAno(AgendasAno info)
        {
            DataBase dataBase = new DataBase();
            AgendasAno agendasAno = new AgendasAno();

            List<String> meses = new List<string>();
            List<Int32> quantidades = new List<Int32>();
            Int32 total = 0;

            StringBuilder sql = new StringBuilder();

            sql.Append(String.Format("declare @ano int = {0}", info.Ano));
            sql.Append(" declare @mes int = 0");
            sql.Append(" declare @ultimoDia int = 0");
            sql.Append(" declare @tabAgendas table (mes varchar(3), quantidade int)");
            sql.Append(" while (@mes < 12)");
            sql.Append(" begin");
            sql.Append(" set @mes = @mes + 1");
            sql.Append(" set @ultimoDia = day(eomonth(cast(cast(@ano as varchar(4)) + '-' + cast(@mes as varchar(2)) + '-01' as datetime)))");

            sql.Append(" insert into @tabAgendas");
            sql.Append(" select");
            sql.Append(" case");
            sql.Append(" when @mes = 1 then 'Jan'");
            sql.Append(" when @mes = 2 then 'Fev'");
            sql.Append(" when @mes = 3 then 'Mar'");
            sql.Append(" when @mes = 4 then 'Abr'");
            sql.Append(" when @mes = 5 then 'Mai'");
            sql.Append(" when @mes = 6 then 'Jun'");
            sql.Append(" when @mes = 7 then 'Jul'");
            sql.Append(" when @mes = 8 then 'Ago'");
            sql.Append(" when @mes = 9 then 'Set'");
            sql.Append(" when @mes = 10 then 'Out'");
            sql.Append(" when @mes = 11 then 'Nov'");
            sql.Append(" when @mes = 12 then 'Dez'");
            sql.Append(" end");
            sql.Append(" , count(*)");
            sql.Append(" from");
            sql.Append(" Agenda a");
            sql.Append(" where");
            sql.Append(" a.Ativo = 1");
            sql.Append(" and a.DataHoraEvento");
            sql.Append(" between");
            sql.Append(" cast(cast(@ano as varchar(4)) + '-' + cast(@mes as varchar(2)) + '-01 00:00:00' as datetime)");
            sql.Append(" and cast(cast(@ano as varchar(4)) + '-' + cast(@mes as varchar(2)) + '-' + cast(@ultimoDia as varchar(2)) + ' 23:59:59' as datetime)");

            sql.Append(" end");

            sql.Append(" select mes, quantidade from @tabAgendas");

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            meses.Add(dr["mes"].ToString());
                            quantidades.Add(Convert.ToInt32(dr["quantidade"]));
                            total = total + Convert.ToInt32(dr["quantidade"]);
                        }
                    }
                }
            }

            agendasAno.Meses = meses;
            agendasAno.Quantidades = quantidades;
            agendasAno.Total = total;

            return agendasAno;
        }

        public List<Int32> AnosDisponiveis()
        {
            DataBase dataBase = new DataBase();
            List<Int32> anosDisponiveis = new List<Int32>();

            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" distinct year(a.DataHoraEvento) [anos]");
            sql.Append(" from");
            sql.Append(" Agenda a");
            sql.Append(" where");
            sql.Append(" a.Ativo = 1");
            sql.Append(" order by");
            sql.Append(" [anos] desc");

            using (SqlConnection connection = dataBase.RetornaConexaoRastreabilidade())
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            anosDisponiveis.Add(Convert.ToInt32(dr["anos"]));
                        }
                    }
                }
            }

            return anosDisponiveis;
        }

        #endregion
    }
}
