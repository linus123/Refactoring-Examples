namespace SharedKernel.Persistence
{
    // Pretend we are calling a database to get the data.
    public class TradeRequestCollectionRepository
    {
        public TradeRequestCollection GetTradeRequests()
        {
            var preferencePrimaryOnly = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = true,
                CapacityPrimaryLimitBuy = 0.01m,
                CapacityPrimaryLimitSell = 0.02m,
                IsHeavilyTradeFilterActive = false,
                IsCapacityEncumberedSharesFilterActive = false,
            };

            var preferenceHeavilyTradedOnly = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = false,
                IsHeavilyTradeFilterActive = true,
                StockHeavilyTradeDay = 2,
                StockHeavilyTradeVolume = 0.9m,
                IsCapacityEncumberedSharesFilterActive = false,
            };

            var preferenceEncumberedOnly = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = false,
                IsHeavilyTradeFilterActive = false,
                IsCapacityEncumberedSharesFilterActive = true,
            };

            var stock01 = new Stock()
            {
                StockId = "0001",
                PriceInUsd = 600,
                SharePrice = 50
            };

            stock01.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var stock02 = new Stock()
            {
                StockId = "0002",
                PriceInUsd = 54,
                SharePrice = 50
            };

            var tradeRequests = new TradeRequest[]
            {
                new TradeRequest()
                {
                    TradeSide = TradeSide.Buy,
                    TradeRequestId = 500,
                    OriginalCapacityQuantity = 600,
                    Stock = stock01,
                    PrimaryLimit = 10,
                    TradeFilterPreference = preferencePrimaryOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Sell,
                    TradeRequestId = 501,
                    OriginalCapacityQuantity = 100,
                    HoldingsQuantity = 30,
                    EncumberedQuantity = 20,
                    Stock = stock01,
                    TradeFilterPreference = preferenceHeavilyTradedOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Buy,
                    TradeRequestId = 502,
                    OriginalCapacityQuantity = 1000,
                    Stock = stock02,
                    TradeFilterPreference = preferenceEncumberedOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Sell,
                    TradeRequestId = 502,
                    OriginalCapacityQuantity = 1000,
                    Stock = stock02,
                    TradeFilterPreference = preferenceEncumberedOnly
                },
            };

            return new TradeRequestCollection(
                tradeRequests);
        }
    }
}