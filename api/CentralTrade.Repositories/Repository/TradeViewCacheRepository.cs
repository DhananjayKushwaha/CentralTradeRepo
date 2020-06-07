using CentralTrade.Models;
using CentralTrade.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CentralTrade.Repositories
{
    //Implemented in meory cache layer - it does not make sense for this use case - just added to demo data caching
    //Caching can be relaced with any persistent caching machanism
    public class TradeViewCacheRepository : ITradeViewCacheRepository
    {
        private const string CacheKey = "TrendingStocks_{0}";
        private readonly IMemoryCache _memoryCache;
        private readonly ITradeViewRepository _tradeViewRepository;

        public TradeViewCacheRepository(ITradeViewRepository tradeViewRepository, IMemoryCache memoryCache)
        {
            _tradeViewRepository = tradeViewRepository;
            _memoryCache = memoryCache;
        }

        public void ClearCache(int topN)
        {
            _memoryCache.Remove(string.Format(CacheKey,topN));
        }

        public async Task<List<StockView>> GetTrendingStocks(int topN, CancellationToken cancellationToken)
        {
            string cacheKey = string.Format(CacheKey, topN);

            if (_memoryCache.TryGetValue(cacheKey, out List<StockView> stockViews))
            {
                return await Task.FromResult(stockViews);
            }

            var repoStockViews = await _tradeViewRepository.GetTrendingStocks(topN, cancellationToken);

            _memoryCache.Set(cacheKey, repoStockViews, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(60 * 60 * 60),//add in confifuration file
                Priority = CacheItemPriority.Normal//add in confifuration file
            });

            return repoStockViews;
        }
    }
}
