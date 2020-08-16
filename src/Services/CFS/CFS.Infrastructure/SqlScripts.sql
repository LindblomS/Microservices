CREATE TABLE [dbo].[Customers](
	[customerId] [int] IDENTITY(1,1) NOT NULL,
	[firstName] [varchar](max) NULL,
	[lastName] [varchar](max) NULL,
	[phoneNumber] [varchar](max) NULL,
	[email] [varchar](max) NULL,
	[street] [varchar](max) NULL,
	[city] [varchar](max) NULL,
	[state] [varchar](max) NULL,
	[country] [varchar](max) NULL,
	[zipCode] [varchar](max) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Facilities](
	[facilityId] [int] IDENTITY(1,1) NOT NULL,
	[customerId] [int] NULL,
	[facilityName] [varchar(50)],
	[street] [varchar](max) NULL,
	[city] [varchar](max) NULL,
	[state] [varchar](max) NULL,
	[country] [varchar](max) NULL,
	[zipCode] [varchar](max) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Services](
	[serviceId] [int] IDENTITY(1,1) NOT NULL,
	[facilityId] [int] NULL,
	[startDate] [date] NULL,
	[stopdate] [date] NULL
) ON [PRIMARY]
GO