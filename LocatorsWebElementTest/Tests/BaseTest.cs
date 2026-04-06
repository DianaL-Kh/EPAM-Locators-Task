using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace LocatorsWebElementTest.Tests
{
	[TestFixture]
	[Parallelizable(ParallelScope.All)]
	public class BaseTest
	{
		protected IWebDriver driver;
		protected WebDriverWait wait;
		protected Actions actions;

		[SetUp]
		public void Setup()
		{
			driver = new ChromeDriver(); 
			driver.Manage().Window.Maximize();
			wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
			actions = new Actions(driver);
		}

		[TearDown]
		public void Teardown()
		{
			driver?.Quit();
			driver?.Dispose();
		}
	}
}