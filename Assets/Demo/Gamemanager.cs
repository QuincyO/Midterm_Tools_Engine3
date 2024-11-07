using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    // Start is called before the first frame update
    
     MyCalender _myCalender;

    private void Awake()
    {
        _myCalender = CalenderManager.CreateCalender("Birthdays");
    }

    void Start()
    {
        _myCalender.AddEvent("1 Birthday", new Date(2024,Month.January,29,12,0));
        _myCalender.AddEvent("2 Birthday", new Date(2024,Month.January,28,12,0));
        _myCalender.AddEvent("3 Birthday", new Date(2024,Month.January,27,12,0));
        _myCalender.AddEvent("4 Birthday", new Date(2024,Month.January,26,12,0));
        _myCalender.AddEvent("5 Birthday", new Date(2024,Month.January,25,12,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
