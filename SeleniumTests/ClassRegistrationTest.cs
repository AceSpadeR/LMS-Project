using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using OpenQA.Selenium.Interactions;

namespace SeleniumTests
{
    [TestClass]
    public class ClassRegistration
    {
        [TestMethod]
        public void StudentRegistration()
        {
            //open chrome
            IWebDriver driver = new ChromeDriver();

            //go to website
            driver.Navigate().GoToUrl("https://lms20240312105559.azurewebsites.net/");

            //test if at login page
            var title = driver.Title;
            Assert.AreEqual("Stellar LMS - Login", driver.Title);

            //sign in as student
            var emailBox = driver.FindElement(By.Id("Email"));
            var passwordBox = driver.FindElement(By.Id("Password"));
            var submitButton = driver.FindElement(By.Id("submitButton"));

            emailBox.SendKeys("student@student.com");
            passwordBox.SendKeys("Student");
            submitButton.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Dashboard", title);

            //Go to classes page
            var classesNavBtn = driver.FindElement(By.CssSelector("a[href='/Classes/Registration']"));
            classesNavBtn.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Your Classes", title);

            //Count the number of classes student has
            var table1 = driver.FindElement(By.CssSelector("tbody"));
            var classesBefore = table1.FindElements(By.XPath("*"));

            //Click register button
            var registerBtn = driver.FindElement(By.CssSelector("a[href='/Classes/Registration/Create']"));
            registerBtn.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Registration", title);

            //Click add class button
            var addBtns = driver.FindElements(By.CssSelector("button.btn.btn-success"));
            //Get last button
            var addBtn = addBtns[addBtns.Count - 1];

            Actions actions = new Actions(driver);
            actions.MoveToElement(addBtn).Perform();
            addBtn.Click();

            //Check to see if class was added and student was taken back to classes page
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Your Classes", title);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            //Click the drop button class
            var dropBtns = driver.FindElements(By.CssSelector("button.btn.btn-danger"));
            var dropBtn = dropBtns[dropBtns.Count - 1];
            actions.MoveToElement(dropBtn).Perform();
            dropBtn.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            //Count the number of classes student has
            var table2 = driver.FindElement(By.CssSelector("tbody"));
            var classesAfter = table2.FindElements(By.XPath("*"));

            //Make sure class count is the same before and after test
            Assert.AreEqual(classesBefore.Count, classesAfter.Count);

            driver.Quit();
        }
    }
}
