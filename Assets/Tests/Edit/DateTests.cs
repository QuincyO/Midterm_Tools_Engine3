using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Quincy.Calender;
using UnityEngine;
using UnityEngine.TestTools;


public class DateTests
{
    // A Test behaves as an ordinary method

    [Test]
    public void Date_EdgeCases12()
    {
        var Date = new Date();
        Date.isMilitaryTime = false;
        Date.Hours = 12;
        Assert.AreEqual(12, Date.Hours);

    }
    
    [Test]
    public void Date_EdgeCases0()
    {
        var Date = new Date();
        Date.isMilitaryTime = false;
        Date.Hours = 0;
        Assert.AreEqual(12, Date.Hours);
    }
    
    [Test]
    public void Date_EdgeCasesLessThan0()
    {
        var Date = new Date();
        Date.isMilitaryTime = false;
        Date.Hours = -50;
        Assert.AreEqual(12, Date.Hours);

    }
    
    [Test]
    public void Date_EdgeCasesGreaterThan12()
    {
        var Date = new Date();
        Date.isMilitaryTime = false;
        Date.Hours = 89;
        Assert.AreEqual(11, Date.Hours);
    }
    
    [Test]
    public void Date_MinuteClampedProperly()
    {
        Date date = new Date();

        // Test setting Minutes above the maximum allowed (59)
        date.Minutes = 75; // Set invalid minute
        Assert.AreEqual(59, date.Minutes, "Minutes was not clamped to 59");

        // Test setting Minutes below the minimum allowed (0)
        date.Minutes = -10; // Set invalid minute
        Assert.AreEqual(0, date.Minutes, "Minutes was not clamped to 0");

        // Test setting Minutes within valid range (0-59)
        date.Minutes = 30; // Set valid minute
        Assert.AreEqual(30, date.Minutes, "Minutes should be set to 30");
    }
    
    [Test]
    public void Date_HourClampedProperlyMilTime()
    {
        Date date = new Date();
        
        //Testing Mil Time
        Date.isMilitaryTime = true;
        for (int i = -50; i < 100; i++)
        {
            date.Hours = i;
            if(i < 0) Assert.AreEqual(0, date.Hours, "Hours was not clamped to 0");
            else if (i is >= 0 and <= 23) Assert.AreEqual(i, date.Hours, "Hours was not clamped to " + i);
            else Assert.AreEqual(23, date.Hours, "Hours was not clamped to 23");
            
        }
    }

    [Test]
    public void Date_DaysClampedProperly()
    {
        // Array with months and their respective maximum days
        var monthMaxDays = new (Month month, int maxDays)[]
        {
            (Month.January, 31),
            (Month.February, 28),
            (Month.March, 31),
            (Month.April, 30),
            (Month.May, 31),
            (Month.June, 30),
            (Month.July, 31),
            (Month.August, 31),
            (Month.September, 30),
            (Month.October, 31),
            (Month.November, 30),
            (Month.December, 31)
        };

        // Test setting day above the maximum allowed for each month
        foreach (var (month, maxDays) in monthMaxDays)
        {
            Date date = new Date();
            date.Month = month;
            date.Day = 50; // Set an invalid day greater than any month's max
        
            // Assert that the day is clamped to the maximum for the month
            Assert.AreEqual(maxDays, date.Day, $"Failed for month {month}");
        }
    }


    [Test]
    public void DateMonthTest()
    {
        for (int i = 1; i <= 12; i++)
        {
            Date date = new Date();
            date.Month = (Month)i;
            Assert.AreEqual(i, (int)date.Month);
        }
    }
    [Test]
    public void CopyConstructorTest()
    {
        int testCount = 5;
        Date[] testData = new Date[testCount];

        //Amount of Dates to test
        for (int i = 0; i < testCount; i++)
        {
            int randomYear = UnityEngine.Random.Range(1900, 9999);
            int randomMonth = UnityEngine.Random.Range(-999, 999);
            int randomDay = UnityEngine.Random.Range(-999, 999);
            int randomHour = UnityEngine.Random.Range(-999, 999);
            int randomMinute = UnityEngine.Random.Range(-999, 999);
            Date newDate = new Date(randomYear, (Month)randomMonth, randomDay, randomHour, randomMinute);
            testData[i] = newDate;
        }

        foreach (Date date in testData)
        {
            Date newDate = new Date(date);
            Assert.AreEqual(newDate.Year, date.Year);
            Assert.AreEqual(newDate.Month, date.Month);
            Assert.AreEqual(newDate.Day, date.Day);
            Assert.AreEqual(newDate.Hours, date.Hours);
            Assert.AreEqual(newDate.Minutes, date.Minutes);
        }
    }
    
       [Test]
    public void TestMonthEnumValues()
    {
        Assert.AreEqual(1, (int)Month.January);
        Assert.AreEqual(2, (int)Month.February);
        Assert.AreEqual(12, (int)Month.December);
    }

    [Test]
    public void TestAddDay_NoRollover()
    {
        Date date = new Date(2024, Month.January, 15, 10, 30); // Jan 15, 2024
        date = date.AddDay(5);
        
        Assert.AreEqual(20, date.Day);
        Assert.AreEqual(Month.January, date.Month);
    }

    [Test]
    public void TestAddDay_WithRolloverToNextMonth()
    {
        Date date = new Date(2024, Month.January, 28, 10, 30); // Jan 28, 2024
        date = date.AddDay(5);
        
        Assert.AreEqual(2, date.Day);
        Assert.AreEqual(Month.February, date.Month);
    }

    [Test]
    public void TestAddDay_WithRolloverToNextYear()
    {
        Date date = new Date(2024, Month.December, 29, 10, 30); // Dec 29, 2024
        date = date.AddDay(5);
        
        Assert.AreEqual(3, date.Day);
        Assert.AreEqual(Month.January, date.Month);
        Assert.AreEqual(2025, date.Year);
    }

    [Test]
    public void TestAddMonths_WithRolloverToNextYear()
    {
        Date date = new Date(2024, Month.November, 15, 10, 30); // Nov 15, 2024
        date = date.AddMonths(3);
        
        Assert.AreEqual(Month.February, date.Month);
        Assert.AreEqual(2025, date.Year);
    }

    [Test]
    public void TestAddMonths_NoRollover()
    {
        Date date = new Date(2024, Month.January, 15, 10, 30); // Jan 15, 2024
        date = date.AddMonths(2);
        
        Assert.AreEqual(Month.March, date.Month);
        Assert.AreEqual(2024, date.Year);
    }

    [Test]
    public void TestAddYears()
    {
        Date date = new Date(2024, Month.January, 15, 10, 30); // Jan 15, 2024
        date = date.AddYears(2);
        
        Assert.AreEqual(2026, date.Year);
    }
    
}
