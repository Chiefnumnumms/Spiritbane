using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShot : MonoBehaviour
{
    #region CodeMonkey
    //InputManager inputManager;
    //PlayerLocomotion playerLocomotion;

    //[SerializeField] private Camera mainCamera;
    //[SerializeField] private Transform debugHitPointTransform;

    //private Vector3 hookShotPosition;

    //private void Awake()
    //{
    //    inputManager = GetComponent<InputManager>();
    //    playerLocomotion = GetComponent<PlayerLocomotion>();
    //}

    //private void Update()
    //{
    //    HandleHookshotStart();
    //}

    //private void HandleHookshotStart()
    //{
    //    if (inputManager.swing_Pressed)
    //    {
    //        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit raycastHit))
    //        { 
    //            // HIT SOMETHING
    //            debugHitPointTransform.position = raycastHit.point;
    //            hookShotPosition = raycastHit.point;
    //        }
    //    }
    //}
    #endregion

    public float hookshotDistance = 10f;
    public float hookshotSpeed = 50f;
    public float hookshotRange = 100f;
    public GameObject hookshotObject;

    InputManager inputManager;
    public Camera mainCamera;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (inputManager.swing_Pressed)
        {
            Hookshot();
        }
    }

    public void Hookshot()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, hookshotRange))
        {
            hookshotObject.SetActive(true);
            hookshotObject.transform.position = transform.position;
            Vector3 hookshotTarget = hit.point - transform.position;
            StartCoroutine(MoveToHookshotTarget(hookshotTarget));
        }
    }

    private IEnumerator MoveToHookshotTarget(Vector3 targetPosition)
    {
        float currentDistance = 0f;
        while (currentDistance < hookshotDistance)
        {
            currentDistance += hookshotSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, currentDistance / hookshotDistance);
            yield return null;
        }
        hookshotObject.SetActive(false);
    }


}
