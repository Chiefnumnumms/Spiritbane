using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Agreskoul : MonoBehaviour 
{
    [Header("Agreskoul")]
    public Transform energy;
    public List<Transform> bladePieces = new List<Transform>();
    [SerializeField] private float scaleFactor;
    [SerializeField] private float speedDecrement;
    private GameObject swordTip;
    public float bladeExtentionSpeed;
    private float maxExtentionDistance = 50.0f;

    private Swinging swingingManager;
    private InputManager inputManager;

    private Vector3 originalBladeScale;
    private Vector3 originalEnergyScale;

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

    }

    private void Update()
    {
        //if (inputManager.swing_Pressed)
        //{
        //    ExecuteSwordSwing();
        //}
        //else
        //{
        //    RetractBlade();
        //}
    }

    public void ExecuteSwordSwing()
    {
        // CALCULATE DISTANCE AND TIME
        Vector3 target = swingingManager.swingPoint;
        Vector3 distance = target - swordTip.transform.position;

        // IF PAST MAX DISTANCE
        if (distance.magnitude >= maxExtentionDistance)
        {
            target = maxExtentionDistance * distance.normalized;
        }

        // SET SCALE IN Y DIRECTION OF THE ENERGY TRANSFORM
        scaleFactor = distance.magnitude / energy.transform.localScale.y;

        // CALCULATE THE NEW Y VALUE BASED ON THE SCALE FACTOR
        float newY = energy.transform.localScale.y * scaleFactor;

        // CALCULATE EXTENTION TIME BASED ON DISTANCE
        float time = distance.magnitude * bladeExtentionSpeed / speedDecrement;

        // CALCULATE THE NEW SIZE OF THE Y VALUE
        Vector3 newSize = new Vector3(originalEnergyScale.x, newY, originalEnergyScale.z);

        // SLERP MOVEMENT OF BLADE
        energy.transform.localScale = Vector3.Slerp(energy.transform.localScale, newSize, time);

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
        float time = scaleFactor * bladeExtentionSpeed;
        
        // SLERP BACK INTO ORIGINAL POSITION
        energy.transform.localScale = Vector3.Slerp(originalEnergyScale, energy.transform.localScale, time);

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

    private void SwordExtention(float inScale, float inDuration)
    {
        //if (!springJoint) return;

        // CACHING THE SIZE OF THE ENERGY OBJECT ACCORDING TO DISTANCE BETWEEN TIP AND GRAPPLE POINT
        Vector3 newSize = energy.transform.localScale * inScale;

        // SETTING THE NEW SIZE TO THE NEW SIZE
        energy.transform.localScale = Vector3.Slerp(energy.transform.localScale, newSize, inDuration);

        foreach (Transform t in bladePieces)
        {
            newSize = t.localScale / inScale;

            t.localScale = Vector3.Slerp(t.localScale, newSize, inDuration);
        }

    }
}
