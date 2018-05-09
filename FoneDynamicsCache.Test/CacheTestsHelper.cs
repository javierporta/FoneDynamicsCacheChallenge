using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace FoneDynamicsCache.Test
{
    class CacheTestsHelper
    {
        /// <summary>
        /// //check cache limit
        /// </summary>
        /// <param name="cacheSize"></param>
        /// <param name="cache"></param>
        public static void AssertCacheLimit(int cacheSize, FoneDynamicsCache cache)
        {
            cache.SpaceUsed.Should().BeLessOrEqualTo(cacheSize, "because cache exceeded limit");
        }
    }
}
