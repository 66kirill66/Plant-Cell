using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


public enum ViewState
{
    CellStructureView,
    MacroView,
    AnimalPlantCellsView,
    MicroView

}
public class SimulationConfig : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void UpdateSimulationConfigValues(int id, string name, int value);


    public ViewState viewState;
    public bool showTooltip;
    [SerializeField] GameObject waterCan;
    public bool lowPressureSupport;
    public float EvaporationTimer { get; private set; }

    public bool setSimulationConfig;
    public GameObject cellStructureView;
    public GameObject cellsMacroView;
    public GameObject animalPlantCellsView;
    public GameObject cellsMicroView;
    public bool displayToxic;



    private int configId;

    public int glucoseCuont;
    public int carbonDioxideCuont;
    private void Awake()
    {
       
        configId = -1;
        setSimulationConfig = false;
       // lowPressureSupport = true;


        //viewState = ViewState.cellStructure;

        if (Application.isEditor)
        {
           // CreateSimulationConfig("{\"id\": 21}");
           //  CreateSimulationConfig("{\"id\": 21,\"numOfGlucose\": 0, \"viewType\": \"MicroView\", \"displayTooltip\": true}"); 

        }
    }
    

    void Start()
    {

    }

    public void SetSimulationConfigValues(string name, int booleanValue)
    {
        Debug.Log(name);
        if(configId != -1 && !Application.isEditor)
        {
            UpdateSimulationConfigValues(configId, name, booleanValue);
        }
    }


    public void CreateSimulationConfig(string json)
    {
        cellStructureView.SetActive(false);
        cellsMicroView.SetActive(false);
        cellsMacroView.SetActive(false);
        animalPlantCellsView.SetActive(false);
        waterCan.SetActive(false);

      
        SimulationData data = JsonUtility.FromJson<SimulationData>(json);
        lowPressureSupport = data.veryLowPressureSupport;
        EvaporationTimer = data.evaporationTimer;
        showTooltip = data.displayTooltip;
        configId = data.id;
        displayToxic = data.displayToxic;
        glucoseCuont = data.numOfGlucose;
        


        switch (data.viewType)
        {
            case "CellStructureView":
                viewState = ViewState.CellStructureView;
                cellStructureView.SetActive(true);
                break;
            case "MacroView":
                waterCan.SetActive(data.displayWaterCan);
                viewState = ViewState.MacroView;
                cellsMacroView.SetActive(true);
                break;
            case "AnimalPlantCellsView":
                viewState = ViewState.AnimalPlantCellsView;
                animalPlantCellsView.SetActive(true);
                break;
            case "MicroView":
                viewState = ViewState.MicroView;
                cellsMicroView.SetActive(true);
                break;
            default:
                break;
        }
        setSimulationConfig = true;
    }
}
