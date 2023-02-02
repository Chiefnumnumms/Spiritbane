using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDisable : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
