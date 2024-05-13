namespace CSharpPrintf.Tests;

[TestClass]
public class InvalidFormatTest
{
    [TestMethod]
    public void Truncated()
    {
        var formatter = new Formatter();

        Assert.AreEqual("", formatter.Format("%"));
        Assert.AreEqual("", formatter.Format("%-+ 0#"));
        Assert.AreEqual("", formatter.Format("%-+ 0#5"));
        Assert.AreEqual("", formatter.Format("%-+ 0#.5"));
        Assert.AreEqual("", formatter.Format("%-+ 0#5.5"));
        Assert.AreEqual(",", formatter.Format("%-+ 0#5,"));
        Assert.AreEqual(",", formatter.Format("%-+ 0#.5,"));
        Assert.AreEqual(",", formatter.Format("%-+ 0#5.5,"));
        Assert.AreEqual(",", formatter.Format("%.2,"));
        Assert.AreEqual(",", formatter.Format("%2,"));
        Assert.AreEqual(",", formatter.Format("%2.2,"));
    }

    [TestMethod]
    public void InvalidType()
    {
        var formatter = new Formatter();

        Assert.AreEqual("b", formatter.Format("%0b"));
    }
}