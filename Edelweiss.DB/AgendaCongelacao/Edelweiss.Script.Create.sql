create database AgendaCongelacao
go

use AgendaCongelacao
go

create table EstadoAgenda
(
	EstadoAgendaID int not null identity(1,1)
	, Estado nvarchar(15) not null
	, CorHexaCss nvarchar(7) not null
	, Ordem int not null
)
go

alter table EstadoAgenda add constraint PK_EstadoAgenda primary key(EstadoAgendaID)
go

alter table EstadoAgenda add constraint UK_EstadoAgenda unique(Estado)
go

create table MedicoExecucaoAgenda
(
	MedicoExecucaoAgendaID int not null identity(1,1)
	, Nome nvarchar(100) not null
	, Email nvarchar(100) not null
	, Celular nvarchar(11) not null
)
go

alter table MedicoExecucaoAgenda add constraint PK_MedicoExecucaoAgenda primary key(MedicoExecucaoAgendaID)
go

create table Agenda
(
	AgendaID int not null identity(1,1)
    , DataHoraEvento datetime not null
    , [Local] nvarchar(100) not null
	, MedicoExecucaoAgendaID int not null
    , NomeMedico nvarchar(100) not null
    , Convenio nvarchar(50) not null
    , Procedimento nvarchar(100) not null
    , TelefoneContato nvarchar(11) not null
	, EstadoAgendaID int not null
	, Ativo bit not null
)
go

alter table Agenda add constraint PK_Agenda primary key(AgendaID)
go

alter table Agenda add constraint FK_Agenda_EstadoAgenda foreign key(EstadoAgendaID) references EstadoAgenda(EstadoAgendaID)
go

alter table Agenda add constraint FK_Agenda_MedicoExecucaoAgenda foreign key(MedicoExecucaoAgendaID) references MedicoExecucaoAgenda (MedicoExecucaoAgendaID)
go

alter table Agenda add constraint DF_Agenda_Ativo default 1 for Ativo
go



create table LogSmsAgenda
(
	LogSmsAgendaID int not null identity(1,1)
	, AgendaID int not null
	, SMSEnviado bit null
	, SMSDataProcessamento datetime null
	, SMSMessageID nvarchar(100) null
	, Observacao nvarchar(1000) null
)
go

alter table LogSmsAgenda add constraint PK_LogSmsAgenda primary key(LogSmsAgendaID)
go

alter table LogSmsAgenda add constraint FK_LogSmsAgenda foreign key(AgendaID) references Agenda (AgendaID)
go



create table UnidadeTempoAgenda
(
	UnidadeTempoAgendaID int not null identity(1,1)
	, Unidade nvarchar(10) not null
)
go

alter table UnidadeTempoAgenda add constraint PK_UnidadeTempoAgenda primary key(UnidadeTempoAgendaID)
go

alter table UnidadeTempoAgenda add constraint UK_UnidadeTempoAgenda_Unidade unique(Unidade)
go



create table ConfiguracaoNotificacaoAgenda
(
	ConfiguracaoNotificacaoAgendaID int not null identity(1,1)
	, Tempo int not null
	, UnidadeTempoAgendaID int not null
)
go

alter table ConfiguracaoNotificacaoAgenda add constraint PK_ConfiguracaoNotificacaoAgenda primary key(ConfiguracaoNotificacaoAgendaID)
go

alter table ConfiguracaoNotificacaoAgenda add constraint UK_ConfiguracaoNotificacaoAgenda_Tempo unique(Tempo)
go

alter table ConfiguracaoNotificacaoAgenda add constraint FK_ConfiguracaoNotificacaoAgenda_UnidadeTempoAgenda foreign key (UnidadeTempoAgendaID) references UnidadeTempoAgenda(UnidadeTempoAgendaID)
go



create table NotificacaoAgenda
(
	AgendaID int not null
	, ConfiguracaoNotificacaoAgendaID int not null
	, Utilizado bit not null
	, Ativo bit not null
)
go

alter table NotificacaoAgenda add constraint PK_NotificacaoAgenda primary key(AgendaID, ConfiguracaoNotificacaoAgendaID)
go

