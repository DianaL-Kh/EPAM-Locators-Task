using LocatorsWebElementTest.Pages;

namespace LocatorsWebElementTest.Tests
{
	public class LocatorTests : BaseTest
	{
		[Test]
		[TestCase(".Net")]
		[TestCase("Java")]
		[TestCase("Python")]
		[TestCase("JavaScript")]
		[TestCase("SQL")]
		public void TestCase1_SearchPosition(string programmingLanguage)
		{
			var homePage = new EpamHomePage(driver, wait); 
			var careersPage = new CareersPage(driver, wait, actions);

			homePage.Open();
			homePage.AcceptCookiesIfPresent();
			homePage.GoToCareers();

			careersPage.ClickStartSearch();
			homePage.AcceptCookiesIfPresent();

			careersPage.EnterKeyword(programmingLanguage);
			careersPage.ClearLocationIfSet();
			careersPage.SelectRemote();

			careersPage.SubmitSearchAndWaitForUpdate();
			careersPage.GoToLastPage();
			careersPage.ClickLastJobCard();

			string entirePageText = careersPage.GetEntirePageText();
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
			var homePage = new EpamHomePage(driver, wait);
			var searchPage = new GlobalSearchPage(driver, wait);

			homePage.Open();
			homePage.AcceptCookiesIfPresent();

			homePage.ClickSearchMagnifier();
			searchPage.SearchFor(searchTerm);

			searchPage.LoadAllResults();

			var allTexts = searchPage.GetAllResultTexts();
			TestContext.WriteLine($"Total results loaded: {allTexts.Count}");

			var itemsWithoutTerm = allTexts
				.Where(text => !text.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase))
				.ToList();

			if (itemsWithoutTerm.Count == 0)
			{
				foreach (var item in itemsWithoutTerm)
					TestContext.WriteLine($"\nMissing '{searchTerm}' in: {item}");
			}

			Assert.That(itemsWithoutTerm, Is.Empty,
				$"{itemsWithoutTerm.Count} result(s) do not contain '{searchTerm}'.");
		}
	}
}