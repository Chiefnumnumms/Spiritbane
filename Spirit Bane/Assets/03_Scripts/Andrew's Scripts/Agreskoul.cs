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

    private Swinging swingingManager;
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

        // CACHE ALL VALUES
        CacheAllValues();
    }

    private void Start()
    {
        swordTip = GameObject.FindGameObjectWithTag("BladeTip");
        energy = GameObject.FindGameObjectWithTag("Energy").GetComponent<Transform>();

        swingingManager = GetComponent<Swinging>();
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
            Vector3 target = swingingManager.swingPoint;
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
}
