using OpenQA.Selenium;

namespace LocatorsWebElementTest.Constants
{
	public static class Locators
	{
		public static readonly By AcceptCookiesBtn = By.CssSelector("button[id*='accept-btn']");
		public static readonly By CookieBanner = By.Id("onetrust-banner-sdk");
		public static readonly By PageBody = By.TagName("body");
		public static readonly By FooterContainer = By.CssSelector("[data-testid='footer-container']");

		public static readonly By CareersLink = By.LinkText("Careers");
		public static readonly By SearchMagnifierIcon = By.XPath("//button[span[@class='is-a11y-only' and text()='Search']]");

		public static readonly By StartSearchBtn = By.CssSelector("a[href*='start-your-search-here']");
		public static readonly By KeywordInput = By.Name("search");
		public static readonly By ClearLocationIcon = By.XPath("//div[contains(@class, 'control') and .//input[@aria-label='Choose your country']]//div[contains(@class, 'clear-indicator')]"); 
		public static readonly By RemoteCheckbox = By.XPath("//label[contains(., 'Remote')]");
		public static readonly By SubmitSearchBtn = By.CssSelector("button[data-event-label='search']:not([disabled])");
		public static readonly By JobCardPanel = By.CssSelector("[data-testid='job-card-panel-container']");
		public static readonly By LastPageBtn = By.XPath("//button[@aria-label='next page']/preceding-sibling::button[1]");
		public static readonly By LastJobCardLink = By.CssSelector("div[class*='JobCard_panel']:last-child a[data-testid='job-card-link']");
		public static readonly By CareersPreloader = By.CssSelector("[data-testid='preloader']");

		public static readonly By GlobalSearchInput = By.Id("new_form_search");
		public static readonly By GlobalFindBtn = By.XPath("//button[contains(@class, 'custom-search-button')]");
		public static readonly By SearchResultsContainer = By.CssSelector("div.search-results__items");
		public static readonly By SearchResultItem = By.CssSelector("article.search-results__item");
		
		public static readonly By SpanViewMore = By.XPath("//span[contains(text(), 'View More')]");
		public static readonly By GlobalSearchPreloader = By.ClassName("preloader");
		public static readonly By ViewMoreButtonTextCss = By.CssSelector("a.search-results__view-more");
	}
}