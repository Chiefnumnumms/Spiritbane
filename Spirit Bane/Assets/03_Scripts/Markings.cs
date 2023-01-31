using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Markings : MonoBehaviour
{
    public Slider slider;

    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetColor("_EmissionColor", Color.cyan * slider.value);
    }
}
