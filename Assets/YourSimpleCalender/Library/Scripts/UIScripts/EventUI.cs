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

        childGameObject = GetComponentInChildren<TMP_Text>().gameObject;
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
        this.eventDetails = eventDetails;

        eventName.text = eventDetails.EventName;

        eventImage.sprite = eventDetails.EventIcon.sprite;

        border.color = borderColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetEventDetails(eventDetails);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
