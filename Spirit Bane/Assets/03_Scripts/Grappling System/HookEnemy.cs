using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class HookEnemy : MonoBehaviour
{
    public InputManager inputManager;

    private GameObject enemyObj;

    LineRenderer lineRenderer;

    Rigidbody rb;

    bool isHooking;
    bool wasEnemyHooked;

    float hookDistance;
    Vector3 originalPosition;
    GameObject returnPoint;

    public float hookSpeed = 1500.0f;
    public const float maxHookDistance = 5.0f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        returnPoint = GameObject.Find("ReturnPoint");

        isHooking = false;
        wasEnemyHooked = false;

        hookDistance = 0.0f;

        originalPosition = new Vector3(returnPoint.transform.position.x, returnPoint.transform.position.y, returnPoint.transform.position.z);

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        originalPosition = new Vector3(returnPoint.transform.position.x, returnPoint.transform.position.y, returnPoint.transform.position.z);

        lineRenderer.SetPosition(0, originalPosition);
        lineRenderer.SetPosition(1, transform.position);

        if (inputManager.grapple_Pressed && !isHooking && !wasEnemyHooked)
        {
            StartHooking();
        }

        ReturnHook();
        BringEnemyTowardsPlayer();
    }

    private void StartHooking()
    {
        isHooking = true;

        rb.isKinematic = false;
        rb.AddForce(transform.forward * hookSpeed);
    }

    private void ReturnHook()
    {
        if (isHooking)
        {
            hookDistance = Vector3.Distance(transform.position, originalPosition);

            // IF OUT OF RANGE OR ENEMY WAS HOOKED
            if (hookDistance > maxHookDistance || wasEnemyHooked)
            {
                rb.isKinematic = true;
                transform.position = originalPosition;
                isHooking = false;
            }
        }
    }

    private void BringEnemyTowardsPlayer()
    {
        if (wasEnemyHooked)
        {
            // POSITION INFRONT OF GRAPPLE GUN
            Vector3 enemyFinalPosition = new Vector3(originalPosition.x, enemyObj.transform.position.y, originalPosition.z);
            enemyObj.transform.position = Vector3.MoveTowards(enemyObj.transform.position, enemyFinalPosition, maxHookDistance);

            // RESET HOOK
            wasEnemyHooked = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("ENEMY WAS HOOKED");

            wasEnemyHooked = true;
            enemyObj = collider.gameObject;
        }
    }
}
