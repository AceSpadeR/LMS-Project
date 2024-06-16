using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SeleniumTests
{
    [TestClass]
    public class AddUser
    {
        [TestMethod]
        public void AddStudent()
        {
            //open chrome
            IWebDriver driver = new ChromeDriver();

            //go to website
            driver.Navigate().GoToUrl("https://lms20240312105559.azurewebsites.net");

            //test if at login page
            var title = driver.Title;
            Assert.AreEqual("Stellar LMS - Login", driver.Title);

            //click on Create an Account
            var CreateBtn = driver.FindElement(By.CssSelector("div.text-center>a")); //PartialLinkText("/Account/SignUp"));
            CreateBtn.Click();

            //test if at Sign-up page
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Sign-Up", title);

            //fill out form
            //data
            string firstName = "Student";
            string lastName = "Test";
            //generate statistically unique email
            int timesting = (int)(DateTime.Now.Ticks % 10000);
            Random rnd = new Random();
            int randomInt = rnd.Next(0, 1000);
            string email = "TestStudent" + timesting + randomInt + "@fake.com";
            //data
            string birthDate = "07201990";
            string password = "StudentTest876";

            //elements
            var firstBox = driver.FindElement(By.Id("User_FirstName"));
            var lastBox = driver.FindElement(By.Id("User_LastName"));
            var emailBox = driver.FindElement(By.Id("User_Email"));
            var birthBox = driver.FindElement(By.Id("User_BirthDate"));
            var roleSelect = driver.FindElement(By.Id("flexRadioStudent"));
            var passBox = driver.FindElement(By.Id("User_Password"));
            var confirmBox = driver.FindElement(By.Id("User_ConfirmPassword"));
            var createBtn = driver.FindElement(By.Id("submitButton"));

            firstBox.SendKeys(firstName);
            lastBox.SendKeys(lastName);
            emailBox.SendKeys(email);
            birthBox.SendKeys(birthDate);
            roleSelect.Click();
            passBox.SendKeys(password);
            confirmBox.SendKeys(password);
            createBtn.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            //test if at Dashboard
            title = driver.Title;
            Assert.AreEqual("Stellar LMS - Dashboard", title);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(1000);


            driver.Quit();

        }
    }
}
