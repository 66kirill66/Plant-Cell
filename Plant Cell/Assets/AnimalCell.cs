using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCell : MonoBehaviour
{
    public int animalMembraneId;
    public int animalCellId;
    [SerializeField] GameObject animalCellObj;
    [SerializeField] GameObject animalCytoplasm;
    [SerializeField] GameObject nuclues;

    private void Awake()
    {

        animalCytoplasm.SetActive(false);
        animalCellObj.SetActive(false);
        nuclues.SetActive(false);
    }
    void Start()
    {
        if (Application.isEditor)
        {
            CreateAnimalCell(2);
        }
    }

    public void CreateAnimalMembrane(int id)
    {
        animalMembraneId = id;
        
    }

    public void CreateAnimalCell(int id)
    {
        animalCellId = id;
        animalCellObj.SetActive(true);
        animalCytoplasm.SetActive(true);
        nuclues.SetActive(true);
    }

    public void MembranePressureNumUpdate(int value)
    {
        if(value == 3)
        {
            animalCellObj.GetComponent<Animator>().SetTrigger("Inflates");
            animalCytoplasm.GetComponent<Animator>().SetTrigger("Inflates");
        }
        else { return; }
    }

    public void AnimalMembraneRuptures()
    {
        animalCytoplasm.GetComponent<Animator>().SetTrigger("Ruptures");
        animalCellObj.GetComponent<Animator>().SetTrigger("Ruptures");
    }
    public void TheCellDies()
    {
        StartCoroutine(ChangeColor(animalCellObj, Color.gray));
        StartCoroutine(ChangeColor(nuclues, Color.gray, animalCytoplasm));
    }
    public IEnumerator ChangeColor(GameObject objSprite, Color color, GameObject animalCytoplasm = null)
    {
        yield return new WaitForEndOfFrame();
        float elapsedTime = 0f; // Время, прошедшее с начала анимации
        float duration = 1f; // Продолжительность анимации
        Color originalColor = objSprite.GetComponent<Renderer>().material.color;
        Color targetColor = color; // Целевой серый цвет

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            // Линейная интерполяция между текущим цветом и серым
            Color newColor = Color.Lerp(originalColor, targetColor, progress);
            objSprite.GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }

        // Устанавливаем окончательный цвет на случай, если цикл не закончит идеально
        objSprite.GetComponent<Renderer>().material.color = targetColor;
        if (animalCytoplasm)
        {
            animalCytoplasm.SetActive(false);
        }
    }
}
