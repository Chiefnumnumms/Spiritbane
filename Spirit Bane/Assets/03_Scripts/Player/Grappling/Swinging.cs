using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Swinging : MonoBehaviour
{
    public InputManager inputManager;

    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;

    [Header("Swinging")]
    private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("Input")]
    //public KeyCode swingKey = KeyCode.Mouse0;

    [Header("In Air Movement")]
    public Rigidbody rb;
    public Transform orientation;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    public List<GameObject> grapplePoints = new List<GameObject>();

    public Material desiredMaterial;
    public Material originalMaterial;

    public bool isSwinging;
    public bool isLookingAtGrapplePoint;

    private Vector3 currentGrapplePosition;

    public List<GameObject> grapplePointsList = new List<GameObject>();
    public List<SpringJoint> sprintJointsList = new List<SpringJoint>();

    private void Start()
    {
        FindGrapplePointsOnStart();

        inputManager = GetComponent<InputManager>();
    }


    public void HandleSwingAction()
    {
        if (inputManager.swingPressed)
        {
            StartSwing();
        }
        else
        {
            StopSwing();
        }

        CheckForSwingPoints();
        HighlightGrapplePoint();

        if (joint != null) InAirMovement();
    }

    private void StartSwing()
    {
        // return if predictionHit not found
        if (predictionHit.point == Vector3.zero) return;

        isSwinging = true;
        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        // ADD SPRINT JOIN TO A LIST
        sprintJointsList.Add(joint);

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        // customize values as you like
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    private void CheckForSwingPoints()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward,
                            out sphereCastHit, maxSwingDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward,
                            out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        // Option 1 - Direct Hit
        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;

        // Option 2 - Indirect (predicted) Hit No Raycast Hit
        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;

        // Option 3 - Miss (nothing in the way)
        else
            realHitPoint = Vector3.zero;

        // realHitPoint found
        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        // realHitPoint not found
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }

    public void StopSwing()
    {
        lr.positionCount = 0;
        isSwinging = false;
        Destroy(joint);
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
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    private void InAirMovement()
    {
        //// Swing In Air Right Side
        //if (Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);

        //// Swing In Air Left Side
        //if (Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);

        //// Swing In Air Forward
        //if (Input.GetKey(KeyCode.W)) rb.AddForce(orientation.right * forwardThrustForce * Time.deltaTime);

        // Shorten Cable
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distancFromPoint = Vector3.Distance(transform.position, swingPoint);

            // Recalculate Joint Distance
            joint.maxDistance = distancFromPoint * 0.8f;
            joint.minDistance = distancFromPoint * 0.25f;
        }

        // Extend Cable
        if (Input.GetKey(KeyCode.S))
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            // Recalculate Join Distance
            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }

    }

    public void HighlightGrapplePoint()
    {
        // Set up the raycast variables
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast the ray and check if it hits a grapple point on the proper layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.transform.gameObject.layer == LayerMask.NameToLayer("GrapplePoint"))
        {
            isLookingAtGrapplePoint = true;

            // Check if the player is looking at the hit grapple point or swinging on it
            if (isSwinging || isLookingAtGrapplePoint)
            {
                // Change the material of the hit grapple point to the desired material
                MeshRenderer grapplePointRenderer = hit.transform.gameObject.GetComponent<MeshRenderer>();
                grapplePointRenderer.material = desiredMaterial;
            }
            else
            {
                // Reset the material of the hit grapple point to the original material
                MeshRenderer grapplePointRenderer = hit.transform.gameObject.GetComponent<MeshRenderer>();
                grapplePointRenderer.material = originalMaterial;
            }
        }
        else
        {
            isLookingAtGrapplePoint = false;
        }
    }

    public void FindGrapplePointsOnStart()
    {
        GameObject[] grapplePointObjects = GameObject.FindGameObjectsWithTag("GrapplePoint");

        foreach (GameObject gp in grapplePointObjects)
        {
            grapplePointsList.Add(gp);
        }
    }



    /*
    private void Start()
    {
        GameObject[] grapplePointObjects = GameObject.FindGameObjectsWithTag("GrapplePoint");

        foreach (GameObject gp in grapplePointObjects)
        {
            grapplePoints.Add(gp);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(swingKey)) StartSwing();
        if (Input.GetKeyUp(swingKey)) StopSwing();

        CheckForSwingPoints();
        HighlightGrapplePoint();

        if (joint != null) InAirMovement();
    }

    void LateUpdate()
    {
        DrawRope();
    }

    private void StartSwing()
    {
        // return if predictionHit not found
        if (predictionHit.point == Vector3.zero) return;

        isSwinging = true;
        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        // customize values as you like
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    private void CheckForSwingPoints()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward,
                            out sphereCastHit, maxSwingDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward,
                            out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        // Option 1 - Direct Hit
        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;

        // Option 2 - Indirect (predicted) Hit No Raycast Hit
        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;

        // Option 3 - Miss (nothing in the way)
        else
            realHitPoint = Vector3.zero;

        // realHitPoint found
        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        // realHitPoint not found
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }

    void StopSwing()
    {
        lr.positionCount = 0;
        isSwinging = false;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    private void InAirMovement()
    {
        // Swing In Air Right Side
        if (Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);

        // Swing In Air Left Side
        if (Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);

        // Swing In Air Forward
        if (Input.GetKey(KeyCode.W)) rb.AddForce(orientation.right * forwardThrustForce * Time.deltaTime);

        // Shorten Cable
        if (Input.GetMouseButton(1))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distancFromPoint = Vector3.Distance(transform.position, swingPoint);

            // Recalculate Joint Distance
            joint.maxDistance = distancFromPoint * 0.8f;
            joint.minDistance = distancFromPoint * 0.25f;
        }

        // Extend Cable
        if (Input.GetKey(KeyCode.S))
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            // Recalculate Join Distance
            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }

    }

    public void HighlightGrapplePoint()
    {
        // Set up the raycast variables
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast the ray and check if it hits a grapple point on the proper layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.transform.gameObject.layer == LayerMask.NameToLayer("GrapplePoint"))
        {
            isLookingAtGrapplePoint = true;

            // Check if the player is looking at the hit grapple point or swinging on it
            if (isSwinging || isLookingAtGrapplePoint)
            {
                // Change the material of the hit grapple point to the desired material
                MeshRenderer grapplePointRenderer = hit.transform.gameObject.GetComponent<MeshRenderer>();
                grapplePointRenderer.material = desiredMaterial;
            }
            else
            {
                // Reset the material of the hit grapple point to the original material
                MeshRenderer grapplePointRenderer = hit.transform.gameObject.GetComponent<MeshRenderer>();
                grapplePointRenderer.material = originalMaterial;
            }
        }
        else
        {
            isLookingAtGrapplePoint = false;
        }
    }
    */
}
