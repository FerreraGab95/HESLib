using System.Linq;
using Extensions;
using Extensions.UnitTests;
using Extensions.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HES.Test
{
    [TestClass]
    public class Html1
    {
        static HtmlDocument doc = LoadHtml();





        [TestMethod]
        public void IdSelectorMustReturnOnlyFirstElement()
        {
            var elements = doc.QuerySelectorAll("#myDiv");

            Assert.IsTrue(elements.IfNoIndex(0).GetAttribute("first") == "1");
        }

        [TestMethod]
        public void GetElementsByAttribute()
        {
            var elements = doc.QuerySelectorAll("*[id=myDiv]");

            Assert.IsTrue(elements.Distinct().Count() == 1 && elements.Count() == 1);
            for (int i = 0; i < elements.Count(); i++)
                Assert.IsTrue(elements.IfNoIndex(i).Id == "myDiv");
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
            Assert.IsTrue(elements.IfNoIndex(0).Id == "spanA");
            Assert.IsTrue(elements.IfNoIndex(1).Id == "spanB");
        }

        [TestMethod]
        public void GetElementsByClassName_WithUnderscore()
        {
            var elements = doc.QuerySelectorAll(".underscore_class");

            Assert.IsTrue(elements.Count() == 1);
            Assert.IsTrue(elements.IfNoIndex(0).Id == "spanB");
        }



        private static HtmlDocument LoadHtml()
        {
            return HtmlNode.ParseDocument(Resource.GetString("Test1.html"));


        }

    }
}
