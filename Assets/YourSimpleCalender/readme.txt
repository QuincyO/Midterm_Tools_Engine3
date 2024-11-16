# YourSimpleCalendar

## Overview

YourSimpleCalendar is a Unity-based calendar system designed to manage events, time progression, and weather effects within your game. It provides a user interface for interacting with the calendar, allowing for easy addition and management of events.

## Features

- **Event Management**: Add, remove, and sort events.
- **Time Progression**: Control the flow of time with adjustable time steps and tick rates.
- **User Interface**: Interactive UI for navigating months, viewing events, and more.

## Installation

1. **Clone the Repository**
  
    Clone the repository

2. **Import into Unity**
    - Open your Unity project.
    - Go to `Assets > Import Package > Custom Package`.
    - Select the downloaded package and import all assets.

## Setup

1. **Add CalendarManager**
    - Drag the `CalendarManager` prefab into your scene.
    - Configure the `CalendarManager` settings in the Inspector.

2. **Create a Calendar**
    - Use the `CalendarManager` to create a new calendar:
      ```csharp
      MyCalendar myCalendar = CalendarManager.CreateCalender("MyCalendarName");
      ```

3. **Display Calendar UI**
    - Ensure the `CalenderUIPrefab` is assigned in the `CalendarManager`.
    - To display the calendar UI:
      ```csharp
      CalendarManager.DisplayCalender(myCalendar);
      ```

## Usage

### Adding Events

You can add events to the calendar using either event details or a `KeyDate` object.

- **Using Event Details**
    ```cs
    myCalendar.AddEvent("Event Name", new Date(month, day, year), endDate, Color.red);
    ```


- **Using KeyDate**
1.  `RightCLick > Create > Scriptable Object Event`
2.  Fill In  
    a)  `Event Name`  
    b)  `Event Color`  
    c)  `Start Date`  
    d)  `Event Icon`  
    e)  add any `Effect Prefabs`  
    
3. If Calendar is attached to game object in hierarchy, add Created Scriptable Object to List of Events. If Calendar is created in code you can add it as
```cs
myCalendar.AddEvent(ScriptableObjectEvent);
```


### Subsribe To Events

You can subsribe to events through code or creating a custom effect using a keydate.

- **Subscribing Manually Through Code**
1. Create a custom script and have it implement
```cs 
ICalendarListener
```

2. Implement the logic you want to occur when the event triggers
3. Anytime Before the Event Occurs you need to set the script to listen for the event.
```cs
MyCalendar calendar = CalendarManager.GetCalendar("CalendarName");

calendar.ListenToEvent("EventName",this);
//or 
calender.ListenToAllEvents(this);
```
4. When the event triggers this will get called
```cs 
public void OnNotify(Quincy.Calendar.Event @event)
{
    //Your Implemented Logic
}
```


- **Creating a Custom Effect to trigger**
1. Create a Custom Script that derives from `EffectScript`
2. Override and implement your logic inside 
```cs
public override void TriggerEffect()
{
    //Your Implemented Logic
}
```
3. Attach this script to a game object
4. Add this GameObject to the `KeyDate` scriptable Objects


### Managing Time

- **Set Time Step**
    ```cs
    CalendarManager.SetTimeStep(5); // Advances time by 5 minutes each tick
    ```


- **Pause/Resume Time**
    ```cs
    CalendarManager.SetPause(true); // Pauses time
    CalendarManager.SetPause(false); // Resumes time
    ```


- **Set Tick Rate**
    ```cs
    CalendarManager.SetTickRate(2.0f); // Sets the tick rate
    ```

### Navigating the Calendar UI

- **Next Month**
    Click the "Next" button in the `CalendarUI` to advance to the next month.

- **Previous Month**
    Click the "Previous" button in the `CalendarUI` to go back to the previous month.

- **Exit Calendar UI**
    Click the "Exit" button to close the calendar interface.

## Code Structure

### Classes

- **CalendarManager**
    - Manages all calendars, events, and time progression.
    - Handles adding/removing calendars and sorting events.

- **MyCalendar**
    - Represents an individual calendar.
    - Manages a list of events.

- **CalendarUI**
    - Handles the user interface for the calendar.
    - Provides methods to update UI elements based on the current state.

### Key Methods

- **CalendarManager**
    - `CreateCalender(string calenderName)`: Creates a new calendar.
    - `DisplayCalender(MyCalendar calender)`: Displays the calendar UI.
    - `AddCalender(MyCalendar calender)`: Adds a calendar to the manager.
    - `SortEvents()`: Sorts all events by date.
    - `SetTimeStep(int minutes)`, `SetPause(bool isPaused)`, `SetTickRate(float rate)`: Manage time progression.

- **MyCalendar**
    - `AddEvent(string eventName, Date eventStartDate, Date? eventEndDate = null, Color color = default)`: Adds a new event.
    - `AddEvent(KeyDate keyDate)`: Adds a new event using a KeyDate object.
    - `RemoveEvent(Event e)`: Removes an event.
    - `GetEventsForMonth(Month month, int year)`: Retrieves events for a specific month and year.

- **CalendarUI**
    - `SetCalender(MyCalendar calender)`: Sets the calendar to be displayed.
    - `UpdateCalenderUI()`: Updates UI elements.
    - `GoNextMonth()`, `GoPreviousMonth()`: Navigate between months.
    - `NewDay(Date newDay)`: Updates UI when a new day begins.

## Customization

- **UI Panels**
    - Customize the `CalendarPanel` components to change how dates and events are displayed.

## Contributing

1. Fork the repository.
2. Create a new branch:
3. Commit your changes:
4. Push to the branch:
5. Open a Pull Request.

