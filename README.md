# Selenium WebDriver Test Automation (.NET, NUnit)

This project contains automated UI tests for the EPAM website using Selenium WebDriver and NUnit. The solution follows the Page Object Model pattern and demonstrates basic and advanced WebDriver features.

**Project type:** Class Library

## Technologies used
- C#
- .NET
- Selenium WebDriver
- NUnit

## Implemented requirements

- **Test automation:** Test cases from the previous task are automated using Selenium WebDriver. All interactions with the browser and web elements are implemented through Page Object classes.
- **Browser interactions:** The following WebDriver commands are used:
  - navigation (GoToUrl)
  - click actions
  - sending input (SendKeys)
  - clearing input fields (Clear)
- **Browser capabilities and configuration:** Chrome browser is configured using ChromeOptions:
  - start maximized
  - incognito mode
  - disabled extensions
- **Wait strategies:** Both types of waits are implemented:
  - Implicit wait (global for driver)
  - Explicit wait (WebDriverWait with ExpectedConditions)
- **Window handling:** Browser window is maximized at the start of each test.
- **Data-driven testing:** Tests are parameterized using NUnit TestCase attribute, allowing execution with multiple input values.
- **Parallel test execution:** Tests are executed in parallel with a limit:
  - LevelOfParallelism is set to 3 to control the number of concurrent test executions
- **Driver lifecycle management:** WebDriver is properly initialized in SetUp and always closed in TearDown:
  - `driver.Quit()` is used to close the browser
  - `driver.Dispose()` is used for cleanup
  - `try-catch-finally` ensures driver is closed even if test fails

## Project structure

- **Constants**
  - Contains locators for all elements (Locators class)
- **Pages** (Page Object classes)
  - EpamHomePage
  - CareersPage
  - GlobalSearchPage
- **Tests**
  - BaseTest (setup and teardown logic)
  - LocatorTests (test scenarios)

## Test scenarios

### Job search test
1. Open EPAM homepage
2. Accept cookies
3. Navigate to Careers
4. Search for a programming language
5. Filter by Remote
6. Open the last job from results
7. Verify that the job description contains the searched keyword

### Global search test
1. Open EPAM homepage
2. Use global search
3. Load all results
4. Verify that each result contains the search term

## Additional improvements based on mentor feedback
- Limited parallel test execution to avoid resource overload
- Ensured browser is always closed even if tests fail
- Added browser configuration using ChromeOptions
- Added implicit wait
- Improved stability with explicit waits

## Notes
- Page Object Model is used for better maintainability
- Locators are separated into a dedicated class
- Explicit waits are used instead of `Thread.Sleep`
- Tests are independent and reusable

## Implementation Notes

- **LinkText**
  Exact match of the anchor text. Used specifically to fulfill the assignment requirement for `LinkText`. 
  *Note:* This locator is language-dependent. It is safe here because our test suite assumes the default English (EN) locale execution. In a real project, the CSS selector `[href='/careers']` or a `data-testid` would be preferred.

- **preceding-sibling[1]**
  Total pages are dynamic, so we find the static 'Next' arrow first. Using `preceding-sibling[1]` reliably targets the last numbered button right before it.

- **PartialLinkText**
  Initially, `By.PartialLinkText("View More")` was considered for the Global Search pagination. However, inspecting the DOM revealed that the text is wrapped inside a child `<span>` tag, rather than being the direct text node of the `<a>` tag. This makes standard `LinkText` locators unreliable or non-functional for this specific element. Therefore, `XPath("//span[contains(text(), 'View More')]")` was used to ensure stability.
  
  An attempt was made to use the following locator and check if the driver sees it at all:
  ```csharp
  public static readonly By ViewMoreBtn = By.PartialLinkText("View");
  var viewBtn = driver.FindElements(Locators.ViewMoreBtn);
  TestContext.WriteLine($"button: {viewBtn.Count}");

- **Test Case 2 ("View More" button handling)**
  I considered using if (elements.Count > 0) elements[0].Click();. However, this button always exists in the DOM, even when it is not visible, making elements.Count > 0 unsuitable. I considered the classes themselves; they have a hidden state under certain conditions. I need further guidance on how to properly handle this button's state (hidden vs. visible) to avoid unnecessary synchronization issues.
	
Recommendations for improving the project were considered. However, the condition with PartialLinkText, as indicated above, could not be met. I do not know how to use it more correctly in this project with this site - I need help here.

All comments in the code have been fixed except for one in the BaseTest file - "S2187 - Add some tests to this class.". Due to the fact that BaseTest is a "constant" for other test executable files.

One of the key requirements of the first task was to demonstrate the use of different types of locators. Below is a mapping table:

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
