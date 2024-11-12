using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Event = Quincy.Calender.Event;

public class CalenderPanel : MonoBehaviour
{
   // [SerializeField] private TextMeshProUGUI _monthText;

   [SerializeField] public TMP_Text dayText {get; private set;}
   [SerializeField] private GameObject highlight;
   [SerializeField] private Image background;
   [SerializeField] private GameObject eventObject;
   [SerializeField] private GameObject eventPrefab;
   [SerializeField] private List<EventUI> events;


   void Awake()
   {
      dayText = GetComponentInChildren<TMP_Text>();
      highlight = transform.Find("Highlight").gameObject;
      background = GetComponent<Image>();
      events = new List<EventUI>();
      eventObject = transform.Find("Events").gameObject;
      events.Add(
         eventObject.transform.GetChild(0).GetComponent<EventUI>()
      );
   }

   public void SetHighlight(bool isHighlighted)
   {
      highlight.SetActive(isHighlighted);
   }
   public void SetDay(int day)
   {
      dayText.text = day.ToString();
   }

   public void MakeUnavailable()
   {
      background.color = new Color(1, .7f, .7f,1);
      dayText.gameObject.SetActive(false);
   }
   public void MakeAvailable()
   {
      background.color = new Color(1, 1, 1,1);
      dayText.gameObject.SetActive(true);
   }

   public void AddEvent(Event e)
   {
      var newEvent = Instantiate(eventPrefab, eventObject.transform);
      newEvent.name = e.EventName;
      var eventUI = newEvent.GetComponent<EventUI>();
      eventUI.SetEventDetails(e);
      events.Add(eventUI);
   }

   public void ClearEvents()
   {
      foreach(var e in events)
      {
         Destroy(e.gameObject);
      }
      events.Clear();
   }
}
