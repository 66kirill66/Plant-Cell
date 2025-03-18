using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuoleMain : MainCell
{
    private WateringMain wateringMain;
    [SerializeField] GameObject plantCell;
    private int currentSize = 2;

    private void Awake()
    {
        microViewobject.SetActive(false);
        simulationConfig = FindObjectOfType<SimulationConfig>();
        comunication = FindObjectOfType<Comunication>();
        
        objectId = -1;
        dividedValue = 2;
        eventName = EventNames.VacuoleAdded;
        type = this.GetType();
    }
    public void Start()
    {
        if (objectPrefab == null || createPosition == null)
        {
            Debug.LogError("VacuoleMain : Not enough data to create object.");
        }

        if (Application.isEditor)
        {
            Create("{\"id\": 9}");
        }
    }

    public void OnVacuoleChaneSize(int value)
    {
        int minValue = simulationConfig.lowPressureSupport ? 1 : 2;
        if (value == -1)
        {
            currentSize -= 1;
        }
        else
        {
            currentSize += 1;
        }
       // currentSize = Mathf.Max(minValue, Mathf.Min(currentSize, 3));
        currentSize = Mathf.Max(1, Mathf.Min(currentSize, 3));
        if (simulationConfig.viewState == ViewState.MacroView)
        {
            if (!wateringMain)
            {
                wateringMain = FindObjectOfType<WateringMain>();
            }
            wateringMain.VacuoleSize(currentSize);
        }
        else if (simulationConfig.viewState == ViewState.AnimalPlantCellsView)
        {
            if(plantCell != null)
            {
                plantCell.GetComponent<CellAnimationLogic>().ChangeVacuoleSize(currentSize);
            }
            else
            {
                Debug.Log("plantCell : not assigned");
            }
        }
    }
}
