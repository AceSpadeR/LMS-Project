using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    [TestClass]
    public class SignIn
    {
        [TestMethod]
        public void StudentSignIn()
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

            driver.Quit();
        }

    }
}
