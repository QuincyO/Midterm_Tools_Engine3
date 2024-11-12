using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalenderUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text monthText;
    [SerializeField] private TMP_Text yearText;

    [SerializeField] private Button nextButton, previousButton;

    [SerializeField] private Date focusedDate;

    [SerializeField] CalenderPanel[] calenderPanels;

     void Awake()
    {


        var texts = GetComponentsInChildren<TMP_Text>();
        foreach (var text in texts)
        {
            if (text.name == "DayText")
            {
                dayText = text;
                continue;
            }
            else if (text.name == "MonthText")
            {
                monthText = text;
                continue;
            }
            else if (text.name == "YearText")
            {
                yearText = text;
                continue;
            }
        }

        #region Set Button Components
        var buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            if (button.name == "NextButton")
            {
                nextButton = button;
                continue;
            }
            else if (button.name == "PreviousButton")
            {
                previousButton = button;
                continue;
            }
        }
        #endregion

        #region Set Calender Panels

        calenderPanels = GetComponentsInChildren<CalenderPanel>();

        for (int i = 0; i < calenderPanels.Length; i++)
        {
            calenderPanels[i].SetHighlight(false);
        }
        #endregion
    }


    void Start()
    {
        nextButton.onClick.AddListener(GoNextMonth);
        previousButton.onClick.AddListener(GoPreviousMonth);
        focusedDate = CalenderManager.Instance.CurrentDate;

        UpdateCalenderUI();
    }



    private void SyncDatesToMonth()
    {

        //Bug: there is a bug, that if you keep on advancing months, the end date will get cut off instead of the first rolling back to fit.
        //Resolution: on the months that there is an overflow, I need to create another line of 7 days to fit the remaining days.
        //Todo: Fix this bug
        
        Date firstOfMonth = new Date(focusedDate.Year, focusedDate.Month, 1, 0, 0);
        int startDayOffset = (int)firstOfMonth.DayOfWeek();
        
        //TODO: Show upcoming events on the calender


        for (int i = 0; i < calenderPanels.Length; i++)
        {
            int day = i - startDayOffset + 1;

            if (day > 0 && day <= focusedDate.MaxDays)
            {
                calenderPanels[i].SetDay(day);
                calenderPanels[i].MakeAvailable();
            }
            else
            {
                calenderPanels[i].SetDay(0);
                calenderPanels[i].MakeUnavailable();
            }
            
            //var date = i % focusedDate.MaxDays + 1;
            //calenderPanels[i].SetDay(date);
        }
    }

    void UpdateCalenderUI()
    {
        SetMonthText();
        SetYearText();

        SetDateHighlight();


        SyncDatesToMonth();

        SyncEventsToDays();
    }

    private void SyncEventsToDays()
    {
        List<Quincy.Calender.Event> eventsThisMonth = CalenderManager.Instance.GetEventsForMonth(focusedDate.Month, focusedDate.Year);
        foreach (var panel in calenderPanels)
        {
            panel.ClearEvents();
        }

        for (int i = 0; i < eventsThisMonth.Count; i++)
        {
            int day = eventsThisMonth[i].startingDate.Day;
            calenderPanels[day - 1].AddEvent(eventsThisMonth[i]);
        }

    }

    private void SetMonthText()
    {
        monthText.text = focusedDate.Month.ToString();
    }
    private void SetYearText()
    {
        yearText.text = focusedDate.Year.ToString();
    }
    private void SetDateHighlight()
    {
        calenderPanels[focusedDate.Day - 1].SetHighlight(false);
        if (focusedDate.Month == CalenderManager.Instance.CurrentDate.Month &&
         focusedDate.Year == CalenderManager.Instance.CurrentDate.Year)
        {
           calenderPanels[focusedDate.Day - 1].SetHighlight(true);
        }
    }
    

    #region Month Switching Methods

    
    void GoNextMonth()
    {
        Date nextMonth = focusedDate.AddMonths(1);
        focusedDate = nextMonth;

        UpdateCalenderUI();
    }

    void GoPreviousMonth()
    {
        Date previousMonth = focusedDate.AddMonths(-1);
        focusedDate = previousMonth;
        UpdateCalenderUI();
    }

    #endregion
}
