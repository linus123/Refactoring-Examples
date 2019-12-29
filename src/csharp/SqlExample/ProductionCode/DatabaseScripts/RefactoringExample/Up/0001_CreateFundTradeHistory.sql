CREATE SCHEMA FundTradeHistory

GO

CREATE TABLE [FundTradeHistory].[Trade](
    [TradeId] [int] NOT NULL,
    [StockId] [uniqueidentifier] NOT NULL,
    [TradeDate] [DateTime] NOT NULL,
    [BrokerCode] [nvarchar](64) NOT NULL,
    [Shares] [decimal](18, 9)   NOT NULL
 CONSTRAINT [PK_FundTradeHistory_Trade] PRIMARY KEY CLUSTERED 
(
    [TradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [FundTradeHistory].[StockData](
    [StockId] [uniqueidentifier] NOT NULL,
    [Source] [int]   NOT NULL,
    [DataType] [int]   NOT NULL,
    [DataDate] [DateTime] NOT NULL,
    [BrokerCode] [nvarchar](64) NOT NULL,
    [Value] [decimal](18, 9)   NOT NULL,
 CONSTRAINT [PK_FundTradeHistory_StockData] PRIMARY KEY CLUSTERED 
(
    [StockId], [Source], [DataType], [DataDate]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
