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
			while (true)
			{
				var acceptBtns = driver.FindElements(Locators.AcceptCookiesBtn);

				if (acceptBtns.Count > 0 && acceptBtns[0].Displayed)
				{
					wait.Until(ExpectedConditions.ElementToBeClickable(acceptBtns[0]));
					acceptBtns[0].Click();

					wait.Until(ExpectedConditions.InvisibilityOfElementLocated(Locators.CookieBanner));

					return; 
				}
			}
		}

		public void GoToCareers() => driver.FindElement(Locators.CareersLink).Click();

		public void ClickSearchMagnifier() => wait.Until(ExpectedConditions.ElementToBeClickable(Locators.SearchMagnifierIcon)).Click();
	}
}