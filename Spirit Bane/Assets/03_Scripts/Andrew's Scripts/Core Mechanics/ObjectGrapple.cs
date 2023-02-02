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
    private PlayerManager playerManager;
    private InputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    public Transform mainCamera;
    public Transform gunTip;
    public LayerMask whatIsGrappable;
    public LineRenderer lineRenderer;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overShootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    public float grapplingCdTimer;

    public bool isGrappling;
    public bool freezePlayer;


    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
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

    private void LateUpdate()
    {
        if (isGrappling)
        {
            // CONTINOUSLY UPDATE THE STARTING POSITION AS THE GUNTIP POSITION
            lineRenderer.SetPosition(0, gunTip.position);
        }
    }

    public void StartGrapple()
    {
        // DONT ALLOW PLAYER TO GRAPPLE IF COOL DOWN IS IN EFFECT
        if (grapplingCdTimer > 0) return;

        isGrappling = true;
        Debug.Log("GRAPPLING");

        // FREEZE THE PLAYER FOR A SECOND
        freezePlayer = true;

        RaycastHit hit;

        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, maxGrappleDistance, whatIsGrappable))
        {
            // STORE THE HIT POINT AS THE GRAPPLE POINT
            grapplePoint = hit.point;

            // EXECUTE THE GRAPPLE WITH A DELAY TIMER
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);


            Debug.Log("GRAPPLING EXECUTED");
        }
        else
        {
            grapplePoint = mainCamera.position + mainCamera.forward * maxGrappleDistance;

            // STOP THE GRAPPLE GIVEN A DELAY
            Invoke(nameof(StopGrapple), grappleDelayTime);
            Debug.Log("GRAPPLING STOPPED");
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

        // DISABLE THE LINE RENDERER
        lineRenderer.enabled = false;   
    }
}
