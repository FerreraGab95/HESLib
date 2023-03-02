using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HESDanfe.Test
{
    [TestClass]
    public class Utils_Test
    {
        [TestMethod]
        public void TipoDFeDeChaveAcesso()
        {
            Assert.AreEqual("NFC-e", Extensions.Util.TipoDFeDeChaveAcesso("00000000000000000000650000000000000000000000"));
            Assert.AreEqual("NF-e", Extensions.Util.TipoDFeDeChaveAcesso("00000000000000000000550000000000000000000000"));
            Assert.AreEqual("CT-e", Extensions.Util.TipoDFeDeChaveAcesso("00000000000000000000570000000000000000000000"));
        }
    }
}
