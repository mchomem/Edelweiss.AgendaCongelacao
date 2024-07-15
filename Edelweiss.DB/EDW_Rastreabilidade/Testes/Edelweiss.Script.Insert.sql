/* Carga para fins de teste.*/

use EDW_Rastreabilidade
go

/* Limpeza dos registros */
delete from LogSmsAgenda
go

delete from NotificacaoAgenda
go

delete from Agenda
go

dbcc checkident ('LogSmsAgenda', reseed, 0)
go

dbcc checkident ('Agenda', reseed, 0)
go
/* Fim: Limpeza dos registros */

declare @maxRows int = 500

declare @tabLocal table (id int identity(1,1), [value] varchar(100))
declare @tabNomeMedico table (id int identity(1,1), [nome] varchar(100), telefone varchar(11))
declare @tabConvenio table (id int identity(1,1), [value] varchar(50))
declare @tabProcedimento table (id int identity(1,1), [value] varchar(30))
declare @tabEstadoAgenda table (id int)
declare @tabNomePaciente table (id int identity(1,1), nome varchar(50))

insert into @tabLocal
values
	('Hospital Moinhos de Ventos')
	, ('Cristo Redentor')
	, ('Pronto socorro')
	, ('Fêmina')
	, ('Santa Casa')
	, ('Mãe de Deus')
	, ('Hospital da PUC')

insert into @tabNomeMedico
values
	('José da Silva', '5133444525')
	, ('Rafael Morais Kinch', '5132354545')
	, ('Alberto Raffuts', '5134651221')
	, ('Joel Santada de Oliveira Warath', '5132987854')
	, ('Bartolomeu Rantz Wicks', '5136359568')
	, ('Celma Ortega Brums', '5134326598')
	, ('Selita Raffaela Shunts', '5132230201')
	, ('Onório Bart Ralflim', '5130013225')
	, ('Katia Valdez Shumooffs', '5139384521')
	, ('Franlkin Hailt Wolfs', '5134254578')

insert into @tabConvenio
values
	('Unimed')
	, ('Centro Clínico Gaúcho')
	, ('SUS')
	, ('Bradesco Sapude')
	, ('Golden Cross')
	, ('Amil')
	, ('Sul América')
	, ('Doctor Clin')
	, ('PCMSO')
	, ('PUC')

insert into @tabProcedimento
values
	('[Teste] Procedimento 1')
	, ('[Teste] Procedimento 2')
	, ('[Teste] Procedimento 3')
	, ('[Teste] Procedimento 4')
	, ('[Teste] Procedimento 5')
	, ('[Teste] Procedimento 6')
	, ('[Teste] Procedimento 7')
	, ('[Teste] Procedimento 8')
	, ('[Teste] Procedimento 9')
	, ('[Teste] Procedimento 10')


insert into @tabNomePaciente
values
	('Alberto Pascoal')
	, ('Bernando Campos')
	, ('Carlos Assunção')
	, ('Diogo da Silva')
	, ('Edgar da Costa')
	, ('Fernando Caipira')
	, ('Garbrile Torres')
	, ('Heraldo Vasconcelos')
	, ('Iderlando Pessoa')
	, ('Jonas Valente')

declare @i int = 0

