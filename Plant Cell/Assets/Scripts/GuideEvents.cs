using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideEvents : MonoBehaviour
{
    public GameObject handPng;
    public GameObject guidePlaces;
    private Comunication comunication;
    private GameObject eventObjectToShow;
    [SerializeField] PipetaLogic animalCellPipeta;
    [SerializeField] PipetaLogic plantCellPipeta;
    

    private readonly float speed = 2f;
    private Vector3 startHandPosition;
    private Coroutine currentCoroutine;

    private Dictionary<string, Action> eventHandlers;



    private void Awake()
    {
        comunication = FindObjectOfType<Comunication>();

        startHandPosition = handPng.transform.position;

        eventHandlers = new Dictionary<string, Action>
        {
            { EventNames.MembraneAdded, () => GetInteractable<Membrane>()?.ClickOnUiObject() },
            { EventNames.CytoplasmAdded, () => GetInteractable<Cytoplasm>()?.ClickOnUiObject() },
            { EventNames.CellNucleusAdded, () => GetInteractable<Nucleus>()?.ClickOnUiObject() },
            { EventNames.FiveMitochondriaAdded, () => GetInteractable<Mitochondrion>()?.ClickOnUiObject() },
            { EventNames.CellWallAdded, () => GetInteractable<CellWall>()?.ClickOnUiObject() },
            { EventNames.ChloroplastAdded, () => GetInteractable<ChloroplastMain>()?.ClickOnUiObject() },
            { EventNames.ToxicAdded, () => GetInteractable<ChloroplastMain>()?.AddToxic() },
            { EventNames.VacuoleAdded, () => GetInteractable<VacuoleMain>()?.ClickOnUiObject() },
            { EventNames.WateringPlant, () => FindObjectOfType<WateringMain>()?.StartWateringPlant() },
            { EventNames.AddLight, () => FindObjectOfType<LightMain>()?.ChancheLightStatus() },
            { EventNames.AddCarbonDioxide, () => FindObjectOfType<CarbonDioxedeTankTogic>()?.CarbonDioxideEF() },
            { EventNames.WaterPlantCellTissue, () => plantCellPipeta?.GetComponent<PipetaLogic>()?.AddWaterPlantCell() },
            { EventNames.WaterAnimalCellTissue, () => animalCellPipeta?.GetComponent<PipetaLogic>()?.AddWaterToAnimalCell() }
        };
    }

    private void Start()
    {
        
    }

    private T GetInteractable<T>() where T : MonoBehaviour, IUiInteractable
    {
        var interactable = FindObjectOfType<T>();
        if (interactable == null)
        {
            Debug.LogWarning($"Interactable of type {typeof(T).Name} not found in the scene!");
        }
        return interactable;
    }

    public void OnUserEventRequest(string eventName)
    {
        if (eventHandlers.TryGetValue(eventName, out var action))
        {
            action?.Invoke();
            comunication.SendEventToPlethora(eventName);
        }
        else
        {
            Debug.LogWarning($"Event {eventName} not assigned!");
        }
    }


    public void ShowColor(string placeName)
    {
        Transform child = guidePlaces.transform.Find(placeName);
        StopCoroutine();
        if (child != null)
        {
            eventObjectToShow = child.gameObject;
            eventObjectToShow.SetActive(true);          
            handPng.SetActive(true);
            currentCoroutine = StartCoroutine(SetHandPosition(child));
        }
        else
        {
            Debug.LogError($"Guide place '{placeName}' not found.");
        }
    }
    public void NoColor()
    {
        if (eventObjectToShow)
        {
            eventObjectToShow.SetActive(false);
        }
    }
    //public void OnUserEventRequest(string eventName)
    //{
    //    switch (eventName)
    //    {
    //        case "Membrane Added":
    //            FindObjectOfType<Membrane>().ClickOnUiObject();
    //            break;
    //        case "Cytoplasm Added":
    //            FindObjectOfType<Cytoplasm>().ClickOnUiObject();
    //            break;
    //        case "Cell Nucleus Added":
    //            FindObjectOfType<Nucleus>().ClickOnUiObject();
    //            break;
    //        case "Five Mitochondria Added":
    //            FindObjectOfType<Mitochondrion>().ClickOnUiObject();
    //            break;
    //        case "Cell Wall Added":
    //            FindObjectOfType<CellWall>().ClickOnUiObject();
    //            break;
    //        case "Chloroplast Added":
    //            FindObjectOfType<ChloroplastMain>().ClickOnUiObject();
    //            break;
    //        case "Vacuole Added":
    //            FindObjectOfType<VacuoleMain>().ClickOnUiObject();
    //            break;
    //        case "Watering Plant":
    //            FindObjectOfType<WateringMain>().StartWateringPlant();
    //            break;
    //        case "Add Light":
    //            FindObjectOfType<LightMain>().ChancheLightStatus();
    //            break;
    //        case "Add CD":
    //            FindObjectOfType<CarbonDioxedeTankTogic>().CarbonDioxideEF();
    //            break;            
    //        case "Water Plant Cell Tissue":
    //            if (plantCellPipeta)
    //            {
    //                plantCellPipeta.AddWaterPlantCell();
    //            }
    //            else { Debug.Log("plantCellPipeta : not assigned"); }
    //            break;
    //        case "Water Animal Cell Tissue":
    //            if (animalCellPipeta)
    //            {
    //                animalCellPipeta.AddWaterToAnimalCell();
    //            }
    //            else { Debug.Log("animalCellPipeta : not assigned"); }
    //            break;
    //        default:
    //            break;
    //    }
    //    comunication.SendEventToPlethora(eventName);
    //}
    public void BackToStart()
    {
        StopCoroutine();
        currentCoroutine = StartCoroutine(Back());
    }
    private void StopCoroutine()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }
    public IEnumerator SetHandPosition(Transform position)
    {
        Vector3 to = position.transform.position;
        while (Vector3.Distance(handPng.transform.position, to) > 0.001f)
        {
            handPng.transform.position = Vector3.Lerp(handPng.transform.position, to, speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForEndOfFrame();
    }
    public IEnumerator Back()
    {
        handPng.SetActive(false);
        while (Vector3.Distance(handPng.transform.position, startHandPosition) > 0.001f)
        {
            handPng.transform.position = Vector3.Lerp(handPng.transform.position, startHandPosition, 1);
            yield return null;
        }
        yield return new WaitForEndOfFrame();
    }
}
