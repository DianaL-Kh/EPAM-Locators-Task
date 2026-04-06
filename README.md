
The suite includes two main Test Cases:
1. TestCase1_SearchPosition: Validates job search functionality, handles pagination (moving to the last page), and verifies content on specific vacancy pages.
2. TestCase2_ValidateGlobalSearch: Tests the global site search using the magnifier icon and validates all results using LINQ queries to ensure high data consistency.

About LinkText:
	LinkText: Exact match of the anchor text. Used specifically to fulfill the assignment requirement for 'LinkText'.
	Note: This locator is language-dependent. It is safe here because our test suite assumes the default English (EN) locale execution. In a real project, the CSS selector [href='/careers'] or a data-testid would be preferred I think.
	
About preceing-sibling[1]:
	Total pages are dynamic, so we find the static 'Next' arrow first. Using preceding-sibling[1] reliably targets the last numbered button right before it.
	
About PartialLinkText:
	Initially, By.PartialLinkText("View More") was considered for the Global Search pagination.
	However, inspecting the DOM revealed that the text is wrapped inside a child <span> tag, rather than being the direct text node of the <a> tag. This makes standard LinkText  locators unreliable or non-functional for this specific element. Therefore, XPath("//span[contains(text(), 'View More')]"); was used for "View More" to ensure stability.

  There was an attempt to use a locator: public static readonly By ViewMoreBtn = By.PartialLinkText("View");
	I even just tried to find out if the driver sees this element at all.
	var viewBtn = driver.FindElements(Locators.ViewMoreBtn);
	TestContext.WriteLine($"button: {viewBtn.Count}");

About test case 2:
	if (elements.Count > 0) elements[0].Click(); - I considered this point, regarding the "View More" button, in the second test case.
	This button always exists, even when it is not visible, so elements.Count > 0 is not suitable.
	I considered the classes themselves, they have the inscription "hidden", under certain conditions.
	I do not know how to better process this button here, so that there is no unnecessary asynchrony - in the "hidden" states and without it.
	
Recommendations for improving the project were considered. However, the condition with PartialLinkText, as indicated above, could not be met. I do not know how to use it more correctly in this project with this site - I need help here.

All comments in the code have been fixed except for one in the BaseTest file - "S2187 - Add some tests to this class.". Due to the fact that BaseTest is a "constant" for other test executable files.

One of the key requirements was to demonstrate usage of various locator types. Below is the mapping table:

| Locator Type | Value |
| :--- | :--- |
| ID | `"onetrust-banner-sdk", "new_form_search"` |
| Name | `"search"` |
| ClassName | `"preloader"` |
| TagName | `"body"` |
| LinkText | `"Careers"` |
| PartialLinkText | `View - ?` |
| CSS (pseudo-class) | `"div[class*='JobCard_panel']:last-child a[data-testid='job-card-link']"` |
| Relative XPath | `"//label[contains(., 'Remote')]", "//button[contains(@class, 'custom-search-button')]", "//span[contains(text(), 'View More')]"` |
| XPath (operator) | `"//button[span[@class='is-a11y-only' and text()='Search']]", "//div[contains(@class, 'control') and .//input[@aria-label='Choose your country']]//div[contains(@class, 'clear-indicator')]"` |
| XPath (axes) | `"//button[@aria-label='next page']/preceding-sibling::button[1]"` |
