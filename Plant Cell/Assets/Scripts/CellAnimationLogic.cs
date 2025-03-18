using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAnimationLogic : MonoBehaviour
{
    [SerializeField] GameObject vacuole;
    [SerializeField] GameObject cytoplasm;
    [SerializeField] GameObject membrane;
    [SerializeField] GameObject cellWall;
    [SerializeField] GameObject cellWallFullObj;
    [SerializeField] GameObject waterEF;
    [SerializeField] GameObject cytoplasmFullObj;
    [SerializeField] GameObject membraneFullObj;

    private Animator cellAnimator;
    [SerializeField] CableConnector cableConnector1;
    [SerializeField] CableConnector cableConnector5;


    private void Awake()
    {
        cellAnimator = GetComponent<Animator>();
        cellAnimator.enabled = false;
        waterEF.SetActive(false);
    }

    void Start()
    {

    }

    public void CellWallChangeSize(int value)
    {
        float currentSize = 1.8f;
        switch (value)
        {
            case 1:
                currentSize = 1.4f;
                break;
            case 2:
                currentSize = 1.8f;
                break;
            case 3:
                currentSize = 2.2f;
                break;
        }
        //float currentSize = 1.8f;
        //if (value == 0)
        //{
        //    currentSize = 1.8f;
        //}
        //if (value == 1)
        //{
        //    currentSize = 2.2f;
        //}
        Vector3 currentScale = new Vector3(currentSize, currentSize, currentSize);
        GameObject newprite = null;
        bool oldpriteOff = false;
        if (currentSize == 2.2f)
        {
            newprite = cellWallFullObj;
            oldpriteOff = true;
        }
        if (value < 2.2f)
        {
            cellWallFullObj.SetActive(false);
            cellWall.SetActive(true);
        }
        StartCoroutine(ChangeObjectSize(cellWall, currentScale, 3, newprite, oldpriteOff));
    }


    public void ChangeVacuoleSize(int value)
    {
        float currentSize = 0;
        switch (value)
        {
            case 1:
                currentSize = 0.5f;
                break;
            case 2:
                currentSize = 1.2f;
                break;
            case 3:
                currentSize = 2.7f;
                break;
        }
        Vector3 currentScale = new Vector3(currentSize, currentSize, currentSize);

        StartCoroutine(ChangeObjectSize(vacuole, currentScale, 4));
    }
    public void ChangeCytoplasmSize(float value)
    {
        float currentSize = value;
        Vector3 currentScale = new Vector3(currentSize, currentSize, currentSize);
        GameObject newprite = null;
        bool oldpriteOff = false;
        if (value == 1.85f)
        {
            newprite = cytoplasmFullObj;
            oldpriteOff = true;
        }
        if (value < 1.85f)
        {
            cytoplasmFullObj.SetActive(false);
            cytoplasm.SetActive(true);
        }
        StartCoroutine(ChangeObjectSize(cytoplasm, currentScale, 5, newprite, oldpriteOff));
    }
    public void ChangeMembraneSize(float value)
    {
        float currentSize = value;
        Vector3 currentScale = new Vector3(currentSize, currentSize, currentSize);
        GameObject newprite = null;
        bool oldpriteOff = false;
        if (value == 1.85f)
        {
            newprite = membraneFullObj;
            oldpriteOff = true;
        }      
        if (value < 1.85f)
        {
            membraneFullObj.SetActive(false);
            membrane.SetActive(true);
        }
        StartCoroutine(ChangeObjectSize(membrane, currentScale, 5, newprite, oldpriteOff));
    }

    public void ChangeWaterEFSize()
    {
        waterEF.SetActive(true);
        waterEF.transform.localScale = new Vector3(6, 6, 0);
        Vector3 currentScale = new Vector3(0, 0, 0);
        StartCoroutine(ChangeObjectSize(waterEF, currentScale, 6));
    }

    IEnumerator ChangeObjectSize(GameObject obj, Vector3 targetScale, float duration, GameObject newSprite = null, bool oldSprite = false)
    {
        Vector3 initialScale = obj.transform.localScale; // Текущий размер объекта
        float elapsedTime = 0f; // Время, прошедшее с начала анимации

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // Увеличиваем прошедшее время
            float progress = elapsedTime / duration; // Рассчитываем прогресс (от 0 до 1)
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, progress); // Плавно меняем размер
            yield return null; // Ждём следующий кадр
        }
        if (newSprite)
        {
            newSprite.SetActive(true);
        }
        if (oldSprite)
        {
            obj.SetActive(false);
        }
        obj.transform.localScale = targetScale; // Устанавливаем конечный размер для точности
    }

    public void RuptureAnimation()
    {
        cableConnector1.BreakCable();
        cableConnector5.BreakCable();
        cellAnimator.enabled = true;
    }
}
