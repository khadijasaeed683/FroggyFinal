using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateBySlider : MonoBehaviour
{
    [Range(-36, 36)] public float Angle = 0f;  // Move this out of the Update method
    public Slider slider;

    // Update is called once per frame
    void Update()
    {
        Angle=slider.value; // Assign the float value directly to the slider
        this.transform.rotation = Quaternion.Euler(0, Angle, 0);  // Use Quaternion.Euler to set the rotation
    }
}
