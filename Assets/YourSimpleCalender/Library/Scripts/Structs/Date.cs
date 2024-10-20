using System;
using static Quincy.Calender.MonthEnum;

namespace Quincy.Calender
{
    [System.Serializable]
    public struct Date
    {
        
        #region Variables
        
        public string EventName{get; set; }
        private bool _isMilitaryTime;
        
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
                return _isMilitaryTime ? _hour % 12 : _hour;
            }
            set
            {
                {
                    _hour = Math.Clamp(value, 0, 24);
                }
            }
        }
        
        public int Minute;
        
        public string Period
        {
            get => _isMorning ? $"am" : $"pm";
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

        public MonthEnum Month
        {
            get => (MonthEnum)_month;
            set => _month = (int)value;
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


    }

}
