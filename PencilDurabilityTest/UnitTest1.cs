using NUnit.Framework;
using PencilDurability.Pencils;
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
            Assert.AreEqual(_pencil.Point, 40000);
        }
        [Test]
        public void TestPointDegredation()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(_pencil.Point, 39995);

            _pencil.WriteToSheetOfPaper("Tes ", sheetOfPaper);
            Assert.AreEqual(_pencil.Point, 39991);
        }
        [Test]
        public void TestPointDegredationWhenPointReachesZero()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.Point = 3;
            TestDelegate d = () => _pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.Throws<PointHasDegradedToZeroException>(d, "", new object[1]);
        }
        [Test]
        public void TestPencilLength()
        {
            _pencil.Length = 11;
            Assert.AreEqual(_pencil.Length, 11);
        }
        [Test]
        public void TestSharpenPencil()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(_pencil.Point, 39995);

            _pencil.Sharpen(40000);
            Assert.AreEqual(_pencil.Point, 40000);
        }
        [Test]
        public void TestSharpenPencilReducesLength()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.WriteToSheetOfPaper("Test", sheetOfPaper);
            Assert.AreEqual(_pencil.Point, 39995);

            _pencil.Sharpen(40000);
            Assert.AreEqual(_pencil.Point, 40000);
            Assert.AreEqual(_pencil.Length, 9);
        }
        [Test]
        public void TestPencilLengthZeroProhibitsSharpening()
        {
            _pencil.Length = 0;
            TestDelegate d = ()=>_pencil.Sharpen(40000);
            Assert.Throws<CannotSharpenPencilLengthZeroException>(d, "", new object[1]);
        }
        [Test]
        public void TestEraseLastOccurrenceOfText()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.WriteToSheetOfPaper("Testing 123 123", sheetOfPaper);

            Eraser eraser = new Eraser(40000);
            var eraserResult = eraser.Erase(sheetOfPaper.Text, "123");
            Assert.AreEqual(eraserResult, "Testing 123    ");

            sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.WriteToSheetOfPaper("How much wood would a woodchuck chuck if a woodchuck could chuck wood?", sheetOfPaper);
            eraserResult = eraser.Erase(sheetOfPaper.Text, "chuck");
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
            _pencil.WriteToSheetOfPaper("Testing 123 123", sheetOfPaper);
            var eraserResult = _pencil.Eraser.Erase(sheetOfPaper.Text, "123");
            Assert.AreEqual(_pencil.Eraser.durability, 79997);
        }
        [Test]
        public void TestEraserDegradesToZeroWhileErasing()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.WriteToSheetOfPaper("Testing 123 123", sheetOfPaper);
            _pencil.Eraser.durability = 2;
            var eraserResult = _pencil.Eraser.Erase(sheetOfPaper.Text, "123");
            Assert.AreEqual(eraserResult, "Testing 123 1  ");

            sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.Eraser.durability = 3;
            _pencil.WriteToSheetOfPaper("Buffalo Bill", sheetOfPaper);
            sheetOfPaper.Text = _pencil.Eraser.Erase(sheetOfPaper.Text, "Bill");
            Assert.AreEqual(sheetOfPaper.Text, "Buffalo B   ");        }
        [Test]
        public void TestEraserDegredationWhenDurabilityIsZero()
        {
            _pencil.Eraser.durability = 0;
            TestDelegate d = () => _pencil.Eraser.Erase("", "");
            Assert.Throws<CannotEraseDurabilityIsZeroException>(d, "", new object[1]);
        }
        [Test]
        public void TestEditText()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.WriteToSheetOfPaper("Testing", sheetOfPaper);
            _pencil.Edit(sheetOfPaper, "123");
            Assert.AreEqual(sheetOfPaper.Text, "Testing123");
        }
        [Test]
        public void TestEditTextWithCollisions()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.WriteToSheetOfPaper("Testing abc 123 123", sheetOfPaper);
            sheetOfPaper.Text = _pencil.Eraser.Erase(sheetOfPaper.Text, "abc");
            Assert.AreEqual(sheetOfPaper.Text, "Testing     123 123");
            _pencil.Edit(sheetOfPaper, "artichoke");
            Assert.AreEqual(sheetOfPaper.Text, "Testing arti@@@k@23");

            sheetOfPaper = _writerUtility.GetASheetOfPaper();
            sheetOfPaper.Text = _pencil.WriteToSheetOfPaper("Testing abc 123 123 xyz", sheetOfPaper);
            sheetOfPaper.Text = _pencil.Eraser.Erase(sheetOfPaper.Text, "abc");
            Assert.AreEqual(sheetOfPaper.Text, "Testing     123 123 xyz");
            _pencil.Edit(sheetOfPaper, "artichoke");
            Assert.AreEqual(sheetOfPaper.Text, "Testing arti@@@k@23 xyz");

            sheetOfPaper = _writerUtility.GetASheetOfPaper();
            sheetOfPaper.Text = _pencil.WriteToSheetOfPaper("An       a day keeps the doctor away", sheetOfPaper);
            _pencil.Edit(sheetOfPaper, "onion");
            Assert.AreEqual(sheetOfPaper.Text, "An onion a day keeps the doctor away");

            sheetOfPaper = _writerUtility.GetASheetOfPaper();
            sheetOfPaper.Text = _pencil.WriteToSheetOfPaper("An       a day keeps the doctor away", sheetOfPaper);
            _pencil.Edit(sheetOfPaper, "artichoke");
            Assert.AreEqual(sheetOfPaper.Text, "An artich@k@ay keeps the doctor away");
        }
        [Test]
        public void TestEditTextWhenPointDegradationReachesZeroDuringTheEdit()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            sheetOfPaper.Text = _pencil.WriteToSheetOfPaper("An       a day keeps the doctor away", sheetOfPaper);
            _pencil.Point = 3;
            TestDelegate d = () => _pencil.Edit(sheetOfPaper, "artichoke");
            Assert.Throws<PointHasDegradedToZeroException>(d, "", new object[1]);
            Assert.AreEqual(sheetOfPaper.Text, "An art      ay keeps the doctor away");
        }
        [Test]
        public void TestWriteTextWhenPointDegradationReachesZeroDuringTheEdit()
        {
            var sheetOfPaper = _writerUtility.GetASheetOfPaper();
            _pencil.Point = 26;
            TestDelegate d = () => _pencil.WriteToSheetOfPaper("An apple a day keeps the doctor away", sheetOfPaper);
            Assert.Throws<PointHasDegradedToZeroException>(d, "", new object[1]);
            Assert.AreEqual(sheetOfPaper.Text, "An apple a day keeps the doctor     ");
        }
    }
}