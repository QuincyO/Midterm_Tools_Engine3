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
        var date = new Date();
        date.isMilitaryTime = false;
        date.Hour = 12;
        Assert.AreEqual(12, date.Hour);

    }
    
    [Test]
    public void Date_EdgeCases0()
    {
        var date = new Date();
        date.isMilitaryTime = false;
        date.Hour = 0;
        Assert.AreEqual(12, date.Hour);

    }
    
    [Test]
    public void Date_EdgeCasesLessThan0()
    {
        var date = new Date();
        date.isMilitaryTime = false;
        date.Hour = -50;
        Assert.AreEqual(12, date.Hour);

    }
    
    [Test]
    public void Date_EdgeCasesGreaterThan12()
    {
        var date = new Date();
        date.isMilitaryTime = false;
        date.Hour = 89;
        Assert.AreEqual(11, date.Hour);
    }
    
    [Test]
    public void Date_MinuteClampedProperly()
    {
        Date date = new Date();

        // Test setting Minute above the maximum allowed (59)
        date.Minute = 75; // Set invalid minute
        Assert.AreEqual(59, date.Minute, "Minute was not clamped to 59");

        // Test setting Minute below the minimum allowed (0)
        date.Minute = -10; // Set invalid minute
        Assert.AreEqual(0, date.Minute, "Minute was not clamped to 0");

        // Test setting Minute within valid range (0-59)
        date.Minute = 30; // Set valid minute
        Assert.AreEqual(30, date.Minute, "Minute should be set to 30");
    }
    
    [Test]
    public void Date_HourClampedProperlyMilTime()
    {
        Date date = new Date();
        
        //Testing Mil Time
        date.isMilitaryTime = true;
        for (int i = -50; i < 100; i++)
        {
            date.Hour = i;
            if(i < 0) Assert.AreEqual(0, date.Hour, "Hour was not clamped to 0");
            else if (i is >= 0 and <= 23) Assert.AreEqual(i, date.Hour, "Hour was not clamped to " + i);
            else Assert.AreEqual(23, date.Hour, "Hour was not clamped to 23");
            
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
            Assert.AreEqual(newDate.Hour, date.Hour);
            Assert.AreEqual(newDate.Minute, date.Minute);
            Assert.AreEqual(newDate.isMilitaryTime, date.isMilitaryTime);
        }
    }
    

}
