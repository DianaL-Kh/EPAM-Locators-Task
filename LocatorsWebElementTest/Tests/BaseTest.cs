using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

[assembly: NUnit.Framework.LevelOfParallelism(3)]

namespace LocatorsWebElementTest.Tests
{
	[TestFixture]
	[Parallelizable(ParallelScope.All)]
	[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
	public class BaseTest
	{
		protected IWebDriver driver;
		protected WebDriverWait wait;
		protected Actions actions;

		[SetUp]
		public void Setup()
		{
			var options = new ChromeOptions();
			options.AddArgument("--start-maximized");
			options.AddArgument("--disable-extensions");
			options.AddArgument("--incognito");

			driver = new ChromeDriver(options);
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
			wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
			actions = new Actions(driver);
		}

		[TearDown]
		public void Teardown()
		{
			try
			{
				driver?.Quit();
			}
			catch (Exception e)
			{
				TestContext.WriteLine($"Error during Quit: {e.Message}");
			}
			finally
			{
				driver?.Dispose();
			}
		}
	}
}