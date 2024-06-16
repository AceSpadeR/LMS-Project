using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace SeleniumTests
{

	[TestClass]
	public class CorrectFeeTest
	{

		private IWebDriver driver;

		[TestInitialize]
		public void Setup()
		{
			driver = new ChromeDriver();
		}

		[TestMethod]
		public void CorrectFee()
		{
			driver.Navigate().GoToUrl("https://lms20240312105559.azurewebsites.net/");

			IWebElement inputField = driver.FindElement(By.Id("Email"));
			inputField.SendKeys("student@student.com");

			inputField = driver.FindElement(By.Id("Password"));
			inputField.SendKeys("Student");

			IWebElement button = driver.FindElement(By.Id("submitButton"));
			button.Click();

			IWebElement link = driver.FindElement(By.Id("Fees"));
			link.Click();

			IWebElement fee = driver.FindElement(By.CssSelector("h3"));

			Assert.AreEqual("TOTAL DUE: $520.92", fee.Text);

			driver.Quit();
		}

	}
}
