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
        }
        #endregion

        #region Set Calender Panels

        calenderPanels = GetComponentsInChildren<CalenderPanel>();
        int count = 1;
        foreach (var panel in calenderPanels)
        {
            panel.SetHighlight(false);
            panel.SetDay(count);
            count++;
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

    void UpdateCalenderUI()
    {
        dayText.text = focusedDate.Day.ToString();
        monthText.text = focusedDate.Month.ToString();
    }
    

    #region Month Switching Methods
    void GoNextMonth()
    {
        // Implement the logic to go to the next month
    }

    void GoPreviousMonth()
    {
        // Implement the logic to go to the previous month
    }

    #endregion
}
