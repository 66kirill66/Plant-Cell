using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MainCell : MonoBehaviour, IUiInteractable
{
    public GameObject objectPrefab;
    public GameObject microViewobject;
    public GameObject cellPosition;
    public GameObject createPosition;

    public SimulationConfig simulationConfig;
    public Comunication comunication;
    //text
    public GameObject textLegend;
    public GameObject objectButtone;
    //event
    protected event Action OnEntityClick;
    public float dividedValue;
    public string eventName;
    public Type type;
    public int uiObjectsCount;
    public int objectId;
    private bool createUI;

    public int uiMicroViewNumber;

    protected void Create(string json)
    {
        StartCoroutine(CreateSemulationObjects(json));
    }
    public IEnumerator CreateSemulationObjects(string json)
    {
        SimulationData data = JsonUtility.FromJson<SimulationData>(json);
        objectId = data.id;
        while (!simulationConfig.setSimulationConfig)
        {
            yield return new WaitForFixedUpdate();
        }
        switch (simulationConfig.viewState)
        {
            case ViewState.CellStructureView:
                HandleCellStructureView(data);
                break;
            case ViewState.MacroView:
                HandleMacroView(data);
                break;
            case ViewState.MicroView:
                HandleMicroView(data);
                break;
        }
    }
    // Virtual methods to override in child class
    protected virtual void HandleCellStructureView(SimulationData data)
    {
        objectButtone.SetActive(true);
        objectButtone.GetComponent<TextManager>().ShowText(simulationConfig.showTooltip);
        EventSubscription();
        uiObjectsCount++;
    }
    protected virtual void HandleMacroView(SimulationData data)
    {
        textLegend.SetActive(true);
        textLegend.GetComponent<TextManager>().ShowText(simulationConfig.showTooltip);
    }
    protected virtual void HandleMicroView(SimulationData data)
    {
        microViewobject.SetActive(true);
        textLegend.SetActive(true);
        textLegend.GetComponent<TextManager>().ShowText(simulationConfig.showTooltip);
        SpawnManager.Instance.SetUiPlace(textLegend, uiMicroViewNumber);
        microViewobject.GetComponent<ObjectData>().objectId = data.id;
    }

    protected void ClickOnUi()
    {
        SendClickOnUi(type, eventName);
        if (!createUI)
        {
            createUI = true;
            GameObject createdObject;
            if (uiObjectsCount == 1)
            {
                createdObject = Instantiate(objectPrefab, createPosition.transform.position, objectPrefab.transform.rotation, this.transform);
                Vector2 startSize = createdObject.transform.localScale;

                Movment moveBehavior = createdObject.AddComponent<Movment>();
                createdObject.transform.localScale /= dividedValue;
                moveBehavior.StartUIMove(cellPosition.transform.position, 40, startSize.x);
                
            }
            else
            {
                uiObjectsCount = Mathf.Max(0, Mathf.Min(uiObjectsCount, 5));
                for (int i = 0; i < uiObjectsCount; i++)
                {
                    Quaternion randomZ = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
                    createdObject = Instantiate(objectPrefab, createPosition.transform.position, randomZ, this.transform);
                    Movment moveBehavior = createdObject.AddComponent<Movment>();
                    // Set final position
                    int childIndex = Mathf.Min(i, cellPosition.transform.childCount - 1);
                    Vector3 targetPosition = cellPosition.transform.GetChild(childIndex).position;
                    createdObject.transform.localScale /= 3;
                    moveBehavior.StartUIMove(targetPosition, 40, 0.7f);
                }
            }
        }
        else
        {
            return;
        }
    }

    protected void EventSubscription()
    {
        OnEntityClick += ClickOnUi;
    }

    public void ClickOnUiObject() // buttone 
    {
        OnEntityClick?.Invoke();
    }



    protected void SendClickOnUi(Type type, string eventName)
    {
        Debug.Log($"Object type: {type}");

        simulationConfig.SetSimulationConfigValues(type.ToString(), 1);
        comunication.SendEventToPlethora(eventName);
    }
    protected float GetSizeByValue(int value)
    {
        float currentValue = 1;
        if (value != 0)
        {
            if (value > 1)
            {
                if (value == 2)
                {
                    currentValue = 1f;
                }
                if (value == 3)
                {
                    currentValue = 1.85f;
                }
            }
            else if (value == 1 && simulationConfig.lowPressureSupport)
            {
                currentValue = 0.8f;
            }
        }
        return currentValue;
    }

    protected string GetPressureTypeByValue(float value)
    {
        string pressureType = "Low";
        switch (value)
        {
            case 0.8f:
                pressureType = "VeryLow";
                break;
            case 1.2f:
                pressureType = "Low";
                break;
            case 1.85f:
                pressureType = "VeryHigh";
                break;
        }
        return pressureType;
    }

    public void OnDestroy()
    {
        OnEntityClick -= ClickOnUiObject;
    }
}
