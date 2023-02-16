using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
