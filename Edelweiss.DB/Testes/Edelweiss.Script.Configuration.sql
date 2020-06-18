use EDW_Rastreabilidade
go

/*
 * 0 - Restaura a configuração original da tabela.
 * 1 - Modifica as configurações da tabela para aparesentação.
 */

declare @set bit = 0 -- <<<< INPUT
declare @id int = 0
declare @tempo int = 0

if(@set = 1)
begin
	set nocount on

	declare @tabConfiguracao table (id int, tempo int, UnidadeTempoID int)
	insert into @tabConfiguracao
	select * from ConfiguracaoNotificacaoAgenda

	set nocount off

	while( (select count(*) from @tabConfiguracao) > 0)
	begin
		select top 1 @id = id from @tabConfiguracao
		set @tempo = @tempo + 1

		update
			ConfiguracaoNotificacaoAgenda
		set
			Tempo = @tempo
			, UnidadeTempoAgendaID = 1
		where
			ConfiguracaoNotificacaoAgendaID = @id

		delete from @tabConfiguracao where id = @id	
	end	
end
else
begin
	set nocount on

	declare @tempos table (id int identity(1,1), tempo int)
	insert into @tempos
	(tempo)
	values (2),(4), (18)

	set nocount off

	alter table ConfiguracaoNotificacaoAgenda drop constraint UK_ConfiguracaoNotificacaoAgenda_Tempo

	while( (select count(*) from @tempos) > 0)
	begin
		select top 1
			@id = id
			, @tempo = tempo
		from
			@tempos

		update
			ConfiguracaoNotificacaoAgenda
		set
			Tempo = @tempo
			, UnidadeTempoAgendaID = 2
		where
			ConfiguracaoNotificacaoAgendaID = @id

		delete from @tempos where id = @id
	end

	alter table ConfiguracaoNotificacaoAgenda add constraint UK_ConfiguracaoNotificacaoAgenda_Tempo unique(Tempo)
end
go
	
select
	cna.ConfiguracaoNotificacaoAgendaID
	, cna.Tempo
	, uta.Unidade
from
	ConfiguracaoNotificacaoAgenda cna
	join UnidadeTempoAgenda uta on (uta.UnidadeTempoAgendaID = cna.UnidadeTempoAgendaID)
go
