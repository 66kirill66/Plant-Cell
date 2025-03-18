using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonDioxide : MonoBehaviour
{
    public int carbonDioxideId;
    public GameObject carbonDioxideTank;
    public GameObject textLegend;
    private SpawnManager spawnManager;
    private SimulationConfig simulationConfig;
    private void Awake()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        simulationConfig = FindObjectOfType<SimulationConfig>();
        carbonDioxideTank.SetActive(false);
    }
    void Start()
    {
        if (Application.isEditor)
        {
            Create(5);
        }
    }

    public IEnumerator CreateSemulationObjects()
    {
        while (!simulationConfig.setSimulationConfig)
        {
            yield return new WaitForFixedUpdate();
        }
        if (simulationConfig.viewState == ViewState.MicroView)
        {
            carbonDioxideTank.SetActive(true);
            textLegend.SetActive(true);
            textLegend.GetComponent<TextManager>().ShowText(simulationConfig.showTooltip);
            SpawnManager.Instance.SetUiPlace(textLegend, 8);
        }
    }

    public void Create(int id)
    {
        carbonDioxideId = id;
        StartCoroutine(CreateSemulationObjects());
    }

    public void CreateCarbonDioxide(int amount)
    {
        if (!TheCell.Instance.isCellDied)
        {
            GameObject pos = spawnManager.GetRandomPlace(10);
            spawnManager.ObjectsCreateor(ObjectToCreate.carbonDioxide, amount, pos);
        }      
    } 
}
