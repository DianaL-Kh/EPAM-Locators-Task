using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace LocatorsWebElementTest.Tests
{
	[TestFixture]
	public class LocatorTests
	{
		private IWebDriver driver;
		private WebDriverWait wait;
		private Actions actions;

		[SetUp]
		public void Setup()
		{
			driver = new ChromeDriver();
			driver.Manage().Window.Maximize();
			wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
			actions = new Actions(driver);
		}

		[Test]
		[TestCase(".Net")]
		[TestCase("Java")]
		[TestCase("Python")]
		[TestCase("JavaScript")]
		[TestCase("SQL")]
		public void TestCase1_SearchPosition(string programmingLanguage)
		{
			var waits = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
			Actions actions = new Actions(driver);

			driver.Navigate().GoToUrl("https://www.epam.com/");

			AcceptCookiesIfPresent();

			driver.FindElement(By.LinkText("Careers")).Click();

			var startSearchBtn = driver.FindElement(By.CssSelector(".button-text-wrapper > span:not(:empty)"));
			startSearchBtn.Click();

			AcceptCookiesIfPresent();

			var keywordField = waits.Until(ExpectedConditions.ElementToBeClickable(By.Name("search")));
			keywordField.SendKeys(programmingLanguage);

			try
			{
				driver.FindElement(By.ClassName("dropdown__clear-indicator")).Click();
			}
			catch (NoSuchElementException) { }

			var oldCards = driver.FindElements(By.XPath("//div[contains(@class,'JobCard_panel')]"));
			IWebElement oldFirstCard = oldCards.Count > 0 ? oldCards[0] : null;

			driver.FindElement(By.XPath("//button[@type='submit' and contains(@class, 'SearchBox_button')]")).Click();
			WaitForStaleness(oldFirstCard);

			waits.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'JobCard_panel')]")));

			driver.FindElement(By.TagName("body")).SendKeys(Keys.End);
			var currentCards = driver.FindElements(By.XPath("//div[contains(@class,'JobCard_panel')]"));
			IWebElement firstCardOnCurrentPage = currentCards.Count > 0 ? currentCards[0] : null;

			WaitAndPerformAction(
				By.XPath("//button[@aria-label='next page']/preceding-sibling::button[1]"),
				element => element.SendKeys(Keys.Enter)
			);

			WaitForStaleness(firstCardOnCurrentPage);

			waits.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'JobCard_panel')]")));
			WaitAndPerformAction(
				By.XPath("(//a[@data-testid='job-card-link'])[last()]"),
				element => element.Click()
			);

			waits.Until(ExpectedConditions.UrlContains("/vacancy/"));
			waits.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-testid='footer-container']")));
			string entirePageText = driver.FindElement(By.TagName("body")).Text;
			TestContext.WriteLine($"Current status: {driver.Url}");

			Assert.That(entirePageText, Does.Contain(programmingLanguage).IgnoreCase,
				$"Text '{programmingLanguage}' not found on job page. URL: {driver.Url}");
		}

		[Test]
		[TestCase("BLOCKCHAIN")]
		[TestCase("Cloud")]
		[TestCase("Automation")]
		public void TestCase2_ValidateGlobalSearch(string searchTerm)
		{
			var waits = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
			Actions actions = new Actions(driver);

			driver.Navigate().GoToUrl("https://www.epam.com/");

			AcceptCookiesIfPresent();

			// Click the magnifier/search icon
			var searchIcon = waits.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.header-search__button")));
			searchIcon.Click();

			// Type the search term
			var searchInput = waits.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input#new_form_search")));
			searchInput.Clear();
			searchInput.SendKeys(searchTerm);

			// Click the "Find" button
			var findButton = waits.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.custom-search-button")));
			findButton.Click();

			// Wait for results to load
			waits.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.search-results__items")));

			//TestContext.WriteLine($"Current time: {DateTime.Now}");
			// Click "View More" until it disappears (load all results)
			while (true)
			{
				try
				{
					// Scroll to bottom of page
					driver.FindElement(By.TagName("body")).SendKeys(Keys.End);

					if (driver.FindElements(By.CssSelector("a.search-results__view-more")).Count > 0 && driver.FindElements(By.CssSelector("a.search-results__view-more button-text")).Count == 0)
					{
						IWebElement childElement = driver.FindElement(By.CssSelector("a.search-results__view-more"));
						TestContext.WriteLine($"child1 element tag name: {childElement.GetAttribute("class")}");

						waits.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.search-results__view-more"))).SendKeys(Keys.Enter);
						TestContext.WriteLine($"child1-2 element tag name: {childElement.GetAttribute("class")}");

					}

					/*
					var viewMoreBtn = waits.Until(ExpectedConditions.ElementToBeClickable(
						By.CssSelector("a.search-results__view-more")));
					*/

					//actions.MoveToElement(viewMoreBtn).Click().Perform();
					//viewMoreBtn.Click();
				}
				catch (ArgumentNullException)
				{
					TestContext.WriteLine($"ArgumentNullException: {DateTime.Now}");
					break;
				}
				/*catch (NoSuchElementException)
				{
					TestContext.WriteLine($"NoSuchElementException: {DateTime.Now}");
					break;
				}*/
				catch (WebDriverTimeoutException)
				{
					TestContext.WriteLine($"WebDriverTimeoutException: {DateTime.Now}");
					break;
				}
			}
			//TestContext.WriteLine($"Current time: {DateTime.Now}");

			// Collect all result articles (title + description combined text)
			var resultItems = driver.FindElements(By.CssSelector("article.search-results__item"));

			// Use LINQ to collect all texts
			var allTexts = resultItems
				.Select(item => item.Text)
				.ToList();

			TestContext.WriteLine($"Total results loaded: {allTexts.Count}");

			// Use LINQ to find items that do NOT contain the search term
			var itemsWithoutTerm = allTexts
				.Where(text => !text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
				.ToList();

			// Log any failing items for diagnostics
			if (itemsWithoutTerm.Any())
			{
				foreach (var item in itemsWithoutTerm)
					TestContext.WriteLine($"\nMissing '{searchTerm}' in: {item}");
			}

			// Assert all results contain the search term
			Assert.That(itemsWithoutTerm, Is.Empty,
				$"{itemsWithoutTerm.Count} result(s) do not contain '{searchTerm}'.");
		}

		private void AcceptCookiesIfPresent()
		{
			try
			{
				var acceptBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("onetrust-accept-btn-handler")));
				acceptBtn.SendKeys(Keys.Enter);

				wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("onetrust-banner-sdk")));
			}
			catch (WebDriverTimeoutException) { }
		}

		private void WaitAndPerformAction(By locator, Action<IWebElement> action)
		{
			wait.Until(d =>
			{
				try
				{
					var element = d.FindElement(locator);

					if (element.Displayed && element.Enabled)
					{
						actions.MoveToElement(element).Perform();
						action(element);
						return true;
					}
					return false;
				}
				catch (StaleElementReferenceException) { return false; }
				catch (NoSuchElementException) { return false; }
				catch (ElementClickInterceptedException) { return false; }
			});
		}

		private void WaitForStaleness(IWebElement element)
		{
			if (element != null)
			{
				try
				{
					wait.Until(ExpectedConditions.StalenessOf(element));
				}
				catch (WebDriverTimeoutException) { }
			}
		}

		[TearDown]
		public void Teardown()
		{
			driver.Quit();
			driver.Dispose();
		}
	}
}