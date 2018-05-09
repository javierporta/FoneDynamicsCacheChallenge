using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoneDynamicsCache;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

//use nunit, use moq, reseach hot to unit test private methods
namespace FoneDynamicsCache.Test
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void ShouldAdd4KeysGet1KeyAndRemoveLRU()
        {
            var cacheSize = 3;
            var cache = new FoneDynamicsCache(cacheSize);
            var keyToTest = "2";
            var valueToTest = "cumbia2";
            cache.AddOrUpdate("1", "cumbia1");

            cache.AddOrUpdate(keyToTest, valueToTest);

            cache.AddOrUpdate("3", "cumbia3");
            cache.AddOrUpdate("4", "cumbia4");

            object resultValue = "";
            if (cache.TryGetValue(keyToTest, out resultValue))
            {
                Assert.AreEqual(valueToTest, resultValue);
            }
            
            CacheTestsHelper.AssertCacheLimit(cacheSize, cache);
        }

        [TestMethod]
        public void ShouldUpdateLRUWhenGetValue()
        {
            var cacheSize = 3;
            var cache = new FoneDynamicsCache(cacheSize);
            var key1 = "1";
            var value1 = "cumbia1";
            var key2 = "2";
            var value2 = "cumbia2";
            var key4 = "4";
            var value4 = "cumbia4";

            cache.AddOrUpdate(key1, value1);
            cache.AddOrUpdate("2", "cumbia2");
            cache.AddOrUpdate("3", "cumbia3");
            object resultValue = "";
            cache.TryGetValue("1", out resultValue); //here it has to update key 1, so "2" should be the new LFU

            cache.AddOrUpdate(key4, value4);
            cache.TryGetValue(key1, out resultValue);

            //check that "1" is still present in cache
            Assert.AreEqual(value1, resultValue, "LRU was not updated correctly and key1 was deleted.");

            //check that "2" is no longer in cache
            object resultValueTwo = "";
            cache.TryGetValue(key2, out resultValueTwo);
            Assert.AreNotEqual(value2, resultValueTwo, "key2 was not deleted and it is equal to value2");
            Assert.AreEqual(null, resultValueTwo, "key2 was not deleted and is not null");

            
            CacheTestsHelper.AssertCacheLimit(cacheSize, cache);
        }

        [TestMethod]
        public void ShouldReturnNullWhenTryingToGetFromEmptyCache()
        {
            var cacheSize = 3;
            var cache = new FoneDynamicsCache(cacheSize);

            object foo = new List<string>();
            cache.TryGetValue("foo", out foo);

            Assert.AreEqual(null, foo, "foo value is not null");

            CacheTestsHelper.AssertCacheLimit(cacheSize, cache);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldNotAllowCacheSizeLessThan1()
        {
            var cacheSize = -5;
            var cache = new FoneDynamicsCache(cacheSize);         
        }

        [TestMethod]
        public void ShouldBePreparedFBombardment()
        {
            var cacheSize = 1000;
            var cache = new FoneDynamicsCache(cacheSize);

            Task[] tasks = new Task[4];
            
            tasks[0]= new Task(() =>
                Parallel.For(0, 5000, i =>
                {
                    cache.AddOrUpdate(i.ToString(), i);
                })
            );

            tasks[1] = new Task(() =>
                 Parallel.For(0, 5000, i =>
                 {
                     object foo = i;
                     cache.TryGetValue(i.ToString(), out foo);
                 })
            );

            tasks[2] = new Task(() =>
                 Parallel.For(0, 5000, i =>
                 {
                     cache.AddOrUpdate(i.ToString(), i);
                 })
            );

            tasks[3] = new Task(() =>
                 Parallel.For(0, 5000, i =>
                 {
                     object foo = i;
                     cache.TryGetValue(i.ToString(), out foo);
                 })
            );

            for (int i = 0; i < 4; i++)
            {
                tasks[i].Start();
            }

            Task.WaitAll(tasks);

            //no error
            Assert.AreEqual(true, true);
        }
    }
}
