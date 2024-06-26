using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CogProject
{
    class Program
    {
        static IWebDriver driver;

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a URL:");
            string inputUrl = Console.ReadLine();

            Console.WriteLine("Please enter the browser (chrome/firefox/edge):");
            string browser = Console.ReadLine().ToLower();

            switch (browser)
            {
                case "chrome":
                    driver = new ChromeDriver();
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "edge":
                    driver = new EdgeDriver();
                    break;
                default:
                    Console.WriteLine("Unsupported browser!");
                    return;
            }

            try
            {
                driver.Navigate().GoToUrl(inputUrl);
                Thread.Sleep(5000);

                List<Tuple<string, string>> elements = new List<Tuple<string, string>>();

                while (true)
                {
                    Console.WriteLine("Enter the locator type (name, id, css, xpath) or 'exit' to finish:");
                    string locatorType = Console.ReadLine().ToLower();

                    if (locatorType == "exit")
                    {
                        break;
                    }

                    Console.WriteLine("Enter the locator value:");
                    string locatorValue = Console.ReadLine();

                    elements.Add(Tuple.Create(locatorType, locatorValue));
                }

                foreach (var element in elements)
                {
                    VerifyElement(element.Item1, element.Item2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                driver.Quit();
            }
        }

        static void VerifyElement(string method, string locator)
        {
            By by = null;
            switch (method.ToLower())
            {
                case "name":
                    by = By.Name(locator);
                    break;
                case "id":
                    by = By.Id(locator);
                    break;
                case "css":
                    by = By.CssSelector(locator);
                    break;
                case "xpath":
                    by = By.XPath(locator);
                    break;
                default:
                    Console.WriteLine($"Unsupported method: {method}");
                    return;
            }

            try
            {
                IWebElement element = driver.FindElement(by);
                if (element.Displayed)
                {
                    Console.WriteLine($"Element located by {method} with value '{locator}' is displayed");
                }
                else
                {
                    Console.WriteLine($"Element located by {method} with value '{locator}' is not present");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Element located by {method} with value '{locator}' not found.");
            }
        }
    }
}
