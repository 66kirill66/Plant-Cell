using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMain : MonoBehaviour
{
    private Comunication comunication;
    public int lightId;
    public GameObject lamp;
    public GameObject lightObject;
    private bool exists;
    private void Awake()
    {
        lamp.SetActive(false);
        lightObject.SetActive(false);
        comunication = FindObjectOfType<Comunication>();
    }
    void Start()
    {

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChancheLightStatus();
        }
    }

    public void ChancheLightStatus()
    {
        comunication.SendEventToPlethora(EventNames.AddLight);
        exists = !exists;
        lightObject.SetActive(exists);
        comunication.UpdateLightStatus(lightId, exists);
    }

    public void CreateLight(string json)
    {
        SimulationData data = JsonUtility.FromJson<SimulationData>(json);
        lightId = data.id;
        lamp.SetActive(true);
        exists = data.exists;
        lightObject.SetActive(exists);
    }
}
