using System.Collections;
using UnityEngine;
using System;

public class Movment : MonoBehaviour
{
    public Vector2 velocity;
    public bool isMove;
    public bool isUIMove;
    public float speed;
    public int objectId;
    public int dictId;

    private Comunication comunication;
    private GameObject collisionObject;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        comunication = FindObjectOfType<Comunication>();
        float rundomX = UnityEngine.Random.Range(0, 2) == 0 ? -5 : 5;
        float rundomY = UnityEngine.Random.Range(0, 2) == 0 ? -5 : 5;
        velocity = new Vector2(rundomX, rundomY);
    }
    private void Start()
    {
        speed = 2;
    }

    private void Update()
    {
        if (isMove && !TheCell.Instance.isCellDied)
        {
            transform.Translate(speed * Time.deltaTime * velocity);
        }
    }
    public IEnumerator UiMovment(Vector3 position, float speed, float endValue)
    {
        while (isUIMove)
        {
            while (Vector3.Distance(transform.position, position) > 0.001f)
            {

                transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
                yield return null;
            }
            while (transform.localScale.x < endValue)
            {
                transform.localScale = new Vector3(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f, transform.localScale.z + 0.2f);
                yield return null;
            }
            transform.localScale = new Vector3(endValue, endValue, endValue);
            isUIMove = false;
        }
    }


    public void StartUIMove(Vector3 newPosition, float speed, float endValue)
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        isUIMove = true;
        currentCoroutine =  StartCoroutine(UiMovment(newPosition, speed, endValue));
    }

    public void CreatedAnimation()
    {
        StartCoroutine(ChangeColor());
    }
    IEnumerator ChangeColor()
    {
        float elapsedTime = 0f; // Time elapsed since the start of the animation
        float duration = 1.5f; // Duration of animation
        Renderer renderer = GetComponent<Renderer>(); // Cache Renderer
        Material material = renderer.material; // Get the material
        Color originalColor = material.color;

        // Initial transparency
        float startAlpha = 0f;
        // Final transparency
        float targetAlpha = 1f;

        // Set the initial transparency
        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, startAlpha);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; 
            float progress = elapsedTime / duration; // Calculate animation progress (from 0 to 1)

            // Interpolate transparency
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, progress);
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);

            yield return null; // Wait for the next frame
        }

        // Set the final transparency value
        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
        isMove = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Берем нормаль из точки контакта
            Vector2 normal = collision.contacts[0].normal;
            // Отражаем вектор скорости
            velocity = Vector2.Reflect(velocity, normal);


        }
        if (collision.gameObject.CompareTag("Mitochondrion") || collision.gameObject.CompareTag("Chloroplast"))
        {
            if (collisionObject != collision.gameObject)
            {
                collisionObject = collision.gameObject;
                if (collision.gameObject.TryGetComponent(out StaticObjectsLogic logic))
                {
                    int otheId = logic.objectId;
                    comunication.SendMeetMassege(objectId, otheId, dictId);
                }
            }
        }
        if (collision.gameObject.CompareTag("Naclues") || collision.gameObject.CompareTag("Vacoule"))
        {
            collisionObject = collision.gameObject;
            if (collision.gameObject.TryGetComponent(out ObjectData data))
            {
                int otheId = data.objectId;
                comunication.SendMeetMassege(objectId, otheId);
            }
        }
    }
}
