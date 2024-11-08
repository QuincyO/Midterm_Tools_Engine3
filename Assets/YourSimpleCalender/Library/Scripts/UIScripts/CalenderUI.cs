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

        for (int i = 0; i < calenderPanels.Length; i++)
        {

            var date = i % focusedDate.MaxDays + 1;
            calenderPanels[i].SetDay(date);
        }
        UpdateCalenderUI();
    }

    void UpdateCalenderUI()
    {
        yearText.text = focusedDate.Year.ToString();
        //dayText.text = focusedDate.Day.ToString();
        monthText.text = focusedDate.Month.ToString();
        calenderPanels[focusedDate.Day - 1].SetHighlight(true);
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
