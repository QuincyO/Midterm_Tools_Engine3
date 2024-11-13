using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using static Quincy.Calender.Month;



namespace Quincy.Calender
{
    [System.Serializable]
    
    //IEquatable was added by my IDE automatically, its Essentially an interface that allows me to say if (Date==Date)
    public struct Date : IEquatable<Date>, IComparable<Date>
    {
        
        #region Variables
        
        public static bool isMilitaryTime;
        
        #region Hours/Minutes
        //Private Backing Fields
        [SerializeField] int _hour;
        [SerializeField]private int minutes;
        [SerializeField]private bool _isMorning;

        [SerializeField]
        public int MaxDays
        {
            get
            {
                return Month switch
                {
                    January or March or May or July or August or October or December => 31,
                    April or June or September or November => 30,
                    February => 28,
                    _ => 0
                };
            }
        }

        
        public int Hours
        {
            get
            {
                if (isMilitaryTime) return _hour;
                else
                {
                    return (_hour == 0 || _hour == 12  )? 12 : _hour % 12;
                }
            } 
            set
            {
               _hour = Math.Clamp(value, 0, 23);
               _isMorning = _hour < 12;
        
            }
        }
        
        public int Minutes
        {
            get => minutes;
            set => minutes = Math.Clamp(value, 0, 59);
        }

        [NotNull]
        public readonly string Period
        {
            get => isMilitaryTime ? "" : (_isMorning ? "AM" : "PM");

        }

        #endregion
        
        #region Year/Month/Day
        //Private Backing Fields//
        [SerializeField]private int _year;
        [SerializeField]private Month _month;
        [SerializeField]private int _day;
        
        
        public int Year
        { 
            readonly get => _year;
            set => _year = value;
        }

        public Month Month
        { 
            readonly get => _month;
            set => _month = (Month)Math.Clamp((int)value, (int)January, (int)December);
        }

        public int Day
        {
            
            readonly get => _day;
            set
            {
                _day = Math.Clamp(value, 1, MaxDays);
            }
        }
            #endregion
            
            
        #endregion

        #region Formatted Date Strings

        public string FormattedHour => Hours.ToString("D2");
        public string FormattedMinutes => Minutes.ToString("D2");
        public string FormattedDay => Day.ToString("D2");
        public string FormattedYear => Year.ToString("D4");

        public float TotalMinutes { 
            get
                {
                    return _hour * 60 + Minutes;
                }
                
            set 
                {

                }
        }
        #endregion

        #region Constructors

        public Date(int year, Month month, int day,int hours,int minutes) : this()
        {
            Year = year;
            Month = month;
            Day = day;
            Hours = hours;
            Minutes = minutes;
        }

        public Date(Date date) : this()
        {
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
            Hours = date.Hours;
            Minutes = date.Minutes;
            _isMorning = date._isMorning;
        }


        
        #endregion

        
        #region Generated by Rider. Overload operators
        public bool Equals(Date other)
        {
            return  _hour == other._hour && minutes == other.minutes && _isMorning == other._isMorning && _year == other._year && _month == other._month && _day == other._day;
        }

        public override bool Equals(object obj)
        {
            return obj is Date other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(isMilitaryTime, _hour, minutes, _isMorning, _year, _month, _day);
        }

        public static bool operator ==(Date left, Date right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Date left, Date right)
        {
            return !left.Equals(right);
        }
        #endregion

        
        #region CompareTo() Interface Function to sort by date
        public int CompareTo(Date other)
        {
            if (other == null) return 1;
            
            var yearComparison = Year.CompareTo(other.Year);
            if (yearComparison != 0) return yearComparison;
            var monthComparison = Month.CompareTo(other.Month);
            if (monthComparison != 0) return monthComparison;
            var dayComparison = Day.CompareTo(other.Day);
            if(dayComparison != 0) return dayComparison;
            var hourComparison = Hours.CompareTo(other.Hours);
            if (hourComparison != 0) return hourComparison;
            var minuteComparison = Minutes.CompareTo(other.Minutes);
            if (minuteComparison != 0) return minuteComparison;
            
            return _isMorning.CompareTo(other._isMorning);
        }
        #endregion
        
        
        #region Methods

