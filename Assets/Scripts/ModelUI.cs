using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UI;

public class ModelUI : MonoBehaviour
{
    [SerializeField] private GameObject prototype;
    [SerializeField] private Slider slider;
    [SerializeField] private Button fadeButton;
    [SerializeField] private Button opaqueButton;
    [SerializeField] private Material fadeMaterial;
    [SerializeField] private Material opaqueMaterial;
    Renderer renderer;
    

    void Start()
    {
        renderer = prototype.GetComponent<Renderer>();
        slider.value = renderer.material.color.a;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        fadeButton.onClick.AddListener(OnChangeFade);
        opaqueButton.onClick.AddListener(OnChangeOpaque);
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
        if(prototype.activeSelf)
        {
            prototype.SetActive(false);
        }
        else
        {
            prototype.SetActive(true);
        }
    }

    private void OnChangeFade()
    {
        prototype.GetComponent<Renderer>().material = fadeMaterial;
    }

    private void OnChangeOpaque()
    {
        prototype.GetComponent<Renderer>().material = opaqueMaterial;
    }
}
