using NUnit.Framework;
using PencilDurability.Pencil;
using PencilDurability.Utility;
using PencilDurability.Paper;
using PencilDurability.Exceptions;
using PencilDurability.Erase;

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
            Pencil pencil = new Pencil(40000, 10);
            WriterUtility util = new WriterUtility();
            var sheetOfPaper = util.GetASheetOfPaper();
            var result = pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(result, sheetOfPaper.Text);
            result = pencil.WriteToSheetOfPaper("ing 123", sheetOfPaper);
            Assert.AreEqual(result, sheetOfPaper.Text);
            Assert.AreEqual("Testing 123", sheetOfPaper.Text);
        }
        [Test]
        public void TestGetASheetOfPaper()
        {
            WriterUtility util = new WriterUtility();
            var result = util.GetASheetOfPaper();
            Assert.IsTrue(result is SheetOfPaper);
        }
        [Test]
        public void TestPencilHasAPoint()
        {
            var pencil = new Pencil(40000, 10);
            Assert.AreEqual(pencil.point, 40000);
        }
        [Test]
        public void TestPointDegredation()
        {
            Pencil pencil = new Pencil(40000, 10);
            WriterUtility util = new WriterUtility();
            var sheetOfPaper = util.GetASheetOfPaper();
            var result = pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(pencil.point, 39995);

            result = pencil.WriteToSheetOfPaper("Tes ", sheetOfPaper);
            Assert.AreEqual(pencil.point, 39991);
        }
        [Test]
        public void TestPencilLength()
        {
            Pencil pencil = new Pencil(40000, 10);
            Assert.AreEqual(pencil.length, 10);
        }
        [Test]
        public void TestSharpenPencil()
        {
            Pencil pencil = new Pencil(40000, 10);
            WriterUtility util = new WriterUtility();
            var sheetOfPaper = util.GetASheetOfPaper();
            var result = pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(pencil.point, 39995);

            pencil.Sharpen(40000);
            Assert.AreEqual(pencil.point, 40000);
        }
        [Test]
        public void TestSharpenPencilReducesLength()
        {
            Pencil pencil = new Pencil(40000, 10);
            WriterUtility util = new WriterUtility();
            var sheetOfPaper = util.GetASheetOfPaper();
            var result = pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(pencil.point, 39995);

            pencil.Sharpen(40000);
            Assert.AreEqual(pencil.point, 40000);
            Assert.AreEqual(pencil.length, 9);
        }
        [Test]
        public void TestPencilLengthZeroProhibitsSharpening()
        {
            Pencil pencil = new Pencil(40000, 0);
            TestDelegate d = ()=>pencil.Sharpen(40000);
            Assert.Throws<CannotSharpenPencilLengthZeroException>(d, "", new object[1]);
        }
        [Test]
        public void TestEraseText()
        {
            Pencil pencil = new Pencil(40000, 10);
            WriterUtility util = new WriterUtility();
            var sheetOfPaper = util.GetASheetOfPaper();
            var result = pencil.WriteToSheetOfPaper("Testing 123", sheetOfPaper);

            Eraser eraser = new Eraser();
            var eraserResult = eraser.Erase(result, "123");
            Assert.AreEqual(eraserResult, "Testing ");
        }
    }
}