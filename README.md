The suite includes two main Test Cases:
1. TestCase1_SearchPosition: Validates job search functionality, handles pagination (moving to the last page), and verifies content on specific vacancy pages.
2. TestCase2_ValidateGlobalSearch: Tests the global site search using the magnifier icon and validates all results using LINQ queries to ensure high data consistency.

One of the key requirements was to demonstrate usage of various locator types. Below is the mapping table:

| Element | Locator Type | Value | Case |
| :--- | :--- | :--- | :--- |
| Accept Cookies Button | ID | `onetrust-accept-btn-handler` | TC1/2 |
| Keywords Input | Name | `search` | TC1 |
| Clear Filter Icon | ClassName | `dropdown__clear-indicator` | TC1 |
| Page Body (End key) | TagName | `body` | TC1/2 |
| Careers Menu | LinkText | `Careers` | TC1 |
| View More Button | PartialLinkText | `View More` | TC2 |
| Start Search Button | CSS (pseudo-class) | `.button-text-wrapper > span:not(:empty)` | TC1 |
| Remote Filter | Relative XPath | `//label[contains(., 'Remote')]` | TC1 |
| Find Button | XPath (operator) | `//button[contains(@class, 'custom-search-button')]` | TC2 |
| Last Page Button | XPath (axes) | `//button[@aria-label='next page']/preceding-sibling::button[1]` | TC1 |
