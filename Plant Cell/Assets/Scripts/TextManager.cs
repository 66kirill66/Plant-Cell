using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        text.enabled = false;

    }
    public void ShowText(bool value)
    {
        text.enabled = value;
    }

}
