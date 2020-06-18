-- Agendamentos de congelação (do ano selecionado).

declare @ano int = 2019
declare @mes int = 0
declare @ultimoDia int = 0
declare @tabAgendas table (mes varchar(3), quantidade int)

while (@mes < 12)
begin
	set @mes = @mes + 1
	set @ultimoDia = day(eomonth(cast(cast(@ano as varchar(4)) + '-' + cast(@mes as varchar(2)) + '-01' as datetime)))

	insert into @tabAgendas
	select
		case
			when @mes = 1 then 'Jan'
			when @mes = 2 then 'Fev'
			when @mes = 3 then 'Mar'
			when @mes = 4 then 'Abr'
			when @mes = 5 then 'Mai'
			when @mes = 6 then 'Jun'
			when @mes = 7 then 'Jul'
			when @mes = 8 then 'Ago'
			when @mes = 9 then 'Set'
			when @mes = 10 then 'Out'
			when @mes = 11 then 'Nov'
			when @mes = 12 then 'Dez'
		end
		, count(*)
	from
		Agenda a
	where
		a.DataHoraEvento
			between
				cast(cast(@ano as varchar(4)) + '-' + cast(@mes as varchar(2)) + '-01 00:00:00' as datetime)
				and cast(cast(@ano as varchar(4)) + '-' + cast(@mes as varchar(2)) + '-' + cast(@ultimoDia as varchar(2)) + ' 23:59:59' as datetime)
end

select * from @tabAgendas
go
