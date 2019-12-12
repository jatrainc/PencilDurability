using NUnit.Framework;
using PencilDurability.Pencil;
using PencilDurability.Utility;

namespace PencilDurabilityTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestPencilWritesToASheetOfPaper()
        {
            Pencil pencil = new Pencil();
            var result = pencil.WriteToSheetOfPaper("Test");

            Assert.AreEqual(result,"Test");
        }
        [Test]
        public void TestGetASheetOfPaper()
        {
            WriterUtility util = new WriterUtility();
            var result = util.GetASheetOfPaper();

            Assert.AreEqual(result, true);
        }
        [Test]
        public void TestCreatePencil()
        {
            Assert.AreEqual(1, 1);
        }
        [Test]
        public void TestPointDegredation()
        {
            Assert.AreEqual(1, 1);
        }
    }
}