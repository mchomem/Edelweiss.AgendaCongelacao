select * from Agenda
go

select * from LogSmsAgenda
go

select * from NotificacaoAgenda
go

/* Tabelas de donínio */
select * from EstadoAgenda
go

select * from UnidadeTempoAgenda
go

/* Tabela de parametrização das notificações da aplicação */
select * from ConfiguracaoNotificacaoAgenda
go



-- Consulta para notificar o cadastro 6 horas antes.
declare @agora datetime = getdate()

select
	a.AgendaID
	, a.DataHoraEvento
	, a.Local
	, a.NomeMedico
	, a.MedicoExecucaoAgendaID
	, a.NomePaciente
	, a.Convenio
	, a.Procedimento
	, a.TelefoneContato
	, ea.EstadoAgendaID
	, ea.Estado
	, a.Ativo
from
	Agenda a
	join EstadoAgenda ea on (ea.EstadoAgendaID = a.EstadoAgendaID)
where
	ea.Estado = 'Agendado'
	and a.Ativo = 1
	and a.DataHoraEvento between @agora and dateadd(hour, 6, @agora)
order by
	a.DataHoraEvento asc
go





-- Obter os lembretes de uma agenda.
select
	a.AgendaID
	, case
		when uta.Unidade = 'Dias'    then dateadd(day,    - cna.tempo, a.DataHoraEvento)
		when uta.Unidade = 'Horas'   then dateadd(hour,   - cna.tempo, a.DataHoraEvento)
		when uta.Unidade = 'Minutos' then dateadd(minute, - cna.tempo, a.DataHoraEvento)
		when uta.Unidade = 'Semanas' then dateadd(week,   - cna.tempo, a.DataHoraEvento)
	end [Inicío do notificação]
	, a.DataHoraEvento [Data/hora agenda]
	, a.Ativo [Agenda ativa?]
	, na.Ativo [Notificação ativa?]
	, na.Utilizado [Notificação foi utilizado?]
	, cna.Tempo
	, uta.Unidade
from
	NotificacaoAgenda na
	join Agenda a on (a.AgendaID = na.AgendaID)
		join ConfiguracaoNotificacaoAgenda cna on (cna.ConfiguracaoNotificacaoAgendaID = na.ConfiguracaoNotificacaoAgendaID)
			join UnidadeTempoAgenda uta on (uta.UnidadeTempoAgendaID = cna.UnidadeTempoAgendaID)
where
	a.Ativo = 1
	and na.Ativo = 1
	and na.Utilizado = 0
	and getdate()
	between
	case
		when uta.Unidade = 'Dias'    then dateadd(day,    - cna.tempo, a.DataHoraEvento)
		when uta.Unidade = 'Horas'   then dateadd(hour,   - cna.tempo, a.DataHoraEvento)
		when uta.Unidade = 'Minutos' then dateadd(minute, - cna.tempo, a.DataHoraEvento)
		when uta.Unidade = 'Semanas' then dateadd(week,   - cna.tempo, a.DataHoraEvento)
	end
	and a.DataHoraEvento
order by
	cna.Tempo desc
go
