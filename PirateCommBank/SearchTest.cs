using System;

using FluentAssertions;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using SeleniumExtras.WaitHelpers;

namespace PirateCommBank
{
    public class SearchTest
    {
        private const string BaseUrl =
            "https://codility-frontend-prod.s3.amazonaws.com/media/task_static/qa_csharp_search/862b0faa506b8487c25a3384cfde8af4/static/attachments/reference_page.html";

        private IWebDriver _driver;

        [SetUp]
        public void BeforeEach()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(BaseUrl);
        }

        [TearDown]
        public void AfterEach()
        {
            _driver.Quit();
        }

        [Test]
        public void test_CheckIfInputAndSearchButtonExists()
        {
            _driver.SearchElement(SearchPage.InputSearchQuery).Displayed.Should().Be(true, "Input search query not visible");
            _driver.SearchElement(SearchPage.ButtonSearch).Displayed.Should().Be(true, "Search button not visible");
        }

        [TestCase("", ExpectedResult = "Provide some query")]
        [TestCase("castle", ExpectedResult = "No results")]
        public string test_SearchIsland(string query)
        {
            _driver.SearchElement(SearchPage.InputSearchQuery).SendKeys(query);
            _driver.SearchElement(SearchPage.ButtonSearch).Click();
            return _driver.SearchElement(SearchPage.ResultDiv).Text;
        }

        [Test]
        public void test_MoreThanOneResult()
        {
            var searchQuery = "isla";
            
            _driver.SearchElement(SearchPage.InputSearchQuery).SendKeys(searchQuery);
            _driver.SearchElement(SearchPage.ButtonSearch).Click();

            _driver.FindElements(SearchPage.SearchResultList).Count.Should().BePositive();

        }

        [Test]
        public void test_OnlyOneResult()
        {
            var searchQuery = "Port Royal";
            
            _driver.SearchElement(SearchPage.InputSearchQuery).SendKeys(searchQuery);
            _driver.SearchElement(SearchPage.ButtonSearch).Click();

            _driver.FindElements(SearchPage.SearchResultList).Count.Should().Be(1);
        }
    }

    public static class WebDriverExtension
    {
        public static IWebElement SearchElement(this IWebDriver driver, By element)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            return wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }
    }

    public static class SearchPage
    {
        public static By InputSearchQuery { get; set; } = By.Id("search-input");
        public static By ButtonSearch { get; set; } = By.Id("search-button");
        public static By SearchResultList { get; set; } = By.Id("search-results");
        public static By OutputContainerDiv { get; set; } = By.Id("output-container");
        public static By ResultDiv { get; set; } = By.CssSelector("#search-results div");
    }
}
