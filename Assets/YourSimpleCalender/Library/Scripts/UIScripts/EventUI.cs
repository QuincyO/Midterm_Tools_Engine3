using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour
{

    [SerializeField] private TMP_Text eventName;
    [SerializeField] private Image eventImage;
    [SerializeField] Image border;
    [SerializeField] Color borderColor;

    [SerializeField] private Quincy.Calender.Event eventDetails; 

     void Awake()
    {
        GameObject childGameObject;
        borderColor = Color.white;

        border = GetComponent<Image>();

        childGameObject = transform.Find("EventText").gameObject;
        if(childGameObject.name == "EventText")
        {
            eventName = childGameObject.GetComponent<TMP_Text>();
        }
        else Debug.LogError("Event Text not found");

        childGameObject = transform.Find("EventIcon").gameObject;
        if(childGameObject.name == "EventIcon")
        {
            eventImage = childGameObject.GetComponent<Image>();
        }
        else Debug.LogError("Event Image not found");


    }

    public void SetEventDetails(Quincy.Calender.Event eventDetails)
    {
        if (eventDetails.EventName == "") return;
        this.eventDetails = eventDetails;
        var date = eventDetails.startingDate;

        eventName.text =
         $"{date.FormattedHour}:{date.FormattedMinutes}{date.Period} " +
         $"{eventDetails.EventName}";

        if (eventDetails.EventIcon == null) eventImage.color = Color.clear;
        else eventImage.sprite = eventDetails.EventIcon.sprite;

        border.color = eventDetails.EventColor;
    }
    


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
