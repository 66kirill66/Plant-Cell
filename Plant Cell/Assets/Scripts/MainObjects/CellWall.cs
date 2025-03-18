using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWall : MainCell
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
        dividedValue = 4;
        eventName = EventNames.CellWallAdded;
        type = this.GetType();


        
    }
    public void Start()
    {
        

        if (objectPrefab == null || createPosition == null)
        {
            Debug.LogError(" CellWall : Not enough data to create object.");
        }

        if (Application.isEditor)
        {
            Create("{\"id\": 1}");
        }

    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{

        //    OnCellWallChangeSize(1);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{

        //    OnCellWallChangeSize(-1);
        //}
    }

    public void OnCellWallChangeSize(int value)
    {
        int minValue = simulationConfig.lowPressureSupport ? 2 : 1;
        currentSize = value;
        currentSize = Mathf.Max(minValue, Mathf.Min(currentSize, 3));

        if (simulationConfig.viewState == ViewState.MacroView)
        {
            if (!wateringMain)
            {
                wateringMain = FindObjectOfType<WateringMain>();
            }
            wateringMain.SetPlantAnimation(currentSize == 3);
            wateringMain.WallChangeSize(currentSize);
        }
        else if (simulationConfig.viewState == ViewState.AnimalPlantCellsView)
        {
            if (plantCell != null)
            {
                plantCell.GetComponent<CellAnimationLogic>().CellWallChangeSize(currentSize);
            }
            else
            {
                Debug.Log("plantCell : not assigned");
            }
        }
    }
}
