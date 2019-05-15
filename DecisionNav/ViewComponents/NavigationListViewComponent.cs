using DecisionNav.Data;
using DecisionNav.Helpers;
using DecisionNav.Models;
using DecisionNav.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecisionNav.ViewComponents
{
    public class NavigationListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public NavigationListViewComponent(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(
        int maxPriority, bool isDone, string position)
        {
            IEnumerable<NavigationList> cacheEntry;

            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = await _context.NavigationList.ToListAsync();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(15));

                // Save data in cache.
                _cache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }
            if (position=="left")
            {
                return View("LeftMenu", cacheEntry);
            }
            else
            {
                return View(cacheEntry);
            }
            
        }

    }
}
