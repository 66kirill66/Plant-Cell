using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChloroplastMain : MainCell
{
    private readonly Dictionary<int, GameObject> chloroplastDictionary = new Dictionary<int, GameObject>();
    private readonly List<GameObject> toxicList = new List<GameObject>();

    public GameObject toxicInjectionButtonen;
    public GameObject toxicCreatePosition;
    public GameObject toxicPrifab;
    private void Awake()
    {
        toxicInjectionButtonen.SetActive(false);
        simulationConfig = FindObjectOfType<SimulationConfig>();
        comunication = FindObjectOfType<Comunication>();
        objectId = -1;
        dividedValue = 3f;
        eventName = EventNames.ChloroplastAdded;
        type = this.GetType();
    }
    public void Start()
    {
        if (objectPrefab == null || createPosition == null)
        {
            Debug.LogError(" ChloroplastMain : Not enough data to create object.");
        }

        
        if (Application.isEditor)
        {
            // Calling the Create method with dynamic data
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

        if (chloroplastDictionary.Count < 10)
        {
            CreateChloroplast(data);
        }
        else
        {
            comunication.SendDeleteEntity(data.id);
        }
        toxicInjectionButtonen.SetActive(simulationConfig.displayToxic);
    }

    public void DeleteChloroplast(int id)  // receive from web
    {
        if (chloroplastDictionary.TryGetValue(id, out GameObject chloroplast))
        {
            if (chloroplast != null)
            {
                Destroy(chloroplast, 1);
            }
            chloroplastDictionary.Remove(id);
        }
    }

    private void CreateChloroplast(SimulationData data)
    {
        if (!TheCell.Instance.isCellDied)
        {
            Vector3 place = SpawnManager.Instance.GetChloroplastSpaunPlacesPlace().transform.position;
            GameObject createdObject = Instantiate(objectPrefab, place, objectPrefab.transform.rotation, this.transform);
            createdObject.transform.SetPositionAndRotation(place, Quaternion.Euler(0, 0, UnityEngine.Random.Range(360, 0)));
            createdObject.transform.localScale = new Vector3(1, 1, 1);
            StaticObjectsLogic objLogic = createdObject.AddComponent<StaticObjectsLogic>();
            objLogic.objectId = data.id;
            chloroplastDictionary.Add(data.id, createdObject);
        }
        else { Debug.Log("The Cell is Died"); }
    }
    public void ProductGlucose(string json) // calling from web
    {
        if (!TheCell.Instance.isCellDied)
        {
            SimulationData data = JsonUtility.FromJson<SimulationData>(json);
            GameObject chloroplastPlase = chloroplastDictionary[data.id];
            SpawnManager.Instance.ObjectsCreateor(ObjectToCreate.glucose, 1, chloroplastPlase);
            if (data.dictId != -1)
            {
                SpawnManager.Instance.DeliteObjectFromDict(data.dictId); // and delited carbonDioxide
            }
        }
    }
    public void AddToxic() // buttone
    {
        comunication.SendEventToPlethora(EventNames.ToxicAdded);
        if (toxicList.Count == 0)
        {
            int index = 0;
            for (int i = 0; i < chloroplastDictionary.Count; i++)
            {
                GameObject chloroplast = chloroplastDictionary.ElementAt(index).Value;
                GameObject createdObject = Instantiate(toxicPrifab, toxicCreatePosition.transform.position, objectPrefab.transform.rotation, this.transform);
                toxicList.Add(createdObject);
                StaticObjectsLogic staticObjectsLogic = chloroplast.GetComponent<StaticObjectsLogic>();
                staticObjectsLogic.AttractsToxic(createdObject, chloroplast.transform.position);
                index++;
            }
        }
        else { return; }
    }
}
