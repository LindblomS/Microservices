create table [dbo].[user](
	[id] UNIQUEIDENTIFIER not null,
	[username] nvarchar(200),
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
	[user_id] UNIQUEIDENTIFIER not null references [dbo].[user](id) on delete cascade,
	constraint [pk_user_claim] primary key clustered ([id] asc)
)

create table [dbo].[user_role](
	[user_id] UNIQUEIDENTIFIER not null references [dbo].[user](id) on delete cascade,
	[role_id] nvarchar(25) not null references [dbo].[role](id) on delete cascade,
	constraint [pk_user_role] primary key (
		[user_id] asc, 
		[role_id] asc
	)
)