using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    public float motionDistance;

    public float speed;

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);

        if(transform.position.y > startingPosition.y + motionDistance || transform.position.y < startingPosition.y - motionDistance)
        {
            speed = speed * -1;
        }
    }
}
