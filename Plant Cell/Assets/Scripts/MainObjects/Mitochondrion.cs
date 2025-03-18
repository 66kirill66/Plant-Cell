using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class Mitochondrion : MainCell
{
    private readonly Dictionary<int, GameObject> mitochondrionDictionary = new Dictionary<int, GameObject>();
    private void Awake()
    {
        simulationConfig = FindObjectOfType<SimulationConfig>();
        comunication = FindObjectOfType<Comunication>();
        objectId = -1;
        dividedValue = 3f;
        eventName = EventNames.FiveMitochondriaAdded;
        type = this.GetType();
    }
    public void Start()
    {
        if (objectPrefab == null || createPosition == null)
        {
            Debug.LogError(" Mitochondrion : Not enough data to create object.");
        }

        simulationConfig = FindObjectOfType<SimulationConfig>();
        if (Application.isEditor)
        {
            Create("{\"id\": 5}");
            Create("{\"id\": 6}");
            Create("{\"id\": 7}");
            Create("{\"id\": 8}");
            Create("{\"id\": 9}");
        }

    }

    protected override void HandleMicroView(SimulationData data)
    {
        textLegend.SetActive(true);
        textLegend.GetComponent<TextManager>().ShowText(simulationConfig.showTooltip);
        SpawnManager.Instance.SetUiPlace(textLegend, uiMicroViewNumber);
        // get position
        Vector3 place = SpawnManager.Instance.GetMitochondrionPlace().transform.position;
       
        GameObject createdObject = Instantiate(objectPrefab, transform.position, objectPrefab.transform.rotation, this.transform);
        createdObject.transform.SetPositionAndRotation(place, Quaternion.Euler(0, 0, UnityEngine.Random.Range(360, 0)));
        createdObject.transform.localScale = new Vector3(1, 1, 1);
        StaticObjectsLogic objLogic = createdObject.AddComponent<StaticObjectsLogic>();
        objLogic.objectId = data.id;
        mitochondrionDictionary.Add(data.id, createdObject);
    }
}
