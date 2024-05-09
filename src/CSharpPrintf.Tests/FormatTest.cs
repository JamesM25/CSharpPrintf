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
}
