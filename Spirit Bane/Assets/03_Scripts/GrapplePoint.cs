using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GrapplePoint : MonoBehaviour
{
    public GameObject swingPoint;
    public GameObject pullPoint;

    private GameObject swingPointCopy;
    private GameObject pullPointCopy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PredictionPoint")
        {
            if (gameObject.tag == "SwingingPoint")
            {
                swingPointCopy = swingPoint;
                Instantiate(swingPointCopy, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            }
            else if (gameObject.tag == "PullPoint")
            {
                pullPointCopy = pullPoint;
                Instantiate(swingPointCopy, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            }
        }
    }
}
