using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine;

public class RandomController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            //CalenderManager.DisplayCalender(Calender);
        }
    }
}
