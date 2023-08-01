using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UI;

public class ModelUI : MonoBehaviour
{
    [SerializeField] private GameObject transmission;
    [SerializeField] private Slider slider;
    Renderer renderer;
    

    void Start()
    {
        renderer = transmission.GetComponent<Renderer>();
        slider.value = renderer.material.color.a;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }


    private void OnSliderValueChanged(float value)
    {
        // Get the current material color from the renderer
        Color color = renderer.material.color;

        // Update the alpha value based on the slider's value (assuming slider value is between 0 and 1)
        color.a = value;

        // Assign the updated color back to the material
        renderer.material.color = color;
    }

    public void ShowOrHide()
    {
        if(transmission.activeSelf)
        {
            transmission.SetActive(false);
        }
        else
        {
            transmission.SetActive(true);
        }
    }
}
