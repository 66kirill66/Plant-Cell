using System.Collections;
using UnityEngine;

public class WateringMain : MonoBehaviour
{
    public GameObject cellsMain;
    [SerializeField] GameObject waterCan;
    [SerializeField] GameObject plant;
    [SerializeField] GameObject evaporationEF;
    private bool isWatering;
    private bool isEvaporation;
    private Comunication comunication;
    private WaterMain waterMain;

    private SimulationConfig simulationConfig;
    private Membrane membrane;

    public float evaporationTimer;
    private bool doubleEvaporation;
    private void Awake()
    {
        evaporationTimer = 8;
        comunication = FindObjectOfType<Comunication>();
        waterMain = FindObjectOfType<WaterMain>();
        simulationConfig = FindObjectOfType<SimulationConfig>();
        membrane = FindObjectOfType<Membrane>();
    }
    void Start()
    {
        StartCoroutine(SetEvaporationTimer());
    }

    IEnumerator SetEvaporationTimer()
    {
        while (!simulationConfig.setSimulationConfig)
        {
            yield return new WaitForFixedUpdate();
        }
        evaporationTimer = simulationConfig.EvaporationTimer;
    }

    private void Update()
    {
        HandleEvaporation();
    }
    private void HandleEvaporation()
    {
        if (evaporationTimer > 0)
        {
            evaporationTimer -= Time.deltaTime;

            if (!isEvaporation && evaporationTimer <= 0)
            {
                isEvaporation = true;
                EvaporationStart();
            }
        }
    }
    private void StopEvaporation()
    {
        StopCoroutine(EvaporationEf());
        isEvaporation = false;
        evaporationEF.SetActive(false);
    }

    private void EvaporationStart()
    {
        StartCoroutine(EvaporationEf());
    }

    IEnumerator EvaporationEf()
    {       
        evaporationEF.SetActive(true);
        yield return new WaitForSeconds(2);
        comunication.ProcessWaterFlow(waterMain.waterId, false);
        yield return new WaitForSeconds(8);
        float newDeley = 0;
        if (doubleEvaporation)
        {
            comunication.ProcessWaterFlow(waterMain.waterId, false);
            newDeley = 5;
        }       
        yield return new WaitForSeconds(newDeley);
        evaporationEF.SetActive(false);
        isEvaporation = false;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isWatering )
        {
            StartWateringPlant();
        }
    }

    public void StartWateringPlant()
    {
        StopEvaporation();
        doubleEvaporation = simulationConfig.lowPressureSupport;
        evaporationTimer = 10;
        isWatering = true;
        comunication.SendEventToPlethora(EventNames.WateringPlant);
        waterCan.GetComponent<Animator>().SetTrigger("watering");
        StartCoroutine(StartAddWaterAnimation());
    }

    public void IsWateringUpdate() // animation event in plant animation
    {
        isWatering = false;
    }

    public void SetPlantAnimation(bool hasWater)
    {
        if (hasWater)
        {
            isEvaporation = false;
            plant.GetComponent<Animator>().SetBool("With water", true);
           
        }
        else
        {
            plant.GetComponent<Animator>().SetBool("With water", false);

        }
    }
    
    
    private IEnumerator StartAddWaterAnimation()
    {
        yield return new WaitForSeconds(2);

        foreach (Transform objectTransform in cellsMain.transform)
        {
            var cellLogic = objectTransform.GetComponent<CellAnimationLogic>();
            if (cellLogic != null)
            {
                cellLogic.ChangeWaterEFSize();
            }
        }

        yield return new WaitForSeconds(2);
        comunication.ProcessWaterFlow(waterMain.waterId, true);

    }

    public void WallChangeSize(int value)
    {
        if (!membrane.membraneRuptures)
        {
            foreach (Transform objectTransform in cellsMain.transform)
            {
                objectTransform.GetComponent<CellAnimationLogic>().CellWallChangeSize(value);
            }
        }
    }

    public void VacuoleSize(int value)
    {
        foreach (Transform objectTransform in cellsMain.transform)
        {
            objectTransform.GetComponent<CellAnimationLogic>().ChangeVacuoleSize(value);
        }
    }

    public void ChangeCytoplasmSize(float Value)
    {
        if (!membrane.membraneRuptures)
        {
            foreach (Transform objectTransform in cellsMain.transform)
            {
                objectTransform.GetComponent<CellAnimationLogic>().ChangeCytoplasmSize(Value);
            }
        }
    }

    public void ChangeMembraneSize(float Value)
    {
        if (!membrane.membraneRuptures)
        {
            foreach (Transform objectTransform in cellsMain.transform)
            {
                objectTransform.GetComponent<CellAnimationLogic>().ChangeMembraneSize(Value);
            }
        }
    }

    public void MembraneRuptures()
    {
        
        foreach (Transform objectTransform in cellsMain.transform)
        {
            objectTransform.GetComponent<CellAnimationLogic>().RuptureAnimation();
        }
        plant.GetComponent<Animator>().SetTrigger("Died");
    }

}
