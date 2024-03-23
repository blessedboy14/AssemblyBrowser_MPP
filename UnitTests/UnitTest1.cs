using AssemblyAnalyzer;

namespace UnitTests
{
    public class Tests
    {
        public string assemblyPath;
        public string currentPath;
        [SetUp]
        public void Setup()
        {
            assemblyPath = "D:\\LABS\\SPP\\Faker\\Faker\\bin\\Debug\\net8.0\\Faker.dll";
            currentPath = "D:\\LABS\\SPP\\AssemblyBrowser\\UnitTests\\bin\\Debug\\net8.0\\UnitTests.dll";
        }

        [Test]
        public void TestBasicCase_WithCorrectOutput()
        {
            AnalyzerResult res = null;
            try
            {
                res = Analyzer.Analyze(assemblyPath);
            } catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.IsTrue(res.Namespaces.Count > 0);
            Assert.IsTrue(res.AssemblyName.Contains("Faker"));
            Assert.IsTrue(res.NamespacesDictionary.ContainsKey("Faker"));
        }

        [Test]
        public void TestBasicCase_CheckEmptyNamespace()
        {
            AnalyzerResult res = null;
            try
            {
                res = Analyzer.Analyze(assemblyPath);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.IsTrue(res.NamespacesDictionary.ContainsKey("Not presented"));
            Assert.IsTrue(res.NamespacesDictionary["Not presented"].typeInfos.Count > 0);
        }

        [Test]
        public void TestResult_FromIncorrectAssemblyFile()
        {
            AnalyzerResult res = null;
            try
            {
                res = Analyzer.Analyze("bad_path.dll");
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
        }

        [Test]
        public void TestResult_FromCurrentAssembly()
        {
            AnalyzerResult res = null;
            try
            {
                res = Analyzer.Analyze(currentPath);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.IsTrue(res.NamespacesDictionary.ContainsKey(typeof(Tests).Namespace));
            Assert.IsTrue(res.NamespacesDictionary[typeof(Tests).Namespace].typeInfos
                .First().methods
                .Find(p => p.Name == "TestResult_FromCurrentAssembly") != null);
        }

        [Test]
        public void TestResult_CheckContainingBasicMethods()
        {
            string[] basicMethods = {"GetHashCode", "Equals", "ToString", "GetType"};
            AnalyzerResult res = null;
            try
            {
                res = Analyzer.Analyze(currentPath);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.AreEqual(res.NamespacesDictionary[typeof(Tests).Namespace].typeInfos
                .First().methods.FindAll(p => basicMethods.Contains(p.Name)).Count(), 4);
        }
    }
}