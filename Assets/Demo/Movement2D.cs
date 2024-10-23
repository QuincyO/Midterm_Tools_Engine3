using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))] 
// This is an Attribute in C#, specifically it is one defined by the...
// Unity API called RequireComponent which means this component cannot exist on a...
// GameObject without the other required component type
public class Movement2D : MonoBehaviour,ICalenderAttendee
{
    [SerializeField] // SerializeField is an attribute which says that this variable can be saved,
                     // and will be saved in the scene
    public Rigidbody2D rb;


                     CalenderEvent calenderEvent;
                     
    [Range(0f, 100f)]
    public float moveSpeed = 6;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CalenderEvent e = new CalenderEvent();
        RegisterNotify(e,PlaySound);
    }
    
    public void PlaySound(CalenderEvent e)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector =
            new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
                );

        rb.velocity = inputVector.normalized * moveSpeed;

        Vector3 position;
        position.x = 1;
    }

    public void AddSelfToEvent(CalenderEvent Event)
    {
        Event.AddAttendee(this);
    }

    public void RemoveSelfFromEvent(CalenderEvent Event)
    {
        Event.RemoveAttendee(this);
    }

    public void RegisterNotify(CalenderEvent Event, UnityAction<CalenderEvent> notify)
    {
        Event.RegisterFunction(notify);
    }


    public void UnregisterNotify(CalenderEvent Event, UnityAction<CalenderEvent> notify)
    {
        Event.UnregisterFunction(notify);
    }

}
