using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using YGOProAnalyticsServer;

namespace YGOProAnalyticsServerTests
{
    [TestFixture]
    class AdminConfigTests
    {
        private const string Path = "configTest.json";
        IAdminConfig _adminConfig;

        [SetUp]
        public void Setup()
        {
            _adminConfig = new AdminConfig();
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }

        [Test]
        public async Task LoadConfigFromFile_FileDoesntExists_CreateNewNotEmptyFile()
        {
            await _adminConfig.LoadConfigFromFile(Path);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(File.Exists(Path));
                Assert.IsNotEmpty(File.ReadAllText(Path));
                Assert.AreEqual(JsonConvert.DeserializeObject<AdminConfig>(File.ReadAllText(Path)).MaxNumberOfResultsPerBrowserPage, _adminConfig.MaxNumberOfResultsPerBrowserPage);
            });
        }
    }
}
