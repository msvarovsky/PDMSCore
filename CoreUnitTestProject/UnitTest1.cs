using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDMSCore.DataManipulation;

namespace CoreUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            Menu m = new Menu();
            m.HtmlText();


            Assert.AreEqual(1, 1);

        }
    }
}
