using NUnit.Framework;
using System.IO;

[TestFixture]
public class TestDataSmoke
{
    [Test]
    public void TestFile_IsCopiedToOutput()
    {
        string path = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "testfile.txt");
        Assert.That(File.Exists(path), $"Missing: {path}");
    }
}
