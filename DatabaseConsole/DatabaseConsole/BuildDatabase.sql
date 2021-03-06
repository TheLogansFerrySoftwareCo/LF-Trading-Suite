USE [StockScreener]
GO
/****** Object:  Table [dbo].[Exchanges]    Script Date: 06/29/2012 18:57:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Exchanges](
	[ID] [varchar](10) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Exchanges] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Watchlists]    Script Date: 06/29/2012 18:57:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Watchlists](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Watchlists] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stocks]    Script Date: 06/29/2012 18:57:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Stocks](
	[Exchange] [varchar](10) NOT NULL,
	[Ticker] [varchar](10) NOT NULL,
	[CompanyName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED 
(
	[Ticker] ASC,
	[Exchange] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WatchListsToStocks]    Script Date: 06/29/2012 18:57:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WatchListsToStocks](
	[WatchListId] [int] NOT NULL,
	[Ticker] [varchar](10) NOT NULL,
	[Exchange] [varchar](10) NOT NULL,
 CONSTRAINT [PK_WatchListsToStocks] PRIMARY KEY CLUSTERED 
(
	[WatchListId] ASC,
	[Ticker] ASC,
	[Exchange] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StockDailies]    Script Date: 06/29/2012 18:57:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StockDailies](
	[Exchange] [varchar](10) NOT NULL,
	[Ticker] [varchar](10) NOT NULL,
	[Date] [date] NOT NULL,
	[OpenPrice] [float] NOT NULL,
	[ClosePrice] [float] NOT NULL,
	[HighPrice] [float] NOT NULL,
	[LowPrice] [float] NOT NULL,
	[Volume] [int] NOT NULL,
 CONSTRAINT [PK_StockDailies] PRIMARY KEY CLUSTERED 
(
	[Exchange] ASC,
	[Ticker] ASC,
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK_StockDailies_Stocks]    Script Date: 06/29/2012 18:57:52 ******/
ALTER TABLE [dbo].[StockDailies]  WITH CHECK ADD  CONSTRAINT [FK_StockDailies_Stocks] FOREIGN KEY([Ticker], [Exchange])
REFERENCES [dbo].[Stocks] ([Ticker], [Exchange])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockDailies] CHECK CONSTRAINT [FK_StockDailies_Stocks]
GO
/****** Object:  ForeignKey [FK_Stocks_Exchanges]    Script Date: 06/29/2012 18:57:52 ******/
ALTER TABLE [dbo].[Stocks]  WITH CHECK ADD  CONSTRAINT [FK_Stocks_Exchanges] FOREIGN KEY([Exchange])
REFERENCES [dbo].[Exchanges] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stocks] CHECK CONSTRAINT [FK_Stocks_Exchanges]
GO
/****** Object:  ForeignKey [FK_WatchListsToStocks_Stocks]    Script Date: 06/29/2012 18:57:52 ******/
ALTER TABLE [dbo].[WatchListsToStocks]  WITH CHECK ADD  CONSTRAINT [FK_WatchListsToStocks_Stocks] FOREIGN KEY([Ticker], [Exchange])
REFERENCES [dbo].[Stocks] ([Ticker], [Exchange])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WatchListsToStocks] CHECK CONSTRAINT [FK_WatchListsToStocks_Stocks]
GO
/****** Object:  ForeignKey [FK_WatchListsToTickers_Watchlists]    Script Date: 06/29/2012 18:57:52 ******/
ALTER TABLE [dbo].[WatchListsToStocks]  WITH CHECK ADD  CONSTRAINT [FK_WatchListsToTickers_Watchlists] FOREIGN KEY([WatchListId])
REFERENCES [dbo].[Watchlists] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WatchListsToStocks] CHECK CONSTRAINT [FK_WatchListsToTickers_Watchlists]
GO
