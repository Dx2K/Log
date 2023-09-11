/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2012 (11.0.5058)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [LOGSISTEMAS]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 29-12-2017 11:14:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sistema] [int] NOT NULL,
	[Tipo] [int] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Excepcion] [nvarchar](max) NULL,
	[Mensaje] [nvarchar](max) NULL,
	[MensajeExcepcion] [nvarchar](max) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sistema]    Script Date: 29-12-2017 11:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sistema](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Glosa] [nvarchar](250) NOT NULL,
	[Sigla] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Sistema] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tipo]    Script Date: 29-12-2017 11:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tipo](
	[Id] [int] NOT NULL,
	[Glosa] [nchar](50) NOT NULL,
 CONSTRAINT [PK_Tipo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Log] ON 
GO

SET IDENTITY_INSERT [dbo].[Log] OFF
GO
SET IDENTITY_INSERT [dbo].[Sistema] ON 
GO
INSERT [dbo].[Sistema] ([Id], [Glosa], [Sigla]) VALUES (1, N'Sistema de Solicitudes', N'SISOL     ')
GO
SET IDENTITY_INSERT [dbo].[Sistema] OFF
GO
INSERT [dbo].[Tipo] ([Id], [Glosa]) VALUES (1, N'Error                                             ')
GO
INSERT [dbo].[Tipo] ([Id], [Glosa]) VALUES (2, N'Info                                              ')
GO
INSERT [dbo].[Tipo] ([Id], [Glosa]) VALUES (3, N'Debug                                             ')
GO
INSERT [dbo].[Tipo] ([Id], [Glosa]) VALUES (4, N'Warning                                           ')
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_Sistema] FOREIGN KEY([Sistema])
REFERENCES [dbo].[Sistema] ([Id])
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_Sistema]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_Tipo] FOREIGN KEY([Tipo])
REFERENCES [dbo].[Tipo] ([Id])
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_Tipo]
GO
/****** Object:  StoredProcedure [dbo].[pa_guardar]    Script Date: 29-12-2017 11:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dx
-- Create date: 20171002
-- Description:	Guarda datos en el Log
-- =============================================
CREATE PROCEDURE [dbo].[pa_guardar]
	@sistema INT,
	@tipo INT,
	@fecha DATETIME,
	@excepcion NVARCHAR(MAX),
	@mensaje NVARCHAR(MAX),
	@mensaje2 NVARCHAR(MAX),
	@salida INT OUTPUT
AS
BEGIN
	INSERT INTO [dbo].[Log]
           ([Sistema]
           ,[Tipo]
           ,[Fecha]
           ,[Excepcion]
           ,[Mensaje]
		   ,[MensajeExcepcion])
     VALUES
           (@sistema
           ,@tipo 
           ,@fecha
           ,@excepcion
           ,@mensaje
		   ,@mensaje2)

	declare @iden int = 0;

	SELECT @iden = @@IDENTITY;


	if (@iden = 0)
	begin
		set @salida = -1;
	end
	else
	begin
		set @salida = @iden;
	end
	return @salida;
END
GO
