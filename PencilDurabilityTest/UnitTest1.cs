using NUnit.Framework;
using PencilDurability.Pencil;
using PencilDurability.Utility;
using PencilDurability.Paper;

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
            WriterUtility util = new WriterUtility();
            var sheetOfPaper = util.GetASheetOfPaper();
            var result = pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(result, sheetOfPaper.Text);
        }
        [Test]
        public void TestGetASheetOfPaper()
        {
            WriterUtility util = new WriterUtility();
            var result = util.GetASheetOfPaper();
            Assert.IsTrue(result is SheetOfPaper);
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