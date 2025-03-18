using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TheCell : MonoBehaviour
{
    public int CellId { get; private set; }
    public bool isCellDied;
    public List<SpriteRenderer> cellEntrails = new List<SpriteRenderer>();
    private float duration = 2f; // Длительность анимации

    public static TheCell Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        CellId = -1;
    }
    void Start()
    {

    }

    public void CreateCell(int id)
    {
        CellId = id;
    }

    public void OnProductGlucose()
    {
        if (!isCellDied)
        {
            SpawnManager.Instance.ObjectsCreateor(ObjectToCreate.glucose, 1);
        }
    }

   
    public void PlantCellDies()
    {
        if (!isCellDied)
        {
            isCellDied = true;
            StartCoroutine(FadeAllSpritesToGray());
        }
    }

    IEnumerator FadeAllSpritesToGray()
    {
        foreach (SpriteRenderer spriteRenderer in cellEntrails)
        {
            StartCoroutine(FadeToGray(spriteRenderer, duration));
            yield return new WaitForSeconds(0.1f); 
        }
    }

    IEnumerator FadeToGray(SpriteRenderer spriteRenderer, float time)
    {
        Color startColor = spriteRenderer.color;
        Color targetColor = Color.gray;

        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            spriteRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor; //  guarantee that the color has definitely changed
    }


}
