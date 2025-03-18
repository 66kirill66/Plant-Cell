using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventConfig", menuName = "Config/EventConfig")]
public class EventConfig : ScriptableObject
{
    public List<EventEntry> events;
}

[System.Serializable]
public class EventEntry
{
    public string eventName;
    public GameObject targetObject;
    public string methodName;
}

