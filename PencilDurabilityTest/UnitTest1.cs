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
        private Pencil _pencil;
        private WriterUtility _writerUtility;
        [SetUp]
        public void Setup()
        {
            _pencil = new Pencil(40000, 10, 80000);
            _writerUtility = new WriterUtility();
        }

        [Test]
        public void TestPencilWritesToASheetOfPaper()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            var result = _pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(result, sheetOfPaper.Text);
            result = _pencil.WriteToSheetOfPaper("ing 123", sheetOfPaper);
            Assert.AreEqual(result, sheetOfPaper.Text);
            Assert.AreEqual("Testing 123", sheetOfPaper.Text);
        }
        [Test]
        public void TestGetASheetOfPaper()
        {
            var result = _writerUtility.GetASheetOfPaper();
            Assert.IsTrue(result is SheetOfPaper);
        }
        [Test]
        public void TestPencilHasAPoint()
        {
            Assert.AreEqual(_pencil.point, 40000);
        }
        [Test]
        public void TestPointDegredation()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            var result = _pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(_pencil.point, 39995);

            result = _pencil.WriteToSheetOfPaper("Tes ", sheetOfPaper);
            Assert.AreEqual(_pencil.point, 39991);
        }
        [Test]
        public void TestPencilLength()
        {
            _pencil.length = 11;
            Assert.AreEqual(_pencil.length, 11);
        }
        [Test]
        public void TestSharpenPencil()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            var result = _pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(_pencil.point, 39995);

            _pencil.Sharpen(40000);
            Assert.AreEqual(_pencil.point, 40000);
        }
        [Test]
        public void TestSharpenPencilReducesLength()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            var result = _pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(_pencil.point, 39995);

            _pencil.Sharpen(40000);
            Assert.AreEqual(_pencil.point, 40000);
            Assert.AreEqual(_pencil.length, 9);
        }
        [Test]
        public void TestPencilLengthZeroProhibitsSharpening()
        {
            _pencil.length = 0;
            TestDelegate d = ()=>_pencil.Sharpen(40000);
            Assert.Throws<CannotSharpenPencilLengthZeroException>(d, "", new object[1]);
        }
        [Test]
        public void TestEraseLastOccurrenceOfText()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            var result = _pencil.WriteToSheetOfPaper("Testing 123 123", sheetOfPaper);

            Eraser eraser = new Eraser(40000);
            var eraserResult = eraser.Erase(result, "123");
            Assert.AreEqual(eraserResult, "Testing 123    ");

            sheetOfPaper = _writerUtility.GetASheetOfPaper();
            result = _pencil.WriteToSheetOfPaper("How much wood would a woodchuck chuck if a woodchuck could chuck wood?", sheetOfPaper);
            eraserResult = eraser.Erase(result, "chuck");
            Assert.AreEqual(eraserResult, "How much wood would a woodchuck chuck if a woodchuck could       wood?");
            eraserResult = eraser.Erase(eraserResult, "chuck");
            Assert.AreEqual(eraserResult, "How much wood would a woodchuck chuck if a wood      could       wood?");
        }
        [Test]
        public void TestEraserDurability()
        {
            Eraser eraser = new Eraser(40000);
            Assert.AreEqual(eraser.durability, 40000);
        }
        [Test]
        public void TestEraserDegredation()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            var result = _pencil.WriteToSheetOfPaper("Testing 123 123", sheetOfPaper);
            var eraserResult = _pencil.eraser.Erase(result, "123");
            Assert.AreEqual(_pencil.eraser.durability, 79997);
        }
        [Test]
        public void TestEraserDegredationWhenDurabilityIsZero()
        {
            _pencil.eraser.durability = 0;
            TestDelegate d = () => _pencil.eraser.Erase("", "");
            Assert.Throws<CannotEraseDurabilityIsZeroException>(d, "", new object[1]);
        }
        [Test]
        public void TestEditText()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            var result = _pencil.Edit("Testing", "123");
            Assert.AreEqual(result, "Testing123");
        }
        [Test]
        public void TestEditTextWithCollisions()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            var result = _pencil.WriteToSheetOfPaper("Testing abc 123 123", sheetOfPaper);
            var eraserResult = _pencil.eraser.Erase(result, "abc");
            Assert.AreEqual(eraserResult, "Testing     123 123");
            var editResult = _pencil.Edit(eraserResult, "artichoke");
            Assert.AreEqual(editResult, "Testing arti@@@k@23");

            sheetOfPaper = _writerUtility.GetASheetOfPaper();
            result = _pencil.WriteToSheetOfPaper("Testing abc 123 123 xyz", sheetOfPaper);
            eraserResult = _pencil.eraser.Erase(result, "abc");
            Assert.AreEqual(eraserResult, "Testing     123 123 xyz");
            editResult = _pencil.Edit(eraserResult, "artichoke");
            Assert.AreEqual(editResult, "Testing arti@@@k@23 xyz");

            sheetOfPaper = _writerUtility.GetASheetOfPaper();
            result = _pencil.WriteToSheetOfPaper("An       a day keeps the doctor away", sheetOfPaper);
            editResult = _pencil.Edit(result, "onion");
            Assert.AreEqual(editResult, "An onion a day keeps the doctor away");

            sheetOfPaper = _writerUtility.GetASheetOfPaper();
            result = _pencil.WriteToSheetOfPaper("An       a day keeps the doctor away", sheetOfPaper);
            editResult = _pencil.Edit(result, "artichoke");
            Assert.AreEqual(editResult, "An artich@k@ay keeps the doctor away");
        }
    }
}