// ==========================================
//           Andrew Anderious
//              2023-01-12
//        SWINGING MECHANIC SCRIPT
// ==========================================

using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Swinging : MonoBehaviour
{
    public InputManager inputManager;
    public PlayerLocomotion playerLocomotionManager;

    [Header("References")]
    public LineRenderer lineRenderer;
    public Transform gunTip, mainCamera, playerModel;
    public LayerMask whatIsGrappleable;

    [Header("Swinging")]
    private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint springJoint;

    [Header("In Air Movement")]
    public Rigidbody rb;
    public Transform orientation;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Prediction Point")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    [Header("Grapple Point Settings")]
    public Material desiredMaterial;
    public Material originalMaterial;

    public bool isSwinging;
    public bool isLookingAtGrapplePoint;

    private Vector3 currentGrapplePosition;
    public float maxIndicationDistance = 25.0f;

    private List<GameObject> grapplePoints = new List<GameObject>();
    private List<SpringJoint> sprintJointsList = new List<SpringJoint>();
    private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

    private void Start()
    {
        // FINDS ALL GRAPPLE POINTS IN THE SCENE
        FindGrapplePointsOnStart();

        inputManager = GetComponent<InputManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotion>();
    }

    public void HandleSwingAction()
    {
        if (inputManager.swing_Pressed) 
        {
            StartSwing();
            DrawRope();
        }
        else
        {
            StopSwing();
        }

        // ONLY HANDLE IN AIR MOVEMENT IF THERE IS A SPRING JOINT TO WORK WITH
        if (springJoint != null) InAirMovement();
    }

    private void StartSwing()
    {
        // IF PREDICTION POINT DOESN'T FIND ANYTHING EXIT
        if (predictionHit.point == Vector3.zero) return;

        // IF THERE IS NO JOINT CURRENTLY
        if (springJoint == null)
        {
            // THEN CREATE A JOINT AND ALLOW THE PLAYER TO SWING
            isSwinging = true;
            swingPoint = predictionHit.point;
            springJoint = playerModel.gameObject.AddComponent<SpringJoint>();
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = swingPoint;
        }

        // THE DISTANCE HOW FAR THE PLAYER IS FROM THE GRAPPLE POINT WHEN GRAPPLING
        // THE LOWER THE MAX VALUE THE CLOSER THE PLAYER IS ABLE TO SWING TOWARDS THE GRAPPLE POINT
        float distanceFromPoint = Vector3.Distance(playerModel.position, swingPoint);
        springJoint.maxDistance = distanceFromPoint * 0.8f;
        springJoint.minDistance = distanceFromPoint * 0.25f;

        // CUSTOMIZE JOINT PROPERTIES
        springJoint.spring = 4.5f;
        springJoint.damper = 7f;
        springJoint.massScale = 4.5f;
 
        lineRenderer.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    public void CheckForSwingPoints()
    {
        if (springJoint != null) return;

        // SETUP RAYCAST VARIABLES
        RaycastHit sphereCastHit;
        RaycastHit raycastHit;

        // HANDLE SPHERE CAST AND RAY CAST 
        Physics.SphereCast(mainCamera.position, predictionSphereCastRadius, mainCamera.forward, out sphereCastHit, maxSwingDistance, whatIsGrappleable);
        Physics.Raycast(mainCamera.position, mainCamera.forward, out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 hitPoint;

        if (raycastHit.point != Vector3.zero)               // DIRECT HIT
        {
            hitPoint = raycastHit.point;

        }
        else if (sphereCastHit.point != Vector3.zero)      // INDIRECT HIT, PREDITION POINT

        {
            hitPoint = sphereCastHit.point;
            HighlightGrapplePoint(maxIndicationDistance);
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

            // HIGHLIGHT GRAPPLE POINT TO INDICATE THAT IT IS A VALID POINT
            HighlightGrapplePoint(maxIndicationDistance);
        }
        else                                                // NOTHING FOUND ON HIT POINT
        {
            // DISABLE THE PREDICTION POINT AS NOTHING WAS SCANNED
            predictionPoint.gameObject.SetActive(false);

            foreach (MeshRenderer meshes in meshRenderers)
            {
                meshes.material = originalMaterial;
            }
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }

    public void StopSwing()
    {
        lineRenderer.positionCount = 0;
        isSwinging = false;
        Destroy(springJoint);
        DestroyAllJoints();
    }

    void DestroyAllJoints()
    {
        if (!isSwinging)
        {
            // DESTROY ALL SPRINT JOINTS WHEN THE PLAYER IS NO LONGER GRAPPLING
            foreach (SpringJoint springJoint in sprintJointsList)
            {
                Destroy(springJoint);
            }
            sprintJointsList.Clear();
        }

    }

    public void DrawRope()
    {
        if (!springJoint) return;

        Debug.Log("Rope Drawn!");

        // DRAW THE ROPE BASED ON THE CURRENT GRAPPLE POSITION AND THE SWINGPOINT IT IS GOING TOWARDS
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        // SET LR POSITIONS
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, currentGrapplePosition);
    }

    private void InAirMovement()
    {
        // IF THE PLAYER IS NOT IN THE AIR, THEN DONT PERFORM INAIRMOVEMENT
        if (playerLocomotionManager.isGrounded) return;

        if (inputManager.d_Pressed) // SWING RIGHT
        {
            // APPLY HORIZONTAL FORCE TO THE RIGIDBODY
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        }
        else if (inputManager.a_Pressed) // SWING LEFT
        {
            // APPLY HORIZONTAL FORCE TO THE RIGIDBODY
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);
        }
        else if (inputManager.w_Pressed) // SWING FORWARD
        {
            // APPLY FORWARD FORCE TO THE RIGIDBODY
            rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);
        }

        // WHEN THE MOUSE BUTTON IS BEING HELD TO SWING, THE CABLE SLOWLY SHRINKS
        // SO THE PLAYER SLOWLY MOVES TOWARDS THE GRAPPLE POINT
        if (inputManager.swing_Pressed)
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            // Recalculate Joint Distance
            springJoint.maxDistance = distanceFromPoint * 0.8f;
            springJoint.minDistance = distanceFromPoint * 0.25f;
        }

        // HANDLES INPUT TO EXTEND THE CABLE WHEN SWINGING
        if (inputManager.rope_Adjust_Pressed)
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            // Recalculate Join Distance
            springJoint.maxDistance = extendedDistanceFromPoint * 0.8f;
            springJoint.minDistance = extendedDistanceFromPoint * 0.25f;
        }

    }

    public void HighlightGrapplePoint(float maxRange)
    {
        // HIGHLIGHTS ALL GRAPPLE POINTS
        // RAYCAST VARIABLES

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // CAST THE RAY TO CHECK IF IT COLLIDES/HITS A GRAPPLE POINT LAYER GIVEN A SPECIFIC MAX RANGE
        if (Physics.Raycast(ray, out hit, maxRange) && hit.transform.gameObject.layer == LayerMask.NameToLayer("GrapplePoint"))
        {
            isLookingAtGrapplePoint = true;

            // PLAYER IS LOOKING AT A VALID GRAPPLE POINT
            if (isLookingAtGrapplePoint)
            {
                // CHANGE THE MATERIAL OF THE POINT PLAYER IS LOOKING TOWARDS
                MeshRenderer grapplePointRenderer = hit.transform.gameObject.GetComponent<MeshRenderer>();
                grapplePointRenderer.material = desiredMaterial;

                meshRenderers.Add(grapplePointRenderer);
            }
        }
    }

    public void FindGrapplePointsOnStart()
    {
        // FIND ALL GRAPPLE POINTS WITH THE PROPER TAG TO STORE THEM IN AS VALID GRAPPLE POINTS
        GameObject[] grapplePointObjects = GameObject.FindGameObjectsWithTag("GrapplePoint");

        foreach (GameObject gp in grapplePointObjects)
        {
            grapplePoints.Add(gp);
        }
    }


}
