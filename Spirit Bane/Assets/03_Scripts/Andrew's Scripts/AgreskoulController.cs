using System.Collections;
using UnityEngine;

public class AgreskoulController : MonoBehaviour
{
    public enum WeaponMode { DEFAULT, SWING, GRAPPLE}

    public WeaponMode currentWeaponMode = WeaponMode.DEFAULT;

    private GameObject target;
    private GameObject maxRangeObject;

    private Transform weaponTransform;
    private Transform swordTipTransform;
    private Transform energyTransform;

    public LayerMask validSwingingPoint;
    public LayerMask validGrapplePoint;
    public LayerMask validPullPoint;

    private InputManager inputManager;

    private void Awake()
    {
        inputManager = GameObject.Find("Gaoh").GetComponent<InputManager>();
    }

    private void Start()
    {
        swordTipTransform = this.transform;
        energyTransform = this.transform.parent;
        weaponTransform = energyTransform.parent.parent;

        maxRangeObject = GameObject.Find("MaxRange");
    }

    private void ChangeSize(Vector3 target)
    {
        #region Old
        //if (this.transform.position != target) return;

        //float distance = Vector3.Distance(this.transform.position, target);

        //float time = distance / speed;

        //energyTransform.transform.localScale += new Vector3(energyTransform.transform.localScale.x, distance * time * Time.deltaTime, energyTransform.transform.localScale.z);
        #endregion

        Vector3 newSize = new Vector3(energyTransform.transform.localScale.x, target.y, energyTransform.transform.localScale.z);

        energyTransform.transform.localScale = newSize;
    }

    private IEnumerator BeginAction()
    {
        if (!target) target = maxRangeObject;

        // While We Have Not Reached The Target
        while (this.transform.position != target.transform.position)
        {
            ChangeSize(target.transform.position);
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Reached Maximum Size");
    }

    private void HandleAction()
    {
        if (inputManager.agreskoul_Pressed)
        {
            StartCoroutine(BeginAction());
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

}
