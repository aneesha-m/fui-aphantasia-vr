using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SliderController : MonoBehaviour
{
    public Slider mainSlider;
    public GameObject sun;
    public float xr, yr, zr;

    public void Start()
    {
        
        //Adds a listener to the main slider and invokes a method when the value changes.
        //x = sun.transform.get
        mainSlider.maxValue = 10;
        mainSlider.minValue = 0;
        
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        var rotationVector = sun.transform.rotation.eulerAngles;
        rotationVector.x = mainSlider.value*10-10;
        sun.transform.rotation = Quaternion.Euler(rotationVector);
        //sun.transform.Rotate(mainSlider.value * 10 - 10, 0,0,Space.Self);
        //sun.transform.rotation.z = mainSlider.value * 10 - 10;


        Debug.Log(mainSlider.value);
    }
}