        public int DaysSince(Date otherDate)
        {
            int days = 0;
            Date date = new Date(otherDate);
            while (date.CompareTo(this) < 0)
            {
                date = date.AddDay(1);
                days++;
            }

            //Debug.Log(days);
            return days;
        }

        public int DayOfWeek()
        {
            int refYear = 1900;
            Month refMonth = (Month)1;
            int refDay = 1;

            int totalDays = DaysSince(new Date(refYear, refMonth, refDay, 0, 0));

            return totalDays % 7;
        }



        public Date AddMinutes(int minutesToAdd)
        {
            Date newDate = this; // Create a copy of the current Date struct
            int totalMinutes = newDate.Minutes + minutesToAdd;
            newDate.Minutes = totalMinutes % 60;
            // Calculate rollover for hours if totalMinutes exceeds 59
            if (totalMinutes >= 60)
            {
                int hoursToAdd = totalMinutes / 60;
                newDate = newDate.AddHours(hoursToAdd);
            }

            return newDate;
        }

        public Date AddHours(int hoursToAdd)
        {
            Date newDate = this; // Create a copy of the current Date struct
            int totalHours = newDate._hour + hoursToAdd;
            newDate._hour = totalHours % 24;
            newDate._isMorning = newDate._hour < 12;

            // Calculate rollover for days if totalHours exceeds 23
            if (totalHours >= 24)
            {
                int daysToAdd = totalHours / 24;
                newDate = newDate.AddDay(daysToAdd);
            }

            return newDate;
        }

        public Date AddDay(int daysToAdd)
        {
            Date newDate = this; // Create a copy of the current Date struct
            int totalDays = newDate.Day + daysToAdd;

            // Handle day overflow by adjusting month/year if necessary
            while (totalDays > newDate.MaxDays)
            {
                totalDays -= newDate.MaxDays;
                newDate = newDate.AddMonths(1); // Increment month when days exceed max for current month
            }

            while (totalDays < 1)
            {
                totalDays += newDate.MaxDays;
                newDate = newDate.AddMonths(-1); // Decrement month when days go below 1
            }

            newDate.Day = totalDays;
            return newDate;
        }

        public Date AddMonths(int monthsToAdd)
        {
            Date newDate = this; // Create a copy of the current Date struct
            int totalMonths = monthsToAdd + (int)newDate.Month;

            // Handle month overflow by adjusting year if necessary
            while (totalMonths > 12)
            {
                totalMonths -= 12;
                newDate = newDate.AddYears(1); // Increment year when months exceed 12
            }

            while (totalMonths < 1)
            {
                totalMonths += 12;
                newDate = newDate.AddYears(-1); // Decrement year when months go below 1
            }

            newDate.Month = (Month)totalMonths;
            return newDate;
        }

        public Date AddYears(int yearsToAdd)
        {
            Date newDate = this; // Create a copy of the current Date struct
            newDate.Year += yearsToAdd;
            return newDate;
        }

        #endregion
        
        #region Overload Operator 

        //These Operator Overloads are Generated By AI
        public override string ToString()
        {
            return $"Date: {Year:D4}-{(int)Month:D2}-{Day:D2}  Time:{Hours:D2}:{Minutes:D2} {Period.ToUpper()}";

        }
        #endregion
        
        #region Operator Overloads

        public static bool operator >(Date left, Date right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <(Date left, Date right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >=(Date left, Date right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <=(Date left, Date right)
        {
            return left.CompareTo(right) <= 0;
        }

        #endregion
    }





    [System.Serializable]
    public struct DateScriptableObject
    {
        public int Year;
        public Month Month;
        public int Day;
        public int Hour;
        int Minute;
    }
}
