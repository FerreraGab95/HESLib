using System.Collections.Generic;
using System.Linq;
using Extensions.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.UnitTests
{
    [TestClass]
    public class Html1
    {
        static IEnumerable<HtmlNode> doc = LoadHtml();

        [TestMethod]
        public void IdSelectorMustReturnOnlyFirstElement()
        {
            var elements = doc.QuerySelectorAll("#myDiv");

            Assert.IsTrue(elements.Count() == 1);
            Assert.IsTrue(elements.IfNoIndex(0).ID == "myDiv");
            Assert.IsTrue(elements.IfNoIndex(0).GetAttribute("first") == "1");
        }

        [TestMethod]
        public void GetElementsByAttribute()
        {
            var elements = doc.QuerySelectorAll("*[id=myDiv]");

            Assert.IsTrue(elements.Distinct().Count() == 1 && elements.Count() == 1);
            for (int i = 0; i < elements.Count(); i++)
                Assert.IsTrue(elements.IfNoIndex(i).ID == "myDiv");
        }

        [TestMethod]
        public void GetElementsByClassName1()
        {
            var elements1 = doc.QuerySelectorAll(".cls-a");
            var elements2 = doc.QuerySelectorAll(".clsb");

            Assert.IsTrue(elements1.Count() == 1);
            for (int i = 0; i < elements1.Count(); i++)
                Assert.IsTrue(elements1.IfNoIndex(i) == elements2.IfNoIndex(i));
        }

        [TestMethod]
        public void GetElementsByClassName_MultiClasses()
        {
            var elements = doc.QuerySelectorAll(".cls-a, .cls-b");

            Assert.IsTrue(elements.Count() == 2);
            Assert.IsTrue(elements.IfNoIndex(0).ID == "spanA");
            Assert.IsTrue(elements.IfNoIndex(1).ID == "spanB");
        }

        [TestMethod]
        public void GetElementsByClassName_WithUnderscore()
        {
            var elements = doc.QuerySelectorAll(".underscore_class");

            Assert.IsTrue(elements.Count() == 1);
            Assert.IsTrue(elements.IfNoIndex(0).ID == "spanB");
        }



        private static IEnumerable<HtmlNode> LoadHtml()
        {
            return HtmlNode.Parse(Resource.GetString("Test1.html"));


        }

    }
}
