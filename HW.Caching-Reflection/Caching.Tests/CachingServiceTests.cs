using System.Threading;
using NUnit.Framework;

namespace Caching.Tests
{
    [TestFixture]
    public class CachingServiceTests
    {
        [Caching(0, 0, 1)]
        private class Pizza
        {
            public string Title { get; set; }
        }

        [Test]
        public void SetToCacheAndGetFromCache_Pizza_Test()
        {
            var cachingService = new CachingService();
            var pizza = new Pizza() { Title = "Pepperoni" };

            cachingService.SetToCache("PepperoniPizza", pizza);
            var pizzaFromCache = cachingService.GetFromCache<Pizza>("PepperoniPizza");

            Assert.AreEqual(pizza.Title, pizzaFromCache.Title);
        }

        [Test]
        public void SetToCacheAndGetFromCache_Null_Test()
        {
            var cachingService = new CachingService();
            var pizza = new Pizza() { Title = "Pepperoni" };

            cachingService.SetToCache("PepperoniPizza", pizza);

            Thread.Sleep(1000);

            var pizzaFromCache = cachingService.GetFromCache<Pizza>("PepperoniPizza");

            Assert.IsNull(pizzaFromCache);
        }
    }
}