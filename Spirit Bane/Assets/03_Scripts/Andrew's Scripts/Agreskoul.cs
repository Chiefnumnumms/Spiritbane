using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    private Swinging swingingManager;
    private InputManager inputManager;
    private GameObject swordTip;

    private Vector3 originalBladeScale;
    private Vector3 originalEnergyScale;
    private Quaternion originalPivotRotation;

    private bool isBladeExtended = false;
    private float bladeRetractionDelay = 0.5f;
    private float bladeRetractionTimer = 0.0f;

    private void Awake()
    {
        // FINDS ALL BLADE PIECES ON START
        FindAllBladePiecesOnStart();

        // CACHE ORIGINAL SCALE OF THE BLADE PIECES
        originalBladeScale = bladePieces[0].localScale;

        // CACHE ORIGINAL SCALE OF THE ENERGY
        originalEnergyScale = energy.localScale;
    }

    private void Start()
    {
        swordTip = GameObject.FindGameObjectWithTag("BladeTip");
        energy = GameObject.FindGameObjectWithTag("Energy").GetComponent<Transform>();

        swingingManager = GetComponent<Swinging>();
        inputManager = GetComponent<InputManager>();

        originalPivotRotation = weaponPivot.transform.rotation;
    }

    public void ExecuteSwordSwing()
    {
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
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, time * Time.deltaTime);

        weaponPivot.LookAt(target);

        foreach (Transform t in bladePieces)
        {
            newY = t.localScale.y / scaleFactor;

            Vector3 newPieceSize = new Vector3(originalBladeScale.x, newY, originalBladeScale.z);

            t.localScale = Vector3.Slerp(t.localScale, newPieceSize, time);
        }
    }


    public void RetractBlade()
    {
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
