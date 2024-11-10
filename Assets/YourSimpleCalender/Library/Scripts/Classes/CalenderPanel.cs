using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalenderPanel : MonoBehaviour
{
   // [SerializeField] private TextMeshProUGUI _monthText;

   [SerializeField] private TMP_Text dayText;
   [SerializeField] private GameObject highlight;
   [SerializeField] private Image background;


   void Awake()
   {
      dayText = GetComponentInChildren<TMP_Text>();
      highlight = transform.Find("Highlight").gameObject;
      background = GetComponent<Image>();
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
}
