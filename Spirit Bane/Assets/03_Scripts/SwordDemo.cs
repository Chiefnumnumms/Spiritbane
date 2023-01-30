using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is only for our demo on milestone 2, it can be deleted after
// - Spencer

public class SwordDemo : MonoBehaviour
{
    public Slider scaleSlider;
    float scale;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scale = scaleSlider.value;
        this.transform.localScale = new Vector3(1.329002f, scale, 1);
    }
}
