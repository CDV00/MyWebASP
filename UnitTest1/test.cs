using CaoDinhVu.Context;
using NUnit.Framework;
using NUnit.Framework;

namespace UnitTest1
{
    [TestFixture]
    public class test
    {
        private CaoDinhVu067Entities _context = new CaoDinhVu067Entities();
        private int a, b,c;
        [SetUp]
        public void SetUp()
        {
            _context = new CaoDinhVu067Entities();
            a = 5;
            b = 5;
            c = 15;
        }
        
        [Test]
        public void TestMethod()
        {
            Assert.AreEqual(a, b);
        }
        [Test]
        
        public void TestMethod1()
        {
           
        }

    }
}  
