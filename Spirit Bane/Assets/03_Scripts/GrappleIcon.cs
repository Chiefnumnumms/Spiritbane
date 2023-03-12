using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleIcon : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float minSize;
    public float maxSize;

    private float sizeChange = 0.01f;

    private void OnEnable()
    {
        transform.LookAt(target);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3( 0, 0, speed * Time.deltaTime));

        transform.localScale += new Vector3(sizeChange, sizeChange, sizeChange);

        if(transform.localScale.x > maxSize || transform.localScale.x < minSize)
        {
            sizeChange = sizeChange * -1;
        }
    }
}
