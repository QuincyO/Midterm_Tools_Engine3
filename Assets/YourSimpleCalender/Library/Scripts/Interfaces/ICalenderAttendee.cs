using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine.Events;

namespace Quincy.Calender
{
    public interface ICalenderAttendee
    {   
        void OnNotify(Event @event);
    }

}
