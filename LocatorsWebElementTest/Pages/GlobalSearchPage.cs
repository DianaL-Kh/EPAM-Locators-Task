using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using LocatorsWebElementTest.Constants;

namespace LocatorsWebElementTest.Pages
{
	public class GlobalSearchPage(IWebDriver driver, WebDriverWait wait)
	{
		public void SearchFor(string term)
		{
			var searchInput = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.GlobalSearchInput));
			searchInput.Clear();
			searchInput.SendKeys(term);

			var findButton = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.GlobalFindBtn));
			findButton.Click();

			wait.Until(ExpectedConditions.ElementIsVisible(Locators.SearchResultsContainer)); 
		}

		public void LoadAllResults()
		{
			while (true)
			{
				driver.FindElement(Locators.PageBody).SendKeys(Keys.End);
				var preloaderStatus = driver.FindElement(Locators.GlobalSearchPreloader);
				var spanElement = driver.FindElement(Locators.SpanViewMore);
				var viewMoreBtn = spanElement.FindElement(By.XPath(".."));

				if (!(viewMoreBtn.GetAttribute("class") ?? "").Contains("hidden"))
				{
					wait.Until(ExpectedConditions.ElementToBeClickable(viewMoreBtn)).SendKeys(Keys.Enter);
				}
				if ((viewMoreBtn.GetAttribute("class") ?? "").Contains("hidden") && (preloaderStatus.GetAttribute("class") ?? "").Contains("hidden"))
				{
					break;
				}
			}
		}

		public List<string> GetAllResultTexts()
		{
			var resultItems = driver.FindElements(Locators.SearchResultItem);
			return [.. resultItems.Select(item => item.Text)];
		}
	}
}