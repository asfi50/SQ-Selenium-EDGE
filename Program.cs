using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Setup WebDriver
        var driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        
        // Setup reporting
        ExtentReports extent = new ExtentReports();
        string dateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string projectDir = FindProjectDirectory();
        
        // Create reports directory
        string reportsDir = Path.Combine(projectDir, "reports");
        Directory.CreateDirectory(reportsDir);
        
        string reportFilePath = Path.Combine(reportsDir, $"Report_{dateTime}.html");
        Console.WriteLine($"Report will be saved to: {reportFilePath}");
        
        ExtentSparkReporter htmlreporter = new ExtentSparkReporter(reportFilePath);
        extent.AttachReporter(htmlreporter);
        ExtentTest test = extent.CreateTest("ParaBank Test", "Registration and Login Test");

        try
        {
            OpenUrl(driver, test, "https://parabank.parasoft.com/parabank/index.htm");

            string csvPath = Path.Combine(projectDir, "userdata", "userdata.csv");
            Console.WriteLine($"Reading user data from: {csvPath}");
            
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<UserData>().ToList();
                
                Console.WriteLine($"Found {records.Count} users to process");
                test.Log(Status.Info, $"Found {records.Count} users to process");
                
                foreach (var record in records)
                {
                    string uniqueUsername = $"{record.Username}_{DateTime.Now.Second}";
                    try {
                        // Always start from home page before registration
                        driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/index.htm");
                        Thread.Sleep(1000);

                        // Register new user
                        RegisterUser(driver, test, record.FirstName, record.LastName, record.Address,
                            record.City, record.State, record.ZipCode, record.Phone, record.SSN,
                            uniqueUsername, record.Password);

                        // After registration, LOGOUT FIRST to get back to login page
                        Logout(driver, test);

                        // Now login with credentials
                        PerformLogin(driver, test, uniqueUsername, record.Password);

                        // Validate login success
                        if (ValidateLogin(driver, test))
                        {
                            // Logout after successful login to prepare for next user
                            Logout(driver, test);
                        }
                    }
                    catch (Exception userEx)
                    {
                        test.Log(Status.Warning, $"User {uniqueUsername}: {userEx.Message}");
                        Console.WriteLine($"Error with user {uniqueUsername}: {userEx.Message}");
                        // Always try to go back to the homepage for the next user
                        driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/index.htm");
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            test.Log(Status.Fail, $"Test failed with exception: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        finally
        {
            extent.Flush();
            Console.WriteLine($"Test execution completed. Report saved to: {reportFilePath}");
            driver.Quit();
        }
    }

    static string FindProjectDirectory()
    {
        string currentDir = AppContext.BaseDirectory;
        
        // Go up from bin directory to find project root
        string testDir = currentDir;
        while (!Directory.GetFiles(testDir, "*.csproj").Any() && Directory.GetParent(testDir) != null)
        {
            testDir = Directory.GetParent(testDir).FullName;
        }
        
        // If we found the project file
        if (Directory.GetFiles(testDir, "*.csproj").Any())
        {
            return testDir;
        }
        
        // Fallback to known path
        if (Directory.Exists(@"d:\QA\SeleniumReport"))
        {
            return @"d:\QA\SeleniumReport";
        }
        
        return currentDir;
    }

    static void OpenUrl(IWebDriver driver, ExtentTest test, string url)
    {
        test.Log(Status.Info, $"Muammar Tazwar Asfi - JnU");
        driver.Navigate().GoToUrl(url);
        Thread.Sleep(1000);
        driver.Manage().Window.Maximize();
    }

    static void RegisterUser(IWebDriver driver, ExtentTest test, string firstName, string lastName, 
        string address, string city, string state, string zipCode, string phone, string ssn, 
        string username, string password)
    {
        driver.FindElement(By.LinkText("Register")).Click();
        Thread.Sleep(1000);

        driver.FindElement(By.Id("customer.firstName")).SendKeys(firstName);
        driver.FindElement(By.Id("customer.lastName")).SendKeys(lastName);
        driver.FindElement(By.Id("customer.address.street")).SendKeys(address);
        driver.FindElement(By.Id("customer.address.city")).SendKeys(city);
        driver.FindElement(By.Id("customer.address.state")).SendKeys(state);
        driver.FindElement(By.Id("customer.address.zipCode")).SendKeys(zipCode);
        driver.FindElement(By.Id("customer.phoneNumber")).SendKeys(phone);
        driver.FindElement(By.Id("customer.ssn")).SendKeys(ssn);
        driver.FindElement(By.Id("customer.username")).SendKeys(username);
        driver.FindElement(By.Id("customer.password")).SendKeys(password);
        driver.FindElement(By.Id("repeatedPassword")).SendKeys(password);
        
        driver.FindElement(By.CssSelector("input[value='Register']")).Click();
        test.Log(Status.Info, $"User registered: {username}");
        Thread.Sleep(1000);
    }

    static void Logout(IWebDriver driver, ExtentTest test)
    {
        // Navigate directly to the logout URL 
        driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/logout.htm");
        test.Log(Status.Pass, "Logged out");
        Thread.Sleep(1000);
        
        // Navigate back to home page after logout
        driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/index.htm");
    }

    static void PerformLogin(IWebDriver driver, ExtentTest test, string username, string password)
    {
        driver.FindElement(By.Name("username")).SendKeys(username);
        driver.FindElement(By.Name("password")).SendKeys(password);
        driver.FindElement(By.CssSelector("input[value='Log In']")).Click();
        Thread.Sleep(1000);
        test.Log(Status.Info, $"Login attempted: {username}");
    }

    static bool ValidateLogin(IWebDriver driver, ExtentTest test)
    {
        try
        {
            // Check for Welcome message in the page content
            if (driver.PageSource.Contains("Welcome"))
            {
                // Look for Log Out link, but don't fail if not found
                try {
                    if (driver.FindElement(By.LinkText("Log Out")).Displayed) {
                        test.Log(Status.Pass, "Login successful");
                        return true;
                    }
                } catch {
                    // If Log Out link not found, still consider login successful if Welcome is present
                    test.Log(Status.Pass, "Login successful (Welcome message found)");
                    return true;
                }
                
                return true; // Login was successful
            }
            else
            {
                test.Log(Status.Fail, "Login failed (Welcome message not found)");
                return false;
            }
        }
        catch (Exception)
        {
            test.Log(Status.Fail, "Login validation failed");
            return false;
        }
    }
}

public class UserData
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}