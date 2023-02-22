using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UIElements;

public class Agreskoul : MonoBehaviour
{
    [Header("Agreskoul")]
    [SerializeField] private float bladeExtentionSpeed;
    [SerializeField] private float speedDecrement;
    [SerializeField] private float scaleFactor;
    [SerializeField] private float maxExtentionDistance = 50.0f;
    [SerializeField] private Transform energy;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private List<Transform> bladePieces = new List<Transform>();
    [SerializeField] private float falingVelocityReductionTimer;
    [SerializeField] private float reductionRate;

    [Header("Swinging Attributes")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform playerModel;
    [SerializeField] private LayerMask validSwingPoint;
    [SerializeField] private float maxSwingDistance = 25f;
    [SerializeField] public Vector3 swingPointHit;
    [SerializeField] private SpringJoint springJoint;

    [Header("In Air Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    [SerializeField] private float horizontalThrustForce;
    [SerializeField] private float forwardThrustForce;
    [SerializeField] private float extendCableSpeed;

    [Header("Prediction Point")]
    [SerializeField] private RaycastHit predictionHit;
    [SerializeField] private float predictionSphereCastRadius;
    [SerializeField] private Transform predictionPoint;

    [Header("Material Set")]
    [SerializeField] private Material desiredMaterial;
    [SerializeField] private Material originalMaterial;

    [SerializeField] private bool isSwinging;
    [SerializeField] private bool isLookingAtGrapplePoint;

    private Vector3 currentSwingingPosition;
    [SerializeField] public float maxIndicationDistance = 25.0f;

    private List<GameObject> grapplePoints = new List<GameObject>();
    private List<SpringJoint> sprintJointsList = new List<SpringJoint>();
    private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

    private AnimationManager animationManager;
    [SerializeField] private PlayerLocomotion playerLocomotion;
    private InputManager inputManager;

    private float originalFallingVelocity;
    private Vector3 originalBladeScale;
    private Vector3 originalEnergyScale;
    private Quaternion originalPivotRotation;
    private Transform originalPivotTransform;
    public Transform pivotTransform;
    private GameObject swordTip;

    private bool isBladeExtended;

    private void Awake()
    {
        // FINDS ALL BLADE PIECES ON START
        FindAllBladePiecesOnStart();

        // FINDS ALL VALID SWINGING POINTS 
        FindSwingingPointsOnStart();

        // CACHE ALL VALUES
        CacheAllValues();
    }

    private void Start()
    {
        swordTip = GameObject.FindGameObjectWithTag("BladeTip");
        energy = GameObject.FindGameObjectWithTag("Energy").GetComponent<Transform>();

        inputManager = GetComponent<InputManager>();
        animationManager = GetComponent<AnimationManager>();
        playerLocomotion = FindObjectOfType<PlayerLocomotion>();
    }

    private void Update()
    {
        // RETURN TO ORIGINAL FALLING VELOCITY
        if (playerLocomotion.isGrounded)
        {
            playerLocomotion.fallingVel = originalFallingVelocity;
        }

        CheckForSwingPoints();
        HighlightSwingingPoint(maxIndicationDistance);
    }

    private void CacheAllValues()
    {
        // CACHE ORIGINAL SCALE OF THE BLADE PIECES
        originalBladeScale = bladePieces[0].localScale;

        // CACHE ORIGINAL SCALE OF THE ENERGY
        originalEnergyScale = energy.localScale;

        // CACHE ORIGINAL PIVOT TRANSFORM
        originalPivotTransform = pivotTransform.transform;

        // CACHE ORIGINAL PIVOT ROTATION
        originalPivotRotation = weaponPivot.transform.rotation;

        // CACHE ORIGINAL FALLING VELOCITY
        originalFallingVelocity = playerLocomotion.fallingVel;

        isBladeExtended = false;
    }

    #region AGRESKOUL MAIN

    public void ExecuteSwordSwing()
    {
        isBladeExtended = true;

        if (isBladeExtended)
        {
            // PLAY ANIMATION
            if (playerLocomotion.inAirTimer <= 0)
            {
                animationManager.PlayTargetAnim("Grapple", false, true);
            }

            // CALCULATE DISTANCE AND TIME
            Vector3 target = swingPointHit;
            Vector3 distance = target - swordTip.transform.position;

            if (distance.magnitude >= maxExtentionDistance) return;

            // IF PAST MAX DISTANCE
            if (distance.magnitude >= maxExtentionDistance)
            {
                target = maxExtentionDistance * distance.normalized;
            }

            // CALCULATE THE ROTATION TO FACE THE TARGET
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

            // SET SCALE IN Y DIRECTION OF THE ENERGY TRANSFORM
            scaleFactor = distance.magnitude / energy.transform.localScale.y;

            // CALCULATE THE NEW Y VALUE BASED ON THE SCALE FACTOR
            float newY = energy.transform.localScale.y * scaleFactor;

            // CALCULATE EXTENTION TIME BASED ON DISTANCE
            float time = distance.magnitude * bladeExtentionSpeed / speedDecrement;

            // CALCULATE THE NEW SIZE OF THE Y VALUE
            Vector3 newSize = new Vector3(originalEnergyScale.x, newY, originalEnergyScale.z);

            // SLERP MOVEMENT AND ROTATION OF BLADE
            energy.transform.localScale = Vector3.Slerp(energy.transform.localScale, newSize, time);

            if (!isBladeExtended)
            {
                // LOOK AT THE GRAPPLE POINT
                weaponPivot.LookAt(null);
            }
            else
            {
                weaponPivot.LookAt(target);
            }

            //foreach (Transform t in bladePieces)
            //{
            //    newY = t.localScale.y / scaleFactor;

            //    Vector3 newPieceSize = new Vector3(originalBladeScale.x, newY, originalBladeScale.z);

            //    t.localScale = Vector3.Slerp(t.localScale, newPieceSize, time);
            //}
        }
    }

    private void HandleInAirVelocity(PlayerLocomotion playerLocomotion, float speedReductionTimer, float reductionRate)
    {
        // ERROR CHECK
        //if (playerLocomotion.isGrounded) return;

        // START THE COURUTINE
        StartCoroutine(SlowInAirVelocity(playerLocomotion, speedReductionTimer, reductionRate));

        // RETURN BACK TO ORIGINAL VELOCITY VALUES
        if (playerLocomotion.isGrounded)
        {
            // ORIGINAL FALLING VELOCITY
            playerLocomotion.fallingVel = originalFallingVelocity;
        }
    }

    public IEnumerator SlowInAirVelocity(PlayerLocomotion playerLocomotion, float speedReductionTimer, float reductionRate)
    {
       
        // CACHE VALUES OF ORIGINAL FALLING VLOCITY
        float originalVelocity = playerLocomotion.fallingVel;
        float inAirTimer = playerLocomotion.inAirTimer;

        if (inAirTimer > 0)
        {
            // SLOWLY REDUCE THE FALLING VELOCITY BASED ON THE TIMER
            //playerLocomotion.fallingVel -= reductionRate * Time.deltaTime;
            playerLocomotion.fallingVel = 1.5f;

            yield return new WaitForSeconds(speedReductionTimer);

            // RESET THE FALLING VELOCITY TO ITS ORIGINAL VALUE
            playerLocomotion.fallingVel = originalVelocity;
            Debug.Log("Falling Velocity To Original >> " + playerLocomotion.fallingVel);
        }
        else
        {
            // RETURN TO ORIGINAL FALLING VELOCITY
            if (playerLocomotion.isGrounded)
            {
                playerLocomotion.fallingVel = originalVelocity;
            }
        }
    }

    public void RetractBlade()
    {
        isBladeExtended = false;

        if (!isBladeExtended)
        {

            // RETURN INTO ORIGINAL POSITION
            pivotTransform = originalPivotTransform;
            pivotTransform.rotation = originalPivotRotation;

            if (energy.transform.localScale.y <= originalEnergyScale.y)
            {

                foreach (Transform t in bladePieces)
                {
                    t.localScale = originalBladeScale;
                }

                energy.transform.localScale = originalEnergyScale;
                return;
            }

            float scaleFactor = originalEnergyScale.y / energy.transform.localScale.y;

            // CALCULATE EXTENTION TIME BASED ON DISTANCE
            float time = scaleFactor * bladeExtentionSpeed * Time.deltaTime;

            // SLERP BACK INTO ORIGINAL POSITION
            energy.transform.localScale = Vector3.Slerp(originalEnergyScale, energy.transform.localScale, time);

            // RETURN BLADE INTO ITS ORIGINAL ROTATION VALUES
            weaponPivot.transform.rotation = originalPivotRotation;

            foreach (Transform t in bladePieces)
            {
                float newY = t.localScale.y / scaleFactor;

                Vector3 newPieceSize = new Vector3(originalBladeScale.x, newY, originalBladeScale.z);

                t.localScale = Vector3.Slerp(t.localScale, newPieceSize, time);
            }

            HandleInAirVelocity(playerLocomotion, falingVelocityReductionTimer, reductionRate);
        }

    }

    public void FindAllBladePiecesOnStart()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("BladePiece");

        foreach (GameObject t in pieces)
        {
            // ADD ALL TRANSFORMS INTO THE LISTS
            bladePieces.Add(t.GetComponent<Transform>());
        }
        Debug.Log("Piece Added!");
    }

    #endregion

    #region SWINGING ACTIONS
    private void InAirMovement()
    {
        // IF THE PLAYER IS NOT IN THE AIR, THEN DONT PERFORM INAIRMOVEMENT
        if (playerLocomotion.isGrounded) return;

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
            Vector3 directionToPoint = swingPointHit - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPointHit);

            // Recalculate Joint Distance
            springJoint.maxDistance = distanceFromPoint * 0.8f;
            springJoint.minDistance = distanceFromPoint * 0.25f;
        }

        // HANDLES INPUT TO EXTEND THE CABLE WHEN SWINGING
        if (inputManager.rope_Adjust_Pressed)
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPointHit) + extendCableSpeed;

