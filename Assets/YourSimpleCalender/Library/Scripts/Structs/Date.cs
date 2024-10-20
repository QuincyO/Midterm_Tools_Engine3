using System;
using UnityEngine.Serialization;
using static Quincy.Calender.Month;


namespace Quincy.Calender
{
    //Todo: If I want to be able to sort the date I need to add  IComparable<Date> interface and use CompareTo() to be able to use dates.Sort() 
    [System.Serializable]
    public struct Date
    {
        
        #region Variables
        
        public bool isMilitaryTime;
        
        #region Hour/Minute
        //Private Backing Fields
        private int _hour;
        private int _minute;
        private bool _isMorning;
        
        
        
        //Properties
        public int Hour
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
                {
                    _hour = Math.Clamp(value, 0, 23);
                    _isMorning = _hour < 12;
                }
            }
        }
        
        public int Minute
        {
            get => _minute;
            set => _minute = Math.Clamp(value, 0, 59);
        }
        
        public string Period
        {
            get => _hour < 12? $"am" : $"pm";
            private set => _isMorning = _hour < 12;
        }
        
        #endregion
        
        #region Year/Month/Day
        //Private Backing Fields//
        private int _year;
        private int _month;
        private int _day;
        
        
        public int Year
        {
            get => _year;
            set => _year = value;
        }

        public Month Month
        {
            get => (Month)_month;
            set => _month = Math.Clamp((int)value, 1, 12);
        }

        public int Day
        {
            get {return _day;}
            set
            {
                int maxDay;
                #region Puts a Clamped value on the day if month doesnt support it
                switch (Month)
                {
                    case None:
                        maxDay = value;
                        break;
                    case January:
                        maxDay = 31;
                        break;
                    case February:
                        maxDay = 28;
                        break;
                    case March:
                        maxDay = 31;
                        break;
                    case April:
                        maxDay = 30;
                        break;
                    case May:
                        maxDay = 31;
                        break;
                    case June:
                        maxDay = 30;
                        break;
                    case July:
                        maxDay = 31;
                        break;
                    case August:
                        maxDay = 31;
                        break;
                    case September:
                        maxDay = 30;
                        break;
                    case October:
                        maxDay = 31;
                        break;
                    case November:
                        maxDay = 30;
                        break;
                    case December:
                        maxDay = 31;
                        break;
                    default:
                        _day = 0;
                        return;
                }
                #endregion
                _day = Math.Clamp(value, 1, maxDay);
            }
        }
            #endregion
            
            
        #endregion

        
        #region Constructors
        
        public Date(int year, Month month, int day,int hour,int minute, bool isMilitaryTime = false) : this()
        {
            this.isMilitaryTime = isMilitaryTime;
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
        }

        public Date(Date date) : this()
        {
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
            Hour = date.Hour;
            Minute = date.Minute;
            Period = date.Period;
            isMilitaryTime = date.isMilitaryTime;
        }
        
        #endregion

    }

}
