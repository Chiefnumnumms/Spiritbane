using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrappleMode
{
    Basic,
    Precise
}

public class ObjectGrapple : MonoBehaviour
{
    [Header("References")]
    private InputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private Swinging swingingManager;
    public Transform mainCamera;
    public Transform gunTip;
    public LayerMask whatIsGrappable;
    public LineRenderer lineRenderer;
    public Rigidbody rb;


    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overShootYAxis;
    public float forwardBoostVelocity;

    [Header("Prediction Point")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    public float grapplingCdTimer;

    public bool isGrappling;
    public bool freezePlayer;

    Agreskoul agreskoulManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        swingingManager = GetComponent<Swinging>();
        agreskoulManager = GetComponent<Agreskoul>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // COUNT DOWN TIMER IF ABOVE 0
        if (grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }

        if (freezePlayer)
        {
            playerLocomotion.playerRb.velocity = Vector3.zero;
        }
    }

    public void StartGrapple()
    {
        // DONT ALLOW PLAYER TO GRAPPLE IF COOL DOWN IS IN EFFECT
        if (grapplingCdTimer > 0) return;
        if (swingingManager.isSwinging) return;

        RaycastHit hit;

        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, maxGrappleDistance, whatIsGrappable))
        {
            isGrappling = true;

            // FREEZE THE PLAYER FOR A SECOND
            freezePlayer = true;

            // STORE THE HIT POINT AS THE GRAPPLE POINT
            grapplePoint = hit.point;

            // EXECUTE THE GRAPPLE WITH A DELAY TIMER
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);

        }
        else
        {
            grapplePoint = mainCamera.position + mainCamera.forward * maxGrappleDistance;

            // STOP THE GRAPPLE GIVEN A DELAY
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        // ENABLE LINE RENDERER
        lineRenderer.enabled = true;

        // SET THE GRAPPLE POINT AS THE END POISITION
        lineRenderer.SetPosition(1, grapplePoint);
    }

    public void ExecuteGrapple()
    {
        freezePlayer = false;

        // CALCULATE THE LOWEST POINT OF THE GRAPPLE
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overShootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overShootYAxis;

        // JUMP TOWARDS THE HIGHEST POINT
        playerLocomotion.JumpToPosition(grapplePoint, highestPointOnArc);

        // BOOST THE PLAYER FORWARD
        rb.AddForce(transform.forward * forwardBoostVelocity, ForceMode.Acceleration);

        // WAIT A SECOND THEN STOP THE GRAPPLE
        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        //UNFREEZE THE PLAYER
        freezePlayer = false;

        // PLAYER IS NO LONGER GRAPPLING
        isGrappling = false;

        // RESET THE COOL DOWN TIMER
        grapplingCdTimer = grapplingCd;

    }

    public void CheckForGrappleObject()
    {
        // SETUP RAYCAST VARIABLES
        RaycastHit sphereCastHit;
        RaycastHit raycastHit;

        // HANDLE SPHERE CAST AND RAY CAST 
        Physics.SphereCast(mainCamera.position, predictionSphereCastRadius, mainCamera.forward, out sphereCastHit, maxGrappleDistance, whatIsGrappable);
        Physics.Raycast(mainCamera.position, mainCamera.forward, out raycastHit, maxGrappleDistance, whatIsGrappable);

        Vector3 hitPoint;

        if (raycastHit.point != Vector3.zero)               // DIRECT HIT
        {
            hitPoint = raycastHit.point;

        }
        else if (sphereCastHit.point != Vector3.zero)      // INDIRECT HIT, PREDITION POINT

        {
            hitPoint = sphereCastHit.point;
        }
        else                                              // NOTHING IN THE WAY 
        {
            hitPoint = Vector3.zero;
        }


        if (hitPoint != Vector3.zero)                   // HITPOINT DETECTED A VALID POINT TO GRAPPLE TO
        {
            // GRAPPLE POINT DETECTED, SET THE PREDICTION POINT TO ACTIVE
            predictionPoint.gameObject.SetActive(true);

            // SET THE PREDICTION POINT TO BE THE SAME POSITION OF WHERE THE PLAYER IS AIMING TOWARDS
            predictionPoint.position = hitPoint;
        }
        else
        {            
            predictionPoint.gameObject.SetActive(false);
        }

    }
}
