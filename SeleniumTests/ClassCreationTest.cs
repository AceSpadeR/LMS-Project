using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace SeleniumTests
{
    [TestClass]
    public class ClassCreation
    {
        [TestMethod]
        public void TeacherClassCreationTest()
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

            emailBox.SendKeys("teacher@teacher.com");
            passwordBox.SendKeys("Teacher");
            submitButton.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Dashboard", title);

            // Navigate to Classes Page
            var classesNavBtn = driver.FindElement(By.CssSelector("a[href='/Classes']"));
            classesNavBtn.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Manage Classes", title);

            // Click Create Class Button
            var createClassBtn = driver.FindElement(By.CssSelector("a[href='/Classes/Create']"));
            createClassBtn.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Create Class", title);

            // Enter Data
            var departmentBox = driver.FindElement(By.Id("Classes_DepartmentName"));
            var courseNumBox = driver.FindElement(By.Id("Classes_CourseNumber"));
            var creditHrsBox = driver.FindElement(By.Id("Classes_CreditHours"));
            var titleBox = driver.FindElement(By.Id("Classes_Title"));
            var locationBox = driver.FindElement(By.Id("Classes_Location"));
            var startTimeBox = driver.FindElement(By.Id("Classes_StartTime"));
            var endTimeBox = driver.FindElement(By.Id("Classes_EndTime"));
            var createBtn = driver.FindElement(By.CssSelector("button.btn.btn-success.w-100"));

            SelectElement selectDepartment = new SelectElement(departmentBox);
            selectDepartment.SelectByText("Computer Science");
            courseNumBox.SendKeys("CS 1000");
            creditHrsBox.SendKeys("4");
            titleBox.SendKeys("Intro to Computer Science");
            locationBox.SendKeys("Noorda 200");
            startTimeBox.SendKeys("1030am");
            endTimeBox.SendKeys("1220pm");
            Actions actions = new Actions(driver);
            actions.MoveToElement(createBtn).Perform();
            createBtn.Click();

            // Click Create Class button
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Manage Classes", title);

            // Click delete class
            var deleteClassBtn = driver.FindElement(By.CssSelector("a.btn.btn-danger.me-0.ms-1:last-of-type"));
            actions.MoveToElement(deleteClassBtn).Perform();
            deleteClassBtn.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Delete Class", title);

            // Click confirm delete 
            var confirmDeleteClassBtn = driver.FindElement(By.CssSelector("button.btn.btn-danger.me-0.ms-1"));
            confirmDeleteClassBtn.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Manage Classes", title);

            driver.Quit();
        }

    }
}
