using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class EnemyGrapple : MonoBehaviour
{
    public InputManager inputManager;

    private GameObject enemyObj;
    LineRenderer lineRenderer;
    Rigidbody rb;

    // Since this script is being meshed with the agraskoul script i've marked my updates to the script just as comments for now so it wont interfere with the merging.
    // the code may not be perfect as i couldnt test properly due to the scripts no longer being on Gaoh but the logic should be more or less correct.
    // if it isnt working or there are any questions about why i set things up the way i did please shoot me a message im happy to help.

    //Add Names of Pullable objects and the final positions that the objects need to be moved towards
    //[SerializeField]
    //IDictionary<GameObject, Vector3> pullableObjects;

    //Add Names of objects that need to be pulled to activate and bools to check if they have been activated by the pull mechanic
    //[SerializeField]
    //IDictionary<GameObject, bool> pullableInteractables;

    //bool wasPullableHooked;
    bool isHooking;
    bool wasEnemyHooked;

    float hookDistance;
    Vector3 originalPosition;
    GameObject returnPoint;

    public float hookSpeed = 1500.0f;
    public const float maxHookDistance = 25.0f;

    private BoxCollider hookCollider;

    EnemyStats enemyHealth;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        returnPoint = GameObject.Find("ReturnPoint");

        isHooking = false;
        //wasPullableHooked = false;
        wasEnemyHooked = false;

        hookDistance = 0.0f;

        originalPosition = new Vector3(returnPoint.transform.position.x, returnPoint.transform.position.y, returnPoint.transform.position.z);

        rb = GetComponent<Rigidbody>();

        hookCollider = GetComponent<BoxCollider>();
        hookCollider.enabled = false;

        enemyHealth = FindObjectOfType<EnemyStats>();
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
        //PullObjectToPos();
        //ActivatePullableObject();
    }

    private void StartHooking()
    {
        isHooking = true;

        // ENABLE COLLIDER 
        hookCollider.enabled = true;
        Debug.Log("ENABLED THE HOOK COLLIDER");

        rb.isKinematic = false;
        rb.AddForce(transform.forward * hookSpeed);

        Vector3 dir = Vector3.right;
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
                hookCollider.enabled = false;
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

    private void PullObjectToPos()
    {
        // rather then move the enemy towards the player check the name of the enemy object against the objects against the dictionary
        // and move it to the position that is stored in the dictonary
        //if(wasPullableHooked)
        //{
        //    foreach (GameObject objects in pullableObjects.Keys)
        //    {
        //        if (pullableObjects.ContainsKey(objects))
        //        {
        //           Vector3 finalEnemyPos;
        //           pullableObjects.TryGetValue(objects, out finalEnemyPos);
        //           enemyObj.transform.position = Vector3.MoveTowards(enemyObj.transform.position, finalEnemyPos, maxHookDistance);

        //           wasPullableHooked = false;
        //        }
        //    }
        //}

        // this may need some changes but this is roughly the train of thought to easly check against and move the objects to their intended pos
    }

    private void ActivatePullableObject()
    {
        //Second verse same as the first we check to see if the game object is in our dictonary and if it is set the bool tied to that dictonary 
        //to true so that we can use it in other scripts to check if the path should be opened
        //if (wasPullableHooked)
        //{
        //    foreach (GameObject interactables in pullableInteractables.Keys)
        //    {
        //        if (pullableInteractables.ContainsKey(interactables))
        //        {
        //            pullableInteractables[interactables] = true;

        //            //Adjust the color or material of the object that has been interacted with to show that its been adjusted
        //            interactables.GetComponent<Renderer>().material.SetColor("Red",Color.red);

        //           wasPullableHooked = false;
        //        }
        //    }
        //}
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("ENEMY WAS HOOKED");

            enemyHealth.TakeDamage(25);

            wasEnemyHooked = true;
            enemyObj = collider.gameObject;
        }

        //Check to see if you got an object that needs to be moved
        //if(collider.gameObject.tag.Equals("Pullable"))
        //{
        //    Debug.Log("Pullable Was Hooked");
        //    wasPullableHooked = true;
        //    enemyObj = collider.gameObject
        //}

    }
}
