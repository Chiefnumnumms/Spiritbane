using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agreskoul : MonoBehaviour
{
    private enum Mechanic { SWINGING, OBJECTGRAPPLE, ENEMYHOOK}

    [SerializeField] private PlayerLocomotion playerLocomotion;
    private AnimationManager animationManager;
    private InputManager inputManager;
    private AgreskoulController agreskoulController;

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

    [SerializeField] private Transform debugHitPointTransform;

    #region Swinging Attributes

    [Header("Swinging Attributes")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform playerModel;
    [SerializeField] private LayerMask isValidSwingPoint;
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
    [SerializeField] private RaycastHit swingPredictionHit;
    [SerializeField] private float swigingPredictionSphereCastRadius;
    [SerializeField] private Transform predictionPointSwing;

    [Header("Material Set")]
    [SerializeField] private Material desiredMaterial;
    [SerializeField] private Material originalMaterial;

    [SerializeField] private bool isSwinging;
    [SerializeField] private bool isLookingAtSwingingPoint;

    private Vector3 currentSwingingPosition;
    [SerializeField] public float maxIndicationDistance = 25.0f;

    private List<GameObject> swingingPoints = new List<GameObject>();
    private List<SpringJoint> springJointsList = new List<SpringJoint>();
    private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

    public Transform swordLookAt;

    #endregion

    #region Private Attributes

    private float originalFallingVelocity;
    private Vector3 originalBladeScale;
    private Vector3 originalEnergyScale;
    private Quaternion originalPivotRotation;
    private Transform originalPivotTransform;
    public Transform pivotTransform;
    private GameObject swordTip;
    private const float MAX_FALLING_VELOCITY = 1.5f;
    private float highestPointOnYArc;

    private bool isBladeExtended;
    public bool keepBladeExtended;
    public bool reachedTarget;

    #endregion

    #region Pulling Attributes

    [Header("Pull Mechanic Settings")]
    [SerializeField] Vector3 pullPointHit;
    [SerializeField] float maxPullDistance;
    [SerializeField] LayerMask objectPullLayer;
    [SerializeField] bool isLookingAtPullPoint;
    [SerializeField] GameObject pullBoxCollider;

    private bool isPullingObject;
    #endregion

    private GameObject goTarget;

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

        predictionPointSwing = GameObject.Find("PredictionPoint").GetComponent<Transform>();

    }

    private void Update()
    {
        // RETURN TO ORIGINAL FALLING VELOCITY
        if (playerLocomotion.isGrounded)
        {
            playerLocomotion.fallingVel = originalFallingVelocity;
        }

        //if (swingPointHit == Vector3.zero)
        //{
        //    weaponPivot.LookAt(swordLookAt);
        //}
        //else if (swingPointHit != null && isSwinging)
        //{
        //    weaponPivot.LookAt(swingPointHit);
        //}

        if (inputManager.agreskoul_Pressed)
        {
            HandlePullPointLookAt();
        }

        //if (isLookingAtPullPoint)
        //{
        //    weaponPivot.LookAt(debugHitPointTransform);
        //}

        CheckForSwingPoints();
        HandleSwingAction();
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
        keepBladeExtended = false;
        reachedTarget = false;

    }

    #region AGRESKOUL MAIN

    public void ChooseMechanicTarget(Vector3 target)
    {
        Vector3 distance = swordTip.transform.position - target;
        float magnitude = distance.magnitude;

        if (magnitude >= maxExtentionDistance)
        {
            target = swordTip.transform.position - distance.normalized * maxExtentionDistance;
            magnitude = maxExtentionDistance;
        }

        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        BoxCollider swordTipCollider = GetComponentInChildren<BoxCollider>();

        // Calculate the scaling factor based on the distance between the sword tip and the collider center
        Vector3 centerOffset = swordTipCollider.bounds.center - swordTip.transform.position;
        float centerDistance = centerOffset.magnitude;
        float scaleFactor = magnitude / centerDistance;

        weaponPivot.rotation = targetRotation;
        weaponPivot.LookAt(target);

        Vector3 newSize = originalEnergyScale;
        newSize.y *= scaleFactor;

        // Calculate the time needed to extend the blade based on the target distance and extension speed
        float time = magnitude * bladeExtentionSpeed / speedDecrement;

        // Lerp the energy transform scale to the new size over the calculated time
        energy.transform.localScale = Vector3.Slerp(energy.transform.localScale, newSize, time);

        // Rotate the sword to face the target
        weaponPivot.rotation = targetRotation;
    }




    public void RetractBlade()
    {
        if (energy.transform.localScale.y <= originalEnergyScale.y)
        {
            ResetBladeToOriginalState();
            return;
        }

        float scaleFactor = originalEnergyScale.y / energy.transform.localScale.y;
        float time = scaleFactor * bladeExtentionSpeed * Time.deltaTime;

        energy.transform.localScale = Vector3.Slerp(originalEnergyScale, energy.transform.localScale, time / 3.0f);
        weaponPivot.rotation = originalPivotRotation;

        foreach (Transform t in bladePieces)
        {
            float newY = t.localScale.y / scaleFactor;
            Vector3 newPieceSize = new Vector3(originalBladeScale.x, newY, originalBladeScale.z);
            t.localScale = Vector3.Slerp(t.localScale, newPieceSize, time);
        }

        HandleInAirVelocity(playerLocomotion, falingVelocityReductionTimer, reductionRate);
    }

    public void ExecuteSwordAction()
    {
        if (isSwinging && !isPullingObject)
        {
            ChooseMechanicTarget(swingPointHit);
        }

        if (isLookingAtPullPoint)
        {
            ChooseMechanicTarget(debugHitPointTransform.position);
        }
    }

    private void ResetBladeToOriginalState()
    {
        pivotTransform = originalPivotTransform;
        pivotTransform.rotation = originalPivotRotation;

        foreach (Transform t in bladePieces)
        {
            t.localScale = originalBladeScale;
        }

        energy.transform.localScale = originalEnergyScale;
    }

    private void HandleInAirVelocity(PlayerLocomotion playerLocomotion, float speedReductionTimer, float reductionRate)
    {
        // START THE COURUTINE
        StartCoroutine(SlowInAirVelocity(playerLocomotion, speedReductionTimer, reductionRate));
    }

    public IEnumerator SlowInAirVelocity(PlayerLocomotion playerLocomotion, float speedReductionTimer, float reductionRate)
    {
        // CACHE VALUES OF ORIGINAL FALLING VELOCITY
        float originalVelocity = playerLocomotion.fallingVel;
        float inAirTimer = playerLocomotion.inAirTimer;

        if (inAirTimer > 0)
        {
            // SET THE FALLING VELOCITY TO 1.5
            playerLocomotion.fallingVel = 1.5f;

            // WAIT FOR A SECOND BEFORE STARTING TO INCREASE THE FALLING VELOCITY
            yield return new WaitForSeconds(2.5f);

            // VERY SLOWLY INCREASE THE FALLING VELOCITY BACK TO ITS ORIGINAL VALUE
            while (playerLocomotion.fallingVel < originalVelocity)
            {
                // INCREASE THE FALLING VELOCITY BY THE REDUCTION RATE PER SECOND
                playerLocomotion.fallingVel += reductionRate * Time.deltaTime;

                // MAKE SURE THE FALLING VELOCITY DOESN'T EXCEED THE ORIGINAL VALUE
                if (playerLocomotion.fallingVel > originalVelocity)
                {
                    playerLocomotion.fallingVel = originalVelocity;
                }

                yield return null;
            }

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

    public void FindAllBladePiecesOnStart()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("BladePiece");

        foreach (GameObject t in pieces)
        {
            // ADD ALL TRANSFORMS INTO THE LISTS
            bladePieces.Add(t.GetComponent<Transform>());
        }
        //Debug.Log("Piece Added!");
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

    #endregion

    #region SWINGING ACTIONS

    public void InAirMovement()
    {
        // IF THE PLAYER IS NOT IN THE AIR, THEN DONT PERFORM INAIRMOVEMENT
        if (playerLocomotion.isGrounded) return;

        if (inputManager.d_Pressed) // SWING RIGHT
        {
            // APPLY HORIZONTAL FORCE TO THE RIGIDBODY
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime, ForceMode.VelocityChange);
        }
        else if (inputManager.a_Pressed) // SWING LEFT
        {
            // APPLY HORIZONTAL FORCE TO THE RIGIDBODY
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime, ForceMode.VelocityChange);
        }
        else if (inputManager.w_Pressed) // SWING FORWARD
        {
            // APPLY FORWARD FORCE TO THE RIGIDBODY
            rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime, ForceMode.VelocityChange);
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
        if (Physics.Raycast(ray, out hit, maxRange) && hit.transform.gameObject.layer == isValidSwingPoint)
        {
            isLookingAtSwingingPoint = true;

            // PLAYER IS LOOKING AT A VALID GRAPPLE POINT
            if (isLookingAtSwingingPoint)
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
            swingingPoints.Add(sp);
        }
    }

    void DestroyAllJoints()
    {
        if (!isSwinging)
        {
            // DESTROY ALL SPRINT JOINTS WHEN THE PLAYER IS NO LONGER GRAPPLING
            foreach (SpringJoint springJoint in springJointsList)
            {
                Destroy(springJoint);
            }
            springJointsList.Clear();
        }

    }

    public void StartSwing()
    {
        // IF PREDICTION POINT DOESN'T FIND ANYTHING EXIT
        if (swingPredictionHit.point == Vector3.zero) return;

        // IF THERE IS NO JOINT CURRENTLY
        if (springJoint == null)
        {
            // THEN CREATE A JOINT AND ALLOW THE PLAYER TO SWING
            isSwinging = true;
            swingPointHit = swingPredictionHit.point;
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

    public void CheckForSwingPoints()
    {
        if (springJoint != null) return;

        // SETUP RAYCAST VARIABLES
        RaycastHit sphereCastHit;
        RaycastHit raycastHit;

        // HANDLE SPHERE CAST AND RAY CAST 
        Physics.SphereCast(mainCamera.position, swigingPredictionSphereCastRadius, mainCamera.forward, out sphereCastHit, maxSwingDistance, isValidSwingPoint);
        Physics.Raycast(mainCamera.position, mainCamera.forward, out raycastHit, maxSwingDistance, isValidSwingPoint);

        Vector3 hitPoint;

        if (raycastHit.point != Vector3.zero)               // DIRECT HIT
        {
            hitPoint = raycastHit.point;
            goTarget = raycastHit.transform.gameObject;
            isLookingAtSwingingPoint = true;
        }
        else if (sphereCastHit.point != Vector3.zero)      // INDIRECT HIT, PREDITION POINT
        {
            hitPoint = sphereCastHit.point;
            goTarget = sphereCastHit.transform.gameObject;
        }
        else                                              // NOTHING IN THE WAY 
        {
            hitPoint = Vector3.zero;
            goTarget = null;
            weaponPivot.LookAt(swordLookAt);
            isLookingAtSwingingPoint = false;
        }

        if (hitPoint != Vector3.zero)                   // HITPOINT DETECTED A VALID POINT TO GRAPPLE TO
        {
            // GRAPPLE POINT DETECTED, SET THE PREDICTION POINT TO ACTIVE
            predictionPointSwing.gameObject.SetActive(true);

            // SET THE PREDICTION POINT TO BE THE SAME POSITION OF WHERE THE PLAYER IS AIMING TOWARDS
            predictionPointSwing.position = hitPoint;
        }
        else                                                // NOTHING FOUND ON HIT POINT
        {
            // DISABLE THE PREDICTION POINT AS NOTHING WAS SCANNED
            predictionPointSwing.gameObject.SetActive(false);

            foreach (MeshRenderer meshes in meshRenderers)
            {
                meshes.material = originalMaterial;
            }
        }

        swingPredictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }

    #endregion

    #region PULL ACTIONS

    public void HandlePullPointLookAt()
    {
        RaycastHit raycastHit;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, maxPullDistance, objectPullLayer))
        {
            isLookingAtPullPoint = true;

            pullPointHit = raycastHit.point;

            debugHitPointTransform.position = raycastHit.point;
        }
        else
        {
            isLookingAtPullPoint = false;
        }
    }

    #endregion

}