            // Recalculate Join Distance
            springJoint.maxDistance = extendedDistanceFromPoint * 0.8f;
            springJoint.minDistance = extendedDistanceFromPoint * 0.25f;
        }

    }

    public void HighlightSwingingPoint(float maxRange)
    {
        // HIGHLIGHTS ALL GRAPPLE POINTS
        // RAYCAST VARIABLES

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // CAST THE RAY TO CHECK IF IT COLLIDES/HITS A GRAPPLE POINT LAYER GIVEN A SPECIFIC MAX RANGE
        if (Physics.Raycast(ray, out hit, maxRange) && hit.transform.gameObject.layer == validSwingPoint)
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

    public void FindSwingingPointsOnStart()
    {
        // FIND ALL GRAPPLE POINTS WITH THE PROPER TAG TO STORE THEM IN AS VALID GRAPPLE POINTS
        GameObject[] swingingPointObjects = GameObject.FindGameObjectsWithTag("SwingingPoint");

        foreach (GameObject sp in swingingPointObjects)
        {
            grapplePoints.Add(sp);
        }
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

    private void StartSwing()
    {
        // IF PREDICTION POINT DOESN'T FIND ANYTHING EXIT
        if (predictionHit.point == Vector3.zero) return;


        // IF THERE IS NO JOINT CURRENTLY
        if (springJoint == null)
        {
            // THEN CREATE A JOINT AND ALLOW THE PLAYER TO SWING
            isSwinging = true;
            swingPointHit = predictionHit.point;
            springJoint = playerModel.gameObject.AddComponent<SpringJoint>();
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = swingPointHit;
        }

        // THE DISTANCE HOW FAR THE PLAYER IS FROM THE GRAPPLE POINT WHEN GRAPPLING
        // THE LOWER THE MAX VALUE THE CLOSER THE PLAYER IS ABLE TO SWING TOWARDS THE GRAPPLE POINT
        float distanceFromPoint = Vector3.Distance(swordTip.transform.position, swingPointHit);
        springJoint.maxDistance = distanceFromPoint * 0.8f;
        springJoint.minDistance = distanceFromPoint * 0.25f;

        // CUSTOMIZE JOINT PROPERTIES
        springJoint.spring = 4.5f;
        springJoint.damper = 7f;
        springJoint.massScale = 4.5f;

        currentSwingingPosition = swordTip.transform.position;
    }

    public void StopSwing()
    {
        isSwinging = false;
        Destroy(springJoint);
        DestroyAllJoints();
    }

    public void HandleSwingAction()
    {
        if (inputManager.swing_Pressed)
        {
            StartSwing();
        }
        else
        {
            StopSwing();
        }

        // ONLY HANDLE IN AIR MOVEMENT IF THERE IS A SPRING JOINT TO WORK WITH
        if (springJoint != null) InAirMovement();
    }

    public void CheckForSwingPoints()
    {
        if (springJoint != null) return;

        // SETUP RAYCAST VARIABLES
        RaycastHit sphereCastHit;
        RaycastHit raycastHit;

        // HANDLE SPHERE CAST AND RAY CAST 
        Physics.SphereCast(swordTip.transform.position, predictionSphereCastRadius, mainCamera.forward, out sphereCastHit, maxSwingDistance, validSwingPoint);
        Physics.Raycast(swordTip.transform.position, mainCamera.forward, out raycastHit, maxSwingDistance, validSwingPoint);

        Vector3 hitPoint;

        Vector3 lookAtPoint = raycastHit.point;


        if (raycastHit.point != Vector3.zero)               // DIRECT HIT
        {
            hitPoint = raycastHit.point;
            swordTip.transform.LookAt(lookAtPoint);
        }
        else if (sphereCastHit.point != Vector3.zero)      // INDIRECT HIT, PREDITION POINT
        {
            hitPoint = sphereCastHit.point;
            HighlightSwingingPoint(maxIndicationDistance);
            swordTip.transform.LookAt(lookAtPoint);
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
            HighlightSwingingPoint(maxIndicationDistance);
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

    #endregion


}
