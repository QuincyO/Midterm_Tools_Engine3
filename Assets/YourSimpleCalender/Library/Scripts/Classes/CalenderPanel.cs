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


   void Awake()
   {
      dayText = GetComponentInChildren<TMP_Text>();
      highlight = transform.Find("Highlight").gameObject;
   }



   public void SetHighlight(bool isHighlighted)
   {
      highlight.SetActive(isHighlighted);
   }
   public void SetDay(int day)
   {
      dayText.text = day.ToString();
   }
}
