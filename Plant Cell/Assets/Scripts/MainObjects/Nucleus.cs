using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Nucleus : MainCell
{
    private void Awake()
    {
        microViewobject.SetActive(false);
        simulationConfig = FindObjectOfType<SimulationConfig>();
        comunication = FindObjectOfType<Comunication>();
        objectId = -1;
        dividedValue = 2;
        eventName = EventNames.CellNucleusAdded;
        type = this.GetType();
    }
    public void Start()
    {
        if (objectPrefab == null || createPosition == null)
        {
            Debug.LogError(" Cytoplasm : Not enough data to create object.");
        }

        if (Application.isEditor)
        {
            Create("{\"id\": 4}");
        }
    }
    
}

