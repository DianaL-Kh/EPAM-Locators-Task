using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using LocatorsWebElementTest.Constants;

namespace LocatorsWebElementTest.Pages
{
	public class EpamHomePage(IWebDriver driver, WebDriverWait wait)
	{
		public void Open()
		{
			driver.Navigate().GoToUrl("https://www.epam.com/");
		}

		public void AcceptCookiesIfPresent()
		{
			var acceptButton = wait.Until(driver =>
			{
				var elements = driver.FindElements(Locators.AcceptCookiesBtn);
				return elements.Count > 0 && elements[0].Displayed
					? elements[0]
					: null;
			});

			if (acceptButton != null)
			{
				acceptButton.Click();

				wait.Until(ExpectedConditions.InvisibilityOfElementLocated(Locators.CookieBanner));
			}
		}

		public void GoToCareers() => driver.FindElement(Locators.CareersLink).Click();

		public void ClickSearchMagnifier() => wait.Until(ExpectedConditions.ElementToBeClickable(Locators.SearchMagnifierIcon)).Click();
	}
}