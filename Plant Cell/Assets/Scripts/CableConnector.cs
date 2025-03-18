using System.Collections;
using UnityEngine;

public class CableConnector : MonoBehaviour
{
    public Transform membranePoint; // Первая точка
    public Transform wallPoint;   // Вторая точка
    private LineRenderer lineRenderer;
    private bool isBroken = false; // Флаг разрыва
    public float startPointSize;
    public float endPointSize;

    public float breakOffset = 0.5f; // Смещение после разрыва
    public float breakAnimationDuration = 1f; // Длительность анимации разрыва

    private Vector3 breakPoint; // Точка разрыва
    private Vector3 upperTarget; // Цель верхней линии
    private Vector3 lowerTarget; // Цель нижней линии

    public bool canBreak;



    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component is missing!");
            return;
        }

        if (membranePoint == null || wallPoint == null)
        {
            Debug.LogError("StartPoint or EndPoint is missing!");
            return;
        }

        // Настраиваем линию
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, membranePoint.position);
        lineRenderer.SetPosition(1, wallPoint.position);

        // Настраиваем кривую ширины
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, startPointSize); // Начало (ширина 0.1)
        curve.AddKey(0.5f, 0.2f); // Середина (ширина 0.2)
        curve.AddKey(1, endPointSize); // Конец (ширина 0.1)
        lineRenderer.widthCurve = curve;
    }

    void Update()
    {
        if (!isBroken)
        {
            // Обновляем позиции до момента разрыва
            lineRenderer.SetPosition(0, membranePoint.position);
            lineRenderer.SetPosition(1, wallPoint.position);

            //// Проверяем ручной разрыв
            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    BreakCable();
            //}
        }
    }

    public void BreakCable()
    {
        if (canBreak)
        {
            // Отключаем оригинальную линию
            lineRenderer.enabled = false;
            if (isBroken) return;

            isBroken = true;

            // Определяем точку разрыва — середина между верхней и нижней точками
            breakPoint = (membranePoint.position + wallPoint.position) / 2;

            // Настраиваем цели движения
            upperTarget = breakPoint + Vector3.up * breakOffset; // Верхняя часть поднимается
            lowerTarget = breakPoint + Vector3.down * breakOffset; // Нижняя часть опускается

            // Создаём верхний LineRenderer
            GameObject upperLineObject = new GameObject("UpperLine");
            upperLineObject.transform.parent = wallPoint; // Привязываем к wallPoint
            LineRenderer upperLineRenderer = upperLineObject.AddComponent<LineRenderer>();
            ConfigureLineRenderer(upperLineRenderer);
            upperLineRenderer.positionCount = 2;
            upperLineRenderer.SetPosition(0, wallPoint.position); // Верхняя линия прикреплена к membranePoint
            upperLineRenderer.SetPosition(1, breakPoint);            // Вторая точка — точка разрыва
            SetLineSize(upperLineRenderer, 0.5f, 1);


            // Создаём нижний LineRenderer
            GameObject lowerLineObject = new GameObject("LowerLine");
            lowerLineObject.transform.parent = membranePoint; // Привязываем к membranePoint
            LineRenderer lowerLineRenderer = lowerLineObject.AddComponent<LineRenderer>();
            ConfigureLineRenderer(lowerLineRenderer);
            lowerLineRenderer.positionCount = 2;
            lowerLineRenderer.SetPosition(0, breakPoint);            // Нижняя линия начинается с точки разрыва
            lowerLineRenderer.SetPosition(1, membranePoint.position);    // Прикреплена к wallPoint
            SetLineSize(lowerLineRenderer, 1, 0.1f);

            // анимация разрыва
            StartCoroutine(AnimateBreak(upperLineRenderer, lowerLineRenderer));
        }
    }

    private void ConfigureLineRenderer(LineRenderer line)
    {
        // Настраиваем материал (используем материал оригинальной линии)
        line.material = lineRenderer.material;
        line.sortingOrder = 1;
        line.useWorldSpace = false; // ВАЖНО: работаем в локальных координатах
    }

    private IEnumerator AnimateBreak(LineRenderer upperLineRenderer, LineRenderer lowerLineRenderer)
    {
        float elapsedTime = 0f;
        Vector3 upperStart = breakPoint; // Начальная точка разрыва для верхней линии
        Vector3 lowerStart = breakPoint; // Начальная точка разрыва для нижней линии

        while (elapsedTime < breakAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / breakAnimationDuration;

            // Плавное движение точки разрыва вверх и вниз
            Vector3 currentUpper = Vector3.Lerp(upperStart, upperTarget, progress);
            Vector3 currentLower = Vector3.Lerp(lowerStart, lowerTarget, progress);

            // Обновляем верхнюю линию
            upperLineRenderer.SetPosition(1, currentUpper); // Движение второй точки (разрыва)

            // Обновляем нижнюю линию
            lowerLineRenderer.SetPosition(0, currentLower); // Движение первой точки (разрыва)

            yield return null;
        }

        // Устанавливаем конечные позиции
        upperLineRenderer.SetPosition(1, upperTarget); // Верхняя линия завершает движение
        lowerLineRenderer.SetPosition(0, lowerTarget); // Нижняя линия завершает движение
    }

    public void SetLineSize(LineRenderer lineRenderer, float startPointSize, float endPointSize)
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, startPointSize); // Начало (ширина 0.1)
        curve.AddKey(0.5f, 0.2f); // Середина (ширина 0.2)
        curve.AddKey(1, endPointSize); // Конец (ширина 0.1)
        lineRenderer.widthCurve = curve;
    }

}
