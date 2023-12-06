using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumDocs.GettingStarted;

public static class FirstScript
{
    public static void Main()
    {
        IWebDriver driver = new ChromeDriver();

        driver.Navigate().GoToUrl("https://www.google.com");

        var title = driver.Title;

        Console.WriteLine(title);
            
        driver.Quit();
    }
}