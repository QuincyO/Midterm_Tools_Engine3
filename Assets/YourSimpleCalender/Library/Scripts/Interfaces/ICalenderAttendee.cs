using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine.Events;

namespace Quincy.Calender
{
    public interface ICalendarListener
    {   
        void OnNotify(Event @event);
    }

}
