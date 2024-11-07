using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine;
using UnityEngine.Events;
using Event = Quincy.Calender.Event;

[RequireComponent(typeof(Rigidbody2D))] 
// This is an Attribute in C#, specifically it is one defined by the...
// Unity API called RequireComponent which means this component cannot exist on a...
// GameObject without the other required component type
public class Movement2D : MonoBehaviour
{
    [SerializeField] // SerializeField is an attribute which says that this variable can be saved,
                     // and will be saved in the scene
    public Rigidbody2D rb;


                     
    [Range(0f, 100f)]
    public float moveSpeed = 6;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Event e = new Event();
        RegisterNotify(e,PlaySound);
    }
    
    public void PlaySound(Event e)
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

    public void AddSelfToEvent(Event Event)
    {
    }

    public void RemoveSelfFromEvent(Event Event)
    {
    }

    public void RegisterNotify(Event Event, UnityAction<Event> notify)
    {
      //  Event.RegisterFunction(notify);
    }


    public void UnregisterNotify(Event Event, UnityAction<Event> notify)
    {
        //Event.UnregisterFunction(notify);
    }

    public void OnNotify()
    {
        throw new NotImplementedException();
    }
}
