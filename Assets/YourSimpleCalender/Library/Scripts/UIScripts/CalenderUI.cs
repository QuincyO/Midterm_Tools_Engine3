using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text monthText;
    [SerializeField] private TMP_Text yearText;

    [SerializeField] private Button nextButton, previousButton, exitButton;

    [SerializeField] private Date focusedDate;

    [SerializeField] CalendarPanel[] calenderPanels;
    [SerializeField] private TMP_Text timeText;

    [SerializeField] public MyCalendar calender;

    /// <summary>
    /// Initializes the Calendar UI components.
    /// </summary>
    void Awake()
    {
        #region Set Text Components
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
            else if (text.name == "TimeText")
            {
                timeText = text;
                continue;
            }

            if (monthText != null && yearText != null && timeText != null)
            {
                break;
            }
        }
        #endregion

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
            else if (button.name == "ExitButton")
            {
                exitButton = button;
                continue;
            }

            if (nextButton != null && previousButton != null && exitButton != null)
            {
                break;
            }
        }
        #endregion

        #region Set Calendar Panels
        calenderPanels = GetComponentsInChildren<CalendarPanel>();

        for (int i = 0; i < calenderPanels.Length; i++)
        {
            calenderPanels[i].name = $"Panel: {i + 1}";
        }
        #endregion

        focusedDate = CalendarManager.Instance.CurrentDate;
    }

    /// <summary>
    /// Subscribes to calendar manager events when enabled.
    /// </summary>
    private void OnEnable()
    {
        CalendarManager.OnTimeChanged += SetTimeText;
        CalendarManager.OnNewDay += NewDay;
    }

    /// <summary>
    /// Unsubscribes from calendar manager events when disabled.
    /// </summary>
    void OnDisable()
    {
        CalendarManager.OnTimeChanged -= SetTimeText;
        CalendarManager.OnNewDay -= NewDay;
    }

    /// <summary>
    /// Updates the time text display based on the current time.
    /// </summary>
    private void SetTimeText()
    {
        string Hours = CalendarManager.Instance.CurrentDate.FormattedHour;
        string Minutes = CalendarManager.Instance.CurrentDate.FormattedMinutes;
        string period = CalendarManager.Instance.CurrentDate.Period;
        timeText.text = $"{Hours}:{Minutes} {period}";
    }

    /// <summary>
    /// Sets up buttons and initializes the calendar UI on start.
    /// </summary>
    void Start()
    {
        nextButton.onClick.AddListener(GoNextMonth);
        previousButton.onClick.AddListener(GoPreviousMonth);
        exitButton.onClick.AddListener(() => Destroy(this.gameObject.transform.parent.gameObject.transform.parent.gameObject));
        focusedDate = CalendarManager.Instance.CurrentDate;
        SetTimeText();
        UpdateCalenderUI();
    }

    /// <summary>
    /// Synchronizes the dates displayed on the calendar panels to the focused month.
    /// </summary>
    private void SyncDatesToMonth()
    {
        //Bug: there is a bug, that if you keep on advancing months, the end date will get cut off instead of the first rolling back to fit.
        //Resolution: on the months that there is an overflow, I need to create another line of 7 days to fit the remaining days.
        //Todo: Fix this bug

        Date firstOfMonth = new Date(focusedDate.Year, focusedDate.Month, 1, 0, 0);
        int startDayOffset = (int)firstOfMonth.DayOfWeek();


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
        }
    }

    /// <summary>
    /// Updates the calendar UI elements to reflect the current state.
    /// </summary>
    void UpdateCalenderUI()
    {
        SetMonthText();
        SetYearText();
        SetDateHighlight();
        SyncDatesToMonth();
        SyncEventsToDays();
    }

    /// <summary>
    /// Synchronizes events to the corresponding day panels.
    /// </summary>
    private void SyncEventsToDays()
    {
        if (calender == null)
        {
            return;
        }
        List<Quincy.Calender.Event> eventsThisMonth = calender.GetEventsForMonth(focusedDate.Month, focusedDate.Year);
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

    /// <summary>
    /// Sets the text for the month display.
    /// </summary>
    private void SetMonthText()
    {
        monthText.text = focusedDate.Month.ToString();
    }

    /// <summary>
    /// Sets the text for the year display.
    /// </summary>
    private void SetYearText()
    {
        yearText.text = focusedDate.Year.ToString();
    }

    /// <summary>
    /// Highlights the current date on the calendar.
    /// </summary>
    private void SetDateHighlight()
    {
        foreach (var panel in calenderPanels)
        {
            panel.SetHighlight(false);
        }
        if (focusedDate.Month == CalendarManager.Instance.CurrentDate.Month &&
            focusedDate.Year == CalendarManager.Instance.CurrentDate.Year)
        {
            calenderPanels[focusedDate.Day - 1].SetHighlight(true);
        }
    }

    #region Change Day

    /// <summary>
    /// Updates the calendar UI when a new day begins.
    /// </summary>
    /// <param name="newDay">The new current date.</param>
    private void NewDay(Date newDay)
    {
        focusedDate = newDay;
        SetTimeText();
        UpdateCalenderUI();
    }

    #endregion

    #region Month Switching Methods

    /// <summary>
    /// Advances the focused date to the next month.
    /// </summary>
    void GoNextMonth()
    {
        Date nextMonth = focusedDate.AddMonths(1);
        focusedDate = nextMonth;
        UpdateCalenderUI();
    }

    /// <summary>
    /// Moves the focused date to the previous month.
    /// </summary>
    void GoPreviousMonth()
    {
        Date previousMonth = focusedDate.AddMonths(-1);
        focusedDate = previousMonth;
        UpdateCalenderUI();
    }

    /// <summary>
    /// Sets the calendar to be displayed by the UI.
    /// </summary>
    /// <param name="calender">The calendar to display.</param>
    internal void SetCalender(MyCalendar calender)
    {
        this.calender = calender;
    }

    #endregion
}
