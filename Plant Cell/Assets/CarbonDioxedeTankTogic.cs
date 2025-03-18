using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonDioxedeTankTogic : MonoBehaviour
{
    [SerializeField] GameObject tankEF;
    [SerializeField] GameObject cellEF;
    [SerializeField] GameObject tankHandle;
    [SerializeField] SpriteRenderer tankSprite;
    private CarbonDioxide carbonDioxide;
    private Comunication comunication;

    private Vector2 handleEnd;
    private void Awake()
    {
        carbonDioxide = FindObjectOfType<CarbonDioxide>();
        comunication = FindObjectOfType<Comunication>();
        handleEnd = new Vector2(-76f, -15.9f);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tankHandle.transform.position = Vector2.Lerp(tankHandle.transform.position, handleEnd, 5 * Time.deltaTime);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CarbonDioxideEF();
        }
    }

    public void CarbonDioxideEF()
    {
        StartCoroutine(StartCarbonDioxideEF());
    }

    IEnumerator StartCarbonDioxideEF() // particles animation(in the bell)
    {
        tankSprite.color = Color.grey;
        yield return new WaitForSeconds(0.3f);
        tankSprite.color = Color.white;
        yield return new WaitForSeconds(1);
        tankEF.SetActive(true);
        handleEnd.x = -74f;
        yield return new WaitForSeconds(2);
        cellEF.SetActive(true);
        comunication.SendEventToPlethora(EventNames.AddCarbonDioxide);
        yield return new WaitForSeconds(5);
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f);
            carbonDioxide.CreateCarbonDioxide(1);
            comunication.CdEntersToCell(carbonDioxide.carbonDioxideId);
        }
        tankEF.SetActive(false);
        handleEnd.x = -76f;
        cellEF.SetActive(false);       
    }
}
