using System;
using System.Collections.Generic;
using Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApplicationTests
{
    [TestClass()]
    public class RepositoryDrinkTests
    {
        protected Repository<Drink> RepositoryDrink;
        private readonly Drink _drink = new Drink("пепси", "url", 500, 120);
        private  Application.Stack<Drink> _stackDrink;
        private readonly int _quality = 5;

        [TestInitialize]
        public void Init()
        {
            _stackDrink = new Application.Stack<Drink>(_drink, _quality);
            RepositoryDrink = new Repository<Drink>(new List<Application.Stack<Drink>>{ _stackDrink });
        }

        [TestMethod()]
        public void AddNewDrinkTest()
        {
            var drink = new Drink("кола", "url", 250, 50);

            var result = RepositoryDrink.Add(drink, 5);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Quantity, 5);
            Assert.AreEqual(result.Entity, drink);
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
        public void ReEevaluateTest()
        {
            Assert.AreEqual(_drink.Price, 120);

            _drink.ReEevaluate(150);
            var result = RepositoryDrink.Get(_drink);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Entity.Equals(_drink));
            Assert.AreEqual(result.Entity.Price, 150);
        }
    }
}