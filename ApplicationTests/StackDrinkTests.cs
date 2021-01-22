using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application;
using System;

namespace ApplicationTests
{
    [TestClass()]
    public class StackDrinkTests
    {
        private readonly Drink _drink = new Drink("пепси", "url", 500, 120);
        protected Stack<Drink> StackDrink;

        [TestInitialize]
        public void Init()
        {
            StackDrink = new Stack<Drink>(_drink, 7);
        }

        [TestMethod()]
        public void AddTest()
        {
            StackDrink.Add(1);

            Assert.AreEqual(StackDrink.Quantity, 8);
        }

        [TestMethod()]
        public void AddNegativeTest()
        {
            StackDrink.Add(-1);

            Assert.AreEqual(StackDrink.Quantity, 6);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNegativeMaxValueTest()
        {
            StackDrink.Add( -Int32.MaxValue);
        }

        [TestMethod()]
        public void GiveOutTest()
        {
            StackDrink.GiveOut(1);

            Assert.AreEqual(StackDrink.Quantity, 6);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GiveOutNegativeTest()
        {
            StackDrink.GiveOut(-1);
        }
        [TestMethod(), ExpectedException(typeof(Exception))]
        public void GiveOutMaxValueTest()
        {
            StackDrink.GiveOut(Int32.MaxValue);
        }
    }
}