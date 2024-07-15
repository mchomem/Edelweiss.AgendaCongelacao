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


select * from Agenda
go

select * from LogSmsAgenda
go

select * from NotificacaoAgenda
go
