using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.VisualStudio;
namespace Tetris.Test
{
    [TestFixture]
    public class MatrixTests
    {
        [TestCase(0,0)]
        [TestCase(1,1)]
        [TestCase(2,2)]
        [TestCase(3,4)]
        [TestCase(4,8)]
        public void TestPow(int num, int result)
        {
            var matrix = new Matrix(0, 0);
            Assert.AreEqual(result, matrix.GetFactor(num));
        }

        [Test]
        public void doTest()
        {
            Assert.Pass();
        }
    }
}
