using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject mitochondrionSpawnPlaces;
    public GameObject chloroplastSpawnPlaces;
    public GameObject startRandomPoint;
    [SerializeField] GameObject glucosePrefab;
    [SerializeField] GameObject carbonDioxiderefab;
    public Dictionary<int, GameObject> objectsDict = new Dictionary<int, GameObject>();
    private int startId;

    public event Action GlucoseCountUpdate;
    private GlucoseMain glucoseMain;
    private CarbonDioxide carbonDioxide;



    public List<GameObject> uiPositionsIndexes = new List<GameObject>();

    public static SpawnManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        glucoseMain = FindObjectOfType<GlucoseMain>();
        carbonDioxide = FindObjectOfType<CarbonDioxide>();
    }
    void Start()
    {
       
    }


    public void SetUiPlace(GameObject textLegend, int placeIndexId)
    {
        RectTransform rectTransform = textLegend.GetComponent<RectTransform>();
        
        rectTransform.anchoredPosition = uiPositionsIndexes[placeIndexId].transform.position;
    }

    public void GlucoseCreatedEvent()
    {
        GlucoseCountUpdate?.Invoke();
    }

    public GameObject GetMitochondrionPlace()
    {
        foreach (Transform child in mitochondrionSpawnPlaces.transform)
        {
            if (child.gameObject.activeSelf) 
            {
                child.gameObject.SetActive(false); 
                return child.gameObject; 
            }
        }

        return null; 
    }

    public GameObject GetChloroplastSpaunPlacesPlace()
    {
        foreach (Transform child in chloroplastSpawnPlaces.transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
                return child.gameObject;
            }
        }
        return null;
    }



    public GameObject GetRandomPlace(float radius = 10)
    {
        //Get the position of the starting point
        Vector3 startPosition = startRandomPoint.transform.position;

        // Generate a random offset inside a circle of radius 10
        Vector3 randomOffset = UnityEngine.Random.insideUnitCircle * radius;

        // Calculate a random position
        Vector3 randomPosition = startPosition + randomOffset;
        GameObject randomPlace = new GameObject("RandomPlace");
        randomPlace.transform.position = randomPosition;
        GameObject.Destroy(randomPlace, 5f);

        return randomPlace;
    }

    public void ObjectsCreateor(ObjectToCreate objName, int amount, GameObject pos = null)
    {
        
        GameObject objectToCreate = objName == ObjectToCreate.glucose ? glucosePrefab : carbonDioxiderefab;
        int objId = objName == ObjectToCreate.glucose ? glucoseMain.GlucoseId : carbonDioxide.carbonDioxideId;
        for (int i = 0; i < amount; i++)
        {
            if (!pos)
            {
                pos = GetRandomPlace();
            }
            GameObject createdObject = Instantiate(objectToCreate, pos.transform.position, glucosePrefab.transform.rotation, this.transform);
            
            Movment move = createdObject.AddComponent<Movment>();
            move.CreatedAnimation();
           // move.isMove = true;
            move.objectId = objId;
            move.dictId = startId;
            objectsDict.Add(startId, createdObject);
            startId++;
            pos = null;
            if (objName == ObjectToCreate.glucose)
            {
                GlucoseCreatedEvent();
            }
        }
    }
    

    public void DeliteObjectFromDict(int objId)
    {
        if (objectsDict.ContainsKey(objId)) 
        {
            GameObject dictObject = objectsDict[objId];
            dictObject.TryGetComponent(out Movment movement);
            movement.isMove = false;
            Destroy(dictObject, 0.3f); // Удаляем объект из сцены с задержкой
            objectsDict.Remove(objId); // Удаляем объект из словаря
        }
        else
        {
            Debug.LogWarning($"Object with ID {objId} not found in the dictionary!");
        }

    }
}