while(@i < @maxRows)
begin
	set @i = @i + 1

	-- Gerar data e hora dinâmico do ano atual até o ano seguinte.
	declare @ano int = floor(rand() * 5 ) + year(getdate())
	declare @mes int = floor(rand() * 12) + 1
	declare @dia int = floor(rand() * 28) + 1
	declare @hora int = floor(rand() * 23)
	declare @minuto int = floor(rand() * 59)
	declare @segundo int = floor(rand() * 59)

	-- Monta a data com os valores gerados.
	declare @data datetime = (
			cast(@ano as varchar)
			+ '-' + (case when @mes < 10 then '0' + cast(@mes as varchar) else cast(@mes as varchar) end)
			+ '-' + (case when @dia < 10 then '0' + cast(@dia as varchar) else cast(@dia as varchar) end)
			+ ' ' + (case when @hora < 10 then '0' + cast(@hora as varchar) else cast(@hora as varchar) end)
			+ ':' + (case when @minuto < 10 then '0' + cast(@minuto as varchar) else cast(@minuto as varchar) end)
			+ ':' + (case when @segundo < 10 then '0' + cast(@segundo as varchar) else cast(@segundo as varchar) end)
		)

	-- Se o dia da semana gerada for num final de semana, gera uma nova data e verifica novamente até não ser final de semana.
	while (datename(weekday, @data) in ('Saturday', 'Sunday'))
	begin
		-- Redefine os valores novamente.
		set @ano = floor(rand() * 2 ) + year(getdate())
		set @mes = floor(rand() * 12) + 1
		set @dia = floor(rand() * 28) + 1
		set @hora = floor(rand() * 23)
		set @minuto = floor(rand() * 59)
		set @segundo = floor(rand() * 59)

		-- Monta a data novamente.
		set @data = (
			cast(@ano as varchar)
			+ '-' + (case when @mes < 10 then '0' + cast(@mes as varchar) else cast(@mes as varchar) end)
			+ '-' + (case when @dia < 10 then '0' + cast(@dia as varchar) else cast(@dia as varchar) end)
			+ ' ' + (case when @hora < 10 then '0' + cast(@hora as varchar) else cast(@hora as varchar) end)
			+ ':' + (case when @minuto < 10 then '0' + cast(@minuto as varchar) else cast(@minuto as varchar) end)
			+ ':' + (case when @segundo < 10 then '0' + cast(@segundo as varchar) else cast(@segundo as varchar) end)
		)
	end

	-- Seleciona um nome de médico randômico.
	declare @idMedico int = (select floor(rand() * (select count(*) from @tabNomeMedico)) + 1)

	insert into [dbo].[Agenda]
	(
		[DataHoraEvento]
		, [Local]
		, [MedicoExecucaoAgendaID]
		, [NomeMedico]
		, [Convenio]
		, [Procedimento]
		, [TelefoneContato]
		, [EstadoAgendaID]
		, [Ativo]
		, [NomePaciente]
	)
	values
	(
		@data
		, (select [value] from @tabLocal where id = (select floor(rand() * (select count(*) from @tabLocal)) + 1) )
		, (select top 1 MedicoExecucaoAgendaID from MedicoExecucaoAgenda)
		, (select [nome] from @tabNomeMedico where id = @idMedico )
		, (select [value] from @tabConvenio where id = (select floor(rand() * (select count(*) from @tabConvenio)) + 1) )
		, (select [value] from @tabProcedimento where id = (select floor(rand() * (select count(*) from @tabProcedimento)) + 1) )
		, (select [telefone] from @tabNomeMedico where id = @idMedico )
		, (select EstadoAgendaID from EstadoAgenda where EstadoAgendaID = (select floor(rand() * 4 )) + 1)		
		, 1
		, (select [nome] from @tabNomePaciente where id = (select floor(rand() * (select count(*) from @tabNomePaciente)) + 1))
	)

	declare @AgendaID int = (select scope_identity())
	declare @enviado bit = (case when (select floor(rand() * 10)) > 5 then 1 else 0 end)
	
	declare @hashSid varchar(20) = ''
	declare @observarcao varchar(1000) = ''

	declare @max int = 0

	if(@enviado = 1)
	begin
		set @hashSid = 'FAKE_SID_'

		while(@max < 20)
		begin
			set @max = @max + 1

			set @hashSid = @hashSid + cast( (select floor(rand() * 10) ) as varchar(1))
		end

		set @observarcao = 'FAKE: foi enviado um sms para o destino +55' + (select TelefoneContato from agenda where AgendaID = @AgendaID)
	end
	else
	begin
		set @observarcao = 'FAKE: falha ao enviar o sms para o destino +55' + (select TelefoneContato from agenda where AgendaID = @AgendaID)
	end

	insert into [dbo].LogSmsAgenda
	(
		AgendaID
		, SMSEnviado
		, SMSDataProcessamento
		, SMSMessageID
		, Observacao
	)
	values
	(
		@AgendaID
		,  @enviado
		, (select DataHoraEvento from Agenda where AgendaID = @AgendaID)
		, @hashSid
		, @observarcao
	)
end
go

