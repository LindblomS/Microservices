create table [dbo].[user](
	[id] char(36) not null,
	[username] nvarchar(200),
	[normailized_username] nvarchar(200) not null,
	[password_hash] nvarchar(max) not null
	constraint [pk_user] primary key clustered([id] asc)
)

create table [dbo].[role](
	[id] nvarchar(25) not null,
	[display_name] nvarchar(50) not null,
	constraint [pk_role] primary key clustered ([id] asc)
)

create table [dbo].[role_claim](
	[id] int identity(1,1) not null,
	[claim_type] nvarchar(50) not null,
	[claim_value] nvarchar(50) not null,
	[role_id] nvarchar(25) not null references [dbo].[role](id) on delete cascade,
	constraint [pk_role_claim] primary key clustered ([id] asc)
)

create table [dbo].[user_claim](
	[id] int identity(1,1) not null,
	[claim_type] nvarchar(50) not null,
	[claim_value] nvarchar(50) not null,
	[user_id] char(36) not null references [dbo].[user](id) on delete cascade,
	constraint [pk_user_claim] primary key clustered ([id] asc)
)

create table [dbo].[user_role](
	[user_id] char(36) not null references [dbo].[user](id) on delete cascade,
	[role_id] nvarchar(25) not null references [dbo].[role](id) on delete cascade,
	constraint [pk_user_role] primary key (
		[user_id] asc, 
		[role_id] asc
	)
)

create table [dbo].[client_request](
	[id] UNIQUEIDENTIFIER not null,
	[name] varchar(100) not null,
	[time] datetime2 not null,
	constraint [pk_client_request] primary key clustered ([id] asc)
)

create table [dbo].[user_token](
	[user_id] char(36) not null references [dbo].[user](id) on delete cascade,
	[name] varchar(256) not null,
	[login_provider] varchar(256) not null,
	[value] varchar(max) not null,
	constraint [pk_user_token] primary key clustered (
		[user_id] asc, 
		[login_provider] asc, 
		[name] asc
	)
)

create table [dbo].[user_login](
	[user_id] char(36) not null references [dbo].[user](id) on delete cascade,
	[provider_key] varchar(128) not null,
	[login_provider] varchar(128) not null,
	[provider_display_name] varchar(256) null,
	constraint [pk_user_login] primary key clustered (
		[login_provider] asc, 
		[provider_key] asc
	)
)