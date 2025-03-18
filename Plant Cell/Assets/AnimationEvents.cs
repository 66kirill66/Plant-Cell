using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Comunication comunication;
    private void Awake()
    {
        comunication = FindObjectOfType<Comunication>();
    }
    void Start()
    {
        
    }

    public void UpdatePressureType(string pressureType) // Event set in animal cell animation 
    {

        // 1 -  when membrane Ruptures and the cell dies - "VeryLow"
        // 2 -  Start normal  animation - "Low"
        // 3 -  Start animation to - "VeryHigh"
        comunication.UpdatemMmbranePressureType("", pressureType);
    }
}
