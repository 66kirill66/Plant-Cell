using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cytoplasm : MainCell
{
    private WateringMain wateringMain;
    [SerializeField] GameObject plantCell;

    private string carrentTypeSize;

    private void Awake()
    {
       // DontDestroyOnLoad(gameObject); // Если нужно сохранить между сценами
        microViewobject.SetActive(false);
        simulationConfig = FindObjectOfType<SimulationConfig>();
        comunication = FindObjectOfType<Comunication>();
        objectId = -1;
        dividedValue = 4;
        eventName = EventNames.CytoplasmAdded;
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

    public void UpdateCytoplasmPressure(int value)
    {
        if (value != 0)
        {
            float currentValue = GetSizeByValue(value);
            if (simulationConfig.viewState == ViewState.MacroView)
            {
                if (!wateringMain)
                {
                    wateringMain = FindObjectOfType<WateringMain>();
                }
                wateringMain.ChangeCytoplasmSize(currentValue);
            }
            else if (simulationConfig.viewState == ViewState.AnimalPlantCellsView)
            {
                if(plantCell != null)
                {
                    plantCell.GetComponent<CellAnimationLogic>().ChangeCytoplasmSize(currentValue);
                }
                else
                {
                    Debug.Log("plantCell : not assigned");
                }
            }

            string type = GetPressureTypeByValue(currentValue);
            carrentTypeSize = type;
            Invoke(nameof(UpdatePressureType), 2);
        }
    }

    private void UpdatePressureType()
    {
        comunication.UpdateCytoplasmPressureType(carrentTypeSize);
    }
}
