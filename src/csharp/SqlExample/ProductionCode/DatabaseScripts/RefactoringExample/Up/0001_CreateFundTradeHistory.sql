CREATE SCHEMA FundTradeHistory

GO

CREATE TABLE [FundTradeHistory].[Trade](
    [TradeId] [int] NOT NULL,
    [FundId] [uniqueidentifier] NOT NULL,
    [TradeDate] [DateTime] NOT NULL,
    [BrokerCode] [nvarchar](64) NOT NULL,
    [Shares] [decimal](18, 9)   NOT NULL
 CONSTRAINT [PK_ME_FeatureFlag_Code] PRIMARY KEY CLUSTERED 
(
    [TradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
