using NUnit.Framework;
using PencilDurability.Pencil;

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