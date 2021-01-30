using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Web.Application;

namespace ApplicationTests
{
    [TestClass()]
    public class MachineStackCoinTests
    {
        private readonly Coin _coin = new Coin(EnumCoin.Five);
        protected MachineStack<Coin> StackCoin;

        [TestInitialize]
        public void Init()
        {
            StackCoin = new MachineStack<Coin>(_coin, 7);
        }

        [TestMethod()]
        public void AddTest()
        {
            StackCoin.Add(1);

            Assert.AreEqual(StackCoin.Quantity, 8);
        }

        [TestMethod()]
        public void AddNegativeTest()
        {
            StackCoin.Add(-1);

            Assert.AreEqual(StackCoin.Quantity, 6);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNegativeMaxValueTest()
        {
            StackCoin.Add( -Int32.MaxValue);
        }

        [TestMethod()]
        public void GiveOutTest()
        {
            StackCoin.GiveOut(1);

            Assert.AreEqual(StackCoin.Quantity, 6);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GiveOutNegativeTest()
        {
            StackCoin.GiveOut(-1);
        }
        [TestMethod(), ExpectedException(typeof(Exception))]
        public void GiveOutMaxValueTest()
        {
            StackCoin.GiveOut(Int32.MaxValue);
        }
    }
}