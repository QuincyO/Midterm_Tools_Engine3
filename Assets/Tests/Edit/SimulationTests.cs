using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Quincy.Calender;
using UnityEngine;
using UnityEngine.TestTools;


public class SimulationTests
{


     [Test]
    public void SimulateIncrementingByMinuteForTwoYears()
    {
        // Start date: January 1, 2024, at 00:00
        Date date = new Date(2024, Month.January, 1, 0, 0);
        
        // Track the current month
        Month currentMonth = date.Month;

        // Total minutes in two non-leap years
        int totalMinutes = 365 * 2 * 24 * 60; // 365 days per year, 24 hours, 60 minutes

        // Path to log file in the project's root directory
        string logFilePath = Path.Combine(Application.dataPath, "../DateIncrementLog.txt");

        // Create or clear the log file
        using (StreamWriter writer = new StreamWriter(logFilePath, false))
        {
            writer.WriteLine("Date Increment Log");
            writer.WriteLine("Start Date: " + date);
            writer.WriteLine("Simulating increments by one minute for two years...");
            writer.WriteLine("---------------------------------------------------");

            // Loop through each minute increment
            for (int i = 0; i < totalMinutes; i++)
            {
                date = date.AddMinutes(1); // Increment the date by one minute

                // Check if the month has changed
                if (date.Minutes == 0)
                {
                    writer.WriteLine($"{date.Year}-{(int)date.Month:D2}-{date.Day:D2}T{date.Hours:D2}:{date.Minutes:D2}");
                    currentMonth = date.Month; // Update currentMonth to the new month
                }
            }

            writer.WriteLine("---------------------------------------------------");
            writer.WriteLine("End Date: " + date);
        }

        // Expected date after two years without leap years
        Date expectedDate = new Date(2026, Month.January, 1, 0, 0);

        Assert.AreEqual(expectedDate.Year, date.Year, "Year does not match expected value.");
        Assert.AreEqual(expectedDate.Month, date.Month, "Month does not match expected value.");
        Assert.AreEqual(expectedDate.Day, date.Day, "Day does not match expected value.");
        Assert.AreEqual(expectedDate.Hours, date.Hours, "Hour does not match expected value.");
        Assert.AreEqual(expectedDate.Minutes, date.Minutes, "Minutes do not match expected value.");
    }

}


    

