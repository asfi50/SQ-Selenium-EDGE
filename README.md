# Selenium Test Automation for ParaBank

## Overview

This project demonstrates automated testing of the ParaBank demo application using Selenium WebDriver with C#. It performs user registration and login verification, generating detailed HTML reports for test results.

## Author

Muammar Tazwar Asfi

## Features

- Automated registration of users from CSV data
- Login verification
- HTML report generation using ExtentReports
- CSV data-driven testing approach

## Project Structure

- `/Program.cs` - Main test script
- `/userdata/userdata.csv` - Test data for multiple users
- `/reports/` - Generated test reports (HTML)

## Requirements

- .NET 9.0 or later
- Chrome browser
- NuGet packages:
  - Selenium.WebDriver
  - Selenium.WebDriver.ChromeDriver
  - CsvHelper
  - ExtentReports

## How to Run

1. Clone the repository
2. Ensure Chrome browser is installed
3. Open the project in Visual Studio or preferred IDE
4. Restore NuGet packages
5. Run the application with `dotnet run`

## Important Notes

- Always edit the CSV file in the project root directory (not in the bin folder)
- The application always uses the CSV from the project directory, not from bin
- If you update the CSV file, the changes will be used in the next test run

## Report Location

Test reports are generated in the `/reports` directory with timestamps in the filename.
