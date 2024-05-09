namespace CSharpPrintf.Tests;

[TestClass]
public class FormatTest
{
    [TestMethod]
    public void PercentLiteral()
    {
        var formatter = new Formatter();
        Assert.AreEqual(
            "%",
            formatter.Format("%%")
        );

        Assert.AreEqual(
            "This test should pass 100% of the time",
            formatter.Format("This test should pass 100%% of the time")
        );
    }

    [TestMethod]
    public void Decimal()
    {
        var formatter = new Formatter();

        Assert.AreEqual(
            "5",
            formatter.Format("%d", 5)
        );

        Assert.AreEqual(
            "The number is 5",
            formatter.Format("The number is %d", 5)
        );

        Assert.AreEqual(
            "The number is 10",
            formatter.Format("The number is %u", 10)
        );

        Assert.ThrowsException<OverflowException>(() => formatter.Format("%u", -10));

        Assert.AreEqual(
            " 10",
            formatter.Format("% d", 10)
        );
        Assert.AreEqual(
            "+10",
            formatter.Format("% +d", 10)
        );

        Assert.AreEqual(
            "-10",
            formatter.Format("% d", -10)
        );
        Assert.AreEqual(
            "-10",
            formatter.Format("% +d", -10)
        );

        Assert.AreEqual(
            " 0",
            formatter.Format("% d", 0)
        );
        Assert.AreEqual(
            "+0",
            formatter.Format("% +d", 0)
        );
    }

    [TestMethod]
    public void Width()
    {
        var formatter = new Formatter();
        
        Assert.AreEqual(
            "   4",
            formatter.Format("%4d", 4)
        );

        Assert.AreEqual(
            "   5",
            formatter.Format("%*d", 4, 5)
        );

        Assert.AreEqual(
            "1 03 0005",
            formatter.Format("%d %0*d %0*d", 1, 2, 3, 4, 5)
        );
    }

    [TestMethod]
    public void Flags()
    {
        var formatter = new Formatter();
        
        Assert.AreEqual(
            "4   ",
            formatter.Format("%-4d", 4)
        );

        Assert.AreEqual(
            "003",
            formatter.Format("%03d", 3)
        );

        Assert.AreEqual(
            "+1 +0 -1",
            formatter.Format("%+d %+d %+d", 1, 0, -1)
        );
    }

    [TestMethod]
    public void Char()
    {
        var formatter = new Formatter();

        Assert.AreEqual(
            "c",
            formatter.Format("%c", 'c')
        );

        Assert.AreEqual(
            "The char is R",
            formatter.Format("The char is %c", 'R')
        );

        Assert.ThrowsException<InvalidCastException>(() => formatter.Format("%c", 1));
    }

    [TestMethod]
    public void String()
    {
        var formatter = new Formatter();

        Assert.AreEqual(
            "Hello, world!",
            formatter.Format("%s", "Hello, world!")
        );

        Assert.AreEqual(
            "Hello",
            formatter.Format("%.5s", "Hello, world!")
        );
    }

    [TestMethod]
    public void FloatFixed()
    {
        var formatter = new Formatter();

        Assert.AreEqual(
            "1.250000",
            formatter.Format("%f", 1.25f)
        );

        Assert.AreEqual(
            "1.000",
            formatter.Format("%.3f", 1f)
        );

        Assert.AreEqual(
            "00000001.2",
            formatter.Format("%010.1f", 1.25f)
        );
    }

    [TestMethod]
    public void FloatSci()
    {
        var formatter = new Formatter();

        Assert.AreEqual(
            "1.250000e+000",
            formatter.Format("%e", 1.25f)
        );
        Assert.AreEqual(
            "1.250000E+000",
            formatter.Format("%E", 1.25f)
        );

        Assert.AreEqual(
            "1.25e+000",
            formatter.Format("%.2e", 1.25f)
        );
        Assert.AreEqual(
            "1.2e+000",
            formatter.Format("%.1e", 1.25f)
        );

        Assert.AreEqual(
            "2.500000E+001",
            formatter.Format("%E", 25f)
        );
    }

    [TestMethod]
    public void FloatNorm()
    {
        var formatter = new Formatter();

        Assert.AreEqual(
            "25",
            formatter.Format("%g", 25.0f)
        );

        Assert.AreEqual(
            "1.5",
            formatter.Format("%g", 1.5f)
        );

        Assert.AreEqual(
            "1.5",
            formatter.Format("%.2g", 1.5f)
        );

        Assert.AreEqual(
            "2",
            formatter.Format("%.1g", 1.5f)
        );
        Assert.AreEqual(
            "1",
            formatter.Format("%.1g", 1.49f)
        );
    }

    [TestMethod]
    public void Hexadecimal()
    {
        var formatter = new Formatter();

        Assert.AreEqual(
            "abc",
            formatter.Format("%x", 0xABC)
        );
        Assert.AreEqual(
            "ABC",
            formatter.Format("%X", 0xABC)
        );

        Assert.AreEqual(
            "0xabc",
            formatter.Format("%#x", 0xABC)
        );
        Assert.AreEqual(
            "0XABC",
            formatter.Format("%#X", 0xABC)
        );

        Assert.AreEqual(
            "00000abc",
            formatter.Format("%08x", 0xABC)
        );
        Assert.AreEqual(
            "0x000abc",
            formatter.Format("%#08x", 0xABC)
        );
        Assert.AreEqual(
            "   0xabc",
            formatter.Format("%#8x", 0xABC)
        );
        Assert.AreEqual(
            "0xabc   ",
            formatter.Format("%#-8x", 0xABC)
        );
        Assert.AreEqual(
            "0xabc   ",
            formatter.Format("%#-08x", 0xABC)
        );
    }

    [TestMethod]
    public void Octal()
    {
        var formatter = new Formatter();

        Assert.AreEqual(
            "777",
            formatter.Format("%o", 511)
        );

        Assert.AreEqual(
            "000000777",
            formatter.Format("%09o", 511)
        );

        Assert.AreEqual(
            "0777",
            formatter.Format("%#o", 511)
        );

        Assert.AreEqual(
            "  0777",
            formatter.Format("%#6o", 511)
        );
        Assert.AreEqual(
            "000777",
            formatter.Format("%#06o", 511)
        );
    }
}
