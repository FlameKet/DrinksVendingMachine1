using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Web.Application;
using Web.Context;

namespace ApplicationTests
{
    [TestClass()]
    public class RepositoryCoinTests
    {
        protected RepositoryCoin RepositoryCoin;
        private readonly Coin _coin = new Coin(EnumCoin.Five);
        private readonly int _quality = 5;

        [TestInitialize]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationContext(options);

            RepositoryCoin = new RepositoryCoin(context);
            RepositoryCoin.Add(_coin, _quality);
        }

        [TestMethod()]
        public void AddNewCoinTest()
        {
            var coin = new Coin(EnumCoin.One);

            var result = RepositoryCoin.Add(coin, 5);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, 5);
            Assert.AreEqual(result.Entity, coin);
            Assert.AreNotEqual(result.Id, 0);
        }
        [TestMethod()]
        public void AddTest()
        {
            var result = RepositoryCoin.Add(_coin, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, _quality + 1);
            Assert.AreEqual(result.Entity, _coin);
        }
        [TestMethod()]
        public void AddNegativeTest()
        {
            var result = RepositoryCoin.Add(_coin, -1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, _quality - 1);
            Assert.AreEqual(result.Entity, _coin);
        }
        [TestMethod()]
        public void AddZeroQuantityTest()
        {
            var result = RepositoryCoin.Add(_coin, 0);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, _quality );
            Assert.AreEqual(result.Entity, _coin);
        }

        [TestMethod()]
        public void GiveOutTest()
        {
            var result = RepositoryCoin.GiveOut(_coin, 2);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, _quality-2);
            Assert.AreEqual(result.Entity, _coin);
        }

        [TestMethod(), ExpectedException(typeof(Exception))]
        public void GiveOutMaxValueTest()
        {
            RepositoryCoin.GiveOut(_coin, Int32.MaxValue);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GiveOutNegativeTest()
        {
            RepositoryCoin.GiveOut(_coin, -1);
        }

        [TestMethod()]
        public void ChangePriceTest()
        {
            Assert.AreEqual(_coin.Blocking, false);

            _coin.ChangeBlocking(true);
            var result = RepositoryCoin.Get(_coin);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Entity.Equals(_coin));
            Assert.AreEqual(result.Entity.Blocking, true);
        }
    }
}