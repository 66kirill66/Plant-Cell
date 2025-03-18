using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Membrane : MainCell
{
    private WateringMain wateringMain;
    [SerializeField] GameObject plantCell;
    private CellWall cellWall;

    private string carrentTypeSize;
    public bool membraneRuptures;
    private void Awake()
    {
        microViewobject.SetActive(false);
        simulationConfig = FindObjectOfType<SimulationConfig>();
        comunication = FindObjectOfType<Comunication>();
        cellWall = FindObjectOfType<CellWall>();
        objectId = -1;
        dividedValue = 4;
        eventName = EventNames.MembraneAdded;
        type = this.GetType();
    }
    public void Start()
    {
        if (objectPrefab == null || createPosition == null)
        {
            Debug.LogError(" Membrane : Not enough data to create object.");
        }

        if (Application.isEditor)
        {
            Create("{\"id\": 3}");
        }
    }

    public void Ruptures()
    {
        if (!membraneRuptures)
        {
            if (simulationConfig.viewState == ViewState.MacroView)
            {
                CheckIfHaveWatering();
                wateringMain.MembraneRuptures();
                membraneRuptures = true;
            }
            else if (simulationConfig.viewState == ViewState.AnimalPlantCellsView)
            {
                // no need to reproduce the RuptureAnimation when simjlation in "AnimalPlantCellsView"
                // plantCell.GetComponent<CellAnimationLogic>().RuptureAnimation();
            }
        }
    }

    public void UpdateMembranePressure(int value)
    {
        if(value != 0)
        {
            float currentValue = GetSizeByValue(value);

            if (simulationConfig.viewState == ViewState.MacroView)
            {
                CheckIfHaveWatering();
                wateringMain.ChangeMembraneSize(currentValue);
            }
            else if (simulationConfig.viewState == ViewState.AnimalPlantCellsView)
            {
                plantCell.GetComponent<CellAnimationLogic>().ChangeMembraneSize(currentValue);
            }

            string type = GetPressureTypeByValue(currentValue);
            carrentTypeSize = type;
            Invoke(nameof(UpdatePressureType), 2);
        }
    }

    private void CheckIfHaveWatering()
    {
        if (!wateringMain)
        {
            wateringMain = FindObjectOfType<WateringMain>();
        }
    }

    private void UpdatePressureType()
    {
        comunication.UpdatemMmbranePressureType("Plant Membrane", carrentTypeSize);
    }
}
