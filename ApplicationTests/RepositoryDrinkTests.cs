using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Web.Application;
using Web.Context;

namespace ApplicationTests
{
    [TestClass()]
    public class RepositoryDrinkTests
    {
        protected RepositoryDrink RepositoryDrink;
        private readonly Drink _drink = new Drink("пепси", "url", 500, 120);
        private readonly int _quality = 5;

        [TestInitialize]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationContext(options);

            RepositoryDrink = new RepositoryDrink(context);
            RepositoryDrink.Add(_drink, _quality);
        }

        [TestMethod()]
        public void AddNewDrinkTest()
        {
            var drink = new Drink("кола", "url", 250, 50);

            var result = RepositoryDrink.Add(drink, 5);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, 5);
            Assert.AreEqual(result.Entity, drink);
            Assert.AreNotEqual(result.Id, 0);
        }
        [TestMethod(), ExpectedException(typeof(ArgumentNullException))]
        public void AddNullNameDrinkTest()
        {
            RepositoryDrink.Add(new Drink(null, "url", 250, 50), 5);
        }
        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddZeroPriceDrinkTest()
        {
            RepositoryDrink.Add(new Drink("кола", "url", 100, 0), 5);
        }
        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddZeroVolumeDrinkTest()
        {
            RepositoryDrink.Add(new Drink("кола", "url", 0, 50), 5);
        }
        [TestMethod()]
        public void AddDrinkTest()
        {
            var result = RepositoryDrink.Add(_drink, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, _quality + 1);
            Assert.AreEqual(result.Entity, _drink);
        }
        [TestMethod()]
        public void AddNegativeDrinkTest()
        {
            var result = RepositoryDrink.Add(_drink, -1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, _quality - 1);
            Assert.AreEqual(result.Entity, _drink);
        }
        [TestMethod()]
        public void AddZeroQuantityDrinkTest()
        {
            var result = RepositoryDrink.Add(_drink, 0);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, _quality );
            Assert.AreEqual(result.Entity, _drink);
        }

        [TestMethod()]
        public void GiveOutTest()
        {
            var result = RepositoryDrink.GiveOut(_drink, 2);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, _quality-2);
            Assert.AreEqual(result.Entity, _drink);
        }

        [TestMethod(), ExpectedException(typeof(Exception))]
        public void GiveOutMaxValueTest()
        {
            RepositoryDrink.GiveOut(_drink, Int32.MaxValue);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GiveOutNegativeTest()
        {
            RepositoryDrink.GiveOut(_drink, -1);
        }

        [TestMethod()]
        public void ChangePriceTest()
        {
            Assert.AreEqual(_drink.Price, 120);

            _drink.ChangePrice(150);
            var result = RepositoryDrink.Get(_drink);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Entity.Equals(_drink));
            Assert.AreEqual(result.Entity.Price, 150);
        }
        [TestMethod(), ExpectedException(typeof(Exception))]
        public void DeleteNotFoundTest()
        {
            RepositoryDrink.Delete(new Drink("drink","",1,1));
        }
        [TestMethod()]
        public void DeleteTest()
        {
            RepositoryDrink.Delete(_drink);

            var result = RepositoryDrink.Get(_drink);

            Assert.IsNull(result);
        }
    }
}