alter table NotificacaoAgenda add constraint FK_NotificacaoAgenda_Agenda foreign key(AgendaID) references Agenda(AgendaID)
go

alter table NotificacaoAgenda add constraint FK_NotificacaoAgenda_ConfiguracaoNotificacaoAgenda foreign key(ConfiguracaoNotificacaoAgendaID) references ConfiguracaoNotificacaoAgenda(ConfiguracaoNotificacaoAgendaID)
go

alter table NotificacaoAgenda add constraint DF_NotificacaoAgenda_Utilizado default 0 for Utilizado
go

alter table NotificacaoAgenda add constraint DF_NotificacaoAgenda_Ativo default 1 for Ativo
go



insert into EstadoAgenda (Estado, CorHexaCss, Ordem)
values
	('Agendado', '#ff6600', 1)
	, ('Cancelado','#ff0000', 2)
	, ('Confirmado', '#008000', 3)
	, ('Finalizado', '#0000ff', 4)
go

insert into UnidadeTempoAgenda
values
	('Minutos')
	, ('Horas')
	, ('Dias')
	, ('Semanas')
go

declare @unidadeTempoAgendaID int = (select UnidadeTempoAgendaID from UnidadeTempoAgenda where Unidade = 'Horas')
declare @tempos table (id int identity, horas int)
insert into @tempos
values(2), (4), (18)
declare @id int = 0
declare @horas int = 0

while ( (select count(*) from @tempos) > 0)
begin
	select top 1
		@id = id
		, @horas = horas
	from
		@tempos

	insert into ConfiguracaoNotificacaoAgenda
	(
		Tempo
		, UnidadeTempoAgendaID
	)
	values
	(
		@horas
		, @unidadeTempoAgendaID
	)

	delete from @tempos where id = @id
end
go


insert into MedicoExecucaoAgenda
(
	Nome
	, Email
	, Celular
)
values
(
	'Maria Isabel Edelweiss'
	, 'mariaisabel.edelweiss@gmail.com.br'
	, '51999826982'
)
go


-- Atualização de 18/10/2019
-- Inclusão do campo NomePaciente

alter table Agenda add NomePaciente nvarchar(200) null
go

-- OBS: como o nome do paciente também é obrigatório todas as rows devem ter o nome do paciente
-- após informar os nomes, executar o comando abaixo.

-- alter table Agenda alter column NomePaciente nvarchar(200) not null
-- go



-- Atualização de 22/10/2019
-- Inclusão da parte de administração com usuário de adminstração da aplicação

drop table UsuarioAdministracaoAgenda
go

create table UsuarioAdministracaoAgenda
(	
	UsuarioAdministracaoAgendaID int not null identity(1,1)
	, Nome nvarchar(100) not null
	, [Login] nvarchar(20) not null
	, Senha nvarchar(300) not null
	, Ativo bit not null
)
go

alter table UsuarioAdministracaoAgenda add constraint PK_UsuarioAdministracaoAgenda primary key(UsuarioAdministracaoAgendaID)
go

alter table UsuarioAdministracaoAgenda add constraint UK_UsuarioAdministracaoAgenda_Login unique([Login])
go

alter table UsuarioAdministracaoAgenda add constraint DF_UsuarioAdministracaoAgenda_Ativo default 1 for Ativo
go

insert into UsuarioAdministracaoAgenda
(
	Nome
	, [Login]
	, Senha
	
)
values
(
	'Administrador'
	, 'admin'
	, 'IPK9NY3qv5MNjKWpxs1w7A==' -- edelweiss
)
go


-- Alteração na restrição da tabela para não permitir registros com mesmo tempo e unidade tempo.
alter table ConfiguracaoNotificacaoAgenda drop constraint UK_ConfiguracaoNotificacaoAgenda_Tempo
go

alter table ConfiguracaoNotificacaoAgenda add constraint UK_ConfiguracaoNotificacaoAgenda_Tempo_UnidadeTempoAgendaID unique(Tempo, UnidadeTempoAgendaID)
go
