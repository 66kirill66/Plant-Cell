using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPrefabLogic : MonoBehaviour
{
    [SerializeField] GameObject drop;
    private SpriteRenderer spriteRenderer;
    private GameObject currentCollision;
    private Collider2D currentCollider;
    private Rigidbody2D rb;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Dish"))
        {
            if (collision.gameObject != currentCollision)
            {
                currentCollider.enabled = false;
                rb.simulated = false;
                currentCollision = collision.gameObject;
                spriteRenderer.enabled = false;
                drop.SetActive(true);
                StartCoroutine(ChangeSizeAndColor());
            }
        }
    }

    IEnumerator ChangeSizeAndColor()
    {
        float elapsedTime = 0f; // Time elapsed since the start of the animation
        float duration = 1;
        Vector3 newScale = new Vector3(2.5f, 0.6f, 1);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; 
            float progress = elapsedTime / duration; 
            drop.transform.localScale = Vector3.Lerp(drop.transform.localScale, newScale, progress); // Smoothly change the size with Lerp
            yield return null; 
        }
        elapsedTime = 0f;
        Color originalColor = drop.GetComponent<Renderer>().material.color; 
        while (elapsedTime < duration)
        {
            elapsedTime += 2 * Time.deltaTime;
            float progress = elapsedTime / duration;
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(originalColor.a, 0f, progress));
            drop.GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }

        drop.GetComponent<Renderer>().material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
