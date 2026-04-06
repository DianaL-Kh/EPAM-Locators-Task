using LocatorsWebElementTest.Constants;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace LocatorsWebElementTest.Pages
{
	public class CareersPage(IWebDriver driver, WebDriverWait wait, Actions actions)
	{
		public void ClickStartSearch() => driver.FindElement(Locators.StartSearchBtn).Click();

		public void EnterKeyword(string keyword)
		{
			var keywordField = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.KeywordInput));
			keywordField.SendKeys(keyword);
		}

		public void ClearLocationIfSet()
		{
			var clearIndicators = driver.FindElements(Locators.ClearLocationIcon);
			wait.Until(ExpectedConditions.ElementToBeClickable(clearIndicators[0]));
			if (clearIndicators.Count > 0 && clearIndicators[0].Displayed)
			{
				clearIndicators[0].Click();
			}
		}

		public void SelectRemote()
		{
			var remoteCheckbox = driver.FindElements(Locators.RemoteCheckbox);
			if (remoteCheckbox.Count > 0)
			{
				remoteCheckbox[0].Click();
			}
		}

		public void SubmitSearchAndWaitForUpdate()
		{
			var oldCards = driver.FindElements(Locators.JobCardPanel);
			IWebElement? oldFirstCard = oldCards.Count > 0 ? oldCards[0] : null;

			driver.FindElement(Locators.SubmitSearchBtn).Click();

			if (oldFirstCard != null)
			{
				wait.Until(ExpectedConditions.StalenessOf(oldFirstCard));
			}

			wait.Until(ExpectedConditions.ElementIsVisible(Locators.JobCardPanel));
		}

		public void GoToLastPage()
		{
			driver.FindElement(Locators.PageBody).SendKeys(Keys.End);

			wait.Until(d =>
			{
				var btns = d.FindElements(Locators.LastPageBtn);
				if (btns.Count == 0) return true;

				try
				{
					return btns[0].Displayed;
				}
				catch (StaleElementReferenceException)
				{
					return false;
				}
			});

			var pageButtons = driver.FindElements(Locators.LastPageBtn);

			if (pageButtons.Count > 0 && pageButtons[0].Displayed)
			{
				var lastBtn = pageButtons[0];

				if ((lastBtn.GetAttribute("class") ?? "").Contains("active"))
				{
					return;
				}

				string targetPageNumber = (lastBtn.GetAttribute("data-event-result") ?? "");

				var oldCards = driver.FindElements(Locators.LastJobCardLink);
				IWebElement? oldCard = oldCards.Count > 0 ? oldCards[0] : null;

				actions.MoveToElement(lastBtn).Perform();
				wait.Until(ExpectedConditions.ElementToBeClickable(lastBtn));
				lastBtn.SendKeys(Keys.Enter);

				wait.Until(d =>
				{
					var targetBtns = d.FindElements(By.XPath($"//button[@data-event-result='{targetPageNumber}']"));
					return targetBtns.Count > 0 && (targetBtns[0].GetAttribute("class") ?? "").Contains("active");
				});

				if (oldCard != null)
				{
					wait.Until(ExpectedConditions.StalenessOf(oldCard));
				}
			}
		}

		public void ClickLastJobCard()
		{
			wait.Until(ExpectedConditions.ElementIsVisible(Locators.JobCardPanel));

			var jobCards = driver.FindElements(Locators.LastJobCardLink);

			if (jobCards.Count > 0 && jobCards[0].Displayed)
			{
				var lastJobCard = jobCards[0];

				actions.MoveToElement(lastJobCard).Perform();

				lastJobCard.Click();
			}

			wait.Until(ExpectedConditions.UrlContains("/vacancy/"));
			wait.Until(ExpectedConditions.ElementIsVisible(Locators.FooterContainer));
		}

		public string GetEntirePageText() => driver.FindElement(Locators.PageBody).Text;
	}
}