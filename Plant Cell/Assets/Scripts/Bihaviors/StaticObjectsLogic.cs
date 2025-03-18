using System.Collections;
using UnityEngine;

public class StaticObjectsLogic : MonoBehaviour
{
    public int objectId;
    public bool haveToxic;
    private SpriteRenderer spriteRenderer;
    private Collider2D objectCollider;

    private void Awake()
    {
        objectCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    private void Start()
    {

    }

    private void Update()
    {
        if (haveToxic)
        {
            spriteRenderer.color = Color.gray;
            objectCollider.enabled = false;
        }
    }
    public void AttractsToxic(GameObject toxic, Vector3 position)
    {
        StartCoroutine(MoveToChloroplast(toxic, position));
    }

    public IEnumerator MoveToChloroplast(GameObject toxic, Vector3 position)
    {
        while (Vector3.Distance(toxic.transform.position, position) > 0.001f)
        {
            toxic.transform.position = Vector3.MoveTowards(toxic.transform.position, position, 7 * Time.deltaTime);
            yield return null;
        }
        haveToxic = true;
    }
}
