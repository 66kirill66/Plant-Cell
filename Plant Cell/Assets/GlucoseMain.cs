using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlucoseMain : MonoBehaviour
{
    public Transform[] tomatoes; // Tomatoes (must be assigned in the inspector)
    private SimulationConfig simulationConfig;
    public GameObject textLegend;
    public int GlucoseId { get; set; }
    public int GlucoseCount { get; set; }
    private readonly float minTomatoesSize = 0.2f; 
    private readonly float maxTomatoesSize = 0.5f; 
    public float maxGlucoseValue = 40f; // Maximum growth value

    private float currentGrowthValue = 0f;


    private void Awake()
    {
        GlucoseCount = 0;
        simulationConfig = FindObjectOfType<SimulationConfig>();
    }
    void Start()
    {
        SpawnManager.Instance.GlucoseCountUpdate += UpdateGlucoseCount;
        if (Application.isEditor)
        {
            Create("{\"id\": 1}");
        }
    }


    void Update()
    {

    }

    public void UpdateGlucoseCount()
    {
        if(GlucoseCount < maxGlucoseValue)
        {
            GlucoseCount++;
            UpdateGrowth(GlucoseCount);
        }
    }
    public void UpdateGrowth(float newGrowthValue)
    {
        currentGrowthValue = Mathf.Clamp(newGrowthValue, 0, maxGlucoseValue);
        UpdateTomatoSizes();
    }
    private void UpdateTomatoSizes()
    {
        // Divide the height by the number of tomatoes
        float growthPerTomato = maxGlucoseValue / tomatoes.Length;
        for (int i = 0; i < tomatoes.Length; i++)
        {
            // Determine the growth rate for each tomato
            float tomatoGrowthValue = Mathf.Clamp(currentGrowthValue - (growthPerTomato * i), 0, growthPerTomato);
            float growthPercent = tomatoGrowthValue / growthPerTomato;
            // Calculate the current size of the tomato
            float newSize = Mathf.Lerp(minTomatoesSize, maxTomatoesSize, growthPercent);
            // Apply the size
            tomatoes[i].localScale = new Vector3(newSize, newSize, newSize);
        }
    }

    public void Create(string json)
    {
        SimulationData data = JsonUtility.FromJson<SimulationData>(json);
        GlucoseId = data.id;
        StartCoroutine(CreateSemulationObjects());
    }
    public IEnumerator CreateSemulationObjects()
    {
        while (!simulationConfig.setSimulationConfig)
        {
            yield return new WaitForFixedUpdate();
        }
        if(simulationConfig.viewState == ViewState.MicroView)
        {
            textLegend.SetActive(true);
            textLegend.GetComponent<TextManager>().ShowText(simulationConfig.showTooltip);
            SpawnManager.Instance.SetUiPlace(textLegend, 9);
            int glucoseAmount = simulationConfig.glucoseCuont;
            SpawnManager.Instance.ObjectsCreateor(ObjectToCreate.glucose, glucoseAmount);
        }
    }
}
