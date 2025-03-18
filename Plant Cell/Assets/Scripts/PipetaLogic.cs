using System.Collections;
using UnityEngine;

public class PipetaLogic : MonoBehaviour
{
    [SerializeField] CellAnimationLogic plantCell;
    [SerializeField] GameObject dropPrifab;
    [SerializeField] GameObject spawnPos;
    [SerializeField] GameObject waterEfect;   
    private Vector3 waterEfectStartScale;
    private Comunication comunication;
    private WaterMain waterMain;
    private AnimalCell animalCell;


    private void Awake()
    {
        comunication = FindObjectOfType<Comunication>();
        waterMain = FindObjectOfType<WaterMain>();
        animalCell = FindObjectOfType<AnimalCell>();
    }
    void Start()
    {
        if (waterEfect)
        {
            waterEfectStartScale = waterEfect.transform.localScale;
        }
    }

    public void AddWaterPlantCell() // buttone event
    {
        StartCoroutine(PlantCellAnimation(2));
    }
    public void AddWaterToAnimalCell() // buttone event
    {       
        StartCoroutine(SpaunDropPrifab(3, 1.5f));
        StartCoroutine(ChangeWaterSize(2));
    }

    IEnumerator PlantCellAnimation(float delay)
    {
        comunication.SendEventToPlethora(EventNames.WaterPlantCellTissue);
        StartCoroutine(SpaunDropPrifab(3, 1.5f));
        yield return new WaitForSeconds(delay);      
        plantCell.GetComponent<CellAnimationLogic>().ChangeWaterEFSize();
        comunication.ProcessWaterFlow(waterMain.waterId, true);
    }

    IEnumerator SpaunDropPrifab(int countValue, float delay)
    {
        if (dropPrifab == null || spawnPos == null)
        {
            Debug.LogError("Missing required references for spawning!");
            yield break;
        }
        float currentDeelay;
        int startValue = 0;
        while (startValue < countValue)
        {
            currentDeelay = startValue == 0 ? 0 : delay;
            yield return new WaitForSeconds(currentDeelay);
            Instantiate(dropPrifab, spawnPos.transform.position, dropPrifab.transform.rotation);
            startValue++;
        }
    }

    IEnumerator ChangeWaterSize(float delay)
    {
        comunication.SendEventToPlethora(EventNames.WaterAnimalCellTissue);
        yield return new WaitForSeconds(delay);
        animalCell.MembranePressureNumUpdate(3); 
        
        if (waterEfect)
        {
            waterEfect.GetComponent<ParticleSystem>().Play();

            float elapsedTime = 0f; // Time elapsed since the start of the animation
            float duration = 6f; 
            Vector3 startScale = waterEfectStartScale;
            Vector3 targetScale = new Vector3(0, 0, 0); // Target size

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime; // Increase the elapsed time
                float progress = elapsedTime / duration; // Calculate progress (from 0 to 1)
                waterEfect.transform.localScale = Vector3.Lerp(startScale, targetScale, progress); // Smoothly change the size
                yield return null; 
            }
            waterEfect.transform.localScale = targetScale;
        }
    }
}
