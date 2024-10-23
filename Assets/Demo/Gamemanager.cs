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
        TimeManager.Initialize();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
