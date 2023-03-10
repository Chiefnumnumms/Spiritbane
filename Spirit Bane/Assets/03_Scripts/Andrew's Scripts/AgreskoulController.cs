using System.Collections;
using TMPro;
using UnityEngine;

public class AgreskoulController : MonoBehaviour
{
    public enum WeaponMode { DEFAULT, SWING, GRAPPLE}
    public WeaponMode currentWeaponMode = WeaponMode.DEFAULT;

    private PlayerLocomotion playerLocomotion;
    private InputManager inputManager;

    private GameObject target;
    private GameObject maxRangeObject;

    private Transform weaponTransform;
    private Transform swordTipTransform;
    private Transform energyTransform;

    public LayerMask validSwingingPoint;
    public LayerMask validGrapplePoint;
    public LayerMask validPullPoint;

    public BoxCollider tipCollider;

    public float smoothFactor = 5.0f;

    private void Awake()
    {
        inputManager = GameObject.Find("Gaoh").GetComponent<InputManager>();
        playerLocomotion = GameObject.Find("Gaoh").GetComponent<PlayerLocomotion>();

        tipCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        swordTipTransform = this.transform;
        energyTransform = this.transform.parent;
        weaponTransform = energyTransform.parent.parent;

        maxRangeObject = GameObject.Find("MaxRange");
    }

    private void Update()
    {
        HandleAction();
    }

    public void ChangeSize(GameObject targetObject)
    {
        if (!playerLocomotion.isGrounded) return;

        // Calculate the center of the target
        Vector3 targetCenter = targetObject.GetComponent<Collider>().bounds.center;

        Vector3 newSize = new Vector3(energyTransform.transform.localScale.x, targetCenter.y, energyTransform.transform.localScale.z);
        energyTransform.transform.localScale = newSize;

        // Calculate the direction towards the target center
        Vector3 direction = targetCenter - swordTipTransform.position;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Smoothly rotate the sword tip towards the target rotation
        weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, targetRotation, Time.deltaTime * smoothFactor);

        Debug.DrawRay(this.transform.position, targetCenter - transform.position, Color.green);
    }

    private IEnumerator BeginAction()
    {
        if (!target) target = maxRangeObject;

        tipCollider.enabled = true;

        // While We Have Not Reached The Target
        while (this.transform.position != target.transform.position)
        {
            ChangeSize(target);
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Reached Maximum Size");
    }

    private void HandleAction()
    {
        if (inputManager.agreskoul_Pressed)
        {
            if (!playerLocomotion.isGrounded) return;

            StartCoroutine(BeginAction());
        }
        else
        {
            tipCollider.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Find Tag Of The Objects That The Sword Collided With
        switch (collision.gameObject.layer)
        {
            default:        // Retract Sword
                //ChangeSize(swordTipTransform);
                break;
            case 03:        // Swing Point
                //GameManager.instance.PlayerGO.GetComponent<PlayerLocomotion>().HandleSwing();
                break;
            case 06:        // Grapple Point
                //GameManager.instance.PlayerGO.GetComponent<PlayerLocomotion>().HandleGrapple();
                break;
            case 09:        // Pull Point
                //GameManager.instance.PlayerGO.GetComponent<PlayerLocomotion>().HandleObjectPull();
                break;
            case 11:        // Enemy Layer
                break;

        }
    }

    #region Old System

    //private void GravitateTowardsObject()
    //{
    //    RaycastHit hit;
    //    float radius = 6.0f;

    //    // Shoot Sphere Cast
    //    if (Physics.SphereCast(GameManager.instance.Cam.transform.position, radius, GameManager.instance.Cam.transform.forward, out hit, 0))
    //    {
    //        if (hit.transform.gameObject.layer == validSwingingPoint
    //           || hit.transform.gameObject.layer == validGrapplePoint
    //           || hit.transform.gameObject.layer == validPullPoint)
    //        {
    //            target = hit.transform.gameObject;
    //        }
    //    }
    //    else
    //    {
    //        target = null;
    //    }
    //}

    //private void ChangeSize(Vector3 target)
    //{
    //    #region Old
    //    //if (this.transform.position != target) return;

    //    //float distance = Vector3.Distance(this.transform.position, target);

    //    //float time = distance / speed;

    //    //energyTransform.transform.localScale += new Vector3(energyTransform.transform.localScale.x, distance * time * Time.deltaTime, energyTransform.transform.localScale.z);
    //    #endregion

    //    Vector3 newSize = new Vector3(energyTransform.transform.localScale.x, target.y, energyTransform.transform.localScale.z);

    //    energyTransform.transform.localScale = newSize;

    //    weaponTransform.LookAt(target);

    //    Debug.DrawRay(this.transform.position, target - transform.position, Color.green);
    //}


    #endregion
}
