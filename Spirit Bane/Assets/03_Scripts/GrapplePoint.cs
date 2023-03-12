using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GrapplePoint : MonoBehaviour
{
    public GameObject swingPoint;
    public GameObject pullPoint;
    public float time;

    private GameObject icon;

    private void Start()
    {
        if (gameObject.tag == "SwingingPoint")
        {
            icon = Instantiate(swingPoint, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);

        }
        else if (gameObject.tag == "PullPoint")
        {
            icon = Instantiate(pullPoint, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
        }

        icon.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PredictionPoint")
        {
            StartCoroutine(ActivateIcon());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "PredictionPoint")
        {
            StopCoroutine(ActivateIcon());
        }
    }

    IEnumerator ActivateIcon()
    {
        icon.SetActive(true);
        yield return new WaitForSeconds(time);
        icon.SetActive(false);
    }
}
