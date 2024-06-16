using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests
{
    [TestClass]
    public class AssignmentCreation
    {
        [TestMethod]
        public void AddAssginment()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://lms20240312105559.azurewebsites.net/");

            Assert.AreEqual("Stellar LMS - Login", driver.Title);

            driver.FindElement(By.Id("Email")).SendKeys("teacher@teacher.com");
            driver.FindElement(By.Id("Password")).SendKeys("Teacher");
            driver.FindElement(By.Id("submitButton")).Click();

            // Find all the cards
            var cards = driver.FindElements(By.CssSelector(".card.border-primary.my-3.mx-3"));

            if (cards.Count > 0)
            {
                // Select the first card
                var firstCard = cards[0];

                // Find the link inside the card and click it
                var linkInFirstCard = firstCard.FindElement(By.CssSelector("a"));
                linkInFirstCard.Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var createAssignmentButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("createAssignmentButton")));
                createAssignmentButton.Click();



                driver.FindElement(By.Id("Assignment_Title")).SendKeys("Test Assignment");

                var dueDateElement = driver.FindElement(By.Id("Assignment_DueDate"));
                dueDateElement.Clear();
                dueDateElement.SendKeys("04/30/2024");
                dueDateElement.SendKeys(Keys.Tab);
                dueDateElement.SendKeys("00:00AM");



                driver.FindElement(By.Id("Assignment_MaxPoints")).SendKeys("100");

                driver.SwitchTo().Frame(driver.FindElement(By.Id("TextareaEnabled_ifr")));

                // Now you can access the element
                driver.FindElement(By.CssSelector("#tinymce > p")).SendKeys("Test Assignment");

                driver.SwitchTo().DefaultContent();

                var button = driver.FindElement(By.XPath("//button[@onclick='return validateDescription()']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", button);



                // Locate the assignment row by its title
                var assignmentRow = driver.FindElement(By.XPath("//td[contains(text(), 'Test Assignment')]//ancestor::tr"));

                // Within the assignment row, locate the delete button using its class name
                var deleteButton = assignmentRow.FindElement(By.CssSelector(".btn.btn-danger.me-0.ms-1"));

                // Click the delete button
                deleteButton.Click();

                driver.FindElement(By.CssSelector("button[type='submit'].btn.btn-danger.me-0.ms-1"));

                IWebElement deleteButtonTwo = driver.FindElement(By.CssSelector("button.btn.btn-danger"));
                deleteButtonTwo.Click();

                Assert.AreEqual("Stellar LMS - Class Details", driver.Title);
                var assignmentRows = driver.FindElements(By.XPath("//td[contains(text(), 'Test Assignment')]//ancestor::tr"));
                Assert.AreEqual(0, assignmentRows.Count, "Test Assignment has not been deleted.");


            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            driver.Quit();
        }
    }
}

